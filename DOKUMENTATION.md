# CustomScienceContracts - Dokumentation

Stand: Juni 2026.

CustomScienceContracts ist ein KSP-1.12-Plugin fuer den Science Mode. Es baut ein
eigenes Missionssystem parallel zum Stock-Contractsystem. Missionen werden ueber
Stock-Spielzustaende geprueft und zahlen beim Einloesen einen Science-Bonus aus.

## Ordner

```text
ksp science contracts/
├── AGENTS.md
├── DOKUMENTATION.md
├── CustomScienceContracts.sln
├── build.sh
├── custom_science_contracts_missionsdesign.md
├── customScienceContracts Logo.png
├── GameData/CustomScienceContracts/
│   ├── Contracts/              generierter Missionskatalog
│   ├── Icons/                  UI-, App- und Body-Icons
│   └── settings.cfg            Tuning-Werte
├── src/CustomScienceContracts/ C#-Quellcode
└── tools/                      Generator und Validatoren
```

Nicht mehr Teil des sauberen Arbeitsordners: alte v3-Spec, alte SOL-Config-Kopien,
`compiled/`, `fertig/`, `für mich/`, `bin/`, `obj/`, `.DS_Store` und fertige DLLs.
Diese Dinge sind entweder veraltet, doppelt oder regenerierbar.

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
        └─ CatalogLoader → ContractCatalog → ContractManager → UI / Save-State
```

Die `.cfg`-Dateien werden nicht von Hand editiert. Aenderungen passieren im
Missionsdesign oder im Generator.

## Wichtigste Laufzeitteile

- `ContractsScenario`: KSP-ScenarioModule, initialisiert Katalog, Settings, UI,
  Events und den 1-Sekunden-Pruefloop.
- `ContractManager`: Status-Flow, Annehmen, Abbrechen, Einloesen, Skip,
  Repeatable-Cooldown und Science-Auszahlung.
- `CheckEvaluation`: wertet die handgeschriebenen `CHECK`-Teilziele aus.
- `SaveFolderStore`: speichert den Fortschritt im Save-Ordner als
  `CustomScienceContracts/contracts_state.cfg`.
- `CscUI`, `SelectionWindow`, `ActiveMissionsWindow`: IMGUI-Oberflaeche.

## Missionen

Aktuelle Quelle: `custom_science_contracts_missionsdesign.md`.

Der Plan enthaelt 140 handgeschriebene Missionen plus Stationsketten, die von
`tools/gen_catalog.py` in vier Katalogdateien erzeugt werden:

- `A_Pioniere.cfg` - bemannte Missionen.
- `B_Spaeher.cfg` - robotische Erkundung.
- `C_Lebensadern.cfg` - Netzwerk und Logistik.
- `D_Stationen.cfg` - generierte Stations-, Basis- und Depotketten.

## Build

Typischer Ablauf:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
./build.sh
```

`build.sh` baut das Plugin gegen die KSP-Managed-DLLs und legt die erzeugte DLL
unter `GameData/CustomScienceContracts/Plugins/` ab. Danach kopiert es den Mod in
die lokale KSP-Installation. Der Plugin-Ordner und die DLL muessen nicht im Repo
liegen, weil sie beim Build neu entstehen.

## Regeln

- Keine Part-Anforderungen.
- Keine Kerbalism-/Simplex-API-Kopplung.
- Keine hardcodeten Body-Groessen oder Atmosphaerenhoehen.
- Keine erfundenen Body-Namen.
- Contract-Texte auf Deutsch in Schweizer Hochdeutsch.

## Pruefstand

Beim letzten Aufraeumen waren beide Validatoren gruen:

```bash
python3 tools/validate_design.py
python3 tools/validate_catalog.py
```
