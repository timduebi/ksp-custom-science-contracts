# Custom Science Contracts - Stock KSP Campaign

This is the optional Stock KSP mission source. It is intentionally separate from the
SOL Quarter-Scale source so the current default catalog is not overwritten during
development.

Campaign shape:
- Crewed-first Kerbin program.
- Mun and Minmus form the early core progression.
- Robotic missions support only the most useful first steps: Mun, Duna, Jool and Eeloo.
- Lifelines build communication and fuel infrastructure.
- The grand-tour body landings are optional but chained.
- Finale: Laythe landing and return, then Eve landing and return.

Body names are stock KSP internal names: Sun, Kerbin, Mun, Minmus, Moho, Eve, Gilly,
Duna, Ike, Dres, Jool, Laythe, Vall, Tylo, Bop, Pol, Eeloo.

== Missions ==

=== MISSION ===
id: cr_kerbin_first_hop
title: First Crewed Hop
sparte: Pioneers
body: Kerbin
prereq: -
reward: 8
repeatable: no
description: Put a Kerbal in a rocket, leave the ground, and prove the program can recover a brave pilot with most of the important bits still attached.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ATMO_FRACTION Kerbin 2 35 | Fly through the lower atmosphere

=== MISSION ===
id: cr_kerbin_high_hop
title: High Atmosphere Hop
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_first_hop
reward: 12
repeatable: no
description: Push the capsule higher into thinner air and learn how Kerbals react when the horizon starts looking suspiciously round.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ATMO_FRACTION Kerbin 35 85 | Reach the upper atmosphere

=== MISSION ===
id: cr_kerbin_suborbital
title: First Crewed Spaceflight
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_high_hop
reward: 20
repeatable: no
description: Cross the edge of space on a suborbital hop. The flight does not need to be elegant, but it does need to be crewed.
check: CREW_MIN 1 | At least one Kerbal aboard
check: SUBORBITAL Kerbin | Suborbital spaceflight over Kerbin

=== MISSION ===
id: cr_kerbin_orbit
title: First Kerbin Orbit
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_suborbital
reward: 35
repeatable: no
description: Place a crewed spacecraft into a stable orbit. Kerbin is now something you can go around, not just fall back onto.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ORBIT_ABOVE Kerbin 80 | Stable Kerbin orbit, periapsis above 80 km

=== MISSION ===
id: cr_kerbin_eva
title: First Orbital EVA
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_orbit
reward: 25
repeatable: no
description: Let a Kerbal step outside in orbit and personally confirm that space is not painted on the window.
check: EVA Kerbin ORBITING | Kerbal on EVA in Kerbin orbit

=== MISSION ===
id: cr_kerbin_three_day_orbit
title: Three Days in Orbit
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_orbit
reward: 45
repeatable: no
description: Keep a crew alive and working in orbit for three days. Snacks accounting is strongly encouraged.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ORBIT_ABOVE Kerbin 80 | Stable Kerbin orbit, periapsis above 80 km
check: DURATION 3 | Hold the crewed orbit for 3 days

=== MISSION ===
id: cr_kerbin_docking
title: First Docking in Orbit
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_orbit
reward: 55
repeatable: no
description: Dock two craft in Kerbin orbit and prove that orbital construction is more than polite bumping.
check: CREW_MIN 1 | Crew aboard the active craft
check: ORBIT_ABOVE Kerbin 80 | Stable Kerbin orbit, periapsis above 80 km
check: DOCK_ANY | Dock two vessels

=== MISSION ===
id: cr_kerbin_station_core
title: Kerbin Station Core
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_docking
reward: 85
repeatable: no
recordStation: kerbin_station
description: Build the first real station in Kerbin orbit with a small resident crew. Give it a name that sounds official in press releases.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Kerbin 100 | Stable Kerbin orbit, periapsis above 100 km
check: DURATION 10 | Operate the station for 10 days

=== MISSION ===
id: cr_kerbin_station_expand4
title: Kerbin Station Expansion
sparte: Pioneers
body: Kerbin
prereq: cr_kerbin_station_core
reward: 110
repeatable: no
stationRef: kerbin_station
description: Expand %station% into a proper orbital workshop with room for a larger crew and larger ideas.
check: CREW_MIN 4 | At least four Kerbals aboard
check: ORBIT_ABOVE Kerbin 100 | Stable Kerbin orbit, periapsis above 100 km
check: DURATION 15 | Operate the expanded station for 15 days

=== MISSION ===
id: cr_mun_flyby
title: Crewed Mun Flyby
sparte: Pioneers
body: Mun
prereq: cr_kerbin_orbit
reward: 55
repeatable: no
description: Send a crew past the Mun and bring back wide eyes, blurry photos, and the first serious argument about landing sites.
check: CREW_MIN 1 | At least one Kerbal aboard
check: FLYBY Mun 200 | Fly by the Mun

=== MISSION ===
id: cr_mun_orbit
title: Crewed Mun Orbit
sparte: Pioneers
body: Mun
prereq: cr_mun_flyby
reward: 75
repeatable: no
description: Enter orbit around the Mun with a crewed spacecraft and spend enough time there to make the surface look temptingly reachable.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ORBIT_ABOVE Mun 12 | Stable Mun orbit, periapsis above 12 km

=== MISSION ===
id: cr_mun_landing
title: Crewed Mun Landing
sparte: Pioneers
body: Mun
prereq: cr_mun_orbit, un_mun_lander
reward: 120
repeatable: no
description: Land Kerbals on the Mun. The robotic scout went first; now it is time to leave bootprints next to the questionable engineering.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Mun | Land on the Mun

=== MISSION ===
id: cr_mun_precision_landing
title: Mun Precision Landing
sparte: Pioneers
body: Mun
prereq: cr_mun_landing, un_mun_rover
reward: 145
repeatable: no
description: Land a crew close to a marked Mun site. The mission board insists this is precision, not just aiming at a larger patch of dust.
check: CREW_MIN 1 | At least one Kerbal aboard
check: MARKER_LANDING Mun 10 | Land within 10 km of the Mun target

=== MISSION ===
id: cr_mun_base2
title: First Mun Outpost
sparte: Pioneers
body: Mun
prereq: cr_mun_precision_landing, cr_kerbin_station_core
reward: 180
repeatable: no
recordStation: mun_base
description: Keep two Kerbals on the Mun long enough to turn a landing site into an outpost.
check: CREW_MIN 2 | At least two Kerbals aboard
check: LANDED Mun | Landed on the Mun
check: DURATION 10 | Hold the outpost for 10 days

=== MISSION ===
id: cr_mun_base4
title: Mun Outpost Expansion
sparte: Pioneers
body: Mun
prereq: cr_mun_base2
reward: 230
repeatable: no
stationRef: mun_base
description: Expand %station% so the Mun feels less like a campsite and more like a place with schedules.
check: CREW_MIN 4 | At least four Kerbals aboard
check: LANDED Mun | Landed on the Mun
check: DURATION 20 | Hold the expanded outpost for 20 days

=== MISSION ===
id: cr_minmus_flyby
title: Crewed Minmus Flyby
sparte: Pioneers
body: Minmus
prereq: cr_mun_orbit
reward: 60
repeatable: no
description: Visit Minmus with a crewed flyby and settle, once and for all, that it is probably not dessert.
check: CREW_MIN 1 | At least one Kerbal aboard
check: FLYBY Minmus 150 | Fly by Minmus

=== MISSION ===
id: cr_minmus_orbit
title: Crewed Minmus Orbit
sparte: Pioneers
body: Minmus
prereq: cr_minmus_flyby
reward: 80
repeatable: no
description: Place a crewed spacecraft in Minmus orbit and enjoy the generous margins that make mission planners suspicious.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ORBIT_ABOVE Minmus 10 | Stable Minmus orbit, periapsis above 10 km

=== MISSION ===
id: cr_minmus_landing
title: Crewed Minmus Landing
sparte: Pioneers
body: Minmus
prereq: cr_minmus_orbit
reward: 115
repeatable: no
description: Land a crew on Minmus and learn that low gravity turns every footstep into a flight plan.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Minmus | Land on Minmus

=== MISSION ===
id: cr_minmus_refinery_scout
title: Minmus Resource Scout
sparte: Pioneers
body: Minmus
prereq: cr_minmus_landing
reward: 150
repeatable: no
description: Mine Ore on Minmus with a crew on site. The engineers want to know whether this tiny moon can become a fuel stop.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ORE_SURFACE Minmus | Mine Ore on the surface of Minmus

=== MISSION ===
id: cr_minmus_base2
title: Minmus Fuel Outpost
sparte: Pioneers
body: Minmus
prereq: cr_minmus_refinery_scout, cr_kerbin_station_expand4
reward: 210
repeatable: no
recordStation: minmus_base
description: Establish a small Minmus outpost that can support future refueling operations and suspiciously efficient interplanetary departures.
check: CREW_MIN 2 | At least two Kerbals aboard
check: LANDED Minmus | Landed on Minmus
check: DURATION 15 | Hold the outpost for 15 days

=== MISSION ===
id: cr_duna_flyby
title: Crewed Duna Flyby
sparte: Pioneers
body: Duna
prereq: cr_minmus_landing, net_deep_space_ring
reward: 130
repeatable: no
description: Send Kerbals past Duna and return with enough red-planet awe to justify a landing program.
check: CREW_MIN 1 | At least one Kerbal aboard
check: FLYBY Duna 600 | Fly by Duna

=== MISSION ===
id: cr_duna_orbit
title: Crewed Duna Orbit
sparte: Pioneers
body: Duna
prereq: cr_duna_flyby, un_duna_orbiter
reward: 170
repeatable: no
description: Enter Duna orbit with a crewed spacecraft and map the rusty world from close enough to worry about the landing legs.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Duna 70 | Stable Duna orbit, periapsis above 70 km

=== MISSION ===
id: cr_ike_landing
title: Crewed Ike Landing
sparte: Pioneers
body: Ike
prereq: cr_duna_orbit
reward: 190
repeatable: no
description: Land on Ike before committing to Duna's atmosphere. It is the sensible moon-shaped rehearsal nobody in mission control will admit they needed.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Ike | Land on Ike

=== MISSION ===
id: cr_duna_landing_return
title: Duna Landing and Return
sparte: Pioneers
body: Duna
prereq: cr_duna_orbit, un_duna_lander, net_duna_comm_network
reward: 280
repeatable: no
description: Land Kerbals on Duna and bring them safely back to Kerbin. This is the first true interplanetary surface expedition.
check: RETURN_FROM_BODY Duna Kerbin | Land crew on Duna, then return crew to Kerbin

=== MISSION ===
id: cr_duna_base2
title: First Duna Outpost
sparte: Pioneers
body: Duna
prereq: cr_duna_landing_return, un_duna_rover
reward: 340
repeatable: no
recordStation: duna_base
description: Establish a small Duna base after the first return mission. The red dust is now part of the space program budget.
check: CREW_MIN 2 | At least two Kerbals aboard
check: LANDED Duna | Landed on Duna
check: DURATION 20 | Hold the outpost for 20 days

=== MISSION ===
id: cr_eve_flyby
title: Crewed Eve Flyby
sparte: Pioneers
body: Eve
prereq: cr_duna_orbit, un_eve_probe_dip
reward: 155
repeatable: no
description: Fly a crew past Eve and let the purple planet quietly warn everyone that landing there is a very different problem.
check: CREW_MIN 1 | At least one Kerbal aboard
check: FLYBY Eve 700 | Fly by Eve

=== MISSION ===
id: cr_gilly_landing
title: Crewed Gilly Landing
sparte: Pioneers
body: Gilly
prereq: cr_eve_flyby
reward: 185
repeatable: no
description: Land on Gilly and practice Eve-system operations on a body where walking away too fast is a navigation hazard.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Gilly | Land on Gilly

=== MISSION ===
id: cr_eve_orbit
title: Crewed Eve Orbit
sparte: Pioneers
body: Eve
prereq: cr_gilly_landing, net_eve_relay_pair
reward: 210
repeatable: no
description: Place a crewed ship in Eve orbit. The surface is not the goal yet; staring at it nervously is.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Eve 95 | Stable Eve orbit, periapsis above 95 km

=== MISSION ===
id: cr_eve_deep_atmo_dip
title: Eve Deep Atmosphere Dip
sparte: Pioneers
body: Eve
prereq: cr_eve_orbit
reward: 260
repeatable: no
description: Dip a crewed craft deep into Eve's atmosphere and survive the lesson. Landing is still for the finale.
check: CREW_MIN 1 | At least one Kerbal aboard
check: ATMO_FRACTION Eve 20 65 | Dip deep into Eve's atmosphere

=== MISSION ===
id: cr_moho_flyby
title: Crewed Moho Flyby
sparte: Pioneers
body: Moho
prereq: cr_eve_orbit
reward: 170
repeatable: no
description: Visit Moho with a crewed flyby and discover that the inner system is mostly heat, speed, and nervous math.
check: CREW_MIN 1 | At least one Kerbal aboard
check: FLYBY Moho 350 | Fly by Moho

=== MISSION ===
id: cr_moho_orbit
title: Crewed Moho Orbit
sparte: Pioneers
body: Moho
prereq: cr_moho_flyby
reward: 220
repeatable: no
description: Capture into Moho orbit with crew aboard. Mission control may now openly respect transfer windows.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Moho 20 | Stable Moho orbit, periapsis above 20 km

=== MISSION ===
id: cr_moho_landing
title: Crewed Moho Landing
sparte: Pioneers
body: Moho
prereq: cr_moho_orbit
reward: 300
repeatable: no
description: Land Kerbals on Moho, plant boots near the Sun, and prove the program can handle brutal inner-system missions.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Moho | Land on Moho

=== MISSION ===
id: cr_dres_flyby
title: Crewed Dres Flyby
sparte: Pioneers
body: Dres
prereq: cr_duna_landing_return
reward: 160
repeatable: no
description: Fly past Dres and remind everyone that lonely little worlds still count.
check: CREW_MIN 1 | At least one Kerbal aboard
check: FLYBY Dres 450 | Fly by Dres

=== MISSION ===
id: cr_dres_orbit
title: Crewed Dres Orbit
sparte: Pioneers
body: Dres
prereq: cr_dres_flyby
reward: 205
repeatable: no
description: Enter orbit around Dres and turn a forgotten transfer into a real destination.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Dres 25 | Stable Dres orbit, periapsis above 25 km

=== MISSION ===
id: cr_dres_landing
title: Crewed Dres Landing
sparte: Pioneers
body: Dres
prereq: cr_dres_orbit
reward: 265
repeatable: no
description: Land on Dres with a crew and make the quiet middle system feel properly visited.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Dres | Land on Dres

=== MISSION ===
id: cr_jool_flyby
title: Crewed Jool Flyby
sparte: Pioneers
body: Jool
prereq: cr_duna_landing_return, un_jool_flyby
reward: 230
repeatable: no
description: Send a crew past Jool. Nobody lands on Jool, but everyone gets very quiet when it fills the window.
check: CREW_MIN 2 | At least two Kerbals aboard
check: FLYBY Jool 3000 | Fly by Jool

=== MISSION ===
id: cr_jool_deep_dip
title: Crewed Jool Deep Atmosphere Dip
sparte: Pioneers
body: Jool
prereq: cr_jool_flyby, un_jool_deep_probe
reward: 360
repeatable: no
description: Dip into Jool's deep atmosphere and climb back out. This is the closest a Kerbal gets to landing on a gas giant.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ATMO_FRACTION Jool 10 55 | Dip deep into Jool's atmosphere

=== MISSION ===
id: cr_laythe_orbit
title: Crewed Laythe Orbit
sparte: Pioneers
body: Laythe
prereq: cr_jool_flyby
reward: 300
repeatable: no
description: Enter Laythe orbit and stare at the suspiciously Kerbin-like blue world from a safe height.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Laythe 60 | Stable Laythe orbit, periapsis above 60 km

=== MISSION ===
id: cr_vall_landing
title: Crewed Vall Landing
sparte: Pioneers
body: Vall
prereq: cr_jool_deep_dip, net_jool_comm_network
reward: 330
repeatable: no
description: Land on Vall before trying Laythe. The Jool system demands at least one icy rehearsal.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Vall | Land on Vall

=== MISSION ===
id: cr_bop_landing
title: Crewed Bop Landing
sparte: Pioneers
body: Bop
prereq: cr_jool_deep_dip
reward: 300
repeatable: no
description: Land on Bop and add the strange little moon to the program's growing list of places that should not be underestimated.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Bop | Land on Bop

=== MISSION ===
id: cr_pol_landing
title: Crewed Pol Landing
sparte: Pioneers
body: Pol
prereq: cr_bop_landing
reward: 310
repeatable: no
description: Land on Pol and prove the crew can handle the outer system's smaller, sharper problems.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Pol | Land on Pol

=== MISSION ===
id: cr_tylo_landing
title: Crewed Tylo Landing
sparte: Pioneers
body: Tylo
prereq: cr_vall_landing, cr_minmus_base2
reward: 460
repeatable: no
description: Land on Tylo, where gravity is rude and engines are judged honestly.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Tylo | Land on Tylo

=== MISSION ===
id: cr_laythe_landing_return
title: Laythe Landing and Return
sparte: Pioneers
body: Laythe
prereq: cr_laythe_orbit, cr_vall_landing, net_laythe_surface_link
reward: 560
repeatable: no
description: Land Kerbals on Laythe and bring them back to Kerbin. The program has now crossed the ocean-moon threshold.
check: RETURN_FROM_BODY Laythe Kerbin | Land crew on Laythe, then return crew to Kerbin

=== MISSION ===
id: cr_eeloo_flyby
title: Crewed Eeloo Flyby
sparte: Pioneers
body: Eeloo
prereq: cr_dres_landing, un_eeloo_flyby
reward: 220
repeatable: no
description: Send a crew to the edge of the stock system and wave at Eeloo before the sunlight gets too faint for dramatic speeches.
check: CREW_MIN 2 | At least two Kerbals aboard
check: FLYBY Eeloo 600 | Fly by Eeloo

=== MISSION ===
id: cr_eeloo_orbit
title: Crewed Eeloo Orbit
sparte: Pioneers
body: Eeloo
prereq: cr_eeloo_flyby, net_eeloo_comm_pair
reward: 290
repeatable: no
description: Capture into Eeloo orbit with crew aboard and make the far edge of the system feel reachable.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Eeloo 30 | Stable Eeloo orbit, periapsis above 30 km

=== MISSION ===
id: cr_eeloo_landing
title: Crewed Eeloo Landing
sparte: Pioneers
body: Eeloo
prereq: cr_eeloo_orbit
reward: 390
repeatable: no
description: Land on Eeloo with a crew and close the outer-system surface chapter.
check: CREW_MIN 1 | At least one Kerbal aboard
check: LANDED Eeloo | Land on Eeloo

=== MISSION ===
id: cr_eve_landing_return
title: Eve Landing and Return
sparte: Pioneers
body: Eve
prereq: cr_laythe_landing_return, cr_duna_landing_return, cr_moho_landing, cr_eve_deep_atmo_dip, net_eve_relay_pair
reward: 900
repeatable: no
description: Land Kerbals on Eve and bring them home. This is the final exam: gravity, atmosphere, ascent design, and nerve.
check: RETURN_FROM_BODY Eve Kerbin | Land crew on Eve, then return crew to Kerbin

=== MISSION ===
id: cr_stock_grand_tour_review
title: Stock System Grand Tour Debrief
sparte: Pioneers
body: Kerbin
prereq: cr_mun_landing, cr_minmus_landing, cr_duna_landing_return, cr_ike_landing, cr_gilly_landing, cr_moho_landing, cr_dres_landing, cr_vall_landing, cr_tylo_landing, cr_bop_landing, cr_pol_landing, cr_laythe_landing_return, cr_eeloo_landing, cr_eve_landing_return
reward: 400
repeatable: no
description: Return to Kerbin orbit after crewed landings across the stock system and let the mission board pretend it planned all of this from the beginning.
check: CREW_MIN 1 | At least one veteran Kerbal aboard
check: ORBIT_ABOVE Kerbin 100 | Stable Kerbin orbit, periapsis above 100 km

=== MISSION ===
id: un_mun_probe_orbit
title: Mun Probe Orbiter
sparte: Robotic Explorers
body: Mun
prereq: cr_kerbin_orbit
reward: 35
repeatable: no
description: Send an uncrewed probe into Mun orbit to scout the first serious landing target.
check: CREW_NONE | Uncrewed vessel
check: ORBIT_ABOVE Mun 12 | Stable Mun orbit, periapsis above 12 km

=== MISSION ===
id: un_mun_lander
title: Mun Test Lander
sparte: Robotic Explorers
body: Mun
prereq: un_mun_probe_orbit
reward: 50
repeatable: no
description: Land an uncrewed craft on the Mun before sending Kerbals down the ladder.
check: CREW_NONE | Uncrewed vessel
check: LANDED Mun | Land on the Mun

=== MISSION ===
id: un_mun_rover
title: Mun Rover Pathfinder
sparte: Robotic Explorers
body: Mun
prereq: un_mun_lander
reward: 70
repeatable: no
description: Put an uncrewed rover-class craft down near a marked Mun site and gather practical surface data for crewed precision landings.
check: CREW_NONE | Uncrewed vessel
check: MARKER_LANDING Mun 12 | Land within 12 km of the Mun rover target

=== MISSION ===
id: un_minmus_mapper
title: Minmus Polar Mapper
sparte: Robotic Explorers
body: Minmus
prereq: cr_mun_landing
reward: 55
repeatable: no
description: Map Minmus from a polar orbit before the fuel-outpost plans get too enthusiastic.
check: CREW_NONE | Uncrewed vessel
check: ORBIT_ABOVE Minmus 10 | Stable Minmus orbit, periapsis above 10 km
check: INCLINATION_MIN Minmus 75 | Inclination at least 75 degrees

=== MISSION ===
id: un_duna_flyby
title: Duna Probe Flyby
sparte: Robotic Explorers
body: Duna
prereq: cr_kerbin_station_core, net_deep_space_ring
reward: 65
repeatable: no
description: Send the first uncrewed probe past Duna and collect the data that makes the crewed plan look less like guessing.
check: CREW_NONE | Uncrewed vessel
check: FLYBY Duna 800 | Fly by Duna

=== MISSION ===
id: un_duna_orbiter
title: Duna Survey Orbiter
sparte: Robotic Explorers
body: Duna
prereq: un_duna_flyby
reward: 90
repeatable: no
description: Establish an uncrewed Duna orbiter to scout landing zones and relay the first proper red-planet maps.
check: CREW_NONE | Uncrewed vessel
check: ORBIT_ABOVE Duna 70 | Stable Duna orbit, periapsis above 70 km

=== MISSION ===
id: un_duna_lander
title: Duna Test Lander
sparte: Robotic Explorers
body: Duna
prereq: un_duna_orbiter
reward: 115
repeatable: no
description: Land an uncrewed craft on Duna to prove the atmosphere can be used without being trusted.
check: CREW_NONE | Uncrewed vessel
check: LANDED Duna | Land on Duna

=== MISSION ===
id: un_duna_rover
title: Duna Rover Pathfinder
sparte: Robotic Explorers
body: Duna
prereq: un_duna_lander
reward: 140
repeatable: no
description: Place a rover-class craft near a marked Duna site and give the future outpost a better map than "red hill near red valley."
check: CREW_NONE | Uncrewed vessel
check: MARKER_LANDING Duna 15 | Land within 15 km of the Duna rover target

=== MISSION ===
id: un_eve_probe_dip
title: Eve Atmospheric Probe
sparte: Robotic Explorers
body: Eve
prereq: cr_minmus_landing
reward: 80
repeatable: no
description: Dip an uncrewed probe into Eve's atmosphere so the crewed program can learn fear from telemetry.
check: CREW_NONE | Uncrewed vessel
check: ATMO_FRACTION Eve 20 75 | Fly through Eve's lower atmosphere

=== MISSION ===
id: un_jool_flyby
title: First Jool Probe
sparte: Robotic Explorers
body: Jool
prereq: un_duna_orbiter, net_deep_space_ring
reward: 130
repeatable: no
description: Send a probe to Jool and confirm that the green giant is large, dramatic, and not a landing target.
check: CREW_NONE | Uncrewed vessel
check: FLYBY Jool 4000 | Fly by Jool

=== MISSION ===
id: un_jool_deep_probe
title: Jool Deep Atmosphere Probe
sparte: Robotic Explorers
body: Jool
prereq: un_jool_flyby
reward: 170
repeatable: no
description: Send an uncrewed probe deep into Jool's atmosphere before any Kerbal tries the same stunt and calls it science.
check: CREW_NONE | Uncrewed vessel
check: ATMO_FRACTION Jool 5 45 | Dip deep into Jool's atmosphere

=== MISSION ===
id: un_laythe_probe_orbit
title: Laythe Probe Orbiter
sparte: Robotic Explorers
body: Laythe
prereq: un_jool_flyby
reward: 135
repeatable: no
description: Park a probe in Laythe orbit to inspect the one moon that looks almost too friendly.
check: CREW_NONE | Uncrewed vessel
check: ORBIT_ABOVE Laythe 60 | Stable Laythe orbit, periapsis above 60 km

=== MISSION ===
id: un_eeloo_flyby
title: Eeloo Probe Flyby
sparte: Robotic Explorers
body: Eeloo
prereq: un_jool_flyby
reward: 120
repeatable: no
description: Send an uncrewed probe to Eeloo and prove the space program can still work where the Sun is mostly a suggestion.
check: CREW_NONE | Uncrewed vessel
check: FLYBY Eeloo 800 | Fly by Eeloo

=== MISSION ===
id: un_eeloo_orbiter
title: Eeloo Survey Orbiter
sparte: Robotic Explorers
body: Eeloo
prereq: un_eeloo_flyby
reward: 155
repeatable: no
description: Establish an uncrewed orbiter around Eeloo to prepare the farthest crewed landing.
check: CREW_NONE | Uncrewed vessel
check: ORBIT_ABOVE Eeloo 30 | Stable Eeloo orbit, periapsis above 30 km

=== MISSION ===
id: un_eeloo_lander
title: Eeloo Test Lander
sparte: Robotic Explorers
body: Eeloo
prereq: un_eeloo_orbiter
reward: 190
repeatable: no
description: Land an uncrewed craft on Eeloo and make the eventual crewed descent feel slightly less outrageous.
check: CREW_NONE | Uncrewed vessel
check: LANDED Eeloo | Land on Eeloo

=== MISSION ===
id: net_kerbin_comm_network3
title: Kerbin Relay Network
sparte: Lifelines
body: Kerbin
prereq: cr_kerbin_orbit
reward: 60
repeatable: no
description: Build the first proper communication network around Kerbin with three satellites above low orbit.
check: VESSEL_COUNT Kerbin 3 2000 | Three satellites in Kerbin orbit, periapsis above 2000 km

=== MISSION ===
id: net_kerbin_polar_comm_network
title: Kerbin Polar Relay Layer
sparte: Lifelines
body: Kerbin
prereq: net_kerbin_comm_network3
reward: 85
repeatable: no
description: Add a polar layer so the network keeps talking even when mission geometry gets awkward.
check: VESSEL_COUNT_INCLINATION Kerbin 3 75 2000 | Three polar Kerbin relays, inclination at least 75 degrees

=== MISSION ===
id: net_mun_comm_network
title: Mun Relay Network
sparte: Lifelines
body: Mun
prereq: cr_mun_orbit, un_mun_probe_orbit
reward: 75
repeatable: no
description: Place three relay satellites around the Mun before the surface program gets busy.
check: VESSEL_COUNT Mun 3 60 | Three satellites in Mun orbit, periapsis above 60 km

=== MISSION ===
id: net_minmus_comm_network
title: Minmus Relay Network
sparte: Lifelines
body: Minmus
prereq: cr_minmus_orbit, un_minmus_mapper
reward: 80
repeatable: no
description: Extend the relay network to Minmus so fuel operations do not depend on heroic antenna pointing.
check: VESSEL_COUNT Minmus 3 60 | Three satellites in Minmus orbit, periapsis above 60 km

=== MISSION ===
id: net_deep_space_ring
title: Deep Space Relay Ring
sparte: Lifelines
body: Sun
subcategory: Outer System
prereq: net_kerbin_comm_network3, cr_kerbin_station_core
reward: 110
repeatable: no
description: Build a solar-orbit relay ring to support the first interplanetary expeditions.
check: VESSEL_COUNT Sun 4 3000000 | Four relays in solar orbit, periapsis above 3000000 km

=== MISSION ===
id: net_duna_comm_network
title: Duna Relay Network
sparte: Lifelines
body: Duna
prereq: un_duna_orbiter, net_deep_space_ring
reward: 130
repeatable: no
description: Build a relay network around Duna before the first crewed landing and return attempt.
check: VESSEL_COUNT Duna 3 150 | Three satellites in Duna orbit, periapsis above 150 km

=== MISSION ===
id: net_eve_relay_pair
title: Eve Relay Pair
sparte: Lifelines
body: Eve
prereq: cr_eve_flyby, net_deep_space_ring
reward: 125
repeatable: no
description: Place a relay pair around Eve to support early orbital operations and the eventual return attempt.
check: VESSEL_COUNT Eve 2 250 | Two satellites in Eve orbit, periapsis above 250 km

=== MISSION ===
id: net_jool_comm_network
title: Jool Relay Network
sparte: Lifelines
body: Jool
prereq: un_jool_flyby, net_deep_space_ring
reward: 190
repeatable: no
description: Build a high relay network around Jool so the moon landings do not vanish behind the biggest planet in the system.
check: VESSEL_COUNT Jool 3 1200 | Three satellites in Jool orbit, periapsis above 1200 km

=== MISSION ===
id: net_laythe_surface_link
title: Laythe Surface Link
sparte: Lifelines
body: Laythe
prereq: cr_laythe_orbit, un_laythe_probe_orbit, net_jool_comm_network
reward: 170
repeatable: no
description: Put a relay layer around Laythe before committing Kerbals to landing and coming home.
check: VESSEL_COUNT Laythe 2 120 | Two satellites in Laythe orbit, periapsis above 120 km

=== MISSION ===
id: net_eeloo_comm_pair
title: Eeloo Relay Pair
sparte: Lifelines
body: Eeloo
prereq: un_eeloo_orbiter, net_deep_space_ring
reward: 170
repeatable: no
description: Build a small relay pair around Eeloo for the farthest crewed landing.
check: VESSEL_COUNT Eeloo 2 90 | Two satellites in Eeloo orbit, periapsis above 90 km

=== MISSION ===
id: net_kerbin_fuel_depot
title: Kerbin Orbital Fuel Depot
sparte: Lifelines
body: Kerbin
prereq: cr_kerbin_station_expand4
reward: 130
repeatable: no
recordStation: kerbin_fuel_depot
description: Build a crew-supported fuel depot in Kerbin orbit. Future deep-space craft will thank you by being slightly less enormous.
check: CREW_MIN 2 | At least two Kerbals aboard
check: ORBIT_ABOVE Kerbin 100 | Stable Kerbin orbit, periapsis above 100 km
check: RESOURCE_MIN LiquidFuel 1440 | At least 1440 LiquidFuel aboard
check: RESOURCE_MIN Oxidizer 1760 | At least 1760 Oxidizer aboard

=== MISSION ===
id: net_minmus_fuel_depot
title: Minmus Fuel Depot
sparte: Lifelines
body: Minmus
prereq: cr_minmus_base2, net_minmus_comm_network
reward: 180
repeatable: no
recordStation: minmus_fuel_depot
description: Establish a fuel depot on Minmus to make interplanetary launches feel almost unfair.
check: CREW_MIN 2 | At least two Kerbals aboard
check: LANDED Minmus | Landed on Minmus
check: RESOURCE_MIN LiquidFuel 1440 | At least 1440 LiquidFuel aboard
check: RESOURCE_MIN Oxidizer 1760 | At least 1760 Oxidizer aboard

=== MISSION ===
id: net_duna_supply_depot
title: Duna Supply Depot
sparte: Lifelines
body: Duna
prereq: cr_duna_base2, net_duna_comm_network
reward: 220
repeatable: no
recordStation: duna_supply_depot
description: Stock a Duna surface depot so the base has more than optimism between transfer windows.
check: CREW_MIN 2 | At least two Kerbals aboard
check: LANDED Duna | Landed on Duna
check: RESOURCE_MIN LiquidFuel 720 | At least 720 LiquidFuel aboard
check: RESOURCE_MIN Oxidizer 880 | At least 880 Oxidizer aboard
