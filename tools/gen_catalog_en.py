#!/usr/bin/env python3
"""Generate the default English SOL contract catalog.

This script writes the main install catalog to:

  GameData/CustomScienceContracts/Contracts

The technical ids, prerequisites, enum values and checks stay identical to the German catalog so
save progress and mission logic remain compatible. Only player-facing titles, descriptions,
subcategories and checklist labels are localized.
"""
import os
import sys

sys.path.insert(0, os.path.dirname(__file__))
import gen_catalog as de

ROOT = de.ROOT
DOC = de.DOC
OUT = os.path.join(ROOT, "GameData", "CustomScienceContracts", "Contracts")

SPARTE = de.SPARTE

SUB_TRANSLATE = {
    "Erde": "Earth",
    "Luna": "Moon",
    "Merkur": "Mercury",
    "Neptun": "Neptune",
    "Sonne": "Sun",
    "Asteroiden": "Asteroids",
    "Interplanetar": "Interplanetary",
    "Logistik": "Logistics",
}

SUBCAT = {body: SUB_TRANSLATE.get(sub, sub) for body, sub in de.SUBCAT.items()}
REVEAL = {SUB_TRANSLATE.get(sub, sub): target for sub, target in de.REVEAL.items()}

DISPLAY = {
    "Moon": "Moon",
    "Sun": "Sun",
    "Mercury": "Mercury",
    "Neptune": "Neptune",
    "Ganymede": "Ganymede",
    "Callisto": "Callisto",
}

TITLE = {
    "un_earth_pad_clear": "First Test Flight",
    "un_earth_upper_atmo": "Upper Earth Atmosphere",
    "un_earth_suborbital": "Uncrewed Suborbital Flight",
    "cr_earth_suborbital": "First Crewed Suborbital Flight",
    "un_earth_orbit": "First Earth Orbit (Uncrewed)",
    "un_earth_science_satellite": "First Science Satellite",
    "un_earth_satellite_pair": "First Earth Satellite Network",
    "un_earth_high_satellite": "High Earth Orbit Satellite",
    "cr_earth_orbit": "First Crewed Earth Orbit",
    "cr_earth_orbit_eva": "First EVA in Earth Orbit",
    "cr_earth_duration_3d": "Three Days in Earth Orbit",
    "cr_earth_docking_demo": "First Docking Maneuver",
    "cr_earth_duration_7d": "One Week in Earth Orbit",
    "cr_earth_trial_station": "Single-Module Orbital Laboratory",
    "cr_luna_flyby_crewed": "First Crewed Lunar Flyby",
    "un_luna_rover": "Rover Landing on the Moon",
    "un_mars_rover": "Rover Landing on Mars",
    "un_luna_polar_landing": "Polar Landing on the Moon",
    "un_mars_polar_landing": "Polar Landing on Mars",
    "un_titan_polar_landing": "Polar Landing on Titan",
    "un_luna_polar_mapping": "Polar Mapping of the Moon",
    "un_mercury_polar_mapping": "Polar Mapping of Mercury",
    "un_mars_polar_mapping": "Polar Mapping of Mars",
    "un_jupiter_polar_mapping": "Polar Mapping of the Jupiter System",
    "un_saturn_polar_mapping": "Polar Mapping of the Saturn System",
    "un_titan_polar_mapping": "Polar Mapping of Titan",
    "un_triton_polar_mapping": "Polar Mapping of Triton",
    "un_pluto_polar_mapping": "Polar Mapping of Pluto",
    "net_earth_comm_network3": "New Earth Communications Network",
    "net_earth_polar_comm_network": "Polar Earth Relay Layer",
    "net_luna_comm_network3": "Moon Communications Network",
    "net_luna_polar_comm_network": "Polar Moon Relay Layer",
    "net_mars_comm_network": "Mars Communications Network",
    "net_solar_comm_network": "Interplanetary Relay Ring",
    "net_jupiter_comm_network": "Jupiter Communications Network",
    "net_saturn_comm_network": "Saturn Communications Network",
    "un_sun_inner_probe": "Inner Solar Probe",
    "cr_titan_base4": "Titan Base (4 Kerbals)",
    "cr_titan_base6": "Titan Base Expansion (6 Kerbals)",
    "cr_titan_base8": "Titan Base Expansion (8 Kerbals)",
}


def disp(body):
    return DISPLAY.get(body, body)


def plural(n, word):
    return f"{n} {word}" if int(float(n)) == 1 else f"{n} {word}s"


def title_for(m):
    if m["id"] in TITLE:
        return TITLE[m["id"]]
    body = disp(m["body"])
    crewed = m["sparte"] == "Pioniere"
    kinds = [k for k, _, _ in m["checks"]]
    days = next((dict(kv).get("days") for k, kv, _ in m["checks"] if k == "DURATION"), None)
    def kv(kind):
        return next((dict(kvl) for kk, kvl, _ in m["checks"] if kk == kind), {})

    if "RESOURCE_MIN" in kinds or "FUEL_MIN" in kinds:
        noun = f"Fuel Depot near {body}" if "LANDED" not in kinds else f"Fuel Depot on {body}"
    elif "RETURN_FROM_BODY" in kinds and "FLYBY" in kinds:
        noun = f"Flyby and Return from {body}"
    elif "RETURN_FROM_BODY" in kinds and ("LANDED" in kinds or "MARKER_LANDING" in kinds):
        noun = f"Landing and Return from {body}"
    elif "ORE_SURFACE" in kinds:
        noun = f"Ore Mining on {body}"
    elif "MARKER_LANDING" in kinds:
        noun = f"Precision Landing on {body}"
    elif "DOCK_ANY" in kinds:
        noun = f"Docking Maneuver in {body} Orbit"
    elif "EVA" in kinds and "LANDED" not in kinds:
        noun = f"EVA in {body} Orbit"
    elif "LANDED" in kinds:
        if days:
            noun = f"{int(float(days))} Days on {body}"
        else:
            noun = f"Landing on {body}"
    elif "FLYBY" in kinds:
        noun = f"Flyby of {body}"
    elif "VESSEL_COUNT_INCLINATION" in kinds or "VESSEL_COUNT" in kinds:
        fleet = "VESSEL_COUNT_INCLINATION" if "VESSEL_COUNT_INCLINATION" in kinds else "VESSEL_COUNT"
        count = int(kv(fleet).get("count", "1"))
        noun = f"Satellite Network around {body}" if count >= 2 else f"High Orbit Satellite around {body}"
        if "VESSEL_COUNT_INCLINATION" in kinds:
            noun = f"Polar {noun}"
    elif "ATMO_FRACTION" in kinds:
        noun = f"Atmospheric Probe at {body}"
    elif "SUBORBITAL" in kinds:
        noun = f"Suborbital Flight over {body}"
    elif "ORBIT_ABOVE" in kinds:
        noun = f"Polar Mapping Orbit around {body}" if "INCLINATION_MIN" in kinds else f"Orbit around {body}"
    else:
        noun = f"Mission at {body}"
    if crewed and not noun[0].isdigit() and not noun.startswith(("Crewed", "First", "Three", "One")):
        noun = f"Crewed {noun}"
    elif not crewed and not noun[0].isdigit() and "Network" not in noun and not noun.startswith(("First", "Polar", "High")):
        noun = f"Robotic {noun}"
    return noun


def mission_action(m, title):
    body = disp(m["body"])
    kinds = [k for k, _, _ in m["checks"]]
    crewed = m["sparte"] == "Pioniere"
    actor = "crew" if crewed else "probe"

    if m["id"].startswith("net_"):
        return f"Build the next communications layer around {body}. It replaces the aging early relay net with a stronger infrastructure backbone."
    if "RETURN_FROM_BODY" in kinds and "FLYBY" in kinds:
        return f"Fly crew past {body}, leave the encounter safely, and bring them back to Earth for recovery."
    if "RETURN_FROM_BODY" in kinds and ("LANDED" in kinds or "MARKER_LANDING" in kinds):
        return f"Land the crew on {body}, complete the surface objectives, and bring them safely back to Earth for recovery."
    if "VESSEL_COUNT_INCLINATION" in kinds:
        return f"Deploy a polar constellation around {body} and keep the required relay craft in orbit at the target inclination."
    if "VESSEL_COUNT" in kinds:
        return f"Deploy the requested constellation around {body} and keep the vessels operating in the required orbit."
    if "MARKER_LANDING" in kinds:
        return f"Land at the designated target zone on {body}. Precision matters because later crews, bases and supply flights need reliable repeatability."
    if "LANDED" in kinds:
        return f"Land the {actor} on {body} and keep the mission stable long enough to prove surface operations are possible."
    if "FLYBY" in kinds:
        return f"Send the {actor} past {body} without entering a permanent orbit. The flyby opens the path for deeper exploration."
    if "ATMO_FRACTION" in kinds:
        return f"Send an atmospheric probe through the target layer at {body} and return useful entry data."
    if "SUBORBITAL" in kinds:
        return f"Launch beyond the atmosphere on a controlled suborbital flight and prove the vehicle can reach space."
    if "DOCK_ANY" in kinds or "DOCK_STATION" in kinds:
        return "Complete the docking operation and demonstrate that separate spacecraft can be joined safely in orbit."
    if "EVA" in kinds:
        return f"Perform an EVA at {body} and prove that Kerbals can work outside the vessel."
    if "ORE_SURFACE" in kinds:
        return f"Extract Ore on {body} and show that local resources can support future operations."
    if "RESOURCE_MIN" in kinds or "FUEL_MIN" in kinds:
        return "Stock the depot with the required resources so future missions can refuel away from the launch pad."
    if "ORBIT_ABOVE" in kinds:
        return f"Place the {actor} into a stable orbit around {body} and hold the required orbital conditions."
    return f"Complete {title} and continue the campaign sequence."


def description_for(m, title):
    action = mission_action(m, title)
    branch = {
        "Pioniere": "crewed exploration",
        "Robotische Erkunder": "robotic exploration",
        "Versorgungsnetz": "network and logistics",
    }.get(m["sparte"], "exploration")
    if m.get("repeatable") == "yes":
        return f"{action} This repeatable {branch} contract keeps established infrastructure useful without blocking the main campaign."
    return f"{action} This {branch} milestone pays a science bonus when all listed objectives are complete."


def label_for(kind, kvl, mission):
    kv = dict(kvl)
    body = disp(kv.get("body", mission.get("body", "")))
    if kind == "CREW_MIN":
        return f"At least {plural(kv['min'], 'Kerbal')} aboard"
    if kind == "CREW_NONE":
        return "Uncrewed, no Kerbals aboard"
    if kind == "CREW_EXACT":
        return f"Exactly {plural(kv['min'], 'Kerbal')} aboard"
    if kind == "SUBORBITAL":
        return f"Suborbital spaceflight above {body}"
    if kind == "LANDED":
        return f"Landed on {body}"
    if kind == "ORE_SURFACE":
        return f"Mine Ore on {body}"
    if kind == "ORBIT_ABOVE":
        return f"Periapsis above {kv['km']} km" if "km" in kv else f"Stable orbit above {body}'s atmosphere"
    if kind == "INCLINATION_MIN":
        return f"Orbital inclination at least {kv['inclinationMin']} degrees"
    if kind == "ATMO_FRACTION":
        return f"Reach {pct(kv['fracMin'])}-{pct(kv['fracMax'])}% of {body}'s atmosphere height"
    if kind == "FLYBY":
        return f"Fly by {body} below {kv['km']} km"
    if kind == "MARKER_LANDING":
        band = "polar " if float(kv.get("latMin", "0")) >= 60 else ""
        return f"Land within {kv['km']} km of the {band}target marker on {body}"
    if kind == "VESSEL_COUNT":
        km = f" above {kv['km']} km" if "km" in kv else ""
        return f"{kv['count']} vessels in orbit{km} around {body}"
    if kind == "VESSEL_COUNT_INCLINATION":
        km = f" above {kv['km']} km" if "km" in kv else ""
        return f"{kv['count']} vessels in orbit{km} around {body} at {kv['inclinationMin']}+ degrees inclination"
    if kind == "EVA":
        return f"EVA at {body}" if body else "EVA"
    if kind == "FUEL_MIN":
        return f"More than {kv['amount']} units of fuel aboard"
    if kind == "RESOURCE_MIN":
        return f"More than {kv['amount']} {kv['resource']} aboard"
    if kind == "DOCK_ANY":
        return "Complete any docking maneuver"
    if kind == "DOCK_STATION":
        return "Dock to the recorded station"
    if kind == "HOLD":
        return f"Hold for {kv['seconds']} seconds"
    if kind == "DURATION":
        days = float(kv["days"])
        return f"Hold continuously for {int(days) if days.is_integer() else kv['days']} day{'s' if days != 1 else ''}"
    if kind == "RETURN_FROM_BODY":
        home = disp(kv.get("returnBody", "Earth"))
        mode = kv.get("returnMode", "")
        return f"Fly by {body}, then return crew to {home}" if mode == "flyby" else f"Return crew safely from {body} to {home}"
    return kind.replace("_", " ").title()


def pct(v):
    return f"{float(v) * 100:.0f}"


def english_checks(m):
    return [(kind, kvl, label_for(kind, kvl, m)) for kind, kvl, _ in m["checks"]]


def mission_contract(m):
    prereqs = [] if m.get("prereq", "-") in ("-", "") else [p.strip() for p in m["prereq"].split(",")]
    sub = SUBCAT[m["body"]]
    reveal = REVEAL.get(sub) if m["sparte"] == "Robotische Erkunder" else None
    title = title_for(m)
    return de.contract(
        m["id"], title, description_for(m, title), SPARTE[m["sparte"]], sub,
        m.get("icon") or de.icon_for(m), m["reward"], prereqs, english_checks(m),
        repeatable=(m.get("repeatable") == "yes"), reveal=reveal)


HEADER = ("// ===========================================================================\n"
          "//  {t}\n"
          "//  GENERATED from custom_science_contracts_missionsdesign.md (tools/gen_catalog_en.py).\n"
          "//  Default English SOL catalog. Do not edit generated cfg files by hand.\n"
          "//  Body names remain internal CelestialBody.name values (Moon = internal body Moon, star = Sun).\n"
          "// ===========================================================================\n\n"
          "CUSTOM_CONTRACT_CATALOG\n{{\n")


def write_file(path, title, body):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(HEADER.format(t=title) + body + "}\n")


def kerbals(n):
    return "1 Kerbal" if int(n) == 1 else f"{n} Kerbals"


def make_check(kind, label, **values):
    return (kind, [(k, v) for k, v in values.items()], label)


def orbit_chain(key, body, sub, orbitword, km, stages, prereq0, station_word, mult):
    out, prev_long = [], None
    for i, n in enumerate(stages):
        build = i == 0
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup, lng = f"cr_{key}_supply{n}", f"cr_{key}_longstay{n}"
        checks = [
            make_check("CREW_MIN", f"Crewed with at least {kerbals(n)} aboard", min=n),
            make_check("ORBIT_ABOVE", f"Stable {orbitword}, periapsis above {km} km", body=body, km=km),
            make_check("DURATION", f"Hold for 10 days with {kerbals(n)} aboard", days=10),
        ]
        if build:
            title = f"{station_word} ({kerbals(n)})"
            desc = f"Build your first {station_word.lower()} and operate it with at least {kerbals(n)} aboard. The station name will be reused by future supply flights."
            out.append(de.contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapStation",
                       round(220 * mult), [prereq0], checks, record=key))
        else:
            title = f"Station Expansion to {kerbals(n)}"
            desc = f"Expand %station% to at least {kerbals(n)} and keep the new operating level stable before the next upgrade."
            out.append(de.contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapStation",
                       round((180 + 20 * n) * mult), [prev_long], checks, ref=key))
        out.append(de.contract(sup, f"Station Resupply ({kerbals(n)})",
                   f"Bring a fresh crew of at least {kerbals(n)} to %station% and dock with the station.",
                   "Bemannt", sub, "TrackingStation_ButtonMapShips", round((110 + 12 * n) * mult), [sid],
                   [make_check("CREW_MIN", f"Supply craft with at least {kerbals(n)} aboard", min=n),
                    make_check("ORBIT_ABOVE", f"Stable {orbitword}, periapsis above {km} km", body=body, km=km),
                    make_check("DOCK_STATION", "Docked to the station", stationKey=key)],
                   repeatable=True, ref=key))
        out.append(de.contract(lng, f"150-Day Operations ({kerbals(n)})",
                   f"Keep %station% crewed continuously for 150 days with at least {kerbals(n)} aboard.",
                   "Bemannt", sub, "TrackingStation_ButtonMapStation", round((260 + 30 * n) * mult), [sup],
                   [make_check("CREW_MIN", f"Crewed with at least {kerbals(n)} aboard", min=n),
                    make_check("ORBIT_ABOVE", f"Stable {orbitword}, periapsis above {km} km", body=body, km=km),
                    make_check("DURATION", f"Hold for 150 days with {kerbals(n)} aboard", days=150)],
                   ref=key))
        prev_long = lng
    return "".join(out)


def base_chain(key, body, sub, stages, prereq0, base_word, mult):
    out, prev_long = [], None
    body_name = disp(body)
    prereq0_list = prereq0 if isinstance(prereq0, list) else [prereq0]
    for i, n in enumerate(stages):
        build = i == 0
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup, lng = f"cr_{key}_supply{n}", f"cr_{key}_longstay{n}"
        checks = [
            make_check("CREW_MIN", f"Crewed with at least {kerbals(n)} aboard", min=n),
            make_check("LANDED", f"Landed on {body_name}", body=body),
            make_check("DURATION", f"Hold for 10 days with {kerbals(n)} aboard", days=10),
        ]
        if build:
            title = f"{base_word} ({kerbals(n)})"
            desc = f"Build your first base on {body_name} and keep it occupied with at least {kerbals(n)}. Its name will be used by future supply flights."
            out.append(de.contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapBase",
                       round(240 * mult), prereq0_list, checks, record=key))
        else:
            title = f"Base Expansion to {kerbals(n)}"
            desc = f"Expand %station% to at least {kerbals(n)} and keep the base operating steadily."
            out.append(de.contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapBase",
                       round((180 + 20 * n) * mult), [prev_long], checks, ref=key))
        out.append(de.contract(sup, f"Base Resupply ({kerbals(n)})",
                   f"Land a fresh crew of at least {kerbals(n)} at %station% and keep surface operations running.",
                   "Bemannt", sub, "TrackingStation_ButtonMapLander", round((110 + 12 * n) * mult), [sid],
                   [make_check("CREW_MIN", f"Supply lander with at least {kerbals(n)} aboard", min=n),
                    make_check("LANDED", f"Landed on {body_name}", body=body)],
                   repeatable=True, ref=key))
        out.append(de.contract(lng, f"150-Day Base Operations ({kerbals(n)})",
                   f"Keep %station% alive continuously for 150 days with at least {kerbals(n)} aboard.",
                   "Bemannt", sub, "TrackingStation_ButtonMapBase", round((260 + 30 * n) * mult), [sup],
                   [make_check("CREW_MIN", f"Crewed with at least {kerbals(n)} aboard", min=n),
                    make_check("LANDED", f"Landed on {body_name}", body=body),
                    make_check("DURATION", f"Hold for 150 days with {kerbals(n)} aboard", days=150)],
                   ref=key))
        prev_long = lng
    return "".join(out)


def fuel_depot_chain(key, sub, stages, prereq0, mult):
    out, prev = [], None
    for i, n in enumerate(stages):
        build = i == 0
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup = f"cr_{key}_supply{n}"
        lf, ox = 1440 * (i + 1), 1760 * (i + 1)
        checks = [
            make_check("CREW_MIN", f"Crewed with at least {kerbals(n)} aboard", min=n),
            make_check("ORBIT_ABOVE", "Stable Earth orbit, periapsis above 130 km", body="Earth", km=130),
            make_check("RESOURCE_MIN", f"At least {lf} LiquidFuel in storage", resource="LiquidFuel", amount=lf),
            make_check("RESOURCE_MIN", f"At least {ox} Oxidizer in storage", resource="Oxidizer", amount=ox),
            make_check("DURATION", "Operate for 10 days", days=10),
        ]
        if build:
            title = f"Orbital Fuel Depot ({kerbals(n)})"
            desc = f"Build a crewed fuel depot in Earth orbit, staff it with at least {kerbals(n)}, and stock {lf} LiquidFuel plus {ox} Oxidizer."
            out.append(de.contract(sid, title, desc, "NetzwerkLogistik", sub, "TrackingStation_ButtonMapStation",
                       round(150 * mult), [prereq0], checks, record=key))
        else:
            title = f"Fuel Depot Expansion ({kerbals(n)})"
            desc = f"Expand %station% to at least {kerbals(n)} and raise its stockpile to {lf} LiquidFuel and {ox} Oxidizer."
            out.append(de.contract(sid, title, desc, "NetzwerkLogistik", sub, "TrackingStation_ButtonMapStation",
                       round((150 + 25 * i) * mult), [prev], checks, ref=key))
        out.append(de.contract(sup, f"Fuel Depot Resupply ({kerbals(n)})",
                   f"Bring fresh fuel and a crew of at least {kerbals(n)} to %station% and dock.",
                   "NetzwerkLogistik", sub, "TrackingStation_ButtonMapShips", round((90 + 12 * n) * mult), [sid],
                   [make_check("CREW_MIN", f"Supply craft with at least {kerbals(n)} aboard", min=n),
                    make_check("ORBIT_ABOVE", "Stable Earth orbit, periapsis above 130 km", body="Earth", km=130),
                    make_check("DOCK_STATION", "Docked to the fuel depot", stationKey=key)],
                   repeatable=True, ref=key))
        prev = sid
    return "".join(out)


def build_stations():
    s = ""
    s += "    // ===== EARTH - Space station (2 -> 12 Kerbals) =====\n"
    s += orbit_chain("earth_station", "Earth", "Earth", "Earth orbit", 130,
                     [2, 3, 4, 6, 8, 10, 12], "cr_luna_landing", "Earth Orbital Station", 1.0)
    s += "\n    // ===== MOON - Space station =====\n"
    s += orbit_chain("moon_station", "Moon", "Moon", "lunar orbit", 25,
                     [2, 3, 4, 6, 8, 10], "cr_earth_station_longstay4", "Lunar Orbital Station", 1.5)
    s += "\n    // ===== MOON - Surface base =====\n"
    s += base_chain("moon_base", "Moon", "Moon", [2, 3, 4, 6, 8, 10],
                    ["cr_luna_precision_landing", "cr_luna_stay_7d"], "Moon Base", 1.5)
    s += "\n    // ===== MARS - Space station =====\n"
    s += orbit_chain("mars_station", "Mars", "Mars", "Mars orbit", 90,
                     [2, 3, 4, 6], "cr_mars_stay_10d", "Mars Orbital Station", 2.4)
    s += "\n    // ===== MARS - Surface base =====\n"
    s += base_chain("mars_base", "Mars", "Mars", [2, 3, 4, 6], "cr_mars_stay_30d", "Mars Base", 2.6)
    s += "\n    // ===== EARTH - Crewed orbital fuel depot =====\n"
    s += fuel_depot_chain("earth_fuel_depot", "Earth", [2, 3, 4, 6], "cr_earth_station_longstay4", 1.0)
    return s


def main():
    text = open(DOC, encoding="utf-8").read()
    missions = de.parse_missions(text)
    buckets = {"Pioniere": [], "Robotische Erkunder": [], "Versorgungsnetz": []}
    for m in missions:
        buckets[m["sparte"]].append(mission_contract(m))
    write_file(os.path.join(OUT, "A_Pioniere.cfg"), "BRANCH A - PIONEERS (crewed)", "".join(buckets["Pioniere"]))
    write_file(os.path.join(OUT, "B_Spaeher.cfg"), "BRANCH B - ROBOTIC EXPLORERS", "".join(buckets["Robotische Erkunder"]))
    write_file(os.path.join(OUT, "C_Lebensadern.cfg"), "BRANCH C - LIFELINES (network/logistics)", "".join(buckets["Versorgungsnetz"]))
    write_file(os.path.join(OUT, "D_Stationen.cfg"), "STATIONS, BASES AND DEPOTS", build_stations())
    print(f"A Pioneers:          {len(buckets['Pioniere'])}")
    print(f"B Robotic Explorers: {len(buckets['Robotische Erkunder'])}")
    print(f"C Lifelines:         {len(buckets['Versorgungsnetz'])}")
    print("D Stations: generated")
    print("Written to", OUT)


if __name__ == "__main__":
    main()
