# Missionsplan-Spec v3 — Custom Contract Plugin

**System:** SOL (Real Solar System Recreation), Quarter-Scale (~2.5x Stock)
**Config:** Kerbalism + Simplex, Community Tech Tree
**Startkoerper:** Earth (KSC)

> Body-Namen werden aus dem configs-Ordner uebernommen (in Claude Code). Hier Platzhalter.

---

## Struktur

**4 Sparten:**
- A) Bemannt
- B) Unbemannte Erkundung
- C) Netzwerk / Logistik / Minen
- D) Wiederholbar (dynamisch, siehe unten)

**Unterkategorien pro Koerper** innerhalb jeder Sparte. Luna ist eine EIGENE Unterkategorie
(nicht unter Earth). Alle anderen Monde sind ihrem Planeten untergeordnet
(Phobos/Deimos -> Mars, Galileische Monde -> Jupiter, usw.).

**Slot-Limit pro Unterkategorie:** Bemannt/Netzwerk max. 3 gleichzeitig sichtbare offene
Contracts pro Koerper-Unterkategorie, Erkundung max. 5. Naechster gesperrt -> Slot leer.

**Fixe Reihenfolge, keine Part-Anforderungen.** Nur Stock-Zustaende.

### Sparte D: Wiederholbar (Mechanik)

- Ein `Repeatable`-Contract lebt VOR dem Erstabschluss in seiner Heimat-Unterkategorie.
- Nach dem Erstabschluss wandert er in die Sparte **Wiederholbar** (raus aus der Heimat-Liste).
- **Cooldown:** erst wieder einloesbar, nachdem >= 2 ANDERE Missionen abgeschlossen wurden.
  - Zaehler `completionsSinceLastClaim`, reset auf 0 beim Einloesen, +1 bei jedem anderen
    Abschluss, claimbar wenn >= 2.
- Im UI: Heimat-Unterkategorie zeigt ihn bis Erstabschluss; danach nur noch in Wiederholbar,
  mit Cooldown-Anzeige ("noch 1 Mission bis wieder verfuegbar").

### Bedingungstypen

| Typ | Pruefung |
|---|---|
| `ALT_FRACTION_ATMO` | Hoehe zwischen f1*atmoDepth und f2*atmoDepth, Situation FLYING |
| `ABOVE_ATMO_SUBORBITAL` | Hoehe > atmoDepth, Situation SUB_ORBITAL |
| `ORBIT` | ORBITING + orbit.PeA > atmoDepth |
| `ORBIT_HIGH` | ORBITING + orbit.PeA > X km |
| `FLYBY` | SOI betreten, nie georbited, SOI verlassen (State-Tracking) |
| `LANDED` | LANDED/SPLASHED auf Zielkoerper |
| `ATMO_ENTRY` | Wechsel zu FLYING ueber Koerper mit Atmosphaere |
| `EVA` | aktiver Kerbal im EVA-Zustand in Ziel-Situation |
| `CREW_DURATION` | Situation + Crew >= N + MET-Spanne >= T Tage |
| `DOCK` | onVesselDock in Ziel-Situation |
| `RENDEZVOUS` | zwei Vessels < D km in Ziel-Situation |
| `ORE_SURFACE` | LANDED + GetResourceAmount("Ore") > 0 |
| `VESSEL_COUNT_ORBIT` | >= N Vessels gleichzeitig ORBITING um Koerper |
| `FUEL_ORBIT` | ORBITING + Treibstoff > Schwelle |
| `MARKER_LANDING` | LANDED + Distanz zu Waypoint <= R |

---

# SPARTE A — BEMANNT

## Erde
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_pad | Startrampe verlassen | ALT_FRACTION_ATMO Earth 0.0-0.2, Crew>=1 | - | 30 | - |
| cr_upperatmo | Obere Atmosphaere | ALT_FRACTION_ATMO Earth 0.5-1.0, Crew>=1 | cr_pad | 45 | - |
| cr_leaveatmo | Atmosphaere verlassen (suborbital) | ABOVE_ATMO_SUBORBITAL Earth, Crew>=1 | cr_upperatmo | 70 | - |
| cr_earth_orbit | Erster bemannter Erdorbit | ORBIT Earth, Crew>=1 | cr_leaveatmo, **un_earth_orbit** | 120 | - |
| cr_earth_eva | EVA im Erdorbit | EVA ORBITING Earth | cr_earth_orbit | 100 | - |
| cr_earth_longorbit | Extended Orbit Stay (15 Tage) | CREW_DURATION ORBIT Earth, Crew>=1, 15d | cr_earth_orbit | 160 | - |
| cr_station_leo | Erste Raumstation (2 Crew/10d) | CREW_DURATION ORBIT Earth, Crew>=2, 10d | **cr_luna_orbit** | 220 | - |
| cr_station_resupply | Stationsversorgung (Andockung) | DOCK ORBIT Earth | cr_station_leo | 130 | JA |
| cr_station_expand3 | Stationsausbau auf 3 Crew | CREW_DURATION ORBIT Earth, Crew>=3, 10d | cr_station_leo | 190 | - |
| cr_station_rotation | Stations-Crewrotation | DOCK ORBIT Earth | cr_station_expand3 | 140 | JA |
| cr_station_expand4 | Stationsausbau auf 4 Crew | CREW_DURATION ORBIT Earth, Crew>=4, 15d | cr_station_expand3 | 240 | - |
| cr_station_longstay | Langzeitcrew Station (60d) | CREW_DURATION ORBIT Earth, Crew>=2, 60d | cr_station_leo | 280 | - |
| cr_station_geo | Bemannte Station Hochorbit | CREW_DURATION ORBIT_HIGH Earth >2000km, Crew>=2, 10d | cr_station_leo | 250 | - |

## Luna  (eigene Unterkategorie)
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_luna_orbit | Bemannter Luna-Orbit (Mond-Gate) | ORBIT Luna, Crew>=1 | **cr_earth_longorbit**, un_luna_orbit | 240 | - |
| cr_luna_land | Erste bemannte Luna-Landung | LANDED Luna, Crew>=1 | cr_luna_orbit, **un_luna_land** | 340 | - |
| cr_luna_marker | Praezisionslandung Luna (<=15 km) | MARKER_LANDING Luna R=15km, Crew>=1 | cr_luna_land | 210 | - |
| cr_luna_eva_sci | Luna-Oberflaechen-EVA + Science | EVA LANDED Luna | cr_luna_land | 170 | - |
| cr_luna_base | Luna-Basis (1 Crew/20d) | CREW_DURATION LANDED Luna, Crew>=1, 20d | cr_luna_land | 320 | - |
| cr_luna_base_expand | Luna-Basis Ausbau (2 Crew) | CREW_DURATION LANDED Luna, Crew>=2, 15d | cr_luna_base | 270 | - |
| cr_luna_base_resupply | Luna-Basis-Versorgung (<=5 km) | MARKER_LANDING Luna R=5km | cr_luna_base | 180 | JA |
| cr_luna_base_rotation | Luna-Basis Crewrotation | MARKER_LANDING Luna R=5km, Crew>=1 | cr_luna_base | 180 | JA |
| cr_luna_station | Luna-Orbitalstation (2 Crew/10d) | CREW_DURATION ORBIT Luna, Crew>=2, 10d | cr_luna_orbit | 260 | - |
| cr_luna_station_resupply | Luna-Station Versorgung | DOCK ORBIT Luna | cr_luna_station | 160 | JA |
| cr_luna_base2 | Zweite Luna-Basis (neuer Standort) | MARKER_LANDING Luna R=15km, Crew>=1 | cr_luna_base | 300 | - |

## Mars  (inkl. Phobos/Deimos)
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_mars_orbit | Bemannter Mars-Orbit | ORBIT Mars, Crew>=1 | cr_luna_land, un_mars_orbit | 420 | - |
| cr_mars_land | Erste bemannte Mars-Landung | LANDED Mars, Crew>=1 | cr_mars_orbit, **un_mars_land** | 560 | - |
| cr_mars_marker | Praezisionslandung Mars (<=15 km) | MARKER_LANDING Mars R=15km, Crew>=1 | cr_mars_land | 390 | - |
| cr_mars_eva_sci | Mars-Oberflaechen-EVA + Science | EVA LANDED Mars | cr_mars_land | 310 | - |
| cr_mars_base | Mars-Basis (2 Crew/30d) | CREW_DURATION LANDED Mars, Crew>=2, 30d | cr_mars_land | 600 | - |
| cr_mars_base_expand | Mars-Basis Ausbau (3 Crew) | CREW_DURATION LANDED Mars, Crew>=3, 20d | cr_mars_base | 500 | - |
| cr_mars_base_resupply | Mars-Basis-Versorgung (<=5 km) | MARKER_LANDING Mars R=5km | cr_mars_base | 330 | JA |
| cr_mars_station | Mars-Orbitalstation (2 Crew/15d) | CREW_DURATION ORBIT Mars, Crew>=2, 15d | cr_mars_orbit | 450 | - |
| cr_mars_station_rotation | Mars-Stationsrotation | DOCK ORBIT Mars | cr_mars_station | 310 | JA |
| cr_mars_base2 | Zweite Mars-Basis (neuer Standort) | MARKER_LANDING Mars R=15km, Crew>=1 | cr_mars_base | 520 | - |
| cr_mars_longstay | Mars-Basis Langzeit (120d) | CREW_DURATION LANDED Mars, Crew>=2, 120d | cr_mars_base | 720 | - |
| cr_phobos_land | Bemannte Phobos-Landung | LANDED Phobos, Crew>=1 | cr_mars_orbit, un_phobos_land | 430 | - |
| cr_deimos_land | Bemannte Deimos-Landung | LANDED Deimos, Crew>=1 | cr_mars_orbit, un_deimos_land | 430 | - |

## Venus  (nur Orbit, keine Station)
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_venus_orbit | Bemannter Venus-Orbit | ORBIT Venus, Crew>=1 | cr_mars_orbit, un_venus_orbit | 480 | - |

## Merkur  (nur Orbit, keine Station)
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_mercury_orbit | Bemannter Merkur-Orbit | ORBIT Mercury, Crew>=1 | cr_venus_orbit, un_mercury_orbit | 560 | - |

## Interplanetar (Cycler)
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_cycler | Erde-Mars-Cycler (Sonnenorbit, 2 Crew/90d) | CREW_DURATION ORBIT Sun, Crew>=2, 90d | cr_mars_base | 750 | - |
| cr_cycler_resupply | Cycler-Versorgung (Andockung) | DOCK ORBIT Sun | cr_cycler | 420 | JA |

## Jupiter  (FINALE — bemannte Mondlandung)
| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| cr_jupiter_orbit | Bemannter Jupiter-Orbit (Strahlungshoelle) | ORBIT Jupiter, Crew>=1 | cr_cycler, un_jupiter_orbit | 800 | - |
| cr_ganymede_land | FINALE: Bemannte Ganymed-Landung | LANDED Ganymede, Crew>=1 | cr_jupiter_orbit, **un_ganymede_land** | 1200 | - |

> Finale = Ganymed (groesster Galileischer Mond, ausserhalb der schlimmsten Strahlung von
> Europa/Io). Falls du lieber Europa als bemanntes Finale willst: tauschbar, aber Strahlung
> dort brutal. Sag Bescheid.

---

# SPARTE B — UNBEMANNTE ERKUNDUNG

## Erde
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_earth_orbit | Erste Sonde in den Erdorbit | ORBIT Earth | - | 40 |

## Luna  (eigene Unterkategorie)
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_luna_flyby | Luna-Vorbeiflug | FLYBY Luna | un_earth_orbit | 70 |
| un_luna_orbit | Luna-Orbit + MITE-Scan | ORBIT Luna | un_luna_flyby | 110 |
| un_luna_land | Unbemannte Luna-Landung + MITE-Scan | LANDED Luna | un_luna_orbit | 180 |

## Mars
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_mars_flyby | Mars-Vorbeiflug | FLYBY Mars | un_earth_orbit | 130 |
| un_mars_orbit | Mars-Orbit + MITE-Scan | ORBIT Mars | un_mars_flyby | 200 |
| un_mars_land | Unbemannte Mars-Landung + MITE-Scan | LANDED Mars | un_mars_orbit | 300 |
| un_phobos_flyby | Phobos-Vorbeiflug | FLYBY Phobos | un_mars_orbit | 150 |
| un_phobos_land | Phobos-Landung | LANDED Phobos | un_phobos_flyby | 230 |
| un_deimos_flyby | Deimos-Vorbeiflug | FLYBY Deimos | un_mars_orbit | 150 |
| un_deimos_land | Deimos-Landung | LANDED Deimos | un_deimos_flyby | 230 |

## Venus
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_venus_flyby | Venus-Vorbeiflug | FLYBY Venus | un_earth_orbit | 130 |
| un_venus_orbit | Venus-Orbit + MITE-Scan | ORBIT Venus | un_venus_flyby | 220 |
| un_venus_atmo | Venus-Atmosphaereneintritt | ATMO_ENTRY Venus | un_venus_orbit | 280 |
| un_venus_land | Unbemannte Venus-Landung (kurzzeitig) | LANDED Venus | un_venus_atmo | 380 |

## Merkur
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_mercury_flyby | Merkur-Vorbeiflug | FLYBY Mercury | un_venus_orbit | 200 |
| un_mercury_orbit | Merkur-Orbit + MITE-Scan | ORBIT Mercury | un_mercury_flyby | 300 |
| un_mercury_land | Unbemannte Merkur-Landung | LANDED Mercury | un_mercury_orbit | 400 |

## Asteroiden  (SOL modelliert viele als Map-Bodies — Namen aus configs)
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_ast_flyby_1 | Asteroiden-Vorbeiflug [Name aus configs] | FLYBY [Ast1] | un_earth_orbit | 200 |
| un_ast_flyby_2 | Asteroiden-Vorbeiflug [Name aus configs] | FLYBY [Ast2] | un_earth_orbit | 200 |
| un_ast_flyby_3 | Asteroiden-Vorbeiflug [Name aus configs] | FLYBY [Ast3] | un_mars_orbit | 230 |
| un_ast_orbit_1 | Asteroiden-Orbit [groesster Ast] | ORBIT [Ast1] | un_ast_flyby_1 | 300 |
| un_ast_land_1 | Asteroiden-Landung/Anker [Ast1] | LANDED [Ast1] | un_ast_orbit_1 | 380 |
| un_ceres_flyby | Ceres-Vorbeiflug | FLYBY Ceres | un_mars_orbit | 280 |
| un_ceres_orbit | Ceres-Orbit | ORBIT Ceres | un_ceres_flyby | 360 |
| un_ceres_land | Ceres-Landung | LANDED Ceres | un_ceres_orbit | 440 |
| un_vesta_flyby | Vesta-Vorbeiflug | FLYBY Vesta | un_mars_orbit | 280 |
| un_vesta_land | Vesta-Landung | LANDED Vesta | un_vesta_flyby | 420 |
> Weitere Asteroiden-Flybys nach gleichem Muster aus dem configs-Ordner ergaenzen.

## Jupiter  (>=10 Mond-Flybys, Highlight-Orbits, Europa-Landung)
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_jupiter_flyby | Jupiter-Vorbeiflug | FLYBY Jupiter | un_mars_orbit | 320 |
| un_jupiter_orbit | Jupiter-Orbit + MITE-Scan | ORBIT Jupiter | un_jupiter_flyby | 450 |
| un_io_flyby | Io-Vorbeiflug | FLYBY Io | un_jupiter_flyby | 200 |
| un_europa_flyby | Europa-Vorbeiflug | FLYBY Europa | un_jupiter_flyby | 200 |
| un_ganymede_flyby | Ganymed-Vorbeiflug | FLYBY Ganymede | un_jupiter_flyby | 200 |
| un_callisto_flyby | Kallisto-Vorbeiflug | FLYBY Callisto | un_jupiter_flyby | 200 |
| un_jup_moon5_flyby | [Mond 5 aus configs] Vorbeiflug | FLYBY [...] | un_jupiter_flyby | 180 |
| un_jup_moon6_flyby | [Mond 6 aus configs] Vorbeiflug | FLYBY [...] | un_jupiter_flyby | 180 |
| un_jup_moon7_flyby | [Mond 7 aus configs] Vorbeiflug | FLYBY [...] | un_jupiter_flyby | 180 |
| un_jup_moon8_flyby | [Mond 8 aus configs] Vorbeiflug | FLYBY [...] | un_jupiter_flyby | 180 |
| un_jup_moon9_flyby | [Mond 9 aus configs] Vorbeiflug | FLYBY [...] | un_jupiter_flyby | 180 |
| un_jup_moon10_flyby | [Mond 10 aus configs] Vorbeiflug | FLYBY [...] | un_jupiter_flyby | 180 |
| un_europa_orbit | Europa-Orbit (Highlight) | ORBIT Europa | un_europa_flyby | 360 |
| un_europa_land | Europa-Landung | LANDED Europa | un_europa_orbit | 520 |
| un_ganymede_orbit | Ganymed-Orbit (Highlight) | ORBIT Ganymede | un_ganymede_flyby | 360 |
| un_ganymede_land | Unbemannte Ganymed-Landung (Gate fuer Finale) | LANDED Ganymede | un_ganymede_orbit | 520 |

## Saturn  (>=10 Mond-Flybys, Highlight-Orbits)
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_saturn_flyby | Saturn-Vorbeiflug | FLYBY Saturn | un_jupiter_flyby | 380 |
| un_saturn_orbit | Saturn-Orbit + MITE-Scan | ORBIT Saturn | un_saturn_flyby | 520 |
| un_titan_flyby | Titan-Vorbeiflug | FLYBY Titan | un_saturn_flyby | 220 |
| un_enceladus_flyby | Enceladus-Vorbeiflug | FLYBY Enceladus | un_saturn_flyby | 220 |
| un_rhea_flyby | Rhea-Vorbeiflug | FLYBY Rhea | un_saturn_flyby | 200 |
| un_iapetus_flyby | Iapetus-Vorbeiflug | FLYBY Iapetus | un_saturn_flyby | 200 |
| un_dione_flyby | Dione-Vorbeiflug | FLYBY Dione | un_saturn_flyby | 180 |
| un_tethys_flyby | Tethys-Vorbeiflug | FLYBY Tethys | un_saturn_flyby | 180 |
| un_sat_moon7_flyby | [Mond 7 aus configs] Vorbeiflug | FLYBY [...] | un_saturn_flyby | 180 |
| un_sat_moon8_flyby | [Mond 8 aus configs] Vorbeiflug | FLYBY [...] | un_saturn_flyby | 180 |
| un_sat_moon9_flyby | [Mond 9 aus configs] Vorbeiflug | FLYBY [...] | un_saturn_flyby | 180 |
| un_sat_moon10_flyby | [Mond 10 aus configs] Vorbeiflug | FLYBY [...] | un_saturn_flyby | 180 |
| un_titan_orbit | Titan-Orbit (Highlight) | ORBIT Titan | un_titan_flyby | 460 |
| un_titan_land | Unbemannte Titan-Landung (Atmosphaere) | LANDED Titan | un_titan_orbit | 620 |
| un_enceladus_orbit | Enceladus-Orbit (Highlight) | ORBIT Enceladus | un_enceladus_flyby | 440 |
| un_enceladus_land | Unbemannte Enceladus-Landung | LANDED Enceladus | un_enceladus_orbit | 580 |

## Uranus  (>=10 Mond-Flybys soweit SOL modelliert, Highlight-Orbit)
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_uranus_flyby | Uranus-Vorbeiflug | FLYBY Uranus | un_saturn_orbit | 600 |
| un_titania_flyby | Titania-Vorbeiflug | FLYBY Titania | un_uranus_flyby | 280 |
| un_oberon_flyby | Oberon-Vorbeiflug | FLYBY Oberon | un_uranus_flyby | 280 |
| un_ariel_flyby | Ariel-Vorbeiflug | FLYBY Ariel | un_uranus_flyby | 260 |
| un_umbriel_flyby | Umbriel-Vorbeiflug | FLYBY Umbriel | un_uranus_flyby | 260 |
| un_miranda_flyby | Miranda-Vorbeiflug | FLYBY Miranda | un_uranus_flyby | 260 |
| un_ura_moonN_flyby | [weitere Monde bis 10 aus configs] | FLYBY [...] | un_uranus_flyby | 240 |
| un_uranus_orbit | Uranus-Orbit (Highlight, optional) | ORBIT Uranus | un_uranus_flyby | 780 |
| un_titania_orbit | Titania-Orbit (Highlight) | ORBIT Titania | un_titania_flyby | 600 |

## Neptun  (>=10 Mond-Flybys soweit SOL modelliert, Highlight-Orbit)
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_neptune_flyby | Neptun-Vorbeiflug | FLYBY Neptune | un_uranus_flyby | 680 |
| un_triton_flyby | Triton-Vorbeiflug | FLYBY Triton | un_neptune_flyby | 320 |
| un_nereid_flyby | Nereid-Vorbeiflug | FLYBY Nereid | un_neptune_flyby | 300 |
| un_proteus_flyby | Proteus-Vorbeiflug | FLYBY Proteus | un_neptune_flyby | 300 |
| un_nep_moonN_flyby | [weitere Monde bis 10 aus configs] | FLYBY [...] | un_neptune_flyby | 280 |
| un_neptune_orbit | Neptun-Orbit (Highlight, optional) | ORBIT Neptune | un_neptune_flyby | 850 |
| un_triton_orbit | Triton-Orbit (Highlight) | ORBIT Triton | un_triton_flyby | 700 |
| un_triton_land | Unbemannte Triton-Landung | LANDED Triton | un_triton_orbit | 900 |

## Pluto
| ID | Titel | Bedingung | Voraussetzung | Science |
|---|---|---|---|---|
| un_pluto_flyby | Pluto-Vorbeiflug | FLYBY Pluto | un_neptune_flyby | 800 |
| un_charon_flyby | Charon-Vorbeiflug | FLYBY Charon | un_pluto_flyby | 400 |
| un_pluto_orbit | Pluto-Orbit (optional) | ORBIT Pluto | un_pluto_flyby | 1000 |
| un_pluto_land | Pluto-Landung (optional) | LANDED Pluto | un_pluto_orbit | 1100 |

---

# SPARTE C — NETZWERK / LOGISTIK / MINEN
(unveraendert ggue. v2 — passt)

| ID | Titel | Bedingung | Voraussetzung | Science | Rep |
|---|---|---|---|---|---|
| net_earth_relay1 | Erster Relais-Sat Erdorbit | ORBIT Earth | - | 50 | - |
| net_earth_network | Erd-Relaisnetz (3 Sats) | VESSEL_COUNT_ORBIT Earth >=3 | net_earth_relay1 | 120 | - |
| net_earth_upgrade | Erd-Relaisnetz Upgrade (Hochorbit) | VESSEL_COUNT_ORBIT Earth >=3 (PeA>2000km) | net_earth_network | 150 | JA |
| net_earth_depot | Treibstoffdepot Erdorbit | FUEL_ORBIT Earth | net_earth_relay1 | 110 | - |
| net_luna_relay | Luna-Relaissatellit | ORBIT Luna | net_earth_network | 130 | - |
| net_luna_scan | Ressourcen-Scan Luna | ORBIT Luna | un_luna_orbit | 120 | - |
| net_luna_mine | Erzabbau Luna | ORE_SURFACE Luna | net_luna_scan, un_luna_land | 200 | JA |
| net_luna_depot | Treibstoffdepot Luna-Orbit | FUEL_ORBIT Luna | net_luna_relay | 170 | - |
| net_mars_network | Mars-Relaisnetz (3 Sats) | VESSEL_COUNT_ORBIT Mars >=3 | un_mars_orbit | 260 | - |
| net_mars_scan | Ressourcen-Scan Mars | ORBIT Mars | un_mars_orbit | 240 | - |
| net_mars_mine | Erzabbau Mars | ORE_SURFACE Mars | net_mars_scan, un_mars_land | 340 | JA |
| net_interplanetary_relay | Interplanetares Relais (Sonnenorbit) | ORBIT Sun | net_mars_network | 300 | - |
| net_supply_run | Generische Frachtversorgung | DOCK | net_earth_depot | 130 | JA |
| net_jupiter_relay | Jupiter-System-Relaisnetz (2 Sats) | VESSEL_COUNT_ORBIT Jupiter >=2 | un_jupiter_orbit | 420 | - |
| net_ganymede_scan | Ressourcen-Scan Ganymed | ORBIT Ganymede | un_jupiter_orbit | 400 | - |
| net_ganymede_mine | Erzabbau Ganymed | ORE_SURFACE Ganymede | net_ganymede_scan, un_ganymede_land | 520 | JA |
| net_saturn_relay | Saturn-System-Relaisnetz (2 Sats) | VESSEL_COUNT_ORBIT Saturn >=2 | un_saturn_orbit | 480 | - |
| net_titan_scan | Ressourcen-Scan Titan | ORBIT Titan | un_saturn_orbit | 460 | - |
| net_titan_mine | Erzabbau Titan | ORE_SURFACE Titan | net_titan_scan, un_titan_land | 600 | JA |

---

## Offene Punkte fuer Claude Code (configs-Ordner)
- Interne Body-Namen (`name`) aller Koerper -> Platzhalter ersetzen.
- Asteroiden-Bodys identifizieren -> un_ast_* befuellen.
- Monde pro aeusserem Planeten auf >=10 auffuellen (soweit SOL modelliert; sonst auf
  tatsaechliche Anzahl reduzieren).
- atmosphereDepth-Werte werden zur Laufzeit aus der API gezogen, nicht hardcoden.

## Tuning
- Marker 15 km Standard, 5 km bei Basis-Versorgung/-Rotation.
- Repeatable-Cooldown: >= 2 andere Abschluesse.
- Slot-Limit pro Unterkategorie: 3 (Bemannt/Netzwerk), 5 (Erkundung).
