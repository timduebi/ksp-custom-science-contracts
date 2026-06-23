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
OUT  = os.path.join(ROOT, "OptionalConfigs", "SOL-German", "GameData", "CustomScienceContracts", "Contracts")

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

ORBIT_MAX_KM = {
    "Earth": 500, "Moon": 100, "Mercury": 180, "Venus": 500, "Mars": 450,
    "Phobos": 30, "Deimos": 30, "Ceres": 120, "Ganymede": 300,
    "Jupiter": 25000, "Saturn": 25000, "Titan": 700,
}

def orbit_max_km(body, min_km=0):
    fallback = max(100.0, float(min_km or 0) * 4.0)
    return int(ORBIT_MAX_KM.get(body, fallback))

EPOCH_EXACT = {
    # Epoch 1: Earth test flights and first orbital techniques.
    "un_earth_pad_clear": 1, "un_earth_upper_atmo": 1, "un_earth_suborbital": 1,
    "un_earth_orbit": 1, "un_earth_science_satellite": 1, "un_earth_satellite_pair": 1,
    "un_earth_high_satellite": 1, "cr_earth_suborbital": 1, "cr_earth_orbit": 1,
    "cr_earth_orbit_eva": 1, "cr_earth_duration_3d": 1,
    "cr_earth_docking_target": 1, "cr_earth_docking_demo": 1,
    "cr_earth_duration_7d": 1, "cr_earth_trial_station": 1,

    # Epoch 2: Luna as the first major horizon.
    "un_luna_flyby": 2, "un_luna_orbit": 2, "un_luna_polar_mapping": 2,
    "un_luna_polar_landing": 2, "un_luna_landing": 2, "un_luna_rover": 2,
    "cr_luna_flyby_crewed": 2, "cr_luna_orbit": 2, "cr_luna_landing": 2,
    "cr_luna_stay_2d": 2, "cr_luna_precision_landing": 2, "cr_luna_stay_7d": 2,

    # Epoch 3: permanent Earth orbit, lunar infrastructure and first lifelines.
    "net_earth_comm_network3": 3, "net_earth_polar_comm_network": 3,
    "net_luna_comm_network3": 3, "net_luna_polar_comm_network": 3,
    "un_venus_flyby": 3, "un_mercury_flyby": 3, "un_mars_flyby": 3,
    "un_venus_orbit": 3, "un_mercury_orbit": 3,
    "cr_luna_station_precision_landing_1": 2, "cr_luna_station_precision_landing_2": 2,

    # Epoch 4: inner worlds after the first interplanetary probes.
    "cr_venus_flyby": 4, "cr_venus_orbit": 4,
    "un_mars_orbit": 4, "un_mars_landing": 4,

    # Epoch 5: Mars as the second main crewed arc.
    "un_mars_polar_mapping": 5,
    "un_mars_polar_landing": 5, "un_mars_rover": 5,
    "un_mars_precision_landing": 5,
    "cr_mars_flyby": 5, "cr_mars_orbit": 5,
    "cr_mars_landing": 5, "cr_mars_precision_landing": 5, "cr_mars_stay_10d": 5,
    "cr_mars_stay_30d": 5,
    "net_mars_comm_network": 5, "net_mars_orbit_supply": 5,
    "un_eros_flyby": 5, "un_vesta_flyby": 5, "un_ceres_flyby": 5,
    "un_eros_orbit": 5, "un_vesta_orbit": 5, "un_ceres_orbit": 5,
    "un_psyche_flyby": 5,

    # Epoch 6: asteroid belt, Mars moons and optional industry.
    "un_phobos_flyby": 6, "un_phobos_orbit": 6,
    "un_deimos_flyby": 6, "un_deimos_orbit": 6,
    "cr_phobos_landing": 6,
    "cr_deimos_landing": 6,
    "net_solar_comm_network": 6, "net_phobos_cache": 6,
    "un_ceres_landing": 6, "un_vesta_landing": 6,
    "un_pallas_flyby": 6, "un_pallas_orbit": 6, "un_psyche_orbit": 6,
    "un_ryugu_flyby": 6, "un_ryugu_landing": 6, "un_ida_flyby": 6,
    "un_dactyl_flyby": 6, "cr_ceres_flyby": 6, "cr_ceres_orbit": 6,
    "cr_ceres_landing": 6, "cr_ceres_stay_7d": 6, "net_ceres_ore_test": 6,
    "net_ceres_supply_cache": 6, "net_psyche_ore_test": 6, "net_vesta_supply_cache": 6,

    # Epoch 7: Jupiter system.
    "net_jupiter_comm_network": 7, "net_callisto_ore_test": 7, "net_callisto_supply_cache": 7,
    "cr_jupiter_flyby": 7, "cr_jupiter_orbit": 7, "cr_ganymede_orbit": 7,
    "cr_ganymede_landing": 7, "cr_ganymede_stay_7d": 7,

    # Epoch 8: Saturn system and Titan.
    "net_saturn_comm_network": 8, "net_titan_supply_test": 8, "net_saturn_transfer_cache": 8,
    "cr_saturn_flyby": 8, "cr_saturn_orbit": 8, "cr_titan_orbit": 8,
    "cr_titan_landing": 8, "cr_titan_stay_7d": 8,

    # Epoch 9: far probes and Titan base finale.
    "cr_titan_base4": 9, "cr_titan_base6": 9, "cr_titan_base8": 9,
}

def _stage_from_chain_suffix(action, stage):
    if action == "build":
        return 2
    return int(stage)

def epoch_for_id(cid):
    if cid in EPOCH_EXACT:
        return EPOCH_EXACT[cid]

    m = re.match(r"cr_(earth_station|moon_station|moon_base|mars_station|mars_base|earth_fuel_depot)_(build|expand|supply|longstay)(\d*)$", cid)
    if m:
        key, action, raw_stage = m.groups()
        stage = _stage_from_chain_suffix(action, raw_stage or "2")
        if key == "earth_station":
            return 2 if stage <= 4 else 4 if stage <= 8 else 6
        if key in ("moon_station", "moon_base"):
            return 3 if stage <= 3 else 4 if stage <= 6 else 6
        if key in ("mars_station", "mars_base"):
            return 5 if stage <= 3 else 6
        if key == "earth_fuel_depot":
            return 4 if stage <= 4 else 6

    if cid.startswith(("un_venus_", "un_mercury_", "un_sun_", "un_mars_", "un_phobos_", "un_deimos_")):
        return 4
    if cid.startswith("cr_mars_"):
        return 5
    if cid.startswith(("un_jupiter_", "un_io_", "un_europa_", "un_ganymede_", "un_callisto_")):
        return 7
    if cid.startswith(("un_saturn_", "un_titan_", "un_enceladus_", "un_rhea_", "un_iapetus_",
                       "un_dione_", "un_tethys_", "un_mimas_", "un_hyperion_", "un_phoebe_")):
        return 8
    if cid.startswith(("un_uranus_", "un_titania_", "un_oberon_", "un_ariel_", "un_umbriel_",
                       "un_miranda_", "un_puck_", "un_neptune_", "un_triton_", "un_nereid_",
                       "un_proteus_", "un_pluto_", "un_charon_", "un_arrokoth_")):
        return 9
    if cid.startswith(("un_luna_", "cr_luna_")):
        return 2
    if cid.startswith(("un_earth_", "cr_earth_")):
        return 1

    return 1

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
 "cr_earth_duration_3d": "Drei Tage im Erdorbit",
 "cr_earth_docking_target": "Andockziel im Erdorbit", "cr_earth_docking_demo": "Erstes Andockmanöver",
 "cr_earth_duration_7d": "Eine Woche im Erdorbit", "cr_earth_trial_station": "Ein-Modul-Labor im Orbit",
 "un_sun_inner_probe": "Sonnennahe Sonde", "cr_luna_flyby_crewed": "Erster bemannter Mond-Vorbeiflug",
 "cr_luna_orbit": "Bemannter Mondorbit und Rückkehr",
 "cr_luna_landing": "Erste bemannte Mondlandung",
 "cr_luna_stay_2d": "Zwei Tage auf Luna",
 "cr_luna_precision_landing": "Präzisionslandung auf Luna",
 "cr_luna_stay_7d": "Sieben Tage auf Luna",
 "cr_titan_base4": "Titanbasis (4 Kerbals)", "cr_titan_base6": "Titanbasis-Ausbau (6 Kerbals)",
 "cr_titan_base8": "Titanbasis-Ausbau (8 Kerbals)",
 "un_luna_rover": "Rover-Landung auf Luna", "un_mars_rover": "Rover-Landung auf Mars",
 "un_luna_polar_landing": "Polarlandung auf Luna", "un_mars_polar_landing": "Polarlandung auf Mars",
 "un_titan_polar_landing": "Polarlandung auf Titan",
}

# ---------------- Parsing ----------------
def parse_check(s):
    """'FLYBY Moon 500 | label' -> (kind, ordered-kv-list, label)."""
    head, _, label = s.partition("|")
    label = label.strip()
    toks = head.split()
    kind = toks[0]; a = toks[1:]
    kv = []
    if kind in ("CREW_MIN", "CREW_EXACT", "CREW_CAPACITY_MIN"): kv = [("min", a[0])]
    elif kind == "CREW_NONE": kv = []
    elif kind in ("SUBORBITAL", "LANDED", "ORE_SURFACE"): kv = [("body", a[0])]
    elif kind == "ORBIT_ABOVE":
        kv = [("body", a[0])] + ([("km", a[1])] if len(a) > 1 else [])
    elif kind == "APOAPSIS_MAX":
        kv = [("body", a[0]), ("km", a[1])]
    elif kind == "INCLINATION_MIN":
        kv = [("body", a[0]), ("inclinationMin", a[1])]
    elif kind == "ATMO_FRACTION":
        kv = [("body", a[0]), ("fracMin", fpct(a[1])), ("fracMax", fpct(a[2]))]
    elif kind == "FLYBY": kv = [("body", a[0]), ("km", a[1])]
    elif kind == "MARKER_LANDING":
        kv = [("body", a[0]), ("km", a[1])] + ([("latMin", a[2]), ("latMax", a[3])] if len(a) > 3 else [])
    elif kind == "VESSEL_COUNT":
        kv = [("body", a[0]), ("count", a[1])] + ([("km", a[2])] if len(a) > 2 else [])
    elif kind == "VESSEL_COUNT_INCLINATION":
        kv = [("body", a[0]), ("count", a[1]), ("inclinationMin", a[2])] + ([("km", a[3])] if len(a) > 3 else [])
    elif kind == "EVA": kv = [("body", a[0])] + ([("situation", a[1])] if len(a) > 1 else [])
    elif kind == "FUEL_MIN": kv = [("amount", a[0])]
    elif kind == "RESOURCE_MIN": kv = [("resource", a[0]), ("amount", a[1])]
    elif kind == "WHEEL_MOTION": kv = [("body", a[0]), ("speed", a[1])]
    elif kind == "DOCK_ANY": kv = []
    elif kind == "DOCK_STATION": kv = [("stationKey", a[0])]
    elif kind == "HOLD": kv = [("seconds", a[0])]
    elif kind == "DURATION": kv = [("days", a[0])]
    elif kind == "RELAY_VESSEL_COUNT":
        kv = [("body", a[0]), ("count", a[1])] + ([("km", a[2])] if len(a) > 2 else [])
    elif kind == "RELAY_VESSEL_COUNT_INCLINATION":
        kv = [("body", a[0]), ("count", a[1]), ("inclinationMin", a[2])] + ([("km", a[3])] if len(a) > 3 else [])
    elif kind == "RETURN_FROM_BODY":
        kv = [("body", a[0]), ("returnBody", a[1])]
        if len(a) > 2: kv.append(("returnMode", a[2]))
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
            mm = re.match(r"(id|sparte|body|prereq|reward|repeatable|recordStation|stationRef|beschreibung_en|beschreibung|icon):\s*(.*)$", s)
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
    elif "DOCK_ANY" in kinds or "DOCK_STATION" in kinds: noun = f"Andockmanöver im Orbit um {body}"
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
    if "DOCK_ANY" in kinds or "DOCK_STATION" in kinds: return "TrackingStation_ButtonMapStation"
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
             repeatable=False, reveal=None, record=None, ref=None, epoch=None):
    s = "    CONTRACT\n    {\n"
    s += f"        id = {cid}\n        title = {title}\n        description = {desc}\n"
    s += f"        sparte = {sparte}\n        subcategory = {sub}\n        icon = {icon}\n"
    s += f"        reward = {reward}\n"
    s += f"        epoch = {epoch if epoch is not None else epoch_for_id(cid)}\n"
    if repeatable: s += "        repeatable = true\n"
    if reveal:  s += f"        revealAllAfter = {reveal}\n"
    if record:  s += f"        recordStationKey = {record}\n"
    if ref:     s += f"        stationRef = {ref}\n"
    for p in prereqs: s += f"        prerequisite = {p}\n"
    s += "        CONDITION\n        {\n            type = COMPOSITE\n"
    s += emit_checks(checks)
    s += "        }\n    }\n"
    return s

SKIP_IDS = {"net_deimos_cache", "cr_phobos_orbit", "cr_deimos_orbit"}

def contract_prereqs(m):
    if m["id"] == "cr_earth_docking_demo":
        return ["cr_earth_docking_target"]
    if m["id"] == "net_phobos_cache":
        return ["cr_mars_landing", "un_phobos_orbit"]
    if m["id"] == "cr_ceres_landing":
        return ["cr_ceres_flyby"]
    return [] if m.get("prereq", "-") in ("-", "") else [p.strip() for p in m["prereq"].split(",")]

def docking_target_contract():
    checks = [
        ("CREW_NONE", [], "unbemannt"),
        ("ORBIT_ABOVE", [("body", "Earth"), ("km", "130")],
         "Stabiler Erdorbit, Periapsis über 130 km"),
        ("HOLD", [("seconds", "10")], "10 Sekunden stabil halten"),
    ]
    desc = ("Bringe ein unbemanntes Andockziel in einen stabilen Erdorbit. Es bleibt als "
            "Trainingsziel registriert, damit die erste Crew gezielt an einem echten Objekt "
            "andocken kann.")
    return contract("cr_earth_docking_target", TITLE["cr_earth_docking_target"], desc,
                    "Bemannt", "Erde", "TrackingStation_ButtonMapStation", 62,
                    ["cr_earth_duration_3d"], checks,
                    record="earth_docking_target", epoch=1)

def mission_contract(m):
    prereqs = contract_prereqs(m)
    sub = SUBCAT[m["body"]]
    reveal = REVEAL.get(sub) if m["sparte"] == "Robotische Erkunder" else None
    checks = catalog_checks(m)
    title_model = dict(m)
    title_model["checks"] = checks
    return contract(m["id"], title_for(title_model), description_for_catalog(m, m["beschreibung"], checks),
                    SPARTE[m["sparte"]], sub,
                    m.get("icon") or icon_for(title_model), m["reward"], prereqs, checks,
                    repeatable=(m.get("repeatable") == "yes"), reveal=reveal)

def catalog_checks(m):
    checks = [normalize_network_check(m, c) for c in m["checks"]]
    checks = apply_curated_check_overrides(m, checks)
    checks = ensure_crewed_orbit_requirements(m, checks)
    if m["id"].endswith("_rover"):
        checks.append(("WHEEL_MOTION", [("body", m["body"]), ("speed", "4")],
                       "Rover mit Rädern fährt am Boden mindestens 4 m/s"))

    if should_add_return_check(m, checks):
        checks.append(return_check_for(m))
    return checks

def ensure_crewed_orbit_requirements(m, checks):
    if m["sparte"] != "Pioniere" or not any(kind == "ORBIT_ABOVE" for kind, _, _ in checks):
        return checks

    out = []
    has_duration = any(kind == "DURATION" for kind, _, _ in checks)
    for kind, kvl, label in checks:
        if kind == "ORBIT_ABOVE":
            out.append((kind, kvl, label))
            values = dict(kvl)
            body = values.get("body", m["body"])
            max_km = orbit_max_km(body, values.get("km", 0))
            out.append(("APOAPSIS_MAX", [("body", body), ("km", str(max_km))],
                        f"Apoapsis unter {max_km} km"))
            continue

        if kind == "HOLD" and not has_duration:
            out.append(("DURATION", [("days", "0.5")], "0,5 Tage im Zielorbit halten"))
            has_duration = True
            continue

        if kind == "DURATION":
            values = dict(kvl)
            days = float(values.get("days", 0))
            if days < 0.5:
                out.append(("DURATION", [("days", "0.5")], "0,5 Tage im Zielorbit halten"))
            else:
                out.append((kind, kvl, label))
            has_duration = True
            continue

        out.append((kind, kvl, label))

    if not has_duration:
        out.append(("DURATION", [("days", "0.5")], "0,5 Tage im Zielorbit halten"))
    return out

def apply_curated_check_overrides(m, checks):
    cid = m["id"]

    if cid == "cr_earth_docking_demo":
        return [("DOCK_STATION", [("stationKey", "earth_docking_target")], "Am Andockziel angedockt")
                if kind == "DOCK_ANY" else (kind, kvl, label)
                for kind, kvl, label in checks]

    if cid == "net_phobos_cache":
        return [
            ("CREW_NONE", [], "unbemannt"),
            ("ORBIT_ABOVE", [("body", "Phobos"), ("km", "8")],
             "Stabiler Orbit um Phobos, Periapsis über 8 km"),
            ("FUEL_MIN", [("amount", "500")], "Treibstoff über 500"),
            ("HOLD", [("seconds", "10")], "10 Sekunden stabil halten"),
        ]

    crew_overrides = {
        "cr_luna_flyby_crewed": 2,
        "cr_luna_orbit": 3,
        "cr_luna_landing": 1,
        "cr_luna_stay_2d": 2,
        "cr_luna_stay_7d": 2,
    }
    if cid not in crew_overrides:
        return checks

    need = crew_overrides[cid]
    label = f"mindestens {need} Kerbal{'s' if need != 1 else ''} an Bord"
    out = []
    replaced = False
    for kind, kvl, old_label in checks:
        if not replaced and kind in ("CREW_MIN", "CREW_EXACT"):
            out.append(("CREW_MIN", [("min", str(need))], label))
            replaced = True
        else:
            out.append((kind, kvl, old_label))
    return out

def normalize_network_check(m, check):
    kind, kvl, label = check
    if kind not in ("VESSEL_COUNT", "VESSEL_COUNT_INCLINATION"):
        return check
    if not is_satellite_network(m, kind, kvl):
        return check

    new_kind = "RELAY_VESSEL_COUNT_INCLINATION" if kind == "VESSEL_COUNT_INCLINATION" else "RELAY_VESSEL_COUNT"
    suffix = " (jede Sonde mit Relaisantenne)"
    return new_kind, kvl, label + ("" if "Relaisantenne" in label else suffix)

def is_satellite_network(m, kind, kvl):
    values = dict(kvl)
    count = int(values.get("count", "1"))
    if count < 2:
        return False
    return m["sparte"] == "Versorgungsnetz" or m["id"] == "un_earth_satellite_pair"

def should_add_return_check(m, checks):
    if m["sparte"] != "Pioniere":
        return False
    if "_base" in m["id"]:
        return False
    return not any(kind == "RETURN_FROM_BODY" for kind, _, _ in checks)

def return_check_for(m):
    kinds = [kind for kind, _, _ in m["checks"]]
    mode = None
    if "FLYBY" in kinds:
        mode = "flyby"
    elif m["body"] == "Earth":
        mode = "home"
    elif not any(kind in ("LANDED", "MARKER_LANDING") for kind in kinds):
        mode = "visit"

    kv = [("body", m["body"]), ("returnBody", "Earth")]
    if mode:
        kv.append(("returnMode", mode))
    return "RETURN_FROM_BODY", kv, "Crew sicher zur Erde zurückbringen"

def description_for_catalog(m, desc, checks):
    if m["id"] == "net_phobos_cache":
        return ("Richte ein unbemanntes Treibstoffdepot im Orbit um Phobos ein. Der kleine "
                "Mond wird zum stillen Helfer für alle künftigen Marsoperationen.")
    original_has_return = any(kind == "RETURN_FROM_BODY" for kind, _, _ in m["checks"])
    generated_has_return = any(kind == "RETURN_FROM_BODY" for kind, _, _ in checks)
    if generated_has_return and not original_has_return:
        return desc.rstrip() + " Die Mission zählt erst, wenn die Crew sicher zur Erde zurückgekehrt ist."
    return desc

HEADER = ("// ===========================================================================\n"
          "//  {t}\n"
          "//  GENERIERT aus custom_science_contracts_missionsdesign.md (tools/gen_catalog.py).\n"
          "//  Optionale deutschsprachige SOL-Konfiguration.\n"
          "//  NICHT von Hand editieren — Designplan/Skript aendern und neu generieren.\n"
          "//  Body-Namen = interne CelestialBody.name (Luna = Moon, Stern = Sun).\n"
          "// ===========================================================================\n\n"
          "CUSTOM_CONTRACT_CATALOG\n{{\n")

def write_file(path, title, body):
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write(HEADER.format(t=title) + body + "}\n")

def write_optional_readme():
    path = os.path.join(ROOT, "OptionalConfigs", "SOL-German", "README.md")
    os.makedirs(os.path.dirname(path), exist_ok=True)
    with open(path, "w", encoding="utf-8") as f:
        f.write("""# German config

Optional config pack that puts the whole SOL (real solar system) campaign into German — the
same missions, prerequisites and rewards, just with German titles, descriptions and labels.

It replaces only the four catalog files in `GameData/CustomScienceContracts/Contracts/`; the
plugin, icons and licenses stay from the main download.

Install:
1. Install the main mod download first.
2. Copy this folder's `GameData` directory into your KSP install root.
3. Allow it to overwrite the four files in `GameData/CustomScienceContracts/Contracts`.

Only contract text/config files are replaced. The plugin, icons, licenses and other
assets come from the main download.
""")

# ---------------- Stationsketten ----------------
def kerbals(n): return "1 Kerbal" if n == 1 else f"{n} Kerbals"
def seats(n): return "1 Platz" if n == 1 else f"{n} Plätze"
def seats_dative(n): return "1 verfügbarem Platz" if n == 1 else f"{n} verfügbaren Plätzen"

def orbit_chain(key, body, sub, orbitword, km, stages, prereq0, station_word, mult):
    out, prev_long = [], None
    crew = lambda n: {"kind": "CREW_MIN", "min": n, "label": f"Bemannt mit mindestens {kerbals(n)} an Bord"}
    capacity = lambda n: {"kind": "CREW_CAPACITY_MIN", "min": n, "label": f"Mindestens {seats(n)} verfügbar"}
    orbit = {"kind": "ORBIT_ABOVE", "body": body, "km": km,
             "label": f"Stabiler {orbitword}, Periapsis über {km} km"}
    apo = {"kind": "APOAPSIS_MAX", "body": body, "km": orbit_max_km(body, km),
           "label": f"Apoapsis unter {orbit_max_km(body, km)} km"}
    def cks(lst): return [(c["kind"], [(k, v) for k, v in c.items() if k != "kind" and k != "label"], c.get("label", "")) for c in lst]
    for i, n in enumerate(stages):
        build = (i == 0)
        sid = f"cr_{key}_build" if build else f"cr_{key}_expand{n}"
        sup, lng = f"cr_{key}_supply{n}", f"cr_{key}_longstay{n}"
        empty = {"kind": "CREW_NONE", "label": "Station unbemannt, keine Kerbals an Bord"}
        if build:
            core = station_word[6:] if station_word.startswith("Erste ") else station_word
            title = f"{station_word} ({seats(n)})"
            desc = (f"Errichte deine erste Raumstation im {orbitword} mit mindestens "
                    f"{seats_dative(n)}. Die Station muss dabei leer und unbemannt sein; Besatzung "
                    f"zählt erst ab der Versorgung. Mit ihr beginnt für dein Programm die Ära ständiger Präsenz — "
                    f"der Name, den du ihr gibst, begleitet jeden künftigen Versorgungsflug.")
            out.append(contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapStation",
                       round(220 * mult), [prereq0],
                       cks([empty, capacity(n), orbit, apo, {"kind": "DURATION", "days": 10, "label": "10 Tage im Zielorbit stabil halten"}]),
                       record=key))
        else:
            title = f"Stationsausbau auf {seats(n)}"
            desc = (f"Erweitere %station% auf mindestens {seats(n)} und "
                    f"halte die Station für diesen Ausbau leer und unbemannt. Besatzung "
                    f"zählt erst ab der nächsten Versorgung.")
            out.append(contract(sid, title, desc, "Bemannt", sub, "TrackingStation_ButtonMapStation",
                       round((180 + 20 * n) * mult), [prev_long],
                       cks([empty, capacity(n), orbit, apo, {"kind": "DURATION", "days": 10, "label": "10 Tage im Zielorbit stabil halten"}]),
                       ref=key))
        out.append(contract(sup, f"Versorgung der Station ({kerbals(n)})",
                   f"Bring eine frische Ablösung von mindestens {kerbals(n)} zu %station% und docke an, um die "
                   f"müde gewordene Stammbesatzung abzulösen.", "Bemannt", sub, "TrackingStation_ButtonMapShips",
                   round((110 + 12 * n) * mult), [sid],
                   cks([{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"},
                        orbit, apo, {"kind": "DOCK_STATION", "stationKey": key, "label": "An der Station angedockt"}]),
                   repeatable=True, ref=key))
        out.append(contract(lng, f"Dauerbetrieb 150 Tage ({kerbals(n)})",
                   f"Halte %station% 150 Tage ununterbrochen mit mindestens {kerbals(n)} besetzt und "
                   f"beweise stabilen Langzeitbetrieb auf dieser Stufe.", "Bemannt", sub, "TrackingStation_ButtonMapStation",
                   round((260 + 30 * n) * mult), [sup],
                   cks([crew(n), orbit, apo, {"kind": "DURATION", "days": 150, "label": f"150 Tage mit {kerbals(n)} ausharren"}]),
                   ref=key))
        prev_long = lng
    return "".join(out)

def moon_base_site_survey_landings():
    def cks(lst): return [(c["kind"], [(k, v) for k, v in c.items() if k != "kind" and k != "label"], c.get("label", "")) for c in lst]
    common = [
        {"kind": "CREW_MIN", "min": 2, "label": "Bemannt mit mindestens 2 Kerbals an Bord"},
        {"kind": "LANDED", "body": "Moon", "label": "Auf Luna gelandet"},
        {"kind": "MARKER_LANDING", "body": "Moon", "km": 5, "label": "Präzisionslandung im Umkreis von 5 km"},
        {"kind": "RETURN_FROM_BODY", "body": "Moon", "returnBody": "Earth", "label": "Crew sicher zur Erde zurückbringen"},
    ]
    first = contract(
        "cr_luna_station_precision_landing_1",
        "Erkundung erster Mondbasis-Standort",
        "Nutze die Erfahrung aus der Erdorbitalstation, um einen möglichen Standort für eine spätere Mondbasis zu testen. Lande präzise, sammle Eindrücke vor Ort und bringe die Crew sicher zurück.",
        "Bemannt", "Luna", "TrackingStation_ButtonMapFlag", 176,
        ["cr_earth_station_expand4"], cks(common), epoch=2)
    second = contract(
        "cr_luna_station_precision_landing_2",
        "Erkundung zweiter Mondbasis-Standort",
        "Teste einen zweiten möglichen Standort für die spätere Mondbasis. Die Mission bleibt ein optionaler Vergleichsflug und blockiert keinen Ausbaupfad.",
        "Bemannt", "Luna", "TrackingStation_ButtonMapFlag", 188,
        ["cr_luna_station_precision_landing_1"], cks(common), epoch=2)
    return first + second

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
        apo = {"kind": "APOAPSIS_MAX", "body": "Earth", "km": orbit_max_km("Earth", 130),
               "label": f"Apoapsis unter {orbit_max_km('Earth', 130)} km"}
        fuel = [{"kind": "RESOURCE_MIN", "resource": "LiquidFuel", "amount": lf, "label": f"Mindestens {lf} LiquidFuel im Tank"},
                {"kind": "RESOURCE_MIN", "resource": "Oxidizer", "amount": ox, "label": f"Mindestens {ox} Oxidizer im Tank"}]
        if build:
            title = f"Treibstoff-Tankstelle im Orbit ({kerbals(n)})"
            desc = (f"Errichte deine bemannte Treibstoff-Tankstelle im Erdorbit, besetze sie mit mindestens "
                    f"{kerbals(n)} und fülle die Tanks auf {lf} LiquidFuel und {ox} Oxidizer. Künftige Reisen "
                    f"sollen hier nachtanken — der Name bleibt für jeden Nachschubflug erhalten.")
            out.append(contract(sid, title, desc, "NetzwerkLogistik", sub, "TrackingStation_ButtonMapStation",
                       round(150 * mult), [prereq0],
                       cks([crew, orbit, apo] + fuel + [{"kind": "DURATION", "days": 10, "label": "10 Tage stabil betrieben"}]),
                       record=key))
        else:
            title = f"Tankstellen-Ausbau ({kerbals(n)})"
            desc = (f"Erweitere %station% auf mindestens {kerbals(n)} und stocke den Vorrat auf {lf} "
                    f"LiquidFuel und {ox} Oxidizer auf.")
            out.append(contract(sid, title, desc, "NetzwerkLogistik", sub, "TrackingStation_ButtonMapStation",
                       round((150 + 25 * i) * mult), [prev],
                       cks([crew, orbit, apo] + fuel + [{"kind": "DURATION", "days": 10, "label": "10 Tage stabil betrieben"}]),
                       ref=key))
        out.append(contract(sup, f"Nachbetankung der Tankstelle ({kerbals(n)})",
                   f"Bring frischen Treibstoff und eine Ablösung von mindestens {kerbals(n)} zu %station% und docke an.",
                   "NetzwerkLogistik", sub, "TrackingStation_ButtonMapShips", round((90 + 12 * n) * mult), [sid],
                   cks([{"kind": "CREW_MIN", "min": n, "label": f"Versorgungsschiff mit mindestens {kerbals(n)} an Bord"},
                        orbit, apo, {"kind": "DOCK_STATION", "stationKey": key, "label": "An der Tankstelle angedockt"}]),
                   repeatable=True, ref=key))
        prev = sid
    return "".join(out)

def build_stations():
    s = ""
    s += "    // ===== ERDE — Raumstation (3 -> 12 Plaetze) =====\n"
    s += orbit_chain("earth_station", "Earth", "Erde", "Erdorbit", 130,
                     [3, 4, 6, 8, 10, 12], "cr_luna_landing", "Erste Raumstation im Erdorbit", 1.0)
    s += "\n    // ===== LUNA — Raumstation (ab Erd-Dauerbetrieb 4) =====\n"
    s += orbit_chain("moon_station", "Moon", "Luna", "Mondorbit", 25,
                     [3, 4, 6, 8, 10], "cr_earth_station_longstay4", "Erste Mond-Raumstation im Mondorbit", 1.5)
    s += "\n    // ===== LUNA — Basisstandort-Erkundungen nach Erdstation 4 =====\n"
    s += moon_base_site_survey_landings()
    s += "\n    // ===== LUNA — Oberflaechenbasis (ab 150 Tage Mondstation 3 Kerbals) =====\n"
    s += base_chain("moon_base", "Moon", "Luna", [2, 3, 4, 6, 8, 10],
                    "cr_moon_station_longstay3", "Erste Mondbasis", 1.5)
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
    for m in missions:
        if m["id"] in SKIP_IDS:
            continue
        if m["id"] == "cr_earth_docking_demo":
            buckets["Pioniere"].append(docking_target_contract())
        buckets[m["sparte"]].append(mission_contract(m))
    write_file(os.path.join(OUT, "A_Pioniere.cfg"), "SPARTE A — PIONIERE (bemannt)", "".join(buckets["Pioniere"]))
    write_file(os.path.join(OUT, "B_Spaeher.cfg"), "SPARTE B — ROBOTISCHE ERKUNDER (unbemannt)", "".join(buckets["Robotische Erkunder"]))
    write_file(os.path.join(OUT, "C_Lebensadern.cfg"), "SPARTE C — VERSORGUNGSNETZ (Logistik)", "".join(buckets["Versorgungsnetz"]))
    write_file(os.path.join(OUT, "D_Stationen.cfg"), "STATIONEN, BASEN & DEPOTS (generierte Ketten)", build_stations())
    write_optional_readme()
    print(f"A Pioniere:           {len(buckets['Pioniere'])}")
    print(f"B Robotische Erkunder:{len(buckets['Robotische Erkunder'])}")
    print(f"C Versorgungsnetz:    {len(buckets['Versorgungsnetz'])}")
    print(f"D Stationen: generiert ({sum(len(s) for s in [build_stations()])} Zeichen)")
    print("Deutsche SOL-Konfiguration geschrieben nach", OUT)
    import gen_catalog_en
    gen_catalog_en.main()

if __name__ == "__main__":
    main()
