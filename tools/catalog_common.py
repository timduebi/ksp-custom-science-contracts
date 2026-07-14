#!/usr/bin/env python3
"""Shared generation policies used by the SOL and Stock catalogs.

Keep compatibility-sensitive rules here so localized generators cannot silently drift.
Mission ids, prerequisites and rewards are never rewritten by these helpers.
"""
import re


def _fraction_percent(value):
    number = float(value) / 100.0
    return f"{number:.3f}".rstrip("0").rstrip(".")


def parse_check(source: str):
    """Parse the mission-design CHECK shorthand for every maintained catalog."""
    head, _, label = source.partition("|")
    tokens = head.split()
    if not tokens:
        raise SystemExit("Empty check line")
    kind, args = tokens[0], tokens[1:]
    values = []
    if kind in ("CREW_MIN", "CREW_EXACT", "CREW_CAPACITY_MIN"):
        values = [("min", args[0])]
    elif kind == "CREW_NONE":
        values = []
    elif kind in ("SUBORBITAL", "LANDED", "ORE_SURFACE"):
        values = [("body", args[0])]
    elif kind == "ORBIT_ABOVE":
        values = [("body", args[0])] + ([("km", args[1])] if len(args) > 1 else [])
    elif kind == "APOAPSIS_MAX":
        values = [("body", args[0]), ("km", args[1])]
    elif kind == "INCLINATION_MIN":
        values = [("body", args[0]), ("inclinationMin", args[1])]
    elif kind == "ATMO_FRACTION":
        values = [("body", args[0]), ("fracMin", _fraction_percent(args[1])),
                  ("fracMax", _fraction_percent(args[2]))]
    elif kind in ("FLYBY", "MARKER_LANDING"):
        values = [("body", args[0]), ("km", args[1])]
        if kind == "MARKER_LANDING" and len(args) > 3:
            values += [("latMin", args[2]), ("latMax", args[3])]
    elif kind in ("VESSEL_COUNT", "RELAY_VESSEL_COUNT"):
        values = [("body", args[0]), ("count", args[1])] + ([("km", args[2])] if len(args) > 2 else [])
    elif kind in ("VESSEL_COUNT_INCLINATION", "RELAY_VESSEL_COUNT_INCLINATION"):
        values = [("body", args[0]), ("count", args[1]), ("inclinationMin", args[2])]
        if len(args) > 3:
            values.append(("km", args[3]))
    elif kind == "EVA":
        values = [("body", args[0])] + ([("situation", args[1])] if len(args) > 1 else [])
    elif kind == "FUEL_MIN":
        values = [("amount", args[0])]
    elif kind == "RESOURCE_MIN":
        values = [("resource", args[0]), ("amount", args[1])]
    elif kind == "WHEEL_MOTION":
        values = [("body", args[0]), ("speed", args[1])]
    elif kind == "DOCK_ANY":
        values = []
    elif kind == "DOCK_STATION":
        values = [("stationKey", args[0])] + ([("body", args[1])] if len(args) > 1 else [])
    elif kind == "HOLD":
        values = [("seconds", args[0])]
    elif kind == "DURATION":
        values = [("days", args[0])]
    elif kind == "RETURN_FROM_BODY":
        values = [("body", args[0]), ("returnBody", args[1])]
        if len(args) > 2:
            values.append(("returnMode", args[2]))
    else:
        raise SystemExit(f"Unknown check '{kind}' in: {source}")
    return kind, values, label.strip()


def stability_days(is_initial_build: bool) -> int:
    """Initial infrastructure proves ten days; later upgrades need only three."""
    return 10 if is_initial_build else 3


def long_stay_days(is_initial_stage: bool) -> int:
    """One real endurance milestone per chain; later tiers verify sixty days."""
    return 150 if is_initial_stage else 60


_STOCK_INITIAL_LONG_STAYS = {
    "st_kerbin_station_longstay3",
    "st_mun_station_longstay3",
    "st_duna_station_longstay2",
}


def normalize_stock_station_policy(mission: dict) -> dict:
    """Apply the common station policy to a parsed Stock design mission in place."""
    mission_id = mission.get("id", "")
    checks = mission.get("checks", [])
    if re.match(r"^st_.*_station_upgrade\d+$", mission_id):
        checks = [check for check in checks if check[0] != "CREW_NONE"]
        checks = [_replace_duration(check, 3, "keep the occupied station stable for 3 days")
                  if check[0] == "DURATION" else check for check in checks]
        mission["checks"] = checks
    elif "_station_longstay" in mission_id and mission_id not in _STOCK_INITIAL_LONG_STAYS:
        mission["checks"] = [_replace_duration(check, 60, "operate continuously for 60 days")
                             if check[0] == "DURATION" else check for check in checks]
        for key in ("description", "beschreibung", "beschreibung_en"):
            if key in mission:
                mission[key] = mission[key].replace("150 days", "60 days")
    return mission


def _replace_duration(check, days: int, label: str):
    kind, values, _old_label = check
    return kind, [(key, days if key == "days" else value) for key, value in values], label
