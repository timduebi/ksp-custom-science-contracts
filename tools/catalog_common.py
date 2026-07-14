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
    # The human-readable label delimiter contains spaces. Bare pipes are allowed inside values,
    # for example compatible module alternatives in MODULE_COUNT.
    head, separator, label = source.partition(" | ")
    if not separator:
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
    elif kind == "RELAY_NETWORK_TOPOLOGY":
        values = [("body", args[0]), ("count", args[1]), ("redundancy", args[2]),
                  ("separationMin", args[3]), ("maxGap", args[4])]
        if len(args) > 5:
            values.append(("inclinationMin", args[5]))
        if len(args) > 6:
            values.append(("km", args[6]))
    elif kind == "EVA":
        values = [("body", args[0])] + ([("situation", args[1])] if len(args) > 1 else [])
    elif kind == "FUEL_MIN":
        values = [("amount", args[0])]
    elif kind == "RESOURCE_MIN":
        values = [("resource", args[0]), ("amount", args[1])]
    elif kind == "RESOURCE_DELIVERY":
        values = [("stationKey", args[0]), ("resource", args[1]), ("amount", args[2])]
        if len(args) > 3:
            values.append(("km", args[3]))
    elif kind == "MASS_MIN":
        values = [("amount", args[0])]
    elif kind == "MODULE_COUNT":
        values = [("module", args[0]), ("count", args[1])]
    elif kind == "POWER_CAPACITY_MIN":
        values = [("amount", args[0])]
    elif kind == "DOCKING_PORT_COUNT":
        values = [("count", args[0])]
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
    """One real endurance milestone per chain; later tiers verify 120 days."""
    return 150 if is_initial_stage else 120


SCIENCE_LAB_MODULES = "ModuleScienceLab|ModuleScienceConverter|Laboratory"


def station_engineering_requirements(seats: int, is_expansion: bool):
    """Scale mandatory station mass, docking capacity and power with habitation capacity."""
    seats = int(seats)
    mass = max(18, 8 + seats * 3) if is_expansion else max(12, 8 + seats * 2)
    ports = max(2, (seats + 1) // 2)
    power = max(1000, seats * 250)
    return mass, ports, power


def recommended_route_order(prerequisites: dict, milestones: set, sort_key):
    """Expand milestone anchors to their complete prerequisite closure and topologically order it."""
    missing = sorted(set(milestones) - set(prerequisites))
    if missing:
        raise SystemExit(f"Recommended-route milestone(s) missing: {', '.join(missing)}")

    route = set()
    stack = list(milestones)
    while stack:
        mission_id = stack.pop()
        if mission_id in route:
            continue
        route.add(mission_id)
        stack.extend(prerequisites.get(mission_id, []))

    remaining = set(route)
    emitted = set()
    ordered = []
    while remaining:
        ready = [mission_id for mission_id in remaining
                 if all(pre not in route or pre in emitted
                        for pre in prerequisites.get(mission_id, []))]
        if not ready:
            raise SystemExit("Recommended route contains a dependency cycle")
        ready.sort(key=sort_key)
        mission_id = ready[0]
        remaining.remove(mission_id)
        emitted.add(mission_id)
        ordered.append(mission_id)
    return {mission_id: index + 1 for index, mission_id in enumerate(ordered)}


_STOCK_INITIAL_LONG_STAYS = {
    "st_kerbin_station_longstay3",
    "st_mun_station_longstay3",
    "st_duna_station_longstay2",
}


def normalize_stock_station_policy(mission: dict) -> dict:
    """Apply the common station policy to a parsed Stock design mission in place."""
    mission_id = mission.get("id", "")
    checks = mission.get("checks", [])
    station_key = mission.get("recordStation", "")
    dedicated_site = any(token in station_key.lower() for token in ("fuel", "depot", "base"))
    is_expansion = bool(re.match(r"^st_.*_station_upgrade\d+$", mission_id))
    is_initial_build = bool(station_key and station_key != "-" and not dedicated_site and
                            any(kind == "CREW_CAPACITY_MIN" for kind, _values, _label in checks) and
                            any(kind == "ORBIT_ABOVE" for kind, _values, _label in checks))

    if is_initial_build or is_expansion:
        if is_expansion:
            checks = [check for check in checks if check[0] != "CREW_NONE"]
            checks = [_replace_duration(check, 3, "keep the occupied station stable for 3 days")
                      if check[0] == "DURATION" else check for check in checks]

        capacity = next((int(dict(values)["min"]) for kind, values, _ in checks
                         if kind == "CREW_CAPACITY_MIN"), 0)
        if capacity > 0:
            mass, ports, power = station_engineering_requirements(capacity, is_expansion)
            required_checks = [
                ("MASS_MIN", [("amount", str(mass))], f"station mass at least {mass} tonnes"),
                ("DOCKING_PORT_COUNT", [("count", str(ports))],
                 f"at least {ports} docking ports"),
                ("POWER_CAPACITY_MIN", [("amount", str(power))],
                 f"ElectricCharge capacity at least {power}"),
            ]
            if is_expansion:
                required_checks.append(
                    ("MODULE_COUNT", [("module", SCIENCE_LAB_MODULES), ("count", "1")],
                     "at least one compatible science laboratory"))
            existing_kinds = {kind for kind, _values, _label in checks}
            checks.extend(check for check in required_checks if check[0] not in existing_kinds)

            actual = {kind: dict(values) for kind, values, _label in checks}
            actual_mass = actual["MASS_MIN"]["amount"]
            actual_ports = actual["DOCKING_PORT_COUNT"]["count"]
            actual_power = actual["POWER_CAPACITY_MIN"]["amount"]
            if is_expansion:
                policy_text = (
                    f" Mandatory engineering requirements: at least {actual_mass} tonnes, "
                    f"{actual_ports} docking ports, {actual_power} ElectricCharge capacity and "
                    "one compatible science laboratory.")
            else:
                policy_text = (
                    f" Mandatory construction certification: at least {actual_mass} tonnes, "
                    f"{actual_ports} docking ports and {actual_power} ElectricCharge capacity. "
                    "A compatible science laboratory becomes mandatory with the first expansion.")
            for key in ("description", "beschreibung", "beschreibung_en"):
                if key in mission:
                    mission[key] = mission[key].rstrip() + policy_text
        mission["checks"] = checks

    if "_station_longstay" in mission_id and mission_id not in _STOCK_INITIAL_LONG_STAYS:
        mission["checks"] = [_replace_duration(check, 120, "operate continuously for 120 days")
                             if check[0] == "DURATION" else check for check in mission.get("checks", checks)]
        for key in ("description", "beschreibung", "beschreibung_en"):
            if key in mission:
                mission[key] = mission[key].replace("150 days", "120 days").replace("60 days", "120 days")
    return mission


def upgrade_operational_checks(mission: dict) -> dict:
    """Upgrade newly accepted relay/logistics missions while ids and prerequisites stay stable.

    The plugin carries an evaluation-schema compatibility path for already-active old missions.
    """
    checks = mission.get("checks", [])
    dock = next((dict(values) for kind, values, _ in checks if kind == "DOCK_STATION"), None)
    upgraded = []
    for kind, values, label in checks:
        data = dict(values)
        if kind in ("RELAY_VESSEL_COUNT", "RELAY_VESSEL_COUNT_INCLINATION"):
            data["redundancy"] = "1"
            data["separationMin"] = "20"
            data["maxGap"] = "150"
            label = (f"phased relay network: {data.get('count', '3')} primary + 1 reserve, "
                     "largest orbital gap at most 150 degrees")
            upgraded.append(("RELAY_NETWORK_TOPOLOGY", list(data.items()), label))
        elif kind == "FUEL_MIN" and dock is not None:
            delivery = [("stationKey", dock["stationKey"]), ("resource", "Fuel"),
                        ("amount", data["amount"]), ("legacyKind", "FUEL_MIN")]
            upgraded.append(("RESOURCE_DELIVERY", delivery,
                             f"deliver at least {data['amount']} fuel to the recorded target"))
        else:
            upgraded.append((kind, values, label))
    mission["checks"] = upgraded
    return mission


def _replace_duration(check, days: int, label: str):
    kind, values, _old_label = check
    return kind, [(key, days if key == "days" else value) for key, value in values], label
