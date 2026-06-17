#!/usr/bin/env python3
"""Erzeugt den kompletten Contract-Katalog aus custom_science_contracts_missionsdesign.md:
  A_Pioniere.cfg     (sparte Pioniere            -> Bemannt)
  B_Spaeher.cfg      (sparte Robotische Erkunder -> UnbemannteErkundung)
  C_Lebensadern.cfg  (sparte Versorgungsnetz     -> NetzwerkLogistik)
  D_Stationen.cfg    (generierte Stations-/Basis-/Depot-Ketten aus STATIONSKETTEN)
Titel werden regelbasiert (+ kuratierte Sonderfaelle) gebildet, das Icon je Mission nach
dominantem Check. Body-Namen sind interne CelestialBody.name (Luna = Moon)."""
import re, os

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DOC  = os.path.join(ROOT, "custom_science_contracts_missionsdesign.md")
OUT  = os.path.join(ROOT, "GameData", "CustomScienceContracts", "Contracts")

SPARTE = {"Pioniere": "Bemannt", "Robotische Erkunder": "UnbemannteErkundung",
          "Versorgungsnetz": "NetzwerkLogistik"}

DISPLAY = {"Earth": "Erde", "Moon": "Luna", "Mercury": "Merkur", "Neptune": "Neptun",
           "Sun": "Sonne", "Ganymede": "Ganymed", "Callisto": "Kallisto"}
def disp(b): return DISPLAY.get(b, b)

SUBCAT = {"Earth": "Erde", "Moon": "Luna", "Mercury": "Merkur", "Sun": "Interplanetar",
          "Venus": "Venus", "Mars": "Mars", "Phobos": "Mars", "Deimos": "Mars",
          "Jupiter": "Jupiter", "Io": "Jupiter", "Europa": "Jupiter", "Ganymede": "Jupiter",
          "Callisto": "Jupiter", "Saturn": "Saturn", "Titan": "Saturn", "Enceladus": "Saturn",
          "Rhea": "Saturn", "Iapetus": "Saturn", "Dione": "Saturn", "Tethys": "Saturn",
          "Mimas": "Saturn", "Hyperion": "Saturn", "Phoebe": "Saturn", "Uranus": "Uranus",
          "Titania": "Uranus", "Oberon": "Uranus", "Ariel": "Uranus", "Umbriel": "Uranus",
          "Miranda": "Uranus", "Puck": "Uranus", "Neptune": "Neptun", "Triton": "Neptun",
          "Nereid": "Neptun", "Proteus": "Neptun", "Pluto": "Pluto", "Charon": "Pluto",
          "Eros": "Asteroiden", "Vesta": "Asteroiden", "Ceres": "Asteroiden", "Psyche": "Asteroiden",
          "Ryugu": "Asteroiden", "Ida": "Asteroiden", "Dactyl": "Asteroiden", "Pallas": "Asteroiden",
          "Arrokoth": "Asteroiden"}

# Aeusseres System: nach dem Planeten-Flyby werden alle Contracts der Unterkat. sichtbar.
REVEAL = {"Jupiter": "un_jupiter_flyby", "Saturn": "un_saturn_flyby", "Uranus": "un_uranus_flyby",
          "Neptun": "un_neptune_flyby", "Pluto": "un_pluto_flyby"}

# Kuratierte Titel (wo die Formel unschoen waere)
TITLE = {
 "un_earth_pad_clear": "Erster Testflug", "un_earth_upper_atmo": "Obere Erdatmosphäre",
 "un_earth_suborbital": "Unbemannter Suborbitalflug", "cr_earth_suborbital": "Erster bemannter Suborbitalflug",
 "un_earth_orbit": "Erster Erdorbit (unbemannt)", "un_earth_science_satellite": "Erster Forschungssatellit",
 "un_earth_satellite_pair": "Erstes Satellitennetz im Erdorbit", "un_earth_high_satellite": "Satellit im Hochorbit",
 "un_luna_polar_mapping": "Polare Kartierung von Luna",
 "un_mercury_polar_mapping": "Polare Kartierung von Merkur",
 "un_mars_polar_mapping": "Polare Kartierung von Mars",
 "un_jupiter_polar_mapping": "Polare Jupiter-Systemkartierung",
 "un_saturn_polar_mapping": "Polare Saturn-Systemkartierung",
 "un_titan_polar_mapping": "Polare Kartierung von Titan",
 "un_triton_polar_mapping": "Polare Kartierung von Triton",
 "un_pluto_polar_mapping": "Polare Kartierung von Pluto",
 "net_earth_comm_network3": "Neues Erd-Kommunikationsnetz",
 "net_earth_polar_comm_network": "Polare Erd-Relais",
 "net_luna_comm_network3": "Luna-Kommunikationsnetz",
 "net_luna_polar_comm_network": "Polare Luna-Relais",
 "net_mars_comm_network": "Mars-Kommunikationsnetz",
 "net_solar_comm_network": "Interplanetarer Relaisring",
 "net_jupiter_comm_network": "Jupiter-Kommunikationsnetz",
 "net_saturn_comm_network": "Saturn-Kommunikationsnetz",
 "cr_earth_orbit": "Erster bemannter Erdorbit", "cr_earth_orbit_eva": "Erste EVA im Erdorbit",
 "cr_earth_duration_3d": "Drei Tage im Erdorbit", "cr_earth_docking_demo": "Erstes Andockmanöver",
 "cr_earth_duration_7d": "Eine Woche im Erdorbit", "cr_earth_trial_station": "Ein-Modul-Labor im Orbit",
 "un_sun_inner_probe": "Sonnennahe Sonde", "cr_luna_flyby_crewed": "Erster bemannter Mond-Vorbeiflug",
 "cr_titan_base4": "Titanbasis (4 Kerbals)", "cr_titan_base6": "Titanbasis-Ausbau (6 Kerbals)",
 "cr_titan_base8": "Titanbasis-Ausbau (8 Kerbals)",
}

# ---------------- Parsing ----------------
def parse_check(s):
    """'FLYBY Moon 500 | label' -> (kind, ordered-kv-list, label)."""
    head, _, label = s.partition("|")
    label = label.strip()
    toks = head.split()
    kind = toks[0]; a = toks[1:]
    kv = []
    if kind in ("CREW_MIN", "CREW_EXACT"): kv = [("min", a[0])]
    elif kind == "CREW_NONE": kv = []
    elif kind in ("SUBORBITAL", "LANDED", "ORE_SURFACE"): kv = [("body", a[0])]
    elif kind == "ORBIT_ABOVE":
        kv = [("body", a[0])] + ([("km", a[1])] if len(a) > 1 else [])
    elif kind == "INCLINATION_MIN":
        kv = [("body", a[0]), ("inclinationMin", a[1])]
    elif kind == "ATMO_FRACTION":
        kv = [("body", a[0]), ("fracMin", fpct(a[1])), ("fracMax", fpct(a[2]))]
    elif kind == "FLYBY": kv = [("body", a[0]), ("km", a[1])]
    elif kind == "MARKER_LANDING": kv = [("body", a[0]), ("km", a[1])]
    elif kind == "VESSEL_COUNT":
        kv = [("body", a[0]), ("count", a[1])] + ([("km", a[2])] if len(a) > 2 else [])
    elif kind == "VESSEL_COUNT_INCLINATION":
        kv = [("body", a[0]), ("count", a[1]), ("inclinationMin", a[2])] + ([("km", a[3])] if len(a) > 3 else [])
    elif kind == "EVA": kv = [("body", a[0])] + ([("situation", a[1])] if len(a) > 1 else [])
    elif kind == "FUEL_MIN": kv = [("amount", a[0])]
    elif kind == "RESOURCE_MIN": kv = [("resource", a[0]), ("amount", a[1])]
    elif kind == "DOCK_ANY": kv = []
    elif kind == "HOLD": kv = [("seconds", a[0])]
    elif kind == "DURATION": kv = [("days", a[0])]
    else: raise SystemExit(f"Unbekannter Check '{kind}' in: {s}")
    return kind, kv, label

def fpct(p):
    v = float(p) / 100.0
    return (f"{v:.3f}").rstrip("0").rstrip(".")

def parse_missions(text):
    out = []
    for blk in text.split("=== MISSION ===")[1:]:
        m, checks = {}, []
        for line in blk.splitlines():
            s = line.strip()
            if not s or s.startswith("=="):
                if s.startswith("=="): break
                continue
            if s.startswith("check:"):
                checks.append(parse_check(s[6:].strip())); continue
            mm = re.match(r"(id|sparte|body|prereq|reward|repeatable|recordStation|stationRef|beschreibung):\s*(.*)$", s)
            if mm: m[mm.group(1)] = mm.group(2).strip()
        if "id" in m:
            m["checks"] = checks
            out.append(m)
    return out

# ---------------- Titel / Icon ----------------
MASC = ("Vorbeiflug", "Orbit", "Ausstieg", "Suborbitalflug")          # -> "Bemannter"
def title_for(m):
    if m["id"] in TITLE: return TITLE[m["id"]]
    body = disp(m["body"]); crew = (m["sparte"] == "Pioniere")
    kinds = [k for k, _, _ in m["checks"]]
    days = next((kv for k, kv, _ in m["checks"] if k == "DURATION"), None)
    days = dict(days).get("days") if days else None
    def kv(k): return next((dict(kvl) for kk, kvl, _ in m["checks"] if kk == k), {})
    if "FUEL_MIN" in kinds:
        noun = (f"Treibstoffdepot im Orbit um {body}" if "ORBIT_ABOVE" in kinds
                else f"Treibstofflager auf {body}" if "LANDED" in kinds else f"Treibstoffdepot bei {body}")
    elif "ORE_SURFACE" in kinds: noun = f"Ore-Förderung auf {body}"
    elif "MARKER_LANDING" in kinds: noun = f"Präzisionslandung auf {body}"
    elif "DOCK_ANY" in kinds: noun = f"Andockmanöver im Orbit um {body}"
    elif "EVA" in kinds and "LANDED" not in kinds: noun = f"Ausstieg im Orbit um {body}"
    elif "LANDED" in kinds:
        noun = f"{int(float(days))} Tage auf {body}" if days else f"Landung auf {body}"
    elif "FLYBY" in kinds: noun = f"Vorbeiflug an {body}"
    elif "VESSEL_COUNT_INCLINATION" in kinds or "VESSEL_COUNT" in kinds:
        fleet = "VESSEL_COUNT_INCLINATION" if "VESSEL_COUNT_INCLINATION" in kinds else "VESSEL_COUNT"
        noun = (f"Satellitennetz um {body}" if int(kv(fleet).get('count', '1')) >= 2
                else f"Satellit im Hochorbit um {body}")
    elif "ATMO_FRACTION" in kinds: noun = f"Atmosphärensonde {body}"
    elif "SUBORBITAL" in kinds: noun = f"Suborbitalflug über {body}"
    elif "ORBIT_ABOVE" in kinds:
        noun = (f"Polarer Kartierungsorbit um {body}" if "INCLINATION_MIN" in kinds else
                f"Forschungssatellit um {body}" if (days and not crew) else f"Orbit um {body}")
    else: noun = f"Mission bei {body}"
    if crew and not noun[0].isdigit():
        adj = "Bemannter" if noun.split()[0] in MASC else "Bemannte"
        noun = f"{adj} {noun}"
    return noun

def icon_for(m):
    kinds = [k for k, _, _ in m["checks"]]
    has_dur = "DURATION" in kinds; crew = (m["sparte"] == "Pioniere")
    if "MARKER_LANDING" in kinds: return "TrackingStation_ButtonMapFlag"
    if "FUEL_MIN" in kinds: return "TrackingStation_ButtonMapBase"
    if "ORE_SURFACE" in kinds: return "TrackingStation_ButtonMapBase"
    if "DOCK_ANY" in kinds: return "TrackingStation_ButtonMapStation"
    if "EVA" in kinds and "LANDED" not in kinds: return "TrackingStation_ButtonMapEVA"
    if "LANDED" in kinds: return "TrackingStation_ButtonMapBase" if has_dur else "TrackingStation_ButtonMapLander"
    if "FLYBY" in kinds: return "TrackingStation_ButtonMapProbe"
    if "VESSEL_COUNT_INCLINATION" in kinds or "VESSEL_COUNT" in kinds: return "TrackingStation_ButtonMapCommunicationsRelay"
    if "ATMO_FRACTION" in kinds or "SUBORBITAL" in kinds: return "TrackingStation_ButtonMapAircraft"
    if "ORBIT_ABOVE" in kinds:
        if has_dur and crew: return "TrackingStation_ButtonMapStation"
        if crew: return "TrackingStation_ButtonMapShips"
        # unbemannter Orbiter/Satellit: Relais-Icon, damit es sich klar vom Flyby (Probe) abhebt
        return "TrackingStation_ButtonMapCommunicationsRelay"
    return "TrackingStation_ButtonMapProbe"

# ---------------- Emission ----------------
def emit_checks(checks):
    s = ""
    for kind, kvl, label in checks:
        s += "            CHECK\n            {\n"
        s += f"                kind = {kind}\n"
        for k, v in kvl: s += f"                {k} = {v}\n"
        if label: s += f"                label = {label}\n"
        s += "            }\n"
    return s

def contract(cid, title, desc, sparte, sub, icon, reward, prereqs, checks,
             repeatable=False, reveal=None, record=None, ref=None):
    s = "    CONTRACT\n    {\n"
    s += f"        id = {cid}\n        title = {title}\n        description = {desc}\n"
    s += f"        sparte = {sparte}\n        subcategory = {sub}\n        icon = {icon}\n"
    s += f"        reward = {reward}\n"
    if repeatable: s += "        repeatable = true\n"
    if reveal:  s += f"        revealAllAfter = {reveal}\n"
    if record:  s += f"        recordStationKey = {record}\n"
    if ref:     s += f"        stationRef = {ref}\n"
    for p in prereqs: s += f"        prerequisite = {p}\n"
    s += "        CONDITION\n        {\n            type = COMPOSITE\n"
    s += emit_checks(checks)
    s += "        }\n    }\n"
    return s

def mission_contract(m):
    prereqs = [] if m.get("prereq", "-") in ("-", "") else [p.strip() for p in m["prereq"].split(",")]
    sub = SUBCAT[m["body"]]
    reveal = REVEAL.get(sub) if m["sparte"] == "Robotische Erkunder" else None
    return contract(m["id"], title_for(m), m["beschreibung"], SPARTE[m["sparte"]], sub,
                    icon_for(m), m["reward"], prereqs, m["checks"],
                    repeatable=(m.get("repeatable") == "yes"), reveal=reveal)

HEADER = ("// ===========================================================================\n"
          "//  {t}\n"
          "//  GENERIERT aus custom_science_contracts_missionsdesign.md (tools/gen_catalog.py).\n"
          "//  NICHT von Hand editieren — Designplan/Skript aendern und neu generieren.\n"
          "//  Body-Namen = interne CelestialBody.name (Luna = Moon, Stern = Sun).\n"
          "// ===========================================================================\n\n"
          "CUSTOM_CONTRACT_CATALOG\n{{\n")

def write_file(path, title, body):
    with open(path, "w", encoding="utf-8") as f:
        f.write(HEADER.format(t=title) + body + "}\n")

# ---------------- Stationsketten ----------------
def kerbals(n): return "1 Kerbal" if n == 1 else f"{n} Kerbals"

def orbit_chain(key, body, sub, orbitword, km, stages, prereq0, station_word, mult):
    out, prev_long = [], None
    crew = lambda n: {"kind": "CREW_MIN", "min": n, "label": f"Bemannt mit mindestens {kerbals(n)} an Bord"}
    orbit = {"kind": "ORBIT_ABOVE", "body": body, "km": km,
             "label": f"Stabiler {orbitword}, Periapsis über {km} km"}
    def cks(lst): return [(c["kind"], [(k, v) for k, v in c.items() if k != "kind" and k != "label"], c.get("label", "")) for c in lst]
    for i, n in enumerate(stages):
        build = (i == 0)
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup, lng = f"cr_{key}_supply{n}", f"cr_{key}_longstay{n}"
        if build:
            core = station_word[6:] if station_word.startswith("Erste ") else station_word
            title = f"{station_word} ({kerbals(n)})"
            desc = (f"Errichte deine erste Raumstation im {orbitword} und halte sie mit mindestens "
                    f"{kerbals(n)} an Bord. Mit ihr beginnt für dein Programm die Ära ständiger Präsenz — "
                    f"der Name, den du ihr gibst, begleitet jeden künftigen Versorgungsflug.")
            out.append(contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapStation",
                       round(220 * mult), [prereq0],
                       cks([crew(n), orbit, {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} stabil halten"}]),
                       record=key))
        else:
            title = f"Stationsausbau auf {kerbals(n)}"
            desc = (f"Erweitere %station% auf mindestens {kerbals(n)} und festige den Betrieb auf der "
                    f"neuen Stufe, ehe der nächste Ausbau ansteht.")
            out.append(contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapStation",
                       round((180 + 20 * n) * mult), [prev_long],
                       cks([crew(n), orbit, {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} stabil halten"}]),
                       ref=key))
        out.append(contract(sup, f"Versorgung der Station ({kerbals(n)})",
                   f"Bring eine frische Ablösung von mindestens {kerbals(n)} zu %station% und docke an, um die "
                   f"müde gewordene Stammbesatzung abzulösen.", "Bemannt", sub, "TrackingStation_ButtonMapShips",
                   round((110 + 12 * n) * mult), [sid],
                   cks([{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"},
                        orbit, {"kind": "DOCK_STATION", "stationKey": key, "label": "An der Station angedockt"}]),
                   repeatable=True, ref=key))
        out.append(contract(lng, f"Dauerbetrieb 150 Tage ({kerbals(n)})",
                   f"Halte %station% 150 Tage ununterbrochen mit mindestens {kerbals(n)} besetzt und "
                   f"beweise stabilen Langzeitbetrieb auf dieser Stufe.", "Bemannt", sub, "TrackingStation_ButtonMapStation",
                   round((260 + 30 * n) * mult), [sup],
                   cks([crew(n), orbit, {"kind": "DURATION", "days": 150, "label": f"150 Tage mit {kerbals(n)} ausharren"}]),
                   ref=key))
        prev_long = lng
    return "".join(out)

def base_chain(key, body, sub, stages, prereq0, base_word, mult):
    out, prev_long = [], None
    bdisp = disp(body)
    crew = lambda n: {"kind": "CREW_MIN", "min": n, "label": f"Bemannt mit mindestens {kerbals(n)} an Bord"}
    landed = {"kind": "LANDED", "body": body, "label": f"Auf {bdisp} gelandet"}
    def cks(lst): return [(c["kind"], [(k, v) for k, v in c.items() if k != "kind" and k != "label"], c.get("label", "")) for c in lst]
    prereq0_list = prereq0 if isinstance(prereq0, list) else [prereq0]
    for i, n in enumerate(stages):
        build = (i == 0)
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup, lng = f"cr_{key}_supply{n}", f"cr_{key}_longstay{n}"
        if build:
            core = base_word[6:] if base_word.startswith("Erste ") else base_word
            title = f"{base_word} ({kerbals(n)})"
            desc = (f"Errichte deine erste Basis auf {bdisp} und halte sie mit mindestens {kerbals(n)} "
                    f"besetzt. Fester Boden unter den Stiefeln — der Name der Basis bleibt erhalten und "
                    f"gilt für jeden weiteren Flug dorthin.")
            out.append(contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapBase",
                       round(240 * mult), prereq0_list,
                       cks([crew(n), landed, {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} halten"}]),
                       record=key))
        else:
            title = f"Basisausbau auf {kerbals(n)}"
            desc = (f"Erweitere %station% auf mindestens {kerbals(n)} und halte den Basisbetrieb stabil, "
                    f"bevor der nächste Ausbau folgt.")
            out.append(contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapBase",
                       round((180 + 20 * n) * mult), [prev_long],
                       cks([crew(n), landed, {"kind": "DURATION", "days": 10, "label": f"10 Tage mit {kerbals(n)} halten"}]),
                       ref=key))
        out.append(contract(sup, f"Versorgung der Basis ({kerbals(n)})",
                   f"Lande eine frische Ablösung von mindestens {kerbals(n)} bei %station% und halte die "
                   f"Besatzung der Basis in Schwung.",
                   "Bemannt", sub, "TrackingStation_ButtonMapLander", round((110 + 12 * n) * mult), [sid],
                   cks([{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"}, landed]),
                   repeatable=True, ref=key))
        out.append(contract(lng, f"Dauerbetrieb 150 Tage ({kerbals(n)})",
                   f"Halte %station% 150 Tage ununterbrochen mit mindestens {kerbals(n)} am Leben und "
                   f"beweise, dass Menschen hier dauerhaft leben können.", "Bemannt", sub, "TrackingStation_ButtonMapBase",
                   round((260 + 30 * n) * mult), [sup],
                   cks([crew(n), landed, {"kind": "DURATION", "days": 150, "label": f"150 Tage mit {kerbals(n)} ausharren"}]),
                   ref=key))
        prev_long = lng
    return "".join(out)

def fuel_depot_chain(key, sub, stages, prereq0, mult):
    """Bemannte Treibstoff-Tankstelle im Erdorbit. Crew = Stufe; Treibstoff startet bei
    1440 LF / 1760 Ox und waechst je Stufe um +1440 / +1760. Kein 150-Tage-Tor."""
    out, prev = [], None
    def cks(lst): return [(c["kind"], [(k, v) for k, v in c.items() if k != "kind" and k != "label"], c.get("label", "")) for c in lst]
    for i, n in enumerate(stages):
        build = (i == 0)
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup = f"cr_{key}_supply{n}"
        lf, ox = 1440 * (i + 1), 1760 * (i + 1)
        crew = {"kind": "CREW_MIN", "min": n, "label": f"Bemannt mit mindestens {kerbals(n)} an Bord"}
        orbit = {"kind": "ORBIT_ABOVE", "body": "Earth", "km": 130, "label": "Stabiler Erdorbit, Periapsis über 130 km"}
        fuel = [{"kind": "RESOURCE_MIN", "resource": "LiquidFuel", "amount": lf, "label": f"Mindestens {lf} LiquidFuel im Tank"},
                {"kind": "RESOURCE_MIN", "resource": "Oxidizer", "amount": ox, "label": f"Mindestens {ox} Oxidizer im Tank"}]
        if build:
            title = f"Treibstoff-Tankstelle im Orbit ({kerbals(n)})"
            desc = (f"Errichte deine bemannte Treibstoff-Tankstelle im Erdorbit, besetze sie mit mindestens "
                    f"{kerbals(n)} und fülle die Tanks auf {lf} LiquidFuel und {ox} Oxidizer. Künftige Reisen "
                    f"sollen hier nachtanken — der Name bleibt für jeden Nachschubflug erhalten.")
            out.append(contract(sid, title, desc, "NetzwerkLogistik", sub, "TrackingStation_ButtonMapStation",
                       round(150 * mult), [prereq0],
                       cks([crew, orbit] + fuel + [{"kind": "DURATION", "days": 10, "label": "10 Tage stabil betrieben"}]),
                       record=key))
        else:
            title = f"Tankstellen-Ausbau ({kerbals(n)})"
            desc = (f"Erweitere %station% auf mindestens {kerbals(n)} und stocke den Vorrat auf {lf} "
                    f"LiquidFuel und {ox} Oxidizer auf.")
            out.append(contract(sid, title, desc, "NetzwerkLogistik", sub, "TrackingStation_ButtonMapStation",
                       round((150 + 25 * i) * mult), [prev],
                       cks([crew, orbit] + fuel + [{"kind": "DURATION", "days": 10, "label": "10 Tage stabil betrieben"}]),
                       ref=key))
        out.append(contract(sup, f"Nachbetankung der Tankstelle ({kerbals(n)})",
                   f"Bring frischen Treibstoff und eine Ablösung von mindestens {kerbals(n)} zu %station% und docke an.",
                   "NetzwerkLogistik", sub, "TrackingStation_ButtonMapShips", round((90 + 12 * n) * mult), [sid],
                   cks([{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"},
                        orbit, {"kind": "DOCK_STATION", "stationKey": key, "label": "An der Tankstelle angedockt"}]),
                   repeatable=True, ref=key))
        prev = sid
    return "".join(out)

def build_stations():
    s = ""
    s += "    // ===== ERDE — Raumstation (2 -> 12 Kerbals) =====\n"
    s += orbit_chain("earth_station", "Earth", "Erde", "Erdorbit", 130,
                     [2, 3, 4, 6, 8, 10, 12], "cr_luna_landing", "Erste Raumstation im Erdorbit", 1.0)
    s += "\n    // ===== LUNA — Raumstation (ab Erd-Dauerbetrieb 4) =====\n"
    s += orbit_chain("moon_station", "Moon", "Luna", "Mondorbit", 25,
                     [2, 3, 4, 6, 8, 10], "cr_earth_station_longstay4", "Erste Mond-Raumstation im Mondorbit", 1.5)
    s += "\n    // ===== LUNA — Oberflaechenbasis (ab Praezisionslandung + 7 Tage) =====\n"
    s += base_chain("moon_base", "Moon", "Luna", [2, 3, 4, 6, 8, 10],
                    ["cr_luna_precision_landing", "cr_luna_stay_7d"], "Erste Mondbasis", 1.5)
    s += "\n    // ===== MARS — Raumstation (ab 10 Tage auf Mars) =====\n"
    s += orbit_chain("mars_station", "Mars", "Mars", "Marsorbit", 90,
                     [2, 3, 4, 6], "cr_mars_stay_10d", "Erste Mars-Raumstation im Marsorbit", 2.4)
    s += "\n    // ===== MARS — Oberflaechenbasis (ab 30 Tage auf Mars) =====\n"
    s += base_chain("mars_base", "Mars", "Mars", [2, 3, 4, 6], "cr_mars_stay_30d", "Erste Marsbasis", 2.6)
    s += "\n    // ===== ERDE — Treibstoff-Tankstelle (Versorgungsnetz, bemannt) =====\n"
    s += fuel_depot_chain("earth_fuel_depot", "Erde", [2, 3, 4, 6], "cr_earth_station_longstay4", 1.0)
    return s

# ---------------- Main ----------------
def main():
    text = open(DOC, encoding="utf-8").read()
    missions = parse_missions(text)
    buckets = {"Pioniere": [], "Robotische Erkunder": [], "Versorgungsnetz": []}
    for m in missions: buckets[m["sparte"]].append(mission_contract(m))
    write_file(os.path.join(OUT, "A_Pioniere.cfg"), "SPARTE A — PIONIERE (bemannt)", "".join(buckets["Pioniere"]))
    write_file(os.path.join(OUT, "B_Spaeher.cfg"), "SPARTE B — ROBOTISCHE ERKUNDER (unbemannt)", "".join(buckets["Robotische Erkunder"]))
    write_file(os.path.join(OUT, "C_Lebensadern.cfg"), "SPARTE C — VERSORGUNGSNETZ (Logistik)", "".join(buckets["Versorgungsnetz"]))
    write_file(os.path.join(OUT, "D_Stationen.cfg"), "STATIONEN, BASEN & DEPOTS (generierte Ketten)", build_stations())
    print(f"A Pioniere:           {len(buckets['Pioniere'])}")
    print(f"B Robotische Erkunder:{len(buckets['Robotische Erkunder'])}")
    print(f"C Versorgungsnetz:    {len(buckets['Versorgungsnetz'])}")
    print(f"D Stationen: generiert ({sum(len(s) for s in [build_stations()])} Zeichen)")
    print("Geschrieben nach", OUT)

if __name__ == "__main__":
    main()
