<p align="center">
  <img src="Logo.png" width="180" alt="Custom Science Contracts">
</p>

<h1 align="center">Custom Science Contracts</h1>

<p align="center"><b>A handcrafted campaign, mission tracker and program history for Kerbal Space Program 1 Science Mode.</b></p>

Custom Science Contracts (CSC) gives Science Mode the structure of a space program without turning it
into Career Mode. Instead of random contracts, the mod provides a designed campaign whose missions form
real prerequisite chains: early probes prepare crewed flights, crewed flights unlock infrastructure,
and stations, bases, relay networks and logistics make later exploration possible.

The campaign is managed through two native KSP launcher buttons: **Mission Control** for planning and
**Active Missions** for live objectives. CSC awards science, records the history of the program and keeps
its state inside the current save folder.

Version **0.8.0** includes 246 missions in the default SOL campaign and 118 missions in the optional
Stock campaign.

## What 0.8 changes

- Station certification is now part of construction, not an optional side mission.
- Every ordinary orbital-station build requires minimum mass, ElectricCharge storage and docking ports.
- Every station expansion rechecks those requirements and also requires a compatible science laboratory,
  beginning with the first expansion.
- Later station and base endurance stages now last 120 days instead of 60 days. The first major endurance
  milestone of each generated chain remains 150 days.
- The old optional Earth/Moon/Mars and Kerbin/Mun/Duna certification cards were removed; their engineering
  purpose is enforced directly by the main infrastructure path.
- Existing saves keep their completed missions, active missions, settings and program log. Historical bare
  state files written by earlier releases remain supported.

## Campaign editions

| Edition | Bodies | Missions | Recommended route | Distribution |
|---|---|---:|---:|---|
| **SOL English** | Earth, Moon, Mars and the real Solar System | 246 | 102 steps | Main download |
| **Stock English** | Kerbin, Mun, Minmus, Duna, Jool, Eve and the stock system | 118 | 65 steps | Optional overlay |
| **SOL German** | Same mission logic and ids as SOL English | 246 | 102 steps | Maintained in the repository; not currently a release asset |

The recommended route is guidance, not a second lock system. CSC calculates a complete, topologically
ordered route from the campaign milestones and every prerequisite needed to reach them. Mission Control
shows only the one mission currently recommended next. Side missions stay available whenever their own
requirements are met.

## Requirements and dependencies

### Required by CSC

| Requirement | Status | Details |
|---|---|---|
| **Kerbal Space Program 1.12.x** | Required | KSP 1.12.0 through 1.12.x; 1.12.5 is the primary target. |
| **Science Mode / Science Sandbox** | Required | The scenario module is registered only for new and existing Science saves. Career and Sandbox are not supported modes. |
| **Exactly one matching campaign profile** | Required | Use the default SOL catalog with SOL bodies, or install the Stock overlay for an unmodified stock system. Never mix the two catalogs. |
| **SOL planet pack** | Required for the main catalog only | The default missions use internal body names such as `Earth`, `Moon`, `Mars`, `Jupiter` and `Saturn`. Install [SOL and its own listed dependencies](https://forum.kerbalspaceprogram.com/topic/229428-112x-sol-a-modern-recreation-of-our-home-system-at-real-quarter-and-stock-scale/). The campaign is balanced around SOL quarter scale. |

CSC's plugin has **no hard dependency** on Contract Configurator, ModuleManager, Toolbar Controller,
Toolbar Continued, Kerbalism or a part pack. The release contains the only CSC assembly required:
`GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll`.

### Recommended progression setup

The campaign is deliberately probe-first. For the intended pacing, use a technology progression in which
probe cores become available before crewed command pods and broad crew access.

- [Community Tech Tree](https://github.com/ChrisAdderley/CommunityTechTree) is the reference extended tree.
- [Probes Before Crew](https://github.com/Moonlington/ProbesBeforeCrew) is the reference probe-first
  progression used for balance checks. If you install PBC, PBC itself requires Community Tech Tree and
  ModuleManager; those are PBC dependencies, not CSC dependencies.
- The 0.8 reward curve is validated against the stock tree plus Community Tech Tree and the literal
  `TechRequired` targets used by Probes Before Crew. CSC is meant to supplement experiment science, not
  replace every experiment in the tree.

### Optional companion mods

- [Kerbalism](https://github.com/Kerbalism/Kerbalism) is recommended, not required. Life support,
  reliability, radiation and background simulation give the long stays and infrastructure chains more
  weight. Install Kerbalism and its dependencies according to Kerbalism's own documentation.
- Compatible station laboratories are recognized through the module alternatives `ModuleScienceLab`,
  `ModuleScienceConverter` and `Laboratory`, covering stock-style and Kerbalism-style labs without a hard
  assembly reference.
- Part packs are optional. CSC checks capabilities and vessel state rather than requiring named parts.

### Explicitly not required

- **Contract Configurator:** CSC has its own catalog, state machine, objective evaluators and UI.
- **Toolbar Controller / Toolbar Continued:** both CSC buttons use KSP's stock `ApplicationLauncher`
  directly. Do not install Toolbar Controller just for CSC.
- **ModuleManager:** CSC does not use ModuleManager patches. It may still be required by another mod in
  your installation.

## Installation

### Clean installation

1. Download `CustomScienceContracts-v0.8.0.zip` from the
   [GitHub release](https://github.com/timduebi/ksp-custom-science-contracts/releases/latest).
2. Close KSP.
3. If upgrading from a very old or manually assembled copy, delete only
   `GameData/CustomScienceContracts`. Save progress is not stored there.
4. Open the ZIP and copy its `GameData` folder into the root of the KSP installation.
5. Start or load a **Science Mode** save.

The final layout must be:

```text
Kerbal Space Program/
└── GameData/
    └── CustomScienceContracts/
        ├── Contracts/
        │   ├── A_Pioniere.cfg
        │   ├── B_Spaeher.cfg
        │   ├── C_Lebensadern.cfg
        │   └── D_Stationen.cfg
        ├── Icons/
        ├── Plugins/
        │   └── CustomScienceContracts.dll
        ├── Sounds/
        ├── CustomScienceContracts.version
        └── settings.cfg
```

Do not create a second nested `GameData/GameData` directory, and do not move the DLL out of
`GameData/CustomScienceContracts/Plugins`.

### Installing the Stock campaign

1. Install the main `CustomScienceContracts-v0.8.0.zip` first; it contains the plugin and assets.
2. Download `CustomScienceContracts-v0.8.0_Stock-Config.zip` from the same release.
3. Copy its `GameData` folder over the KSP installation and confirm replacement of the four files in
   `GameData/CustomScienceContracts/Contracts`.

The Stock package is an overlay, not a standalone mod. It contains no DLL. To return to SOL, restore the
four contract files from the main ZIP. Do not leave Stock and SOL contract files side by side under other
names; KSP loads every `.cfg` in the folder.

### Updating an existing save

Updating from 0.7.x to 0.8.0 does not require a new save. CSC recognizes both the historical bare
`ConfigNode.Save` document and the explicitly wrapped state layout. Completed ids, active missions,
timers, vessel/fleet bindings, station records, settings and Program Log entries are retained.

The six removed optional certification mission ids are simply ignored if they exist in an older state
file. They were side missions and were never prerequisites, so removing them cannot break the campaign
graph. Newly accepted station construction and expansion missions use the 0.8 engineering rules.
Compatibility metadata keeps already-active legacy relay and delivery missions on the rules under which
they were accepted.

## In-game interface

### Mission Control

Mission Control is available in the Space Center, Tracking Station, VAB and SPH.

- **Campaign Atlas:** nine epochs, four fixed branches, body groups, mission cards and arrows representing
  actual prerequisites. Completed cards remain visible as history.
- **Recommended next:** one plain recommendation at the top. There is no route counter and recommended
  missions do not hide or disable side content.
- **Locked paths:** a locked card lists the shortest complete sequence of missions needed to unlock it.
- **Repeatables:** every unlocked repeatable appears in a dedicated view with its cooldown state, number
  of flights and remaining completions before it reopens.
- **Program Log:** an append-only chronological log of claims and skips, including universal time, actual
  science paid, vessel and crew snapshot. Imported pre-0.7 completions remain recorded without invented
  payouts.
- **Settings:** UI scale, notifications and independent Economy, Pacing and Operations difficulty axes.
- **Legend:** the `?` button explains colors, symbols and cooldown behavior.

### Active Missions

The Active Missions window is available in all supported launcher scenes, including flight. It shows each
accepted objective as live pass/fail state, supports vessel or fleet assignment and exposes claim and abort
actions. A green dot appears on its launcher icon while at least one mission is ready to claim.

Objectives becoming true changes a mission to **Ready to Claim**; rewards and unlocks are applied only
when the player claims it. This avoids accidental completion and makes the program log authoritative.

### Stock launcher placement

CSC registers both buttons with KSP's stock launcher lane through `AddApplication`, the same lane used by
stock applications. It deliberately does not use `AddModApplication`, which would place them in the
expandable mod drawer. If an old Toolbar Controller installation warning mentions CSC, remove stale files
from an older manual installation and reinstall the 0.8 ZIP cleanly.

## Mission structure

The four campaign branches are:

- **Pioneers:** crewed flights, EVAs, landings, returns and major exploration milestones.
- **Robotic Explorers:** sounding probes, satellites, orbiters, landers, rovers and robotic surveys.
- **Stations:** orbital stations, surface bases, depots, construction, expansion, resupply and endurance.
- **Lifelines:** relay networks and supporting logistics.

Visible-card limits keep each branch readable, but they do not change prerequisites. Completing visible
work advances the branch frontier and reveals queued missions. Active-mission limits are separate per
branch so a long station operation does not consume a robotic slot.

## Station engineering rules in 0.8

Station certification is mandatory and evaluated on the station vessel used by the construction or
expansion mission.

| Stage | Crew | Mass | Docking ports | ElectricCharge capacity | Laboratory | Stabilization |
|---|---|---|---|---|---|---|
| Initial orbital-station build | Uncrewed | `max(12, 8 + 2 × seats)` tonnes | `max(2, ceil(seats / 2))` | `max(1000, 250 × seats)` | Not yet required | 10 days |
| Every station expansion | Existing crew may remain | `max(18, 8 + 3 × seats)` tonnes | `max(2, ceil(seats / 2))` | `max(1000, 250 × seats)` | At least one compatible lab | 3 days |

Examples:

- A new three-seat station needs at least 14 t, two docking ports and 1000 ElectricCharge.
- Its four-seat first expansion needs at least 20 t, two docking ports, 1000 ElectricCharge and a lab.
- An eight-seat expansion needs at least 32 t, four docking ports, 2000 ElectricCharge and a lab.

Stock's Eve support station and Jool gateway retain their stricter authored power/docking requirements;
0.8 adds the missing mass baseline rather than weakening them. Fuel is not a requirement for ordinary
orbital stations. Fuel checks remain only where they are meaningful: dedicated depots, fuel stations and
surface-base logistics.

Generated station and base chains use a 150-day first endurance milestone. Every later stage that
previously used 60 days now requires 120 days. Expansion stabilization remains three days so the
engineering audit does not add another long empty waiting period.

## How objectives are evaluated

- **Orbit:** periapsis minimums can be paired with apoapsis caps; both must be true on the evaluated vessel.
- **Crew and capacity:** actual crew count and available seats are separate checks. Nearby EVA Kerbals count
  only within the intended vessel context.
- **Duration:** timers use in-game universal time and continue while the vessel is unloaded. Vessel identity
  survives scene changes and docking merges.
- **Return:** crewed return missions track the destination first and require at least one Kerbal who reached
  it to return safely to the named home body.
- **Precision landing:** deterministic per-save waypoints are recreated after scene changes; water-bearing
  worlds avoid ocean targets where the mission requires land.
- **Rovers:** landing alone is insufficient when wheel motion is required.
- **Relay networks:** only real relay-capable vessels count. With stock CommNet enabled, relays must have a
  live connection. Later networks require orbital phasing and an operational reserve, not just a raw count.
- **Resource delivery:** the acceptance-time stock is recorded. A logistics mission checks fresh resources
  actually delivered to the recorded target, including the docking merge for orbital deliveries.
- **Station capabilities:** mass, module alternatives, ElectricCharge capacity and docking-port count are
  evaluated from the actual vessel parts.

## Rewards, difficulty and repeatables

CSC pays science only. The paid amount is the catalog reward multiplied by the save's Economy setting.
The built-in presets are:

| Preset | Economy | Repeatable cooldown | Active limits |
|---|---:|---:|---|
| Casual | 1.3× science | 1 other completion | Higher |
| Normal | 1.0× science | 2 other completions | Default |
| Hard | 0.4× science | 3 other completions | Lower |

Economy, Pacing and Operations can be selected independently; mixed choices are stored as **Custom**.
The same settings are exposed in KSP's native Difficulty Options for Science saves.

A repeatable mission cannot immediately refresh itself. Other mission completions advance its cooldown,
and its card always states the remaining count.

## Save data and recovery

CSC stores save-specific state at:

```text
saves/<SaveName>/CustomScienceContracts/contracts_state.cfg
```

Writes are atomic. Before replacement, the prior valid state is retained as
`contracts_state.cfg.bak`; loading falls back to that backup if the primary file is malformed. The mod
does not write campaign state into `persistent.sfs`, and removing `GameData/CustomScienceContracts` does
not delete the save-folder state.

If progress appears missing, do not start skipping missions. Preserve both state files and attach them to
a bug report together with `KSP.log`.

## Troubleshooting

### No buttons appear

- Confirm the save is Science Mode.
- Confirm there is exactly one DLL at
  `GameData/CustomScienceContracts/Plugins/CustomScienceContracts.dll`.
- Search `KSP.log` for `[CSC]` startup diagnostics.
- Check that no duplicate old CSC folder exists elsewhere under `GameData`.

### Missions reference the wrong planets

The wrong catalog is installed. SOL must use the main catalog; a normal stock system must use the Stock
overlay. Replace all four files together.

### A station build will not complete

Expand the objective list and verify every independent property: orbit, capacity, mass, docking ports,
ElectricCharge and stabilization time. Expansions additionally require a recognized lab module. The
active or assigned vessel must be the station being certified.

### A relay count is high enough but the objective fails

Check enabled relay antennas, CommNet connectivity, assigned fleet membership, orbital separation and the
reserve satellite required by later topology missions.

### A delivery mission ignores resources already at the destination

That is intentional. The mission records the destination stock at acceptance and requires a new delivery;
it is not a snapshot inventory objective.

## Development and verification

Campaign `.cfg` files are generated artifacts and must not be edited by hand. Source and workflow:

- `custom_science_contracts_missionsdesign.md` — shared SOL design source.
- `custom_science_contracts_stock_missionsdesign.md` — Stock design source.
- `tools/gen_catalog_en.py`, `tools/gen_catalog.py`, `tools/gen_catalog_stock.py` — generators.
- `tools/validate_design.py`, `tools/validate_catalog.py` — structural and semantic validation.
- `tools/analyze_balance.py` — Stock + CTT + Probes Before Crew compatibility/reward report.
- `tests/` — Python policy tests and pure C# core-logic tests.
- `tools/release.py` — deterministic build, package and SHA-256 manifest.

See [DEVELOPMENT.md](DEVELOPMENT.md) for the complete contributor workflow and
[DOKUMENTATION.md](DOKUMENTATION.md) for architecture, persistence and check-engine details.

## Feedback and bug reports

Open an issue on the [GitHub issue tracker](https://github.com/timduebi/ksp-custom-science-contracts/issues)
and include:

- KSP version and operating system;
- SOL or Stock catalog;
- affected mission id/title;
- steps to reproduce;
- `KSP.log`;
- for progress problems, both `contracts_state.cfg` and `.bak`.

## License and third-party assets

Custom Science Contracts is licensed under GNU GPL 3.0; see [LICENSE](LICENSE) and
[LICENSES/GPL-3.0.txt](LICENSES/GPL-3.0.txt). Bundled image assets retain their respective licenses:
ZTheme (GPL-3.0) and Kerbal Planet Emblems (MIT). Details are in
[THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md). No third-party code is bundled.
