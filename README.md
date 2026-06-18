# CustomScienceContracts

CustomScienceContracts is a Kerbal Space Program 1.12.x plugin for Science Mode.
It adds its own mission and objective system that runs alongside the stock game
without using Contract Configurator.

The current mission campaign is built for a SOL Quarter-Scale installation. It
starts with early uncrewed Earth tests and grows into crewed lunar and Martian
exploration, stations, bases, logistics chains, communication networks,
asteroid branches, outer-planet probes and late-game Titan infrastructure.

## Languages

The main release ships with the original German contract catalog.

An optional English contract catalog is published as a separate release asset.
Install the main mod first, then install the English contract pack over it to
replace only the mission text/config files:

```text
GameData/CustomScienceContracts/Contracts/
```

The plugin UI, repository documentation and release notes are English from
version 0.2.0 onward. Stock KSP contract configs are planned but not included yet.

## Installation

1. Download `CustomScienceContracts-vX.Y.Z.zip` from the release page.
2. Extract it into your Kerbal Space Program folder.
3. Start a Science Mode save.

The installed structure should look like this:

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

To use the English mission texts, download the matching
`CustomScienceContracts-EnglishContracts-vX.Y.Z.zip` asset and copy its
`GameData` folder over the main installation.

## Dependencies

Required for play:

- Kerbal Space Program 1.12.x.
- A SOL Quarter-Scale setup with matching internal body names such as `Earth`,
  `Moon`, `Mars`, `Jupiter`, `Saturn`, and so on.

Not required:

- Contract Configurator.
- Kerbalism.
- Simplex.
- Any part pack for mission checks.

The mod does not require specific parts. Antennas, scanners and payloads are
sometimes mentioned narratively, but the actual checks use stock KSP state only:
orbit, landing, crew, vessel count, resources, inclination, flyby state and time.

## What It Adds

- Four mission branches:
  - Pioneers
  - Robotic Explorers
  - Lifelines
  - Repeatable
- A mission control window for selecting missions.
- An active missions window with objective progress.
- Save-specific progress outside the `.sfs` file.
- Science rewards paid when completed missions are claimed.
- Repeatable logistics missions with a cooldown after first completion.

## Gameplay Notes

- Height requirements are minimum values. A mission that says `periapsis above
  2000 km` requires the periapsis to be greater than 2000 km.
- Polar missions require at least 75 degrees inclination.
- Communication networks count real vessels in orbit. Debris, flags, asteroids
  and deployed science objects do not count.
- Repeatable missions become available again after two other mission completions.

## Save Data

Progress is stored per save at:

```text
saves/<SaveName>/CustomScienceContracts/contracts_state.cfg
```

This file stores active missions, completed missions, timers, flyby state,
waypoint state, repeatable cooldowns and registered stations/bases.

## Development

The German mission design source is:

```text
custom_science_contracts_missionsdesign.md
```

Generated catalog files must not be edited by hand.

Common workflow:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
python3 tools/gen_catalog_en.py
python3 tools/validate_catalog.py OptionalConfigs/English/GameData/CustomScienceContracts/Contracts
./build.sh
```

See `DOKUMENTATION.md` for the architecture, runtime flow and release process.

## License and Third-Party Assets

CustomScienceContracts is licensed under the GNU General Public License version
3.0. See `LICENSE` and `LICENSES/GPL-3.0.txt`.

Some included image assets are third-party assets and are not claimed as original
CustomScienceContracts artwork:

- Unmodified image assets from ZTheme, licensed under GNU GPL v3.0.
- Unmodified image assets from Kerbal Planet Emblems, licensed under MIT.

No code from ZTheme or Kerbal Planet Emblems is used. The third-party assets were
not modified. See `THIRD_PARTY_NOTICES.md` for details.
