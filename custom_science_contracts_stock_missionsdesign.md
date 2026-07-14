# WISSENSCHAFT

Das Missionssystem bleibt ein Bonus zum normalen Science-Mode und ersetzt Stock-Science nicht. Die Belohnungen unterstützen den Fortschritt, aber Experimente, Proben, EVA-Berichte und eigene Erkundung bleiben die Hauptquelle für grosse Tech-Sprünge.

Diese Stock-Kerbol-Kampagne ist nicht mehr probe-first. Kerbin, Mun und Minmus beginnen bewusst kerbaliger: frühe Crew-Flüge sind normal und erwünscht. Ab Duna, Eve, Moho, Jool und Eeloo wird die Kampagne wieder strukturierter, weil gefährliche oder entfernte Ziele zuerst robotisch verstanden werden sollen.

Grobe Skala: frühe Kerbin-Missionen 8–150, Mun und Minmus 85–420, innere Planetenziele 140–650, Duna bemannt und Infrastruktur 360–1050, Jool-Monde 280–1350, Laythe-Basis 1350–2600, Eeloo 620–1260, Eve-Rückkehrfinale 3000.

# ABLAUF

Epoche 1 — First Sparks: Die Kampagne startet im echten Stock-Kerbal-Stil mit einem direkten bemannten Start. Danach folgen suborbitaler Flug, erster Orbit, EVA und ein dreitägiger Kerbin-Orbit-Aufenthalt. Frühe unbemannte Kerbin-Missionen sind optionale Wissenschafts- und Lernmissionen und blockieren den Crewpfad nicht.

Epoche 2 — Orbital Habits: Das erste Docking wird nicht als isolierte Demonstration geführt. Stattdessen wird zuerst eine unbemannte Kerbin-Station mit drei Plätzen gestartet, danach dockt ein Crewfahrzeug an. Jede Station wird über 150 Tage getestet. Nach dem ersten 150-Tage-Betrieb der Kerbin-Station öffnet sich die grosse Kampagne. Kerbin bekommt zusätzlich ein dreiteiliges Relay-Netz und später ein Upgrade auf sechs Relays, davon drei polar.

Epoche 3 — Mun or Bust: Mun ist der erste grosse Horizont. Kerbals dürfen nach dem Mun-Orbit direkt landen. Der unbemannte Mun-Orbiter wird parallel zum Crew-Orbit verfügbar und ist nur für Infrastruktur relevant. Rover, Präzisionslandungen und eine polare Präzisionslandung bereiten Station, Basis und Relay-Upgrade vor. Die Mun-Station startet mit drei Kerbals und wächst auf sechs, die Mun-Basis startet mit drei und wächst auf sechs.

Epoche 4 — Minty Operations: Minmus öffnet nach der ersten Mun-Basis. Es gibt Flyby, Landung, Präzisionslandung, Relay-Netz und eine Fuel-Base. Diese Minmus-Fuel-Base ist ein Pflichtziel innerhalb des Infrastrukturzweigs, blockiert aber keine Hauptprogression.

Epoche 5 — Inner Mischief: Eve, Gilly, Moho und Dres öffnen als innere Neben- und Vorbereitungspfade. Eve-Orbit und Eve-Atmosphärensonde öffnen gleichzeitig nach dem Eve-Flyby; nur die Atmosphärensonde ist Pflicht für den unbemannten Eve-Lander. Crew bekommt zunächst Eve-Orbit sowie Gilly-Flyby und Gilly-Landung. Moho erhält die Mohole-Suche am Nordpol. Dres bleibt optional und blockiert nichts.

Epoche 6 — Red Dust: Duna ist der erste ernste interplanetare Hauptbogen. Robotische Flyby-, Orbit-, Lander- und Rover-Missionen bereiten die Crew-Landung vor. Die Duna-Orbitstation kommt bewusst erst nach der ersten Duna-Landung, startet mit zwei Kerbals und wächst bis acht. Nach dem ersten Upgrade auf vier Kerbals kann die Deep-Space-Phase beginnen. Duna bekommt zusätzlich ein Relay-Netz und eine Surface Base.

Epoche 7 — Deep-Space Lifeline: Der interplanetare Relay-Ring in Sonnenorbit öffnet Jool und Eeloo gleichzeitig. Eeloo bekommt Flyby, optionalen unbemannten Lander und danach eine direkte Crew-Landung ohne Crew-Flyby und ohne Crew-Orbit.

Epoche 8 — Jool Frontier: Jool beginnt ohne unbemannten Flyby, aber mit Atmosphäreneintritt und unbemanntem Jool-Orbiter. Der Jool-Orbiter öffnet die Mondoperationen. Jeder Mond braucht seinen eigenen unbemannten Flyby, danach darf die Crew direkt landen. Laythe braucht zusätzlich einen Atmosphärenlander, das Laythe-Relay-Netz, das Jool-Relay-Netz sowie die ausgebauten Kerbin- und Mun-Relay-Netze mit je sechs Satelliten, davon drei polar. Die Laythe-Basis wächst bis auf 15 Kerbals. Tylo bekommt einen einmaligen 3-Kerbal-Challenge-Outpost ohne Ausbau und ohne Resupply.

Epoche 9 — The Purple Finale: Das Finale ist die bemannte Eve-Landung mit Rückkehr nach Kerbin. Voraussetzung sind die leistungsfähige Eve-Supportstation, die unbemannte Gilly-Fuel-Station und die bewiesene Laythe-Landung. Danach folgt als letzter absurder Bonus eine kleine Eve-Basis mit der Frage: are we living here now?

# STATIONSKETTEN

chain: body=Kerbin | key=kerbin_station | typ=station | prereq=st_kerbin_station_core3 | stufen=3,4,6,8,10
chain: body=Kerbin | key=kerbin_fuel_depot | typ=station | prereq=dep_kerbin_fuel_depot_core | stufen=1
chain: body=Mun | key=mun_station | typ=station | prereq=st_mun_station_core3 | stufen=3,6
chain: body=Mun | key=mun_base | typ=base | prereq=base_mun_base3 | stufen=3,6
chain: body=Minmus | key=minmus_base | typ=base | prereq=base_minmus_fuel_base | stufen=3
chain: body=Duna | key=duna_station | typ=station | prereq=st_duna_station_core2 | stufen=2,4,6,8
chain: body=Duna | key=duna_base | typ=base | prereq=base_duna_base2 | stufen=2,4,6
chain: body=Eve | key=eve_support_station | typ=station | prereq=st_eve_support_station | stufen=3
chain: body=Eve | key=eve_surface_base | typ=base | prereq=base_eve_are_we_living_here_now | stufen=3
chain: body=Gilly | key=gilly_fuel_station | typ=base | prereq=un_gilly_fuel_station | stufen=1
chain: body=Jool | key=jool_gateway | typ=station | prereq=st_jool_gateway | stufen=3
chain: body=Laythe | key=laythe_base | typ=base | prereq=base_laythe_base3 | stufen=3,6,10,15

# MISSIONEN

=== MISSION ===
id: cr_kerbin_first_launch
title: The First Button Press
sparte: Pioniere
body: Kerbin
epoche: 1
prereq: -
reward: 8
repeatable: no
recordStation: -
stationRef: -
beschreibung: Launch the first Kerbal vehicle from Kerbin. It does not need to go far, high, or in the planned direction. The important part is that someone bravely sat on top of it and the space program now has momentum.
beschreibung_en: Launch the first Kerbal vehicle from Kerbin. It does not need to go far, high, or in the planned direction. The important part is that someone bravely sat on top of it and the space program now has momentum.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: ATMO_FRACTION Kerbin 1 10 | clear the pad and reach the lower atmosphere
=== MISSION ===
id: cr_kerbin_suborbital
title: First Hop
sparte: Pioniere
body: Kerbin
epoche: 1
prereq: cr_kerbin_first_launch
reward: 18
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a Kerbal above the atmosphere on a short suborbital hop. The flight proves that Kerbals can leave Kerbin, look outside, panic briefly, and still come home useful.
beschreibung_en: Send a Kerbal above the atmosphere on a short suborbital hop. The flight proves that Kerbals can leave Kerbin, look outside, panic briefly, and still come home useful.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: SUBORBITAL Kerbin | suborbital spaceflight over Kerbin
=== MISSION ===
id: cr_kerbin_orbit
title: Sideways Counts
sparte: Pioniere
body: Kerbin
epoche: 1
prereq: cr_kerbin_suborbital
reward: 36
repeatable: no
recordStation: -
stationRef: -
beschreibung: Put a Kerbal into stable orbit around Kerbin. This is the moment Mission Control realizes the program is no longer just launching brave volunteers upward, but also sideways.
beschreibung_en: Put a Kerbal into stable orbit around Kerbin. This is the moment Mission Control realizes the program is no longer just launching brave volunteers upward, but also sideways.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: cr_kerbin_eva
title: Open the Hatch
sparte: Pioniere
body: Kerbin
epoche: 1
prereq: cr_kerbin_orbit
reward: 48
repeatable: no
recordStation: -
stationRef: -
beschreibung: Let a Kerbal leave the spacecraft in Kerbin orbit. The official reason is EVA training. The unofficial reason is that someone wanted to know whether the hatch really opens.
beschreibung_en: Let a Kerbal leave the spacecraft in Kerbin orbit. The official reason is EVA training. The unofficial reason is that someone wanted to know whether the hatch really opens.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: EVA Kerbin ORBITING | EVA in Kerbin orbit
=== MISSION ===
id: cr_kerbin_orbit_3d
title: Three Days, Mostly Fine
sparte: Pioniere
body: Kerbin
epoche: 1
prereq: cr_kerbin_eva
reward: 64
repeatable: no
recordStation: -
stationRef: -
beschreibung: Keep a crew in Kerbin orbit for three days. The mission proves that Kerbals can live in space long enough to get bored, start experiments, and label random panels as important.
beschreibung_en: Keep a crew in Kerbin orbit for three days. The mission proves that Kerbals can live in space long enough to get bored, start experiments, and label random panels as important.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 3 | remain in orbit for 3 days
=== MISSION ===
id: un_kerbin_sounding_probe
title: The Sensible Probe, Late
sparte: Robotische Erkunder
body: Kerbin
epoche: 1
prereq: cr_kerbin_first_launch
reward: 10
repeatable: no
recordStation: -
stationRef: -
beschreibung: Launch an uncrewed test probe into Kerbin’s atmosphere. It is cheaper than risking a Kerbal, which is exactly why the space program only thought of it after risking a Kerbal.
beschreibung_en: Launch an uncrewed test probe into Kerbin’s atmosphere. It is cheaper than risking a Kerbal, which is exactly why the space program only thought of it after risking a Kerbal.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Kerbin 60 90 | reach the upper atmosphere
=== MISSION ===
id: un_kerbin_first_satellite
title: Beep Goes the Future
sparte: Robotische Erkunder
body: Kerbin
epoche: 1
prereq: cr_kerbin_orbit
reward: 28
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place the first uncrewed satellite in Kerbin orbit. It cannot wave at the cameras, but it can beep, and beeping is science.
beschreibung_en: Place the first uncrewed satellite in Kerbin orbit. It cannot wave at the cameras, but it can beep, and beeping is science.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 1 | operate for 1 day
=== MISSION ===
id: un_kerbin_polar_satellite
title: Over the Awkward Bits
sparte: Robotische Erkunder
body: Kerbin
epoche: 1
prereq: un_kerbin_first_satellite
reward: 38
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place an uncrewed satellite into a polar Kerbin orbit. The new path covers all the awkward places the first satellite missed while pretending the equator was enough.
beschreibung_en: Place an uncrewed satellite into a polar Kerbin orbit. The new path covers all the awkward places the first satellite missed while pretending the equator was enough.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Kerbin 250 | periapsis above 250 km
check: INCLINATION_MIN Kerbin 75 | orbit inclination above 75 degrees
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: st_kerbin_station_core3
title: Empty Room in Space
sparte: Stationen
body: Kerbin
epoche: 2
prereq: cr_kerbin_orbit_3d
reward: 90
repeatable: no
recordStation: kerbin_station
stationRef: -
beschreibung: Launch an uncrewed station core into Kerbin orbit with room for three Kerbals. Nobody is aboard yet, which gives engineers a rare chance to discover problems before they become conversations.
beschreibung_en: Launch an uncrewed station core into Kerbin orbit with room for three Kerbals. Nobody is aboard yet, which gives engineers a rare chance to discover problems before they become conversations.
check: CREW_NONE | uncrewed station core
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 10 | keep the empty station stable for 10 days
check: CREW_CAPACITY_MIN 3 | station has room for at least 3 Kerbals
=== MISSION ===
id: st_kerbin_station_crew3
title: Moving Day with Docking
sparte: Stationen
body: Kerbin
epoche: 2
prereq: st_kerbin_station_core3
reward: 105
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Dock a crew vehicle to the new Kerbin station and bring three Kerbals aboard. This is the program’s first real docking operation, carefully disguised as moving in.
beschreibung_en: Dock a crew vehicle to the new Kerbin station and bring three Kerbals aboard. This is the program’s first real docking operation, carefully disguised as moving in.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DOCK_ANY | dock a crew vehicle to the station
=== MISSION ===
id: st_kerbin_station_longstay3
title: 150 Days of Snacks
sparte: Stationen
body: Kerbin
epoche: 2
prereq: st_kerbin_station_crew3
reward: 150
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Keep three Kerbals aboard the Kerbin station for 150 days. If the station still has air, snacks, power, and roughly the same number of parts at the end, it counts as a success.
beschreibung_en: Keep three Kerbals aboard the Kerbin station for 150 days. If the station still has air, snacks, power, and roughly the same number of parts at the end, it counts as a success.
check: CREW_MIN 3 | at least 3 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: rep_kerbin_station_resupply
title: Snacks Around Home
sparte: Stationen
body: Kerbin
epoche: 2
prereq: st_kerbin_station_longstay3
reward: 45
repeatable: yes
recordStation: -
stationRef: kerbin_station
beschreibung: Deliver an uncrewed supply vehicle to the Kerbin station. It keeps the first real station alive and gives Mission Control another docking checklist to misplace.
beschreibung_en: Deliver an uncrewed supply vehicle to the Kerbin station. It keeps the first real station alive and gives Mission Control another docking checklist to misplace.
check: CREW_NONE | uncrewed supply vehicle
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DOCK_STATION kerbin_station Kerbin | dock with the Kerbin station
=== MISSION ===
id: st_kerbin_station_upgrade4
title: One More Seat
sparte: Stationen
body: Kerbin
epoche: 3
prereq: st_kerbin_station_longstay3
reward: 120
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 4 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
beschreibung_en: Expand the Kerbin station so 4 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 4 | station has room for at least 4 Kerbals
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_kerbin_station_longstay4
title: Four Kerbals, Still Fine
sparte: Stationen
body: Kerbin
epoche: 3
prereq: st_kerbin_station_upgrade4
reward: 165
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the Kerbin station with 4 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
beschreibung_en: Operate the Kerbin station with 4 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
check: CREW_MIN 4 | at least 4 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: st_kerbin_station_upgrade6
title: Room for Six
sparte: Stationen
body: Kerbin
epoche: 4
prereq: st_kerbin_station_longstay4
reward: 160
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 6 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
beschreibung_en: Expand the Kerbin station so 6 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 6 | station has room for at least 6 Kerbals
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_kerbin_station_longstay6
title: Crowded but Scientific
sparte: Stationen
body: Kerbin
epoche: 4
prereq: st_kerbin_station_upgrade6
reward: 205
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the Kerbin station with 6 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
beschreibung_en: Operate the Kerbin station with 6 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
check: CREW_MIN 6 | at least 6 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: st_kerbin_station_upgrade8
title: The Big Can
sparte: Stationen
body: Kerbin
epoche: 5
prereq: st_kerbin_station_longstay6
reward: 210
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 8 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
beschreibung_en: Expand the Kerbin station so 8 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 8 | station has room for at least 8 Kerbals
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_kerbin_station_longstay8
title: Eight Up, None Down
sparte: Stationen
body: Kerbin
epoche: 5
prereq: st_kerbin_station_upgrade8
reward: 255
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the Kerbin station with 8 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
beschreibung_en: Operate the Kerbin station with 8 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
check: CREW_MIN 8 | at least 8 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: st_kerbin_station_upgrade10
title: Kerbin Hotel Ten
sparte: Stationen
body: Kerbin
epoche: 6
prereq: st_kerbin_station_longstay8
reward: 270
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 10 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
beschreibung_en: Expand the Kerbin station so 10 Kerbals can live there. The extra capacity is officially for science, although Mission Control admits it also helps when everyone brings too many snacks.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 10 | station has room for at least 10 Kerbals
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_kerbin_station_longstay10
title: The Ten-Kerbal Test
sparte: Stationen
body: Kerbin
epoche: 6
prereq: st_kerbin_station_upgrade10
reward: 315
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the Kerbin station with 10 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
beschreibung_en: Operate the Kerbin station with 10 Kerbals for 150 days. Bigger crews mean bigger science, bigger checklists, and more people insisting the strange noise was already there.
check: CREW_MIN 10 | at least 10 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: net_kerbin_relay3
title: Three Beeps Around Home
sparte: Versorgungsnetz
body: Kerbin
epoche: 2
prereq: un_kerbin_first_satellite
reward: 70
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Kerbin orbit. The network gives Mission Control a reliable way to talk to spacecraft, probes, stations, and Kerbals who claim they were just testing the controls.
beschreibung_en: Place three relay-capable satellites in Kerbin orbit. The network gives Mission Control a reliable way to talk to spacecraft, probes, stations, and Kerbals who claim they were just testing the controls.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Kerbin 3 1000 | 3 relay-capable satellites in Kerbin orbit above 1000 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: net_kerbin_relay6_polar
title: Polar Calls Included
sparte: Versorgungsnetz
body: Kerbin
epoche: 2
prereq: net_kerbin_relay3, un_kerbin_polar_satellite
reward: 110
repeatable: no
recordStation: -
stationRef: -
beschreibung: Expand the Kerbin relay network to six satellites, with three of them in polar orbits. This closes the awkward communication gaps that previously occurred whenever a mission flew somewhere inconvenient.
beschreibung_en: Expand the Kerbin relay network to six satellites, with three of them in polar orbits. This closes the awkward communication gaps that previously occurred whenever a mission flew somewhere inconvenient.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Kerbin 6 1000 | 6 relay-capable satellites in Kerbin orbit above 1000 km
check: RELAY_VESSEL_COUNT_INCLINATION Kerbin 3 75 1000 | 3 relay-capable satellites in polar Kerbin orbit
check: DURATION 1 | upgraded network remains active for 1 day
=== MISSION ===
id: dep_kerbin_fuel_depot_core
title: Gas Station Above Kerbin
sparte: Stationen
body: Kerbin
epoche: 2
prereq: st_kerbin_station_longstay3
reward: 95
repeatable: no
recordStation: kerbin_fuel_depot
stationRef: -
beschreibung: Place an uncrewed fuel depot in Kerbin orbit. It is not required for every mission, but it makes the program look much more prepared than it usually is.
beschreibung_en: Place an uncrewed fuel depot in Kerbin orbit. It is not required for every mission, but it makes the program look much more prepared than it usually is.
check: CREW_NONE | uncrewed fuel depot
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: FUEL_MIN 1000 | fuel reserves above 1000
=== MISSION ===
id: dep_kerbin_fuel_delivery
title: Top Off the Tin Can
sparte: Stationen
body: Kerbin
epoche: 2
prereq: dep_kerbin_fuel_depot_core
reward: 45
repeatable: yes
recordStation: -
stationRef: kerbin_fuel_depot
beschreibung: Deliver fuel to the Kerbin orbit depot. The accountants call it logistics; the pilots call it another excuse to rendezvous with something flammable.
beschreibung_en: Deliver fuel to the Kerbin orbit depot. The accountants call it logistics; the pilots call it another excuse to rendezvous with something flammable.
check: CREW_NONE | uncrewed delivery vehicle
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: FUEL_MIN 800 | deliver at least 800 fuel
check: DOCK_STATION kerbin_fuel_depot Kerbin | dock with the Kerbin fuel depot
=== MISSION ===
id: cr_mun_flyby
title: Wave at the Mun
sparte: Pioniere
body: Mun
epoche: 3
prereq: st_kerbin_station_longstay3
reward: 95
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past the Mun and bring them home. They do not land yet, but they get close enough for everyone to agree that the next idea is obvious and probably dangerous.
beschreibung_en: Send Kerbals past the Mun and bring them home. They do not land yet, but they get close enough for everyone to agree that the next idea is obvious and probably dangerous.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: FLYBY Mun 800 | fly past the Mun below 800 km
check: RETURN_FROM_BODY Mun Kerbin flyby | return the crew safely to Kerbin
=== MISSION ===
id: cr_mun_orbit
title: Circling the Grey Problem
sparte: Pioniere
body: Mun
epoche: 3
prereq: cr_mun_flyby
reward: 125
repeatable: no
recordStation: -
stationRef: -
beschreibung: Put Kerbals into orbit around the Mun. From here, the surface looks close, quiet, and suspiciously landable.
beschreibung_en: Put Kerbals into orbit around the Mun. From here, the surface looks close, quiet, and suspiciously landable.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: HOLD 10 | hold stable Mun orbit for 10 seconds
=== MISSION ===
id: un_mun_orbiter
title: Maps Before Maybe
sparte: Robotische Erkunder
body: Mun
epoche: 3
prereq: cr_mun_orbit
reward: 90
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send an uncrewed orbiter to the Mun. It maps the surface, studies landing zones, and politely arrives at the same time the crew is already proving that maps would have been useful.
beschreibung_en: Send an uncrewed orbiter to the Mun. It maps the surface, studies landing zones, and politely arrives at the same time the crew is already proving that maps would have been useful.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: HOLD 10 | hold stable Mun orbit for 10 seconds
=== MISSION ===
id: cr_mun_landing
title: Boots, Flags, No Plan B
sparte: Pioniere
body: Mun
epoche: 3
prereq: cr_mun_orbit
reward: 190
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on the Mun and let them step outside. The program finally reaches another world the traditional Kerbal way: with courage, math, and a landing site selected mostly by optimism.
beschreibung_en: Land Kerbals on the Mun and let them step outside. The program finally reaches another world the traditional Kerbal way: with courage, math, and a landing site selected mostly by optimism.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Mun | landed on the Mun
check: EVA Mun LANDED | EVA on the Mun
check: RETURN_FROM_BODY Mun Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_mun_rover
title: Find a Flat-ish Spot
sparte: Robotische Erkunder
body: Mun
epoche: 3
prereq: cr_mun_landing
reward: 120
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Land a rover on the Mun and drive it across the surface. Its mission is to find a future base site that is flat, useful, photogenic, and not currently occupied by debris from earlier confidence.
beschreibung_en: Land a rover on the Mun and drive it across the surface. Its mission is to find a future base site that is flat, useful, photogenic, and not currently occupied by debris from earlier confidence.
check: CREW_NONE | uncrewed rover
check: LANDED Mun | landed on the Mun
check: WHEEL_MOTION Mun 4 | drive the rover on the surface at 4 m/s or faster
=== MISSION ===
id: un_mun_precision_rover_landing
title: Park the Scout Exactly There
sparte: Robotische Erkunder
body: Mun
epoche: 3
prereq: un_mun_rover
reward: 150
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Land a rover precisely at the chosen Mun base site. Mission Control insists this exact spot matters because the rover found excellent terrain, which mostly means it did not immediately roll away.
beschreibung_en: Land a rover precisely at the chosen Mun base site. Mission Control insists this exact spot matters because the rover found excellent terrain, which mostly means it did not immediately roll away.
check: CREW_NONE | uncrewed rover
check: LANDED Mun | landed on the Mun
check: MARKER_LANDING Mun 5 | land within 5 km of the marked base site
check: WHEEL_MOTION Mun 4 | drive the rover on the surface at 4 m/s or faster
=== MISSION ===
id: cr_mun_precision_landing
title: Land Near the Wheels
sparte: Pioniere
body: Mun
epoche: 3
prereq: cr_mun_landing, un_mun_precision_rover_landing
reward: 210
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals near the scouting rover. This proves the program can aim for a planned site instead of discovering the landing zone only after touching it.
beschreibung_en: Land Kerbals near the scouting rover. This proves the program can aim for a planned site instead of discovering the landing zone only after touching it.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Mun | landed on the Mun
check: MARKER_LANDING Mun 5 | land within 5 km of the marked site
check: RETURN_FROM_BODY Mun Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: cr_mun_polar_precision_landing
title: Pole Position
sparte: Pioniere
body: Mun
epoche: 3
prereq: cr_mun_landing, un_mun_orbiter
reward: 230
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals near one of the Mun’s poles. The site was chosen for science, lighting conditions, and because someone in Mission Control drew a circle there and now refuses to erase it.
beschreibung_en: Land Kerbals near one of the Mun’s poles. The site was chosen for science, lighting conditions, and because someone in Mission Control drew a circle there and now refuses to erase it.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Mun | landed on the Mun
check: MARKER_LANDING Mun 8 70 90 | land near a marked polar site
check: RETURN_FROM_BODY Mun Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: net_mun_relay3
title: Mun Says Hello
sparte: Versorgungsnetz
body: Mun
epoche: 3
prereq: cr_mun_landing, un_mun_orbiter
reward: 115
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Mun orbit. The Mun is close enough to visit, but far enough away that shouting is no longer an approved communications method.
beschreibung_en: Place three relay-capable satellites in Mun orbit. The Mun is close enough to visit, but far enough away that shouting is no longer an approved communications method.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Mun 3 250 | 3 relay-capable satellites in Mun orbit above 250 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: net_mun_relay6_polar
title: Mun Polar Party Line
sparte: Versorgungsnetz
body: Mun
epoche: 3
prereq: net_mun_relay3, cr_mun_polar_precision_landing
reward: 160
repeatable: no
recordStation: -
stationRef: -
beschreibung: Expand the Mun relay network to six satellites, with three in polar orbits. This gives the Mun proper coverage, including the awkward places Kerbals keep choosing for dramatic landings.
beschreibung_en: Expand the Mun relay network to six satellites, with three in polar orbits. This gives the Mun proper coverage, including the awkward places Kerbals keep choosing for dramatic landings.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Mun 6 250 | 6 relay-capable satellites in Mun orbit above 250 km
check: RELAY_VESSEL_COUNT_INCLINATION Mun 3 75 250 | 3 relay-capable satellites in polar Mun orbit
check: DURATION 1 | upgraded network remains active for 1 day
=== MISSION ===
id: st_mun_station_core3
title: Gateway-ish Around Mun
sparte: Stationen
body: Mun
epoche: 3
prereq: cr_mun_precision_landing, net_mun_relay3
reward: 180
repeatable: no
recordStation: mun_station
stationRef: -
beschreibung: Launch a three-Kerbal station into Mun orbit. It gives landers a place to dock, crews a place to regroup, and Mission Control a place to point at when claiming this was all planned.
beschreibung_en: Launch a three-Kerbal station into Mun orbit. It gives landers a place to dock, crews a place to regroup, and Mission Control a place to point at when claiming this was all planned.
check: CREW_NONE | uncrewed station core
check: ORBIT_ABOVE Mun 20 | stable Mun orbit
check: DURATION 10 | keep the empty station stable for 10 days
check: CREW_CAPACITY_MIN 3 | station has room for at least 3 Kerbals
=== MISSION ===
id: st_mun_station_crew3
title: Mun Station Move-In
sparte: Stationen
body: Mun
epoche: 3
prereq: st_mun_station_core3
reward: 195
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Bring three Kerbals to the Mun station. The station becomes a real outpost once someone is aboard to notice what the engineers forgot.
beschreibung_en: Bring three Kerbals to the Mun station. The station becomes a real outpost once someone is aboard to notice what the engineers forgot.
check: CREW_MIN 3 | at least 3 Kerbals aboard the station
check: ORBIT_ABOVE Mun 20 | stable Mun orbit
check: DOCK_ANY | dock with the Mun station
=== MISSION ===
id: st_mun_station_longstay3
title: Grey Orbit Residency
sparte: Stationen
body: Mun
epoche: 3
prereq: st_mun_station_crew3
reward: 260
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Operate the Mun station with three Kerbals for 150 days. The crew learns how to live around another world without constantly looking down and asking whether they left the lander lights on.
beschreibung_en: Operate the Mun station with three Kerbals for 150 days. The crew learns how to live around another world without constantly looking down and asking whether they left the lander lights on.
check: CREW_MIN 3 | at least 3 Kerbals aboard the station
check: ORBIT_ABOVE Mun 20 | stable Mun orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: rep_mun_station_resupply
title: Grey Orbit Supply Run
sparte: Stationen
body: Mun
epoche: 3
prereq: st_mun_station_longstay3
reward: 90
repeatable: yes
recordStation: -
stationRef: mun_station
beschreibung: Deliver an uncrewed supply vehicle to the Mun orbital station. The Mun may be close, but the station still appreciates spare parts and snacks that are not dust-flavored.
beschreibung_en: Deliver an uncrewed supply vehicle to the Mun orbital station. The Mun may be close, but the station still appreciates spare parts and snacks that are not dust-flavored.
check: CREW_NONE | uncrewed supply vehicle
check: ORBIT_ABOVE Mun 20 | stable Mun orbit
check: DOCK_STATION mun_station Mun | dock with the Mun orbital station
=== MISSION ===
id: st_mun_station_upgrade6
title: More Room Over Mun
sparte: Stationen
body: Mun
epoche: 4
prereq: st_mun_station_longstay3
reward: 280
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Expand the Mun station to support six Kerbals. The extra capacity turns it from a heroic outpost into something dangerously close to a workplace.
beschreibung_en: Expand the Mun station to support six Kerbals. The extra capacity turns it from a heroic outpost into something dangerously close to a workplace.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 6 | station has room for at least 6 Kerbals
check: ORBIT_ABOVE Mun 20 | stable Mun orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_mun_station_longstay6
title: Six Around the Mun
sparte: Stationen
body: Mun
epoche: 4
prereq: st_mun_station_upgrade6
reward: 340
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Operate the six-Kerbal Mun station for 150 days. At this scale, the Mun has stopped being a visit and started becoming a schedule.
beschreibung_en: Operate the six-Kerbal Mun station for 150 days. At this scale, the Mun has stopped being a visit and started becoming a schedule.
check: CREW_MIN 6 | at least 6 Kerbals aboard the station
check: ORBIT_ABOVE Mun 20 | stable Mun orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: base_mun_base3
title: Not Just Footprints
sparte: Stationen
body: Mun
epoche: 3
prereq: st_mun_station_longstay3, cr_mun_precision_landing
reward: 320
repeatable: no
recordStation: mun_base
stationRef: -
beschreibung: Build the first Mun base and support three Kerbals on the surface. The base starts small, but it proves the Mun can be more than a place for footprints, flags, and suspiciously abandoned descent stages.
beschreibung_en: Build the first Mun base and support three Kerbals on the surface. The base starts small, but it proves the Mun can be more than a place for footprints, flags, and suspiciously abandoned descent stages.
check: CREW_MIN 3 | at least 3 Kerbals at the base
check: LANDED Mun | base landed on the Mun
check: DURATION 30 | operate the base for 30 days
=== MISSION ===
id: rep_mun_base_supply
title: Grey Dust Delivery
sparte: Stationen
body: Mun
epoche: 3
prereq: base_mun_base3
reward: 75
repeatable: yes
recordStation: -
stationRef: mun_base
beschreibung: Deliver supplies to the Mun base. The base crew appreciates fuel, spare parts, and anything that was not accidentally labeled as decorative rock.
beschreibung_en: Deliver supplies to the Mun base. The base crew appreciates fuel, spare parts, and anything that was not accidentally labeled as decorative rock.
check: CREW_NONE | uncrewed supply vehicle
check: LANDED Mun | landed on the Mun
check: RESOURCE_DELIVERY mun_base Fuel 300 2 | deliver at least 300 fuel within 2 km of the Mun base
=== MISSION ===
id: base_mun_base6
title: Mun Base, Now With Neighbours
sparte: Stationen
body: Mun
epoche: 4
prereq: base_mun_base3, st_mun_station_upgrade6
reward: 420
repeatable: no
recordStation: -
stationRef: mun_base
beschreibung: Expand the Mun base to support six Kerbals. The settlement now has enough crew to do real science, real maintenance, and real arguments about who parked the rover upside down.
beschreibung_en: Expand the Mun base to support six Kerbals. The settlement now has enough crew to do real science, real maintenance, and real arguments about who parked the rover upside down.
check: CREW_MIN 6 | at least 6 Kerbals at the base
check: LANDED Mun | base landed on the Mun
check: DURATION 60 | operate the expanded base for 60 days
=== MISSION ===
id: cr_minmus_flyby
title: Minty Flyby
sparte: Pioniere
body: Minmus
epoche: 4
prereq: base_mun_base3
reward: 130
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Minmus. The moon looks small, minty, and harmless, which is exactly the kind of description that has caused trouble before.
beschreibung_en: Send Kerbals past Minmus. The moon looks small, minty, and harmless, which is exactly the kind of description that has caused trouble before.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: FLYBY Minmus 500 | fly past Minmus below 500 km
check: RETURN_FROM_BODY Minmus Kerbin flyby | return the crew safely to Kerbin
=== MISSION ===
id: cr_minmus_landing
title: Touch the Dessert Moon
sparte: Pioniere
body: Minmus
epoche: 4
prereq: cr_minmus_flyby
reward: 175
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Minmus. The low gravity makes every step feel heroic, ridiculous, and slightly too easy.
beschreibung_en: Land Kerbals on Minmus. The low gravity makes every step feel heroic, ridiculous, and slightly too easy.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Minmus | landed on Minmus
check: EVA Minmus LANDED | EVA on Minmus
check: RETURN_FROM_BODY Minmus Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: cr_minmus_precision_landing
title: Land on the Good Mint
sparte: Pioniere
body: Minmus
epoche: 4
prereq: cr_minmus_landing
reward: 210
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land precisely on Minmus at the planned site. The terrain is flat, the gravity is forgiving, and somehow the navigation team still managed to make this sound difficult.
beschreibung_en: Land precisely on Minmus at the planned site. The terrain is flat, the gravity is forgiving, and somehow the navigation team still managed to make this sound difficult.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Minmus | landed on Minmus
check: MARKER_LANDING Minmus 5 | land within 5 km of the marked site
check: RETURN_FROM_BODY Minmus Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: net_minmus_relay3
title: Minty Signal Triangle
sparte: Versorgungsnetz
body: Minmus
epoche: 4
prereq: cr_minmus_landing
reward: 135
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Minmus orbit. This ensures that future fuel operations can report success, failure, or that the tanker is slowly drifting away.
beschreibung_en: Place three relay-capable satellites in Minmus orbit. This ensures that future fuel operations can report success, failure, or that the tanker is slowly drifting away.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Minmus 3 150 | 3 relay-capable satellites in Minmus orbit above 150 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: base_minmus_fuel_base
title: Fuel from the Flats
sparte: Stationen
body: Minmus
epoche: 4
prereq: cr_minmus_precision_landing, net_minmus_relay3
reward: 300
repeatable: no
recordStation: minmus_base
stationRef: -
beschreibung: Build a Minmus surface base with fuel equipment for future exploration. It does not block the main campaign, but it gives the program a practical refueling foothold in Kerbin’s backyard.
beschreibung_en: Build a Minmus surface base with fuel equipment for future exploration. It does not block the main campaign, but it gives the program a practical refueling foothold in Kerbin’s backyard.
check: CREW_MIN 3 | at least 3 Kerbals at the fuel base
check: LANDED Minmus | base landed on Minmus
check: ORE_SURFACE Minmus | Ore present at the fuel site
check: FUEL_MIN 600 | fuel equipment stores at least 600 fuel
check: DURATION 30 | operate the fuel base for 30 days
=== MISSION ===
id: un_eve_flyby
title: Purple Warning Shot
sparte: Robotische Erkunder
body: Eve
epoche: 5
prereq: cr_mun_landing, st_kerbin_station_longstay3
reward: 150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Eve. The planet looks beautiful, purple, and completely unwilling to let anything leave again.
beschreibung_en: Send a probe past Eve. The planet looks beautiful, purple, and completely unwilling to let anything leave again.
check: CREW_NONE | uncrewed vessel
check: FLYBY Eve 5000 | fly past Eve below 5000 km
=== MISSION ===
id: un_eve_orbit
title: Orbit the Trap
sparte: Robotische Erkunder
body: Eve
epoche: 5
prereq: un_eve_flyby
reward: 210
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place a probe into Eve orbit. From above, the planet looks calm enough to fool new engineers and experienced enough to terrify old ones.
beschreibung_en: Place a probe into Eve orbit. From above, the planet looks calm enough to fool new engineers and experienced enough to terrify old ones.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Eve | stable Eve orbit
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: un_eve_atmo_probe
title: Taste the Soup
sparte: Robotische Erkunder
body: Eve
epoche: 5
prereq: un_eve_flyby
reward: 220
repeatable: no
recordStation: -
stationRef: -
beschreibung: Drop a probe into Eve’s atmosphere and collect data while it descends. The probe is not expected to enjoy the experience.
beschreibung_en: Drop a probe into Eve’s atmosphere and collect data while it descends. The probe is not expected to enjoy the experience.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Eve 60 90 | enter Eve upper atmosphere
=== MISSION ===
id: un_eve_lander
title: Down Is Easy
sparte: Robotische Erkunder
body: Eve
epoche: 5
prereq: un_eve_atmo_probe
reward: 290
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Eve. Reaching the surface is the easy part, which is exactly the sort of sentence that should worry everyone.
beschreibung_en: Land a probe on Eve. Reaching the surface is the easy part, which is exactly the sort of sentence that should worry everyone.
check: CREW_NONE | uncrewed vessel
check: LANDED Eve | landed on Eve
=== MISSION ===
id: net_eve_relay3
title: Purple Phone Network
sparte: Versorgungsnetz
body: Eve
epoche: 5
prereq: un_eve_orbit
reward: 240
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Eve orbit. Every future Eve mission needs strong communications, mostly so Mission Control can say we are still evaluating options in real time.
beschreibung_en: Place three relay-capable satellites in Eve orbit. Every future Eve mission needs strong communications, mostly so Mission Control can say we are still evaluating options in real time.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Eve 3 1000 | 3 relay-capable satellites in Eve orbit above 1000 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: cr_eve_orbit
title: Look, Do Not Land
sparte: Pioniere
body: Eve
epoche: 5
prereq: un_eve_orbit, st_kerbin_station_longstay3
reward: 420
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals into orbit around Eve. They are close enough to see the surface clearly and far enough away to still believe they can go home.
beschreibung_en: Send Kerbals into orbit around Eve. They are close enough to see the surface clearly and far enough away to still believe they can go home.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: ORBIT_ABOVE Eve | stable Eve orbit
check: DURATION 5 | remain in Eve orbit for 5 days
=== MISSION ===
id: cr_gilly_flyby
title: Tiny Moon, Big Shrug
sparte: Pioniere
body: Gilly
epoche: 9
prereq: cr_eve_orbit
reward: 260
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Gilly. It is small, strange, and may technically count as a moon if everyone agrees not to ask too many questions.
beschreibung_en: Send Kerbals past Gilly. It is small, strange, and may technically count as a moon if everyone agrees not to ask too many questions.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: FLYBY Gilly 100 | fly past Gilly below 100 km
=== MISSION ===
id: cr_gilly_landing
title: Hold On to Gilly
sparte: Pioniere
body: Gilly
epoche: 9
prereq: cr_gilly_flyby
reward: 320
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Gilly. The gravity is so low that staying landed is almost more impressive than arriving.
beschreibung_en: Land Kerbals on Gilly. The gravity is so low that staying landed is almost more impressive than arriving.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Gilly | landed on Gilly
check: EVA Gilly LANDED | EVA on Gilly
check: RETURN_FROM_BODY Gilly Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_gilly_fuel_station
title: Fuel Pebble
sparte: Stationen
body: Gilly
epoche: 9
prereq: cr_gilly_landing, un_eve_lander
reward: 260
repeatable: no
recordStation: gilly_fuel_station
stationRef: -
beschreibung: Place an uncrewed fuel station at Gilly. The little moon becomes Eve’s practical support rock, which is more responsibility than anyone expected from something so tiny.
beschreibung_en: Place an uncrewed fuel station at Gilly. The little moon becomes Eve’s practical support rock, which is more responsibility than anyone expected from something so tiny.
check: CREW_NONE | uncrewed fuel station
check: LANDED Gilly | fuel station landed on Gilly
check: ORE_SURFACE Gilly | Ore present at the fuel site
check: FUEL_MIN 900 | fuel reserves above 900
check: DURATION 30 | keep the fuel station operating for 30 days
=== MISSION ===
id: st_eve_support_station
title: The Emergency Excuse
sparte: Stationen
body: Eve
epoche: 9
prereq: cr_eve_orbit, un_eve_lander, net_eve_relay3
reward: 520
repeatable: no
recordStation: eve_support_station
stationRef: -
beschreibung: Build a capable support station in Eve orbit. It exists for emergencies, return planning, and giving Mission Control something comforting to point at before approving a landing attempt.
beschreibung_en: Build a capable support station in Eve orbit. It exists for emergencies, return planning, and giving Mission Control something comforting to point at before approving a landing attempt.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 3 | support station has room for at least 3 Kerbals
check: ORBIT_ABOVE Eve | stable Eve orbit
check: DURATION 10 | keep the empty station stable for 10 days
check: DOCKING_PORT_COUNT 2 | at least 2 docking ports for return support
check: POWER_CAPACITY_MIN 1500 | ElectricCharge capacity at least 1500
=== MISSION ===
id: un_moho_flyby
title: Sunburn Scout
sparte: Robotische Erkunder
body: Moho
epoche: 5
prereq: cr_mun_landing
reward: 170
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Moho. The mission proves the program can reach the hot inner system without immediately becoming part of it.
beschreibung_en: Send a probe past Moho. The mission proves the program can reach the hot inner system without immediately becoming part of it.
check: CREW_NONE | uncrewed vessel
check: FLYBY Moho 3000 | fly past Moho below 3000 km
=== MISSION ===
id: un_moho_orbit
title: Hot Little Orbit
sparte: Robotische Erkunder
body: Moho
epoche: 5
prereq: un_moho_flyby
reward: 250
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place a probe into orbit around Moho. The planet is small, fast, hot, and clearly built to punish lazy transfer windows.
beschreibung_en: Place a probe into orbit around Moho. The planet is small, fast, hot, and clearly built to punish lazy transfer windows.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Moho 20 | periapsis above 20 km
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: un_moho_lander
title: Touch the Toast
sparte: Robotische Erkunder
body: Moho
epoche: 5
prereq: un_moho_orbit
reward: 310
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Moho and study the scorched surface. The data confirms that Moho is exactly as unfriendly as it looked from orbit.
beschreibung_en: Land a probe on Moho and study the scorched surface. The data confirms that Moho is exactly as unfriendly as it looked from orbit.
check: CREW_NONE | uncrewed vessel
check: LANDED Moho | landed on Moho
=== MISSION ===
id: un_moho_north_pole_search
title: Where Is the Hole?
sparte: Robotische Erkunder
body: Moho
epoche: 5
prereq: un_moho_lander
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe to Moho’s far north and investigate the strange polar anomaly. The official goal is geological science. The unofficial goal is to find out whether the planet has a hole in it.
beschreibung_en: Send a probe to Moho’s far north and investigate the strange polar anomaly. The official goal is geological science. The unofficial goal is to find out whether the planet has a hole in it.
check: CREW_NONE | uncrewed vessel
check: LANDED Moho | landed on Moho
check: MARKER_LANDING Moho 6 84 90 | land near the marked north-pole anomaly
=== MISSION ===
id: cr_moho_landing
title: Kerbals on the Frying Pan
sparte: Pioniere
body: Moho
epoche: 5
prereq: un_moho_lander, st_kerbin_station_longstay3
reward: 560
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Moho and return them safely. This is not the easiest way to prove competence, which is probably why the program chose it.
beschreibung_en: Land Kerbals on Moho and return them safely. This is not the easiest way to prove competence, which is probably why the program chose it.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Moho | landed on Moho
check: EVA Moho LANDED | EVA on Moho
check: RETURN_FROM_BODY Moho Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: cr_moho_mohole_precision_landing
title: Do Not Miss the Hole
sparte: Pioniere
body: Moho
epoche: 5
prereq: cr_moho_landing, un_moho_north_pole_search
reward: 650
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals near Moho’s north-pole anomaly and investigate the Mohole. The landing site must be exact, because somewhere near the mysterious hole is not a recovery plan.
beschreibung_en: Land Kerbals near Moho’s north-pole anomaly and investigate the Mohole. The landing site must be exact, because somewhere near the mysterious hole is not a recovery plan.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Moho | landed on Moho
check: MARKER_LANDING Moho 6 84 90 | land near the Mohole search site
check: RETURN_FROM_BODY Moho Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_dres_flyby
title: Telescope Smudge Check
sparte: Robotische Erkunder
body: Dres
epoche: 5
prereq: cr_mun_landing
reward: 140
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe to investigate Dres. Mission Control is not yet sure whether it is a real dwarf planet, a telescope stain, or a rumor with gravity.
beschreibung_en: Send a probe to investigate Dres. Mission Control is not yet sure whether it is a real dwarf planet, a telescope stain, or a rumor with gravity.
check: CREW_NONE | uncrewed vessel
check: FLYBY Dres 1000 | fly past Dres below 1000 km
=== MISSION ===
id: un_dres_orbit
title: It Was Real
sparte: Robotische Erkunder
body: Dres
epoche: 5
prereq: un_dres_flyby
reward: 190
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place a probe into orbit around Dres. The good news is that Dres is real. The bad news is that now someone has to update the maps.
beschreibung_en: Place a probe into orbit around Dres. The good news is that Dres is real. The bad news is that now someone has to update the maps.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Dres 20 | periapsis above 20 km
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: cr_dres_landing
title: We Remembered Dres
sparte: Pioniere
body: Dres
epoche: 5
prereq: un_dres_orbit, st_kerbin_station_longstay3
reward: 430
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Dres and prove, conclusively and with footprints, that the program remembered it exists.
beschreibung_en: Land Kerbals on Dres and prove, conclusively and with footprints, that the program remembered it exists.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Dres | landed on Dres
check: EVA Dres LANDED | EVA on Dres
check: RETURN_FROM_BODY Dres Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_duna_flyby
title: Red Dust Preview
sparte: Robotische Erkunder
body: Duna
epoche: 6
prereq: cr_mun_landing, st_kerbin_station_longstay3
reward: 180
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Duna. The red planet becomes the first truly serious interplanetary target, which means the program briefly acts professional.
beschreibung_en: Send a probe past Duna. The red planet becomes the first truly serious interplanetary target, which means the program briefly acts professional.
check: CREW_NONE | uncrewed vessel
check: FLYBY Duna 5000 | fly past Duna below 5000 km
=== MISSION ===
id: un_duna_orbit
title: Map the Rust
sparte: Robotische Erkunder
body: Duna
epoche: 6
prereq: un_duna_flyby
reward: 250
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place a probe into orbit around Duna and map future landing sites. For once, the crew will receive useful information before being sent there.
beschreibung_en: Place a probe into orbit around Duna and map future landing sites. For once, the crew will receive useful information before being sent there.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Duna | stable Duna orbit
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: un_duna_lander
title: First Red Touchdown
sparte: Robotische Erkunder
body: Duna
epoche: 6
prereq: un_duna_orbit
reward: 330
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Duna and touch the red dust before the Kerbals do. It is scouting, science, and a warning label with antennas.
beschreibung_en: Land a probe on Duna and touch the red dust before the Kerbals do. It is scouting, science, and a warning label with antennas.
check: CREW_NONE | uncrewed vessel
check: LANDED Duna | landed on Duna
=== MISSION ===
id: un_duna_rover
title: Rover Before Regret
sparte: Robotische Erkunder
body: Duna
epoche: 6
prereq: un_duna_lander
reward: 390
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Land a rover on Duna and scout the future crewed landing area. The rover’s job is to find a safe site and not become the first monument to overconfidence.
beschreibung_en: Land a rover on Duna and scout the future crewed landing area. The rover’s job is to find a safe site and not become the first monument to overconfidence.
check: CREW_NONE | uncrewed rover
check: LANDED Duna | landed on Duna
check: MARKER_LANDING Duna 10 | land within 10 km of the marked crew site
check: WHEEL_MOTION Duna 4 | drive the rover on the surface at 4 m/s or faster
=== MISSION ===
id: net_duna_relay3
title: Red Planet Hotline
sparte: Versorgungsnetz
body: Duna
epoche: 6
prereq: un_duna_orbit
reward: 270
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Duna orbit. Duna is far enough away that mission updates should arrive through proper antennas, not hope.
beschreibung_en: Place three relay-capable satellites in Duna orbit. Duna is far enough away that mission updates should arrive through proper antennas, not hope.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Duna 3 1000 | 3 relay-capable satellites in Duna orbit above 1000 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: cr_duna_flyby
title: Wave at Duna
sparte: Pioniere
body: Duna
epoche: 6
prereq: un_duna_orbit, st_kerbin_station_longstay3
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Duna and return them safely. They see the red planet up close and immediately begin making landing suggestions nobody approved.
beschreibung_en: Send Kerbals past Duna and return them safely. They see the red planet up close and immediately begin making landing suggestions nobody approved.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: FLYBY Duna 8000 | fly past Duna below 8000 km
check: RETURN_FROM_BODY Duna Kerbin flyby | return the crew safely to Kerbin
=== MISSION ===
id: cr_duna_orbit
title: Crew Above the Rust
sparte: Pioniere
body: Duna
epoche: 6
prereq: cr_duna_flyby, un_duna_lander
reward: 520
repeatable: no
recordStation: -
stationRef: -
beschreibung: Put Kerbals into orbit around Duna. The crew studies the planet from above while pretending they are not already picking landing sites.
beschreibung_en: Put Kerbals into orbit around Duna. The crew studies the planet from above while pretending they are not already picking landing sites.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 10 | remain in Duna orbit for 10 days
=== MISSION ===
id: cr_duna_landing
title: Boots in Red Dust
sparte: Pioniere
body: Duna
epoche: 6
prereq: cr_duna_orbit, un_duna_rover, net_duna_relay3
reward: 720
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Duna and return them safely to Kerbin. This is the program’s first great interplanetary surface expedition and the start of serious deep-space operations.
beschreibung_en: Land Kerbals on Duna and return them safely to Kerbin. This is the program’s first great interplanetary surface expedition and the start of serious deep-space operations.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Duna | landed on Duna
check: EVA Duna LANDED | EVA on Duna
check: RETURN_FROM_BODY Duna Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: cr_ike_flyby
title: Ike Looks Involved
sparte: Pioniere
body: Ike
epoche: 6
prereq: cr_duna_orbit
reward: 260
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Ike while operating in the Duna system. Ike is large enough to be useful and close enough to be blamed for navigational mistakes.
beschreibung_en: Send Kerbals past Ike while operating in the Duna system. Ike is large enough to be useful and close enough to be blamed for navigational mistakes.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: FLYBY Ike 500 | fly past Ike below 500 km
=== MISSION ===
id: cr_ike_landing
title: Park on the Big Rock
sparte: Pioniere
body: Ike
epoche: 6
prereq: cr_ike_flyby
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Ike and return them safely. The moon becomes a practical side objective in Duna space, without needing its own infrastructure chain.
beschreibung_en: Land Kerbals on Ike and return them safely. The moon becomes a practical side objective in Duna space, without needing its own infrastructure chain.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Ike | landed on Ike
check: EVA Ike LANDED | EVA on Ike
check: RETURN_FROM_BODY Ike Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: st_duna_station_core2
title: Station After the Landing
sparte: Stationen
body: Duna
epoche: 6
prereq: cr_duna_landing
reward: 500
repeatable: no
recordStation: duna_station
stationRef: -
beschreibung: After the first Duna landing, place a station core in Duna orbit for two Kerbals. The program has finally learned to build the orbital support station after proving it needed one.
beschreibung_en: After the first Duna landing, place a station core in Duna orbit for two Kerbals. The program has finally learned to build the orbital support station after proving it needed one.
check: CREW_NONE | uncrewed station core
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 10 | keep the empty station stable for 10 days
check: CREW_CAPACITY_MIN 2 | station has room for at least 2 Kerbals
=== MISSION ===
id: st_duna_station_crew2
title: Two Kerbals Over Duna
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_core2
reward: 540
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Bring two Kerbals to the Duna station. The outpost becomes the red planet’s first permanent orbital workplace, which sounds better than waiting room.
beschreibung_en: Bring two Kerbals to the Duna station. The outpost becomes the red planet’s first permanent orbital workplace, which sounds better than waiting room.
check: CREW_MIN 2 | at least 2 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DOCK_ANY | dock with the Duna station
=== MISSION ===
id: st_duna_station_longstay2
title: Red Orbit Residence
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_crew2
reward: 650
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the Duna station with two Kerbals for 150 days. The crew watches the red planet below and tries not to mention that the first landing happened without this station.
beschreibung_en: Operate the Duna station with two Kerbals for 150 days. The crew watches the red planet below and tries not to mention that the first landing happened without this station.
check: CREW_MIN 2 | at least 2 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: rep_duna_orbit_supply
title: Red Orbit Supply Run
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_longstay2
reward: 140
repeatable: yes
recordStation: -
stationRef: duna_station
beschreibung: Deliver an uncrewed supply vehicle to Duna orbit. Interplanetary logistics are slow, expensive, and still easier than explaining why the pantry is empty.
beschreibung_en: Deliver an uncrewed supply vehicle to Duna orbit. Interplanetary logistics are slow, expensive, and still easier than explaining why the pantry is empty.
check: CREW_NONE | uncrewed supply vehicle
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DOCK_STATION duna_station Duna | dock with the Duna station
=== MISSION ===
id: st_duna_station_upgrade4
title: Duna Gets Serious
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_longstay2
reward: 700
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Expand the Duna station to support 4 Kerbals. The upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
beschreibung_en: Expand the Duna station to support 4 Kerbals. The upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 4 | station has room for at least 4 Kerbals
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_duna_station_longstay4
title: Four Watch the Rust
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_upgrade4
reward: 790
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the 4-Kerbal Duna station for 150 days. The bigger the station gets, the more normal it becomes to have a second home around another planet.
beschreibung_en: Operate the 4-Kerbal Duna station for 150 days. The bigger the station gets, the more normal it becomes to have a second home around another planet.
check: CREW_MIN 4 | at least 4 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: st_duna_station_upgrade6
title: More Seats, More Dust
sparte: Stationen
body: Duna
epoche: 7
prereq: st_duna_station_longstay4
reward: 780
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Expand the Duna station to support 6 Kerbals. The upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
beschreibung_en: Expand the Duna station to support 6 Kerbals. The upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 6 | station has room for at least 6 Kerbals
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_duna_station_longstay6
title: Six in Red Orbit
sparte: Stationen
body: Duna
epoche: 7
prereq: st_duna_station_upgrade6
reward: 870
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the 6-Kerbal Duna station for 150 days. The bigger the station gets, the more normal it becomes to have a second home around another planet.
beschreibung_en: Operate the 6-Kerbal Duna station for 150 days. The bigger the station gets, the more normal it becomes to have a second home around another planet.
check: CREW_MIN 6 | at least 6 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: st_duna_station_upgrade8
title: Deep Red Expansion
sparte: Stationen
body: Duna
epoche: 7
prereq: st_duna_station_longstay6
reward: 860
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Expand the Duna station to support 8 Kerbals. The upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
beschreibung_en: Expand the Duna station to support 8 Kerbals. The upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 8 | station has room for at least 8 Kerbals
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 10 | keep the empty station stable for 10 days
=== MISSION ===
id: st_duna_station_longstay8
title: Eight Over Duna
sparte: Stationen
body: Duna
epoche: 7
prereq: st_duna_station_upgrade8
reward: 950
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the 8-Kerbal Duna station for 150 days. The bigger the station gets, the more normal it becomes to have a second home around another planet.
beschreibung_en: Operate the 8-Kerbal Duna station for 150 days. The bigger the station gets, the more normal it becomes to have a second home around another planet.
check: CREW_MIN 8 | at least 8 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days
=== MISSION ===
id: base_duna_base2
title: The Red Front Door
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_longstay2, cr_duna_landing
reward: 760
repeatable: no
recordStation: duna_base
stationRef: -
beschreibung: Build the first Duna surface base and support two Kerbals. The red planet is no longer just a destination; it is now a place with a door, a flag, and a worrying maintenance list.
beschreibung_en: Build the first Duna surface base and support two Kerbals. The red planet is no longer just a destination; it is now a place with a door, a flag, and a worrying maintenance list.
check: CREW_MIN 2 | at least 2 Kerbals at the base
check: LANDED Duna | base landed on Duna
check: DURATION 30 | operate the base for 30 days
=== MISSION ===
id: base_duna_base4
title: Duna Base Grows
sparte: Stationen
body: Duna
epoche: 6
prereq: base_duna_base2
reward: 880
repeatable: no
recordStation: -
stationRef: duna_base
beschreibung: Expand the Duna base to four Kerbals. The settlement now has enough crew to call problems departments instead of emergencies.
beschreibung_en: Expand the Duna base to four Kerbals. The settlement now has enough crew to call problems departments instead of emergencies.
check: CREW_MIN 4 | at least 4 Kerbals at the base
check: LANDED Duna | base landed on Duna
check: DURATION 60 | operate the expanded base for 60 days
=== MISSION ===
id: base_duna_base6
title: Permanent-ish on Duna
sparte: Stationen
body: Duna
epoche: 6
prereq: base_duna_base4
reward: 1050
repeatable: no
recordStation: -
stationRef: duna_base
beschreibung: Expand the Duna base to six Kerbals. Duna has become a true interplanetary workplace, complete with science schedules and rover parking disputes.
beschreibung_en: Expand the Duna base to six Kerbals. Duna has become a true interplanetary workplace, complete with science schedules and rover parking disputes.
check: CREW_MIN 6 | at least 6 Kerbals at the base
check: LANDED Duna | base landed on Duna
check: DURATION 100 | operate the expanded base for 100 days
=== MISSION ===
id: net_interplanetary_relay3
title: The Sunlit Switchboard
sparte: Versorgungsnetz
body: Sun
epoche: 7
prereq: st_duna_station_upgrade4, net_duna_relay3, net_kerbin_relay3
reward: 520
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in solar orbit. The network links the inner system to the deep-space program and gives probes a way to report that they are very, very far away.
beschreibung_en: Place three relay-capable satellites in solar orbit. The network links the inner system to the deep-space program and gives probes a way to report that they are very, very far away.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Sun 3 1000000 | 3 relay-capable satellites in solar orbit
check: DURATION 1 | interplanetary relay ring remains active for 1 day
=== MISSION ===
id: un_eeloo_flyby
title: Cold Edge Hello
sparte: Robotische Erkunder
body: Eeloo
epoche: 7
prereq: net_interplanetary_relay3
reward: 620
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Eeloo. It is cold, distant, lonely, and exactly the kind of place the program points at before asking, Can we land there?
beschreibung_en: Send a probe past Eeloo. It is cold, distant, lonely, and exactly the kind of place the program points at before asking, Can we land there?
check: CREW_NONE | uncrewed vessel
check: FLYBY Eeloo 3000 | fly past Eeloo below 3000 km
=== MISSION ===
id: un_eeloo_lander
title: The Lonely Snowball
sparte: Robotische Erkunder
body: Eeloo
epoche: 7
prereq: un_eeloo_flyby
reward: 900
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Eeloo. The little world at the edge of the system receives the program’s quietest and coldest surface scout.
beschreibung_en: Land a probe on Eeloo. The little world at the edge of the system receives the program’s quietest and coldest surface scout.
check: CREW_NONE | uncrewed vessel
check: LANDED Eeloo | landed on Eeloo
=== MISSION ===
id: cr_eeloo_landing
title: Directly to the Freezer
sparte: Pioniere
body: Eeloo
epoche: 7
prereq: un_eeloo_flyby
reward: 1260
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals directly to land on Eeloo after the flyby data is available. No crewed flyby, no crewed orbit rehearsal — just a very long trip and a very brave landing checklist.
beschreibung_en: Send Kerbals directly to land on Eeloo after the flyby data is available. No crewed flyby, no crewed orbit rehearsal — just a very long trip and a very brave landing checklist.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Eeloo | landed on Eeloo
check: EVA Eeloo LANDED | EVA on Eeloo
check: RETURN_FROM_BODY Eeloo Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_jool_atmo_probe
title: Green Soup Dive
sparte: Robotische Erkunder
body: Jool
epoche: 8
prereq: net_interplanetary_relay3
reward: 640
repeatable: no
recordStation: -
stationRef: -
beschreibung: Drop a probe into Jool’s atmosphere. It will not come back, but it will send priceless data while discovering how many shades of green a pressure warning can have.
beschreibung_en: Drop a probe into Jool’s atmosphere. It will not come back, but it will send priceless data while discovering how many shades of green a pressure warning can have.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Jool 70 95 | enter Jool upper atmosphere
=== MISSION ===
id: un_jool_orbiter
title: Orbit the Big Green
sparte: Robotische Erkunder
body: Jool
epoche: 8
prereq: un_jool_atmo_probe
reward: 780
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place a probe into orbit around Jool. This opens the system for serious moon operations and proves the program can navigate where everything is large, fast, and inconvenient.
beschreibung_en: Place a probe into orbit around Jool. This opens the system for serious moon operations and proves the program can navigate where everything is large, fast, and inconvenient.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Jool | stable Jool orbit
check: HOLD 10 | hold stable orbit for 10 seconds
=== MISSION ===
id: net_jool_relay3
title: Jool System Switchboard
sparte: Versorgungsnetz
body: Jool
epoche: 8
prereq: un_jool_orbiter
reward: 720
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Jool orbit. The Jool system is too big for improvisation, even by Kerbal standards.
beschreibung_en: Place three relay-capable satellites in Jool orbit. The Jool system is too big for improvisation, even by Kerbal standards.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Jool 3 10000 | 3 relay-capable satellites in Jool orbit above 10000 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: st_jool_gateway
title: A Hub in Trouble
sparte: Stationen
body: Jool
epoche: 8
prereq: net_jool_relay3
reward: 850
repeatable: no
recordStation: jool_gateway
stationRef: -
beschreibung: Build an orbital support hub in the Jool system. It is not required for every landing, but it gives the program docking space, power, plans, and the illusion of control.
beschreibung_en: Build an orbital support hub in the Jool system. It is not required for every landing, but it gives the program docking space, power, plans, and the illusion of control.
check: CREW_NONE | empty station, no Kerbals aboard
check: CREW_CAPACITY_MIN 3 | gateway has room for at least 3 Kerbals
check: ORBIT_ABOVE Jool | stable Jool orbit
check: DURATION 10 | keep the empty station stable for 10 days
check: DOCKING_PORT_COUNT 3 | at least 3 docking ports for deep-space traffic
check: POWER_CAPACITY_MIN 2500 | ElectricCharge capacity at least 2500
=== MISSION ===
id: un_laythe_flyby
title: Kerbin-ish, Suspiciously
sparte: Robotische Erkunder
body: Laythe
epoche: 8
prereq: un_jool_orbiter
reward: 420
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Laythe. It looks almost welcoming, which is suspicious because it is orbiting Jool.
beschreibung_en: Send a probe past Laythe. It looks almost welcoming, which is suspicious because it is orbiting Jool.
check: CREW_NONE | uncrewed vessel
check: FLYBY Laythe 2000 | fly past Laythe below 2000 km
=== MISSION ===
id: un_laythe_atmo_lander
title: Splash Test, Maybe
sparte: Robotische Erkunder
body: Laythe
epoche: 8
prereq: un_laythe_flyby
reward: 560
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send an uncrewed lander through Laythe’s atmosphere and down to the surface. The probe checks air, water, landing conditions, and whether almost like Kerbin is a dangerous phrase.
beschreibung_en: Send an uncrewed lander through Laythe’s atmosphere and down to the surface. The probe checks air, water, landing conditions, and whether almost like Kerbin is a dangerous phrase.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Laythe 60 90 | enter Laythe upper atmosphere
check: LANDED Laythe | landed on Laythe
=== MISSION ===
id: net_laythe_relay3
title: Beach Signal Network
sparte: Versorgungsnetz
body: Laythe
epoche: 8
prereq: un_laythe_flyby, net_jool_relay3
reward: 620
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Laythe orbit. A world with oceans, atmosphere, and future bases deserves communications better than shouting through Jool’s radiation belts.
beschreibung_en: Place three relay-capable satellites in Laythe orbit. A world with oceans, atmosphere, and future bases deserves communications better than shouting through Jool’s radiation belts.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Laythe 3 500 | 3 relay-capable satellites in Laythe orbit above 500 km
check: DURATION 1 | network remains active for 1 day
=== MISSION ===
id: cr_laythe_landing
title: A Beach Too Far
sparte: Pioniere
body: Laythe
epoche: 8
prereq: un_laythe_atmo_lander, net_laythe_relay3, net_kerbin_relay6_polar, net_mun_relay6_polar
reward: 1250
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Laythe and return them safely. It is the most Kerbin-like world in the Jool system, which is another way of saying the program has found a distant place to make familiar mistakes.
beschreibung_en: Land Kerbals on Laythe and return them safely. It is the most Kerbin-like world in the Jool system, which is another way of saying the program has found a distant place to make familiar mistakes.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Laythe | landed on Laythe
check: EVA Laythe LANDED | EVA on Laythe
check: RETURN_FROM_BODY Laythe Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: base_laythe_base3
title: First Beach Camp
sparte: Stationen
body: Laythe
epoche: 8
prereq: cr_laythe_landing, net_laythe_relay3
reward: 1350
repeatable: no
recordStation: laythe_base
stationRef: -
beschreibung: Build the first Laythe base and support three Kerbals on the surface. The colony begins as a beachhead, a research outpost, and possibly the most expensive seaside camp in history.
beschreibung_en: Build the first Laythe base and support three Kerbals on the surface. The colony begins as a beachhead, a research outpost, and possibly the most expensive seaside camp in history.
check: CREW_MIN 3 | at least 3 Kerbals at the base
check: LANDED Laythe | base landed on Laythe
check: DURATION 30 | operate the base for 30 days
=== MISSION ===
id: rep_laythe_base_supply
title: Beachhead Supply Run
sparte: Stationen
body: Laythe
epoche: 8
prereq: base_laythe_base3
reward: 260
repeatable: yes
recordStation: -
stationRef: laythe_base
beschreibung: Deliver supplies to the Laythe base. The colony may have an atmosphere and oceans, but it still cannot manufacture replacement ladders out of optimism.
beschreibung_en: Deliver supplies to the Laythe base. The colony may have an atmosphere and oceans, but it still cannot manufacture replacement ladders out of optimism.
check: CREW_NONE | uncrewed supply vehicle
check: LANDED Laythe | landed on Laythe
check: RESOURCE_DELIVERY laythe_base Fuel 800 2 | deliver at least 800 fuel within 2 km of the Laythe base
=== MISSION ===
id: base_laythe_base6
title: Six by the Sea
sparte: Stationen
body: Laythe
epoche: 8
prereq: base_laythe_base3
reward: 1650
repeatable: no
recordStation: -
stationRef: laythe_base
beschreibung: Expand the Laythe base to support 6 Kerbals. The settlement is no longer just a landing site; it is turning into a real off-world community with science, maintenance, and beach arguments.
beschreibung_en: Expand the Laythe base to support 6 Kerbals. The settlement is no longer just a landing site; it is turning into a real off-world community with science, maintenance, and beach arguments.
check: CREW_MIN 6 | at least 6 Kerbals at the base
check: LANDED Laythe | base landed on Laythe
check: DURATION 60 | operate the expanded base for 60 days
=== MISSION ===
id: base_laythe_base10
title: Laythe Gets Crowded
sparte: Stationen
body: Laythe
epoche: 8
prereq: base_laythe_base6
reward: 2100
repeatable: no
recordStation: -
stationRef: laythe_base
beschreibung: Expand the Laythe base to support 10 Kerbals. The settlement is no longer just a landing site; it is turning into a real off-world community with science, maintenance, and beach arguments.
beschreibung_en: Expand the Laythe base to support 10 Kerbals. The settlement is no longer just a landing site; it is turning into a real off-world community with science, maintenance, and beach arguments.
check: CREW_MIN 10 | at least 10 Kerbals at the base
check: LANDED Laythe | base landed on Laythe
check: DURATION 100 | operate the expanded base for 100 days
=== MISSION ===
id: base_laythe_base15
title: Almost a Town
sparte: Stationen
body: Laythe
epoche: 8
prereq: base_laythe_base10
reward: 2600
repeatable: no
recordStation: -
stationRef: laythe_base
beschreibung: Expand the Laythe base to support 15 Kerbals. The settlement is no longer just a landing site; it is turning into a real off-world community with science, maintenance, and beach arguments.
beschreibung_en: Expand the Laythe base to support 15 Kerbals. The settlement is no longer just a landing site; it is turning into a real off-world community with science, maintenance, and beach arguments.
check: CREW_MIN 15 | at least 15 Kerbals at the base
check: LANDED Laythe | base landed on Laythe
check: DURATION 150 | operate the expanded base for 150 days
=== MISSION ===
id: un_vall_flyby
title: Ice Looks Nice
sparte: Robotische Erkunder
body: Vall
epoche: 8
prereq: un_jool_orbiter
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Vall. The moon is cold, bright, and scientifically interesting enough to make landing there sound reasonable.
beschreibung_en: Send a probe past Vall. The moon is cold, bright, and scientifically interesting enough to make landing there sound reasonable.
check: CREW_NONE | uncrewed vessel
check: FLYBY Vall 1000 | fly past Vall below 1000 km
=== MISSION ===
id: cr_vall_landing
title: Boots on Blue Ice
sparte: Pioniere
body: Vall
epoche: 8
prereq: un_vall_flyby
reward: 720
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Vall and return them safely. It is a clean, icy Jool-system landing for crews who want adventure without oceans or Tylo-level regret.
beschreibung_en: Land Kerbals on Vall and return them safely. It is a clean, icy Jool-system landing for crews who want adventure without oceans or Tylo-level regret.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Vall | landed on Vall
check: EVA Vall LANDED | EVA on Vall
check: RETURN_FROM_BODY Vall Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_tylo_flyby
title: Gravity Has No Chill
sparte: Robotische Erkunder
body: Tylo
epoche: 8
prereq: un_jool_orbiter
reward: 380
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Tylo. The moon looks like a normal landing target until engineers remember it has gravity and no atmosphere, which is rude.
beschreibung_en: Send a probe past Tylo. The moon looks like a normal landing target until engineers remember it has gravity and no atmosphere, which is rude.
check: CREW_NONE | uncrewed vessel
check: FLYBY Tylo 1000 | fly past Tylo below 1000 km
=== MISSION ===
id: cr_tylo_landing
title: Because It Was Hard
sparte: Pioniere
body: Tylo
epoche: 8
prereq: un_tylo_flyby
reward: 1150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Tylo and return them safely. This mission exists because someone said it was too difficult and everyone else heard that as a proposal.
beschreibung_en: Land Kerbals on Tylo and return them safely. This mission exists because someone said it was too difficult and everyone else heard that as a proposal.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Tylo | landed on Tylo
check: EVA Tylo LANDED | EVA on Tylo
check: RETURN_FROM_BODY Tylo Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: outpost_tylo_three_kerbals
title: Three Kerbals, One Bad Idea
sparte: Pioniere
body: Tylo
epoche: 8
prereq: cr_tylo_landing
reward: 980
repeatable: no
recordStation: -
stationRef: -
beschreibung: Establish a one-time three-Kerbal outpost on Tylo. There is no expansion, no regular resupply, and no sensible reason to do it except that it will look magnificent on the mission board.
beschreibung_en: Establish a one-time three-Kerbal outpost on Tylo. There is no expansion, no regular resupply, and no sensible reason to do it except that it will look magnificent on the mission board.
check: CREW_EXACT 3 | exactly 3 Kerbals at the outpost
check: LANDED Tylo | outpost landed on Tylo
check: DURATION 30 | operate the challenge outpost for 30 days
=== MISSION ===
id: un_bop_flyby
title: Bop Knows Something
sparte: Robotische Erkunder
body: Bop
epoche: 8
prereq: un_jool_orbiter
reward: 280
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Bop. The moon is small, strange, and gives the impression that it knows something.
beschreibung_en: Send a probe past Bop. The moon is small, strange, and gives the impression that it knows something.
check: CREW_NONE | uncrewed vessel
check: FLYBY Bop 500 | fly past Bop below 500 km
=== MISSION ===
id: cr_bop_landing
title: Touch the Weird Rock
sparte: Pioniere
body: Bop
epoche: 8
prereq: un_bop_flyby
reward: 520
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Bop. It is not the largest Jool moon, but it may be the weirdest, which counts for something.
beschreibung_en: Land Kerbals on Bop. It is not the largest Jool moon, but it may be the weirdest, which counts for something.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Bop | landed on Bop
check: EVA Bop LANDED | EVA on Bop
check: RETURN_FROM_BODY Bop Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: un_pol_flyby
title: Lumpy Little Detour
sparte: Robotische Erkunder
body: Pol
epoche: 8
prereq: un_jool_orbiter
reward: 280
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Pol. It is lumpy, distant, and looks like a place where navigation errors go to retire.
beschreibung_en: Send a probe past Pol. It is lumpy, distant, and looks like a place where navigation errors go to retire.
check: CREW_NONE | uncrewed vessel
check: FLYBY Pol 500 | fly past Pol below 500 km
=== MISSION ===
id: cr_pol_landing
title: Boldly on Pol
sparte: Pioniere
body: Pol
epoche: 8
prereq: un_pol_flyby
reward: 520
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Pol and return them safely. The landing may be gentle, but the mission report will still use the word bold.
beschreibung_en: Land Kerbals on Pol and return them safely. The landing may be gentle, but the mission report will still use the word bold.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Pol | landed on Pol
check: EVA Pol LANDED | EVA on Pol
check: RETURN_FROM_BODY Pol Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: cr_eve_landing_return
title: The Purple Final Exam
sparte: Pioniere
body: Eve
epoche: 9
prereq: st_eve_support_station, un_gilly_fuel_station, cr_laythe_landing
reward: 3000
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals on Eve and return them safely to Kerbin. This is the final exam of the entire space program: the planet is beautiful, the gravity is unforgiving, and the ascent vehicle was apparently designed by someone who enjoys suspense.
beschreibung_en: Land Kerbals on Eve and return them safely to Kerbin. This is the final exam of the entire space program: the planet is beautiful, the gravity is unforgiving, and the ascent vehicle was apparently designed by someone who enjoys suspense.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Eve | landed on Eve
check: EVA Eve LANDED | EVA on Eve
check: RETURN_FROM_BODY Eve Kerbin | return the crew safely to Kerbin
=== MISSION ===
id: base_eve_are_we_living_here_now
title: Are We Living Here Now?
sparte: Stationen
body: Eve
epoche: 9
prereq: cr_eve_landing_return
reward: 1800
repeatable: no
recordStation: eve_surface_base
stationRef: -
beschreibung: Establish a small Eve surface base after the successful return mission. The mission report begins with a simple question: are we living here now?
beschreibung_en: Establish a small Eve surface base after the successful return mission. The mission report begins with a simple question: are we living here now?
check: CREW_MIN 3 | at least 3 Kerbals at the Eve base
check: LANDED Eve | base landed on Eve
check: DURATION 30 | operate the Eve base for 30 days
=== MISSION ===
id: opt_kerbin_station_certification
title: Optional Kerbin Station Certification
sparte: Stationen
body: Kerbin
epoche: 2
prereq: st_kerbin_station_upgrade4
reward: 35
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Perform an optional engineering audit of the Kerbin station. Mass, a science laboratory, electrical reserve and docking capacity are checked without blocking the campaign.
beschreibung_en: Perform an optional engineering audit of the Kerbin station. Mass, a science laboratory, electrical reserve and docking capacity are checked without blocking the campaign.
check: ORBIT_ABOVE Kerbin 80 | stable Kerbin orbit above 80 km
check: MASS_MIN 12 | station mass at least 12 tonnes
check: MODULE_COUNT ModuleScienceLab|ModuleScienceConverter|Laboratory 1 | at least one compatible science laboratory
check: POWER_CAPACITY_MIN 800 | ElectricCharge capacity at least 800
check: DOCKING_PORT_COUNT 2 | at least two docking ports
=== MISSION ===
id: opt_mun_station_certification
title: Optional Mun Station Certification
sparte: Stationen
body: Mun
epoche: 3
prereq: st_mun_station_core3
reward: 55
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Certify the Mun station as a real orbital laboratory with sufficient mass, science hardware, stored power and docking capacity. This remains an optional side mission.
beschreibung_en: Certify the Mun station as a real orbital laboratory with sufficient mass, science hardware, stored power and docking capacity. This remains an optional side mission.
check: ORBIT_ABOVE Mun 15 | stable Mun orbit above 15 km
check: MASS_MIN 16 | station mass at least 16 tonnes
check: MODULE_COUNT ModuleScienceLab|ModuleScienceConverter|Laboratory 1 | at least one compatible science laboratory
check: POWER_CAPACITY_MIN 1200 | ElectricCharge capacity at least 1200
check: DOCKING_PORT_COUNT 2 | at least two docking ports
=== MISSION ===
id: opt_duna_station_certification
title: Optional Duna Station Certification
sparte: Stationen
body: Duna
epoche: 6
prereq: st_duna_station_core2
reward: 90
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Give the Duna station a deep-space engineering certification. Its mass, laboratory module, power reserve and docking ports must support long operations, but the audit never gates later missions.
beschreibung_en: Give the Duna station a deep-space engineering certification. Its mass, laboratory module, power reserve and docking ports must support long operations, but the audit never gates later missions.
check: ORBIT_ABOVE Duna 60 | stable Duna orbit above 60 km
check: MASS_MIN 20 | station mass at least 20 tonnes
check: MODULE_COUNT ModuleScienceLab|ModuleScienceConverter|Laboratory 1 | at least one compatible science laboratory
check: POWER_CAPACITY_MIN 1600 | ElectricCharge capacity at least 1600
check: DOCKING_PORT_COUNT 2 | at least two docking ports
