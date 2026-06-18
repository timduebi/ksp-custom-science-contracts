#!/usr/bin/env python3
"""Generate the Stock KSP English contract catalog.

Both editions live on `main` and share one engine. The default GameData catalog is the
SOL English one; this generator writes the Stock catalog into OptionalConfigs/Stock, from
where tools/make_release.sh assembles the Stock edition download.
"""
import os
import re

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DOC = os.path.join(ROOT, "custom_science_contracts_stock_missionsdesign.md")
OUT = os.path.join(ROOT, "OptionalConfigs", "Stock", "GameData", "CustomScienceContracts", "Contracts")

SPARTE = {
    "Pioneers": "Bemannt",
    "Robotic Explorers": "UnbemannteErkundung",
    "Lifelines": "NetzwerkLogistik",
}

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

BUCKET_FILES = {
    "Pioneers": ("A_Pioniere.cfg", "BRANCH A - PIONEERS (crewed)"),
    "Robotic Explorers": ("B_Spaeher.cfg", "BRANCH B - ROBOTIC EXPLORERS"),
    "Lifelines": ("C_Lebensadern.cfg", "BRANCH C - LIFELINES"),
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


def fpct(p):
    v = float(p) / 100.0
    return (f"{v:.3f}").rstrip("0").rstrip(".")


def parse_check(s):
    head, _, label = s.partition("|")
    label = label.strip()
    toks = head.split()
    if not toks:
        raise SystemExit("Empty check line")
    kind, a = toks[0], toks[1:]
    kv = []
    if kind in ("CREW_MIN", "CREW_EXACT"):
        kv = [("min", a[0])]
    elif kind == "CREW_NONE":
        kv = []
    elif kind in ("SUBORBITAL", "LANDED", "ORE_SURFACE"):
        kv = [("body", a[0])]
    elif kind == "ORBIT_ABOVE":
        kv = [("body", a[0])] + ([("km", a[1])] if len(a) > 1 else [])
    elif kind == "INCLINATION_MIN":
        kv = [("body", a[0]), ("inclinationMin", a[1])]
    elif kind == "ATMO_FRACTION":
        kv = [("body", a[0]), ("fracMin", fpct(a[1])), ("fracMax", fpct(a[2]))]
    elif kind == "FLYBY":
        kv = [("body", a[0]), ("km", a[1])]
    elif kind == "MARKER_LANDING":
        kv = [("body", a[0]), ("km", a[1])] + ([("latMin", a[2]), ("latMax", a[3])] if len(a) > 3 else [])
    elif kind == "VESSEL_COUNT":
        kv = [("body", a[0]), ("count", a[1])] + ([("km", a[2])] if len(a) > 2 else [])
    elif kind == "VESSEL_COUNT_INCLINATION":
        kv = [("body", a[0]), ("count", a[1]), ("inclinationMin", a[2])] + ([("km", a[3])] if len(a) > 3 else [])
    elif kind == "EVA":
        kv = [("body", a[0])] + ([("situation", a[1])] if len(a) > 1 else [])
    elif kind == "FUEL_MIN":
        kv = [("amount", a[0])]
    elif kind == "RESOURCE_MIN":
        kv = [("resource", a[0]), ("amount", a[1])]
    elif kind == "DOCK_ANY":
        kv = []
    elif kind == "DOCK_STATION":
        kv = [("stationKey", a[0])] + ([("body", a[1])] if len(a) > 1 else [])
    elif kind == "HOLD":
        kv = [("seconds", a[0])]
    elif kind == "DURATION":
        kv = [("days", a[0])]
    elif kind == "RETURN_FROM_BODY":
        kv = [("body", a[0]), ("returnBody", a[1])]
    else:
        raise SystemExit(f"Unknown check '{kind}' in: {s}")
    return kind, kv, label


def parse_missions(text):
    missions = []
    keys = "id|title|sparte|body|subcategory|prereq|reward|repeatable|recordStation|stationRef|description|icon"
    for blk in text.split("=== MISSION ===")[1:]:
        m, checks = {}, []
        for line in blk.splitlines():
            s = line.strip()
            if not s:
                continue
            if s.startswith("=="):
                break
            if s.startswith("check:"):
                checks.append(parse_check(s[6:].strip()))
                continue
            mm = re.match(rf"({keys}):\s*(.*)$", s)
            if mm:
                m[mm.group(1)] = mm.group(2).strip()
        if "id" in m:
            m["checks"] = checks
            missions.append(m)
    return missions


def icon_for(m):
    kinds = [k for k, _, _ in m["checks"]]
    if "MARKER_LANDING" in kinds:
        return "TrackingStation_ButtonMapFlag"
    if "ORE_SURFACE" in kinds or "FUEL_MIN" in kinds or "RESOURCE_MIN" in kinds:
        return "TrackingStation_ButtonMapBase"
    if "DOCK_ANY" in kinds or "DOCK_STATION" in kinds:
        return "TrackingStation_ButtonMapStation"
    if "EVA" in kinds and "LANDED" not in kinds:
        return "TrackingStation_ButtonMapEVA"
    if "RETURN_FROM_BODY" in kinds:
        return "TrackingStation_ButtonMapShips"
    if "LANDED" in kinds:
        return "TrackingStation_ButtonMapBase" if "DURATION" in kinds else "TrackingStation_ButtonMapLander"
    if "FLYBY" in kinds:
        return "TrackingStation_ButtonMapProbe"
    if "VESSEL_COUNT" in kinds or "VESSEL_COUNT_INCLINATION" in kinds:
        return "TrackingStation_ButtonMapCommunicationsRelay"
    if "ATMO_FRACTION" in kinds or "SUBORBITAL" in kinds:
        return "TrackingStation_ButtonMapAircraft"
    if "ORBIT_ABOVE" in kinds:
        return "TrackingStation_ButtonMapShips" if m["sparte"] == "Pioneers" else "TrackingStation_ButtonMapCommunicationsRelay"
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


def contract(m):
    prereqs = [] if m.get("prereq", "-") in ("-", "") else [p.strip() for p in m["prereq"].split(",")]
    sparte = SPARTE[m["sparte"]]
    sub = m.get("subcategory") or SUBCAT[m["body"]]
    out = "    CONTRACT\n    {\n"
    out += f"        id = {m['id']}\n"
    out += f"        title = {m.get('title', m['id'])}\n"
    out += f"        description = {m.get('description', '')}\n"
    out += f"        sparte = {sparte}\n"
    out += f"        subcategory = {sub}\n"
    out += f"        icon = {m.get('icon') or icon_for(m)}\n"
    out += f"        reward = {m['reward']}\n"
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


def write_file(path, title, body):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(HEADER.format(title=title) + body + "}\n")


def write_readme(counts):
    readme = os.path.join(ROOT, "OptionalConfigs", "Stock", "README.md")
    os.makedirs(os.path.dirname(readme), exist_ok=True)
    text = """# Custom Science Contracts - Stock KSP campaign (English)

A custom science-mode contract campaign for **stock** Kerbal Space Program. This is the
Stock edition main download and is English only. (The real-solar-system **SOL** edition
is released separately.)

Install by copying the `GameData` folder from this download into your KSP install root,
merging it into your existing `GameData/`. You should end up with
`Kerbal Space Program/GameData/CustomScienceContracts/` containing `Plugins/`,
`Contracts/`, `Icons/` and `settings.cfg`.

Campaign shape:
- Crewed-first Kerbin program, then Mun and Minmus.
- Focused robotic support for Mun, Duna, Jool and Eeloo.
- Communication networks for Kerbin, Mun/Minmus, Duna, Jool and Eeloo.
- Optional grand-tour style crewed landings across the Stock system.
- Finale: Laythe landing and return, followed by Eve landing and return.
"""
    with open(readme, "w", encoding="utf-8") as f:
        f.write(text)


def main():
    missions = parse_missions(open(DOC, encoding="utf-8").read())
    buckets = {name: [] for name in BUCKET_FILES}
    for m in missions:
        buckets[m["sparte"]].append(contract(m))
    for name, (fn, title) in BUCKET_FILES.items():
        write_file(os.path.join(OUT, fn), title, "".join(buckets[name]))
    write_file(os.path.join(OUT, "D_Stationen.cfg"), "STATION CHAINS - not used by the Stock English pack", "")
    write_readme({k: len(v) for k, v in buckets.items()})
    for name, items in buckets.items():
        print(f"{name}: {len(items)}")
    print("Written to", OUT)


if __name__ == "__main__":
    main()
