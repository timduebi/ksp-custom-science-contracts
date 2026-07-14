#!/usr/bin/env python3
"""Generate the optional Stock KSP contract catalog.

The default GameData catalog remains SOL. This generator reads the separate Stock
mission design and writes only OptionalConfigs/Stock, so Stock players can install
it as an overlay without changing the SOL source catalogs.
"""
import os
import re
from catalog_common import (normalize_stock_station_policy, parse_check as parse_common_check,
                            recommended_route_order, upgrade_operational_checks)

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DOC = os.path.join(ROOT, "custom_science_contracts_stock_missionsdesign.md")
OUT = os.path.join(ROOT, "OptionalConfigs", "Stock", "GameData", "CustomScienceContracts", "Contracts")

SPARTE = {
    "Pioniere": "Bemannt",
    "Robotische Erkunder": "UnbemannteErkundung",
    "Versorgungsnetz": "NetzwerkLogistik",
    "Stationen": "Stationen",
    "Pioneers": "Bemannt",
    "Robotic Explorers": "UnbemannteErkundung",
    "Lifelines": "NetzwerkLogistik",
    "Stations": "Stationen",
}

BUCKET_FILES = {
    "Bemannt": ("A_Pioniere.cfg", "BRANCH A - PIONEERS (crewed)"),
    "UnbemannteErkundung": ("B_Spaeher.cfg", "BRANCH B - ROBOTIC EXPLORERS"),
    "Stationen": ("D_Stationen.cfg", "BRANCH D - STATIONS (infrastructure chains)"),
    "NetzwerkLogistik": ("C_Lebensadern.cfg", "BRANCH C - LIFELINES"),
}

# Epoch story texts shown by the atlas intro panel (EPOCH nodes in the A catalog file).
# English retellings of the German chapter notes in the Stock design source.
EPOCH_TEXTS = {
    1: "True Stock style: a crewed launch right away — suborbital hops, the first orbit, an EVA "
       "and three days around Kerbin. Early probes fly alongside as optional science runs.",
    2: "Docking is learned the honest way: a three-seat Kerbin station goes up uncrewed, then "
       "the crew arrives — and stays. 150-day rotations, a fuel depot and Kerbin's first relay "
       "net make orbit a place to live, not to visit.",
    3: "The Mun is the first great leap. Probes prepare the ground, crews orbit and land, rovers "
       "and precision landings scout the sites — then a Mun station and the first surface base "
       "take root.",
    4: "Minmus opens as the program's fuel country: flyby, landing, a relay net and the fuel "
       "base on its flats — while the Mun infrastructure grows to six Kerbals.",
    5: "Eve, Moho and Dres open the inner side paths: atmo probes and landers at Eve, the "
       "Mohole hunt at Moho's north pole. Optional, stubborn, rewarding.",
    6: "Duna is the first serious interplanetary arc: probes, rovers and crew landings, then a "
       "station in orbit and a base in the dust. After the four-seat upgrade the deep-space "
       "phase can begin.",
    7: "An interplanetary relay ring in solar orbit opens Jool and Eeloo at once. Duna "
       "operations grow into deep-space routine, and Eeloo gets a direct crewed landing at the "
       "edge of the system.",
    8: "The giant and its moons: atmospheric entry at Jool, then moon by moon — Laythe with its "
       "own base growing to fifteen Kerbals, and a three-Kerbal challenge outpost on Tylo.",
    9: "Everything led here: Gilly as the fuel stepping stone, a support station above the "
       "purple planet — then the crewed Eve landing and return. And afterwards, one absurd "
       "question: are we living here now?",
}

EPOCH_TRANSITIONS = {
    1: "Kerbin orbit is no longer a stunt. The next task is to make it somewhere Kerbals can live, work and call home from.",
    2: "Docking, long stays and a working relay net turn Kerbin orbit into infrastructure. The Mun is now a destination rather than scenery.",
    3: "Bootprints, a station and a base make the Mun part of the program. Minmus offers the first chance to turn exploration into practical fuel operations.",
    4: "The Kerbin system can now support itself. Mission Control is free to open the stubborn inner-system side paths before committing to Duna.",
    5: "Eve, Moho and Dres have tested the program's patience. The next chapter focuses every lesson on one sustained interplanetary campaign: Duna.",
    6: "Duna is no longer a flag stop but an operating destination. Its expanded station and relay network can now carry signals and crews into deep space.",
    7: "The interplanetary switchboard is live. Jool's moons are connected to home, and the program can attempt its most varied frontier yet.",
    8: "Laythe proves that a living outpost can exist in the Jool system. The final examination waits at Eve, where landing is easy and leaving is not.",
    9: "The recommended route is complete. Kerbals have learned to launch, dock, settle moons, cross interplanetary space and climb back from Eve.",
}

CAMPAIGN_FINALE = ("MISSION CONTROL â€” FINAL ENTRY: The Eve crew is home. The purple planet took every lesson the "
                    "program had learned and returned proof that none of it was wasted. The campaign is complete; "
                    "unfinished side missions remain open for whichever wonderfully unnecessary idea comes next.")

RECOMMENDED_MILESTONES = {
    "cr_kerbin_first_launch", "cr_kerbin_orbit", "cr_kerbin_orbit_3d",
    "st_kerbin_station_core3", "st_kerbin_station_longstay3", "net_kerbin_relay3",
    "cr_mun_landing", "st_mun_station_longstay3", "base_mun_base3",
    "cr_minmus_landing", "base_minmus_fuel_base",
    "cr_eve_orbit", "net_eve_relay3",
    "net_duna_relay3", "cr_duna_landing", "st_duna_station_upgrade4",
    "net_interplanetary_relay3", "cr_eeloo_landing",
    "net_jool_relay3", "cr_laythe_landing", "base_laythe_base3",
    "st_eve_support_station", "cr_eve_landing_return",
}

def epoch_nodes(epoch_names):
    s = ""
    for number in sorted(EPOCH_TEXTS):
        s += "    EPOCH\n    {\n"
        s += f"        number = {number}\n"
        s += f"        name = {epoch_names.get(number, 'Epoch ' + str(number))}\n"
        s += f"        description = {EPOCH_TEXTS[number]}\n"
        s += "    }\n"
    return s + "\n"

SUBCAT = {
    "Sun": "Outer System",
    "Kerbin": "Kerbin",
    "Mun": "Mun",
    "Minmus": "Minmus",
    "Moho": "Moho",
    "Eve": "Eve",
    "Gilly": "Gilly",
    "Duna": "Duna",
    "Ike": "Ike",
    "Dres": "Dres",
    "Jool": "Jool System",
    "Laythe": "Laythe",
    "Vall": "Vall",
    "Tylo": "Tylo",
    "Bop": "Bop",
    "Pol": "Pol",
    "Eeloo": "Eeloo",
}

BODY_NAMES = {
    "kerbin": "Kerbin",
    "mun": "Mun",
    "minmus": "Minmus",
    "moho": "Moho",
    "eve": "Eve",
    "gilly": "Gilly",
    "duna": "Duna",
    "ike": "Ike",
    "dres": "Dres",
    "jool": "Jool",
    "laythe": "Laythe",
    "vall": "Vall",
    "tylo": "Tylo",
    "bop": "Bop",
    "pol": "Pol",
    "eeloo": "Eeloo",
    "sun": "Sun",
}

WORD_NAMES = {
    "cr": "Crewed",
    "un": "Uncrewed",
    "net": "Relay",
    "st": "Station",
    "dep": "Depot",
    "rep": "Repeatable",
    "eva": "EVA",
    "atmo": "Atmosphere",
    "3d": "3-Day",
    "10d": "10-Day",
    "30d": "30-Day",
    "longstay": "Long Stay",
    "flyby": "Flyby",
    "orbiter": "Orbiter",
    "orbit": "Orbit",
    "lander": "Lander",
    "landing": "Landing",
    "relay": "Relay",
    "polar": "Polar",
    "precision": "Precision",
    "rover": "Rover",
    "fuel": "Fuel",
    "depot": "Depot",
    "base": "Base",
    "supply": "Supply",
    "upgrade": "Upgrade",
    "core": "Core",
    "station": "Station",
    "support": "Support",
    "gateway": "Gateway",
    "return": "Return",
    "launch": "Launch",
}

EPOCH_EXACT = {
    # 1 - Getting Away With It
    "cr_kerbin_first_launch": 1,
    "cr_kerbin_suborbital": 1,
    "cr_kerbin_orbit": 1,
    "cr_kerbin_eva": 1,
    "cr_kerbin_orbit_3d": 1,
    "un_kerbin_sounding_probe": 1,
    "un_kerbin_first_satellite": 1,
    "un_kerbin_polar_satellite": 1,

    # 2 - Small Station, Big Ideas
    "st_kerbin_station_core3": 2,
    "st_kerbin_station_crew3": 2,
    "st_kerbin_station_longstay3": 2,
    "st_kerbin_station_upgrade4": 2,
    "st_kerbin_station_longstay4": 2,
    "st_kerbin_station_upgrade6": 2,
    "st_kerbin_station_longstay6": 2,
    "st_kerbin_station_upgrade8": 2,
    "st_kerbin_station_longstay8": 2,
    "st_kerbin_station_upgrade10": 2,
    "st_kerbin_station_longstay10": 2,
    "net_kerbin_relay3": 2,
    "net_kerbin_relay6_polar": 2,
    "dep_kerbin_fuel_depot_core": 2,
    "dep_kerbin_fuel_delivery": 2,
    "rep_kerbin_station_resupply": 2,

    # 3 - Mun or Bust
    "cr_mun_flyby": 3,
    "cr_mun_orbit": 3,
    "un_mun_orbiter": 3,
    "cr_mun_landing": 3,
    "un_mun_rover": 3,
    "un_mun_precision_rover_landing": 3,
    "cr_mun_precision_landing": 3,
    "cr_mun_polar_precision_landing": 3,
    "net_mun_relay3": 3,
    "net_mun_relay6_polar": 3,
    "st_mun_station_core3": 3,
    "st_mun_station_longstay3": 3,
    "st_mun_station_upgrade6": 3,
    "st_mun_station_longstay6": 3,
    "base_mun_base3": 3,
    "base_mun_base6": 3,
    "rep_mun_base_supply": 3,

    # 4 - Minty Fuel Dreams
    "cr_minmus_flyby": 4,
    "cr_minmus_landing": 4,
    "cr_minmus_precision_landing": 4,
    "net_minmus_relay3": 4,
    "base_minmus_fuel_base": 4,

    # 7 - The Deep-Space Switchboard
    "net_interplanetary_relay3": 7,
    "un_eeloo_flyby": 7,
    "un_eeloo_lander": 7,
    "cr_eeloo_landing": 7,
    "un_jool_atmo_probe": 7,

    # 9 - The Purple Final Exam
    "cr_eve_landing_return": 9,
}

DEFAULT_EPOCH_NAMES = {
    1: "First Sparks",
    2: "Orbital Habits",
    3: "Mun or Bust",
    4: "Minty Operations",
    5: "Inner Mischief",
    6: "Red Dust",
    7: "Deep-Space Lifeline",
    8: "Jool Frontier",
    9: "The Purple Finale",
}

HEADER = """// ===========================================================================
//  {title}
//  GENERATED from custom_science_contracts_stock_missionsdesign.md (tools/gen_catalog_stock.py).
//  DO NOT EDIT BY HAND - change the mission design file and regenerate.
//  Body names are stock KSP internal CelestialBody.name values.
// ===========================================================================

CUSTOM_CONTRACT_CATALOG
{{
"""


def parse_epoch_names(text):
    names = dict(DEFAULT_EPOCH_NAMES)
    for line in text.splitlines():
        match = re.match(r"Epoche\s+(\d+)\s+[—-]\s+([^:]+):", line.strip())
        if match:
            names[int(match.group(1))] = match.group(2).strip()
    return names


def parse_missions(text):
    missions = []
    keys = ("id|title|sparte|body|subcategory|epoche|epoch|epochName|prereq|reward|repeatable|recordStation|"
            "stationRef|description|beschreibung|beschreibung_en|icon")
    for blk in text.split("=== MISSION ===")[1:]:
        m, checks = {}, []
        for line in blk.splitlines():
            s = line.strip()
            if not s:
                continue
            if s.startswith("=="):
                break
            if s.startswith("check:"):
                checks.append(parse_common_check(s[6:].strip()))
                continue
            mm = re.match(rf"({keys}):\s*(.*)$", s)
            if mm:
                m[mm.group(1)] = mm.group(2).strip()
        if "id" in m:
            m["checks"] = checks
            missions.append(upgrade_operational_checks(normalize_stock_station_policy(m)))
    return missions


def fallback_epoch_for_id(cid):
    if cid in EPOCH_EXACT:
        return EPOCH_EXACT[cid]
    if cid.startswith(("un_eve_", "cr_eve_", "cr_gilly_", "un_gilly_", "un_moho_", "cr_moho_", "un_dres_", "cr_dres_", "st_eve_", "un_gilly_")):
        return 5
    if cid.startswith(("un_duna_", "cr_duna_", "cr_ike_", "st_duna_", "base_duna_", "net_duna_", "rep_duna_")):
        return 6
    if cid.startswith(("un_jool_", "net_jool_", "st_jool_", "un_laythe_", "cr_laythe_", "base_laythe_",
                       "un_vall_", "cr_vall_", "un_tylo_", "cr_tylo_", "outpost_tylo_", "un_bop_",
                       "cr_bop_", "un_pol_", "cr_pol_", "rep_laythe_")):
        return 8
    return 1


def epoch_for(m):
    value = m.get("epoche") or m.get("epoch")
    if value:
        try:
            epoch = int(value)
        except ValueError:
            raise SystemExit(f"Invalid epoche '{value}' for mission '{m['id']}'")
        if 1 <= epoch <= 9:
            return epoch
        raise SystemExit(f"Invalid epoche '{value}' for mission '{m['id']}'")
    return fallback_epoch_for_id(m["id"])


def title_for(m):
    if m.get("title"):
        return m["title"]
    cid = m["id"]
    words = []
    for token in cid.split("_"):
        match = re.match(r"([a-z]+)(\d+d?|\d+)?$", token)
        base = token
        suffix = ""
        if match:
            base, suffix = match.groups()
            suffix = suffix or ""
        word = BODY_NAMES.get(base) or WORD_NAMES.get(base) or base.capitalize()
        if suffix:
            if suffix.endswith("d"):
                word += " " + suffix[:-1] + "-Day"
            else:
                word += " " + suffix
        words.append(word)
    title = " ".join(words)
    title = re.sub(r"^(Crewed|Uncrewed|Relay|Station|Depot|Repeatable) ", "", title)
    title = title.replace("Landing Return", "Landing and Return")
    return title


def description_for(m):
    description = (m.get("description") or m.get("beschreibung_en") or m.get("beschreibung") or "").strip()
    if any(kind == "RELAY_NETWORK_TOPOLOGY" for kind, _, _ in m["checks"]):
        description += (" Add one operational reserve satellite and phase the constellation so a single "
                        "failure does not break the network.")
    return description


def icon_for(m):
    kinds = [k for k, _, _ in m["checks"]]
    if "WHEEL_MOTION" in kinds:
        return "TrackingStation_ButtonMapRover"
    if "MARKER_LANDING" in kinds:
        return "TrackingStation_ButtonMapFlag"
    if ("RELAY_VESSEL_COUNT" in kinds or "RELAY_VESSEL_COUNT_INCLINATION" in kinds or
            "RELAY_NETWORK_TOPOLOGY" in kinds):
        return "TrackingStation_ButtonMapCommunicationsRelay"
    if "ORE_SURFACE" in kinds or "RESOURCE_MIN" in kinds:
        return "TrackingStation_ButtonMapBase"
    if "FUEL_MIN" in kinds or "RESOURCE_DELIVERY" in kinds:
        return "TrackingStation_ButtonMapBase"
    if "CREW_CAPACITY_MIN" in kinds:
        return "TrackingStation_ButtonMapStation"
    if any(kind in kinds for kind in ("MASS_MIN", "MODULE_COUNT", "POWER_CAPACITY_MIN", "DOCKING_PORT_COUNT")):
        return "TrackingStation_ButtonMapStation"
    if "DOCK_ANY" in kinds or "DOCK_STATION" in kinds:
        return "TrackingStation_ButtonMapStation"
    if "EVA" in kinds and "LANDED" not in kinds:
        return "TrackingStation_ButtonMapEVA"
    if "LANDED" in kinds:
        return "TrackingStation_ButtonMapBase" if "DURATION" in kinds else "TrackingStation_ButtonMapLander"
    if "FLYBY" in kinds:
        sparte = SPARTE.get(m["sparte"], m["sparte"])
        return "TrackingStation_ButtonMapShips" if sparte == "Bemannt" else "TrackingStation_ButtonMapProbe"
    if "VESSEL_COUNT" in kinds or "VESSEL_COUNT_INCLINATION" in kinds:
        return "TrackingStation_ButtonMapCommunicationsRelay"
    if "ATMO_FRACTION" in kinds or "SUBORBITAL" in kinds:
        return "TrackingStation_ButtonMapAircraft"
    if "ORBIT_ABOVE" in kinds:
        sparte = SPARTE.get(m["sparte"], m["sparte"])
        return "TrackingStation_ButtonMapShips" if sparte == "Bemannt" else "TrackingStation_ButtonMapCommunicationsRelay"
    return "TrackingStation_ButtonMapProbe"


def emit_checks(checks):
    out = ""
    for kind, kvl, label in checks:
        out += "            CHECK\n            {\n"
        out += f"                kind = {kind}\n"
        for k, v in kvl:
            out += f"                {k} = {v}\n"
        if label:
            out += f"                label = {label}\n"
        out += "            }\n"
    return out


def contract(m, epoch_names):
    prereqs = [] if m.get("prereq", "-") in ("-", "") else [p.strip() for p in m["prereq"].split(",")]
    sparte = SPARTE[m["sparte"]]
    sub = m.get("subcategory") or SUBCAT[m["body"]]
    epoch = epoch_for(m)
    _CONTRACT_META[m["id"]] = {
        "prerequisites": list(prereqs), "epoch": epoch, "branch": sparte,
        "sequence": len(_CONTRACT_META),
    }
    out = "    CONTRACT\n    {\n"
    out += f"        id = {m['id']}\n"
    out += f"        title = {title_for(m)}\n"
    out += f"        description = {description_for(m)}\n"
    out += f"        sparte = {sparte}\n"
    out += f"        subcategory = {sub}\n"
    out += f"        icon = {m.get('icon') or icon_for(m)}\n"
    out += f"        reward = {m['reward']}\n"
    out += f"        epoch = {epoch}\n"
    out += f"        epochName = {m.get('epochName') or epoch_names.get(epoch, DEFAULT_EPOCH_NAMES[epoch])}\n"
    if m.get("repeatable") == "yes":
        out += "        repeatable = true\n"
    if m.get("recordStation", "-") != "-":
        out += f"        recordStationKey = {m['recordStation']}\n"
    if m.get("stationRef", "-") != "-":
        out += f"        stationRef = {m['stationRef']}\n"
    for p in prereqs:
        out += f"        prerequisite = {p}\n"
    out += "        CONDITION\n        {\n"
    out += "            type = COMPOSITE\n"
    out += emit_checks(m["checks"])
    out += "        }\n"
    out += "    }\n"
    return out


_CONTRACT_META = {}


def build_recommended_route():
    branch_priority = {"UnbemannteErkundung": 0, "Bemannt": 1,
                       "NetzwerkLogistik": 2, "Stationen": 3}
    prerequisites = {mission_id: meta["prerequisites"]
                     for mission_id, meta in _CONTRACT_META.items()}
    return recommended_route_order(prerequisites, RECOMMENDED_MILESTONES,
        lambda mission_id: (_CONTRACT_META[mission_id]["epoch"],
                            branch_priority.get(_CONTRACT_META[mission_id]["branch"], 9),
                            _CONTRACT_META[mission_id]["sequence"], mission_id))


def mark_recommended_route(body, route_order):
    for mission_id, order in route_order.items():
        needle = f"        id = {mission_id}\n"
        if needle in body:
            body = body.replace(needle, needle +
                f"        recommended = true\n        recommendedOrder = {order}\n", 1)
    return body


def write_file(path, title, body):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(HEADER.format(title=title) + body + "}\n")


def write_readme(counts, epoch_names):
    readme = os.path.join(ROOT, "OptionalConfigs", "Stock", "README.md")
    os.makedirs(os.path.dirname(readme), exist_ok=True)
    chapter_lines = "\n".join(
        f"{i}. {epoch_names.get(i, DEFAULT_EPOCH_NAMES[i])}"
        for i in range(1, 10)
    )
    text = f"""# Stock KSP config

Optional config pack for the stock Kerbol system. It replaces only the four contract
catalog files in `GameData/CustomScienceContracts/Contracts/`; the plugin, icons and
licenses stay from the main download.

Install:
1. Install the main mod download from the same release first.
2. Copy this folder's `GameData` directory into your KSP install root.
3. Confirm overwriting the four catalog files in `GameData/CustomScienceContracts/Contracts/`.

Run only one config at a time. To go back to SOL, restore the four default catalog files
from the main download.

Requires CustomScienceContracts 0.8.0 from the same release. The 0.8 Stock catalog integrates
mandatory station engineering checks and 120-day later endurance stages; do not combine it with
an older DLL.

Campaign chapters:
{chapter_lines}

Generated mission counts:
- Pioneers: {counts.get('Bemannt', 0)}
- Robotic Explorers: {counts.get('UnbemannteErkundung', 0)}
- Stations: {counts.get('Stationen', 0)}
- Lifelines: {counts.get('NetzwerkLogistik', 0)}
- Total: {sum(counts.values())}

Station cores require minimum mass, docking ports and ElectricCharge capacity. Every station
expansion additionally requires a compatible stock- or Kerbalism-style laboratory. The Stock
overlay has no dependency on SOL, Contract Configurator, Toolbar Controller or Kerbalism.
"""
    with open(readme, "w", encoding="utf-8") as f:
        f.write(text)


def main():
    _CONTRACT_META.clear()
    text = open(DOC, encoding="utf-8").read()
    missions = parse_missions(text)
    epoch_names = parse_epoch_names(text)
    buckets = {name: [] for name in BUCKET_FILES}
    for m in missions:
        buckets[SPARTE[m["sparte"]]].append(contract(m, epoch_names))
    route_order = build_recommended_route()
    for name, (fn, title) in BUCKET_FILES.items():
        body = mark_recommended_route("".join(buckets[name]), route_order)
        if name == "Bemannt":
            body = epoch_nodes(epoch_names) + body
        write_file(os.path.join(OUT, fn), title, body)
    write_readme({k: len(v) for k, v in buckets.items()}, epoch_names)
    for name, items in buckets.items():
        print(f"{name}: {len(items)}")
    print(f"Recommended route: {len(route_order)} missions")
    print("Written to", OUT)


if __name__ == "__main__":
    main()
