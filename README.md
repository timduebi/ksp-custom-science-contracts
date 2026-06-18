<p align="center">
  <img src="Logo.png" width="180" alt="Custom Science Contracts">
</p>

<h1 align="center">Custom Science Contracts</h1>

<p align="center"><b>Custom Science Contracts</b> finally gives KSP's Science Mode a clear thread to follow.</p>

> ⚠️ **Work in progress — first alpha.** This is an early release. Expect rough edges, and bugs may
> show up. Bug reports and suggestions are very welcome — see [Feedback & bug reports](#feedback--bug-reports).

---

**Custom Science Contracts** adds its own structured science campaign to Science Mode.

Plain Science Mode gives you a lot of freedom, but it often feels aimless. You gather science, unlock parts and decide for yourself where to go next — but there is no real thread tying it together. Career Mode has contracts, yet they often feel random, out of place or annoying: part tests, throwaway jobs and missions with no real connection.

**Custom Science Contracts combines the best of both worlds:** the freedom and research focus of Science Mode with the clear mission structure of a campaign — but without the random, annoying Career-Mode jobs.

The mod walks you through your own science campaign step by step: from the first simple flights, through orbit, space stations, lunar bases and supply missions, all the way to a realistic expansion across the whole solar system. Every mission has a clear goal and is part of a progression that makes sense.

The campaign is deliberately realistic. You won't suddenly be sent to Jool while you've barely managed Duna. New targets are prepared logically: first robotic scouting, then orbital missions, then landings, longer stays and finally permanent infrastructure. Just like in real spaceflight, nobody lands somewhere without a robotic mission having been there first.

Crewed missions build on each other too. Before you send a crew to a distant target like Duna, you first have to prove that your space agency can support longer stays in space. Long space-station missions, for example, show that you've mastered life support, supply, crew rotation and sustained operations. Only then do bigger interplanetary steps become believable and worthwhile.

A special focus is on infrastructure. In the normal game, space stations, lunar bases or outposts are often optional building projects with no real gameplay purpose. **Custom Science Contracts turns them into an important part of your progression.** Building, expanding, operating, supplying and restocking resources are rewarded with science and finally give stations and bases a real purpose.

Resource and network missions are part of the campaign as well. You're not just sent somewhere to gather science — you gradually build up a working spaceflight infrastructure: orbital stations, bases, supply routes, ore mining and mission networks.

This solves several big problems of normal Science and Career gameplay: Science Mode is often too aimless, Career contracts are often too random, and infrastructure like stations or bases is normally barely necessary. **Custom Science Contracts gives your space program a clear direction, meaningful milestones and a realistic progression.**

The mod is made for players who enjoy Science Mode but want more structure, more purpose and a real campaign.

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
        └── settings.cfg
```

### Optional config swaps

The main download ships with the Sol campaign. Two optional config packs on the same release change **which missions you fly**. Each one replaces only the four catalog files in `GameData/CustomScienceContracts/Contracts/`. Install the main download first, then unzip one pack over it and confirm overwriting — run only **one** config at a time.

- **`CustomScienceContracts-vX.Y.Z_Stock-Config.zip`** — rebuilds the whole campaign for the **stock Kerbol system** (Kerbin, the Mun, Minmus, Duna, Jool, Laythe, Eve …). For a stock, non-rescaled game.
- **`CustomScienceContracts-vX.Y.Z_German-Config.zip`** — the Sol campaign with all contract text in **German**. Same missions, only the language changes.

To go back to the default, re-extract the four `Contracts/*.cfg` from the main download.

## Gameplay notes

- Altitude requirements are minimums (e.g. "periapsis above 2000 km" means greater than 2000 km).
- Polar missions require at least 75° inclination.
- Communication networks count real vessels in orbit — debris, flags, asteroids and deployed science objects do not count.
- Progress is stored per save at `saves/<Name>/CustomScienceContracts/contracts_state.cfg`.

## Feedback & bug reports

This is an early alpha and very much a work in progress, so things can break. **Bug reports and suggestions are very welcome** — please open an issue on the [GitHub issue tracker](https://github.com/timduebi/ksp-custom-science-contracts/issues). Helpful details: KSP version, which config you run (Sol / Stock / German), the mission, and a `KSP.log` if you have it.

## Development

The campaigns are generated from design plans; the generated `.cfg` files are **never** edited by hand. Architecture, build and release workflow are documented in [`DEVELOPMENT.md`](DEVELOPMENT.md), with technical details in [`DOKUMENTATION.md`](DOKUMENTATION.md).

## License and third-party assets

Custom Science Contracts is licensed under the GNU General Public License 3.0 (see [`LICENSE`](LICENSE)). Some image assets are third-party and shipped unmodified: ZTheme (GPL-3.0) and Kerbal Planet Emblems (MIT). No code from those projects is used. Details in [`THIRD_PARTY_NOTICES.md`](THIRD_PARTY_NOTICES.md).
