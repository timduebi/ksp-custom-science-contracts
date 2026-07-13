<p align="center">
  <img src="Logo.png" width="180" alt="Custom Science Contracts">
</p>

<h1 align="center">Custom Science Contracts</h1>

<p align="center"><b>Custom Science Contracts</b> finally gives KSP's Science Mode a clear thread to follow.</p>

---

Science Mode is great — but after the first few flights it can start to feel aimless. You collect science, unlock parts, and then you're left to invent the whole progression yourself.

Career Mode has contracts, but too many of them feel random, repetitive or disconnected from the space program you actually want to build.

**Custom Science Contracts is built to fix exactly that.**

It keeps the freedom of Science Mode and adds a structured campaign on top: first flights, orbit, robotic scouting, crewed missions, stations, bases, supply routes, resource missions, long-duration stays and deep-space exploration. Instead of random contracts, you follow a clear mission path — each step prepared by the last — so your space program feels like it's actually growing.

Everything runs through the mod's own in-game menu. Right inside KSP you can browse available missions, track the ones you're flying, review what you've completed and watch your overall campaign progress.

Custom Science Contracts is made for players who love Science Mode but want their save to feel like a real space program: less aimless grinding, fewer throwaway tasks, and more meaningful milestones.

## Mission Control

Everything is managed from the in-game **Mission Control** window:

- **Campaign Atlas** — the campaign timeline. Each epoch opens with a short story intro and a
  completion count, followed by branch rows (Pioneers, Robotic Explorers, Stations, Lifelines),
  mission cards grouped by celestial body and arrows for real prerequisites. Station, base and
  depot chains live in their own Stations branch, growing stage by stage across the epochs.
  Completed missions stay visible in green with the in-game date of their first completion —
  including repeatable missions, so the atlas is a full chronicle of everything you have achieved.
- **Repeatables** — one page with every unlocked repeatable mission (marked ↻), grouped by target
  body. Each card always shows whether it is ready or how many more missions you need to complete
  before it refreshes ("Available after 2 more missions"), with a progress bar and how often you
  have flown it.
- **Program Log** — every completed mission in chronological order with its in-game date, epoch
  and reward: your campaign as a flight log.
- **Active Missions** — a separate window (resizable) tracking accepted missions with live
  objective status, vessel assignment and claim/abort actions. Its toolbar icon shows a green
  dot while something is ready to claim.
- **On-screen updates** — short messages and a soft chime announce when a mission becomes
  claimable, what a claim paid out and unlocked, when a repeatable's cooldown finishes and when
  you complete a whole epoch (both can be turned off in the settings).
- **Settings** — science multiplier, UI scale for high-DPI displays, difficulty presets
  (Casual / Normal / Hard) and notification toggles. A "?" button in Mission Control explains
  every color and symbol.

## Features

- A dedicated science campaign for Science Mode
- Clear, realistic progression from your first flight to the outer solar system
- Combines the freedom of Science Mode with the structure of a campaign
- No random part-test contracts
- Missions with a clear thread and well-defined goals
- Robotic scouting before crewed missions
- Long space-station stays as preparation for interplanetary crews
- Science rewards for stations, bases, expansion and operation
- Meaningful infrastructure missions for supply, resources and networks
- Ore mining, resupply and mission networks as part of the progression
- No aimless science grinding
- Progression for the stock Kerbol system and the Sol system
- Especially recommended with Kerbalism

## Compatibility

- **Kerbal Space Program 1.12.x**, Science Mode.
- **Stock Kerbol system** — via the optional Stock config (see below).
- **[Sol](https://forum.kerbalspaceprogram.com/topic/229428-112x-sol-a-modern-recreation-of-our-home-system-at-real-quarter-and-stock-scale/)** — the main download is built for the Sol planet pack (a modern recreation of our home system at real, quarter and stock scale). It uses real body names like `Earth`, `Moon`, `Mars`, `Jupiter`, `Saturn` … and is tuned for the quarter-scale setup.
- **Required for the campaign balance:** use a tech-tree or progression mod that unlocks probes before crewed command pods/crew access. The mission flow now assumes robotic spacecraft come first and crewed missions open later.
- **Kerbalism is recommended** but not required: long missions, supply, life support, stations and bases gain even more meaning with it.
- **No** Contract Configurator and **no** specific part packs needed — objectives only check stock state (orbit, landing, crew, vessel count, resources, flyby, time …).

## Installation

1. Download **`CustomScienceContracts-vX.Y.Z.zip`** from the [releases page](https://github.com/timduebi/ksp-custom-science-contracts/releases/latest).
2. Unzip it and copy the `GameData` folder into your KSP install.
3. Start a Science Mode save.

You should end up with:

```text
Kerbal Space Program/
└── GameData/
    └── CustomScienceContracts/
        ├── Contracts/
        ├── Icons/
        ├── Plugins/
        │   └── CustomScienceContracts.dll
        ├── CustomScienceContracts.version
        └── settings.cfg
```

### Optional config swap

The main download ships with the Sol campaign. One optional config pack on the same release changes **which missions you fly**. It replaces only the four catalog files in `GameData/CustomScienceContracts/Contracts/`. Install the main download first, then unzip the pack from the **same release** over it and confirm overwriting.

- **`CustomScienceContracts-vX.Y.Z_Stock-Config.zip`** — rebuilds the whole campaign for the **stock Kerbol system** (Kerbin, the Mun, Minmus, Duna, Jool, Laythe, Eve …). For a stock, non-rescaled game.

To go back to the default, re-extract the four `Contracts/*.cfg` from the main download.

A German version of the Sol campaign is maintained in the repository but is currently not
shipped as a release asset — it returns once the plugin UI is translatable as well.

## Gameplay notes

- Altitude requirements are minimums (e.g. "periapsis above 2000 km" means greater than 2000 km).
- Polar missions require at least 75° inclination.
- Communication networks count real vessels in orbit — debris, flags, asteroids and deployed science objects do not count.
- Repeatable missions (↻) go on a short cooldown after each claim: complete two other missions and
  they become available again in the Repeatables tab. The card always shows the remaining count.
- Progress is stored per save at `saves/<Name>/CustomScienceContracts/contracts_state.cfg`.

## Feedback & bug reports

**Bug reports and suggestions are very welcome** — please open an issue on the [GitHub issue tracker](https://github.com/timduebi/ksp-custom-science-contracts/issues). Helpful details: KSP version, which config you run (Sol / Stock), the mission, and a `KSP.log` if you have it.

## Development

The campaigns are generated from design plans; the generated `.cfg` files are **never** edited by hand. Architecture, build and release workflow are documented in [`DEVELOPMENT.md`](DEVELOPMENT.md), with technical details in [`DOKUMENTATION.md`](DOKUMENTATION.md).

## License and third-party assets

Custom Science Contracts is licensed under the GNU General Public License 3.0 (see [`LICENSE`](LICENSE)). Some image assets are third-party and shipped unmodified: ZTheme (GPL-3.0) and Kerbal Planet Emblems (MIT). No code from those projects is used. Details in [`THIRD_PARTY_NOTICES.md`](THIRD_PARTY_NOTICES.md).
