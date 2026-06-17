# AGENTS.md - CustomScienceContracts

## Was wir bauen
Ein KSP1-Plugin in C# fuer KSP 1.12.x. Es stellt im Science Mode ein eigenes
Missions-/Zielsystem bereit, parallel und unabhaengig vom Stock-Contractsystem.
Erfuellte Missionen geben einen Science-Bonus ueber
`ResearchAndDevelopment.AddScience(amount, TransactionReasons.Cheating)`.

## Aktuelle Quelle der Missionen
**Single Source of Truth:** `custom_science_contracts_missionsdesign.md`.

Die Dateien in `GameData/CustomScienceContracts/Contracts/*.cfg` sind generiert.
Sie werden nicht von Hand editiert, sondern ueber `tools/gen_catalog.py` neu erzeugt.
Die alte v3-Spec, alte SOL-Config-Kopien und alte Auslieferungsordner sind bewusst
nicht mehr Teil des Arbeitsordners.

## Saubere Ordnerstruktur
- `src/CustomScienceContracts/` - C#-Quellcode.
- `GameData/CustomScienceContracts/` - KSP-Moddaten: generierter Katalog, Icons, Settings.
- `tools/` - Generator und Validatoren fuer Missionsdesign und Katalog.
- `custom_science_contracts_missionsdesign.md` - Ablauf und Missionsdesign.
- `customScienceContracts Logo.png` - Logo/Artwork, bewusst behalten.
- `DOKUMENTATION.md` - Architektur- und Workflow-Dokumentation.

## Build / Workflow
1. Missionsdesign aendern: `custom_science_contracts_missionsdesign.md`.
2. Design pruefen: `python3 tools/validate_design.py`.
3. Katalog erzeugen: `python3 tools/gen_catalog.py`.
4. Katalog pruefen: `python3 tools/validate_catalog.py`.
5. Plugin bauen/deployen: `./build.sh`.

Build-Ausgaben wie `bin/`, `obj/`, DLLs und Release-Exportordner werden nicht als
Quellbestand gepflegt. Sie sind regenerierbar.

## Laufzeitregeln
- Persistenz ueber `ScenarioModule` und eine editierbare Save-Datei:
  `saves/<save>/CustomScienceContracts/contracts_state.cfg`.
- UI ueber `ApplicationLauncher` und IMGUI-Fenster.
- Pruef-Loop nicht jeden Frame, sondern per Coroutine etwa alle 1 s ueber aktive Missionen.
- Body-Groessen, Atmosphaerenhoehen und Tageslaenge immer aus der KSP-API ziehen.

## Contract-Flow
- `Locked`: Voraussetzungen fehlen.
- `Available`: Voraussetzungen erfuellt, sichtbar nach Sichtbarkeitsregeln.
- `Active`: angenommen und im Pruef-Loop.
- `ReadyToClaim`: Bedingungen erfuellt, Reward noch nicht ausgezahlt.
- `CompletedOnce`: abgeschlossen.

Repeatables wandern nach Erstabschluss in die Sparte `Wiederholbar` und brauchen
den Cooldown von zwei anderen Abschluessen.

## Harte Verbote
- Keine Part-Anforderungen.
- Keine Kopplung an Kerbalism/Simplex-APIs.
- Keine hardcodeten Body-Groessen oder Atmosphaerenhoehen.
- Keine erfundenen Body-Namen.

## Text / Stil
Contract-Titel und Beschreibungen bleiben auf Deutsch in Schweizer Hochdeutsch
mit `ss` statt `ß`. Code und Kommentare duerfen Englisch sein.
Das UI darf vom alten "kein Dunkelblau"-Hinweis abweichen; die aktuelle dunkle
Oberflaeche mit hellem Akzent ist gewollt.
