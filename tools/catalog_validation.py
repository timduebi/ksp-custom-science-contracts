#!/usr/bin/env python3
"""Small dependency-free parser and semantic validator for generated KSP catalogs."""
from dataclasses import dataclass, field
from pathlib import Path
import re


KNOWN_CHECKS = {
    "CREW_MIN", "CREW_EXACT", "CREW_NONE", "CREW_CAPACITY_MIN", "SUBORBITAL",
    "LANDED", "ORBIT_ABOVE", "APOAPSIS_MAX", "INCLINATION_MIN", "ATMO_FRACTION",
    "FLYBY", "MARKER_LANDING", "VESSEL_COUNT", "VESSEL_COUNT_INCLINATION",
    "RELAY_VESSEL_COUNT", "RELAY_VESSEL_COUNT_INCLINATION", "EVA", "FUEL_MIN",
    "RESOURCE_MIN", "ORE_SURFACE", "WHEEL_MOTION", "DOCK_ANY", "DOCK_STATION",
    "HOLD", "DURATION", "RETURN_FROM_BODY",
}
BODY_CHECKS = {
    "SUBORBITAL", "LANDED", "ORBIT_ABOVE", "APOAPSIS_MAX", "INCLINATION_MIN",
    "ATMO_FRACTION", "FLYBY", "MARKER_LANDING", "VESSEL_COUNT",
    "VESSEL_COUNT_INCLINATION", "RELAY_VESSEL_COUNT", "RELAY_VESSEL_COUNT_INCLINATION",
    "EVA", "ORE_SURFACE", "WHEEL_MOTION", "RETURN_FROM_BODY",
}


@dataclass
class Check:
    kind: str
    values: dict


@dataclass
class Mission:
    id: str
    values: dict
    prerequisites: list = field(default_factory=list)
    checks: list = field(default_factory=list)
    source: str = ""


def blocks(text: str, keyword: str):
    pattern = re.compile(rf"(?m)^\s*{re.escape(keyword)}\s*$")
    for match in pattern.finditer(text):
        opening = text.find("{", match.end())
        if opening < 0:
            continue
        depth = 0
        for pos in range(opening, len(text)):
            if text[pos] == "{":
                depth += 1
            elif text[pos] == "}":
                depth -= 1
                if depth == 0:
                    yield text[opening + 1:pos]
                    break


def top_values(block: str):
    result = {}
    repeated = {}
    depth = 0
    for raw in block.splitlines():
        line = raw.split("//", 1)[0].strip()
        if not line:
            continue
        if depth == 0 and "=" in line and "{" not in line:
            key, value = [part.strip() for part in line.split("=", 1)]
            repeated.setdefault(key, []).append(value)
            result[key] = value
        depth += line.count("{") - line.count("}")
    return result, repeated


def load_catalog(directory):
    missions = []
    for path in sorted(Path(directory).glob("*.cfg")):
        text = path.read_text(encoding="utf-8")
        for contract in blocks(text, "CONTRACT"):
            values, repeated = top_values(contract)
            checks = []
            for raw_check in blocks(contract, "CHECK"):
                check_values, _ = top_values(raw_check)
                checks.append(Check(check_values.get("kind", ""), check_values))
            missions.append(Mission(values.get("id", ""), values,
                                    repeated.get("prerequisite", []), checks, str(path)))
    return missions


def validate(missions):
    errors = []
    by_id = {}
    for mission in missions:
        where = f"{mission.source}:{mission.id or '<missing id>'}"
        if not mission.id:
            errors.append(f"{where}: missing id")
            continue
        if mission.id in by_id:
            errors.append(f"{where}: duplicate id (also in {by_id[mission.id].source})")
        by_id[mission.id] = mission
        for required in ("title", "description", "sparte", "reward", "epoch"):
            if not mission.values.get(required):
                errors.append(f"{where}: missing {required}")
        try:
            if float(mission.values.get("reward", "0")) <= 0:
                errors.append(f"{where}: reward must be positive")
        except ValueError:
            errors.append(f"{where}: invalid reward")
        try:
            if not 1 <= int(mission.values.get("epoch", "0")) <= 99:
                errors.append(f"{where}: epoch outside supported range")
        except ValueError:
            errors.append(f"{where}: invalid epoch")
        if not mission.checks:
            errors.append(f"{where}: shipped mission has no COMPOSITE checks")
        for check in mission.checks:
            if check.kind not in KNOWN_CHECKS:
                errors.append(f"{where}: unknown check {check.kind!r}")
            if check.kind in BODY_CHECKS and not check.values.get("body"):
                errors.append(f"{where}: {check.kind} requires body")
            if check.kind == "RETURN_FROM_BODY" and not check.values.get("returnBody"):
                errors.append(f"{where}: RETURN_FROM_BODY requires returnBody")
            if check.kind in ("DURATION", "HOLD"):
                key = "days" if check.kind == "DURATION" else "seconds"
                try:
                    if float(check.values.get(key, "0")) <= 0:
                        errors.append(f"{where}: {check.kind} requires positive {key}")
                except ValueError:
                    errors.append(f"{where}: invalid {check.kind} value")
        if re.match(r"^cr_.*_station_expand\d+$", mission.id) or re.match(r"^st_.*_station_upgrade\d+$", mission.id):
            if any(check.kind == "CREW_NONE" for check in mission.checks):
                errors.append(f"{where}: station expansion must not require evacuation")

    for mission in missions:
        for prereq in mission.prerequisites:
            if prereq not in by_id:
                errors.append(f"{mission.source}:{mission.id}: unknown prerequisite {prereq}")
        ref = mission.values.get("stationRef")
        if ref and not any(m.values.get("recordStationKey") == ref for m in missions):
            errors.append(f"{mission.source}:{mission.id}: stationRef {ref} has no recorder")

    state = {}
    def visit(mid, chain):
        if state.get(mid) == 1:
            errors.append("prerequisite cycle: " + " -> ".join(chain + [mid]))
            return
        if state.get(mid) == 2 or mid not in by_id:
            return
        state[mid] = 1
        for prereq in by_id[mid].prerequisites:
            visit(prereq, chain + [mid])
        state[mid] = 2
    for mid in by_id:
        visit(mid, [])

    bodies = {check.values.get("body") for mission in missions for check in mission.checks}
    if {"Earth", "Kerbin"}.issubset(bodies):
        errors.append("catalog mixes SOL and Stock body profiles")
    return errors

