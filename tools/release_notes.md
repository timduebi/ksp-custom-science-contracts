## KSP Custom Science Contracts

A custom science-mode contract campaign for Kerbal Space Program. It adds its own progression of
crewed and robotic missions — flights, landings, returns, stations, bases, logistics and comm
networks — that runs alongside the stock science game without Contract Configurator.

---

### Download — required
**`CustomScienceContracts-v0.3.0.zip`** — the complete mod (plugin + the SOL real-solar-system
campaign).

1. Download and unzip.
2. Copy the `GameData` folder from the zip into your KSP install root, merging it into your
   existing `GameData/`.
3. You should end up with `Kerbal Space Program/GameData/CustomScienceContracts/` containing
   `Plugins/`, `Contracts/`, `Icons/` and `settings.cfg`.

That's a complete, playable install.

---

### Optional config swaps
Want a different campaign? These drop-in packs change **which missions you fly** without touching
the plugin. Each one replaces **only the four catalog files** in
`GameData/CustomScienceContracts/Contracts/` (`A_Pioniere.cfg`, `B_Spaeher.cfg`,
`C_Lebensadern.cfg`, `D_Stationen.cfg`). Install the main download first, then unzip one of these
into your KSP install root and **confirm overwriting** those four files.

- **Stock KSP system** — **`CustomScienceContracts-v0.3.0_Stock-Config.zip`**
  Rebuilds the whole campaign for the stock KSP solar system. Same mission structure, but flown to
  Kerbin, the Mun, Minmus, Duna, Jool, Laythe, Eve and the other stock bodies instead of the real
  solar system. Pick this if you play a stock (non-rescaled) game.

- **German** — **`CustomScienceContracts-v0.3.0_German-Config.zip`**
  The SOL real-solar-system campaign with all contract titles, descriptions and checklist labels
  in German. Same missions, prerequisites and rewards — just the language changes.

**Switching back or between configs:** re-extract the four `Contracts/*.cfg` from the main
download (or from another config pack). Only ever run **one** config at a time — the packs
overwrite the same four files.
