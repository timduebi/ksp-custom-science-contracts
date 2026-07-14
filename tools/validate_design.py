#!/usr/bin/env python3
"""Validate custom_science_contracts_missionsdesign.md against engine capabilities.

Checks duplicate ids, dangling prerequisites including generated station-chain ids, unknown
check types, unknown bodies and invalid branches. Read-only.
"""
import re, os, sys

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DOC = sys.argv[1] if len(sys.argv) > 1 else os.path.join(ROOT, "custom_science_contracts_missionsdesign.md")
if not os.path.isabs(DOC):
    DOC = os.path.join(ROOT, DOC)
PROFILE = (sys.argv[2].lower() if len(sys.argv) > 2 else ("stock" if "stock" in DOC.lower() else "sol"))
text = open(DOC, encoding="utf-8").read()

SOL_BODIES = {  # known internal body names from SOL
 "Amalthea","Ariel","Arrokoth","Callisto","Ceres","Charon","Dactyl","Deimos","Dione",
 "Earth","Enceladus","Eros","Europa","Ganymede","Hydra","Hyperion","Iapetus","Ida","Io",
 "Jupiter","Kerberos","Mars","Mercury","Mimas","Miranda","Moon","Neptune","Nereid","Nix",
 "Oberon","Pallas","Phobos","Phoebe","Pluto","Proteus","Psyche","Puck","Rhea","Ryugu",
 "Saturn","Styx","Sun","Tethys","Thebe","Titan","Titania","Triton","Umbriel","Uranus",
 "Venus","Vesta"}
STOCK_BODIES = {
 "Sun","Kerbin","Mun","Minmus","Moho","Eve","Gilly","Duna","Ike","Dres","Jool",
 "Laythe","Vall","Tylo","Bop","Pol","Eeloo"}
BODIES = STOCK_BODIES if PROFILE == "stock" else SOL_BODIES

# Check token in the design plan -> whether the first argument is a body.
CHECK_BODY = {"ATMO_FRACTION","SUBORBITAL","ORBIT_ABOVE","APOAPSIS_MAX","INCLINATION_MIN","VESSEL_COUNT","VESSEL_COUNT_INCLINATION",
              "RELAY_VESSEL_COUNT","RELAY_VESSEL_COUNT_INCLINATION","EVA","FLYBY","LANDED","MARKER_LANDING",
              "ORE_SURFACE","RETURN_FROM_BODY","WHEEL_MOTION","RELAY_NETWORK_TOPOLOGY"}
CHECK_NOBODY = {"CREW_NONE","CREW_MIN","CREW_EXACT","CREW_CAPACITY_MIN","HOLD","DURATION","DOCK_ANY",
                "DOCK_STATION","FUEL_MIN","RESOURCE_MIN","RESOURCE_DELIVERY","MASS_MIN","MODULE_COUNT",
                "POWER_CAPACITY_MIN","DOCKING_PORT_COUNT"}
KNOWN_CHECKS = CHECK_BODY | CHECK_NOBODY
SPARTEN = {"Pioniere","Robotische Erkunder","Versorgungsnetz","Stationen",
           "Pioneers","Robotic Explorers","Lifelines","Stations"}

# ---- Parse missions ----
missions = []
for blk in text.split("=== MISSION ===")[1:]:
    m, checks = {}, []
    for line in blk.splitlines():
        s = line.strip()
        if not s: continue
        if s.startswith("=="): break
        if s.startswith("check:"):
            checks.append(s[len("check:"):].strip()); continue
        mm = re.match(r"(id|title|sparte|body|epoche|epoch|epochName|prereq|reward|repeatable|recordStation|stationRef|beschreibung_en|beschreibung|description|icon):\s*(.*)$", s)
        if mm: m[mm.group(1)] = mm.group(2).strip()
    if "id" in m:
        m["checks"] = checks
        missions.append(m)

# ---- Parse station chains and derive generated ids ----
chains = []
for line in text.splitlines():
    if line.strip().startswith("chain:"):
        d = {}
        for part in line.split("chain:")[1].split("|"):
            if "=" in part:
                k, v = part.split("=", 1); d[k.strip()] = v.strip()
        chains.append(d)

gen_ids, gen_ref_targets = set(), set()
support_ids = set()
if PROFILE != "stock":
    support_ids = {
        "cr_earth_docking_target",
        "cr_luna_station_precision_landing_1",
        "cr_luna_station_precision_landing_2",
    }
    for ch in chains:
        key = ch["key"]; stufen = [int(x) for x in ch["stufen"].split(",")]
        for i, n in enumerate(stufen):
            gen_ids.add(f"cr_{key}_build" if i == 0 else f"cr_{key}_expand{n}")
            gen_ids.add(f"cr_{key}_supply{n}")
            gen_ids.add(f"cr_{key}_longstay{n}")

# ---- Collect ids ----
defined = set()
dups = []
for m in missions:
    if m["id"] in defined: dups.append(m["id"])
    defined.add(m["id"])
all_ids = defined | gen_ids | support_ids

# Prerequisites of all missions and chain starts.
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

# Check types + bodies.
unknown_checks, unknown_bodies, used_checks, used_bodies = set(), set(), {}, set()
for m in missions:
    for c in m["checks"]:
        toks = c.split("|")[0].split()
        kind = toks[0]
        used_checks[kind] = used_checks.get(kind, 0) + 1
        if kind not in KNOWN_CHECKS: unknown_checks.add(kind)
        if kind in CHECK_BODY and len(toks) >= 2:
            used_bodies.add(toks[1])
            if toks[1] not in BODIES: unknown_bodies.add(toks[1])
        if kind == "WHEEL_MOTION" and len(toks) < 3:
            unknown_checks.add(f"{kind}:missing_body_or_speed")
        if kind == "RETURN_FROM_BODY" and len(toks) >= 3:
            used_bodies.add(toks[2])
            if toks[2] not in BODIES: unknown_bodies.add(toks[2])
    b = m.get("body")
    if b and b not in BODIES: unknown_bodies.add(b)

# Branches.
bad_sparte = sorted({m.get("sparte","?") for m in missions if m.get("sparte") not in SPARTEN})
by_sparte = {}
for m in missions: by_sparte[m.get("sparte","?")] = by_sparte.get(m.get("sparte","?"),0)+1

# recordStation/stationRef consistency.
record_keys = {m["recordStation"] for m in missions if m.get("recordStation","-") != "-"}
ref_keys    = {m["stationRef"]    for m in missions if m.get("stationRef","-") != "-"}

# Every SOL mission must carry a hand-written English translation (beschreibung_en).
# The stock design doc uses its own English `description` field instead.
missing_en = sorted(m["id"] for m in missions if not m.get("beschreibung_en","").strip()) if PROFILE != "stock" else []
missing_stock_title = sorted(m["id"] for m in missions if PROFILE == "stock" and not m.get("title","").strip())
missing_stock_epoch = sorted(m["id"] for m in missions if PROFILE == "stock" and not (m.get("epoche") or m.get("epoch") or "").strip())
bad_stock_epoch = []
for m in missions:
    if PROFILE != "stock":
        continue
    value = (m.get("epoche") or m.get("epoch") or "").strip()
    if not value:
        continue
    try:
        epoch = int(value)
    except ValueError:
        bad_stock_epoch.append(f"{m['id']}:{value}")
        continue
    if epoch < 1 or epoch > 9:
        bad_stock_epoch.append(f"{m['id']}:{value}")

# ---- Report ----
print("Design file:", DOC)
print("Profile:", PROFILE)
print(f"Missions: {len(missions)}   (station chains generate {len(gen_ids)} additional ids)")
print("Generated support ids:", sorted(support_ids))
print("Per branch:", by_sparte)
print("Chains:", [c['key'] for c in chains])
print()
print("Distinct check types:", sorted(used_checks.keys()))
print("Check frequency:", dict(sorted(used_checks.items(), key=lambda kv:-kv[1])))
print()
print(f"Distinct bodies used: {len(used_bodies)}")
print()
def show(title, items):
    print(f"[{'FAIL' if items else 'OK'}] {title}: {sorted(items) if items else '—'}")
show("Duplicate ids", dups)
show("Dangling prerequisites", dangling)
show("Unknown check types", unknown_checks)
show("Unknown/invented bodies", unknown_bodies)
show("Invalid branches", bad_sparte)
show("Missions without beschreibung_en", missing_en)
show("Stock missions without title", missing_stock_title)
show("Stock missions without epoche", missing_stock_epoch)
show("Invalid Stock epoche values", bad_stock_epoch)
print()
print("recordStation-Keys:", sorted(record_keys) or "—")
print("stationRef-Keys:", sorted(ref_keys) or "—")
print("Referenced generated station ids:",
      sorted({p for _,p in prereq_edges if p in gen_ids}))

ok = not (dups or dangling or unknown_checks or unknown_bodies or bad_sparte or missing_en or
          missing_stock_title or missing_stock_epoch or bad_stock_epoch)
print("\n==> " + ("VALIDATION OK" if ok else "VALIDATION HAS FINDINGS"))
sys.exit(0 if ok else 1)
