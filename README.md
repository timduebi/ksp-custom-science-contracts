<p align="center">
  <img src="Logo.png" width="180" alt="Custom Science Contracts">
</p>

<h1 align="center">Custom Science Contracts</h1>

<p align="center"><b>Custom Science Contracts</b> gibt dem KSP Science Mode endlich einen roten Faden.</p>

---

**Custom Science Contracts** fügt eine eigene, strukturierte Wissenschaftskampagne für den Science Mode hinzu.

Der normale Science Mode gibt dir zwar viel Freiheit, fühlt sich aber oft ziellos an. Man sammelt Science, schaltet Teile frei und entscheidet selbst, wohin es als Nächstes geht – aber ein echter roter Faden fehlt. Der Career Mode hat zwar Contracts, diese wirken jedoch oft zufällig, unpassend oder störend: Part-Tests, belanglose Aufträge und Missionen ohne echten Zusammenhang.

**Custom Science Contracts verbindet das Beste aus beiden Welten:** die Freiheit und den Forschungsfokus des Science Mode mit der klaren Missionsstruktur einer Kampagne – aber ohne zufällige, nervige Career-Mode-Aufträge.

Der Mod führt dich Schritt für Schritt durch eine eigene Wissenschaftskampagne: von den ersten einfachen Flügen, über Orbit, Raumstationen, Mondbasen und Versorgungsmissionen, bis hin zur realistischen Expansion ins gesamte Sonnensystem. Jede Mission hat ein klares Ziel und ist Teil einer nachvollziehbaren Progression.

Die Kampagne ist bewusst realistisch aufgebaut. Du wirst nicht plötzlich nach Jool geschickt, obwohl du noch kaum Erfahrung mit Duna hast. Neue Ziele werden logisch vorbereitet: Zuerst kommen robotische Vorerkundungen, dann Orbitmissionen, danach Landungen, längere Aufenthalte und schließlich dauerhafte Infrastruktur. So wie in der echten Raumfahrt landet niemand einfach irgendwo, ohne vorher robotisch dort gewesen zu sein.

Auch bemannte Missionen bauen sinnvoll aufeinander auf. Bevor du eine Crew zu einem weit entfernten Ziel wie Duna schickst, musst du zuerst beweisen, dass deine Raumfahrtagentur längere Aufenthalte im All unterstützen kann. Lange Raumstationsmissionen zeigen zum Beispiel, dass du Lebenserhaltung, Versorgung, Crewrotation und Missionsbetrieb über längere Zeit beherrschst. Erst danach werden größere interplanetare Schritte glaubwürdig und sinnvoll.

Ein besonderer Fokus liegt auf Infrastruktur. Im normalen Spiel sind Raumstationen, Mondbasen oder Außenposten oft eher freiwillige Bauprojekte ohne echten spielerischen Nutzen. **Custom Science Contracts macht sie zu einem wichtigen Teil deiner Progression.** Der Bau, Ausbau, Betrieb, die Versorgung und Wiederaufstockung von Ressourcen werden mit Science belohnt und geben Stationen und Basen endlich einen echten Zweck.

Auch Ressourcen- und Netzwerkmissionen sind Teil der Kampagne. Du wirst nicht nur irgendwohin geschickt, um Science zu sammeln, sondern baust nach und nach eine funktionierende Raumfahrtinfrastruktur auf: Orbitalstationen, Basen, Versorgungsrouten, Erzförderung und Missionsnetzwerke.

Der Mod löst damit mehrere große Probleme des normalen Science- und Career-Gameplays: Science Mode ist oft zu ziellos, Career Contracts sind oft zu zufällig, und Infrastruktur wie Stationen oder Basen ist normalerweise kaum notwendig. **Custom Science Contracts gibt deinem Space Program eine klare Richtung, sinnvolle Zwischenziele und eine realistische Progression.**

Der Mod ist für Spieler gedacht, die den Science Mode mögen, sich aber mehr Struktur, mehr Sinn und eine echte Kampagne wünschen.

## Features

- Eigene Wissenschaftskampagne für den Science Mode
- Klare, realistische Progression vom ersten Flug bis ins äußere Sonnensystem
- Verbindet die Freiheit des Science Mode mit der Struktur einer Kampagne
- Keine zufälligen Part-Test-Contracts
- Missionen mit rotem Faden und klaren Zielen
- Robotische Vorerkundung vor bemannten Missionen
- Lange Raumstationsaufenthalte als Vorbereitung für interplanetare Crews
- Science-Belohnungen für Raumstationen, Basen, Ausbau und Betrieb
- Sinnvolle Infrastruktur-Missionen für Versorgung, Ressourcen und Netzwerke
- Erzförderung, Ressourcennachschub und Missionsnetzwerke als Teil der Progression
- Kein zielloses Science-Grinding
- Progression für Stock Kerbol System und SOL / reales Sonnensystem
- Besonders empfohlen mit Kerbalism

## Kompatibilität

- **Kerbal Space Program 1.12.x**, Science Mode.
- **Stock Kerbol System** – über die optionale Stock-Config (siehe unten).
- **SOL / reales Sonnensystem** – der Standard-Download ist auf reale Körpernamen ausgelegt (`Earth`, `Moon`, `Mars`, `Jupiter`, `Saturn` …, abgestimmt auf SOL Quarter-Scale).
- **Kerbalism wird empfohlen**, ist aber keine Voraussetzung: lange Missionen, Versorgung, Lebenserhaltung, Stationen und Basen bekommen damit noch mehr Bedeutung.
- **Kein** Contract Configurator und **keine** bestimmten Part-Packs nötig – die Missionsziele prüfen nur Stock-Zustände (Orbit, Landung, Crew, Schiffsanzahl, Ressourcen, Vorbeiflug, Zeit …).

## Installation

1. Lade **`CustomScienceContracts-vX.Y.Z.zip`** von der [Release-Seite](https://github.com/timduebi/ksp-custom-science-contracts/releases/latest) herunter.
2. Entpacke das Archiv und kopiere den Ordner `GameData` in dein KSP-Verzeichnis.
3. Starte einen Science-Mode-Spielstand.

Danach sollte es so aussehen:

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

### Optionale Config-Swaps

Der Standard-Download enthält die SOL-Kampagne (reales Sonnensystem). Zwei optionale Config-Packs im selben Release ändern, **welche Missionen** du fliegst. Sie ersetzen jeweils nur die vier Katalog-Dateien in `GameData/CustomScienceContracts/Contracts/`. Erst den Haupt-Download installieren, dann ein Pack darüber entpacken und das Überschreiben bestätigen – immer nur **eine** Config gleichzeitig.

- **`CustomScienceContracts-vX.Y.Z_Stock-Config.zip`** – baut die ganze Kampagne auf das **Stock-Kerbol-System** um (Kerbin, Mun, Minmus, Duna, Jool, Laythe, Eve …). Für ein normales, nicht skaliertes Spiel.
- **`CustomScienceContracts-vX.Y.Z_German-Config.zip`** – die SOL-Kampagne mit allen Contract-Texten auf **Deutsch**. Gleiche Missionen, nur die Sprache ändert sich.

Zurück zum Standard: die vier `Contracts/*.cfg` einfach erneut aus dem Haupt-Download entpacken.

## Hinweise zum Gameplay

- Höhenangaben sind Mindestwerte (z. B. „Periapsis über 2000 km" = mehr als 2000 km).
- Polarmissionen verlangen mindestens 75° Inklination.
- Kommunikationsnetze zählen echte Vessels im Orbit – Debris, Flaggen, Asteroiden und ausgesetzte Science-Objekte zählen nicht.
- Der Fortschritt wird pro Spielstand gespeichert unter `saves/<Name>/CustomScienceContracts/contracts_state.cfg`.

## Entwicklung

Die Kampagnen werden aus Designplänen generiert, die generierten `.cfg`-Dateien werden **nie** von Hand bearbeitet. Architektur, Build und Release-Workflow sind in [`DEVELOPMENT.md`](DEVELOPMENT.md) beschrieben, technische Details in [`DOKUMENTATION.md`](DOKUMENTATION.md).

## Lizenz und Drittanbieter-Assets

Custom Science Contracts steht unter der GNU General Public License 3.0 (siehe [`LICENSE`](LICENSE)). Einige Bild-Assets stammen von Dritten und werden unverändert mitgeliefert: ZTheme (GPL-3.0) und Kerbal Planet Emblems (MIT). Es wird kein Code dieser Projekte verwendet. Details in [`THIRD_PARTY_NOTICES.md`](THIRD_PARTY_NOTICES.md).
