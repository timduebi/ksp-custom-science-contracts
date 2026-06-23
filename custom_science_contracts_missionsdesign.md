# WISSENSCHAFT

Das Missionssystem bleibt ein Bonus zum normalen Science-Mode und ersetzt Stock-Science nicht. Die Belohnungen unterstützen den Fortschritt, aber Experimente, Proben und eigene Erkundung bleiben die Hauptquelle für grosse Tech-Sprünge.

Frühe Erdmissionen geben kleine Werte, Luna und cislunare Infrastruktur geben spürbare mittlere Werte, Mars gibt hohe Werte, und die äusseren Welten liefern grosse Einzelboni. Wiederholbare Versorgung, Depots und Logistik zahlen moderat aus, damit sie nützlich bleiben, aber keine Erstmission verdrängen.

Grobe Skala: frühe Erde 6–130, Luna 70–225, innere Planetenziele 100–550, Mars bemannt 540–630, Asteroiden 65–450, Jupiter 235–1150, Saturn und Titan 215–1860, Fernsonden 245–1500, Titanbasis-Finale 1350–2280.

# ABLAUF

Die Kampagne beginnt historisch und vorsichtig. Unbemannte Testflüge verlassen zuerst den Startbereich, steigen in die obere Atmosphäre, verlassen die Atmosphäre suborbital und erreichen anschliessend den ersten Erdorbit. Erst danach folgen bemannte suborbitale Flüge, der erste bemannte Erdorbit, die erste EVA, ein Docking im Erdorbit, ein 7-Tage-Flug und ein temporäres Ein-Modul-Labor. Dieses Labor dauert 15 Tage, bleibt kein Teil der späteren Stationskette und wird danach narrativ deorbitiert.

Luna ist der erste grosse Horizont. Nach der ersten EVA im Erdorbit wird der unbemannte Luna-Vorbeiflug geöffnet, danach folgen unbemannter Luna-Orbit und unbemannte Luna-Landung. Der bemannte Luna-Vorbeiflug `cr_luna_flyby_crewed` ist der Meilenstein, ab dem gesperrte Aufträge als Vorschau erscheinen. Der erste bemannte Luna-Orbit braucht den bemannten Vorbeiflug und die erste unbemannte Landung. Die erste bemannte Luna-Landung enthält direkt die erste Oberflächen-EVA.

Nach der ersten bemannten Luna-Landung beginnt die echte Erdstation bereits in Moonrise. Sie ist eine neue Stationskette und hat keine technische Verbindung zum temporären Ein-Modul-Labor. Stationsbau und Stationsausbau müssen leer und unbemannt sein; sie prüfen zunächst nur verfügbare Plätze. Kerbals werden erst bei Versorgung und Langzeitbetrieb gezählt. Die Erdstation startet mit 3 Plätzen, wächst in Moonrise bis 4 Plätze, in Inner Reach bis 8 Plätze und in Beltworks bis 12 Plätze. Nach dem Ausbau der Erdstation auf 4 Plätze werden zwei optionale Luna-Landungen als Standorttests für spätere Mondbasen geöffnet. Die Mondstation startet direkt mit 3 Plätzen. Eine Mondbasis setzt den 150-Tage-Betrieb der 3er-Mondstation voraus. Das Erdorbit-Fuel-Depot ist ein optionaler Versorgungsnetz-Zweig, öffnet später als die Erdstation und blockiert keine bemannte Marsmission.

Ab Epoche 3 wird das frühe Erd-Satellitennetz aus der Pionierzeit narrativ zu alt. Im Versorgungsnetz entsteht deshalb eine neue Kommunikations-Lebensader: zuerst Erde, dann Luna, danach Mars, ein interplanetarer Sonnenorbit-Ring und schliesslich die grossen Relais um Jupiter und Saturn. Erde und Luna bekommen je eine Grundkonstellation mit drei Satelliten und danach eine polare Ausbaustufe mit drei weiteren Satelliten.

Das innere Sonnensystem öffnet sich robotisch nach dem bemannten Luna-Vorbeiflug. Nach den ersten Flybys folgen die ersten robotischen Orbits früher: Venus und Merkur können schon in der Flyby-Epoche in einen Orbit gehen, Mars-Orbit und erste robotische Marslandung rücken eine Epoche nach vorne. Venus bekommt die Landung weiterhin erst später. Bemannt bleiben Venus-Vorbeiflug und Venus-Orbit optional. Für bemannten Mars-Vorbeiflug und bemannten Mars-Orbit reicht die normale Erdstation mit Longstay-Stufe 3.

Mars ist der zweite grosse bemannte Hauptbogen. Nach bemanntem Mars-Orbit, robotischer Präzisionslandung und Langzeiterfahrung folgen bemannte Marslandung mit EVA, 10 Tage auf Mars und 30 Tage auf Mars. Phobos und Deimos werden nach bemannter Marslandung direkt als bemannte Landungen verfügbar; eigene bemannte Orbitmissionen für diese kleinen Monde gibt es nicht mehr. Das Versorgungsdepot entsteht im Orbit um Phobos; ein eigenes Deimos-Depot gibt es nicht mehr. Marsstation und Marsbasis starten danach als neue Infrastruktur.

Asteroiden sind vollständig Bonus. Eros, Vesta, Ceres, Pallas, Psyche, Ryugu, Ida, Dactyl und Arrokoth geben Wissenschaft, Ressourcenoptionen und erzählerische Tiefe, blockieren aber keine Hauptmission zu Mars, Jupiter, Saturn, Titan, Triton, Pluto und Arrokoth. Ceres ist das einzige bemannte Asteroidenziel und bleibt ein Prestigeast: Nach dem bemannten Vorbeiflug ist die Landung direkt möglich, der bemannte Ceres-Orbit bleibt optional.

Jupiter wird robotisch früh sichtbar: der unbemannte Jupiter-Vorbeiflug öffnet nach unbemanntem Mars-Orbit, der unbemannte Jupiter-Orbit folgt direkt danach. Io, Europa, Ganymed und Kallisto erhalten robotische Vorbeiflüge. Kallisto erhält einen optionalen Ressourcenzweig. Ganymed ist das einzige bemannte Landungsziel im Jupitersystem.

Saturn öffnet nach dem unbemannten Jupiter-Orbit. Im Saturnsystem erhalten nur Titan und Enceladus Orbit- und Landemissionen. Rhea, Iapetus, Dione, Tethys, Mimas, Hyperion und Phoebe bleiben Vorbeiflugziele. Titan ist das letzte reguläre bemannte Landungsziel. Nach dem unbemannten Saturn-Vorbeiflug werden Uranus, Neptun, Pluto und Arrokoth gleichzeitig sichtbar.

Die Schluss-Epoche bleibt robotisch. Triton und Pluto erhalten jeweils Vorbeiflug, Orbit und Landung. Uranus, Uranusmonde, Neptunmonde, Charon und Arrokoth bleiben Vorbeiflugziele. Gasriesen und Eisriesen erhalten nach dem jeweiligen Vorbeiflug eine unbemannte Atmosphärensonde. Als letzter bemannter Abschluss entsteht auf Titan eine Sonderbasis mit Crew 4, danach Crew 6 und Crew 8, ohne normale Versorgungskette zwischen den Ausbaustufen.

# STATIONSKETTEN

chain: body=Earth | key=earth_station | typ=station | prereq=cr_luna_landing | stufen=3,4,6,8,10,12
chain: body=Earth | key=earth_fuel_depot | typ=station | prereq=cr_earth_station_longstay4 | stufen=2,3,4,6
chain: body=Moon | key=moon_station | typ=station | prereq=cr_earth_station_longstay4 | stufen=3,4,6,8,10
chain: body=Moon | key=moon_base | typ=base | prereq=cr_moon_station_longstay3 | stufen=2,3,4,6,8,10
chain: body=Mars | key=mars_station | typ=station | prereq=cr_mars_stay_10d | stufen=2,3,4,6
chain: body=Mars | key=mars_base | typ=base | prereq=cr_mars_stay_30d | stufen=2,3,4,6

# GENERIERTE STRUKTURREGELN

Diese Regeln werden beim Erzeugen der Kataloge angewendet und gehören zum aktuellen Missionsstand.

- Das Andocktraining erzeugt zuerst `cr_earth_docking_target`. `cr_earth_docking_demo` hängt an diesem Ziel und prüft ein Docking mit dem registrierten Ziel statt ein beliebiges Docking.
- Stationsbau und Stationsausbau in Orbitketten prüfen `CREW_NONE`, `CREW_CAPACITY_MIN`, `ORBIT_ABOVE`, `APOAPSIS_MAX` und 10 Tage Stabilität. Sie müssen leer sein; Crew zählt erst bei Versorgung und Longstay.
- Stationsversorgung prüft Crew an Bord des Versorgungsschiffs, Zielorbit, maximale Apoapsis und Docking mit der registrierten Station.
- Stations-Longstay prüft Crew an Bord der Station, Zielorbit, maximale Apoapsis und 150 Tage Dauerbetrieb.
- Die Erdstation läuft 3, 4, 6, 8, 10, 12 Plätze. Moonrise enthält 3 und 4 Plätze, Inner Reach enthält 6 und 8 Plätze, Beltworks enthält 10 und 12 Plätze.
- Die Mondstation läuft 3, 4, 6, 8, 10 Plätze und startet nach `cr_earth_station_longstay4`.
- Die Mondbasis läuft 2, 3, 4, 6, 8, 10 Kerbals und startet nach `cr_moon_station_longstay3`.
- Nach `cr_earth_station_expand4` entstehen zwei optionale Luna-Standorttests: `cr_luna_station_precision_landing_1` und `cr_luna_station_precision_landing_2`. Sie blockieren keine spätere Mission.
- Alle bemannten Orbitmissionen bekommen eine maximale Apoapsis. Wenn keine echte Dauerforderung vorhanden ist, wird mindestens `DURATION 0.5` gesetzt.
- Alle bemannten Nicht-Basis-Landungen und Flybys müssen sicher zur Erde zurückkehren, wenn der Missionsblock keinen eigenen Rückkehrcheck trägt.
- Rovermissionen prüfen Räder und mindestens 4 m/s Bodenbewegung.
- Satellitennetzwerke prüfen Relaisantennen.
- Wiederholbare Missionen wandern nach dem ersten Abschluss in den Repeatables-Pool.

# MISSIONEN

=== MISSION ===
id: un_earth_pad_clear
sparte: Robotische Erkunder
body: Earth
prereq: -
reward: 6
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke deinen ersten unbemannten Prüfkörper knapp über den Startplatz und sammle die allerersten Telemetriedaten. Aus Plänen wird Bewegung — und jeder grosse Flug deines Programms beginnt mit diesem kleinen Hüpfer.
beschreibung_en: Send your first uncrewed test article just clear of the launch pad and gather the very first telemetry. Plans turn into motion — and every great flight of your program begins with this small hop.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Earth 1 10 | untere Atmosphäre erreicht

=== MISSION ===
id: un_earth_upper_atmo
sparte: Robotische Erkunder
body: Earth
prereq: un_earth_pad_clear
reward: 10
repeatable: no
recordStation: -
stationRef: -
beschreibung: Treibe die nächste Testkapsel hinauf in die dünne, kalte Oberluft. Die Messungen dort oben geben deinem Team das Vertrauen für schnellere, höhere Flüge — der Orbit ist nicht mehr weit.
beschreibung_en: Push the next test capsule up into the thin, cold upper air. The readings up there give your team the confidence for faster, higher flights — orbit is no longer far away.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Earth 60 90 | obere Atmosphäre erreicht

=== MISSION ===
id: un_earth_suborbital
sparte: Robotische Erkunder
body: Earth
prereq: un_earth_upper_atmo
reward: 18
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass dein erstes unbemanntes Fahrzeug die Atmosphäre durchstossen und für wenige Minuten ins Schwarze gleiten. Ein suborbitaler Sprung, der aus einem Testprogramm den Anfang echter Raumfahrt macht.
beschreibung_en: Let your first uncrewed vehicle punch through the atmosphere and coast for a few minutes into the black. A suborbital leap that turns a test program into the start of real spaceflight.
check: CREW_NONE | unbemannt
check: SUBORBITAL Earth | suborbitaler Raumflug über Erde

=== MISSION ===
id: cr_earth_suborbital
sparte: Pioniere
body: Earth
prereq: un_earth_suborbital
reward: 28
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke den ersten Kerbal über die Atmosphäre hinaus — ein kurzer, mutiger Sprung ins Schwarze. Dieser Flug wird als erster bemannter Schritt deines Programms in die Geschichte eingehen.
beschreibung_en: Send the first Kerbal beyond the atmosphere — a short, brave jump into the black. This flight will go down in history as your program's first crewed step.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: SUBORBITAL Earth | suborbitaler Raumflug über Erde

=== MISSION ===
id: un_earth_orbit
sparte: Robotische Erkunder
body: Earth
prereq: un_earth_suborbital
reward: 32
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe deine erste Sonde in eine stabile Erdumlaufbahn und halte sie dort. Damit schreibt dein Programm ein kleines Stück Raumfahrtgeschichte — und beweist, dass du nicht nur hochfliegen, sondern oben bleiben kannst. Der erste bemannte Orbit rückt in greifbare Nähe.
beschreibung_en: Place your first probe into a stable Earth orbit and hold it there. Your program writes a small piece of spaceflight history — proving you can not only fly high but stay up. The first crewed orbit is within reach.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_earth_science_satellite
sparte: Robotische Erkunder
body: Earth
prereq: un_earth_orbit
reward: 38
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze deinen ersten echten Forschungssatelliten aus und lass ihn einen vollen Tag über der Erde arbeiten. Dein Programm lernt, den Orbit nicht nur zu erreichen, sondern als Werkzeug zu nutzen.
beschreibung_en: Deploy your first real research satellite and keep it working over Earth for a full day. Your program learns to use orbit not just as a destination but as a tool.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: un_earth_satellite_pair
sparte: Robotische Erkunder
body: Earth
prereq: un_earth_science_satellite
reward: 48
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe drei Satelliten gleichzeitig in die Erdumlaufbahn und halte sie einen Tag im Betrieb. Aus einzelnen Geräten wird ein erstes Netz — die Keimzelle künftiger Navigation und Kommunikation.
beschreibung_en: Place three satellites into Earth orbit at once and keep them operating for a day. Single instruments become a first network — the seed of future navigation and communication.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Earth 3 | 3 Fahrzeuge gleichzeitig im Erdorbit
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: un_earth_high_satellite
sparte: Robotische Erkunder
body: Earth
prereq: un_earth_satellite_pair
reward: 56
repeatable: no
recordStation: -
stationRef: -
beschreibung: Platziere einen Satelliten weit draussen im hohen Erdorbit und halte ihn einen Tag auf Position. Von dort gewinnt dein Programm Überblick, Reichweite und ein Gefühl für die grossen Bahnen, die zu Luna und Mars führen.
beschreibung_en: Place a satellite far out in high Earth orbit and hold it on station for a day. From there your program gains reach, overview and a feel for the wide orbits that lead to the Moon and Mars.
check: CREW_NONE | unbemannt
check: VESSEL_COUNT Earth 1 1000 | 1 Fahrzeug im hohen Erdorbit
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: cr_earth_orbit
sparte: Pioniere
body: Earth
prereq: cr_earth_suborbital, un_earth_orbit
reward: 56
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe den ersten Kerbal in eine stabile Erdumlaufbahn. Ein historischer Moment, der aus deinem Raumfahrtprogramm eine ernstzunehmende Kraft macht — und den Weg zu Ausstieg, Docking und Station ebnet.
beschreibung_en: Put the first Kerbal into a stable Earth orbit. A historic moment that turns your space program into a serious force — and clears the way to EVA, docking and stations.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_earth_orbit_eva
sparte: Pioniere
body: Earth
prereq: cr_earth_orbit
reward: 68
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass einen Kerbal das Fahrzeug verlassen und frei im Vakuum schweben. Dieser erste Ausstieg ist die Grundlage für alles, was später Hände im All braucht: Stationen, Reparaturen, ferne Landungen.
beschreibung_en: Send a Kerbal outside the vehicle to float free in the vacuum. This first spacewalk is the foundation for everything that later needs hands in space: stations, repairs, distant landings.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: EVA Earth ORBITING | EVA im Erdorbit

=== MISSION ===
id: cr_earth_duration_3d
sparte: Pioniere
body: Earth
prereq: cr_earth_orbit_eva
reward: 76
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte eine Besatzung drei Tage ununterbrochen im Erdorbit. Dein Programm sammelt die erste echte Erfahrung darin, dass Kerbals im All nicht nur kurz zu Gast sind, sondern arbeiten können.
beschreibung_en: Keep a crew in Earth orbit for three uninterrupted days. Your program gains its first real experience that Kerbals are not just brief visitors in space, but can work there.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DURATION 3 | 3 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_earth_docking_demo
sparte: Pioniere
body: Earth
prereq: cr_earth_docking_target
reward: 88
repeatable: no
recordStation: -
stationRef: -
beschreibung: Führe das erste Andockmanöver zwischen zwei Fahrzeugen im Erdorbit durch. Diese Technik wird später Stationen, Depots und interplanetare Schiffe zusammenfügen — heute übst du sie zum ersten Mal.
beschreibung_en: Perform the first docking between two vehicles in Earth orbit. This technique will later join stations, depots and interplanetary ships — today you practice it for the first time.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DOCK_STATION earth_docking_target | Am Andockziel angedockt

=== MISSION ===
id: cr_earth_duration_7d
sparte: Pioniere
body: Earth
prereq: cr_earth_docking_demo
reward: 104
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass eine Besatzung eine ganze Woche im Erdorbit leben und arbeiten. Sieben Tage Routine im All sind die Generalprobe für die langen Reisen nach Luna und Mars.
beschreibung_en: Have a crew live and work in Earth orbit for a full week. Seven days of routine in space are the dress rehearsal for the long voyages to the Moon and Mars.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DURATION 7 | 7 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_earth_trial_station
sparte: Pioniere
body: Earth
prereq: cr_earth_duration_7d, un_earth_high_satellite
reward: 128
repeatable: no
recordStation: -
stationRef: -
beschreibung: Betreibe ein kleines Ein-Modul-Labor mit genau zwei Kerbals fünfzehn Tage lang im Erdorbit. Ein erster Stationsversuch deines Programms — danach wird das Modul ausgemustert, doch die Erfahrung bleibt.
beschreibung_en: Operate a small single-module lab with exactly two Kerbals for fifteen days in Earth orbit. Your program's first attempt at a station — the module is retired afterwards, but the experience stays.
check: CREW_EXACT 2 | genau 2 Kerbals an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DURATION 15 | 15 Tage ununterbrochen ausharren

=== MISSION ===
id: un_luna_flyby
sparte: Robotische Erkunder
body: Moon
prereq: cr_earth_orbit_eva
reward: 72
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde dicht an Luna vorbei. Zum ersten Mal berührt dein Programm den Raum einer anderen Welt — ein Vorgeschmack auf alles, was jenseits der Erde wartet.
beschreibung_en: Send a probe close past the Moon. For the first time your program touches the space of another world — a taste of everything that waits beyond Earth.
check: CREW_NONE | unbemannt
check: FLYBY Moon 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_luna_orbit
sparte: Robotische Erkunder
body: Moon
prereq: un_luna_flyby
reward: 92
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in einen stabilen Mondorbit und beginne, Luna zu kartieren. Aus dem flüchtigen Besuch wird systematische Erkundung — die Grundlage für die erste Landung.
beschreibung_en: Place a probe into a stable lunar orbit and begin mapping the Moon. A fleeting visit becomes systematic exploration — the groundwork for the first landing.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Moon 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_luna_polar_mapping
sparte: Robotische Erkunder
body: Moon
prereq: un_luna_orbit
reward: 130
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in einen polaren Mondorbit und kartiere Luna von Pol zu Pol. Die zusätzlichen Bahnen zeigen Landegebiete, Schattenzonen und Übergänge, die ein normaler Äquatororbit nur streift.
beschreibung_en: Place a probe into a polar lunar orbit and map the Moon from pole to pole. The extra passes reveal landing sites, shadow zones and transitions that an equatorial orbit only grazes.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Moon 100 | Periapsis über 100 km
check: INCLINATION_MIN Moon 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_luna_polar_landing
sparte: Robotische Erkunder
body: Moon
prereq: un_luna_polar_mapping
reward: 160
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde punktgenau in der Polregion von Luna ab. Die Eis- und Schattenzonen der Pole sind für spätere Basen besonders interessant — und eine Landung dort ist deutlich anspruchsvoller als am Äquator.
beschreibung_en: Set a probe down precisely in the Moon's polar region. The ice and shadow zones of the poles are especially interesting for later bases — and landing there is far harder than at the equator.
check: CREW_NONE | unbemannt
check: LANDED Moon | auf Luna gelandet
check: MARKER_LANDING Moon 8 70 90 | Pollandung im Umkreis von 8 km

=== MISSION ===
id: cr_luna_flyby_crewed
sparte: Pioniere
body: Moon
prereq: un_luna_orbit, cr_earth_duration_3d
reward: 128
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke erstmals Kerbals bis zum Mond und sicher wieder zurück. Ein bemannter Vorbeiflug, der beweist: deine Crew kann den Heimatorbit verlassen. Ab hier zeichnen sich am Horizont schon die nächsten grossen Ziele ab.
beschreibung_en: Send Kerbals out to the Moon and safely back for the first time. A crewed flyby that proves your crew can leave home orbit. From here the next great targets already loom on the horizon.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: FLYBY Moon 800 | Vorbeiflug unter 800 km

=== MISSION ===
id: un_luna_landing
sparte: Robotische Erkunder
body: Moon
prereq: un_luna_orbit
reward: 104
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine unbemannte Sonde sanft auf dem Mond ab. Diese Landung macht den Boden vermessbar und die erste bemannte Mission zur Oberfläche verantwortbar.
beschreibung_en: Set an uncrewed probe down softly on the Moon. This landing makes the surface measurable and the first crewed mission to it justifiable.
check: CREW_NONE | unbemannt
check: LANDED Moon | auf Luna gelandet

=== MISSION ===
id: un_luna_rover
sparte: Robotische Erkunder
body: Moon
prereq: un_luna_landing
reward: 140
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Setze einen unbemannten Rover punktgenau am vorbereiteten Mondgebiet ab. Erst diese kontrollierte Ziellandung beweist, dass auch die erste bemannte Crew sicher am geplanten Ort aufsetzen kann.
beschreibung_en: Set an uncrewed rover down precisely at the prepared lunar site. Only this controlled pinpoint landing proves that the first crew can also touch down safely at the planned spot.
check: CREW_NONE | unbemannt
check: LANDED Moon | auf Luna gelandet
check: MARKER_LANDING Moon 5 | Landung im Umkreis von 5 km

=== MISSION ===
id: cr_luna_orbit
sparte: Pioniere
body: Moon
prereq: cr_luna_flyby_crewed, un_luna_landing
reward: 152
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe Kerbals in den Orbit um Luna. Die Besatzung ist nicht mehr auf Durchreise, sondern arbeitet an einer fremden Welt — der letzte Schritt vor dem ersten Fussabdruck.
beschreibung_en: Put Kerbals into orbit around the Moon. The crew is no longer just passing through but working at another world — the last step before the first footprint.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Moon 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_luna_landing
sparte: Pioniere
body: Moon
prereq: cr_luna_orbit, un_luna_rover
reward: 208
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze den ersten Kerbal auf Luna ab und lass ihn aussteigen. Ein Fussabdruck, der bleibt — und der Anfang von allem, was dein Programm später auf der Oberfläche aufbaut, von der ersten Basis bis zum Daueraufenthalt. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Set the first Kerbal down on the Moon and let them step out. A footprint that lasts — and the beginning of everything your program later builds on the surface, from the first base to permanent habitation. The crew must then return safely to Earth.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: EVA Moon LANDED | EVA auf Luna
check: RETURN_FROM_BODY Moon Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_luna_stay_2d
sparte: Pioniere
body: Moon
prereq: cr_luna_landing
reward: 168
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass eine Besatzung zwei Tage auf dem Mond arbeiten. Aus der ersten Landung werden die ersten echten Arbeitsschichten auf einer anderen Welt. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Have a crew work on the Moon for two days. The first landing becomes the first real work shifts on another world. The crew must then return safely to Earth.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: DURATION 2 | 2 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Moon Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_luna_precision_landing
sparte: Pioniere
body: Moon
prereq: cr_luna_landing
reward: 184
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bring eine bemannte Landefähre punktgenau am vorbereiteten Mondgebiet nieder. So beweist dein Programm, dass Module und Besatzungen sich später am selben Ort treffen können — die Voraussetzung für eine echte Basis. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Bring a crewed lander down precisely at the prepared lunar site. This proves modules and crews can later meet at the same spot — the prerequisite for a real base. The crew must then return safely to Earth.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: MARKER_LANDING Moon 5 | Landung im Umkreis von 5 km
check: RETURN_FROM_BODY Moon Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_luna_stay_7d
sparte: Pioniere
body: Moon
prereq: cr_luna_stay_2d
reward: 224
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte eine Besatzung eine volle Woche auf Luna. Diese sieben Tage machen die spätere Mondbasis glaubhaft — und den langen Weg nach Mars eine Spur weniger furchteinflössend. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Keep a crew on the Moon for a full week. These seven days make the later lunar base believable — and the long road to Mars a little less daunting. The crew must then return safely to Earth.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Moon Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: net_earth_comm_network3
sparte: Versorgungsnetz
body: Earth
prereq: cr_earth_station_longstay3
reward: 120
repeatable: no
recordStation: -
stationRef: -
beschreibung: Ersetze das erste, alternde Erdnetz durch eine neue Kommunikationskonstellation. Drei moderne Relais im Erdorbit geben Stationen, Depots und Langstreckenflügen wieder eine verlässliche Stimme.
beschreibung_en: Replace the first, aging Earth network with a new communications constellation. Three modern relays in Earth orbit give stations, depots and long-range flights a reliable voice again.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Earth 3 2000 | 3 Satelliten gleichzeitig im Erdorbit, Periapsis über 2000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: net_earth_polar_comm_network
sparte: Versorgungsnetz
body: Earth
prereq: net_earth_comm_network3
reward: 150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Ergänze das neue Erdnetz um eine polare Relais-Schale. Drei weitere Satelliten decken hohe Breiten und schräge Missionsprofile ab, damit die alte Pionierkonstellation endgültig abgelöst werden kann.
beschreibung_en: Add a polar relay shell to the new Earth network. Three more satellites cover high latitudes and steep mission profiles so the old pioneer constellation can finally be retired.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Earth 6 2000 | 6 Satelliten gleichzeitig im Erdorbit, Periapsis über 2000 km
check: VESSEL_COUNT_INCLINATION Earth 3 75 2000 | 3 Satelliten im Erdorbit mit Inklination über 75 Grad und Periapsis über 2000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: net_luna_comm_network3
sparte: Versorgungsnetz
body: Moon
prereq: net_earth_polar_comm_network, un_luna_orbit
reward: 150
repeatable: no
recordStation: -
stationRef: -
beschreibung: Ziehe die Kommunikations-Lebensader bis Luna. Drei Relais im Mondorbit verbinden Orbiter, Landefähren und die kommende Infrastruktur mit dem erneuerten Erdnetz.
beschreibung_en: Extend the communications lifeline out to the Moon. Three relays in lunar orbit link orbiters, landers and the coming infrastructure to the renewed Earth network.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Moon 3 2000 | 3 Satelliten gleichzeitig im Mondorbit, Periapsis über 2000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: net_luna_polar_comm_network
sparte: Versorgungsnetz
body: Moon
prereq: net_luna_comm_network3
reward: 185
repeatable: no
recordStation: -
stationRef: -
beschreibung: Baue das Mondnetz um polare Bahnen aus. Drei zusätzliche Relais machen Lunas Randzonen, hohe Breiten und spätere Basisstandorte leichter erreichbar.
beschreibung_en: Expand the lunar network with polar orbits. Three additional relays make the Moon's edges, high latitudes and future base sites easier to reach.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Moon 6 2000 | 6 Satelliten gleichzeitig im Mondorbit, Periapsis über 2000 km
check: VESSEL_COUNT_INCLINATION Moon 3 75 2000 | 3 Satelliten im Mondorbit mit Inklination über 75 Grad und Periapsis über 2000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: un_venus_flyby
sparte: Robotische Erkunder
body: Venus
prereq: cr_luna_flyby_crewed
reward: 104
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Venus vorbei und wirf den ersten nahen Blick auf die verhüllte Schwesterwelt der Erde. Schön von weitem, lebensfeindlich von nahem — und das erste grosse Ziel im inneren System.
beschreibung_en: Send a probe past Venus and take the first close look at Earth's veiled sister world. Beautiful from afar, deadly up close — and the first great target in the inner system.
check: CREW_NONE | unbemannt
check: FLYBY Venus 5000 | Vorbeiflug unter 5000 km

=== MISSION ===
id: un_venus_orbit
sparte: Robotische Erkunder
body: Venus
prereq: un_venus_flyby
reward: 144
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Venusorbit und beobachte eine Welt, die unter ewigen Wolken glüht. Jede Bahn liefert Daten über einen Planeten, der einladend wirkt und doch alles verschlingt.
beschreibung_en: Place a probe into Venus orbit and watch a world glowing beneath eternal clouds. Every pass returns data on a planet that looks inviting yet devours everything.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Venus | stabiler Venus-Orbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_venus_atmo_probe
sparte: Robotische Erkunder
body: Venus
prereq: un_venus_orbit
reward: 168
repeatable: no
recordStation: -
stationRef: -
beschreibung: Tauche eine Sonde in die obere Venusatmosphäre und miss Druck, Hitze und fremde Chemie, solange das Signal hält. Jede Sekunde dort unten ist hart erkämpfte Wissenschaft.
beschreibung_en: Dip a probe into the upper Venusian atmosphere and measure pressure, heat and alien chemistry for as long as the signal holds. Every second down there is hard-won science.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Venus 60 90 | obere Atmosphäre erreicht

=== MISSION ===
id: un_venus_landing
sparte: Robotische Erkunder
body: Venus
prereq: un_venus_atmo_probe
reward: 224
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine unbemannte Sonde auf der Venusoberfläche ab. Die Hölle aus Hitze und Druck lässt Maschinen nur kurze, kostbare Datenfenster — nutze sie.
beschreibung_en: Set an uncrewed probe down on the surface of Venus. The inferno of heat and pressure gives machines only short, precious windows of data — use them.
check: CREW_NONE | unbemannt
check: LANDED Venus | auf Venus gelandet

=== MISSION ===
id: cr_venus_flyby
sparte: Pioniere
body: Venus
prereq: un_venus_orbit, cr_earth_station_longstay3
reward: 280
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke zwei Kerbals an Venus vorbei. Die Besatzung sieht die Nachbarwelt mit eigenen Augen, während dein Programm sie aus sicherer Distanz erforscht — bemannt bleibt Venus ein Ort des Vorbeiflugs. Nach dem Vorbeiflug muss die Crew sicher zur Erde zurückkehren.
beschreibung_en: Send two Kerbals past Venus. The crew sees the neighbouring world with their own eyes while your program studies it from a safe distance — crewed, Venus stays a place to fly past. After the flyby the crew must return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: FLYBY Venus 8000 | Vorbeiflug unter 8000 km
check: RETURN_FROM_BODY Venus Earth flyby | Crew nach dem Vorbeiflug sicher zur Erde zurückbringen

=== MISSION ===
id: cr_venus_orbit
sparte: Pioniere
body: Venus
prereq: cr_venus_flyby, un_venus_landing
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe zwei Kerbals in den Venusorbit und halte sie zehn Tage dort. Die Mission zeigt, dass dein Programm selbst die feindlichsten Welten mit Disziplin und Geduld erreicht.
beschreibung_en: Put two Kerbals into Venus orbit and hold them there for ten days. The mission shows your program can reach even the most hostile worlds with discipline and patience.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: ORBIT_ABOVE Venus | stabiler Venus-Orbit
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: un_mercury_flyby
sparte: Robotische Erkunder
body: Mercury
prereq: cr_luna_flyby_crewed
reward: 120
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Merkur vorbei, tief hinab in die Sonnenglut. Dein Programm wagt sich an den heissesten, schnellsten Ort des inneren Systems.
beschreibung_en: Send a probe past Mercury, down into the glare of the Sun. Your program ventures to the hottest, fastest place in the inner system.
check: CREW_NONE | unbemannt
check: FLYBY Mercury 3000 | Vorbeiflug unter 3000 km

=== MISSION ===
id: un_mercury_orbit
sparte: Robotische Erkunder
body: Mercury
prereq: un_mercury_flyby
reward: 176
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Merkurorbit und kartiere die kleine, verbrannte Welt am Rand der Sonne — ein Balanceakt zwischen Hitze und Schwerkraft.
beschreibung_en: Place a probe into Mercury orbit and map the small, scorched world at the Sun's edge — a balancing act between heat and gravity.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mercury 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_mercury_polar_mapping
sparte: Robotische Erkunder
body: Mercury
prereq: un_mercury_orbit
reward: 230
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lege eine Sonde in eine polare Merkurumlaufbahn und kartiere die verbrannte Welt über beide Pole hinweg. Diese Bahn macht aus dem ersten Orbit eine echte Vermessung.
beschreibung_en: Put a probe into a polar Mercury orbit and map the scorched world across both poles. This orbit turns a first pass into a true survey.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mercury 50 | Periapsis über 50 km
check: INCLINATION_MIN Mercury 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_mercury_landing
sparte: Robotische Erkunder
body: Mercury
prereq: un_mercury_orbit
reward: 248
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde auf Merkur ab. Dein Programm behandelt die extreme kleine Welt als das, was sie ist: ein Ziel für zähe Maschinen, nicht für Menschen.
beschreibung_en: Set a probe down on Mercury. Your program treats the extreme little world for what it is: a target for tough machines, not for people.
check: CREW_NONE | unbemannt
check: LANDED Mercury | auf Merkur gelandet

=== MISSION ===
id: un_sun_inner_probe
sparte: Robotische Erkunder
body: Sun
prereq: un_mercury_flyby
reward: 208
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde auf eine enge Bahn um die Sonne und sammle Daten aus nächster Nähe am Zentrum, das alle Reisen bestimmt. Näher kommt deinem Stern so bald niemand.
beschreibung_en: Send a probe onto a tight orbit around the Sun and gather data up close at the centre that governs every journey. Nobody will get nearer to your star any time soon.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Sun 1000000 | Periapsis über 1000000 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_mars_flyby
sparte: Robotische Erkunder
body: Mars
prereq: cr_luna_flyby_crewed
reward: 128
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Mars vorbei. Der rote Planet verwandelt sich vom fernen Traum in das nächste grosse Arbeitsziel deines Programms.
beschreibung_en: Send a probe past Mars. The red planet turns from a distant dream into your program's next great working target.
check: CREW_NONE | unbemannt
check: FLYBY Mars 5000 | Vorbeiflug unter 5000 km

=== MISSION ===
id: un_mars_orbit
sparte: Robotische Erkunder
body: Mars
prereq: un_mars_flyby
reward: 184
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Marsorbit und kartiere die Landegebiete von oben. Jede Bahn ebnet den Weg für die Kerbals, die eines Tages dort unten stehen werden.
beschreibung_en: Place a probe into Mars orbit and map the landing sites from above. Every pass paves the way for the Kerbals who will one day stand down there.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mars | stabiler Mars-Orbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_mars_polar_mapping
sparte: Robotische Erkunder
body: Mars
prereq: un_mars_orbit
reward: 245
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in einen polaren Marsorbit und vermesse den roten Planeten für spätere Landegebiete. Die polare Bahn verbindet Äquator, Hochland, Eiskappen und Randzonen zu einer vollständigen Karte.
beschreibung_en: Put a probe into a polar Mars orbit and survey the red planet for future landing sites. The polar path ties equator, highlands, ice caps and edges into one complete map.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mars 250 | Periapsis über 250 km
check: INCLINATION_MIN Mars 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_mars_polar_landing
sparte: Robotische Erkunder
body: Mars
prereq: un_mars_polar_mapping
reward: 330
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde punktgenau in der Polregion des Mars ab. Die Eiskappen und Randzonen der Pole sind wissenschaftlich besonders ergiebig — und die Landung dort verlangt mehr Präzision als am Äquator.
beschreibung_en: Set a probe down precisely in the polar region of Mars. The ice caps and edges of the poles are especially rich scientifically — and landing there demands more precision than at the equator.
check: CREW_NONE | unbemannt
check: LANDED Mars | auf Mars gelandet
check: MARKER_LANDING Mars 12 70 90 | Pollandung im Umkreis von 12 km

=== MISSION ===
id: net_mars_comm_network
sparte: Versorgungsnetz
body: Mars
prereq: net_luna_polar_comm_network, un_mars_orbit
reward: 210
repeatable: no
recordStation: -
stationRef: -
beschreibung: Verlängere die Kommunikationskette bis Mars. Drei Relais im Marsorbit schaffen die Infrastruktur, auf die Landefähren, Stationen und spätere Oberflächencrews angewiesen sein werden.
beschreibung_en: Extend the communications chain out to Mars. Three relays in Mars orbit build the infrastructure that landers, stations and later surface crews will depend on.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Mars 3 2000 | 3 Satelliten gleichzeitig im Marsorbit, Periapsis über 2000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: net_solar_comm_network
sparte: Versorgungsnetz
body: Sun
prereq: net_mars_comm_network, un_sun_inner_probe
reward: 260
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze einen interplanetaren Kommunikationsring in Sonnenorbit. Drei Relais auf eigenen Bahnen machen aus einzelnen Planetennetzen ein zusammenhängendes Sonnensystem-Netz.
beschreibung_en: Place an interplanetary communications ring in solar orbit. Three relays on their own paths turn separate planetary networks into one connected solar-system grid.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Sun 3 1000000 | 3 Relais im Sonnenorbit, Periapsis über 1000000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: un_mars_landing
sparte: Robotische Erkunder
body: Mars
prereq: un_mars_orbit
reward: 248
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf dem Mars und berühre den roten Staub — lange bevor die erste Besatzung kommt. Ein robotischer Vorbote für das zweite grosse Kapitel.
beschreibung_en: Land a probe on Mars and touch the red dust — long before the first crew arrives. A robotic herald for the second great chapter.
check: CREW_NONE | unbemannt
check: LANDED Mars | auf Mars gelandet

=== MISSION ===
id: un_mars_rover
sparte: Robotische Erkunder
body: Mars
prereq: un_mars_landing
reward: 320
repeatable: no
recordStation: -
stationRef: -
icon: TrackingStation_ButtonMapRover
beschreibung: Setze einen unbemannten Rover punktgenau am vorbereiteten Marsgebiet ab. Die kontrollierte Ziellandung sichert der späteren bemannten Crew einen klaren, erprobten Aufsetzpunkt — ohne sie bleibt die bemannte Marslandung gesperrt.
beschreibung_en: Set an uncrewed rover down precisely at the prepared Mars site. The controlled pinpoint landing secures a clear, proven touchdown point for the later crew — without it the crewed Mars landing stays locked.
check: CREW_NONE | unbemannt
check: LANDED Mars | auf Mars gelandet
check: MARKER_LANDING Mars 10 | Landung im Umkreis von 10 km

=== MISSION ===
id: un_mars_precision_landing
sparte: Robotische Erkunder
body: Mars
prereq: un_mars_landing
reward: 304
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde punktgenau am vorbereiteten Marsgebiet ab. Diese Treffsicherheit gibt der ersten bemannten Landung ein klares, sicheres Ziel.
beschreibung_en: Set a probe down precisely at the prepared Mars site. This accuracy gives the first crewed landing a clear, safe target.
check: CREW_NONE | unbemannt
check: LANDED Mars | auf Mars gelandet
check: MARKER_LANDING Mars 10 | Landung im Umkreis von 10 km

=== MISSION ===
id: cr_mars_flyby
sparte: Pioniere
body: Mars
prereq: un_mars_orbit, cr_earth_station_longstay3, cr_luna_stay_7d
reward: 420
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke zwei Kerbals an Mars vorbei. Die Besatzung sieht den roten Planeten aus der Nähe — eine Generalprobe für den Tag, an dem dein Programm dort landet. Nach dem Vorbeiflug muss die Crew sicher zur Erde zurückkehren.
beschreibung_en: Send two Kerbals past Mars. The crew sees the red planet up close — a dress rehearsal for the day your program lands there. After the flyby the crew must return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: FLYBY Mars 8000 | Vorbeiflug unter 8000 km
check: RETURN_FROM_BODY Mars Earth flyby | Crew nach dem Vorbeiflug sicher zur Erde zurückbringen

=== MISSION ===
id: cr_mars_orbit
sparte: Pioniere
body: Mars
prereq: cr_mars_flyby, un_mars_landing, cr_earth_station_longstay3
reward: 540
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe zwei Kerbals in den Marsorbit und halte sie zehn Tage über dem roten Planeten. Was bisher nur Sonden taten, tut nun eine Crew — die Landung ist zum Greifen nah.
beschreibung_en: Put two Kerbals into Mars orbit and hold them ten days above the red planet. What only probes have done so far, a crew now does — the landing is within reach.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: ORBIT_ABOVE Mars | stabiler Mars-Orbit
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: un_phobos_flyby
sparte: Robotische Erkunder
body: Phobos
prereq: un_mars_orbit
reward: 104
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde dicht an Phobos vorbei. Der zerklüftete innere Marsmond wird zum ersten kleinen Ziel im Marsraum.
beschreibung_en: Send a probe close past Phobos. The rugged inner Martian moon becomes the first small target in Mars space.
check: CREW_NONE | unbemannt
check: FLYBY Phobos 50 | Vorbeiflug unter 50 km

=== MISSION ===
id: un_phobos_orbit
sparte: Robotische Erkunder
body: Phobos
prereq: un_phobos_flyby
reward: 136
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in eine enge Bahn um Phobos und prüfe den Mond als möglichen Stützpunkt für künftige Marsoperationen.
beschreibung_en: Put a probe into a tight orbit around Phobos and assess the moon as a possible staging point for future Mars operations.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Phobos 8 | Periapsis über 8 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_deimos_flyby
sparte: Robotische Erkunder
body: Deimos
prereq: un_mars_orbit
reward: 104
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Deimos vorbei, den äusseren der beiden Marsmonde. Er markiert den stillen Rand des frühen Marsraums.
beschreibung_en: Send a probe past Deimos, the outer of the two Martian moons. It marks the quiet edge of early Mars space.
check: CREW_NONE | unbemannt
check: FLYBY Deimos 50 | Vorbeiflug unter 50 km

=== MISSION ===
id: un_deimos_orbit
sparte: Robotische Erkunder
body: Deimos
prereq: un_deimos_flyby
reward: 136
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Deimos und sammle Daten über einen ruhigen Aussenposten hoch über Mars.
beschreibung_en: Put a probe into orbit around Deimos and gather data on a calm outpost high above Mars.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Deimos 8 | Periapsis über 8 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_jupiter_flyby
sparte: Robotische Erkunder
body: Jupiter
prereq: un_mars_orbit
reward: 270
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Jupiter vorbei, lange bevor Kerbals so weit reisen. Zum ersten Mal sieht dein Programm das äussere System aus der Nähe — und ein ganzes Reich aus Monden tut sich auf.
beschreibung_en: Send a probe past Jupiter, long before Kerbals travel that far. For the first time your program sees the outer system up close — and a whole realm of moons opens up.
check: CREW_NONE | unbemannt
check: FLYBY Jupiter 50000 | Vorbeiflug unter 50000 km

=== MISSION ===
id: un_jupiter_atmo_probe
sparte: Robotische Erkunder
body: Jupiter
prereq: un_jupiter_flyby
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass eine Sonde in Jupiters oberste Wolkenschichten eintauchen. Die Messungen erzählen von einer Welt ohne festen Boden, nur Sturm und Tiefe.
beschreibung_en: Let a probe dip into Jupiter's uppermost cloud layers. The readings tell of a world with no solid ground, only storm and depth.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Jupiter 70 95 | obere Atmosphäre erreicht

=== MISSION ===
id: un_jupiter_orbit
sparte: Robotische Erkunder
body: Jupiter
prereq: un_jupiter_flyby
reward: 345
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Jupiterorbit und öffne damit ein ganzes System aus Monden, Strahlung und gewaltigen Distanzen für dein Programm.
beschreibung_en: Place a probe into Jupiter orbit and open a whole system of moons, radiation and vast distances to your program.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Jupiter | stabiler Jupiter-Orbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_jupiter_polar_mapping
sparte: Robotische Erkunder
body: Jupiter
prereq: un_jupiter_orbit
reward: 430
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde auf eine steile Bahn um Jupiter und kartiere das System aus hoher Inklination. Diese Perspektive verbindet Magnetosphäre, Polregionen und Monde zu einem besseren Gesamtbild.
beschreibung_en: Put a probe onto a steep orbit around Jupiter and map the system from high inclination. This view ties magnetosphere, polar regions and moons into a better overall picture.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Jupiter 10000 | Periapsis über 10000 km
check: INCLINATION_MIN Jupiter 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: net_jupiter_comm_network
sparte: Versorgungsnetz
body: Jupiter
prereq: net_solar_comm_network, un_jupiter_orbit
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Verankere das interplanetare Netz im Jupitersystem. Drei Relais um Jupiter halten die Verbindung zu den grossen Monden offen, bevor bemannte Missionen in diese gewaltige Distanz aufbrechen.
beschreibung_en: Anchor the interplanetary network in the Jupiter system. Three relays around Jupiter keep the link to the great moons open before crewed missions set out across this enormous distance.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Jupiter 3 10000 | 3 Satelliten gleichzeitig im Jupiterorbit, Periapsis über 10000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: un_eros_flyby
sparte: Robotische Erkunder
body: Eros
prereq: cr_luna_flyby_crewed
reward: 72
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Eros vorbei und eröffne den freiwilligen Asteroidenzweig deines Programms — ein erster Blick auf die kleinen Wanderer zwischen den Planeten.
beschreibung_en: Send a probe past Eros and open your program's optional asteroid branch — a first look at the little wanderers between the planets.
check: CREW_NONE | unbemannt
check: FLYBY Eros 100 | Vorbeiflug unter 100 km

=== MISSION ===
id: un_eros_orbit
sparte: Robotische Erkunder
body: Eros
prereq: un_eros_flyby
reward: 104
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in eine enge Bahn um Eros. Dein Programm lernt, dass winzige Körper mit kaum Schwerkraft die grösste Präzision verlangen.
beschreibung_en: Put a probe into a tight orbit around Eros. Your program learns that tiny bodies with barely any gravity demand the greatest precision.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Eros 5 | Periapsis über 5 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_vesta_flyby
sparte: Robotische Erkunder
body: Vesta
prereq: un_mars_flyby
reward: 120
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Vesta vorbei. Der Asteroidengürtel zeigt deinem Programm seine erste richtig grosse Welt.
beschreibung_en: Send a probe past Vesta. The asteroid belt shows your program its first truly large world.
check: CREW_NONE | unbemannt
check: FLYBY Vesta 300 | Vorbeiflug unter 300 km

=== MISSION ===
id: un_ceres_flyby
sparte: Robotische Erkunder
body: Ceres
prereq: un_mars_orbit
reward: 152
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Ceres vorbei, den grössten Körper des Gürtels. Schon dieser Vorbeiflug deutet an, wie lohnend die kleine Zwergplanetenwelt später wird.
beschreibung_en: Send a probe past Ceres, the largest body in the belt. Even this flyby hints at how rewarding the little dwarf-planet world will become later.
check: CREW_NONE | unbemannt
check: FLYBY Ceres 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: cr_mars_landing
sparte: Pioniere
body: Mars
prereq: cr_mars_orbit, un_mars_rover, cr_earth_station_longstay3, cr_luna_stay_7d
reward: 540
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf dem Mars und lass sie den roten Staub betreten. Nach Luna beginnt hier das zweite grosse Kapitel deines Programms — Menschen auf einer anderen Welt. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Land two Kerbals on Mars and let them set foot in the red dust. After the Moon, the second great chapter of your program begins here — people on another world. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: EVA Mars LANDED | EVA auf Mars
check: RETURN_FROM_BODY Mars Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_mars_precision_landing
sparte: Pioniere
body: Mars
prereq: cr_mars_landing
reward: 435
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine bemannte Marslandung punktgenau am vorbereiteten Gebiet ab. Die gewonnene Treffsicherheit ist die Voraussetzung für eine dauerhafte Basis. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Bring a crewed Mars landing down precisely at the prepared site. The accuracy you gain is the prerequisite for a permanent base. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: MARKER_LANDING Mars 10 | Landung im Umkreis von 10 km
check: RETURN_FROM_BODY Mars Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_mars_stay_10d
sparte: Pioniere
body: Mars
prereq: cr_mars_landing
reward: 480
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte zwei Kerbals zehn Tage auf dem Mars. Aus der ersten Landung wird eine echte Forschungsmission — der rote Planet wird zum Arbeitsplatz. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Keep two Kerbals on Mars for ten days. The first landing becomes a real research mission — the red planet turns into a workplace. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: DURATION 10 | 10 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Mars Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_mars_stay_30d
sparte: Pioniere
body: Mars
prereq: cr_mars_stay_10d, cr_mars_precision_landing
reward: 630
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte zwei Kerbals dreissig Tage auf dem Mars. Damit wird der Planet vom Landeziel zum künftigen Aussenposten — und die Marsbasis denkbar. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Keep two Kerbals on Mars for thirty days. This turns the planet from a landing target into a future outpost — and makes a Mars base conceivable. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: DURATION 30 | 30 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Mars Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_phobos_landing
sparte: Pioniere
body: Phobos
prereq: cr_mars_landing, un_phobos_orbit
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf Phobos. Der Marsraum wird vom einzelnen Planeten zu einem System aus Zielen — Mond für Mond. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Land two Kerbals on Phobos. Mars space turns from a single planet into a system of targets — moon by moon. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Phobos | auf Phobos gelandet
check: EVA Phobos LANDED | EVA auf Phobos
check: RETURN_FROM_BODY Phobos Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_deimos_landing
sparte: Pioniere
body: Deimos
prereq: cr_mars_landing, un_deimos_orbit
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf Deimos. Von hier draussen wirkt Mars wie der Mittelpunkt eines ganz neuen Arbeitsgebiets. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Land two Kerbals on Deimos. From out here, Mars looks like the centre of a whole new field of work. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Deimos | auf Deimos gelandet
check: EVA Deimos LANDED | EVA auf Deimos
check: RETURN_FROM_BODY Deimos Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: net_mars_orbit_supply
sparte: Versorgungsnetz
body: Mars
prereq: cr_mars_orbit, un_mars_landing
reward: 108
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Bring ein unbemanntes Versorgungsfahrzeug mit vollen Tanks in den Marsorbit. Dein Programm lernt, den roten Planeten regelmässig und verlässlich zu bedienen.
beschreibung_en: Bring an uncrewed supply vehicle with full tanks into Mars orbit. Your program learns to service the red planet regularly and reliably.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mars | stabiler Mars-Orbit
check: FUEL_MIN 800 | Treibstoff über 800
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: net_phobos_cache
sparte: Versorgungsnetz
body: Phobos
prereq: cr_mars_landing, un_phobos_orbit
reward: 126
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Richte ein unbemanntes Treibstoffdepot im Orbit um Phobos ein. Der kleine Mond wird zum stillen Helfer für alle künftigen Marsoperationen.
beschreibung_en: Set up an uncrewed fuel depot in orbit around Phobos. The small moon becomes a quiet helper for all future Mars operations.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Phobos 8 | Stabiler Orbit um Phobos, Periapsis über 8 km
check: FUEL_MIN 500 | Treibstoff über 500
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_vesta_orbit
sparte: Robotische Erkunder
body: Vesta
prereq: un_vesta_flyby
reward: 126
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Vesta und mach den Bonuszweig im Gürtel wissenschaftlich reicher.
beschreibung_en: Put a probe into orbit around Vesta and make the belt's bonus branch scientifically richer.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Vesta 10 | Periapsis über 10 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_ceres_orbit
sparte: Robotische Erkunder
body: Ceres
prereq: un_ceres_flyby
reward: 156
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Ceres. Die grösste Welt des Gürtels bekommt damit echtes Gewicht in deinem Erkundungsprogramm.
beschreibung_en: Put a probe into orbit around Ceres. The largest world of the belt gains real weight in your exploration program.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Ceres 15 | Periapsis über 15 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_psyche_flyby
sparte: Robotische Erkunder
body: Psyche
prereq: un_ceres_orbit
reward: 126
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Psyche vorbei. Der ungewöhnlich metallische Körper weckt Vorstellungen von Bergbau und Industrie ferner Zukunft.
beschreibung_en: Send a probe past Psyche. The unusually metallic body sparks visions of mining and industry in a distant future.
check: CREW_NONE | unbemannt
check: FLYBY Psyche 200 | Vorbeiflug unter 200 km

=== MISSION ===
id: un_ceres_landing
sparte: Robotische Erkunder
body: Ceres
prereq: un_ceres_orbit
reward: 204
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf Ceres. Der Asteroidengürtel bekommt seinen wichtigsten robotischen Bodenkontakt — und Ceres rückt als bemanntes Ziel in den Blick.
beschreibung_en: Land a probe on Ceres. The asteroid belt gets its most important robotic surface contact — and Ceres comes into view as a crewed target.
check: CREW_NONE | unbemannt
check: LANDED Ceres | auf Ceres gelandet

=== MISSION ===
id: un_vesta_landing
sparte: Robotische Erkunder
body: Vesta
prereq: un_vesta_orbit
reward: 168
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde auf Vesta ab und vergleiche zwei sehr verschiedene Gürtelwelten aus nächster Nähe.
beschreibung_en: Set a probe down on Vesta and compare two very different belt worlds up close.
check: CREW_NONE | unbemannt
check: LANDED Vesta | auf Vesta gelandet

=== MISSION ===
id: un_pallas_flyby
sparte: Robotische Erkunder
body: Pallas
prereq: un_ceres_orbit
reward: 108
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Pallas vorbei. Der Gürtel bleibt ein freiwilliges Feld für alle, die noch mehr entdecken wollen.
beschreibung_en: Send a probe past Pallas. The belt remains an optional field for anyone who wants to discover even more.
check: CREW_NONE | unbemannt
check: FLYBY Pallas 300 | Vorbeiflug unter 300 km

=== MISSION ===
id: un_pallas_orbit
sparte: Robotische Erkunder
body: Pallas
prereq: un_pallas_flyby
reward: 138
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Pallas und sammle Vergleichsdaten zu den anderen Riesen des Gürtels.
beschreibung_en: Put a probe into orbit around Pallas and gather comparison data on the other giants of the belt.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Pallas 10 | Periapsis über 10 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_psyche_orbit
sparte: Robotische Erkunder
body: Psyche
prereq: un_psyche_flyby
reward: 162
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Psyche. Der Metallkörper wird zum Sinnbild für Ressourcenforschung abseits des Hauptpfads.
beschreibung_en: Put a probe into orbit around Psyche. The metal body becomes a symbol of resource research away from the main path.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Psyche 8 | Periapsis über 8 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_ryugu_flyby
sparte: Robotische Erkunder
body: Ryugu
prereq: un_eros_orbit
reward: 78
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde dicht an Ryugu vorbei. Der winzige, dunkle Asteroid fordert Präzision und belohnt sie mit feinen Daten.
beschreibung_en: Send a probe close past Ryugu. The tiny, dark asteroid demands precision and rewards it with fine data.
check: CREW_NONE | unbemannt
check: FLYBY Ryugu 50 | Vorbeiflug unter 50 km

=== MISSION ===
id: un_ryugu_landing
sparte: Robotische Erkunder
body: Ryugu
prereq: un_ryugu_flyby
reward: 108
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf dem winzigen Ryugu. Ein leiser Triumph deiner Navigation — auf einem Körper, der kaum Schwerkraft hat.
beschreibung_en: Land a probe on the tiny Ryugu. A quiet triumph of your navigation — on a body with almost no gravity.
check: CREW_NONE | unbemannt
check: LANDED Ryugu | auf Ryugu gelandet

=== MISSION ===
id: un_ida_flyby
sparte: Robotische Erkunder
body: Ida
prereq: un_vesta_orbit
reward: 84
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Ida vorbei und füge dem Gürtel eine weitere unregelmässige Welt hinzu.
beschreibung_en: Send a probe past Ida and add another irregular world to the belt.
check: CREW_NONE | unbemannt
check: FLYBY Ida 100 | Vorbeiflug unter 100 km

=== MISSION ===
id: un_dactyl_flyby
sparte: Robotische Erkunder
body: Dactyl
prereq: un_ida_flyby
reward: 66
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde am winzigen Dactyl vorbei, dem Begleiter von Ida. Selbst der kleinste Brocken bekommt seinen Platz in deiner Chronik.
beschreibung_en: Send a probe past the tiny Dactyl, Ida's companion. Even the smallest rock earns its place in your chronicle.
check: CREW_NONE | unbemannt
check: FLYBY Dactyl 20 | Vorbeiflug unter 20 km

=== MISSION ===
id: cr_ceres_flyby
sparte: Pioniere
body: Ceres
prereq: un_ceres_landing, cr_mars_stay_30d, cr_earth_station_longstay12
reward: 270
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke zwei Kerbals an Ceres vorbei. Dieser Prestigeflug zeigt, dass dein Programm selbst kleine Gürtelwelten bemannt erreichen kann.
beschreibung_en: Send two Kerbals past Ceres. This prestige flight shows your program can reach even small belt worlds with a crew.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: FLYBY Ceres 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: cr_ceres_orbit
sparte: Pioniere
body: Ceres
prereq: cr_ceres_flyby
reward: 345
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe zwei Kerbals in den Orbit um Ceres und halte sie zehn Tage. Die grösste Asteroidenwelt wird zur bemannten Nebenbühne deines Programms.
beschreibung_en: Put two Kerbals into orbit around Ceres and hold them ten days. The largest asteroid world becomes a crewed side stage for your program.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: ORBIT_ABOVE Ceres 15 | Periapsis über 15 km
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_ceres_landing
sparte: Pioniere
body: Ceres
prereq: cr_ceres_flyby
reward: 450
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf Ceres. Ein freiwilliger Höhepunkt fernab des Hauptpfads — und ein Zeichen für die enorme Reichweite deines Programms. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Land two Kerbals on Ceres. An optional highlight far from the main path — and a sign of your program's enormous reach. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Ceres | auf Ceres gelandet
check: EVA Ceres LANDED | EVA auf Ceres
check: RETURN_FROM_BODY Ceres Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_ceres_stay_7d
sparte: Pioniere
body: Ceres
prereq: cr_ceres_landing
reward: 405
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte zwei Kerbals sieben Tage auf Ceres. Dein Programm lernt, in winzigen Schwerefeldern über längere Zeit zu leben und zu arbeiten. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Keep two Kerbals on Ceres for seven days. Your program learns to live and work in tiny gravity fields over a longer time. The crew must then return safely to Earth.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Ceres | auf Ceres gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Ceres Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: net_ceres_ore_test
sparte: Versorgungsnetz
body: Ceres
prereq: un_ceres_landing
reward: 126
repeatable: no
recordStation: -
stationRef: -
beschreibung: Förder unbemannt erstes Erz auf Ceres. Der Bonuszweig bekommt damit eine glaubhafte Industrie-Erzählung — Rohstoffe aus dem Gürtel.
beschreibung_en: Mine the first ore on Ceres with an uncrewed rig. The bonus branch gains a believable industrial story — raw materials from the belt.
check: CREW_NONE | unbemannt
check: LANDED Ceres | auf Ceres gelandet
check: ORE_SURFACE Ceres | Ore auf Oberfläche gefördert
check: RESOURCE_MIN Ore 50 | Ore über 50

=== MISSION ===
id: net_ceres_supply_cache
sparte: Versorgungsnetz
body: Ceres
prereq: cr_ceres_stay_7d, net_ceres_ore_test
reward: 156
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Lege auf Ceres einen Vorratspunkt mit Treibstoff an. Er stützt freiwillige Fernoperationen, ohne den Hauptpfad deines Programms aufzuhalten.
beschreibung_en: Lay down a fuel cache on Ceres. It supports optional deep-range operations without holding up your program's main path.
check: CREW_NONE | unbemannt
check: LANDED Ceres | auf Ceres gelandet
check: FUEL_MIN 600 | Treibstoff über 600

=== MISSION ===
id: net_psyche_ore_test
sparte: Versorgungsnetz
body: Psyche
prereq: un_psyche_orbit
reward: 138
repeatable: no
recordStation: -
stationRef: -
beschreibung: Förder unbemannt Erz auf Psyche und prüfe, ob die kleinen Metallwelten praktischen Zukunftswert tragen.
beschreibung_en: Mine ore on Psyche with an uncrewed rig and test whether the little metal worlds carry practical future value.
check: CREW_NONE | unbemannt
check: LANDED Psyche | auf Psyche gelandet
check: ORE_SURFACE Psyche | Ore auf Oberfläche gefördert
check: RESOURCE_MIN Ore 50 | Ore über 50

=== MISSION ===
id: net_vesta_supply_cache
sparte: Versorgungsnetz
body: Vesta
prereq: un_vesta_landing
reward: 114
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Lege auf Vesta einen kleinen Treibstoff-Vorratspunkt an. Der Bonuszweig wird nützlicher, bleibt aber frei von jeder Pflicht.
beschreibung_en: Set up a small fuel cache on Vesta. The bonus branch grows more useful while staying free of any obligation.
check: CREW_NONE | unbemannt
check: LANDED Vesta | auf Vesta gelandet
check: FUEL_MIN 400 | Treibstoff über 400

=== MISSION ===
id: un_io_flyby
sparte: Robotische Erkunder
body: Io
prereq: un_jupiter_flyby
reward: 234
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde dicht an Io vorbei und sieh die wildeste, vulkanischste Welt des Jupitersystems aus der Nähe.
beschreibung_en: Send a probe close past Io and see the wildest, most volcanic world of the Jupiter system up close.
check: CREW_NONE | unbemannt
check: FLYBY Io 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: un_europa_flyby
sparte: Robotische Erkunder
body: Europa
prereq: un_jupiter_flyby
reward: 246
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Europa vorbei. Unter der hellen Eiskruste wird ein Ozean vermutet — die Daten dieses Vorbeiflugs sind ein wissenschaftliches Versprechen.
beschreibung_en: Send a probe past Europa. An ocean is suspected beneath the bright ice crust — the data from this flyby are a scientific promise.
check: CREW_NONE | unbemannt
check: FLYBY Europa 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: un_ganymede_flyby
sparte: Robotische Erkunder
body: Ganymede
prereq: un_jupiter_flyby
reward: 258
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Ganymed vorbei, den grössten Mond des Sonnensystems. Dein Programm erkundet hier den Ort, der später die einzige bemannte Landung im Jupiterraum tragen wird.
beschreibung_en: Send a probe past Ganymede, the largest moon in the solar system. Here your program scouts the place that will later carry the only crewed landing in Jupiter space.
check: CREW_NONE | unbemannt
check: FLYBY Ganymede 1500 | Vorbeiflug unter 1500 km

=== MISSION ===
id: un_callisto_flyby
sparte: Robotische Erkunder
body: Callisto
prereq: un_jupiter_flyby
reward: 246
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Kallisto vorbei. Der ruhige, vernarbte Aussenmond wirkt wie ein natürlicher Platz für freiwillige Logistik.
beschreibung_en: Send a probe past Callisto. The calm, scarred outer moon looks like a natural spot for optional logistics.
check: CREW_NONE | unbemannt
check: FLYBY Callisto 1500 | Vorbeiflug unter 1500 km

=== MISSION ===
id: un_callisto_landing
sparte: Robotische Erkunder
body: Callisto
prereq: un_callisto_flyby
reward: 330
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf Kallisto und gib dem optionalen Logistikzweig im Jupiterraum festen Boden unter den Füssen.
beschreibung_en: Land a probe on Callisto and give the optional logistics branch in Jupiter space solid ground underfoot.
check: CREW_NONE | unbemannt
check: LANDED Callisto | auf Kallisto gelandet

=== MISSION ===
id: net_callisto_ore_test
sparte: Versorgungsnetz
body: Callisto
prereq: un_callisto_landing
reward: 186
repeatable: no
recordStation: -
stationRef: -
beschreibung: Förder unbemannt Erz auf Kallisto und teste dort eine Reserve für sehr tiefe Expeditionen ins äussere System.
beschreibung_en: Mine ore on Callisto with an uncrewed rig and test a reserve there for very deep expeditions into the outer system.
check: CREW_NONE | unbemannt
check: LANDED Callisto | auf Kallisto gelandet
check: ORE_SURFACE Callisto | Ore auf Oberfläche gefördert
check: RESOURCE_MIN Ore 80 | Ore über 80

=== MISSION ===
id: net_callisto_supply_cache
sparte: Versorgungsnetz
body: Callisto
prereq: net_callisto_ore_test
reward: 216
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Lege auf Kallisto einen Treibstoff-Vorratspunkt an. Er stützt freiwillige Fernpläne, während Ganymed der bemannte Hauptpfad bleibt.
beschreibung_en: Lay down a fuel cache on Callisto. It supports optional far-range plans while Ganymede stays the crewed main path.
check: CREW_NONE | unbemannt
check: LANDED Callisto | auf Kallisto gelandet
check: FUEL_MIN 1000 | Treibstoff über 1000

=== MISSION ===
id: cr_jupiter_flyby
sparte: Pioniere
body: Jupiter
prereq: un_jupiter_orbit, cr_mars_station_longstay3, cr_earth_station_longstay12
reward: 780
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke drei Kerbals durch den Jupiterraum. Der erste bemannte Besuch bei einem Gasriesen macht dein Programm endgültig zur Fernraumorganisation.
beschreibung_en: Send three Kerbals through Jupiter space. The first crewed visit to a gas giant makes your program a deep-space organisation for good.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: FLYBY Jupiter 80000 | Vorbeiflug unter 80000 km

=== MISSION ===
id: cr_jupiter_orbit
sparte: Pioniere
body: Jupiter
prereq: cr_jupiter_flyby, un_ganymede_flyby
reward: 960
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe drei Kerbals in den Jupiterorbit und halte sie zehn Tage in einer Region, die einst allein den Sonden gehörte.
beschreibung_en: Put three Kerbals into Jupiter orbit and hold them ten days in a region that once belonged to probes alone.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: ORBIT_ABOVE Jupiter | stabiler Jupiter-Orbit
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_ganymede_orbit
sparte: Pioniere
body: Ganymede
prereq: cr_jupiter_orbit, un_ganymede_flyby
reward: 900
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe drei Kerbals in den Orbit um Ganymed. Der grösste Mond wird zum sicheren Trittstein im gewaltigen Jupitersystem.
beschreibung_en: Put three Kerbals into orbit around Ganymede. The largest moon becomes a safe stepping stone in the vast Jupiter system.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: ORBIT_ABOVE Ganymede 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_ganymede_landing
sparte: Pioniere
body: Ganymede
prereq: cr_ganymede_orbit
reward: 1140
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande drei Kerbals auf Ganymed. Das Jupitersystem bekommt seinen einzigen bemannten Landungshöhepunkt — ein Fussabdruck unvorstellbar weit von zu Hause. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Land three Kerbals on Ganymede. The Jupiter system gets its single crewed landing highlight — a footprint unimaginably far from home. The crew must then return safely to Earth.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Ganymede | auf Ganymed gelandet
check: EVA Ganymede LANDED | EVA auf Ganymed
check: RETURN_FROM_BODY Ganymede Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_ganymede_stay_7d
sparte: Pioniere
body: Ganymede
prereq: cr_ganymede_landing
reward: 1080
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte drei Kerbals sieben Tage auf Ganymed. Diese Ausdauer macht Saturn als letztes bemanntes Fernziel überhaupt erst denkbar. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Keep three Kerbals on Ganymede for seven days. This endurance is what makes Saturn conceivable as a final crewed deep-space target at all. The crew must then return safely to Earth.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Ganymede | auf Ganymed gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Ganymede Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: un_saturn_flyby
sparte: Robotische Erkunder
body: Saturn
prereq: un_jupiter_orbit
reward: 390
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Saturn vorbei. Mit diesem Flug rücken zugleich Uranus, Neptun, Pluto und das ferne Arrokoth als Schlussziele in Sicht.
beschreibung_en: Send a probe past Saturn. With this flight, Uranus, Neptune, Pluto and distant Arrokoth come into view as the closing targets.
check: CREW_NONE | unbemannt
check: FLYBY Saturn 80000 | Vorbeiflug unter 80000 km

=== MISSION ===
id: un_saturn_atmo_probe
sparte: Robotische Erkunder
body: Saturn
prereq: un_saturn_flyby
reward: 450
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass eine Sonde in Saturns oberste Wolken eintauchen und sammle direkte Daten unter den berühmten Ringen.
beschreibung_en: Let a probe dip into Saturn's uppermost clouds and gather direct data beneath the famous rings.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Saturn 70 95 | obere Atmosphäre erreicht

=== MISSION ===
id: un_saturn_orbit
sparte: Robotische Erkunder
body: Saturn
prereq: un_saturn_flyby
reward: 495
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Saturnorbit. Der Ringplanet wird zur letzten grossen Bühne, bevor Kerbals nach Titan aufbrechen.
beschreibung_en: Place a probe into Saturn orbit. The ringed planet becomes the last great stage before Kerbals set out for Titan.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Saturn | stabiler Saturn-Orbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_saturn_polar_mapping
sparte: Robotische Erkunder
body: Saturn
prereq: un_saturn_orbit
reward: 610
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in einen steilen Saturnorbit und vermesse Ringe, Polregionen und Monde aus einer neuen Geometrie. Das Ringsystem wird erst aus dieser Perspektive wirklich lesbar.
beschreibung_en: Put a probe into a steep Saturn orbit and survey rings, polar regions and moons from a new geometry. Only from this view does the ring system truly become readable.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Saturn 10000 | Periapsis über 10000 km
check: INCLINATION_MIN Saturn 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: net_saturn_comm_network
sparte: Versorgungsnetz
body: Saturn
prereq: net_jupiter_comm_network, un_saturn_orbit
reward: 460
repeatable: no
recordStation: -
stationRef: -
beschreibung: Spanne das Kommunikationsnetz bis Saturn. Drei Relais im Saturnorbit sichern die Verbindung zu Ringplanet, Titan und den fernen Aussenposten der letzten grossen bemannten Reise.
beschreibung_en: Stretch the communications network out to Saturn. Three relays in Saturn orbit secure the link to the ringed planet, Titan and the distant outposts of the last great crewed voyage.
check: CREW_NONE | aktives Fahrzeug unbemannt
check: VESSEL_COUNT Saturn 3 10000 | 3 Satelliten gleichzeitig im Saturnorbit, Periapsis über 10000 km
check: DURATION 1 | 1 Tag ununterbrochen ausharren

=== MISSION ===
id: un_titan_flyby
sparte: Robotische Erkunder
body: Titan
prereq: un_saturn_flyby
reward: 345
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde dicht an Titan vorbei und fange erste Signale aus seiner orangefarbenen Dunsthülle ein. Was darunter liegt, ahnt bisher niemand — doch die Daten entscheiden, ob der Mond einmal das letzte grosse Ziel einer bemannten Landung wird.
beschreibung_en: Send a probe close past Titan and catch the first signals from its orange haze. What lies beneath, no one yet knows — but the data decide whether the moon will one day be the final great target of a crewed landing.
check: CREW_NONE | unbemannt
check: FLYBY Titan 3000 | Vorbeiflug unter 3000 km

=== MISSION ===
id: un_titan_orbit
sparte: Robotische Erkunder
body: Titan
prereq: un_titan_flyby
reward: 435
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Titanorbit und kartiere die Welt, auf der deine Kerbals eines Tages am weitesten draussen landen werden.
beschreibung_en: Place a probe into Titan orbit and map the world where your Kerbals will one day land farthest from home.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Titan | stabiler Titan-Orbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_titan_polar_mapping
sparte: Robotische Erkunder
body: Titan
prereq: un_titan_orbit
reward: 540
repeatable: no
recordStation: -
stationRef: -
beschreibung: Kartiere Titan aus einem polaren Orbit. Die steile Bahn sammelt Daten über Seen, Dunstschichten und mögliche Landegebiete, bevor Kerbals sich an diese ferne Welt wagen.
beschreibung_en: Map Titan from a polar orbit. The steep path gathers data on lakes, haze layers and possible landing sites before Kerbals dare approach this distant world.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Titan 250 | Periapsis über 250 km
check: INCLINATION_MIN Titan 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_titan_atmo_probe
sparte: Robotische Erkunder
body: Titan
prereq: un_titan_orbit
reward: 480
repeatable: no
recordStation: -
stationRef: -
beschreibung: Tauche eine Sonde in Titans dichte obere Atmosphäre. Jede Messung macht die fremde Welt greifbarer und die spätere Landung sicherer.
beschreibung_en: Dip a probe into Titan's dense upper atmosphere. Every measurement makes the alien world more tangible and the later landing safer.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Titan 60 90 | obere Atmosphäre erreicht

=== MISSION ===
id: un_titan_landing
sparte: Robotische Erkunder
body: Titan
prereq: un_titan_atmo_probe
reward: 570
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf Titan. Der künftige letzte bemannte Landeort deines Programms bekommt seinen ersten sicheren Bodenkontakt.
beschreibung_en: Land a probe on Titan. Your program's future final crewed landing site gets its first safe surface contact.
check: CREW_NONE | unbemannt
check: LANDED Titan | auf Titan gelandet

=== MISSION ===
id: un_titan_polar_landing
sparte: Robotische Erkunder
body: Titan
prereq: un_titan_landing, un_titan_polar_mapping
reward: 620
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine Sonde punktgenau in der Polregion von Titan ab. Unter dichtem Dunst und bei wenig Sicht ist eine Ziellandung an Titans Polen die anspruchsvollste robotische Landung deines Programms.
beschreibung_en: Set a probe down precisely in Titan's polar region. Under thick haze and poor visibility, a pinpoint landing at Titan's poles is your program's most demanding robotic landing.
check: CREW_NONE | unbemannt
check: LANDED Titan | auf Titan gelandet
check: MARKER_LANDING Titan 15 70 90 | Pollandung im Umkreis von 15 km

=== MISSION ===
id: un_enceladus_flyby
sparte: Robotische Erkunder
body: Enceladus
prereq: un_saturn_orbit
reward: 294
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Enceladus vorbei, den kleinen Eismond mit den Geysiren. Neben Titan wird er zum wichtigsten robotischen Ziel im Saturnsystem.
beschreibung_en: Send a probe past Enceladus, the small icy moon with the geysers. Alongside Titan, it becomes the most important robotic target in the Saturn system.
check: CREW_NONE | unbemannt
check: FLYBY Enceladus 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_enceladus_orbit
sparte: Robotische Erkunder
body: Enceladus
prereq: un_enceladus_flyby
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in eine enge Bahn um Enceladus und untersuche die Eiswelt genauer als jeden anderen kleinen Saturnmond.
beschreibung_en: Put a probe into a tight orbit around Enceladus and study the ice world more closely than any other small Saturn moon.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Enceladus 10 | Periapsis über 10 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_enceladus_landing
sparte: Robotische Erkunder
body: Enceladus
prereq: un_enceladus_orbit
reward: 450
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf Enceladus. Die kleine Eiswelt mit ihren Fontänen wird zum robotischen Höhepunkt neben Titan.
beschreibung_en: Land a probe on Enceladus. The small icy world with its plumes becomes a robotic highlight alongside Titan.
check: CREW_NONE | unbemannt
check: LANDED Enceladus | auf Enceladus gelandet

=== MISSION ===
id: un_rhea_flyby
sparte: Robotische Erkunder
body: Rhea
prereq: un_saturn_orbit
reward: 240
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Rhea vorbei und ergänze dein Saturnarchiv um einen kurzen, sauberen Besuch.
beschreibung_en: Send a probe past Rhea and add a short, clean visit to your Saturn archive.
check: CREW_NONE | unbemannt
check: FLYBY Rhea 800 | Vorbeiflug unter 800 km

=== MISSION ===
id: un_iapetus_flyby
sparte: Robotische Erkunder
body: Iapetus
prereq: un_saturn_orbit
reward: 255
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Iapetus vorbei, den zweifarbigen Mond. Seine rätselhafte Oberfläche liefert deinem Programm einen markanten Moment.
beschreibung_en: Send a probe past Iapetus, the two-toned moon. Its puzzling surface gives your program a striking moment.
check: CREW_NONE | unbemannt
check: FLYBY Iapetus 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: un_dione_flyby
sparte: Robotische Erkunder
body: Dione
prereq: un_saturn_orbit
reward: 228
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Dione vorbei — ein knapper, präziser Wissenschaftsbesuch im Saturnsystem.
beschreibung_en: Send a probe past Dione — a brief, precise science visit in the Saturn system.
check: CREW_NONE | unbemannt
check: FLYBY Dione 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_tethys_flyby
sparte: Robotische Erkunder
body: Tethys
prereq: un_saturn_orbit
reward: 222
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Tethys vorbei und sammle ein weiteres Puzzleteil des Ringplaneten-Systems.
beschreibung_en: Send a probe past Tethys and collect another piece of the ringed planet's puzzle.
check: CREW_NONE | unbemannt
check: FLYBY Tethys 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_mimas_flyby
sparte: Robotische Erkunder
body: Mimas
prereq: un_saturn_orbit
reward: 216
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde am kleinen Mimas vorbei, dessen riesiger Krater ihn fast wie eine Kampfstation aussehen lässt.
beschreibung_en: Send a probe past little Mimas, whose enormous crater makes it look almost like a battle station.
check: CREW_NONE | unbemannt
check: FLYBY Mimas 300 | Vorbeiflug unter 300 km

=== MISSION ===
id: un_hyperion_flyby
sparte: Robotische Erkunder
body: Hyperion
prereq: un_saturn_orbit
reward: 228
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Hyperion vorbei. Der unregelmässige, schwammartige Mond wirkt wie ein rätselhaftes Bruchstück.
beschreibung_en: Send a probe past Hyperion. The irregular, sponge-like moon looks like a puzzling fragment.
check: CREW_NONE | unbemannt
check: FLYBY Hyperion 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_phoebe_flyby
sparte: Robotische Erkunder
body: Phoebe
prereq: un_saturn_orbit
reward: 246
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Phoebe vorbei, den fernen, rückläufigen Aussenmond, der den Rand des Saturnsystems markiert.
beschreibung_en: Send a probe past Phoebe, the distant, retrograde outer moon that marks the edge of the Saturn system.
check: CREW_NONE | unbemannt
check: FLYBY Phoebe 800 | Vorbeiflug unter 800 km

=== MISSION ===
id: cr_saturn_flyby
sparte: Pioniere
body: Saturn
prereq: un_saturn_orbit, cr_ganymede_stay_7d, cr_mars_station_longstay6
reward: 1380
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke drei Kerbals durch den Saturnraum. Die Ringe markieren den letzten grossen bemannten Aufbruch deines Programms.
beschreibung_en: Send three Kerbals through Saturn space. The rings mark your program's last great crewed departure.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: FLYBY Saturn 100000 | Vorbeiflug unter 100000 km

=== MISSION ===
id: cr_saturn_orbit
sparte: Pioniere
body: Saturn
prereq: cr_saturn_flyby, un_titan_landing
reward: 1620
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe drei Kerbals in den Saturnorbit und halte sie zehn Tage. Die Besatzung steht am Rand des letzten begehbaren Kapitels.
beschreibung_en: Put three Kerbals into Saturn orbit and hold them ten days. The crew stands at the threshold of the last accessible chapter.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: ORBIT_ABOVE Saturn | stabiler Saturn-Orbit
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_titan_orbit
sparte: Pioniere
body: Titan
prereq: cr_saturn_orbit, un_titan_landing
reward: 1500
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe drei Kerbals in den Orbit um Titan. Unter ihnen liegt die letzte Welt, die dein Programm bemannt betreten wird.
beschreibung_en: Put three Kerbals into orbit around Titan. Below them lies the last world your program will set foot on.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: ORBIT_ABOVE Titan | stabiler Titan-Orbit
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_titan_landing
sparte: Pioniere
body: Titan
prereq: cr_titan_orbit
reward: 1860
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande drei Kerbals auf Titan. Diese dichte, fremde Welt wird zur letzten grossen bemannten Landung deiner ganzen Kampagne. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Land three Kerbals on Titan. This dense, alien world becomes the final great crewed landing of your entire campaign. The crew must then return safely to Earth.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: EVA Titan LANDED | EVA auf Titan
check: RETURN_FROM_BODY Titan Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: cr_titan_stay_7d
sparte: Pioniere
body: Titan
prereq: cr_titan_landing
reward: 1740
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte drei Kerbals sieben Tage auf Titan. Danach übernehmen wieder Sonden den Weg hinaus in die fernsten Regionen. Die Crew muss danach sicher zur Erde zurückkehren.
beschreibung_en: Keep three Kerbals on Titan for seven days. After that, probes take over the way out into the most distant regions again. The crew must then return safely to Earth.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren
check: RETURN_FROM_BODY Titan Earth | Crew sicher zur Erde zurückbringen

=== MISSION ===
id: net_titan_supply_test
sparte: Versorgungsnetz
body: Titan
prereq: cr_titan_landing
reward: 270
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Lande ein unbemanntes Versorgungsfahrzeug auf Titan und leg dem fernsten bemannten Zielraum eine logistische Reserve hin.
beschreibung_en: Land an uncrewed supply vehicle on Titan and lay a logistical reserve down in the most distant crewed target zone.
check: CREW_NONE | unbemannt
check: LANDED Titan | auf Titan gelandet
check: FUEL_MIN 800 | Treibstoff über 800

=== MISSION ===
id: net_saturn_transfer_cache
sparte: Versorgungsnetz
body: Saturn
prereq: cr_saturn_orbit
reward: 330
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Bring ein unbemanntes Versorgungsfahrzeug mit vollen Tanks in den Saturnorbit und stütze die Rückkehrplanung im letzten bemannten Fernraum.
beschreibung_en: Bring an uncrewed supply vehicle with full tanks into Saturn orbit and support return planning in the last crewed deep-space region.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Saturn | stabiler Saturn-Orbit
check: FUEL_MIN 1200 | Treibstoff über 1200
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_uranus_flyby
sparte: Robotische Erkunder
body: Uranus
prereq: un_saturn_flyby
reward: 480
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Uranus vorbei. Nach Saturn füllt sich die letzte Karte deines Programms nun in mehrere Richtungen zugleich.
beschreibung_en: Send a probe past Uranus. After Saturn, your program's final map now fills in several directions at once.
check: CREW_NONE | unbemannt
check: FLYBY Uranus 80000 | Vorbeiflug unter 80000 km

=== MISSION ===
id: un_uranus_atmo_probe
sparte: Robotische Erkunder
body: Uranus
prereq: un_uranus_flyby
reward: 555
repeatable: no
recordStation: -
stationRef: -
beschreibung: Tauche eine Sonde in den gekippten Eisriesen Uranus. Direkte Messungen aus einer Distanz, die einst unvorstellbar war.
beschreibung_en: Dip a probe into the tilted ice giant Uranus. Direct measurements from a distance that was once unimaginable.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Uranus 70 95 | obere Atmosphäre erreicht

=== MISSION ===
id: un_titania_flyby
sparte: Robotische Erkunder
body: Titania
prereq: un_uranus_flyby
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Titania vorbei, den grössten Uranusmond — ein kurzes, wertvolles Fenster in eine sehr ferne Region.
beschreibung_en: Send a probe past Titania, the largest moon of Uranus — a brief, valuable window into a very distant region.
check: CREW_NONE | unbemannt
check: FLYBY Titania 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: un_oberon_flyby
sparte: Robotische Erkunder
body: Oberon
prereq: un_uranus_flyby
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Oberon vorbei und ergänze das Schlussarchiv deines Programms um den äussersten grossen Uranusmond.
beschreibung_en: Send a probe past Oberon and add the outermost large moon of Uranus to your program's closing archive.
check: CREW_NONE | unbemannt
check: FLYBY Oberon 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: un_ariel_flyby
sparte: Robotische Erkunder
body: Ariel
prereq: un_uranus_flyby
reward: 294
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Ariel vorbei. Die letzte Epoche sammelt ferne Momentaufnahmen von hohem wissenschaftlichem Wert.
beschreibung_en: Send a probe past Ariel. The final era gathers distant snapshots of high scientific value.
check: CREW_NONE | unbemannt
check: FLYBY Ariel 800 | Vorbeiflug unter 800 km

=== MISSION ===
id: un_umbriel_flyby
sparte: Robotische Erkunder
body: Umbriel
prereq: un_uranus_flyby
reward: 294
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an den dunklen Umbriel vorbei. Auch die finstersten fernen Monde bekommen ihren Platz in deiner Chronik.
beschreibung_en: Send a probe past dark Umbriel. Even the dimmest distant moons earn their place in your chronicle.
check: CREW_NONE | unbemannt
check: FLYBY Umbriel 800 | Vorbeiflug unter 800 km

=== MISSION ===
id: un_miranda_flyby
sparte: Robotische Erkunder
body: Miranda
prereq: un_uranus_flyby
reward: 282
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Miranda vorbei, deren zerklüftete Oberfläche den langen Flug mit lauter neuen Rätseln belohnt.
beschreibung_en: Send a probe past Miranda, whose jagged surface rewards the long flight with all kinds of new riddles.
check: CREW_NONE | unbemannt
check: FLYBY Miranda 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_puck_flyby
sparte: Robotische Erkunder
body: Puck
prereq: un_uranus_flyby
reward: 246
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde am kleinen Puck vorbei — ein kurzer Schlussauftritt für einen unscheinbaren Mond.
beschreibung_en: Send a probe past little Puck — a brief closing appearance for an unremarkable moon.
check: CREW_NONE | unbemannt
check: FLYBY Puck 300 | Vorbeiflug unter 300 km

=== MISSION ===
id: un_neptune_flyby
sparte: Robotische Erkunder
body: Neptune
prereq: un_saturn_flyby
reward: 540
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Neptun vorbei. Der tiefblaue Eisriese öffnet das grosse Finale rund um Triton.
beschreibung_en: Send a probe past Neptune. The deep-blue ice giant opens the grand finale around Triton.
check: CREW_NONE | unbemannt
check: FLYBY Neptune 80000 | Vorbeiflug unter 80000 km

=== MISSION ===
id: un_neptune_atmo_probe
sparte: Robotische Erkunder
body: Neptune
prereq: un_neptune_flyby
reward: 615
repeatable: no
recordStation: -
stationRef: -
beschreibung: Tauche eine Sonde in Neptuns obere Atmosphäre — Messungen vom äussersten Rand der klassischen Planetenwelt.
beschreibung_en: Dip a probe into Neptune's upper atmosphere — readings from the outermost edge of the classical planetary realm.
check: CREW_NONE | unbemannt
check: ATMO_FRACTION Neptune 70 95 | obere Atmosphäre erreicht

=== MISSION ===
id: un_triton_flyby
sparte: Robotische Erkunder
body: Triton
prereq: un_neptune_flyby
reward: 450
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Triton vorbei, den grossen rückläufigen Mond. Er wird zum eigentlichen Ziel im Neptunsystem.
beschreibung_en: Send a probe past Triton, the large retrograde moon. It becomes the real target in the Neptune system.
check: CREW_NONE | unbemannt
check: FLYBY Triton 1500 | Vorbeiflug unter 1500 km

=== MISSION ===
id: un_triton_orbit
sparte: Robotische Erkunder
body: Triton
prereq: un_triton_flyby
reward: 660
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Triton. Die Schluss-Epoche bekommt eines ihrer wichtigsten wissenschaftlichen Zentren.
beschreibung_en: Put a probe into orbit around Triton. The closing era gains one of its most important scientific centres.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Triton 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_triton_polar_mapping
sparte: Robotische Erkunder
body: Triton
prereq: un_triton_orbit
reward: 820
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in eine polare Tritonbahn und vermesse den grossen rückläufigen Mond von allen Breiten. Für die Schluss-Epoche ist das eine der wertvollsten Karten im Neptunsystem.
beschreibung_en: Put a probe into a polar Triton orbit and survey the large retrograde moon from every latitude. For the closing era, this is one of the most valuable maps in the Neptune system.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Triton 50 | Periapsis über 50 km
check: INCLINATION_MIN Triton 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_triton_landing
sparte: Robotische Erkunder
body: Triton
prereq: un_triton_orbit
reward: 1020
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf Triton. Das Neptunsystem erhält sein grosses robotisches Landungsfinale, fast unfassbar weit von der Erde.
beschreibung_en: Land a probe on Triton. The Neptune system gets its great robotic landing finale, almost incomprehensibly far from Earth.
check: CREW_NONE | unbemannt
check: LANDED Triton | auf Triton gelandet

=== MISSION ===
id: un_nereid_flyby
sparte: Robotische Erkunder
body: Nereid
prereq: un_neptune_flyby
reward: 285
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Nereid vorbei, den fernen, exzentrischen Mond — ein kurzer, kostbarer Besuch.
beschreibung_en: Send a probe past Nereid, the distant, eccentric moon — a brief, precious visit.
check: CREW_NONE | unbemannt
check: FLYBY Nereid 800 | Vorbeiflug unter 800 km

=== MISSION ===
id: un_proteus_flyby
sparte: Robotische Erkunder
body: Proteus
prereq: un_neptune_flyby
reward: 270
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Proteus vorbei. Die letzte Karte füllt sich Stück für Stück mit kleinen, fernen Welten.
beschreibung_en: Send a probe past Proteus. The final map fills in piece by piece with small, distant worlds.
check: CREW_NONE | unbemannt
check: FLYBY Proteus 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: un_pluto_flyby
sparte: Robotische Erkunder
body: Pluto
prereq: un_saturn_flyby
reward: 600
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Pluto vorbei. Am Rand der bekannten Karte wartet eine Welt, die lange nur ein Lichtpunkt war.
beschreibung_en: Send a probe past Pluto. At the edge of the known map waits a world that was long only a point of light.
check: CREW_NONE | unbemannt
check: FLYBY Pluto 20000 | Vorbeiflug unter 20000 km

=== MISSION ===
id: un_pluto_orbit
sparte: Robotische Erkunder
body: Pluto
prereq: un_pluto_flyby
reward: 840
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe eine Sonde in den Orbit um Pluto und mach aus dem flüchtigen Vorbeiflug eine letzte grosse Untersuchung.
beschreibung_en: Put a probe into orbit around Pluto and turn the fleeting flyby into one last great survey.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Pluto 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_pluto_polar_mapping
sparte: Robotische Erkunder
body: Pluto
prereq: un_pluto_orbit
reward: 1020
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lege eine Sonde in einen polaren Plutoorbit und mache aus dem fernen Lichtpunkt eine echte Weltkarte. Die hohe Inklination liefert die Daten, die ein einzelner Äquatororbit nie vollständig sieht.
beschreibung_en: Put a probe into a polar Pluto orbit and turn the distant point of light into a real world map. The high inclination delivers the data a single equatorial orbit never fully sees.
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Pluto 50 | Periapsis über 50 km
check: INCLINATION_MIN Pluto 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: un_pluto_landing
sparte: Robotische Erkunder
body: Pluto
prereq: un_pluto_orbit
reward: 1260
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande eine Sonde auf Pluto. Die ferne, eisige Oberfläche wird zum letzten grossen Landepunkt deiner robotischen Kampagne.
beschreibung_en: Land a probe on Pluto. The distant, icy surface becomes the last great landing point of your robotic campaign.
check: CREW_NONE | unbemannt
check: LANDED Pluto | auf Pluto gelandet

=== MISSION ===
id: un_charon_flyby
sparte: Robotische Erkunder
body: Charon
prereq: un_pluto_flyby
reward: 360
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Charon vorbei, Plutos grossen Begleiter. Das Doppelsystem bekommt seinen Platz im Abschlussarchiv.
beschreibung_en: Send a probe past Charon, Pluto's large companion. The double system earns its place in the closing archive.
check: CREW_NONE | unbemannt
check: FLYBY Charon 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: un_arrokoth_flyby
sparte: Robotische Erkunder
body: Arrokoth
prereq: un_saturn_flyby
reward: 1500
repeatable: no
recordStation: -
stationRef: -
beschreibung: Schicke eine Sonde an Arrokoth vorbei, den fernsten je besuchten Brocken. Dein Programm erreicht das symbolische Ende der bekannten Karte.
beschreibung_en: Send a probe past Arrokoth, the most distant rock ever visited. Your program reaches the symbolic end of the known map.
check: CREW_NONE | unbemannt
check: FLYBY Arrokoth 1000 | Vorbeiflug unter 1000 km

=== MISSION ===
id: cr_titan_base4
sparte: Pioniere
body: Titan
prereq: cr_titan_stay_7d
reward: 1350
repeatable: no
recordStation: -
stationRef: -
beschreibung: Errichte auf Titan den fernsten bemannten Aussenposten und halte ihn mit vier Kerbals dreissig Tage am Leben. Am äussersten Rand des begehbaren Raums entsteht ein neues Zuhause.
beschreibung_en: Build the most distant crewed outpost on Titan and keep it alive with four Kerbals for thirty days. At the very edge of accessible space, a new home takes shape.
check: CREW_MIN 4 | mindestens 4 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: DURATION 30 | 30 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_titan_base6
sparte: Pioniere
body: Titan
prereq: cr_titan_base4
reward: 1770
repeatable: no
recordStation: -
stationRef: -
beschreibung: Erweitere die Titanbasis auf sechs Kerbals und halte sie sechzig Tage stabil. Kein neuer Aufbruch, sondern Wachstum am äussersten bemannten Rand.
beschreibung_en: Expand the Titan base to six Kerbals and hold it steady for sixty days. Not a new departure, but growth at the outermost crewed frontier.
check: CREW_MIN 6 | mindestens 6 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: DURATION 60 | 60 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_titan_base8
sparte: Pioniere
body: Titan
prereq: cr_titan_base6
reward: 2280
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bring die Titanbasis auf acht Kerbals und halte sie hundertfünfzig Tage am Leben. Damit endet der bemannte Teil deiner Kampagne — während die fernsten Sonden weiter berichten.
beschreibung_en: Bring the Titan base to eight Kerbals and keep it alive for a hundred and fifty days. With this, the crewed part of your campaign ends — while the most distant probes keep reporting.
check: CREW_MIN 8 | mindestens 8 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: DURATION 150 | 150 Tage ununterbrochen ausharren
