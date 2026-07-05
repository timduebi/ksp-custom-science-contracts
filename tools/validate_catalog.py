#!/usr/bin/env python3
"""Validate generated cfg catalog files as one engine-facing mission catalog.

Default target:
  GameData/CustomScienceContracts/Contracts

Optional target / profile:
  python3 tools/validate_catalog.py OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts
  python3 tools/validate_catalog.py GameData/CustomScienceContracts/Contracts stock
"""
import re, os, sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CDIR = sys.argv[1] if len(sys.argv) > 1 else os.path.join(ROOT, "GameData", "CustomScienceContracts", "Contracts")
if not os.path.isabs(CDIR):
    CDIR = os.path.join(ROOT, CDIR)
PROFILE = (sys.argv[2].lower() if len(sys.argv) > 2 else ("stock" if "stock" in CDIR.lower() else "sol"))
FILES = ["A_Pioniere.cfg", "B_Spaeher.cfg", "C_Lebensadern.cfg", "D_Stationen.cfg"]

CHECK_KINDS = {"CREW_MIN","CREW_NONE","CREW_EXACT","CREW_CAPACITY_MIN","ON_BODY","SITUATION","SUBORBITAL","LANDED",
  "PERIAPSIS_MIN","ORBIT_ABOVE","APOAPSIS_MAX","INCLINATION_MIN","ABOVE_ATMOSPHERE","SUBORBITAL_ABOVE_ATMO","ATMO_FRACTION",
  "ORE_PRESENT","ORE_SURFACE","FUEL_MIN","RESOURCE_MIN","WHEEL_MOTION","EVA","DOCK_STATION","DOCK_ANY",
  "VESSEL_COUNT","VESSEL_COUNT_INCLINATION","RELAY_VESSEL_COUNT","RELAY_VESSEL_COUNT_INCLINATION",
  "FLYBY","MARKER_LANDING","RETURN_FROM_BODY","HOLD","DURATION"}
SPARTEN = {"Bemannt","UnbemannteErkundung","Stationen","NetzwerkLogistik"}
SOL_BODIES = {"Amalthea","Ariel","Arrokoth","Callisto","Ceres","Charon","Dactyl","Deimos","Dione",
  "Earth","Enceladus","Eros","Europa","Ganymede","Hydra","Hyperion","Iapetus","Ida","Io","Jupiter",
  "Kerberos","Mars","Mercury","Mimas","Miranda","Moon","Neptune","Nereid","Nix","Oberon","Pallas",
  "Phobos","Phoebe","Pluto","Proteus","Psyche","Puck","Rhea","Ryugu","Saturn","Styx","Sun","Tethys",
  "Thebe","Titan","Titania","Triton","Umbriel","Uranus","Venus","Vesta"}
STOCK_BODIES = {"Sun","Kerbin","Mun","Minmus","Moho","Eve","Gilly","Duna","Ike","Dres","Jool",
  "Laythe","Vall","Tylo","Bop","Pol","Eeloo"}
BODIES = STOCK_BODIES if PROFILE == "stock" else SOL_BODIES
ICONS = {  # in GameData/.../Icons/UI vorhanden
  "TrackingStation_ButtonMapProbe","TrackingStation_ButtonMapLander","TrackingStation_ButtonMapFlag",
  "TrackingStation_ButtonMapEVA","TrackingStation_ButtonMapShips","TrackingStation_ButtonMapStation",
  "TrackingStation_ButtonMapBase","TrackingStation_ButtonMapCommunicationsRelay",
  "TrackingStation_ButtonMapAircraft","TrackingStation_ButtonMapRover",
  "icon_Aircraft","icon_EVA","icon_base","icon_flag","icon_lander","icon_probe","icon_relay",
  "icon_rover","icon_ships","icon_station"}

def blocks(text, name):
    """Return all brace-aware '{name} { ... }' blocks. The node name must be on its own line so
    CONTRACT does not accidentally match CUSTOM_CONTRACT_CATALOG."""
    out = []
    for mt in re.finditer(r'(?m)^[ \t]*' + re.escape(name) + r'[ \t]*$', text):
        b = text.find("{", mt.end())
        if b == -1: continue
        depth, j = 0, b
        while j < len(text):
            if text[j] == "{": depth += 1
            elif text[j] == "}":
                depth -= 1
                if depth == 0: break
            j += 1
        out.append(text[b + 1:j])
    return out

def vals(block, key):
    return re.findall(rf"^\s*{key}\s*=\s*(.+?)\s*$", block, re.M)
def val(block, key):
    v = vals(block, key); return v[0] if v else None

contracts, dups, issues = {}, [], []
all_ids, record_keys, ref_keys, dock_keys = set(), set(), set(), set()
per_file = {}

for fn in FILES:
    path = os.path.join(CDIR, fn)
    if not os.path.exists(path):
        issues.append(f"missing file: {path}")
        continue
    text = open(path, encoding="utf-8").read()
    cs = blocks(text, "CONTRACT")
    per_file[fn] = len(cs)
    for c in cs:
        cid = val(c, "id")
        if not cid: issues.append(f"{fn}: CONTRACT ohne id"); continue
        if cid in all_ids: dups.append(cid)
        all_ids.add(cid); contracts[cid] = (fn, c)

for cid, (fn, c) in contracts.items():
    sp = val(c, "sparte")
    if sp not in SPARTEN: issues.append(f"{cid}: branch '{sp}'")
    if not val(c, "title"): issues.append(f"{cid}: missing title")
    if not val(c, "description"): issues.append(f"{cid}: missing description")
    ep = val(c, "epoch")
    if ep and (not ep.isdigit() or not 1 <= int(ep) <= 9):
        issues.append(f"{cid}: invalid epoch '{ep}'")
    ic = val(c, "icon")
    if ic and ic not in ICONS: issues.append(f"{cid}: unknown icon '{ic}'")
    if val(c, "recordStationKey"): record_keys.add(val(c, "recordStationKey"))
    if val(c, "stationRef"): ref_keys.add(val(c, "stationRef"))
    for chk in blocks(c, "CHECK"):
        k = val(chk, "kind")
        if k not in CHECK_KINDS: issues.append(f"{cid}: Check-Kind '{k}'")
        b = val(chk, "body")
        if b and b not in BODIES: issues.append(f"{cid}: body '{b}'")
        rb = val(chk, "returnBody")
        if rb and rb not in BODIES: issues.append(f"{cid}: returnBody '{rb}'")
        if k == "DOCK_STATION":
            sk = val(chk, "stationKey")
            if sk: dock_keys.add(sk)
            else: issues.append(f"{cid}: DOCK_STATION without stationKey")
    # COMPOSITE needs at least one CHECK.
    if "type = COMPOSITE" in c and not blocks(c, "CHECK"):
        issues.append(f"{cid}: COMPOSITE without CHECK")

# Prerequisites + revealAllAfter must resolve.
dangling, bad_reveal = set(), set()
for cid, (fn, c) in contracts.items():
    for p in vals(c, "prerequisite"):
        if p not in all_ids: dangling.add(f"{cid} -> {p}")
    r = val(c, "revealAllAfter")
    if r and r not in all_ids: bad_reveal.add(f"{cid} -> {r}")

# Station keys: every referenced stationRef/DOCK_STATION must be produced by one recordStationKey.
unrecorded = (ref_keys | dock_keys) - record_keys

def show(t, items):
    bad = bool(items)
    print(f"[{'FAIL' if bad else 'OK'}] {t}: {sorted(items) if bad else '—'}")
    return bad

print("Catalog directory:", CDIR)
print("Contracts total:", len(all_ids), " per file:", per_file)
print("recordStationKeys:", sorted(record_keys))
print()
fail = False
fail |= show("Duplicate IDs", dups)
fail |= show("Dangling prerequisites", dangling)
fail |= show("Invalid revealAllAfter", bad_reveal)
fail |= show("Station keys without recordStationKey", unrecorded)
fail |= show("Other findings", issues)
print("\n==> " + ("CATALOG OK" if not fail else "CATALOG HAS FINDINGS"))
sys.exit(1 if fail else 0)
