# CustomScienceContracts — Dokumentation

Architektur, Aufbau und Ziel des Mods. Stand: Juni 2026.

---

## 1. Ziel des Mods

**CustomScienceContracts** ist ein KSP-1.12-Plugin (C#), das ein **eigenes Missions-/Ziel-System**
bereitstellt — parallel und unabhängig vom Stock-Contract-System. Es läuft im **Science-Mode**
(Science Sandbox) und ist auf das Planetenpack **SOL (Real Solar System Recreation, Quarter-Scale)**
zugeschnitten.

- Der Spieler bekommt eine kuratierte, erzählerische Missionskampagne (von den ersten Testflügen
  bis zur Titanbasis), die den Fortschritt strukturiert.
- Erfüllte Missionen geben einen **Wissenschafts-Bonus** via
  `ResearchAndDevelopment.AddScience(...)`. Das System ist ein **Bonus** zum normalen Science-Mode,
  kein Ersatz — Experimente und eigene Erkundung bleiben die Hauptquelle.
- Gemessen wird ausschliesslich über **Stock-Spielzustände** (Orbit, Landung, Crew, Treibstoff,
  Andocken, …). **Keine** Abhängigkeit zu Part-Mods oder Lebenserhaltungs-APIs; thematische Inhalte
  bleiben rein narrativ in den Beschreibungstexten.

### Harte Regeln (aus CLAUDE.md)
- Keine Part-Anforderungen, keine Kerbalism/Simplex-Kopplung.
- Keine hardcodeten Body-Grössen/Atmosphärenhöhen — immer aus der KSP-API.
- Keine erfundenen Body-Namen (alle gegen die SOL-Configs verifiziert).
- Texte auf Schweizer Hochdeutsch ("ss" statt "ß"); Code/Kommentare Englisch ok.

---

## 2. Verzeichnisstruktur

```
ksp science contracts/
├── CLAUDE.md                                  Verbindliche Projektregeln
├── custom_science_contracts_missionsdesign.md SINGLE SOURCE OF TRUTH der Missionen
├── DOKUMENTATION.md                           (dieses Dokument)
├── build.sh                                    Build + Deploy ins KSP-GameData
├── CustomScienceContracts.sln
├── src/CustomScienceContracts/                 C#-Quellcode (net48)
├── GameData/CustomScienceContracts/            Auslieferbares Mod-Verzeichnis
│   ├── Plugins/CustomScienceContracts.dll      (Build-Output)
│   ├── Contracts/{A,B,C,D}*.cfg                GENERIERTER Katalog
│   ├── Icons/{UI,Bodies,App}/                  Texturen
│   └── settings.cfg                            Tuning-Werte
├── Sol-Configs/                                Referenz: SOL-Planetenpack (Body-Namen)
├── tools/                                       Generatoren + Validatoren (Python)
├── fertig/                                      Auslieferung: kompletter Release-Mod
└── für mich/                                    Auslieferung: Mod + vorprogressierter Spielstand
```

---

## 3. Gesamtarchitektur

Zwei getrennte Welten, verbunden über die `.cfg`-Dateien:

```
  AUTOREN-PIPELINE (Build-Zeit, Python)            LAUFZEIT (im Spiel, C#)
  ───────────────────────────────────             ────────────────────────
  missionsdesign.md ──gen_catalog.py──▶ Contracts/*.cfg ──CatalogLoader──▶ ContractCatalog
        ▲                                    │                                   │
   rewrite_desc.py                      validate_*.py                       ContractManager
   (Texte)                                                          (Status-Flow, Tick, Reward)
                                                                            │
                                                            EvaluationContext + Evaluatoren
                                                                            │
                                                       SaveFolderStore ◀──▶ contracts_state.cfg
                                                                            │
                                                                  CscUI / Fenster (IMGUI)
```

### 3.1 Autoren-Pipeline (Build-Zeit)
Der **gesamte Katalog wird generiert** — die `.cfg`-Dateien werden NIE von Hand editiert.

- **`missionsdesign.md`** ist die Quelle: 140 Missionen im `=== MISSION ===`-Blockformat plus ein
  `STATIONSKETTEN`-Abschnitt (`chain:`-Zeilen).
- **`tools/gen_catalog.py`** parst die Datei und schreibt vier `.cfg`-Dateien:
  - `A_Pioniere.cfg` — Sparte *Pioniere* (bemannt) → enum `Bemannt`
  - `B_Spaeher.cfg` — *Robotische Erkunder* → enum `UnbemannteErkundung`
  - `C_Lebensadern.cfg` — *Versorgungsnetz* → enum `NetzwerkLogistik`
  - `D_Stationen.cfg` — generierte Stations-/Basis-/Depot-Ketten
  Dabei werden Titel regelbasiert gebildet, das Icon je Mission nach dominantem Check gewählt,
  die Unterkategorie aus dem Body abgeleitet (Monde nisten unter ihrem Planeten) und jeder
  `check:` in einen `CHECK`-Knoten übersetzt.
- **`tools/rewrite_desc.py`** ersetzt die `beschreibung:`-Zeilen im Plan durch die kuratierten
  Story-Texte (imperativ + kleine Story + Vorausblick + konkrete Specs).
- **`tools/validate_design.py`** / **`tools/validate_catalog.py`** prüfen Plan bzw. erzeugte
  cfgs auf doppelte IDs, hängende Voraussetzungen, unbekannte Check-Typen/Bodies/Sparten.
- **`tools/make_state.py`** erzeugt einen vorprogressten `contracts_state.cfg` (Voraussetzungs-
  Closure gewählter Meilensteine → für die "für mich"-Auslieferung).
- *(`tools/gen_stations.py` ist überholt — die Ketten sind in `gen_catalog.py` integriert.)*

### 3.2 Laufzeit (im Spiel)
`ContractsScenario` (ein `ScenarioModule`) ist der Lifecycle-Host: lädt Katalog + Save-State,
registriert die Evaluatoren, hostet den 1-Sekunden-Prüfloop und besitzt die UI.

---

## 4. Datenmodell (`src/.../Model/`)

| Typ | Zweck |
|-----|-------|
| `MissionContract` | Eine Mission: Definition (Id, Titel, Beschreibung, Sparte, Unterkategorie, Icon, Voraussetzungen, Reward, Repeatable, recordStationKey/stationRef) **plus** Laufzeit-State (`Status`, `TotalCompletions`, `CompletionsSinceLastClaim`, `Progress`-ConfigNode). |
| `Sparte` | `Bemannt`, `UnbemannteErkundung`, `NetzwerkLogistik`, `Wiederholbar`. |
| `MissionStatus` | `Locked → Available → Active → ReadyToClaim → CompletedOnce`. |
| `Condition` / `ConditionType` | Eine Prüfbedingung. **Heute** ist jede Mission `type = COMPOSITE` mit `CHECK`-Subknoten. Die Legacy-Einzeltypen (FLYBY, LANDED, …) existieren weiter, werden vom Katalog aber nicht mehr genutzt. |
| `Check` / `CheckKind` | Atomares, einzeln (grün/rot) bewertetes Teilziel mit eigenem Label. |

### 4.1 Check-Typen (`CheckKind`)
Crew: `CREW_MIN`, `CREW_NONE`, `CREW_EXACT` · Ort/Bahn: `ON_BODY`, `SITUATION`, `LANDED`,
`ORBIT_ABOVE` (km=0 ⇒ API-Atmosphärenhöhe), `PERIAPSIS_MIN`, `ABOVE_ATMOSPHERE`, `SUBORBITAL` ·
Atmosphäre: `ATMO_FRACTION` (Bruchteil der `atmosphereDepth`) · Ressourcen: `FUEL_MIN`,
`RESOURCE_MIN`, `ORE_PRESENT`, `ORE_SURFACE` · Aktionen: `EVA` (Body+Situation), `DOCK_ANY`,
`DOCK_STATION` (an gemerkte Station) · Flotte: `VESSEL_COUNT` · **Zustandsbehaftet:** `FLYBY`
(SOI-Durchflug, rastet ein), `MARKER_LANDING` (Zielpunkt + Distanz) · **Timer:** `HOLD` (Sekunden),
`DURATION` (Tage).

---

## 5. Auswertung (`src/.../Conditions/`)

- **`CheckEvaluation`** ist das Herz: wertet eine COMPOSITE-Condition über ihre Checks aus.
  - Jeder Check wird einzeln bewertet, das Ergebnis landet als `c{i}_{j}` in `contract.Progress`
    (für die UI rot/grün).
  - **Timer** (`HOLD`/`DURATION`): startet, sobald alle übrigen Checks gleichzeitig erfüllt sind;
    Startzeit in `Progress`, läuft auch unfokussiert/unloaded weiter. Verfolgt ein konkretes Vessel
    über `persistentId`.
  - **`FLYBY`**: scannt alle realen Vessels, merkt pro Vessel SOI-Eintritt/min. PeA/„orbited",
    rastet bei erfolgreichem Durchflug dauerhaft ein (`done=1`). State pro Check unter `fb{i}_{j}`.
  - **`MARKER_LANDING`**: legt einmalig einen Zielpunkt fest (persistiert `ml{i}_{j}_*`), erzeugt
    einen sichtbaren FinePrint-Waypoint (neu nach jedem Laden) und prüft die Grosskreisdistanz.
- **`CompositeEvaluator`** hängt nur am Lifecycle (`OnCleared` → Marker-Waypoints entfernen).
- **`SimpleConditions` / `TrackingConditions`**: Legacy-Einzeltyp-Evaluatoren (FLYBY, MARKER_LANDING,
  RENDEZVOUS, ORBIT, LANDED, …) — vollständig, aber vom aktuellen Katalog ungenutzt.
- **`VesselQuery`**: geteilte API-Abfragen (Situation, Orbit-über-Atmosphäre, Ressourcen,
  Tageslänge, „echte" Vessels). Keine hardcodeten Body-Werte.
- **`GameEventBridge`**: puffert diskrete Events (Andocken, SOI-/Situationswechsel) pro Tick.
- **`EvaluationContext`**: Schnappschuss (UT, Vessels, Events, Stations) pro Prüf-Tick.

---

## 6. Steuerung & Regeln (`src/.../Core/`)

- **`ContractManager`** — Status-Flow und Geschäftslogik:
  - `Tick()` prüft aktive Missionen → bei Erfüllung `ReadyToClaim` (grün, noch nicht ausgezahlt).
  - `Accept` / `Abandon` / `Claim` / `Skip`.
  - **`ScienceMultiplier`** (0.1–3.0): Faktor auf alle Auszahlungen.
  - **Abbruch-Strafe**: `Abandon` zieht die halbe (multiplizierte) Belohnung vom Punktestand ab.
  - **`Skip`**: Notausgang — markiert erledigt (schaltet Folgemissionen frei), zahlt nichts.
  - **Stationsbau**: ist `RecordStationKey` gesetzt, wird beim Erfüllen das aktive Vessel als Station
    gemerkt (→ `StationRegistry`).
- **`StationRegistry`** — gemerkte Stationen/Basen (Name + persistentId) je Schlüssel; `%station%`
  in Beschreibungen wird daraus ersetzt; `DOCK_STATION`/Versorgungsaufträge referenzieren sie.
- **`VisibilityRules`** — was im Auswahlfenster sichtbar ist: Bemannt 3 (→5 ab 50 % erledigt),
  Erkundung 4 pro Unterkategorie (`revealAllAfter` hebt das Limit nach dem Planeten-Flyby auf),
  Netzwerk 3 pro Unterkategorie.
- **`ActiveLimits`** — gleichzeitig aktiv: Bemannt 3 / Erkundung 10 / Netzwerk 5.
- **Repeatable-Cooldown**: nach Einlösen erst wieder annehmbar nach 2 anderen Abschlüssen.
- **`Tuning`** — alle Stellschrauben, überschreibbar via `settings.cfg`. Inkl.
  `LockedPreviewTrigger` (`cr_luna_flyby_crewed`): ab dann werden gesperrte Missionen als Vorschau
  angezeigt; und `UnlockAll` (Testschalter — hebt Voraussetzungen, Limits und Cooldown auf).
- **`BodyResolver`** — Name → `CelestialBody` (gecacht).

---

## 7. Persistenz (`src/.../Persistence/`)

- **`ContractsScenario`** — `[KSPScenario]` für Science-Sandbox; `OnLoad`/`OnSave`, Prüf-Coroutine
  (Intervall aus `Tuning.CheckIntervalSeconds`), Event-Abos, UI-Erzeugung.
- **`SaveFolderStore`** — schreibt/liest den Laufzeit-State als **editierbare** Datei im Save-Ordner:
  `saves/<save>/CustomScienceContracts/contracts_state.cfg` (Root `CUSTOM_SCIENCE_CONTRACTS_STATE`).
  Speichert je Mission `status/totalCompletions/completionsSinceLastClaim` + `PROGRESS`, dazu
  `scienceMultiplier`, `unlockAll` und die `STATION`-Einträge. Diese Datei ist autoritativ; fehlt
  sie, wird frisch aus dem Katalog geseedet.

---

## 8. UI (`src/.../UI/`, IMGUI)

- **`CscUI`** (`MonoBehaviour`) — zwei AppLauncher-Buttons (`Icons/App/aktiv.png`,
  `missionen.png`), zeichnet die Fenster mit abgerundetem Rahmen, hält das Einstellungsfenster und
  den Abbruch-Bestätigungsdialog. *Aktive Missionen* überall sichtbar; *Missionskontrolle* nur in
  SpaceCenter/Editor/Ortungsstation.
- **`SelectionWindow`** — Tabs (Sparten) → aufklappbare Planeten- → Mond-Gruppen → Missionen;
  Vorschau gesperrter Missionen; **Zahnrad oben rechts** (`settings.png`, grau) öffnet die
  Einstellungen.
- **`ActiveMissionsWindow`** — aktive Missionen **nach Sparte gruppiert** (gestapelt, aufklappbar),
  Checkliste rot/grün, grüner Einlöse-Haken / roter Abbruch-X (mit roter Punkte-Warnung).
- **`SettingsWindow`** — Wissenschafts-Multiplikator (Slider), „alle Missionen freischalten"
  (Toggle), „aktive Mission überspringen".
- **`BodyVisual`** — Farb-/Icon-Zuordnung für Körper/Sparten/Unterkategorien; `MissionIcon(c)`
  liefert das pro Mission gewählte Icon; deutsche Anzeigenamen (Earth→Erde, Moon→Luna, …).
- **`Theme`** — dunkles Theme; **nur das Fenster ist abgerundet**, alle Innenflächen eckig.
  Wissenschaftswerte werden mit dem `science symbol.png` im Akzent-Blau dargestellt.
- **`IconLibrary`** — lädt Texturen (UI/Bodies/App) gecacht über die GameDatabase.
- **`SparteDisplay`** — kreative Anzeigenamen der Sparten (Pioniere / Robotische Erkunder /
  Versorgungsnetz / Daueraufträge).

---

## 9. Stationsketten (generiert in `gen_catalog.py`)

ID-Schema je Stufe N: `cr_<key>_{build | expand<N> | supply<N> | longstay<N>}`.

| Kette | Typ | Stufen (Crew) | Voraussetzung | Sparte |
|-------|-----|---------------|---------------|--------|
| `earth_station` | Orbit | 2,3,4,6,8,10,12 | `cr_luna_landing` | Bemannt |
| `moon_station` | Orbit | 2,3,4,6,8,10 | `cr_earth_station_longstay4` | Bemannt |
| `moon_base` | Oberfläche | 2,3,4,6,8,10 | `cr_luna_precision_landing` + `cr_luna_stay_7d` | Bemannt |
| `mars_station` | Orbit | 2,3,4,6 | `cr_mars_stay_10d` | Bemannt |
| `mars_base` | Oberfläche | 2,3,4,6 | `cr_mars_stay_30d` | Bemannt |
| `earth_fuel_depot` | Treibstoff-Tankstelle (bemannt) | 2,3,4,6 | `cr_earth_station_longstay4` | NetzwerkLogistik |

Muster der bemannten Stations-/Basisketten je Stufe: **Ausbau(N) → Versorgung(N, wiederholbar) →
Dauerbetrieb 150 Tage(N)** (der Dauerbetrieb ist das Tor zur nächsten Stufe). Der Fuel-Depot hat
**kein** 150-Tage-Tor: Crew = Stufe, Treibstoff startet bei 1440 LiquidFuel / 1760 Oxidizer und
wächst je Stufe um +1440 / +1760; je Stufe eine wiederholbare Nachbetankung.

---

## 10. cfg-Format (Referenz)

```
CUSTOM_CONTRACT_CATALOG
{
    CONTRACT
    {
        id = un_earth_orbit
        title = Erster Erdorbit (unbemannt)
        description = Bringe deine erste Sonde …          // KEINE { } im Text! %station% = Stationsname
        sparte = UnbemannteErkundung                       // Enum-Name, nicht der Anzeigename
        subcategory = Erde
        icon = TrackingStation_ButtonMapCommunicationsRelay
        reward = 32
        revealAllAfter = un_jupiter_flyby                  // optional (äusseres System)
        repeatable = true                                  // optional
        recordStationKey = earth_station                   // optional (Stationsbau)
        stationRef = earth_station                         // optional (Stationsbezug)
        prerequisite = un_earth_suborbital                 // 0..n
        CONDITION
        {
            type = COMPOSITE
            CHECK { kind = CREW_NONE  label = unbemannt }
            CHECK { kind = ORBIT_ABOVE  body = Earth  label = stabiler Erdorbit }
            CHECK { kind = HOLD  seconds = 10  label = 10 Sekunden stabil halten }
        }
    }
}
```

**ConfigNode-Falle:** `{` und `}` sind Node-Trenner — niemals in `description=` verwenden.
Platzhalter für den Stationsnamen ist `%station%`.

---

## 11. Build & Auslieferung

- **Bauen/Deployen:** `./build.sh` (baut Release net48 gegen die KSP-Managed-DLLs und kopiert
  `GameData/CustomScienceContracts` ins KSP). `dotnet` liegt unter `/usr/local/share/dotnet/`.
- **Katalog neu erzeugen:** `python3 tools/gen_catalog.py` → danach
  `python3 tools/validate_catalog.py` (muss grün sein).
- **Auslieferungen:**
  - `fertig/` — kompletter Release-Mod (verboseLogging aus) zum Herunterladen/Spielen.
  - `für mich/` — derselbe Mod plus `Spielstand/contracts_state.cfg` (vorprogressiert bis
    Satelliten im Erdorbit, bemannter Erdorbit, unbemannter Mond-Vorbeiflug).

---

## 12. Typischer Erweiterungs-Workflow

1. Mission(en) im `missionsdesign.md` ergänzen/ändern (bzw. Text in `rewrite_desc.py`).
2. `python3 tools/validate_design.py` → `python3 tools/gen_catalog.py` →
   `python3 tools/validate_catalog.py`.
3. Bei Engine-Änderungen (neuer Check-Typ o. Ä.) den C#-Code anpassen.
4. `./build.sh`, im Spiel testen (Testschalter `unlockAll`, `verboseLogging` in `settings.cfg`).
5. Neue Body-Namen IMMER gegen `Sol-Configs/` prüfen (keine erfundenen Bodies).
