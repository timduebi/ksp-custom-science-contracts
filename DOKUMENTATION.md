# CustomScienceContracts - Projektdokumentation

Stand: Juni 2026.

CustomScienceContracts ist ein KSP-1.12.x-Plugin fuer den Science Mode. Es
stellt ein eigenes Missions- und Zielsystem bereit, das parallel zum
Stock-Contractsystem laeuft. Der Mod ist fuer eine SOL-Quarter-Scale-Kampagne
geschrieben und nutzt die internen Body-Namen dieses Systems, zum Beispiel
`Earth`, `Moon`, `Mars`, `Jupiter`, `Saturn`, `Triton` und `Pluto`.

Der Mod ersetzt keine Stock-Science. Er gibt beim Abschluss definierter
Missionen zusaetzliche Science-Punkte aus und fuehrt den Spieler dadurch durch
eine lange, erzählerische Erkundungskampagne.

## Kurzfassung

- Spielmodus: KSP 1 Science Mode.
- KSP-Zielversion: 1.12.x.
- Sprache der Missionen: Deutsch, Schweizer Hochdeutsch.
- Laufzeit-Abhaengigkeiten: KSP 1.12.x und die SOL-Quarter-Scale-Umgebung, fuer
  die die Body-Namen geschrieben sind.
- Keine Abhaengigkeit von Contract Configurator.
- Keine Abhaengigkeit von Kerbalism, Simplex oder anderen Gameplay-APIs.
- Pruefung erfolgt nur ueber Stock-KSP-Zustaende: Orbit, Landung, Crew,
  Ressourcen, Treibstoff, EVA, Andocken, Flybys, Waypoints, Vessel-Anzahl,
  Inklination und Zeit.

## Ordnerstruktur

```text
ksp science contracts/
├── README.md                         Spieler- und Release-Information
├── AGENTS.md                         Arbeitsregeln fuer Codex
├── DOKUMENTATION.md                  diese technische Dokumentation
├── CustomScienceContracts.sln        C# Solution
├── build.sh                          lokaler Build und KSP-Deploy
├── custom_science_contracts_missionsdesign.md
│                                      Single Source of Truth fuer Missionen
├── customScienceContracts Logo.png   Logo/Artwork
├── GameData/CustomScienceContracts/
│   ├── Contracts/                    generierter Missionskatalog
│   ├── Icons/                        App-, Body- und UI-Icons
│   ├── Plugins/                      Build-Ausgabe, nicht versioniert
│   └── settings.cfg                  UI-/Balance-/Debug-Settings
├── src/CustomScienceContracts/       C#-Quellcode
└── tools/                            Generator und Validatoren
```

Nicht zum Quellbestand gehoeren alte Exportordner, alte v3-Spezifikationen,
alte SOL-Config-Kopien, `.DS_Store`, `bin/`, `obj/` und fertige DLLs. Diese
Dinge sind entweder veraltet, lokal oder regenerierbar.

## Release-Inhalt

Ein Spieler braucht nur den fertigen Release-Inhalt:

```text
GameData/
└── CustomScienceContracts/
    ├── Contracts/
    ├── Icons/
    ├── Plugins/
    │   └── CustomScienceContracts.dll
    └── settings.cfg
README.md
```

Installation: den enthaltenen `GameData`-Ordner in die KSP-Installation
kopieren und vorhandene Dateien zusammenfuehren.

## Abhaengigkeiten

### Zum Spielen

- Kerbal Space Program 1.12.x.
- Eine SOL-Quarter-Scale-Installation mit den Body-Namen, die der Mod nutzt.
- Keine weiteren Plugin-Abhaengigkeiten.

Der Mod verwendet ausschliesslich KSP- und Unity-Klassen, die mit KSP 1.12.x
geliefert werden. Es gibt keine Part-Anforderungen und keine API-Kopplung an
Kerbalism, Simplex, Contract Configurator oder andere Mods.

### Zum Bauen

- .NET SDK, das `net48`-Projekte bauen kann.
- NuGet-Paket `Microsoft.NETFramework.ReferenceAssemblies` fuer net48 auf
  macOS/Linux.
- KSP-Managed-Assemblies aus der lokalen KSP-Installation:
  - `Assembly-CSharp.dll`
  - `UnityEngine.dll`
  - `UnityEngine.CoreModule.dll`
  - `UnityEngine.IMGUIModule.dll`
  - `UnityEngine.ImageConversionModule.dll`
  - `UnityEngine.PhysicsModule.dll`
  - `UnityEngine.AnimationModule.dll`
  - `UnityEngine.TextRenderingModule.dll`

Die Pfade werden ueber `KSPManaged` oder `KSPRoot` an MSBuild uebergeben. Das
lokale `build.sh` nutzt standardmaessig die Steam-KSP-Installation auf macOS.

## Missionsdesign

Die Missionsquelle ist:

```text
custom_science_contracts_missionsdesign.md
```

Diese Datei ist die Single Source of Truth. Die Dateien unter
`GameData/CustomScienceContracts/Contracts/*.cfg` werden daraus generiert und
werden nicht von Hand editiert.

Der Missionsplan enthaelt aktuell:

- 156 handgeschriebene Missionen.
- 89 generierte Stations-, Basis- und Depot-Auftraege.
- Insgesamt 245 spielbare Contracts im generierten Katalog.

Die Missionen sind in vier spielerische Sparten organisiert:

- `Pioniere`: bemannte Hauptmissionen.
- `Robotische Erkunder`: unbemannte Flybys, Orbits, Landungen, Atmosphaeren- und
  Kartierungsmissionen.
- `Versorgungsnetz`: Infrastruktur, Kommunikationsnetze, Treibstoff, Ore und
  Logistik.
- `Wiederholbar`: abgeschlossene wiederholbare Auftraege wandern nach ihrem
  Erstabschluss hierhin.

## Kampagnenbogen

Der Ablauf beginnt mit unbemannten Erdtests, fuehrt ueber erste
Erdorbit-Missionen und ein fruehes 3-Satelliten-Netz zu bemannten Fluegen,
Luna, Stationen, Mars, Asteroiden, Jupiter, Saturn, Titan und den fernen
robotischen Schlusszielen.

Wichtige neuere Infrastruktur:

- Das fruehe Erdnetz besteht aus 3 Satelliten.
- Ab Epoche 3 entsteht ein neues Kommunikationsnetz in `Versorgungsnetz`.
- Kommunikationskette:
  - Erde: 3 Relais ueber 2000 km Periapsis.
  - Erde polar: 6 Relais total, davon 3 mit Inklination ueber 75 Grad.
  - Luna: 3 Relais ueber 2000 km Periapsis.
  - Luna polar: 6 Relais total, davon 3 mit Inklination ueber 75 Grad.
  - Mars: 3 Relais ueber 2000 km Periapsis.
  - Interplanetarer Sonnenorbit-Ring.
  - Jupiter-Kommunikationsnetz.
  - Saturn-Kommunikationsnetz.
- Optionale Polar-Kartierungsmissionen werden erst nach dem jeweiligen normalen
  Orbit sichtbar und sind keine Voraussetzung fuer andere Missionen.

## Datenfluss

```text
custom_science_contracts_missionsdesign.md
        │
        ├─ tools/validate_design.py
        │
        └─ tools/gen_catalog.py
                │
                ▼
GameData/CustomScienceContracts/Contracts/*.cfg
        │
        ▼
CatalogLoader
        │
        ▼
ContractCatalog
        │
        ▼
ContractManager
        │
        ├─ UI
        ├─ CheckEvaluation
        ├─ StationRegistry
        └─ SaveFolderStore
```

## Generierter Katalog

`tools/gen_catalog.py` erzeugt vier Dateien:

- `A_Pioniere.cfg`: bemannte Missionen.
- `B_Spaeher.cfg`: robotische Erkundung.
- `C_Lebensadern.cfg`: Netzwerk, Kommunikation und Logistik.
- `D_Stationen.cfg`: generierte Stations-, Basis- und Depotketten.

Der Generator setzt ausserdem Titel, Unterkategorien, Icons und `revealAllAfter`
fuer das aeussere System. `Sun` wird in der Unterkategorie `Interplanetar`
angezeigt.

## Runtime-Architektur

### Einstieg

`ContractsScenario` ist ein KSP-`ScenarioModule`. Es wird in relevanten Szenen
geladen, initialisiert Katalog, Manager, Settings, Events und UI und startet
einen Coroutine-Pruefloop.

Der Pruefloop laeuft nicht jeden Frame, sondern ungefaehr einmal pro Sekunde.
Das schont die Performance und reicht fuer Missionsziele, die sowieso ueber
Spielzustaende, Zeiten und Events laufen.

### Katalog

`CatalogLoader` liest alle `CUSTOM_CONTRACT_CATALOG`-Nodes aus der KSP
`GameDatabase`. Jede Mission wird als `MissionContract` geladen. Die
`CONDITION`-Nodes werden in `Condition` und, bei zusammengesetzten Zielen, in
einzelne `Check`-Objekte zerlegt.

### Manager

`ContractManager` verwaltet:

- Statuswechsel.
- Annahme und Abbruch.
- aktive Limits pro Sparte.
- Abschluss und Einloesen.
- Science-Auszahlung.
- Repeatable-Cooldown.
- Stationsregistrierung fuer spaetere Versorgung.

Beim Einloesen zahlt der Mod Science ueber:

```csharp
ResearchAndDevelopment.Instance.AddScience(amount, TransactionReasons.Cheating)
```

Das ist technisch der stabile Stock-Weg, um Science im Science Mode direkt zu
veraendern.

### Persistenz

Fortschritt wird pro Save gespeichert:

```text
saves/<SaveName>/CustomScienceContracts/contracts_state.cfg
```

Gespeichert werden unter anderem:

- abgeschlossene Missionen.
- aktive Missionen.
- `ReadyToClaim`-Status.
- Repeatable-Cooldown-Zaehler.
- Timer-Fortschritt.
- Flyby-State.
- Waypoint-/Marker-State.
- registrierte Stationen, Basen und Depots.

### UI

Die UI ist eine IMGUI-Oberflaeche mit ApplicationLauncher-Buttons:

- Auswahlfenster fuer verfuegbare Missionen.
- Aktive-Missionen-Fenster mit Fortschritt.
- Settings-Fenster.

Die Missionen werden nach Sparte und Unterkategorie gruppiert. Body-Icons,
Mission-Icons und Farben kommen aus `BodyVisual` und `IconLibrary`.

Icons werden direkt aus dem Modordner geladen. `GameDatabase` ist nur ein
Fallback, damit veraltete gecachte Icons im Spiel seltener ein Problem sind.

## Contract-Status

Eine Mission bewegt sich durch diese Zustaende:

- `Locked`: Voraussetzungen fehlen.
- `Available`: Voraussetzungen sind erfuellt, aber die Mission ist noch nicht
  angenommen.
- `Active`: Mission wurde angenommen und wird geprueft.
- `ReadyToClaim`: alle Bedingungen sind erfuellt, Reward wurde aber noch nicht
  ausgezahlt.
- `CompletedOnce`: Mission wurde mindestens einmal abgeschlossen.

Nicht wiederholbare Missionen sind danach fertig. Wiederholbare Missionen
wandern nach dem Erstabschluss in `Wiederholbar`.

## Sichtbarkeit

Die Sichtbarkeit wird in `VisibilityRules` begrenzt:

- Bemannte Missionen: anfangs 3 sichtbare verfuegbare Missionen, spaeter 5.
- Robotische Erkundung: 4 sichtbare Missionen pro Unterkategorie.
- Aeusseres System: nach dem Planeten-Flyby koennen alle Missionen der
  Unterkategorie sichtbar werden.
- Netzwerk/Logistik: 3 sichtbare Missionen pro Unterkategorie.

Aktive Limits stehen in `Tuning`:

- Bemannt: 3 aktive Missionen.
- Robotische Erkundung: 10 aktive Missionen.
- Netzwerk/Logistik: 5 aktive Missionen.

## Repeatables

Wiederholbare Missionen sind nach dem Erstabschluss nicht mehr in ihrer
Heimatliste, sondern in `Wiederholbar`.

Cooldown-Regel:

- Nach Einloesen wird der Zaehler auf 0 gesetzt.
- Jede andere abgeschlossene Mission erhoeht ihn um 1.
- Wieder annehmbar ab 2 anderen Abschluessen.

## Check-System

Die meisten Missionen nutzen eine `COMPOSITE`-Condition mit mehreren `CHECK`
Teilzielen. Dadurch kann die UI jede Teilbedingung einzeln anzeigen.

Wichtige Check-Typen:

- `CREW_NONE`, `CREW_MIN`, `CREW_EXACT`: Besatzung.
- `ORBIT_ABOVE Body [km]`: aktives Vessel in stabilem Orbit. Mit `km` muss die
  Periapsis strikt groesser als dieser Wert sein. Ohne `km` wird die
  Atmosphaerenhoehe des Bodies aus der KSP-API genutzt.
- `INCLINATION_MIN Body degrees`: Orbit-Inklination mindestens Wert.
- `VESSEL_COUNT Body count [km]`: Anzahl echter Vessels im Orbit, optional mit
  Periapsis strikt groesser als `km`.
- `VESSEL_COUNT_INCLINATION Body count degrees [km]`: wie `VESSEL_COUNT`, aber
  mit Mindest-Inklination.
- `LANDED Body`: gelandet oder gesplasht.
- `ATMO_FRACTION Body min max`: Hoehe als Anteil der Atmosphaerenhoehe.
- `FLYBY Body km`: SOI-Durchflug ohne Orbit/Landung, optional mit maximaler
  Annaeherung.
- `MARKER_LANDING Body km`: Landung bei einem gesetzten Zielpunkt.
- `EVA Body Situation`: Kerbal im EVA-Zustand.
- `DOCK_ANY`, `DOCK_STATION`: Andocken.
- `FUEL_MIN`, `RESOURCE_MIN`, `ORE_SURFACE`: Ressourcen und Logistik.
- `HOLD seconds`: Zustand fuer Sekunden halten.
- `DURATION days`: Zustand fuer KSP-Tage halten.

Wichtig: Hoehenchecks sind als "ueber der Zahl" implementiert, nicht als
"genau diese Zahl". Bei `ORBIT_ABOVE Mars 250` gilt also `PeA > 250 km`.

## Stationen, Basen und Depots

Stationsketten werden aus dem Abschnitt `STATIONSKETTEN` im Missionsdesign
generiert. Beim Bauauftrag merkt sich der Mod das Vessel als konkrete Station
oder Basis. Spaetere Versorgung und Andockmissionen koennen dann gezielt gegen
dieses Vessel pruefen.

Beispiele:

- Erdstation.
- Mondstation.
- Mondbasis.
- Marsstation.
- Marsbasis.
- Erdorbit-Treibstoffdepot.

## Build-Workflow

Normaler Arbeitsablauf:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
./build.sh
```

Direkter Build ohne KSP-Kopie:

```bash
dotnet build -c Release \
  -p:KSPManaged="/path/to/KSP/KSP.app/Contents/Resources/Data/Managed" \
  CustomScienceContracts.sln
```

Nach dem Build liegt die DLL hier:

```text
GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll
```

`build.sh` kopiert danach den ganzen Modordner in die lokale KSP-Installation.

## Release-Workflow

1. Design pruefen.
2. Katalog generieren.
3. Katalog pruefen.
4. Release-Build erstellen.
5. Paket bauen mit:

```text
README.md
GameData/CustomScienceContracts/
```

6. Git-Commit und GitHub-Tag setzen.
7. Zip als GitHub-Release-Asset hochladen.

## Entwicklungsregeln

- Missionen immer zuerst im Missionsdesign aendern.
- Generierte `.cfg`-Dateien nicht von Hand pflegen.
- Keine erfundenen Body-Namen.
- Keine hardcodeten Body-Radien oder Atmosphaerenhoehen.
- Keine Part-Anforderungen.
- Keine Kopplung an Kerbalism, Simplex oder andere Mod-APIs.
- UI darf die aktuelle dunkle Optik behalten.
- Contract-Titel und -Beschreibungen bleiben deutsch.

## Pruefstand

Letzter bekannter Pruefstand:

```text
python3 tools/validate_design.py  -> OK
python3 tools/validate_catalog.py -> OK
dotnet build -c Release           -> OK
```

Beim Build kann eine NuGet-Warnung zu Sicherheitsdaten auftreten, wenn kein
Netzwerkzugriff auf `https://api.nuget.org/v3/index.json` besteht. Diese Warnung
betrifft nicht den Mod-Code.
