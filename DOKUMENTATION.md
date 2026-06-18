# CustomScienceContracts Documentation

## Purpose

CustomScienceContracts is a KSP 1.12.x Science Mode plugin. It provides a
separate mission system that is independent from the stock career contract
system. Missions are selected in a custom UI, tracked through stock KSP state and
paid out as science bonuses when the player claims them.

The mod does not require Contract Configurator, Kerbalism, Simplex or specific
part packs.

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

Repeatable missions stay in their home branch until first completion. After that
they appear only in the repeatable branch and require two other mission
completions before they can be accepted again.

## Branches

The UI displays four branches:

- Pioneers: crewed progression.
- Robotic Explorers: probes, mapping, landers and flybys.
- Lifelines: communication networks, logistics and depots.
- Repeatable: repeatable missions after first completion.

The internal enum names are still German (`Bemannt`, `UnbemannteErkundung`,
`NetzwerkLogistik`, `Wiederholbar`). They are stable config/save keys and should
not be renamed without migration.

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
- Vessel/body state: `ORBIT_ABOVE`, `LANDED`, `SUBORBITAL`, `EVA`.
- Orbit details: `INCLINATION_MIN`, `VESSEL_COUNT`,
  `VESSEL_COUNT_INCLINATION`.
- Time: `HOLD`, `DURATION`.
- Events: `DOCK_ANY`, `DOCK_STATION`.
- Stateful goals: `FLYBY`, `MARKER_LANDING`, `RETURN_FROM_BODY`.
- Resources: `FUEL_MIN`, `RESOURCE_MIN`, `ORE_SURFACE`.
- Atmosphere: `ATMO_FRACTION`, `SUBORBITAL_ABOVE_ATMO`.

All body sizes, atmosphere heights and day lengths are read from the KSP API at
runtime. The code must not hardcode those values.

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
- timer state,
- flyby state,
- marker target state,
- registered station/base vessels,
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

Default English output, shipped in the main download:

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

The Stock KSP campaign is kept on its own English-only branch/track and is not
mixed into the SOL release catalog.

## Validation

Run the full validation workflow after mission or generator changes:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
python3 tools/validate_catalog.py OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts
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

The main release zip should contain:

```text
GameData/
README.md
LICENSE
THIRD_PARTY_NOTICES.md
LICENSES/
CHANGELOG.md
DOKUMENTATION.md
```

The optional German SOL config zip should contain:

```text
GameData/CustomScienceContracts/Contracts/
README.md
```

The optional German zip is installed after the main mod and replaces only the
contract catalog files.

## Legal Notes

The mod code is GPL-3.0. Some bundled image assets are third-party assets:

- ZTheme image assets, unmodified, GPL-3.0.
- Kerbal Planet Emblems image assets, unmodified, MIT.

No code from those projects is used. Do not claim that all assets are original
CustomScienceContracts artwork.
