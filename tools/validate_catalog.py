#!/usr/bin/env python3
"""Validiert die GENERIERTEN cfg-Dateien (A/B/C/D) als Gesamtkatalog gegen die Engine:
Duplikate, haengende Voraussetzungen, revealAllAfter-Ziele, Check-Kinds (gegen CheckKind-Enum),
Sparten, Bodies, Stations-Schluessel (recordStationKey vs. DOCK_STATION/stationRef)."""
import re, os, sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
CDIR = os.path.join(ROOT, "GameData", "CustomScienceContracts", "Contracts")
FILES = ["A_Pioniere.cfg", "B_Spaeher.cfg", "C_Lebensadern.cfg", "D_Stationen.cfg"]

CHECK_KINDS = {"CREW_MIN","CREW_NONE","CREW_EXACT","ON_BODY","SITUATION","SUBORBITAL","LANDED",
  "PERIAPSIS_MIN","ORBIT_ABOVE","INCLINATION_MIN","ABOVE_ATMOSPHERE","SUBORBITAL_ABOVE_ATMO","ATMO_FRACTION",
  "ORE_PRESENT","ORE_SURFACE","FUEL_MIN","RESOURCE_MIN","EVA","DOCK_STATION","DOCK_ANY",
  "VESSEL_COUNT","VESSEL_COUNT_INCLINATION","FLYBY","MARKER_LANDING","HOLD","DURATION"}
SPARTEN = {"Bemannt","UnbemannteErkundung","NetzwerkLogistik"}
SOL_BODIES = {"Amalthea","Ariel","Arrokoth","Callisto","Ceres","Charon","Dactyl","Deimos","Dione",
  "Earth","Enceladus","Eros","Europa","Ganymede","Hydra","Hyperion","Iapetus","Ida","Io","Jupiter",
  "Kerberos","Mars","Mercury","Mimas","Miranda","Moon","Neptune","Nereid","Nix","Oberon","Pallas",
  "Phobos","Phoebe","Pluto","Proteus","Psyche","Puck","Rhea","Ryugu","Saturn","Styx","Sun","Tethys",
  "Thebe","Titan","Titania","Triton","Umbriel","Uranus","Venus","Vesta"}
ICONS = {  # in GameData/.../Icons/UI vorhanden
  "TrackingStation_ButtonMapProbe","TrackingStation_ButtonMapLander","TrackingStation_ButtonMapFlag",
  "TrackingStation_ButtonMapEVA","TrackingStation_ButtonMapShips","TrackingStation_ButtonMapStation",
  "TrackingStation_ButtonMapBase","TrackingStation_ButtonMapCommunicationsRelay",
  "TrackingStation_ButtonMapAircraft","TrackingStation_ButtonMapRover"}

def blocks(text, name):
    """Alle '{name} { ... }'-Bloecke (brace-aware), liefert inneren Text. Der Knotenname muss eine
    eigene Zeile sein — sonst matcht 'CONTRACT' faelschlich in 'CUSTOM_CONTRACT_CATALOG'."""
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
    text = open(os.path.join(CDIR, fn), encoding="utf-8").read()
    cs = blocks(text, "CONTRACT")
    per_file[fn] = len(cs)
    for c in cs:
        cid = val(c, "id")
        if not cid: issues.append(f"{fn}: CONTRACT ohne id"); continue
        if cid in all_ids: dups.append(cid)
        all_ids.add(cid); contracts[cid] = (fn, c)

for cid, (fn, c) in contracts.items():
    sp = val(c, "sparte")
    if sp not in SPARTEN: issues.append(f"{cid}: Sparte '{sp}'")
    if not val(c, "title"): issues.append(f"{cid}: kein title")
    if not val(c, "description"): issues.append(f"{cid}: keine description")
    ic = val(c, "icon")
    if ic and ic not in ICONS: issues.append(f"{cid}: unbekanntes icon '{ic}'")
    if val(c, "recordStationKey"): record_keys.add(val(c, "recordStationKey"))
    if val(c, "stationRef"): ref_keys.add(val(c, "stationRef"))
    for chk in blocks(c, "CHECK"):
        k = val(chk, "kind")
        if k not in CHECK_KINDS: issues.append(f"{cid}: Check-Kind '{k}'")
        b = val(chk, "body")
        if b and b not in SOL_BODIES: issues.append(f"{cid}: Body '{b}'")
        if k == "DOCK_STATION":
            sk = val(chk, "stationKey")
            if sk: dock_keys.add(sk)
            else: issues.append(f"{cid}: DOCK_STATION ohne stationKey")
    # COMPOSITE muss mind. 1 CHECK haben
    if "type = COMPOSITE" in c and not blocks(c, "CHECK"):
        issues.append(f"{cid}: COMPOSITE ohne CHECK")

# Voraussetzungen + revealAllAfter aufloesbar?
dangling, bad_reveal = set(), set()
for cid, (fn, c) in contracts.items():
    for p in vals(c, "prerequisite"):
        if p not in all_ids: dangling.add(f"{cid} -> {p}")
    r = val(c, "revealAllAfter")
    if r and r not in all_ids: bad_reveal.add(f"{cid} -> {r}")

# Stationsschluessel: jeder referenzierte (stationRef/DOCK_STATION) muss von genau einem recordStationKey erzeugt werden
unrecorded = (ref_keys | dock_keys) - record_keys

def show(t, items):
    bad = bool(items)
    print(f"[{'FAIL' if bad else 'OK'}] {t}: {sorted(items) if bad else '—'}")
    return bad

print("Contracts gesamt:", len(all_ids), " pro Datei:", per_file)
print("recordStationKeys:", sorted(record_keys))
print()
fail = False
fail |= show("Doppelte IDs", dups)
fail |= show("Haengende Voraussetzungen", dangling)
fail |= show("Ungueltige revealAllAfter", bad_reveal)
fail |= show("Stationsschluessel ohne recordStationKey", unrecorded)
fail |= show("Sonstige Befunde", issues)
print("\n==> " + ("KATALOG OK" if not fail else "KATALOG MIT BEFUNDEN"))
sys.exit(1 if fail else 0)
