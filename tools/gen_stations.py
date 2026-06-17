#!/usr/bin/env python3
# Generiert die regulaeren Stations-/Basis-Ketten (gleiches Muster, getemplatet) nach
# GameData/.../Contracts/D_Stationen.cfg und entfernt die alten Ketten-Contracts aus A_Pioniere.cfg.
import re, os

ROOT = os.path.join(os.path.dirname(__file__), "..", "GameData", "CustomScienceContracts", "Contracts")
A = os.path.join(ROOT, "A_Pioniere.cfg")
D = os.path.join(ROOT, "D_Stationen.cfg")

def kerbals(n): return "1 Kerbal" if n == 1 else f"{n} Kerbals"

def emit_checks(checks):
    s = ""
    for c in checks:
        s += "        CHECK\n        {\n"
        for k, v in c.items():
            s += f"            {k} = {v}\n"
        s += "        }\n"
    return s

def contract(cid, title, desc, sub, reward, prereqs, checks, repeatable=False,
             record=None, ref=None):
    s = "    CONTRACT\n    {\n"
    s += f"        id = {cid}\n"
    s += f"        title = {title}\n"
    s += f"        description = {desc}\n"
    s += "        sparte = Bemannt\n"
    s += f"        subcategory = {sub}\n"
    s += f"        reward = {reward}\n"
    if repeatable: s += "        repeatable = true\n"
    if record: s += f"        recordStationKey = {record}\n"
    if ref: s += f"        stationRef = {ref}\n"
    for p in prereqs: s += f"        prerequisite = {p}\n"
    s += "        CONDITION\n        {\n            type = COMPOSITE\n"
    s += emit_checks(checks)
    s += "        }\n    }\n"
    return s

def orbit_chain(prefix, key, body, sub, orbitword, km, stages, build_id, prereq0,
                station_word, mult, header):
    out = [header]
    prev_long = None
    for i, n in enumerate(stages):
        is_build = (i == 0)
        stage_id = build_id if is_build else f"{prefix}_expand{n}"
        sup_id = f"{prefix}_supply{n}"
        long_id = f"{prefix}_longstay{n}"
        crew_lbl = f"Bemannt mit mindestens {kerbals(n)} an Bord"
        orbit_chk = {"kind": "ORBIT_ABOVE", "body": body, "km": km,
                     "label": f"Stabiler {orbitword}, Periapsis über {km} km"}
        # Ausbau / Bau
        if is_build:
            core = station_word[6:] if station_word.startswith("Erste ") else station_word
            title = f"{station_word} ({kerbals(n)})"
            desc = (f"Errichte deine {core} und halte sie 10 Tage am Stück mit {kerbals(n)} "
                    f"besetzt. Der Name, den du ihr beim Bau gibst, wird gespeichert — alle Versorgungs- "
                    f"und Ausbauflüge beziehen sich auf genau diese Station.")
            out.append(contract(stage_id, title, desc, sub, round(220*mult), [prereq0],
                       [{"kind": "CREW_MIN", "min": n, "label": crew_lbl}, orbit_chk,
                        {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} stabil halten"}],
                       record=key))
            stage_prereq = stage_id
        else:
            title = f"Stationsausbau auf {kerbals(n)}"
            desc = (f"Erweitere %station% auf {n} ständige Besatzungsmitglieder und halte den Betrieb "
                    f"10 Tage stabil auf der neuen Stufe, ehe der nächste Schritt ansteht.")
            out.append(contract(stage_id, title, desc, sub, round((180+20*n)*mult), [prev_long],
                       [{"kind": "CREW_MIN", "min": n, "label": crew_lbl}, orbit_chk,
                        {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} stabil halten"}],
                       ref=key))
            stage_prereq = stage_id
        # Versorgung (wiederholbar, Pflicht-Tor vor Langzeit)
        sup_title = f"Versorgung der Station ({kerbals(n)})"
        sup_desc = (f"Bring eine vollständige Ablösung von {kerbals(n)} zu %station% und docke an. "
                    f"Pflichtflug vor dem Langzeitbetrieb dieser Stufe, danach beliebig wiederholbar.")
        out.append(contract(sup_id, sup_title, sup_desc, sub, round((110+12*n)*mult), [stage_prereq],
                   [{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"},
                    orbit_chk, {"kind": "DOCK_STATION", "stationKey": key}],
                   repeatable=True, ref=key))
        # Langzeitbetrieb (Tor zum naechsten Ausbau)
        long_title = f"Dauerbetrieb 150 Tage ({kerbals(n)})"
        long_desc = (f"Halte %station% 150 Tage ununterbrochen mit {kerbals(n)} besetzt und beweise "
                     f"stabilen Langzeitbetrieb auf dieser Stufe.")
        out.append(contract(long_id, long_title, long_desc, sub, round((260+30*n)*mult), [sup_id],
                   [{"kind": "CREW_MIN", "min": n, "label": crew_lbl}, orbit_chk,
                    {"kind": "DURATION", "days": 150, "label": f"150 Tage mit {kerbals(n)} ausharren"}],
                   ref=key))
        prev_long = long_id
    return "".join(out), prev_long

def base_chain(prefix, key, body, sub, stages, build_id, prereq0, base_word, mult, header):
    out = [header]
    prev_long = None
    landed = lambda: {"kind": "SITUATION", "situation": "LANDED", "label": "Sicher auf der Oberfläche gelandet"}
    onbody = {"kind": "ON_BODY", "body": body, "label": f"Auf {('dem Mond' if body=='Moon' else body)}"}
    for i, n in enumerate(stages):
        is_build = (i == 0)
        stage_id = build_id if is_build else f"{prefix}_expand{n}"
        sup_id = f"{prefix}_supply{n}"
        long_id = f"{prefix}_longstay{n}"
        crew_lbl = f"Bemannt mit mindestens {kerbals(n)} an Bord"
        if is_build:
            core = base_word[6:] if base_word.startswith("Erste ") else base_word
            title = f"{base_word} ({kerbals(n)})"
            desc = (f"Errichte deine {core} auf der Oberfläche und halte sie 10 Tage mit {kerbals(n)} "
                    f"besetzt. Der Name wird gespeichert — Versorgungs- und Ausbauflüge beziehen sich darauf.")
            out.append(contract(stage_id, title, desc, sub, round(240*mult), [prereq0],
                       [{"kind": "CREW_MIN", "min": n, "label": crew_lbl}, onbody, landed(),
                        {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} halten"}],
                       record=key))
            stage_prereq = stage_id
        else:
            title = f"Basisausbau auf {kerbals(n)}"
            desc = (f"Erweitere %station% auf {n} Besatzungsmitglieder und halte den Basisbetrieb 10 Tage "
                    f"stabil, bevor der nächste Schritt ansteht.")
            out.append(contract(stage_id, title, desc, sub, round((180+20*n)*mult), [prev_long],
                       [{"kind": "CREW_MIN", "min": n, "label": crew_lbl}, onbody, landed(),
                        {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} halten"}],
                       ref=key))
            stage_prereq = stage_id
        sup_title = f"Versorgung der Basis ({kerbals(n)})"
        sup_desc = (f"Lande ein bemanntes Versorgungsschiff mit {kerbals(n)} bei %station%. Pflichtflug "
                    f"vor dem Langzeitbetrieb dieser Stufe, danach beliebig wiederholbar.")
        out.append(contract(sup_id, sup_title, sup_desc, sub, round((110+12*n)*mult), [stage_prereq],
                   [{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"},
                    onbody, landed()], repeatable=True, ref=key))
        long_title = f"Dauerbetrieb 150 Tage ({kerbals(n)})"
        long_desc = f"Halte %station% 150 Tage ununterbrochen mit {kerbals(n)} besetzt."
        out.append(contract(long_id, long_title, long_desc, sub, round((260+30*n)*mult), [sup_id],
                   [{"kind": "CREW_MIN", "min": n, "label": crew_lbl}, onbody, landed(),
                    {"kind": "DURATION", "days": 150, "label": f"150 Tage mit {kerbals(n)} ausharren"}],
                   ref=key))
        prev_long = long_id
    return "".join(out), prev_long

EARTH = [2, 3, 4, 6, 8, 10, 12]
MOON_ST = [2, 3, 4, 6, 8, 10, 12]
MARS_ST = [2, 3, 4, 6]
MOON_BASE = [1, 2, 3, 4]

earth, earth_last = orbit_chain("cr_station", "earth_station", "Earth", "Erde", "Erdorbit", 130,
    EARTH, "cr_station_leo", "cr_luna_orbit", "Erste Raumstation im Erdorbit", 1.0,
    "    // ===== ERDE — Raumstation (2 -> 12 Kerbals) =====\n")
moon_st, moon_st_last = orbit_chain("cr_luna_station", "moon_station", "Moon", "Luna", "Mondorbit", 25,
    MOON_ST, "cr_luna_station", "cr_station_expand4", "Erste Mond-Raumstation im Mondorbit", 1.5,
    "\n    // ===== LUNA — Raumstation (ab Erd-Ausbau 4) =====\n")
moon_base, moon_base_last = base_chain("cr_luna_base", "moon_base", "Moon", "Luna",
    MOON_BASE, "cr_luna_base", "cr_station_expand4", "Erste Mondbasis", 1.5,
    "\n    // ===== LUNA — Oberflaechenbasis (ab Erd-Ausbau 4) =====\n")
mars_st, mars_st_last = orbit_chain("cr_mars_station", "mars_station", "Mars", "Mars", "Marsorbit", 90,
    MARS_ST, "cr_mars_station", moon_st_last, "Erste Mars-Raumstation im Marsorbit", 2.4,
    "\n    // ===== MARS — Raumstation (ab voll ausgebauter Mondstation) =====\n")

# Extras: GEO-Station, Marsflug-Demonstrator, bemannter Mond-Vorbeiflug (Vorschau-Tor)
extras = "\n    // ===== Sondermissionen =====\n"
extras += contract("cr_station_geo", "Bemannte Station im Hochorbit (2 Kerbals)",
    "Bringe eine bemannte Station in den geostationären Hochorbit der Erde und halte sie dort 10 Tage — ein anspruchsvoller Aussichtsposten weit über der gewohnten Bahn.",
    "Erde", 260, ["cr_station_leo"],
    [{"kind": "CREW_MIN", "min": 2, "label": "Bemannt mit mindestens 2 Kerbals an Bord"},
     {"kind": "ORBIT_ABOVE", "body": "Earth", "km": 2000, "label": "Hochorbit, Periapsis über 2000 km"},
     {"kind": "DURATION", "days": 10, "label": "10 Tage stabil halten"}], ref="earth_station")
extras += contract("cr_station_longstay200", "Marsflug-Demonstrator (200 Tage)",
    "Bevor Menschen zum Mars aufbrechen, baue eigens ein autarkes Langzeit-Raumschiff — ausdrücklich keine Station — und halte zwei Kerbals 200 Tage ununterbrochen im Erdorbit als Generalprobe für die lange Reise.",
    "Erde", 620, ["cr_station_expand4"],
    [{"kind": "CREW_EXACT", "min": 2, "label": "Eigenes Transitschiff mit genau 2 Kerbals an Bord"},
     {"kind": "ORBIT_ABOVE", "body": "Earth", "km": 130, "label": "Stabiler Erdorbit, Periapsis über 130 km"},
     {"kind": "DURATION", "days": 200, "label": "200 Tage ununterbrochen ausharren"}])
extras += contract("cr_luna_flyby_crewed", "Erster bemannter Mond-Vorbeiflug",
    "Schicke erstmals Menschen bis zum Mond und zurück: ein bemannter Vorbeiflug, der zeigt, dass deine Crew den Heimatorbit verlassen und sicher heimkehren kann. Ab hier zeichnen sich kommende Ziele am Horizont ab.",
    "Luna", 200, ["cr_earth_longorbit"],
    [{"kind": "CREW_MIN", "min": 1, "label": "Bemannt mit mindestens 1 Kerbal an Bord"},
     {"kind": "ON_BODY", "body": "Moon", "label": "Mond-Einflusssphäre erreicht"}])

header = ("// ===========================================================================\n"
          "//  SPARTE A — STATIONEN & BASEN (generiert, gleiches Muster pro Ausbaustufe)\n"
          "//  Quelle: tools/gen_stations.py — nicht von Hand editieren, sondern Skript anpassen.\n"
          "//  Muster je Stufe N: Ausbau(N) -> Versorgung(N, wiederholbar) -> Dauerbetrieb 150 T(N).\n"
          "// ===========================================================================\n\n"
          "CUSTOM_CONTRACT_CATALOG\n{\n")
with open(D, "w", encoding="utf-8") as f:
    f.write(header + earth + moon_st + moon_base + mars_st + extras + "}\n")

# --- A_Pioniere.cfg: alte Ketten-Contracts entfernen ---
drop = set()
for n in EARTH:
    for p in (f"cr_station_expand{n}", f"cr_station_supply{n}", f"cr_station_longstay{n}"): drop.add(p)
drop |= {"cr_station_leo", "cr_station_resupply", "cr_station_rotation", "cr_station_longstay",
         "cr_station_expand3", "cr_station_expand4", "cr_station_expand6", "cr_station_geo",
         "cr_station_longstay200",
         "cr_luna_station", "cr_luna_station_resupply",
         "cr_luna_base", "cr_luna_base_expand", "cr_luna_base_expand4", "cr_luna_base_resupply",
         "cr_luna_base_rotation", "cr_luna_base2",
         "cr_mars_station", "cr_mars_station_rotation"}

txt = open(A, encoding="utf-8").read()
cat = txt.find("CUSTOM_CONTRACT_CATALOG")
bstart = txt.find("{", cat)
header_a = txt[:bstart+1]
body = txt[bstart+1:]
pos = 0
kept = []
dropped = []
while True:
    idx = body.find("CONTRACT", pos)
    if idx == -1:
        tail = body[pos:]
        break
    pre = body[pos:idx]
    brace = body.find("{", idx)
    depth = 0; j = brace
    while j < len(body):
        if body[j] == "{": depth += 1
        elif body[j] == "}":
            depth -= 1
            if depth == 0: break
        j += 1
    block = body[idx:j+1]
    m = re.search(r"\bid\s*=\s*(\S+)", block)
    cid = m.group(1) if m else None
    if cid in drop:
        dropped.append(cid)
    else:
        kept.append(pre + block)
    pos = j + 1
open(A, "w", encoding="utf-8").write(header_a + "".join(kept) + tail)
print("Dropped from A:", len(dropped))
print("Earth last:", earth_last, "| Moon st last:", moon_st_last, "| Mars last:", mars_st_last)
print("D written:", D)
