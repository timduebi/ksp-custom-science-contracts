#!/usr/bin/env python3
"""Ersetzt die 'beschreibung:'-Zeilen in custom_science_contracts_missionsdesign.md durch den
neuen Stil (imperativ + kleine Story + Vorausblick + konkrete Specs). Quelle bleibt der Plan;
danach gen_catalog.py neu laufen lassen. Meldet fehlende/ueberzaehlige IDs."""
import re, os

ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DOC = os.path.join(ROOT, "custom_science_contracts_missionsdesign.md")

DESC = {
 # --- Erde unbemannt ---
 "un_earth_pad_clear": "Schicke deinen ersten unbemannten Prüfkörper knapp über den Startplatz und sammle die allerersten Telemetriedaten. Aus Plänen wird Bewegung — und jeder grosse Flug deines Programms beginnt mit diesem kleinen Hüpfer.",
 "un_earth_upper_atmo": "Treibe die nächste Testkapsel hinauf in die dünne, kalte Oberluft. Die Messungen dort oben geben deinem Team das Vertrauen für schnellere, höhere Flüge — der Orbit ist nicht mehr weit.",
 "un_earth_suborbital": "Lass dein erstes unbemanntes Fahrzeug die Atmosphäre durchstossen und für wenige Minuten ins Schwarze gleiten. Ein suborbitaler Sprung, der aus einem Testprogramm den Anfang echter Raumfahrt macht.",
 "un_earth_orbit": "Bringe deine erste Sonde in eine stabile Erdumlaufbahn und halte sie dort. Damit schreibt dein Programm ein kleines Stück Raumfahrtgeschichte — und beweist, dass du nicht nur hochfliegen, sondern oben bleiben kannst. Der erste bemannte Orbit rückt in greifbare Nähe.",
 "un_earth_science_satellite": "Setze deinen ersten echten Forschungssatelliten aus und lass ihn einen vollen Tag über der Erde arbeiten. Dein Programm lernt, den Orbit nicht nur zu erreichen, sondern als Werkzeug zu nutzen.",
 "un_earth_satellite_pair": "Bringe zwei Satelliten gleichzeitig in die Erdumlaufbahn und halte beide einen Tag im Betrieb. Aus einzelnen Geräten wird ein erstes Netz — die Keimzelle künftiger Navigation und Kommunikation.",
 "un_earth_high_satellite": "Platziere einen Satelliten weit draussen im hohen Erdorbit und halte ihn einen Tag auf Position. Von dort gewinnt dein Programm Überblick, Reichweite und ein Gefühl für die grossen Bahnen, die zu Luna und Mars führen.",
 # --- Erde bemannt ---
 "cr_earth_suborbital": "Schicke den ersten Kerbal über die Atmosphäre hinaus — ein kurzer, mutiger Sprung ins Schwarze. Dieser Flug wird als erster bemannter Schritt deines Programms in die Geschichte eingehen.",
 "cr_earth_orbit": "Bringe den ersten Kerbal in eine stabile Erdumlaufbahn. Ein historischer Moment, der aus deinem Raumfahrtprogramm eine ernstzunehmende Kraft macht — und den Weg zu Ausstieg, Docking und Station ebnet.",
 "cr_earth_orbit_eva": "Lass einen Kerbal das Fahrzeug verlassen und frei im Vakuum schweben. Dieser erste Ausstieg ist die Grundlage für alles, was später Hände im All braucht: Stationen, Reparaturen, ferne Landungen.",
 "cr_earth_duration_3d": "Halte eine Besatzung drei Tage ununterbrochen im Erdorbit. Dein Programm sammelt die erste echte Erfahrung darin, dass Kerbals im All nicht nur kurz zu Gast sind, sondern arbeiten können.",
 "cr_earth_docking_demo": "Führe das erste Andockmanöver zwischen zwei Fahrzeugen im Erdorbit durch. Diese Technik wird später Stationen, Depots und interplanetare Schiffe zusammenfügen — heute übst du sie zum ersten Mal.",
 "cr_earth_duration_7d": "Lass eine Besatzung eine ganze Woche im Erdorbit leben und arbeiten. Sieben Tage Routine im All sind die Generalprobe für die langen Reisen nach Luna und Mars.",
 "cr_earth_trial_station": "Betreibe ein kleines Ein-Modul-Labor mit genau zwei Kerbals fünfzehn Tage lang im Erdorbit. Ein erster Stationsversuch deines Programms — danach wird das Modul ausgemustert, doch die Erfahrung bleibt.",
 # --- Luna unbemannt ---
 "un_luna_flyby": "Schicke eine Sonde dicht an Luna vorbei. Zum ersten Mal berührt dein Programm den Raum einer anderen Welt — ein Vorgeschmack auf alles, was jenseits der Erde wartet.",
 "un_luna_orbit": "Bringe eine Sonde in einen stabilen Mondorbit und beginne, Luna zu kartieren. Aus dem flüchtigen Besuch wird systematische Erkundung — die Grundlage für die erste Landung.",
 "un_luna_landing": "Setze eine unbemannte Sonde sanft auf dem Mond ab. Diese Landung macht den Boden vermessbar und die erste bemannte Mission zur Oberfläche verantwortbar.",
 # --- Luna bemannt ---
 "cr_luna_flyby_crewed": "Schicke erstmals Kerbals bis zum Mond und sicher wieder zurück. Ein bemannter Vorbeiflug, der beweist: deine Crew kann den Heimatorbit verlassen. Ab hier zeichnen sich am Horizont schon die nächsten grossen Ziele ab.",
 "cr_luna_orbit": "Bringe Kerbals in den Orbit um Luna. Die Besatzung ist nicht mehr auf Durchreise, sondern arbeitet an einer fremden Welt — der letzte Schritt vor dem ersten Fussabdruck.",
 "cr_luna_landing": "Setze den ersten Kerbal auf Luna ab und lass ihn aussteigen. Ein Fussabdruck, der bleibt — und der Anfang von allem, was dein Programm später auf der Oberfläche aufbaut, von der ersten Basis bis zum Daueraufenthalt.",
 "cr_luna_stay_2d": "Lass eine Besatzung zwei Tage auf dem Mond arbeiten. Aus der ersten Landung werden die ersten echten Arbeitsschichten auf einer anderen Welt.",
 "cr_luna_precision_landing": "Bring eine bemannte Landefähre punktgenau am vorbereiteten Mondgebiet nieder. So beweist dein Programm, dass Module und Besatzungen sich später am selben Ort treffen können — die Voraussetzung für eine echte Basis.",
 "cr_luna_stay_7d": "Halte eine Besatzung eine volle Woche auf Luna. Diese sieben Tage machen die spätere Mondbasis glaubhaft — und den langen Weg nach Mars eine Spur weniger furchteinflössend.",
 # --- Venus ---
 "un_venus_flyby": "Schicke eine Sonde an Venus vorbei und wirf den ersten nahen Blick auf die verhüllte Schwesterwelt der Erde. Schön von weitem, lebensfeindlich von nahem — und das erste grosse Ziel im inneren System.",
 "un_venus_orbit": "Bringe eine Sonde in den Venusorbit und beobachte eine Welt, die unter ewigen Wolken glüht. Jede Bahn liefert Daten über einen Planeten, der einladend wirkt und doch alles verschlingt.",
 "un_venus_atmo_probe": "Tauche eine Sonde in die obere Venusatmosphäre und miss Druck, Hitze und fremde Chemie, solange das Signal hält. Jede Sekunde dort unten ist hart erkämpfte Wissenschaft.",
 "un_venus_landing": "Setze eine unbemannte Sonde auf der Venusoberfläche ab. Die Hölle aus Hitze und Druck lässt Maschinen nur kurze, kostbare Datenfenster — nutze sie.",
 "cr_venus_flyby": "Schicke zwei Kerbals an Venus vorbei. Die Besatzung sieht die Nachbarwelt mit eigenen Augen, während dein Programm sie aus sicherer Distanz erforscht — bemannt bleibt Venus ein Ort des Vorbeiflugs.",
 "cr_venus_orbit": "Bringe zwei Kerbals in den Venusorbit und halte sie zehn Tage dort. Die Mission zeigt, dass dein Programm selbst die feindlichsten Welten mit Disziplin und Geduld erreicht.",
 # --- Merkur / Sonne ---
 "un_mercury_flyby": "Schicke eine Sonde an Merkur vorbei, tief hinab in die Sonnenglut. Dein Programm wagt sich an den heissesten, schnellsten Ort des inneren Systems.",
 "un_mercury_orbit": "Bringe eine Sonde in den Merkurorbit und kartiere die kleine, verbrannte Welt am Rand der Sonne — ein Balanceakt zwischen Hitze und Schwerkraft.",
 "un_mercury_landing": "Setze eine Sonde auf Merkur ab. Dein Programm behandelt die extreme kleine Welt als das, was sie ist: ein Ziel für zähe Maschinen, nicht für Menschen.",
 "un_sun_inner_probe": "Schicke eine Sonde auf eine enge Bahn um die Sonne und sammle Daten aus nächster Nähe am Zentrum, das alle Reisen bestimmt. Näher kommt deinem Stern so bald niemand.",
 # --- Mars unbemannt ---
 "un_mars_flyby": "Schicke eine Sonde an Mars vorbei. Der rote Planet verwandelt sich vom fernen Traum in das nächste grosse Arbeitsziel deines Programms.",
 "un_mars_orbit": "Bringe eine Sonde in den Marsorbit und kartiere die Landegebiete von oben. Jede Bahn ebnet den Weg für die Kerbals, die eines Tages dort unten stehen werden.",
 "un_mars_landing": "Lande eine Sonde auf dem Mars und berühre den roten Staub — lange bevor die erste Besatzung kommt. Ein robotischer Vorbote für das zweite grosse Kapitel.",
 "un_mars_precision_landing": "Setze eine Sonde punktgenau am vorbereiteten Marsgebiet ab. Diese Treffsicherheit gibt der ersten bemannten Landung ein klares, sicheres Ziel.",
 "cr_mars_flyby": "Schicke zwei Kerbals an Mars vorbei. Die Besatzung sieht den roten Planeten aus der Nähe — eine Generalprobe für den Tag, an dem dein Programm dort landet.",
 "cr_mars_orbit": "Bringe zwei Kerbals in den Marsorbit und halte sie zehn Tage über dem roten Planeten. Was bisher nur Sonden taten, tut nun eine Crew — die Landung ist zum Greifen nah.",
 # --- Marsmonde unbemannt ---
 "un_phobos_flyby": "Schicke eine Sonde dicht an Phobos vorbei. Der zerklüftete innere Marsmond wird zum ersten kleinen Ziel im Marsraum.",
 "un_phobos_orbit": "Bringe eine Sonde in eine enge Bahn um Phobos und prüfe den Mond als möglichen Stützpunkt für künftige Marsoperationen.",
 "un_deimos_flyby": "Schicke eine Sonde an Deimos vorbei, den äusseren der beiden Marsmonde. Er markiert den stillen Rand des frühen Marsraums.",
 "un_deimos_orbit": "Bringe eine Sonde in den Orbit um Deimos und sammle Daten über einen ruhigen Aussenposten hoch über Mars.",
 # --- Jupiter früh ---
 "un_jupiter_flyby": "Schicke eine Sonde an Jupiter vorbei, lange bevor Kerbals so weit reisen. Zum ersten Mal sieht dein Programm das äussere System aus der Nähe — und ein ganzes Reich aus Monden tut sich auf.",
 "un_jupiter_atmo_probe": "Lass eine Sonde in Jupiters oberste Wolkenschichten eintauchen. Die Messungen erzählen von einer Welt ohne festen Boden, nur Sturm und Tiefe.",
 "un_jupiter_orbit": "Bringe eine Sonde in den Jupiterorbit und öffne damit ein ganzes System aus Monden, Strahlung und gewaltigen Distanzen für dein Programm.",
 # --- Asteroiden früh ---
 "un_eros_flyby": "Schicke eine Sonde an Eros vorbei und eröffne den freiwilligen Asteroidenzweig deines Programms — ein erster Blick auf die kleinen Wanderer zwischen den Planeten.",
 "un_eros_orbit": "Bringe eine Sonde in eine enge Bahn um Eros. Dein Programm lernt, dass winzige Körper mit kaum Schwerkraft die grösste Präzision verlangen.",
 "un_vesta_flyby": "Schicke eine Sonde an Vesta vorbei. Der Asteroidengürtel zeigt deinem Programm seine erste richtig grosse Welt.",
 "un_ceres_flyby": "Schicke eine Sonde an Ceres vorbei, den grössten Körper des Gürtels. Schon dieser Vorbeiflug deutet an, wie lohnend die kleine Zwergplanetenwelt später wird.",
 # --- Mars bemannt Landung ---
 "cr_mars_landing": "Lande zwei Kerbals auf dem Mars und lass sie den roten Staub betreten. Nach Luna beginnt hier das zweite grosse Kapitel deines Programms — Menschen auf einer anderen Welt.",
 "cr_mars_precision_landing": "Setze eine bemannte Marslandung punktgenau am vorbereiteten Gebiet ab. Die gewonnene Treffsicherheit ist die Voraussetzung für eine dauerhafte Basis.",
 "cr_mars_stay_10d": "Halte zwei Kerbals zehn Tage auf dem Mars. Aus der ersten Landung wird eine echte Forschungsmission — der rote Planet wird zum Arbeitsplatz.",
 "cr_mars_stay_30d": "Halte zwei Kerbals dreissig Tage auf dem Mars. Damit wird der Planet vom Landeziel zum künftigen Aussenposten — und die Marsbasis denkbar.",
 # --- Phobos/Deimos bemannt ---
 "cr_phobos_orbit": "Bringe zwei Kerbals in den Orbit um Phobos. Der kleine Mond wird zum nahen bemannten Aussenposten im Marsraum.",
 "cr_phobos_landing": "Lande zwei Kerbals auf Phobos. Der Marsraum wird vom einzelnen Planeten zu einem System aus Zielen — Mond für Mond.",
 "cr_deimos_orbit": "Bringe zwei Kerbals in den Orbit um Deimos und erreiche damit den äusseren Rand des bemannten Marsraums.",
 "cr_deimos_landing": "Lande zwei Kerbals auf Deimos. Von hier draussen wirkt Mars wie der Mittelpunkt eines ganz neuen Arbeitsgebiets.",
 # --- Mars Versorgung ---
 "net_mars_orbit_supply": "Bring ein unbemanntes Versorgungsfahrzeug mit vollen Tanks in den Marsorbit. Dein Programm lernt, den roten Planeten regelmässig und verlässlich zu bedienen.",
 "net_phobos_cache": "Richte auf Phobos ein unbemanntes Treibstofflager ein. Der kleine Mond wird zum stillen Helfer für alle künftigen Marsoperationen.",
 "net_deimos_cache": "Richte auf Deimos ein unbemanntes Treibstofflager ein und sichere dir eine Reserve am äussersten Rand des Marsraums.",
 # --- Asteroiden Orbits/Landungen ---
 "un_vesta_orbit": "Bringe eine Sonde in den Orbit um Vesta und mach den Bonuszweig im Gürtel wissenschaftlich reicher.",
 "un_ceres_orbit": "Bringe eine Sonde in den Orbit um Ceres. Die grösste Welt des Gürtels bekommt damit echtes Gewicht in deinem Erkundungsprogramm.",
 "un_psyche_flyby": "Schicke eine Sonde an Psyche vorbei. Der ungewöhnlich metallische Körper weckt Vorstellungen von Bergbau und Industrie ferner Zukunft.",
 "un_ceres_landing": "Lande eine Sonde auf Ceres. Der Asteroidengürtel bekommt seinen wichtigsten robotischen Bodenkontakt — und Ceres rückt als bemanntes Ziel in den Blick.",
 "un_vesta_landing": "Setze eine Sonde auf Vesta ab und vergleiche zwei sehr verschiedene Gürtelwelten aus nächster Nähe.",
 "un_pallas_flyby": "Schicke eine Sonde an Pallas vorbei. Der Gürtel bleibt ein freiwilliges Feld für alle, die noch mehr entdecken wollen.",
 "un_pallas_orbit": "Bringe eine Sonde in den Orbit um Pallas und sammle Vergleichsdaten zu den anderen Riesen des Gürtels.",
 "un_psyche_orbit": "Bringe eine Sonde in den Orbit um Psyche. Der Metallkörper wird zum Sinnbild für Ressourcenforschung abseits des Hauptpfads.",
 "un_ryugu_flyby": "Schicke eine Sonde dicht an Ryugu vorbei. Der winzige, dunkle Asteroid fordert Präzision und belohnt sie mit feinen Daten.",
 "un_ryugu_landing": "Lande eine Sonde auf dem winzigen Ryugu. Ein leiser Triumph deiner Navigation — auf einem Körper, der kaum Schwerkraft hat.",
 "un_ida_flyby": "Schicke eine Sonde an Ida vorbei und füge dem Gürtel eine weitere unregelmässige Welt hinzu.",
 "un_dactyl_flyby": "Schicke eine Sonde am winzigen Dactyl vorbei, dem Begleiter von Ida. Selbst der kleinste Brocken bekommt seinen Platz in deiner Chronik.",
 # --- Ceres bemannt ---
 "cr_ceres_flyby": "Schicke zwei Kerbals an Ceres vorbei. Dieser Prestigeflug zeigt, dass dein Programm selbst kleine Gürtelwelten bemannt erreichen kann.",
 "cr_ceres_orbit": "Bringe zwei Kerbals in den Orbit um Ceres und halte sie zehn Tage. Die grösste Asteroidenwelt wird zur bemannten Nebenbühne deines Programms.",
 "cr_ceres_landing": "Lande zwei Kerbals auf Ceres. Ein freiwilliger Höhepunkt fernab des Hauptpfads — und ein Zeichen für die enorme Reichweite deines Programms.",
 "cr_ceres_stay_7d": "Halte zwei Kerbals sieben Tage auf Ceres. Dein Programm lernt, in winzigen Schwerefeldern über längere Zeit zu leben und zu arbeiten.",
 # --- Asteroiden Versorgung ---
 "net_ceres_ore_test": "Förder unbemannt erstes Erz auf Ceres. Der Bonuszweig bekommt damit eine glaubhafte Industrie-Erzählung — Rohstoffe aus dem Gürtel.",
 "net_ceres_supply_cache": "Lege auf Ceres einen Vorratspunkt mit Treibstoff an. Er stützt freiwillige Fernoperationen, ohne den Hauptpfad deines Programms aufzuhalten.",
 "net_psyche_ore_test": "Förder unbemannt Erz auf Psyche und prüfe, ob die kleinen Metallwelten praktischen Zukunftswert tragen.",
 "net_vesta_supply_cache": "Lege auf Vesta einen kleinen Treibstoff-Vorratspunkt an. Der Bonuszweig wird nützlicher, bleibt aber frei von jeder Pflicht.",
 # --- Jupitermonde unbemannt ---
 "un_io_flyby": "Schicke eine Sonde dicht an Io vorbei und sieh die wildeste, vulkanischste Welt des Jupitersystems aus der Nähe.",
 "un_europa_flyby": "Schicke eine Sonde an Europa vorbei. Unter der hellen Eiskruste wird ein Ozean vermutet — die Daten dieses Vorbeiflugs sind ein wissenschaftliches Versprechen.",
 "un_ganymede_flyby": "Schicke eine Sonde an Ganymed vorbei, den grössten Mond des Sonnensystems. Dein Programm erkundet hier den Ort, der später die einzige bemannte Landung im Jupiterraum tragen wird.",
 "un_callisto_flyby": "Schicke eine Sonde an Kallisto vorbei. Der ruhige, vernarbte Aussenmond wirkt wie ein natürlicher Platz für freiwillige Logistik.",
 "un_callisto_landing": "Lande eine Sonde auf Kallisto und gib dem optionalen Logistikzweig im Jupiterraum festen Boden unter den Füssen.",
 # --- Jupiter Versorgung ---
 "net_callisto_ore_test": "Förder unbemannt Erz auf Kallisto und teste dort eine Reserve für sehr tiefe Expeditionen ins äussere System.",
 "net_callisto_supply_cache": "Lege auf Kallisto einen Treibstoff-Vorratspunkt an. Er stützt freiwillige Fernpläne, während Ganymed der bemannte Hauptpfad bleibt.",
 # --- Jupiter bemannt ---
 "cr_jupiter_flyby": "Schicke drei Kerbals durch den Jupiterraum. Der erste bemannte Besuch bei einem Gasriesen macht dein Programm endgültig zur Fernraumorganisation.",
 "cr_jupiter_orbit": "Bringe drei Kerbals in den Jupiterorbit und halte sie zehn Tage in einer Region, die einst allein den Sonden gehörte.",
 "cr_ganymede_orbit": "Bringe drei Kerbals in den Orbit um Ganymed. Der grösste Mond wird zum sicheren Trittstein im gewaltigen Jupitersystem.",
 "cr_ganymede_landing": "Lande drei Kerbals auf Ganymed. Das Jupitersystem bekommt seinen einzigen bemannten Landungshöhepunkt — ein Fussabdruck unvorstellbar weit von zu Hause.",
 "cr_ganymede_stay_7d": "Halte drei Kerbals sieben Tage auf Ganymed. Diese Ausdauer macht Saturn als letztes bemanntes Fernziel überhaupt erst denkbar.",
 # --- Saturn unbemannt ---
 "un_saturn_flyby": "Schicke eine Sonde an Saturn vorbei. Mit diesem Flug rücken zugleich Uranus, Neptun, Pluto und das ferne Arrokoth als Schlussziele in Sicht.",
 "un_saturn_atmo_probe": "Lass eine Sonde in Saturns oberste Wolken eintauchen und sammle direkte Daten unter den berühmten Ringen.",
 "un_saturn_orbit": "Bringe eine Sonde in den Saturnorbit. Der Ringplanet wird zur letzten grossen Bühne, bevor Kerbals nach Titan aufbrechen.",
 "un_titan_flyby": "Schicke eine Sonde dicht an Titan vorbei und fange erste Signale aus seiner orangefarbenen Dunsthülle ein. Was darunter liegt, ahnt bisher niemand — doch die Daten entscheiden, ob der Mond einmal das letzte grosse Ziel einer bemannten Landung wird.",
 "un_titan_orbit": "Bringe eine Sonde in den Titanorbit und kartiere die Welt, auf der deine Kerbals eines Tages am weitesten draussen landen werden.",
 "un_titan_atmo_probe": "Tauche eine Sonde in Titans dichte obere Atmosphäre. Jede Messung macht die fremde Welt greifbarer und die spätere Landung sicherer.",
 "un_titan_landing": "Lande eine Sonde auf Titan. Der künftige letzte bemannte Landeort deines Programms bekommt seinen ersten sicheren Bodenkontakt.",
 "un_enceladus_flyby": "Schicke eine Sonde an Enceladus vorbei, den kleinen Eismond mit den Geysiren. Neben Titan wird er zum wichtigsten robotischen Ziel im Saturnsystem.",
 "un_enceladus_orbit": "Bringe eine Sonde in eine enge Bahn um Enceladus und untersuche die Eiswelt genauer als jeden anderen kleinen Saturnmond.",
 "un_enceladus_landing": "Lande eine Sonde auf Enceladus. Die kleine Eiswelt mit ihren Fontänen wird zum robotischen Höhepunkt neben Titan.",
 "un_rhea_flyby": "Schicke eine Sonde an Rhea vorbei und ergänze dein Saturnarchiv um einen kurzen, sauberen Besuch.",
 "un_iapetus_flyby": "Schicke eine Sonde an Iapetus vorbei, den zweifarbigen Mond. Seine rätselhafte Oberfläche liefert deinem Programm einen markanten Moment.",
 "un_dione_flyby": "Schicke eine Sonde an Dione vorbei — ein knapper, präziser Wissenschaftsbesuch im Saturnsystem.",
 "un_tethys_flyby": "Schicke eine Sonde an Tethys vorbei und sammle ein weiteres Puzzleteil des Ringplaneten-Systems.",
 "un_mimas_flyby": "Schicke eine Sonde am kleinen Mimas vorbei, dessen riesiger Krater ihn fast wie eine Kampfstation aussehen lässt.",
 "un_hyperion_flyby": "Schicke eine Sonde an Hyperion vorbei. Der unregelmässige, schwammartige Mond wirkt wie ein rätselhaftes Bruchstück.",
 "un_phoebe_flyby": "Schicke eine Sonde an Phoebe vorbei, den fernen, rückläufigen Aussenmond, der den Rand des Saturnsystems markiert.",
 # --- Saturn bemannt ---
 "cr_saturn_flyby": "Schicke drei Kerbals durch den Saturnraum. Die Ringe markieren den letzten grossen bemannten Aufbruch deines Programms.",
 "cr_saturn_orbit": "Bringe drei Kerbals in den Saturnorbit und halte sie zehn Tage. Die Besatzung steht am Rand des letzten begehbaren Kapitels.",
 "cr_titan_orbit": "Bringe drei Kerbals in den Orbit um Titan. Unter ihnen liegt die letzte Welt, die dein Programm bemannt betreten wird.",
 "cr_titan_landing": "Lande drei Kerbals auf Titan. Diese dichte, fremde Welt wird zur letzten grossen bemannten Landung deiner ganzen Kampagne.",
 "cr_titan_stay_7d": "Halte drei Kerbals sieben Tage auf Titan. Danach übernehmen wieder Sonden den Weg hinaus in die fernsten Regionen.",
 # --- Saturn/Titan Versorgung ---
 "net_titan_supply_test": "Lande ein unbemanntes Versorgungsfahrzeug auf Titan und leg dem fernsten bemannten Zielraum eine logistische Reserve hin.",
 "net_saturn_transfer_cache": "Bring ein unbemanntes Versorgungsfahrzeug mit vollen Tanks in den Saturnorbit und stütze die Rückkehrplanung im letzten bemannten Fernraum.",
 # --- Uranus & Monde ---
 "un_uranus_flyby": "Schicke eine Sonde an Uranus vorbei. Nach Saturn füllt sich die letzte Karte deines Programms nun in mehrere Richtungen zugleich.",
 "un_uranus_atmo_probe": "Tauche eine Sonde in den gekippten Eisriesen Uranus. Direkte Messungen aus einer Distanz, die einst unvorstellbar war.",
 "un_titania_flyby": "Schicke eine Sonde an Titania vorbei, den grössten Uranusmond — ein kurzes, wertvolles Fenster in eine sehr ferne Region.",
 "un_oberon_flyby": "Schicke eine Sonde an Oberon vorbei und ergänze das Schlussarchiv deines Programms um den äussersten grossen Uranusmond.",
 "un_ariel_flyby": "Schicke eine Sonde an Ariel vorbei. Die letzte Epoche sammelt ferne Momentaufnahmen von hohem wissenschaftlichem Wert.",
 "un_umbriel_flyby": "Schicke eine Sonde an den dunklen Umbriel vorbei. Auch die finstersten fernen Monde bekommen ihren Platz in deiner Chronik.",
 "un_miranda_flyby": "Schicke eine Sonde an Miranda vorbei, deren zerklüftete Oberfläche den langen Flug mit lauter neuen Rätseln belohnt.",
 "un_puck_flyby": "Schicke eine Sonde am kleinen Puck vorbei — ein kurzer Schlussauftritt für einen unscheinbaren Mond.",
 # --- Neptun & Monde ---
 "un_neptune_flyby": "Schicke eine Sonde an Neptun vorbei. Der tiefblaue Eisriese öffnet das grosse Finale rund um Triton.",
 "un_neptune_atmo_probe": "Tauche eine Sonde in Neptuns obere Atmosphäre — Messungen vom äussersten Rand der klassischen Planetenwelt.",
 "un_triton_flyby": "Schicke eine Sonde an Triton vorbei, den grossen rückläufigen Mond. Er wird zum eigentlichen Ziel im Neptunsystem.",
 "un_triton_orbit": "Bringe eine Sonde in den Orbit um Triton. Die Schluss-Epoche bekommt eines ihrer wichtigsten wissenschaftlichen Zentren.",
 "un_triton_landing": "Lande eine Sonde auf Triton. Das Neptunsystem erhält sein grosses robotisches Landungsfinale, fast unfassbar weit von der Erde.",
 "un_nereid_flyby": "Schicke eine Sonde an Nereid vorbei, den fernen, exzentrischen Mond — ein kurzer, kostbarer Besuch.",
 "un_proteus_flyby": "Schicke eine Sonde an Proteus vorbei. Die letzte Karte füllt sich Stück für Stück mit kleinen, fernen Welten.",
 # --- Pluto / Charon / Arrokoth ---
 "un_pluto_flyby": "Schicke eine Sonde an Pluto vorbei. Am Rand der bekannten Karte wartet eine Welt, die lange nur ein Lichtpunkt war.",
 "un_pluto_orbit": "Bringe eine Sonde in den Orbit um Pluto und mach aus dem flüchtigen Vorbeiflug eine letzte grosse Untersuchung.",
 "un_pluto_landing": "Lande eine Sonde auf Pluto. Die ferne, eisige Oberfläche wird zum letzten grossen Landepunkt deiner robotischen Kampagne.",
 "un_charon_flyby": "Schicke eine Sonde an Charon vorbei, Plutos grossen Begleiter. Das Doppelsystem bekommt seinen Platz im Abschlussarchiv.",
 "un_arrokoth_flyby": "Schicke eine Sonde an Arrokoth vorbei, den fernsten je besuchten Brocken. Dein Programm erreicht das symbolische Ende der bekannten Karte.",
 # --- Titanbasis-Finale ---
 "cr_titan_base4": "Errichte auf Titan den fernsten bemannten Aussenposten und halte ihn mit vier Kerbals dreissig Tage am Leben. Am äussersten Rand des begehbaren Raums entsteht ein neues Zuhause.",
 "cr_titan_base6": "Erweitere die Titanbasis auf sechs Kerbals und halte sie sechzig Tage stabil. Kein neuer Aufbruch, sondern Wachstum am äussersten bemannten Rand.",
 "cr_titan_base8": "Bring die Titanbasis auf acht Kerbals und halte sie hundertfünfzig Tage am Leben. Damit endet der bemannte Teil deiner Kampagne — während die fernsten Sonden weiter berichten.",
}

def main():
    lines = open(DOC, encoding="utf-8").read().splitlines(keepends=True)
    cur, applied, md_ids = None, 0, set()
    for i, ln in enumerate(lines):
        mid = re.match(r"\s*id:\s*(\S+)", ln)
        if mid:
            cur = mid.group(1); md_ids.add(cur); continue
        if ln.strip().startswith("beschreibung:") and cur:
            if cur in DESC:
                indent = ln[:len(ln) - len(ln.lstrip())]
                lines[i] = f"{indent}beschreibung: {DESC[cur]}\n"
                applied += 1
    open(DOC, "w", encoding="utf-8").write("".join(lines))

    missing = sorted(md_ids - set(DESC))
    extra = sorted(set(DESC) - md_ids)
    print(f"Beschreibungen ersetzt: {applied} / {len(md_ids)} Missionen")
    print(f"[{'FAIL' if missing else 'OK'}] Ohne neuen Text: {missing or '—'}")
    print(f"[{'WARN' if extra else 'OK'}] DESC-IDs ohne Mission: {extra or '—'}")

if __name__ == "__main__":
    main()
