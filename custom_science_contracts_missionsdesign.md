# WISSENSCHAFT

Das Missionssystem bleibt ein Bonus zum normalen Science-Mode und ersetzt Stock-Science nicht. Die Belohnungen unterstützen den Fortschritt, aber Experimente, Proben und eigene Erkundung bleiben die Hauptquelle für grosse Tech-Sprünge.

Frühe Erdmissionen geben kleine Werte, Luna und cislunare Infrastruktur geben spürbare mittlere Werte, Mars gibt hohe Werte, und die äusseren Welten liefern grosse Einzelboni. Wiederholbare Versorgung, Depots und Logistik zahlen moderat aus, damit sie nützlich bleiben, aber keine Erstmission verdrängen.

Grobe Skala: frühe Erde 6–130, Luna 70–225, innere Planetenziele 100–550, Mars bemannt 540–630, Asteroiden 65–450, Jupiter 235–1150, Saturn und Titan 215–1860, Fernsonden 245–1500, Titanbasis-Finale 1350–2280.

# ABLAUF

Die Kampagne beginnt historisch und vorsichtig. Unbemannte Testflüge verlassen zuerst den Startbereich, steigen in die obere Atmosphäre, verlassen die Atmosphäre suborbital und erreichen anschliessend den ersten Erdorbit. Erst danach folgen bemannte suborbitale Flüge, der erste bemannte Erdorbit, die erste EVA, ein Docking im Erdorbit, ein 7-Tage-Flug und ein temporäres Ein-Modul-Labor. Dieses Labor dauert 15 Tage, bleibt kein Teil der späteren Stationskette und wird danach narrativ deorbitiert.

Luna ist der erste grosse Horizont. Nach der ersten EVA im Erdorbit wird der unbemannte Luna-Vorbeiflug geöffnet, danach folgen unbemannter Luna-Orbit und unbemannte Luna-Landung. Der bemannte Luna-Vorbeiflug `cr_luna_flyby_crewed` ist der Meilenstein, ab dem gesperrte Aufträge als Vorschau erscheinen. Der erste bemannte Luna-Orbit braucht den bemannten Vorbeiflug und die erste unbemannte Landung. Die erste bemannte Luna-Landung enthält direkt die erste Oberflächen-EVA.

Nach der ersten bemannten Luna-Landung beginnt die echte Erdstation. Sie ist eine neue Stationskette und hat keine technische Verbindung zum temporären Ein-Modul-Labor. Die Erdstation wächst epochenweise: in Epoche 3 bis Crew 4, in Epoche 4 bis Crew 8 und in Epoche 5 bis Crew 12. Mondstation und Mondbasis starten in Epoche 3 mit Crew 2, wachsen in Epoche 3 auf 3, in Epoche 4 auf 4 und 6 und in Epoche 5 bis 6 auf 8 und 10. Eine Mondbasis setzt bemannte Luna-Präzisionslandung und 7 Tage auf Luna voraus. Das Erdorbit-Fuel-Depot ist ein optionaler Versorgungsnetz-Zweig, öffnet später als die Erdstation und blockiert keine bemannte Marsmission.

Ab Epoche 3 wird das frühe Erd-Satellitennetz aus der Pionierzeit narrativ zu alt. Im Versorgungsnetz entsteht deshalb eine neue Kommunikations-Lebensader: zuerst Erde, dann Luna, danach Mars, ein interplanetarer Sonnenorbit-Ring und schliesslich die grossen Relais um Jupiter und Saturn. Erde und Luna bekommen je eine Grundkonstellation mit drei Satelliten und danach eine polare Ausbaustufe mit drei weiteren Satelliten.

Das innere Sonnensystem öffnet sich robotisch nach dem bemannten Luna-Vorbeiflug. Venus bekommt Vorbeiflug, Orbit, Atmosphärensonde und Landung, bemannt nur Vorbeiflug und Orbit. Merkur bleibt komplett robotisch. Mars wird robotisch vorbereitet und erhält bereits in Epoche 4 einen bemannten Vorbeiflug und einen bemannten 10-Tage-Orbit. Für bemannten Mars-Vorbeiflug und bemannten Mars-Orbit reicht die normale Erdstation mit Longstay-Stufe 3.

Mars ist der zweite grosse bemannte Hauptbogen. Nach bemanntem Mars-Orbit, robotischer Präzisionslandung und Langzeiterfahrung folgen bemannte Marslandung mit EVA, 10 Tage auf Mars und 30 Tage auf Mars. Phobos und Deimos werden nach bemannter Marslandung parallel als bemannter Orbit und bemannte Landung verfügbar; die Landung setzt den bemannten Orbit desselben Mondes nicht voraus. Marsstation und Marsbasis starten danach als neue Infrastruktur.

Asteroiden sind vollständig Bonus. Eros, Vesta, Ceres, Pallas, Psyche, Ryugu, Ida, Dactyl und Arrokoth geben Wissenschaft, Ressourcenoptionen und erzählerische Tiefe, blockieren aber keine Hauptmission zu Mars, Jupiter, Saturn, Titan, Triton, Pluto und Arrokoth. Ceres ist das einzige bemannte Asteroidenziel und bleibt ein Prestigeast.

Jupiter wird robotisch früh sichtbar: der unbemannte Jupiter-Vorbeiflug öffnet nach unbemanntem Mars-Orbit, der unbemannte Jupiter-Orbit folgt direkt danach. Io, Europa, Ganymed und Kallisto erhalten robotische Vorbeiflüge. Kallisto erhält einen optionalen Ressourcenzweig. Ganymed ist das einzige bemannte Landungsziel im Jupitersystem.

Saturn öffnet nach dem unbemannten Jupiter-Orbit. Im Saturnsystem erhalten nur Titan und Enceladus Orbit- und Landemissionen. Rhea, Iapetus, Dione, Tethys, Mimas, Hyperion und Phoebe bleiben Vorbeiflugziele. Titan ist das letzte reguläre bemannte Landungsziel. Nach dem unbemannten Saturn-Vorbeiflug werden Uranus, Neptun, Pluto und Arrokoth gleichzeitig sichtbar.

Die Schluss-Epoche bleibt robotisch. Triton und Pluto erhalten jeweils Vorbeiflug, Orbit und Landung. Uranus, Uranusmonde, Neptunmonde, Charon und Arrokoth bleiben Vorbeiflugziele. Gasriesen und Eisriesen erhalten nach dem jeweiligen Vorbeiflug eine unbemannte Atmosphärensonde. Als letzter bemannter Abschluss entsteht auf Titan eine Sonderbasis mit Crew 4, danach Crew 6 und Crew 8, ohne normale Versorgungskette zwischen den Ausbaustufen.

# STATIONSKETTEN

chain: body=Earth | key=earth_station | typ=station | prereq=cr_luna_landing | stufen=2,3,4,6,8,10,12
chain: body=Earth | key=earth_fuel_depot | typ=station | prereq=cr_earth_station_longstay4 | stufen=2,3,4,6
chain: body=Moon | key=moon_station | typ=station | prereq=cr_earth_station_longstay4 | stufen=2,3,4,6,8,10
chain: body=Moon | key=moon_base | typ=base | prereq=cr_luna_precision_landing, cr_luna_stay_7d | stufen=2,3,4,6,8,10
chain: body=Mars | key=mars_station | typ=station | prereq=cr_mars_stay_10d | stufen=2,3,4,6
chain: body=Mars | key=mars_base | typ=base | prereq=cr_mars_stay_30d | stufen=2,3,4,6

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
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DURATION 3 | 3 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_earth_docking_demo
sparte: Pioniere
body: Earth
prereq: cr_earth_duration_3d
reward: 88
repeatable: no
recordStation: -
stationRef: -
beschreibung: Führe das erste Andockmanöver zwischen zwei Fahrzeugen im Erdorbit durch. Diese Technik wird später Stationen, Depots und interplanetare Schiffe zusammenfügen — heute übst du sie zum ersten Mal.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Earth | stabiler Erdorbit
check: DOCK_ANY | beliebiges Andockmanöver

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
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Moon 100 | Periapsis über 100 km
check: INCLINATION_MIN Moon 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

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
check: CREW_NONE | unbemannt
check: LANDED Moon | auf Luna gelandet

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
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: ORBIT_ABOVE Moon 20 | Periapsis über 20 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_luna_landing
sparte: Pioniere
body: Moon
prereq: cr_luna_orbit
reward: 208
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze den ersten Kerbal auf Luna ab und lass ihn aussteigen. Ein Fussabdruck, der bleibt — und der Anfang von allem, was dein Programm später auf der Oberfläche aufbaut, von der ersten Basis bis zum Daueraufenthalt.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: EVA Moon LANDED | EVA auf Luna

=== MISSION ===
id: cr_luna_stay_2d
sparte: Pioniere
body: Moon
prereq: cr_luna_landing
reward: 168
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lass eine Besatzung zwei Tage auf dem Mond arbeiten. Aus der ersten Landung werden die ersten echten Arbeitsschichten auf einer anderen Welt.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: DURATION 2 | 2 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_luna_precision_landing
sparte: Pioniere
body: Moon
prereq: cr_luna_landing
reward: 184
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bring eine bemannte Landefähre punktgenau am vorbereiteten Mondgebiet nieder. So beweist dein Programm, dass Module und Besatzungen sich später am selben Ort treffen können — die Voraussetzung für eine echte Basis.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: MARKER_LANDING Moon 5 | Landung im Umkreis von 5 km

=== MISSION ===
id: cr_luna_stay_7d
sparte: Pioniere
body: Moon
prereq: cr_luna_stay_2d
reward: 224
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte eine Besatzung eine volle Woche auf Luna. Diese sieben Tage machen die spätere Mondbasis glaubhaft — und den langen Weg nach Mars eine Spur weniger furchteinflössend.
check: CREW_MIN 1 | mindestens 1 Kerbal an Bord
check: LANDED Moon | auf Luna gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren

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
beschreibung: Schicke zwei Kerbals an Venus vorbei. Die Besatzung sieht die Nachbarwelt mit eigenen Augen, während dein Programm sie aus sicherer Distanz erforscht — bemannt bleibt Venus ein Ort des Vorbeiflugs.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: FLYBY Venus 8000 | Vorbeiflug unter 8000 km

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
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mars 250 | Periapsis über 250 km
check: INCLINATION_MIN Mars 75 | Orbit-Inklination über 75 Grad
check: HOLD 10 | 10 Sekunden stabil halten

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
check: CREW_NONE | unbemannt
check: LANDED Mars | auf Mars gelandet

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
beschreibung: Schicke zwei Kerbals an Mars vorbei. Die Besatzung sieht den roten Planeten aus der Nähe — eine Generalprobe für den Tag, an dem dein Programm dort landet.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: FLYBY Mars 8000 | Vorbeiflug unter 8000 km

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
check: CREW_NONE | unbemannt
check: FLYBY Ceres 500 | Vorbeiflug unter 500 km

=== MISSION ===
id: cr_mars_landing
sparte: Pioniere
body: Mars
prereq: cr_mars_orbit, un_mars_precision_landing, cr_earth_station_longstay3, cr_luna_stay_7d
reward: 540
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf dem Mars und lass sie den roten Staub betreten. Nach Luna beginnt hier das zweite grosse Kapitel deines Programms — Menschen auf einer anderen Welt.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: EVA Mars LANDED | EVA auf Mars

=== MISSION ===
id: cr_mars_precision_landing
sparte: Pioniere
body: Mars
prereq: cr_mars_landing
reward: 435
repeatable: no
recordStation: -
stationRef: -
beschreibung: Setze eine bemannte Marslandung punktgenau am vorbereiteten Gebiet ab. Die gewonnene Treffsicherheit ist die Voraussetzung für eine dauerhafte Basis.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: MARKER_LANDING Mars 10 | Landung im Umkreis von 10 km

=== MISSION ===
id: cr_mars_stay_10d
sparte: Pioniere
body: Mars
prereq: cr_mars_landing
reward: 480
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte zwei Kerbals zehn Tage auf dem Mars. Aus der ersten Landung wird eine echte Forschungsmission — der rote Planet wird zum Arbeitsplatz.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_mars_stay_30d
sparte: Pioniere
body: Mars
prereq: cr_mars_stay_10d, cr_mars_precision_landing
reward: 630
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte zwei Kerbals dreissig Tage auf dem Mars. Damit wird der Planet vom Landeziel zum künftigen Aussenposten — und die Marsbasis denkbar.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Mars | auf Mars gelandet
check: DURATION 30 | 30 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_phobos_orbit
sparte: Pioniere
body: Phobos
prereq: cr_mars_landing, un_phobos_orbit
reward: 255
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe zwei Kerbals in den Orbit um Phobos. Der kleine Mond wird zum nahen bemannten Aussenposten im Marsraum.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: ORBIT_ABOVE Phobos 8 | Periapsis über 8 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_phobos_landing
sparte: Pioniere
body: Phobos
prereq: cr_mars_landing, un_phobos_orbit
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf Phobos. Der Marsraum wird vom einzelnen Planeten zu einem System aus Zielen — Mond für Mond.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Phobos | auf Phobos gelandet
check: EVA Phobos LANDED | EVA auf Phobos

=== MISSION ===
id: cr_deimos_orbit
sparte: Pioniere
body: Deimos
prereq: cr_mars_landing, un_deimos_orbit
reward: 255
repeatable: no
recordStation: -
stationRef: -
beschreibung: Bringe zwei Kerbals in den Orbit um Deimos und erreiche damit den äusseren Rand des bemannten Marsraums.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: ORBIT_ABOVE Deimos 8 | Periapsis über 8 km
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: cr_deimos_landing
sparte: Pioniere
body: Deimos
prereq: cr_mars_landing, un_deimos_orbit
reward: 315
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf Deimos. Von hier draussen wirkt Mars wie der Mittelpunkt eines ganz neuen Arbeitsgebiets.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Deimos | auf Deimos gelandet
check: EVA Deimos LANDED | EVA auf Deimos

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
check: CREW_NONE | unbemannt
check: ORBIT_ABOVE Mars | stabiler Mars-Orbit
check: FUEL_MIN 800 | Treibstoff über 800
check: HOLD 10 | 10 Sekunden stabil halten

=== MISSION ===
id: net_phobos_cache
sparte: Versorgungsnetz
body: Phobos
prereq: cr_phobos_landing
reward: 126
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Richte auf Phobos ein unbemanntes Treibstofflager ein. Der kleine Mond wird zum stillen Helfer für alle künftigen Marsoperationen.
check: CREW_NONE | unbemannt
check: LANDED Phobos | auf Phobos gelandet
check: FUEL_MIN 500 | Treibstoff über 500

=== MISSION ===
id: net_deimos_cache
sparte: Versorgungsnetz
body: Deimos
prereq: cr_deimos_landing
reward: 126
repeatable: yes
recordStation: -
stationRef: -
beschreibung: Richte auf Deimos ein unbemanntes Treibstofflager ein und sichere dir eine Reserve am äussersten Rand des Marsraums.
check: CREW_NONE | unbemannt
check: LANDED Deimos | auf Deimos gelandet
check: FUEL_MIN 500 | Treibstoff über 500

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
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: ORBIT_ABOVE Ceres 15 | Periapsis über 15 km
check: DURATION 10 | 10 Tage ununterbrochen ausharren

=== MISSION ===
id: cr_ceres_landing
sparte: Pioniere
body: Ceres
prereq: cr_ceres_orbit
reward: 450
repeatable: no
recordStation: -
stationRef: -
beschreibung: Lande zwei Kerbals auf Ceres. Ein freiwilliger Höhepunkt fernab des Hauptpfads — und ein Zeichen für die enorme Reichweite deines Programms.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Ceres | auf Ceres gelandet
check: EVA Ceres LANDED | EVA auf Ceres

=== MISSION ===
id: cr_ceres_stay_7d
sparte: Pioniere
body: Ceres
prereq: cr_ceres_landing
reward: 405
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte zwei Kerbals sieben Tage auf Ceres. Dein Programm lernt, in winzigen Schwerefeldern über längere Zeit zu leben und zu arbeiten.
check: CREW_MIN 2 | mindestens 2 Kerbals an Bord
check: LANDED Ceres | auf Ceres gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren

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
beschreibung: Lande drei Kerbals auf Ganymed. Das Jupitersystem bekommt seinen einzigen bemannten Landungshöhepunkt — ein Fussabdruck unvorstellbar weit von zu Hause.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Ganymede | auf Ganymed gelandet
check: EVA Ganymede LANDED | EVA auf Ganymed

=== MISSION ===
id: cr_ganymede_stay_7d
sparte: Pioniere
body: Ganymede
prereq: cr_ganymede_landing
reward: 1080
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte drei Kerbals sieben Tage auf Ganymed. Diese Ausdauer macht Saturn als letztes bemanntes Fernziel überhaupt erst denkbar.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Ganymede | auf Ganymed gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren

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
check: CREW_NONE | unbemannt
check: LANDED Titan | auf Titan gelandet

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
beschreibung: Lande drei Kerbals auf Titan. Diese dichte, fremde Welt wird zur letzten grossen bemannten Landung deiner ganzen Kampagne.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: EVA Titan LANDED | EVA auf Titan

=== MISSION ===
id: cr_titan_stay_7d
sparte: Pioniere
body: Titan
prereq: cr_titan_landing
reward: 1740
repeatable: no
recordStation: -
stationRef: -
beschreibung: Halte drei Kerbals sieben Tage auf Titan. Danach übernehmen wieder Sonden den Weg hinaus in die fernsten Regionen.
check: CREW_MIN 3 | mindestens 3 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: DURATION 7 | 7 Tage ununterbrochen ausharren

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
check: CREW_MIN 8 | mindestens 8 Kerbals an Bord
check: LANDED Titan | auf Titan gelandet
check: DURATION 150 | 150 Tage ununterbrochen ausharren
