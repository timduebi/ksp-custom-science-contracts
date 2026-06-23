# CustomScienceContracts Documentation

## Purpose

CustomScienceContracts is a KSP 1.12.x Science Mode plugin. It provides a
separate mission system that is independent from the stock career contract
system. Missions are selected in a custom UI, tracked through stock KSP state and
paid out as science bonuses when the player claims them.

The mod does not require Contract Configurator, Kerbalism, Simplex or specific
part packs.

The campaign balance assumes a probe-first tech progression. Players should use
a tech-tree or progression mod that unlocks probes before crewed command pods or
crew access; otherwise the early robotic-before-crewed mission flow can be
bypassed by the stock tech order.

## Runtime Overview

At game load, `ContractsScenario` initializes the mod:

1. Load tuning values from `GameData/CustomScienceContracts/settings.cfg`.
2. Load all `CUSTOM_CONTRACT_CATALOG` nodes through KSP `GameDatabase`.
3. Build the in-memory `ContractCatalog`.
4. Load save-specific state from
   `saves/<SaveName>/CustomScienceContracts/contracts_state.cfg`.
5. Register condition evaluators.
6. Create the UI.
7. Start the 1-second check loop.

The check loop evaluates active missions in Flight, Tracking Station, Space
Center and Editor scenes. In-game universal time remains authoritative, so
timers do not advance in the editor while the universe clock is frozen.

## Mission Flow

Each mission moves through these states:

- `Locked`: prerequisites are not complete.
- `Available`: prerequisites are complete and visibility rules allow display.
- `Active`: accepted by the player and tracked by the check loop.
- `ReadyToClaim`: all objectives are complete; reward is waiting.
- `CompletedOnce`: claimed at least once.

Repeatable missions stay in their home branch after first completion so
completed supply steps remain visible in their station/base chain. They also
appear in the repeatable branch and require two other mission completions before
they can be accepted again.

## Branches

Mission Control has two top-level pools:

- Campaign Atlas: the normal campaign flow, including completed repeatable
  cards in their original home branch for context.
- Repeatables: a shortcut pool for missions that have completed once and are
  now in their cooldown pool.

Within an epoch page, the campaign atlas displays three branch rows:

- Pioneers: crewed progression.
- Robotic Explorers: probes, mapping, landers and flybys.
- Lifelines: communication networks, logistics and depots.

The internal enum names are still German (`Bemannt`, `UnbemannteErkundung`,
`NetzwerkLogistik`, `Wiederholbar`). They are stable config/save keys and should
not be renamed without migration.

## Mission Control Atlas

`SelectionWindow` renders Mission Control as a resizable atlas. The window opens
near fullscreen by default (`missionCenterScale` in `settings.cfg`) and can be
resized from the lower-right corner.

The layout is:

1. Campaign/Repeatable mode tabs.
2. Epoch tabs with counts for currently acceptable missions.
3. One epoch page at a time.
4. Branch rows for Pioneers, Robotic Explorers and Lifelines.
5. Body rows inside each branch.
6. Mission cards connected by prerequisite arrows.

Columns are based only on real prerequisites. Cards that share a body row but do
not depend on each other are placed in vertical lanes instead of being pushed
horizontally, so optional side missions do not look like false blockers.
Repeatable cards remain part of the campaign layout after completion. This keeps
station resupply, base supply and depot supply steps visible between their
build/expansion and long-stay follow-ups instead of leaving holes in the atlas.

Mission cards show only the compact summary by default: title, body and science
reward. Expanding a card shows description, action, requirements, cross-epoch
unlocks and optionally objectives. Locked cards show unlock requirements instead
of the mission description. Cross-epoch unlock hints use blue while the target is
still locked and green once the target has unlocked. Dependency arrows use
rounded curve connectors with arrowheads so crossings remain readable in dense
epochs.

## Visibility Rules

Home-branch visibility is computed by `VisibilityRules`:

- Pioneers: initially 3 visible available missions; later 5 once at least half
  of the crewed branch is completed.
- Robotic Explorers: 4 visible missions per subcategory.
- Outer-system robotic branches can reveal all missions in a subcategory after
  the planet flyby is completed.
- Lifelines: 3 visible missions per subcategory.

Active limits are enforced by `ActiveLimits`:

- Pioneers: 3 active missions.
- Robotic Explorers: 10 active missions.
- Lifelines: 5 active missions.

## Conditions and Checks

Modern missions use `COMPOSITE` conditions with multiple `CHECK` subnodes. Each
check has its own UI line and can be shown as fulfilled or open.

Supported check types include:

- Crew checks: `CREW_MIN`, `CREW_NONE`, `CREW_EXACT`.
- Vessel/body state: `CREW_CAPACITY_MIN`, `ORBIT_ABOVE`, `APOAPSIS_MAX`,
  `LANDED`, `SUBORBITAL`, `EVA`.
- Orbit details: `INCLINATION_MIN`, `VESSEL_COUNT`,
  `VESSEL_COUNT_INCLINATION`, `RELAY_VESSEL_COUNT`,
  `RELAY_VESSEL_COUNT_INCLINATION`.
- Time: `HOLD`, `DURATION`.
- Events: `DOCK_ANY`, `DOCK_STATION`.
- Stateful goals: `FLYBY`, `MARKER_LANDING`, `RETURN_FROM_BODY`.
- Resources and ground movement: `FUEL_MIN`, `RESOURCE_MIN`, `ORE_SURFACE`,
  `WHEEL_MOTION`.
- Atmosphere: `ATMO_FRACTION`, `SUBORBITAL_ABOVE_ATMO`.

All body sizes, atmosphere heights and day lengths are read from the KSP API at
runtime. The code must not hardcode those values.

## Vessel Binding

Active single-vessel missions can be pinned to a concrete craft from the Active
Missions window. `MissionBinding` stores `assignedVid`, `assignedName` and
`assignedLost` inside the mission `PROGRESS` node. When a vessel is assigned,
single-vessel checks, flybys, EVA proximity and duration timers use that vessel
instead of whichever craft happens to be focused.

Missions with `recordStationKey` must have an assigned vessel before they can
complete, so station/base identity is recorded from the intended craft. Follow-up
long-stay and expansion missions with `stationRef` auto-assign the registered
station on accept. Supply missions with `DOCK_STATION` are not auto-assigned to
the station; they keep tracking the player's supply craft while docking is
checked against the registered station.

Docking merges are remapped every tick for timer subjects, mission assignments,
fleet members and `StationRegistry` entries. This keeps station references valid
after a resupply craft docks and KSP replaces one persistent vessel id with the
docking survivor.

Network missions are fleet-bindable. Assigned satellites are stored as `FLEET`
nodes in active mission progress; network checks count only those assigned
vessels so unrelated craft cannot satisfy the objective. On claim, the completed
fleet is copied into `FleetRegistry` as `FLEET_RECORD` nodes, allowing follow-up
network missions to inherit the predecessor's satellites.

## Waypoints

Precision landing missions use `MARKER_LANDING`. The target coordinates are
stored in mission progress. The visible FinePrint waypoint is created only in
Flight/Map/Tracking scenes and recreated after scene changes if needed.

This avoids the KSP waypoint object being created in scenes where it may not
survive into flight.

## Persistence

Save state is stored outside the `.sfs` file:

```text
saves/<SaveName>/CustomScienceContracts/contracts_state.cfg
```

It contains:

- mission status,
- total completions,
- repeatable cooldown counters,
- active mission progress,
- assigned vessel ids/names/lost flags,
- active fleet assignments,
- timer state,
- flyby state,
- marker target state,
- registered station/base vessels,
- completed network fleet records,
- science multiplier and unlock-all test setting.

If the state file is missing or broken, the mod seeds state from the catalog.

## Mission Catalogs

The SOL catalog is generated from:

```text
custom_science_contracts_missionsdesign.md
```

Generator:

```bash
python3 tools/gen_catalog.py
```

Default output, shipped in the main download:

```text
GameData/CustomScienceContracts/Contracts/
├── A_Pioniere.cfg
├── B_Spaeher.cfg
├── C_Lebensadern.cfg
└── D_Stationen.cfg
```

The optional German SOL config is generated from the same mission source:

```text
OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts/
```

Both catalogs use the same ids, prerequisites, rewards and checks. They change
only player-facing titles, descriptions, subcategory labels and checklist labels.

The Stock KSP campaign ships as an optional config pack (`OptionalConfigs/Stock/`)
on the same release, replacing only the four catalog files above.

Stock is generated from:

```text
custom_science_contracts_stock_missionsdesign.md
```

Each Stock mission block must include an explicit `title:` and `epoche:` value.
`tools/gen_catalog_stock.py` reads the Stock chapter names from the `Epoche N —
Name:` lines in the design source and writes both `epoch` and `epochName` into
the generated optional config. `tools/validate_design.py ... stock` fails when a
Stock mission is missing either field, which prevents new mission ids from
falling back into the first chapter.

Catalog generation contains curated SOL flow overrides when the mission design
needs structured generated behavior:

- crewed non-base missions can receive generated `RETURN_FROM_BODY` checks,
- rover missions receive `WHEEL_MOTION`,
- satellite networks can be normalized to relay-specific vessel-count checks,
- crewed orbit missions can receive generated apoapsis caps and a minimum
  half-day orbital hold,
- the first docking maneuver uses a generated recorded docking target,
- generated station, base and depot chains are placed into epochs by id/stage,
- orbital station build/expansion checks can require an empty uncrewed station
  with enough available seats; crew is counted from resupply onward,
- the Moon base is gated behind the 150-day, 3-Kerbal lunar station operation,
  while two early lunar base-site survey landings unlock after the
  Earth station reaches four seats and remain optional side missions,
- early asteroid-belt scouting can be placed in Red Horizon while Beltworks
  remains reserved for later belt landings, industry and crewed work,
- Phobos depot and Ceres landing progression intentionally skip unnecessary
  crewed-orbit prerequisites when the mission can start directly from the
  relevant landing/flyby setup,
- optional generated missions such as the removed Deimos fuel depot must be
  filtered in the generator, not by editing `.cfg` output.

The key learning from the 0.4 atlas work is that epoch assignment is a
presentation and pacing layer, not a dependency layer. For SOL, move missions
between epochs through the generator's `epoch_for_id` / `EPOCH_EXACT` rules. For
Stock, move missions by changing the mission's `epoche:` value in the Stock
design source. Do not change mission order or prerequisites unless the gameplay
flow itself is intentionally changing.

## Icons

Runtime icon loading goes through `IconLibrary`. Catalogs and fallback code may
still request legacy KSP tracking-station icon keys, but `IconLibrary.UI` maps
those keys to the bundled `icon_...` files first. This prevents stock or cached
textures with the same names from replacing the intended mission glyphs after
scene changes.

When adding a new mission icon:

- add the PNG under `GameData/CustomScienceContracts/Icons/UI/`,
- prefer a unique `icon_...` key,
- update `IconLibrary.MapUiIcon` if it replaces a legacy key,
- add the key to `tools/validate_catalog.py`.

## Validation

Run the full validation workflow after mission or generator changes:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
python3 tools/validate_catalog.py OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts
python3 tools/validate_design.py custom_science_contracts_stock_missionsdesign.md
python3 tools/validate_catalog.py OptionalConfigs/Stock/GameData/CustomScienceContracts/Contracts stock
```

The catalog validator checks:

- duplicate ids,
- dangling prerequisites,
- invalid `revealAllAfter` references,
- unsupported check kinds,
- missing SOL bodies,
- unknown icons,
- station keys without matching `recordStationKey`.

## Build

On this machine, use the explicit dotnet path:

```bash
/usr/local/share/dotnet/dotnet build -c Release \
  -p:KSPManaged="$HOME/Library/Application Support/Steam/steamapps/common/Kerbal Space Program/KSP.app/Contents/Resources/Data/Managed" \
  CustomScienceContracts.sln
```

The build copies `CustomScienceContracts.dll` into:

```text
GameData/CustomScienceContracts/Plugins/
```

## Release Packaging

Use the release script:

```bash
tools/make_release.sh
tools/make_release.sh --publish
```

The script reads the version from `src/CustomScienceContracts/Core/ModInfo.cs`,
validates catalogs, builds the shared DLL, packages the three ZIPs and, with
`--publish`, creates or updates the GitHub release. Keep `ModInfo.Version` and
`CustomScienceContracts.csproj` in sync.

The main release zip should contain:

```text
GameData/
README.md
LICENSE
THIRD_PARTY_NOTICES.md
LICENSES/
```

Release packaging deletes nested `*.zip` files from the copied `GameData`
folder before zipping, so old local artifacts do not get bundled into the
player download.

The optional Stock config zip should contain:

```text
GameData/CustomScienceContracts/Contracts/
README.md
```

The optional German SOL config zip should contain:

```text
GameData/CustomScienceContracts/Contracts/
README.md
```

The optional German zip is installed after the main mod and replaces only the
contract catalog files.

## 0.4 Learnings

- Keep code, comments and plugin UI in English; German player-facing contract
  text belongs in the German generated catalog.
- Do not hand-edit generated contract cfg files. Even small title, epoch or
  prerequisite changes should live in the design source or generator.
- UI layout should never imply dependencies that do not exist. Only draw arrows
  for actual prerequisites, and place optional side missions as parallel lanes.
- The campaign is now probe-first by design. Document this clearly in releases
  because the stock tech tree can unlock crewed options too early.
- KSP texture caching can surface wrong icons after scene changes. Unique
  bundled `icon_...` names plus an explicit runtime mapping are more reliable
  than relying on stock tracking-station names.
- Dense chapters should be thinned by epoch placement first, not by fake
  dependencies. Optional missions that unlock nothing should sit in parallel and
  should not emit outgoing atlas arrows.
- Infrastructure gates should match the campaign story: lunar bases should come
  after sustained lunar station operations, while direct small-body landings do
  not need an artificial crewed-orbit mission first.

## Legal Notes

The mod code is GPL-3.0. Some bundled image assets are third-party assets:

- ZTheme image assets, unmodified, GPL-3.0.
- Kerbal Planet Emblems image assets, unmodified, MIT.

No code from those projects is used. Do not claim that all assets are original
CustomScienceContracts artwork.
