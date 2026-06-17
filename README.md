# CustomScienceContracts

CustomScienceContracts ist ein KSP-1.12.x-Plugin fuer den Science Mode. Es fuegt
ein eigenes Missionssystem hinzu, das parallel zum Stock-Contractsystem laeuft.
Die Missionen fuehren durch eine SOL-Quarter-Scale-Kampagne von den ersten
unbemannten Erdtests bis zu Luna, Mars, Asteroiden, Jupiter, Saturn, Titan und
fernen robotischen Zielen.

Der Mod prueft nur Stock-KSP-Zustaende und vergibt beim Abschluss Science-Boni.
Er braucht keinen Contract Configurator und keine Kerbalism-/Simplex-API.

## Installation

1. Lade das Release-Zip herunter.
2. Entpacke es.
3. Kopiere den enthaltenen `GameData`-Ordner in deine KSP-Installation.
4. Starte KSP im Science Mode.

Die Zielstruktur sieht danach so aus:

```text
Kerbal Space Program/
└── GameData/
    └── CustomScienceContracts/
        ├── Contracts/
        ├── Icons/
        ├── Plugins/
        │   └── CustomScienceContracts.dll
        └── settings.cfg
```

## Abhaengigkeiten

Zum Spielen:

- Kerbal Space Program 1.12.x.
- Eine SOL-Quarter-Scale-Installation mit passenden internen Body-Namen
  (`Earth`, `Moon`, `Mars`, `Jupiter`, `Saturn`, usw.).

Nicht benoetigt:

- Contract Configurator.
- Kerbalism.
- Simplex.
- Part-Packs fuer Missionsbedingungen.

Der Mod stellt keine Part-Anforderungen. Antennen, Scanner und wissenschaftliche
Nutzlasten sind im Text teilweise erzählerisch gemeint; technisch geprueft
werden nur KSP-Zustaende wie Orbit, Landung, Crew, Vessel-Anzahl, Ressourcen,
Inklination, Flyby und Zeit.

## Was der Mod macht

- Fuegt eigene Missionslisten mit vier Sparten hinzu:
  - Pioniere
  - Robotische Erkunder
  - Versorgungsnetz
  - Wiederholbar
- Laesst Missionen aktiv annehmen.
- Prueft aktive Missionen etwa einmal pro Sekunde.
- Zeigt Teilziele und Fortschritt in einer eigenen UI.
- Speichert Fortschritt im aktuellen Save.
- Vergibt Science-Boni beim Einloesen.
- Verschiebt wiederholbare Missionen nach Erstabschluss in die Sparte
  `Wiederholbar`.

## Wichtige Missionsketten

- Fruehe unbemannte Erdtests und erster Erdorbit.
- Erstes 3-Satelliten-Netz im Erdorbit.
- Bemannte Erdfluege, EVA, Docking und temporäres Ein-Modul-Labor.
- Luna-Flyby, Luna-Orbit, Luna-Landung und Mondinfrastruktur.
- Neue Kommunikations-Lebensader ab Epoche 3:
  - Erde
  - polare Erd-Relais
  - Luna
  - polare Luna-Relais
  - Mars
  - interplanetarer Sonnenorbit-Ring
  - Jupiter
  - Saturn
- Optionale Polar-Kartierungen nach erfolgreichen normalen Orbits.
- Mars-Hauptbogen mit Landung, Station und Basis.
- Asteroiden-Bonuszweige.
- Jupiter-, Saturn-, Titan- und Schluss-Epochen-Ziele.

## Spielhinweise

- Hoehenangaben sind Mindestwerte. Wenn eine Mission `Periapsis ueber 2000 km`
  verlangt, muss die Periapsis wirklich groesser als 2000 km sein.
- Polarorbit-Missionen verlangen mindestens 75 Grad Inklination.
- Kommunikationsnetze zaehlen echte Vessels im Orbit. Debris, Flags,
  Asteroiden und Deployed-Science-Objekte zaehlen nicht.
- Wiederholbare Missionen haben einen Cooldown: nach dem Einloesen muessen erst
  zwei andere Missionen abgeschlossen werden.

## Fortschritt und Save-Dateien

Der Mod speichert pro Spielstand:

```text
saves/<SaveName>/CustomScienceContracts/contracts_state.cfg
```

Darin stehen aktive Missionen, abgeschlossene Missionen, Timer, Flyby-State,
Waypoint-State, Repeatable-Cooldowns und registrierte Stationen/Basen.

## Entwicklung

Die Missionsquelle ist:

```text
custom_science_contracts_missionsdesign.md
```

Die Contract-Dateien in `GameData/CustomScienceContracts/Contracts/*.cfg` werden
generiert. Nicht von Hand editieren.

Typischer Entwicklungsablauf:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
./build.sh
```

Zum Bauen werden die KSP-Managed-Assemblies aus einer lokalen KSP-Installation
gebraucht. Details stehen in `DOKUMENTATION.md`.

## Lizenz

Noch nicht festgelegt.
