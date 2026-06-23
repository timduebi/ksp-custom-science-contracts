# WISSENSCHAFT

Das Missionssystem bleibt ein Bonus zum normalen Science-Mode und ersetzt Stock-Science nicht. Die Belohnungen unterstützen den Fortschritt, aber Experimente, Proben, EVA-Berichte und eigene Erkundung bleiben die Hauptquelle für grosse Tech-Sprünge.

Diese Stock-Kerbol-Kampagne ist nicht mehr probe-first. Kerbin, Mun und Minmus beginnen bewusst kerbaliger: frühe Crew-Flüge sind normal und erwünscht. Ab Duna, Eve, Moho, Jool und Eeloo wird die Kampagne wieder strukturierter, weil gefährliche oder entfernte Ziele zuerst robotisch verstanden werden sollen.

Grobe Skala: frühe Kerbin-Missionen 8–150, Mun und Minmus 85–340, innere Planetenziele 120–560, Duna bemannt und Infrastruktur 330–850, Jool-Monde 320–1350, Laythe-Basis 1350–2600, Eeloo 600–1260, Eve-Rückkehrfinale 3000.

# ABLAUF

Die Kampagne startet im echten Stock-Kerbal-Stil: Der erste grosse Schritt ist direkt ein bemannter Start. Danach folgen suborbitaler Flug, erster Orbit, EVA und ein mehrtägiger Aufenthalt im Kerbin-Orbit. Unbemannte Kerbin-Tests und Satelliten bleiben als optionale Wissenschafts- und Lernmissionen erhalten, blockieren aber den ersten Crewpfad nicht.

Das erste Docking wird nicht als isolierte Demonstration geführt. Stattdessen wird zuerst eine unbemannte Kerbin-Station mit drei Plätzen gestartet. Anschliessend dockt ein Crewfahrzeug an und bringt drei Kerbals an Bord. Jede echte Station wird über 150 Tage getestet. Nach dem ersten 150-Tage-Betrieb der Kerbin-Station öffnet sich die grosse Kampagne; spätere Kerbin-Stationsausbauten bleiben Infrastrukturziele, blockieren aber nicht künstlich die Hauptprogression.

Mun ist der erste grosse Horizont. Kerbals dürfen nach dem Mun-Orbit direkt auf der Oberfläche landen. Der unbemannte Mun-Orbiter wird parallel zum Crew-Orbit verfügbar und bleibt optional. Für spätere Infrastruktur werden Rover, Präzisionslandungen und ein polarer Zielanflug wichtiger. Die Mun-Station startet mit drei Kerbals und kann später auf sechs Kerbals ausgebaut werden. Die Mun-Basis startet mit drei Kerbals und kann auf sechs Kerbals wachsen.

Minmus bleibt spielerisch und praktisch. Es gibt keinen langen Minmus-Aufenthalt als Pflichtbogen. Stattdessen führen Flyby, Landung und Präzisionslandung zu einer Minmus-Fuel-Base. Diese Basis wird erst nach der ersten Mun-Basis geöffnet, blockiert aber keine grosse Hauptprogression.

Duna ist der erste ernsthafte interplanetare Hauptbogen. Hier werden Flyby, Orbiter, Lander und Rover robotisch vorbereitet, bevor Kerbals landen. Die Duna-Orbitstation kommt bewusst erst nach der ersten Duna-Landung. Sie startet mit zwei Kerbals und wächst bis acht. Nach dem ersten Upgrade auf vier Kerbals kann die Jool-/Eeloo-Phase beginnen.

Eve bleibt gefährlich. Eve-Orbit und Eve-Atmosphärensonde öffnen parallel nach dem Eve-Flyby; nur die Atmosphärensonde ist Pflicht für den ersten unbemannten Eve-Lander. Für Crew gibt es zunächst Eve-Orbit, Gilly-Flyby und Gilly-Landung. Eine Eve-Orbit-Supportstation mit Treibstoffreserve und eine unbemannte Gilly-Fuel-Station sind Pflicht für das grosse Finale: bemannt auf Eve landen und wieder nach Kerbin zurückkehren.

Moho ist ein Prestigezweig. Nach robotischer Vorerkundung folgt eine direkte bemannte Landung. Zusätzlich gibt es eine präzise Nordpol-/Mohole-Suche und eine bemannte Präzisionslandung am Mohole-Ziel.

Dres bleibt optional und blockiert nichts. Der erste Flyby spielt bewusst mit der Frage, ob Dres wirklich ein Himmelskörper oder nur ein Fleck auf dem Teleskop ist. Nach Flyby und Orbit kann eine Crew landen, aber der Hauptpfad geht ohne Dres weiter.

Jool und Eeloo öffnen gleichzeitig nach dem interplanetaren Relay-Ring und dem ersten Ausbau der Duna-Station. Für Jool gibt es keinen unbemannten Flyby als Pflicht. Stattdessen startet der Jool-Bogen mit einer Atmosphärensonde und einem unbemannten Jool-Orbiter. Der Jool-Orbiter öffnet das System; jeder Mond braucht dann seinen eigenen unbemannten Flyby, bevor Kerbals direkt landen dürfen. Ein bemannter Jool-Orbit ist keine Voraussetzung für Mondlandungen.

Laythe ist der grosse Jool-Infrastrukturpfad. Vor der bemannten Laythe-Landung braucht es den Laythe-Flyby, einen unbemannten Atmosphärenlander, das Jool-Relay-Netz, das Laythe-Relay-Netz sowie die ausgebauten Kerbin- und Mun-Relays mit je sechs Satelliten, davon drei polar. Die Laythe-Basis wächst danach bis auf 15 Kerbals.

Tylo bekommt einen einmaligen 3-Kerbal-Challenge-Outpost ohne Ausbau und ohne normale Versorgungskette. Vall, Bop und Pol bleiben Landungsziele ohne Infrastrukturketten. Eeloo bekommt Flyby, unbemannten Lander und danach die Möglichkeit zu einer direkten Crew-Landung ohne Crew-Orbit.

# STATIONSKETTEN

chain: body=Kerbin | key=kerbin_station | typ=station | prereq=st_kerbin_station_core3 | stufen=3,4,6,8,10
chain: body=Kerbin | key=kerbin_fuel_depot | typ=station | prereq=dep_kerbin_fuel_depot_core | stufen=1
chain: body=Mun | key=mun_station | typ=station | prereq=st_mun_station_core3 | stufen=3,6
chain: body=Mun | key=mun_base | typ=base | prereq=base_mun_base3 | stufen=3,6
chain: body=Minmus | key=minmus_base | typ=base | prereq=base_minmus_fuel_base | stufen=3
chain: body=Duna | key=duna_station | typ=station | prereq=st_duna_station_core2 | stufen=2,4,6,8
chain: body=Duna | key=duna_base | typ=base | prereq=base_duna_base2 | stufen=2,4,6
chain: body=Eve | key=eve_support_station | typ=station | prereq=st_eve_support_station | stufen=3
chain: body=Gilly | key=gilly_fuel_station | typ=base | prereq=un_gilly_fuel_station | stufen=1
chain: body=Jool | key=jool_gateway | typ=station | prereq=st_jool_gateway | stufen=3
chain: body=Laythe | key=laythe_base | typ=base | prereq=base_laythe_base3 | stufen=3,6,10,15

# MISSIONEN

=== MISSION ===
id: cr_kerbin_first_launch
title: First Crewed Hop
sparte: Pioniere
body: Kerbin
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
sparte: Pioniere
body: Kerbin
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
sparte: Pioniere
body: Kerbin
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
sparte: Pioniere
body: Kerbin
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
sparte: Pioniere
body: Kerbin
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
sparte: Robotische Erkunder
body: Kerbin
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
sparte: Robotische Erkunder
body: Kerbin
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
sparte: Robotische Erkunder
body: Kerbin
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
sparte: Versorgungsnetz
body: Kerbin
prereq: cr_kerbin_orbit_3d
reward: 90
repeatable: no
recordStation: kerbin_station
stationRef: -
beschreibung: Launch an uncrewed station core into Kerbin orbit with room for three Kerbals. Nobody is aboard yet, which gives engineers a rare chance to discover problems before they become conversations.
beschreibung_en: Launch an uncrewed station core into Kerbin orbit with room for three Kerbals. Nobody is aboard yet, which gives engineers a rare chance to discover problems before they become conversations.
check: CREW_NONE | uncrewed station core
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: CREW_CAPACITY_MIN 3 | station has room for at least 3 Kerbals

=== MISSION ===
id: st_kerbin_station_crew3
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_core3
reward: 105
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Dock a crew vehicle to the new Kerbin station and bring three Kerbals aboard. This is the program’s first real docking operation, carefully disguised as moving in.
beschreibung_en: Dock a crew vehicle to the new Kerbin station and bring three Kerbals aboard. This is the program’s first real docking operation, carefully disguised as moving in.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DOCK_ANY | perform a docking maneuver

=== MISSION ===
id: st_kerbin_station_longstay3
sparte: Versorgungsnetz
body: Kerbin
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
id: st_kerbin_station_upgrade4
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay3
reward: 120
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 4 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
beschreibung_en: Expand the Kerbin station so 4 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: CREW_CAPACITY_MIN 4 | station has room for at least 4 Kerbals

=== MISSION ===
id: st_kerbin_station_longstay4
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_upgrade4
reward: 165
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the expanded Kerbin station with 4 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
beschreibung_en: Operate the expanded Kerbin station with 4 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
check: CREW_MIN 4 | at least 4 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_kerbin_station_upgrade6
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay4
reward: 150
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 6 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
beschreibung_en: Expand the Kerbin station so 6 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: CREW_CAPACITY_MIN 6 | station has room for at least 6 Kerbals

=== MISSION ===
id: st_kerbin_station_longstay6
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_upgrade6
reward: 195
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the expanded Kerbin station with 6 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
beschreibung_en: Operate the expanded Kerbin station with 6 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
check: CREW_MIN 6 | at least 6 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_kerbin_station_upgrade8
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay6
reward: 180
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 8 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
beschreibung_en: Expand the Kerbin station so 8 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: CREW_CAPACITY_MIN 8 | station has room for at least 8 Kerbals

=== MISSION ===
id: st_kerbin_station_longstay8
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_upgrade8
reward: 225
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the expanded Kerbin station with 8 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
beschreibung_en: Operate the expanded Kerbin station with 8 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
check: CREW_MIN 8 | at least 8 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_kerbin_station_upgrade10
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay8
reward: 220
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Expand the Kerbin station so 10 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
beschreibung_en: Expand the Kerbin station so 10 Kerbals can live there. The extra seats are officially for science and unofficially for whoever lost the meeting about station comfort.
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: CREW_CAPACITY_MIN 10 | station has room for at least 10 Kerbals

=== MISSION ===
id: st_kerbin_station_longstay10
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_upgrade10
reward: 265
repeatable: no
recordStation: -
stationRef: kerbin_station
beschreibung: Operate the expanded Kerbin station with 10 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
beschreibung_en: Operate the expanded Kerbin station with 10 Kerbals for 150 days. The longer the crew stays, the more Mission Control insists this was always the plan.
check: CREW_MIN 10 | at least 10 Kerbals aboard the station
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: net_kerbin_relay3
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay3
reward: 85
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Kerbin orbit. The network gives Mission Control a reliable way to talk to spacecraft, probes, stations, and Kerbals who claim they were just testing the controls.
beschreibung_en: Place three relay-capable satellites in Kerbin orbit. The network gives Mission Control a reliable way to talk to spacecraft, probes, stations, and Kerbals who claim they were just testing the controls.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Kerbin 3 500 | 3 relay satellites in Kerbin orbit with periapsis above 500 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: net_kerbin_relay6_polar
sparte: Versorgungsnetz
body: Kerbin
prereq: net_kerbin_relay3
reward: 130
repeatable: no
recordStation: -
stationRef: -
beschreibung: Expand the Kerbin relay network to six satellites, with three of them in polar orbits. This closes the awkward communication gaps that previously occurred whenever a mission flew somewhere inconvenient.
beschreibung_en: Expand the Kerbin relay network to six satellites, with three of them in polar orbits. This closes the awkward communication gaps that previously occurred whenever a mission flew somewhere inconvenient.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Kerbin 6 500 | 6 relay satellites in Kerbin orbit with periapsis above 500 km
check: RELAY_VESSEL_COUNT_INCLINATION Kerbin 3 75 500 | 3 relay satellites in polar Kerbin orbit
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: dep_kerbin_fuel_depot_core
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay3
reward: 95
repeatable: no
recordStation: kerbin_fuel_depot
stationRef: -
beschreibung: Place an uncrewed fuel depot in Kerbin orbit. It is not required for every mission, but it makes the program look much more prepared than it usually is.
beschreibung_en: Place an uncrewed fuel depot in Kerbin orbit. It is not required for every mission, but it makes the program look much more prepared than it usually is.
check: CREW_NONE | uncrewed depot
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: FUEL_MIN 1000 | fuel reserve above 1000 units

=== MISSION ===
id: dep_kerbin_fuel_delivery
sparte: Versorgungsnetz
body: Kerbin
prereq: dep_kerbin_fuel_depot_core
reward: 55
repeatable: yes
recordStation: -
stationRef: kerbin_fuel_depot
beschreibung: Deliver fuel to the Kerbin orbital depot. The paperwork calls it logistics; the flight crew calls it bringing snacks for engines.
beschreibung_en: Deliver fuel to the Kerbin orbital depot. The paperwork calls it logistics; the flight crew calls it bringing snacks for engines.
check: CREW_NONE | uncrewed tanker
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: FUEL_MIN 800 | fuel reserve above 800 units
check: DOCK_ANY | dock with the depot

=== MISSION ===
id: cr_mun_flyby
sparte: Pioniere
body: Mun
prereq: st_kerbin_station_longstay3
reward: 90
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past the Mun and bring them home. They do not land yet, but they do get close enough for everyone to agree that the next idea is obvious and probably dangerous.
beschreibung_en: Send Kerbals past the Mun and bring them home. They do not land yet, but they do get close enough for everyone to agree that the next idea is obvious and probably dangerous.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: FLYBY Mun 500 | fly by the Mun below 500 km
check: RETURN_FROM_BODY Mun Kerbin flyby | return the crew safely to Kerbin after the flyby

=== MISSION ===
id: cr_mun_orbit
sparte: Pioniere
body: Mun
prereq: cr_mun_flyby
reward: 120
repeatable: no
recordStation: -
stationRef: -
beschreibung: Put Kerbals into orbit around the Mun. From here, the surface looks close, quiet, and suspiciously landable.
beschreibung_en: Put Kerbals into orbit around the Mun. From here, the surface looks close, quiet, and suspiciously landable.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: HOLD 10 | hold stable orbit for 10 seconds

=== MISSION ===
id: un_mun_orbiter
sparte: Robotische Erkunder
body: Mun
prereq: cr_mun_orbit
reward: 85
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send an uncrewed orbiter to the Mun. It maps the surface, studies landing zones, and politely arrives at the same time the crew is already proving that maps would have been useful.
beschreibung_en: Send an uncrewed orbiter to the Mun. It maps the surface, studies landing zones, and politely arrives at the same time the crew is already proving that maps would have been useful.
check: CREW_NONE | uncrewed vessel
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: HOLD 10 | hold stable orbit for 10 seconds

=== MISSION ===
id: cr_mun_landing
sparte: Pioniere
body: Mun
prereq: cr_mun_orbit
reward: 165
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
sparte: Robotische Erkunder
body: Mun
prereq: cr_mun_landing
reward: 110
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
sparte: Robotische Erkunder
body: Mun
prereq: un_mun_rover
reward: 135
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Land a rover precisely at the chosen Mun base site. Mission Control insists this exact spot matters because the rover found excellent terrain, which mostly means it did not immediately roll away.
beschreibung_en: Land a rover precisely at the chosen Mun base site. Mission Control insists this exact spot matters because the rover found excellent terrain, which mostly means it did not immediately roll away.
check: CREW_NONE | uncrewed rover
check: LANDED Mun | landed on the Mun
check: MARKER_LANDING Mun 5 | land within 5 km of the target site
check: WHEEL_MOTION Mun 4 | drive the rover on the surface at 4 m/s or faster

=== MISSION ===
id: cr_mun_precision_landing
sparte: Pioniere
body: Mun
prereq: un_mun_precision_rover_landing, cr_mun_landing
reward: 175
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals near the scouting rover. This proves the program can aim for a planned site instead of discovering the landing zone only after touching it.
beschreibung_en: Land Kerbals near the scouting rover. This proves the program can aim for a planned site instead of discovering the landing zone only after touching it.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Mun | landed on the Mun
check: MARKER_LANDING Mun 5 | land within 5 km of the target site
check: RETURN_FROM_BODY Mun Kerbin | return the crew safely to Kerbin

=== MISSION ===
id: cr_mun_polar_precision_landing
sparte: Pioniere
body: Mun
prereq: cr_mun_landing
reward: 190
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals near one of the Mun’s poles. The site was chosen for science, lighting conditions, and because someone in Mission Control drew a circle there and now refuses to erase it.
beschreibung_en: Land Kerbals near one of the Mun’s poles. The site was chosen for science, lighting conditions, and because someone in Mission Control drew a circle there and now refuses to erase it.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Mun | landed on the Mun
check: MARKER_LANDING Mun 8 70 90 | land within 8 km of a polar target site
check: RETURN_FROM_BODY Mun Kerbin | return the crew safely to Kerbin

=== MISSION ===
id: net_mun_relay3
sparte: Versorgungsnetz
body: Mun
prereq: cr_mun_landing
reward: 120
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Mun orbit. The Mun is close enough to visit, but far enough away that shouting is no longer an approved communications method.
beschreibung_en: Place three relay-capable satellites in Mun orbit. The Mun is close enough to visit, but far enough away that shouting is no longer an approved communications method.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Mun 3 200 | 3 relay satellites in Mun orbit with periapsis above 200 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: net_mun_relay6_polar
sparte: Versorgungsnetz
body: Mun
prereq: net_mun_relay3
reward: 170
repeatable: no
recordStation: -
stationRef: -
beschreibung: Expand the Mun relay network to six satellites, with three in polar orbits. This gives the Mun proper coverage, including the awkward places Kerbals keep choosing for dramatic landings.
beschreibung_en: Expand the Mun relay network to six satellites, with three in polar orbits. This gives the Mun proper coverage, including the awkward places Kerbals keep choosing for dramatic landings.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Mun 6 200 | 6 relay satellites in Mun orbit with periapsis above 200 km
check: RELAY_VESSEL_COUNT_INCLINATION Mun 3 75 200 | 3 relay satellites in polar Mun orbit
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: st_mun_station_core3
sparte: Versorgungsnetz
body: Mun
prereq: net_mun_relay3, cr_mun_precision_landing
reward: 180
repeatable: no
recordStation: mun_station
stationRef: -
beschreibung: Launch a three-Kerbal station into Mun orbit. It gives landers a place to dock, crews a place to regroup, and Mission Control a place to point at when claiming this was all planned.
beschreibung_en: Launch a three-Kerbal station into Mun orbit. It gives landers a place to dock, crews a place to regroup, and Mission Control a place to point at when claiming this was all planned.
check: CREW_NONE | uncrewed station core
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: CREW_CAPACITY_MIN 3 | station has room for at least 3 Kerbals

=== MISSION ===
id: st_mun_station_longstay3
sparte: Versorgungsnetz
body: Mun
prereq: st_mun_station_core3
reward: 225
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Operate the Mun station with three Kerbals for 150 days. The crew learns how to live around another world without constantly looking down and asking whether they left the lander lights on.
beschreibung_en: Operate the Mun station with three Kerbals for 150 days. The crew learns how to live around another world without constantly looking down and asking whether they left the lander lights on.
check: CREW_MIN 3 | at least 3 Kerbals aboard the station
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_mun_station_upgrade6
sparte: Versorgungsnetz
body: Mun
prereq: st_mun_station_longstay3
reward: 240
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Expand the Mun station to support six Kerbals. The extra capacity turns it from a heroic outpost into something dangerously close to a workplace.
beschreibung_en: Expand the Mun station to support six Kerbals. The extra capacity turns it from a heroic outpost into something dangerously close to a workplace.
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: CREW_CAPACITY_MIN 6 | station has room for at least 6 Kerbals

=== MISSION ===
id: st_mun_station_longstay6
sparte: Versorgungsnetz
body: Mun
prereq: st_mun_station_upgrade6
reward: 290
repeatable: no
recordStation: -
stationRef: mun_station
beschreibung: Operate the expanded Mun station with six Kerbals for 150 days. The station is now large enough that someone can get lost and still be technically on duty.
beschreibung_en: Operate the expanded Mun station with six Kerbals for 150 days. The station is now large enough that someone can get lost and still be technically on duty.
check: CREW_MIN 6 | at least 6 Kerbals aboard the station
check: ORBIT_ABOVE Mun 20 | periapsis above 20 km
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: base_mun_base3
sparte: Versorgungsnetz
body: Mun
prereq: st_mun_station_longstay3, cr_mun_precision_landing, un_mun_precision_rover_landing
reward: 260
repeatable: no
recordStation: mun_base
stationRef: -
beschreibung: Build the first Mun base and support three Kerbals on the surface. The base starts small, but it proves the Mun can be more than a place for footprints, flags, and suspiciously abandoned descent stages.
beschreibung_en: Build the first Mun base and support three Kerbals on the surface. The base starts small, but it proves the Mun can be more than a place for footprints, flags, and suspiciously abandoned descent stages.
check: CREW_MIN 3 | at least 3 Kerbals at the base
check: LANDED Mun | landed on the Mun
check: DURATION 30 | operate the base for 30 days

=== MISSION ===
id: base_mun_base6
sparte: Versorgungsnetz
body: Mun
prereq: base_mun_base3
reward: 340
repeatable: no
recordStation: -
stationRef: mun_base
beschreibung: Expand the Mun base to support six Kerbals. The settlement now has enough crew to do real science, real maintenance, and real arguments about who parked the rover upside down.
beschreibung_en: Expand the Mun base to support six Kerbals. The settlement now has enough crew to do real science, real maintenance, and real arguments about who parked the rover upside down.
check: CREW_MIN 6 | at least 6 Kerbals at the base
check: LANDED Mun | landed on the Mun
check: DURATION 150 | operate the expanded base for 150 days

=== MISSION ===
id: cr_minmus_flyby
sparte: Pioniere
body: Minmus
prereq: cr_mun_landing
reward: 95
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Minmus. The moon looks small, minty, and harmless, which is exactly the kind of description that has caused trouble before.
beschreibung_en: Send Kerbals past Minmus. The moon looks small, minty, and harmless, which is exactly the kind of description that has caused trouble before.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: FLYBY Minmus 300 | fly by Minmus below 300 km
check: RETURN_FROM_BODY Minmus Kerbin flyby | return the crew safely to Kerbin after the flyby

=== MISSION ===
id: cr_minmus_landing
sparte: Pioniere
body: Minmus
prereq: cr_minmus_flyby
reward: 145
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
sparte: Pioniere
body: Minmus
prereq: cr_minmus_landing
reward: 165
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land precisely on Minmus at the planned site. The terrain is flat, the gravity is forgiving, and somehow the navigation team still managed to make this sound difficult.
beschreibung_en: Land precisely on Minmus at the planned site. The terrain is flat, the gravity is forgiving, and somehow the navigation team still managed to make this sound difficult.
check: CREW_MIN 1 | at least 1 Kerbal aboard
check: LANDED Minmus | landed on Minmus
check: MARKER_LANDING Minmus 5 | land within 5 km of the target site
check: RETURN_FROM_BODY Minmus Kerbin | return the crew safely to Kerbin

=== MISSION ===
id: net_minmus_relay3
sparte: Versorgungsnetz
body: Minmus
prereq: cr_minmus_landing
reward: 125
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Minmus orbit. This ensures that future fuel operations can report success, failure, or the tanker slowly drifting away.
beschreibung_en: Place three relay-capable satellites in Minmus orbit. This ensures that future fuel operations can report success, failure, or the tanker slowly drifting away.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Minmus 3 100 | 3 relay satellites in Minmus orbit with periapsis above 100 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: base_minmus_fuel_base
sparte: Versorgungsnetz
body: Minmus
prereq: base_mun_base3, cr_minmus_precision_landing, net_minmus_relay3
reward: 230
repeatable: no
recordStation: minmus_base
stationRef: -
beschreibung: Build a Minmus surface base with fuel equipment for future exploration. It does not block the main campaign, but it gives the program a practical refueling foothold in Kerbin’s backyard.
beschreibung_en: Build a Minmus surface base with fuel equipment for future exploration. It does not block the main campaign, but it gives the program a practical refueling foothold in Kerbin’s backyard.
check: CREW_MIN 3 | at least 3 Kerbals at the base
check: LANDED Minmus | landed on Minmus
check: ORE_SURFACE Minmus | mine ore on the surface
check: RESOURCE_MIN Ore 50 | store at least 50 units of ore
check: FUEL_MIN 500 | fuel reserve above 500 units

=== MISSION ===
id: un_eve_flyby
sparte: Robotische Erkunder
body: Eve
prereq: st_kerbin_station_longstay3
reward: 130
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Eve. The planet looks beautiful, purple, and completely unwilling to let anything leave again.
beschreibung_en: Send a probe past Eve. The planet looks beautiful, purple, and completely unwilling to let anything leave again.
check: CREW_NONE | uncrewed vessel
check: FLYBY Eve 5000 | fly by Eve below 5000 km

=== MISSION ===
id: un_eve_orbit
sparte: Robotische Erkunder
body: Eve
prereq: un_eve_flyby
reward: 180
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
sparte: Robotische Erkunder
body: Eve
prereq: un_eve_flyby
reward: 190
repeatable: no
recordStation: -
stationRef: -
beschreibung: Drop a probe into Eve’s atmosphere and collect data while it descends. The probe is not expected to enjoy the experience.
beschreibung_en: Drop a probe into Eve’s atmosphere and collect data while it descends. The probe is not expected to enjoy the experience.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Eve 60 90 | enter Eve’s upper atmosphere

=== MISSION ===
id: un_eve_lander
sparte: Robotische Erkunder
body: Eve
prereq: un_eve_atmo_probe
reward: 245
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Eve. Reaching the surface is the easy part, which is exactly the sort of sentence that should worry everyone.
beschreibung_en: Land a probe on Eve. Reaching the surface is the easy part, which is exactly the sort of sentence that should worry everyone.
check: CREW_NONE | uncrewed vessel
check: LANDED Eve | landed on Eve

=== MISSION ===
id: net_eve_relay3
sparte: Versorgungsnetz
body: Eve
prereq: un_eve_orbit
reward: 210
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Eve orbit. Every future Eve mission needs strong communications, mostly so Mission Control can say we are still evaluating options in real time.
beschreibung_en: Place three relay-capable satellites in Eve orbit. Every future Eve mission needs strong communications, mostly so Mission Control can say we are still evaluating options in real time.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Eve 3 1000 | 3 relay satellites in Eve orbit with periapsis above 1000 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: cr_eve_orbit
sparte: Pioniere
body: Eve
prereq: un_eve_orbit, net_eve_relay3, st_kerbin_station_longstay3
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals into orbit around Eve. They are close enough to see the surface clearly and far enough away to still believe they can go home.
beschreibung_en: Send Kerbals into orbit around Eve. They are close enough to see the surface clearly and far enough away to still believe they can go home.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: ORBIT_ABOVE Eve | stable Eve orbit
check: DURATION 10 | remain in Eve orbit for 10 days

=== MISSION ===
id: cr_gilly_flyby
sparte: Pioniere
body: Gilly
prereq: cr_eve_orbit
reward: 180
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Gilly. It is small, strange, and may technically count as a moon if everyone agrees not to ask too many questions.
beschreibung_en: Send Kerbals past Gilly. It is small, strange, and may technically count as a moon if everyone agrees not to ask too many questions.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: FLYBY Gilly 100 | fly by Gilly below 100 km

=== MISSION ===
id: cr_gilly_landing
sparte: Pioniere
body: Gilly
prereq: cr_gilly_flyby
reward: 230
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
sparte: Versorgungsnetz
body: Gilly
prereq: cr_gilly_landing, net_eve_relay3
reward: 240
repeatable: no
recordStation: gilly_fuel_station
stationRef: -
beschreibung: Place an uncrewed fuel station at Gilly. The little moon becomes Eve’s practical support rock, which is more responsibility than anyone expected from something so tiny.
beschreibung_en: Place an uncrewed fuel station at Gilly. The little moon becomes Eve’s practical support rock, which is more responsibility than anyone expected from something so tiny.
check: CREW_NONE | uncrewed fuel station
check: LANDED Gilly | landed on Gilly
check: FUEL_MIN 1000 | fuel reserve above 1000 units

=== MISSION ===
id: st_eve_support_station
sparte: Versorgungsnetz
body: Eve
prereq: cr_eve_orbit, net_eve_relay3, un_eve_lander
reward: 480
repeatable: no
recordStation: eve_support_station
stationRef: -
beschreibung: Build a fuel-equipped support station in Eve orbit. It exists for emergencies, return planning, and giving Mission Control something comforting to point at before approving a landing attempt.
beschreibung_en: Build a fuel-equipped support station in Eve orbit. It exists for emergencies, return planning, and giving Mission Control something comforting to point at before approving a landing attempt.
check: CREW_NONE | uncrewed support station
check: ORBIT_ABOVE Eve | stable Eve orbit
check: CREW_CAPACITY_MIN 3 | station has room for at least 3 Kerbals
check: FUEL_MIN 2000 | emergency fuel reserve above 2000 units

=== MISSION ===
id: un_moho_flyby
sparte: Robotische Erkunder
body: Moho
prereq: st_kerbin_station_longstay3
reward: 150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Moho. The mission proves the program can reach the hot inner system without immediately becoming part of it.
beschreibung_en: Send a probe past Moho. The mission proves the program can reach the hot inner system without immediately becoming part of it.
check: CREW_NONE | uncrewed vessel
check: FLYBY Moho 2000 | fly by Moho below 2000 km

=== MISSION ===
id: un_moho_orbit
sparte: Robotische Erkunder
body: Moho
prereq: un_moho_flyby
reward: 220
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
sparte: Robotische Erkunder
body: Moho
prereq: un_moho_orbit
reward: 280
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Moho and study the scorched surface. The data confirms that Moho is exactly as unfriendly as it looked from orbit.
beschreibung_en: Land a probe on Moho and study the scorched surface. The data confirms that Moho is exactly as unfriendly as it looked from orbit.
check: CREW_NONE | uncrewed vessel
check: LANDED Moho | landed on Moho

=== MISSION ===
id: un_moho_north_pole_search
sparte: Robotische Erkunder
body: Moho
prereq: un_moho_lander
reward: 330
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe to Moho’s far north and investigate the strange polar anomaly. The official goal is geological science. The unofficial goal is to find out whether the planet has a hole in it.
beschreibung_en: Send a probe to Moho’s far north and investigate the strange polar anomaly. The official goal is geological science. The unofficial goal is to find out whether the planet has a hole in it.
check: CREW_NONE | uncrewed vessel
check: LANDED Moho | landed on Moho
check: MARKER_LANDING Moho 3 85 90 | land within 3 km of the north-pole anomaly

=== MISSION ===
id: cr_moho_landing
sparte: Pioniere
body: Moho
prereq: un_moho_lander, st_kerbin_station_longstay3
reward: 480
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
sparte: Pioniere
body: Moho
prereq: cr_moho_landing, un_moho_north_pole_search
reward: 560
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land Kerbals near Moho’s north-pole anomaly and investigate the Mohole. The landing site must be exact, because somewhere near the mysterious hole is not a recovery plan.
beschreibung_en: Land Kerbals near Moho’s north-pole anomaly and investigate the Mohole. The landing site must be exact, because somewhere near the mysterious hole is not a recovery plan.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: LANDED Moho | landed on Moho
check: MARKER_LANDING Moho 3 85 90 | land within 3 km of the Mohole target
check: RETURN_FROM_BODY Moho Kerbin | return the crew safely to Kerbin

=== MISSION ===
id: un_dres_flyby
sparte: Robotische Erkunder
body: Dres
prereq: st_kerbin_station_longstay3
reward: 120
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe to investigate Dres. Mission Control is not yet sure whether it is a real dwarf planet, a telescope stain, or a rumor with gravity.
beschreibung_en: Send a probe to investigate Dres. Mission Control is not yet sure whether it is a real dwarf planet, a telescope stain, or a rumor with gravity.
check: CREW_NONE | uncrewed vessel
check: FLYBY Dres 1000 | fly by Dres below 1000 km

=== MISSION ===
id: un_dres_orbit
sparte: Robotische Erkunder
body: Dres
prereq: un_dres_flyby
reward: 175
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
sparte: Pioniere
body: Dres
prereq: un_dres_orbit
reward: 360
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
sparte: Robotische Erkunder
body: Duna
prereq: st_kerbin_station_longstay3
reward: 150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Duna. The red planet becomes the first truly serious interplanetary target, which means the program briefly acts professional.
beschreibung_en: Send a probe past Duna. The red planet becomes the first truly serious interplanetary target, which means the program briefly acts professional.
check: CREW_NONE | uncrewed vessel
check: FLYBY Duna 3000 | fly by Duna below 3000 km

=== MISSION ===
id: un_duna_orbit
sparte: Robotische Erkunder
body: Duna
prereq: un_duna_flyby
reward: 220
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
sparte: Robotische Erkunder
body: Duna
prereq: un_duna_orbit
reward: 285
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Duna and touch the red dust before the Kerbals do. It is scouting, science, and a warning label with antennas.
beschreibung_en: Land a probe on Duna and touch the red dust before the Kerbals do. It is scouting, science, and a warning label with antennas.
check: CREW_NONE | uncrewed vessel
check: LANDED Duna | landed on Duna

=== MISSION ===
id: un_duna_rover
sparte: Robotische Erkunder
body: Duna
prereq: un_duna_lander
reward: 340
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Land a rover on Duna and scout the future crewed landing area. The rover’s job is to find a safe site and not become the first monument to overconfidence.
beschreibung_en: Land a rover on Duna and scout the future crewed landing area. The rover’s job is to find a safe site and not become the first monument to overconfidence.
check: CREW_NONE | uncrewed rover
check: LANDED Duna | landed on Duna
check: MARKER_LANDING Duna 10 | land within 10 km of the target site
check: WHEEL_MOTION Duna 4 | drive the rover on the surface at 4 m/s or faster

=== MISSION ===
id: net_duna_relay3
sparte: Versorgungsnetz
body: Duna
prereq: un_duna_orbit
reward: 240
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Duna orbit. Duna is far enough away that mission updates should arrive through proper antennas, not hope.
beschreibung_en: Place three relay-capable satellites in Duna orbit. Duna is far enough away that mission updates should arrive through proper antennas, not hope.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Duna 3 800 | 3 relay satellites in Duna orbit with periapsis above 800 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: cr_duna_flyby
sparte: Pioniere
body: Duna
prereq: un_duna_orbit, st_kerbin_station_longstay3
reward: 330
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Duna and return them safely. They see the red planet up close and immediately begin making landing suggestions nobody approved.
beschreibung_en: Send Kerbals past Duna and return them safely. They see the red planet up close and immediately begin making landing suggestions nobody approved.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: FLYBY Duna 5000 | fly by Duna below 5000 km
check: RETURN_FROM_BODY Duna Kerbin flyby | return the crew safely to Kerbin after the flyby

=== MISSION ===
id: cr_duna_orbit
sparte: Pioniere
body: Duna
prereq: cr_duna_flyby, net_duna_relay3
reward: 430
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
sparte: Pioniere
body: Duna
prereq: cr_duna_orbit, un_duna_rover
reward: 560
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
sparte: Pioniere
body: Ike
prereq: cr_duna_orbit
reward: 150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals past Ike while operating in the Duna system. Ike is large enough to be useful and close enough to be blamed for navigational mistakes.
beschreibung_en: Send Kerbals past Ike while operating in the Duna system. Ike is large enough to be useful and close enough to be blamed for navigational mistakes.
check: CREW_MIN 2 | at least 2 Kerbals aboard
check: FLYBY Ike 300 | fly by Ike below 300 km

=== MISSION ===
id: cr_ike_landing
sparte: Pioniere
body: Ike
prereq: cr_ike_flyby
reward: 260
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
sparte: Versorgungsnetz
body: Duna
prereq: cr_duna_landing
reward: 420
repeatable: no
recordStation: duna_station
stationRef: -
beschreibung: After the first Duna landing, place a station core in Duna orbit for two Kerbals. The program has finally learned to build the orbital support station after proving it needed one.
beschreibung_en: After the first Duna landing, place a station core in Duna orbit for two Kerbals. The program has finally learned to build the orbital support station after proving it needed one.
check: CREW_NONE | uncrewed station core
check: ORBIT_ABOVE Duna | stable Duna orbit
check: CREW_CAPACITY_MIN 2 | station has room for at least 2 Kerbals

=== MISSION ===
id: st_duna_station_longstay2
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_core2
reward: 480
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the Duna station with two Kerbals for 150 days. The crew watches the red planet below and tries not to mention that the first landing happened without this station.
beschreibung_en: Operate the Duna station with two Kerbals for 150 days. The crew watches the red planet below and tries not to mention that the first landing happened without this station.
check: CREW_MIN 2 | at least 2 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_duna_station_upgrade4
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_longstay2
reward: 560
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Expand the Duna station to support four Kerbals. This upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
beschreibung_en: Expand the Duna station to support four Kerbals. This upgrade proves the program can maintain a real interplanetary outpost, not just a heroic visit with extra paperwork.
check: ORBIT_ABOVE Duna | stable Duna orbit
check: CREW_CAPACITY_MIN 4 | station has room for at least 4 Kerbals

=== MISSION ===
id: st_duna_station_longstay4
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_upgrade4
reward: 610
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the Duna station with 4 Kerbals for 150 days. Deep-space routine is still heroic, but now it comes with maintenance schedules.
beschreibung_en: Operate the Duna station with 4 Kerbals for 150 days. Deep-space routine is still heroic, but now it comes with maintenance schedules.
check: CREW_MIN 4 | at least 4 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_duna_station_upgrade6
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_longstay4
reward: 670
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Expand the Duna station to support 6 Kerbals. The outpost grows from daring expedition hardware into a real interplanetary workplace.
beschreibung_en: Expand the Duna station to support 6 Kerbals. The outpost grows from daring expedition hardware into a real interplanetary workplace.
check: ORBIT_ABOVE Duna | stable Duna orbit
check: CREW_CAPACITY_MIN 6 | station has room for at least 6 Kerbals

=== MISSION ===
id: st_duna_station_longstay6
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_upgrade6
reward: 720
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the Duna station with 6 Kerbals for 150 days. Deep-space routine is still heroic, but now it comes with maintenance schedules.
beschreibung_en: Operate the Duna station with 6 Kerbals for 150 days. Deep-space routine is still heroic, but now it comes with maintenance schedules.
check: CREW_MIN 6 | at least 6 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: st_duna_station_upgrade8
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_longstay6
reward: 780
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Expand the Duna station to support 8 Kerbals. The outpost grows from daring expedition hardware into a real interplanetary workplace.
beschreibung_en: Expand the Duna station to support 8 Kerbals. The outpost grows from daring expedition hardware into a real interplanetary workplace.
check: ORBIT_ABOVE Duna | stable Duna orbit
check: CREW_CAPACITY_MIN 8 | station has room for at least 8 Kerbals

=== MISSION ===
id: st_duna_station_longstay8
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_upgrade8
reward: 850
repeatable: no
recordStation: -
stationRef: duna_station
beschreibung: Operate the Duna station with 8 Kerbals for 150 days. Deep-space routine is still heroic, but now it comes with maintenance schedules.
beschreibung_en: Operate the Duna station with 8 Kerbals for 150 days. Deep-space routine is still heroic, but now it comes with maintenance schedules.
check: CREW_MIN 8 | at least 8 Kerbals aboard the station
check: ORBIT_ABOVE Duna | stable Duna orbit
check: DURATION 150 | operate continuously for 150 days

=== MISSION ===
id: base_duna_base2
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_longstay2, cr_duna_landing, un_duna_rover
reward: 560
repeatable: no
recordStation: duna_base
stationRef: -
beschreibung: Build the first Duna surface base and support two Kerbals. The red planet is no longer just a destination; it is now a place with a door, a flag, and a worrying maintenance list.
beschreibung_en: Build the first Duna surface base and support two Kerbals. The red planet is no longer just a destination; it is now a place with a door, a flag, and a worrying maintenance list.
check: CREW_MIN 2 | at least 2 Kerbals at the base
check: LANDED Duna | landed on Duna
check: DURATION 30 | operate the base for 30 days

=== MISSION ===
id: base_duna_base4
sparte: Versorgungsnetz
body: Duna
prereq: base_duna_base2
reward: 680
repeatable: no
recordStation: -
stationRef: duna_base
beschreibung: Expand the Duna base to support four Kerbals. The base becomes large enough for science teams, engineering teams, and a team assigned to finding the missing rover.
beschreibung_en: Expand the Duna base to support four Kerbals. The base becomes large enough for science teams, engineering teams, and a team assigned to finding the missing rover.
check: CREW_MIN 4 | at least 4 Kerbals at the base
check: LANDED Duna | landed on Duna
check: DURATION 150 | operate the expanded base for 150 days

=== MISSION ===
id: base_duna_base6
sparte: Versorgungsnetz
body: Duna
prereq: base_duna_base4
reward: 820
repeatable: no
recordStation: -
stationRef: duna_base
beschreibung: Expand the Duna base to support six Kerbals. At this size, the red planet outpost becomes a serious foundation for the deep-space program.
beschreibung_en: Expand the Duna base to support six Kerbals. At this size, the red planet outpost becomes a serious foundation for the deep-space program.
check: CREW_MIN 6 | at least 6 Kerbals at the base
check: LANDED Duna | landed on Duna
check: DURATION 150 | operate the expanded base for 150 days

=== MISSION ===
id: net_interplanetary_relay3
sparte: Versorgungsnetz
body: Sun
prereq: st_duna_station_upgrade4, net_duna_relay3
reward: 420
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in solar orbit. The network links the inner system to the deep-space program and gives probes a way to report that they are very, very far away.
beschreibung_en: Place three relay-capable satellites in solar orbit. The network links the inner system to the deep-space program and gives probes a way to report that they are very, very far away.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Sun 3 1000000 | 3 relay satellites in solar orbit with periapsis above 1000000 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: un_eeloo_flyby
sparte: Robotische Erkunder
body: Eeloo
prereq: net_interplanetary_relay3
reward: 600
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Eeloo. It is cold, distant, lonely, and exactly the kind of place the program points at before asking, Can we land there?
beschreibung_en: Send a probe past Eeloo. It is cold, distant, lonely, and exactly the kind of place the program points at before asking, Can we land there?
check: CREW_NONE | uncrewed vessel
check: FLYBY Eeloo 2000 | fly by Eeloo below 2000 km

=== MISSION ===
id: un_eeloo_lander
sparte: Robotische Erkunder
body: Eeloo
prereq: un_eeloo_flyby
reward: 840
repeatable: no
recordStation: -
stationRef: -
beschreibung: Land a probe on Eeloo. The little world at the edge of the system receives the program’s quietest and coldest surface scout.
beschreibung_en: Land a probe on Eeloo. The little world at the edge of the system receives the program’s quietest and coldest surface scout.
check: CREW_NONE | uncrewed vessel
check: LANDED Eeloo | landed on Eeloo

=== MISSION ===
id: cr_eeloo_landing
sparte: Pioniere
body: Eeloo
prereq: un_eeloo_flyby, st_duna_station_upgrade4
reward: 1260
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send Kerbals directly to land on Eeloo after the flyby data is available. No crewed flyby, no crewed orbit rehearsal, just a very long trip and a very brave landing checklist.
beschreibung_en: Send Kerbals directly to land on Eeloo after the flyby data is available. No crewed flyby, no crewed orbit rehearsal, just a very long trip and a very brave landing checklist.
check: CREW_MIN 3 | at least 3 Kerbals aboard
check: LANDED Eeloo | landed on Eeloo
check: EVA Eeloo LANDED | EVA on Eeloo
check: RETURN_FROM_BODY Eeloo Kerbin | return the crew safely to Kerbin

=== MISSION ===
id: un_jool_atmo_probe
sparte: Robotische Erkunder
body: Jool
prereq: net_interplanetary_relay3
reward: 520
repeatable: no
recordStation: -
stationRef: -
beschreibung: Drop a probe into Jool’s atmosphere. It will not come back, but it will send priceless data while discovering how many shades of green a pressure warning can have.
beschreibung_en: Drop a probe into Jool’s atmosphere. It will not come back, but it will send priceless data while discovering how many shades of green a pressure warning can have.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Jool 70 95 | enter Jool’s upper atmosphere

=== MISSION ===
id: un_jool_orbiter
sparte: Robotische Erkunder
body: Jool
prereq: un_jool_atmo_probe
reward: 620
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
sparte: Versorgungsnetz
body: Jool
prereq: un_jool_orbiter
reward: 600
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Jool orbit. The Jool system is too big for improvisation, even by Kerbal standards.
beschreibung_en: Place three relay-capable satellites in Jool orbit. The Jool system is too big for improvisation, even by Kerbal standards.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Jool 3 10000 | 3 relay satellites in Jool orbit with periapsis above 10000 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: st_jool_gateway
sparte: Versorgungsnetz
body: Jool
prereq: net_jool_relay3
reward: 760
repeatable: no
recordStation: jool_gateway
stationRef: -
beschreibung: Build an orbital support hub in the Jool system. It is not required for every landing, but it gives the program a place to store fuel, plans, and the illusion of control.
beschreibung_en: Build an orbital support hub in the Jool system. It is not required for every landing, but it gives the program a place to store fuel, plans, and the illusion of control.
check: CREW_NONE | uncrewed support hub
check: ORBIT_ABOVE Jool | stable Jool orbit
check: CREW_CAPACITY_MIN 3 | hub has room for at least 3 Kerbals
check: FUEL_MIN 2500 | fuel reserve above 2500 units

=== MISSION ===
id: un_laythe_flyby
sparte: Robotische Erkunder
body: Laythe
prereq: un_jool_orbiter
reward: 430
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Laythe. It looks almost welcoming, which is suspicious because it is orbiting Jool.
beschreibung_en: Send a probe past Laythe. It looks almost welcoming, which is suspicious because it is orbiting Jool.
check: CREW_NONE | uncrewed vessel
check: FLYBY Laythe 1000 | fly by Laythe below 1000 km

=== MISSION ===
id: un_laythe_atmo_lander
sparte: Robotische Erkunder
body: Laythe
prereq: un_laythe_flyby
reward: 620
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send an uncrewed lander through Laythe’s atmosphere and down to the surface. The probe checks air, water, landing conditions, and whether almost like Kerbin is a dangerous phrase.
beschreibung_en: Send an uncrewed lander through Laythe’s atmosphere and down to the surface. The probe checks air, water, landing conditions, and whether almost like Kerbin is a dangerous phrase.
check: CREW_NONE | uncrewed vessel
check: ATMO_FRACTION Laythe 60 90 | enter Laythe’s upper atmosphere
check: LANDED Laythe | landed on Laythe

=== MISSION ===
id: net_laythe_relay3
sparte: Versorgungsnetz
body: Laythe
prereq: un_laythe_flyby, net_jool_relay3
reward: 520
repeatable: no
recordStation: -
stationRef: -
beschreibung: Place three relay-capable satellites in Laythe orbit. A world with oceans, atmosphere, and future bases deserves communications better than shouting through Jool’s radiation belts.
beschreibung_en: Place three relay-capable satellites in Laythe orbit. A world with oceans, atmosphere, and future bases deserves communications better than shouting through Jool’s radiation belts.
check: CREW_NONE | active vessel is uncrewed
check: RELAY_VESSEL_COUNT Laythe 3 500 | 3 relay satellites in Laythe orbit with periapsis above 500 km
check: DURATION 1 | operate for 1 day

=== MISSION ===
id: cr_laythe_landing
sparte: Pioniere
body: Laythe
prereq: un_laythe_atmo_lander, net_laythe_relay3, net_kerbin_relay6_polar, net_mun_relay6_polar, st_duna_station_upgrade4
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
sparte: Versorgungsnetz
body: Laythe
prereq: cr_laythe_landing, net_laythe_relay3
reward: 1350
repeatable: no
recordStation: laythe_base
stationRef: -
beschreibung: Build the first Laythe base and support three Kerbals on the surface. The colony begins as a beachhead, a research outpost, and possibly the most expensive seaside camp in history.
beschreibung_en: Build the first Laythe base and support three Kerbals on the surface. The colony begins as a beachhead, a research outpost, and possibly the most expensive seaside camp in history.
check: CREW_MIN 3 | at least 3 Kerbals at the base
check: LANDED Laythe | landed on Laythe
check: DURATION 30 | operate the base for 30 days

=== MISSION ===
id: base_laythe_base6
sparte: Versorgungsnetz
body: Laythe
prereq: base_laythe_base3
reward: 1650
repeatable: no
recordStation: -
stationRef: laythe_base
beschreibung: Expand the Laythe base to support 6 Kerbals. At this size, the outpost becomes less like a landing site and more like a real off-world settlement with weather.
beschreibung_en: Expand the Laythe base to support 6 Kerbals. At this size, the outpost becomes less like a landing site and more like a real off-world settlement with weather.
check: CREW_MIN 6 | at least 6 Kerbals at the base
check: LANDED Laythe | landed on Laythe
check: DURATION 150 | operate the expanded base for 150 days

=== MISSION ===
id: base_laythe_base10
sparte: Versorgungsnetz
body: Laythe
prereq: base_laythe_base6
reward: 2050
repeatable: no
recordStation: -
stationRef: laythe_base
beschreibung: Expand the Laythe base to support 10 Kerbals. At this size, the outpost becomes less like a landing site and more like a real off-world settlement with weather.
beschreibung_en: Expand the Laythe base to support 10 Kerbals. At this size, the outpost becomes less like a landing site and more like a real off-world settlement with weather.
check: CREW_MIN 10 | at least 10 Kerbals at the base
check: LANDED Laythe | landed on Laythe
check: DURATION 150 | operate the expanded base for 150 days

=== MISSION ===
id: base_laythe_base15
sparte: Versorgungsnetz
body: Laythe
prereq: base_laythe_base10
reward: 2600
repeatable: no
recordStation: -
stationRef: laythe_base
beschreibung: Expand the Laythe base to support 15 Kerbals. At this size, the outpost becomes less like a landing site and more like a real off-world settlement with weather.
beschreibung_en: Expand the Laythe base to support 15 Kerbals. At this size, the outpost becomes less like a landing site and more like a real off-world settlement with weather.
check: CREW_MIN 15 | at least 15 Kerbals at the base
check: LANDED Laythe | landed on Laythe
check: DURATION 150 | operate the expanded base for 150 days

=== MISSION ===
id: un_vall_flyby
sparte: Robotische Erkunder
body: Vall
prereq: un_jool_orbiter
reward: 390
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Vall. The moon is cold, bright, and scientifically interesting enough to make landing there sound reasonable.
beschreibung_en: Send a probe past Vall. The moon is cold, bright, and scientifically interesting enough to make landing there sound reasonable.
check: CREW_NONE | uncrewed vessel
check: FLYBY Vall 1000 | fly by Vall below 1000 km

=== MISSION ===
id: cr_vall_landing
sparte: Pioniere
body: Vall
prereq: un_vall_flyby, st_duna_station_upgrade4
reward: 900
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
sparte: Robotische Erkunder
body: Tylo
prereq: un_jool_orbiter
reward: 410
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Tylo. The moon looks like a normal landing target until engineers remember it has gravity and no atmosphere, which is rude.
beschreibung_en: Send a probe past Tylo. The moon looks like a normal landing target until engineers remember it has gravity and no atmosphere, which is rude.
check: CREW_NONE | uncrewed vessel
check: FLYBY Tylo 1000 | fly by Tylo below 1000 km

=== MISSION ===
id: cr_tylo_landing
sparte: Pioniere
body: Tylo
prereq: un_tylo_flyby, st_duna_station_upgrade4
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
sparte: Versorgungsnetz
body: Tylo
prereq: cr_tylo_landing
reward: 980
repeatable: no
recordStation: -
stationRef: -
beschreibung: Establish a one-time three-Kerbal outpost on Tylo. There is no expansion, no regular resupply, and no sensible reason to do it except that it will look magnificent on the mission board.
beschreibung_en: Establish a one-time three-Kerbal outpost on Tylo. There is no expansion, no regular resupply, and no sensible reason to do it except that it will look magnificent on the mission board.
check: CREW_EXACT 3 | exactly 3 Kerbals at the outpost
check: LANDED Tylo | landed on Tylo
check: DURATION 30 | hold the outpost for 30 days

=== MISSION ===
id: un_bop_flyby
sparte: Robotische Erkunder
body: Bop
prereq: un_jool_orbiter
reward: 320
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Bop. The moon is small, strange, and gives the impression that it knows something.
beschreibung_en: Send a probe past Bop. The moon is small, strange, and gives the impression that it knows something.
check: CREW_NONE | uncrewed vessel
check: FLYBY Bop 500 | fly by Bop below 500 km

=== MISSION ===
id: cr_bop_landing
sparte: Pioniere
body: Bop
prereq: un_bop_flyby, st_duna_station_upgrade4
reward: 700
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
sparte: Robotische Erkunder
body: Pol
prereq: un_jool_orbiter
reward: 320
repeatable: no
recordStation: -
stationRef: -
beschreibung: Send a probe past Pol. It is lumpy, distant, and looks like a place where navigation errors go to retire.
beschreibung_en: Send a probe past Pol. It is lumpy, distant, and looks like a place where navigation errors go to retire.
check: CREW_NONE | uncrewed vessel
check: FLYBY Pol 500 | fly by Pol below 500 km

=== MISSION ===
id: cr_pol_landing
sparte: Pioniere
body: Pol
prereq: un_pol_flyby, st_duna_station_upgrade4
reward: 700
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
sparte: Pioniere
body: Eve
prereq: st_eve_support_station, un_gilly_fuel_station, cr_eve_orbit, un_eve_lander, base_duna_base6
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
id: rep_kerbin_station_resupply
sparte: Versorgungsnetz
body: Kerbin
prereq: st_kerbin_station_longstay3
reward: 45
repeatable: yes
recordStation: -
stationRef: kerbin_station
beschreibung: Resupply the Kerbin station with an uncrewed vehicle. The delivery keeps the station running and gives engineers another chance to send the wrong adapter.
beschreibung_en: Resupply the Kerbin station with an uncrewed vehicle. The delivery keeps the station running and gives engineers another chance to send the wrong adapter.
check: CREW_NONE | uncrewed supply vehicle
check: ORBIT_ABOVE Kerbin | stable Kerbin orbit
check: FUEL_MIN 300 | useful supplies and propellant aboard
check: DOCK_ANY | dock with the station

=== MISSION ===
id: rep_mun_base_supply
sparte: Versorgungsnetz
body: Mun
prereq: base_mun_base3
reward: 75
repeatable: yes
recordStation: -
stationRef: mun_base
beschreibung: Deliver supplies to the Mun base. The base crew appreciates fuel, spare parts, and anything that was not accidentally labeled as decorative rock.
beschreibung_en: Deliver supplies to the Mun base. The base crew appreciates fuel, spare parts, and anything that was not accidentally labeled as decorative rock.
check: CREW_NONE | uncrewed supply vehicle
check: LANDED Mun | landed on the Mun
check: FUEL_MIN 300 | fuel reserve above 300 units

=== MISSION ===
id: rep_duna_orbit_supply
sparte: Versorgungsnetz
body: Duna
prereq: st_duna_station_longstay2
reward: 140
repeatable: yes
recordStation: -
stationRef: duna_station
beschreibung: Deliver an uncrewed supply vehicle to Duna orbit. Interplanetary logistics are slow, expensive, and somehow still easier than explaining why the pantry is empty.
beschreibung_en: Deliver an uncrewed supply vehicle to Duna orbit. Interplanetary logistics are slow, expensive, and somehow still easier than explaining why the pantry is empty.
check: CREW_NONE | uncrewed supply vehicle
check: ORBIT_ABOVE Duna | stable Duna orbit
check: FUEL_MIN 800 | fuel reserve above 800 units

=== MISSION ===
id: rep_laythe_base_supply
sparte: Versorgungsnetz
body: Laythe
prereq: base_laythe_base3
reward: 260
repeatable: yes
recordStation: -
stationRef: laythe_base
beschreibung: Deliver supplies to the Laythe base. The colony may have an atmosphere and oceans, but it still cannot manufacture replacement ladders out of optimism.
beschreibung_en: Deliver supplies to the Laythe base. The colony may have an atmosphere and oceans, but it still cannot manufacture replacement ladders out of optimism.
check: CREW_NONE | uncrewed supply vehicle
check: LANDED Laythe | landed on Laythe
check: FUEL_MIN 800 | fuel reserve above 800 units
