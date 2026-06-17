#!/usr/bin/env python3
"""Validiert custom_science_contracts_missionsdesign.md gegen die Engine-Faehigkeiten:
Duplikate, haengende Voraussetzungen (inkl. generierter Stationsketten-IDs),
unbekannte Check-Typen, unbekannte Bodies, Sparten. Nur Lesen, keine Aenderung."""
import re, os, sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DOC = os.path.join(ROOT, "custom_science_contracts_missionsdesign.md")
text = open(DOC, encoding="utf-8").read()

SOL_BODIES = {  # bekannte interne Body-Namen aus SOL
 "Amalthea","Ariel","Arrokoth","Callisto","Ceres","Charon","Dactyl","Deimos","Dione",
 "Earth","Enceladus","Eros","Europa","Ganymede","Hydra","Hyperion","Iapetus","Ida","Io",
 "Jupiter","Kerberos","Mars","Mercury","Mimas","Miranda","Moon","Neptune","Nereid","Nix",
 "Oberon","Pallas","Phobos","Phoebe","Pluto","Proteus","Psyche","Puck","Rhea","Ryugu",
 "Saturn","Styx","Sun","Tethys","Thebe","Titan","Titania","Triton","Umbriel","Uranus",
 "Venus","Vesta"}

# Check-Token im Designplan -> hat es einen Body als 1. Argument?
CHECK_BODY = {"ATMO_FRACTION","SUBORBITAL","ORBIT_ABOVE","INCLINATION_MIN","VESSEL_COUNT","VESSEL_COUNT_INCLINATION",
              "EVA","FLYBY","LANDED","MARKER_LANDING","ORE_SURFACE"}
CHECK_NOBODY = {"CREW_NONE","CREW_MIN","CREW_EXACT","HOLD","DURATION","DOCK_ANY",
                "DOCK_STATION","FUEL_MIN","RESOURCE_MIN"}
KNOWN_CHECKS = CHECK_BODY | CHECK_NOBODY
SPARTEN = {"Pioniere","Robotische Erkunder","Versorgungsnetz"}

# ---- Missionen parsen ----
missions = []
for blk in text.split("=== MISSION ===")[1:]:
    m, checks = {}, []
    for line in blk.splitlines():
        s = line.strip()
        if not s: continue
        if s.startswith("=="): break
        if s.startswith("check:"):
            checks.append(s[len("check:"):].strip()); continue
        mm = re.match(r"(id|sparte|body|prereq|reward|repeatable|recordStation|stationRef|beschreibung|icon):\s*(.*)$", s)
        if mm: m[mm.group(1)] = mm.group(2).strip()
    if "id" in m:
        m["checks"] = checks
        missions.append(m)

# ---- Stationsketten parsen + generierte IDs ableiten ----
chains = []
for line in text.splitlines():
    if line.strip().startswith("chain:"):
        d = {}
        for part in line.split("chain:")[1].split("|"):
            if "=" in part:
                k, v = part.split("=", 1); d[k.strip()] = v.strip()
        chains.append(d)

gen_ids, gen_ref_targets = set(), set()
for ch in chains:
    key = ch["key"]; stufen = [int(x) for x in ch["stufen"].split(",")]
    for i, n in enumerate(stufen):
        gen_ids.add(f"cr_{key}_build" if i == 0 else f"cr_{key}_expand{n}")
        gen_ids.add(f"cr_{key}_supply{n}")
        gen_ids.add(f"cr_{key}_longstay{n}")

# ---- Sammeln ----
defined = set()
dups = []
for m in missions:
    if m["id"] in defined: dups.append(m["id"])
    defined.add(m["id"])
all_ids = defined | gen_ids

# Voraussetzungen aller Missionen + Kettenstart
prereq_edges = []
for m in missions:
    pr = m.get("prereq", "-")
    if pr and pr != "-":
        for p in [x.strip() for x in pr.split(",")]:
            prereq_edges.append((m["id"], p))
for ch in chains:
    pr = ch.get("prereq", "-")
    if pr and pr != "-":
        for p in [x.strip() for x in pr.split(",")]:
            prereq_edges.append((f"chain:{ch['key']}", p))

dangling = sorted({p for _, p in prereq_edges if p not in all_ids})

# Check-Typen + Bodies
unknown_checks, unknown_bodies, used_checks, used_bodies = set(), set(), {}, set()
for m in missions:
    for c in m["checks"]:
        toks = c.split("|")[0].split()
        kind = toks[0]
        used_checks[kind] = used_checks.get(kind, 0) + 1
        if kind not in KNOWN_CHECKS: unknown_checks.add(kind)
        if kind in CHECK_BODY and len(toks) >= 2:
            used_bodies.add(toks[1])
            if toks[1] not in SOL_BODIES: unknown_bodies.add(toks[1])
    b = m.get("body")
    if b and b not in SOL_BODIES: unknown_bodies.add(b)

# Sparten
bad_sparte = sorted({m.get("sparte","?") for m in missions if m.get("sparte") not in SPARTEN})
by_sparte = {}
for m in missions: by_sparte[m.get("sparte","?")] = by_sparte.get(m.get("sparte","?"),0)+1

# recordStation/stationRef Konsistenz
record_keys = {m["recordStation"] for m in missions if m.get("recordStation","-") != "-"}
ref_keys    = {m["stationRef"]    for m in missions if m.get("stationRef","-") != "-"}

# ---- Report ----
print(f"Missionen: {len(missions)}   (Stationsketten generieren zusaetzlich {len(gen_ids)} IDs)")
print("Pro Sparte:", by_sparte)
print("Ketten:", [c['key'] for c in chains])
print()
print("Distinct Check-Typen:", sorted(used_checks.keys()))
print("Check-Haeufigkeit:", dict(sorted(used_checks.items(), key=lambda kv:-kv[1])))
print()
print(f"Distinct Bodies benutzt: {len(used_bodies)}")
print()
def show(title, items):
    print(f"[{'FAIL' if items else 'OK'}] {title}: {sorted(items) if items else '—'}")
show("Doppelte IDs", dups)
show("Haengende Voraussetzungen", dangling)
show("Unbekannte Check-Typen", unknown_checks)
show("Unbekannte/erfundene Bodies", unknown_bodies)
show("Ungueltige Sparten", bad_sparte)
print()
print("recordStation-Keys:", sorted(record_keys) or "—")
print("stationRef-Keys:", sorted(ref_keys) or "—")
print("Referenzierte generierte Stations-IDs:",
      sorted({p for _,p in prereq_edges if p in gen_ids}))

ok = not (dups or dangling or unknown_checks or unknown_bodies or bad_sparte)
print("\n==> " + ("VALIDIERUNG OK" if ok else "VALIDIERUNG MIT BEFUNDEN"))
sys.exit(0 if ok else 1)
