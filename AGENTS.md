# AGENTS.md — Custom Contract Plugin (KSP1 / SOL Quarter-Scale)

## Was wir bauen
Ein KSP1-Plugin (C#) das ein eigenes Contract-/Ziel-System bereitstellt, parallel und
unabhaengig vom Stock-Career-Contractsystem. Lauffaehig im **Science Mode**. Es zeigt
selbst definierte Missionen, trackt deren Erfuellung ueber reine Stock-Spielzustaende und
vergibt bei Abschluss einen Science-Bonus via
`ResearchAndDevelopment.AddScience(amount, TransactionReasons.Cheating)`.

**Single Source of Truth fuer die Missionen:** die hochgeladene Datei
`Missionsplan_Spec_SOL_v3.md`. Diese AGENTS.md enthaelt zusaetzlich verbindliche
Korrekturen (Abschnitt "Korrekturen zur v3-Spec"), die ueber die Datei gelten.

## Setup / Build
- KSP 1.12.x, C#, Ziel-Framework .NET 4.x (Unity-Mono).
- Referenzen aus `KSP_x64_Data/Managed/`: `Assembly-CSharp.dll`, `UnityEngine.*.dll`.
- Output-DLL nach `GameData/CustomContracts/Plugins/`.
- Persistenz ueber `ScenarioModule` mit `[KSPScenario(...)]`, Speichern/Laden via
  `OnSave`/`OnLoad` in ein `ConfigNode` (Fortschritt, aktive Missionen, Cooldown-Zaehler,
  gesetzte Marker-Waypoints).
- UI ueber `ApplicationLauncher`-Button + `OnGUI`/`GUILayout`-Fenster.
- Pruef-Loop NICHT jeden Frame: Coroutine alle ~1 s ueber die AKTIVEN Missionen.

## Erste Schritte (bevor Contract-Liste gecoded wird)
1. **Body-Namen abgleichen.** Aus dem configs-Ordner (oder per Debug-Dump
   `foreach b in FlightGlobals.Bodies -> b.name`) die internen `name`-Strings ziehen.
   Die Platzhalter in der v3-Spec (`Earth`, `Luna`, `Mars`, `Mercury`, `Ganymede`, ...)
   gegen die echten Strings mappen. **Mir die fertige Mapping-Tabelle zeigen und bestaetigen
   lassen, bevor die Contracts hardcoded werden.**
2. **Asteroiden klaeren:** Sind sie echte `CelestialBody` (dann FLYBY/ORBIT/LANDED normal)
   oder spawnbare Tracking-Objekte (dann `RENDEZVOUS` an ein Vessel)? Aus configs ableiten.
3. **Aeussere Monde:** pro aeusserem Planeten auf >=10 Flyby-Contracts auffuellen, SOWEIT
   SOL so viele Monde modelliert. Gibt es weniger, auf die tatsaechliche Anzahl reduzieren —
   keine erfundenen Bodies anlegen.

## Datenmodell
```
enum Sparte { Bemannt, UnbemannteErkundung, NetzwerkLogistik, Wiederholbar }
class MissionContract {
  string Id, Titel, Beschreibung;
  Sparte HeimatSparte;            // Bemannt/UnbemannteErkundung/NetzwerkLogistik
  string Unterkategorie;          // Koerper-Name (Luna eigene Kat.; Monde unter Planet)
  string[] Voraussetzungen;       // IDs, muessen alle Completed sein
  Condition[] Bedingungen;
  float ScienceReward;
  bool Repeatable;
  // Laufzeit-State:
  Status status;                  // Locked / Available / Active / CompletedOnce
  int completionsSinceLastClaim;  // fuer Repeatable-Cooldown
}
```

## Contract-States & Flow
- **Locked** -> nicht alle Voraussetzungen Completed.
- **Available** -> alle Voraussetzungen Completed, im Auswahlfenster sichtbar (Sichtbarkeits-
  regeln s.u.). NICHT getrackt.
- **Active** -> vom Spieler angenommen, wird im Pruef-Loop getrackt, zaehlt gegen das
  Aktiv-Limit. Annehmen nur moeglich, solange Limit nicht erreicht.
- **CompletedOnce** -> Bedingungen erfuellt, Reward ausgezahlt.
  - Nicht-Repeatable: endgueltig fertig.
  - Repeatable: wandert in Sparte **Wiederholbar**, raus aus der Heimat-Liste.

## Repeatable / Sparte Wiederholbar
- Vor Erstabschluss lebt der Contract in seiner Heimat-Unterkategorie.
- Nach Erstabschluss erscheint er nur noch in der Sparte **Wiederholbar**.
- **Cooldown:** erneut annehmbar erst, wenn seit dem letzten Einloesen **>= 2 andere
  Missionen** abgeschlossen wurden. Zaehler `completionsSinceLastClaim`: reset 0 beim
  Einloesen, +1 bei jedem anderen Abschluss, annehmbar wenn >= 2. UI zeigt Restzaehler.

## Sichtbarkeitsregeln (Auswahlfenster, = "Available")
- Eine Mission wird NUR angezeigt, wenn alle Voraussetzungen erfuellt sind.
- **Bemannt:** max. **3** gleichzeitig sichtbare Available-Contracts. Sobald **>= 50 %**
  aller bemannten Contracts CompletedOnce sind, steigt das Limit auf **5**.
- **Unbemannte Erkundung:** **4** Available pro Unterkategorie. AUSNAHME aeusseres System:
  zuerst ist nur der Planeten-Flyby sichtbar; ist dieser abgeschlossen, werden **alle**
  Contracts dieser Unterkategorie sichtbar (4er-Limit faellt fuer diese Unterkat. weg).
  -> Unterkategorie-Feld `RevealAllAfter = un_<planet>_flyby`.
- **Netzwerk/Logistik:** max. **3** sichtbar pro Unterkategorie.

## Aktiv-Limits (gleichzeitig angenommene Missionen)
- Bemannt: max. **3** aktiv.
- Unbemannte Erkundung: max. **10** aktiv.
- Netzwerk/Logistik: max. **5** aktiv.
- Annehmen bei erreichtem Limit blockiert (UI-Hinweis).

## Bedingungstypen
Einfach (zuerst implementieren): `ORBIT`, `ORBIT_HIGH`, `LANDED`, `ATMO_ENTRY`,
`ALT_FRACTION_ATMO`, `ABOVE_ATMO_SUBORBITAL`, `EVA`, `CREW_DURATION`, `DOCK`,
`ORE_SURFACE`, `VESSEL_COUNT_ORBIT`, `FUEL_ORBIT`.
Knifflig (State-Tracking, zuletzt): `FLYBY`, `MARKER_LANDING`, `RENDEZVOUS`.
- `ALT_FRACTION_ATMO` / `ABOVE_ATMO_SUBORBITAL`: Schwellen als Bruchteil von
  `CelestialBody.atmosphereDepth`, zur Laufzeit gezogen, NIE hardcoden.
- `MARKER_LANDING`: Waypoint beim Annehmen setzen, bei LANDED Distanz <= R pruefen.
  R = 15 km Standard, 5 km bei Basis-Versorgung/-Rotation.

## Korrekturen zur v3-Spec (gelten ueber die hochgeladene Datei)
1. **Startphase umverteilt:**
   - `cr_pad` und `cr_upperatmo` -> werden UNBEMANNT: `un_pad`, `un_upperatmo`
     (Unterkat. Erde, Crew nicht erforderlich).
   - "Atmosphaere verlassen" existiert in BEIDEN Sparten: `un_leaveatmo` (unbemannt) und
     `cr_leaveatmo` (bemannt).
   - Unbemannte Erde-Kette: `un_pad` -> `un_upperatmo` -> `un_leaveatmo` -> `un_earth_orbit`.
2. **Bemannte Starts erst nach Satellit ausserhalb Atmosphaere:**
   - `cr_leaveatmo` (erste bemannte Mission) Voraussetzung: **`un_leaveatmo`**.
   - Bemannte Erde-Kette startet bei `cr_leaveatmo` (kein bemanntes pad/upperatmo mehr).
3. **Venus & Merkur: zuerst bemannter Flyby.**
   - Neu `cr_venus_flyby` (FLYBY Venus, Crew>=1; Voraus: cr_mars_orbit, un_venus_orbit)
     vor `cr_venus_orbit` (Voraus jetzt: cr_venus_flyby).
   - Neu `cr_mercury_flyby` (FLYBY Mercury, Crew>=1; Voraus: cr_venus_orbit, un_mercury_orbit)
     vor `cr_mercury_orbit` (Voraus jetzt: cr_mercury_flyby).
4. **Mars erst nach erster Raumstation:**
   - `cr_mars_orbit` Voraussetzung zusaetzlich: **`cr_station_leo`**.
5. **Stations-Langzeit >200 Tage, frueh:**
   - Neu `cr_station_longstay200` (CREW_DURATION ORBIT Earth, Crew>=3, 200d),
     Voraussetzung: `cr_station_expand3`. Reward hoch (~450). Erscheint frueh im
     Stations-Block, NICHT erst nach Mars.
6. **Crew-Groessen erhoeht:**
   - Erde-Station: Ausbaustufen auf 4 und dann **6** Crew (`cr_station_expand4` Crew>=4,
     neu `cr_station_expand6` Crew>=6 / 15d, Voraus: cr_station_expand4).
   - Luna-Basis: Ausbau auf **3** Crew (`cr_luna_base_expand` Crew>=3), optional zweite
     Stufe Crew>=4.
7. Alles uebrige aus v3 bleibt gueltig (Mond-Gate, Ganymed-Finale, Kategorie C, usw.).

## UI-Hinweise
- 4 Tabs (Sparten), darunter Unterkategorien (Koerper) als aufklappbare Gruppen.
- Zweites Fenster: aktive Missionen mit Fortschritt/Status.
- Aktiver Tab/Text klar lesbar (heller Text auf dunklem Grund). Dunkles, kontrastreiches
  Theme, EIN starker Akzent (Hellblau oder grün). Kein Dunkelblau.

## Verbote (hart)
- KEINE Part-Anforderungen.
- KEINE Kopplung an Kerbalism/Simplex-API (MITE/Scan bleiben rein narrativ in der
  Beschreibung; geprueft wird nur Orbit/Landung).
- KEINE hardcoded Body-Groessen/Atmosphaerenhoehen — immer aus der API.
- KEINE erfundenen Body-Namen.

## Arbeitsweise
- Geruest zuerst (Datenmodell, ScenarioModule-Persistenz, Sparten-/Unterkategorie-Manager
  mit Sichtbarkeits- und Aktiv-Limits, UI-Skelett), dann einfache Bedingungen, dann die
  drei kniffligen.
- Bei groesseren Architektur-Entscheidungen kurz Ruecksprache.
- Contract-Titel/Beschreibungen auf Deutsch (Schweizer Hochdeutsch, "ss" statt "ss"),
  Code/Kommentare Englisch ok.
