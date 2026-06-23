## Custom Science Contracts 0.4.3

**Custom Science Contracts** gives KSP Science Mode a structured mission campaign:
robotic scouting, crewed milestones, stations, bases, supply routes, relays and
long-duration operations all feed into one progression.

> Alpha release: expect rough edges. Bug reports are welcome on GitHub; KSP version,
> active config, mission id/name and `KSP.log` are the most useful details.

### Important requirement

The default Sol campaign assumes a probe-first progression. Use a tech-tree or
progression mod that unlocks probes before crewed command pods/crew access;
otherwise the default campaign balance and early mission flow will not line up
correctly. The optional Stock Kerbol campaign is deliberately crewed-first and
does not require that probe-first setup.

### Main download

**`CustomScienceContracts-v0.4.3.zip`** — the complete mod with the default Sol campaign.

1. Download and unzip.
2. Copy the `GameData` folder into your KSP install root.
3. Start a Science Mode save and open Mission Control from the toolbar.

### Optional config swaps

Install the main download from the same release first, then unzip one of these
over it and overwrite the four `GameData/CustomScienceContracts/Contracts/*.cfg`
files. Use only one config swap at a time.

- **Stock Kerbol system** — **`CustomScienceContracts-v0.4.3_Stock-Config.zip`**
- **German Sol** — **`CustomScienceContracts-v0.4.3_Sol-German-Config.zip`**

### Highlights in 0.4.3

- Rebuilt the optional Stock Kerbol campaign from the revised mission design with
  explicit titles and per-mission epoch assignments.
- Stock Mission Control chapters now use the new Stock flow: First Sparks,
  Orbital Habits, Mun or Bust, Minty Operations, Inner Mischief, Red Dust,
  Deep-Space Lifeline, Jool Frontier and The Purple Finale.
- Added Stock repeatables for Kerbin station resupply, Kerbin fuel delivery, Mun
  orbital station resupply, Mun base supply, Duna orbit supply and Laythe base
  supply.
- Improved Stock infrastructure requirements: empty station construction and
  expansion checks, registered station docking for orbital deliveries, and Ore
  presence at fuel sites.
- Rover missions now validate actual wheeled surface movement at 4 m/s or faster
  in both Stock and Sol flows.
- Mission Control dependency layout now orders branch rows by local dependency
  flow and spreads multi-prerequisite arrow endpoints to reduce visual clutter.
- Stock design validation now fails if a mission is missing a title or explicit
  epoch, preventing new missions from silently falling into the first chapter.
