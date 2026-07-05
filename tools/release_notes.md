## Custom Science Contracts 0.6.1 — The Stations Branch

**Custom Science Contracts** gives KSP Science Mode a structured mission campaign:
robotic scouting, crewed milestones, stations, bases, supply routes, relays and
long-duration operations all feed into one progression.

0.6.1 gives the infrastructure program its own home: a new **Stations** branch
holds every station, base and depot chain, growing stage by stage across the
epochs instead of flooding a single chapter. On top of the 0.6.0 story release:
epoch intros, a mission chronicle, on-screen claim/unlock/epoch announcements
and a rebalanced campaign with a crewed finale.

### Important requirement

The default Sol campaign assumes a probe-first progression. Use a tech-tree or
progression mod that unlocks probes before crewed command pods/crew access;
otherwise the default campaign balance and early mission flow will not line up
correctly. The optional Stock Kerbol campaign is deliberately crewed-first and
does not require that probe-first setup.

### Main download

**`CustomScienceContracts-v0.6.1.zip`** — the complete mod with the default Sol campaign.

1. Download and unzip.
2. Copy the `GameData` folder into your KSP install root.
3. Start a Science Mode save and open Mission Control from the toolbar.

### Optional config swap

Install the main download first, then unzip this over it and overwrite the four
`GameData/CustomScienceContracts/Contracts/*.cfg` files:

- **Stock Kerbol system** — **`CustomScienceContracts-v0.6.1_Stock-Config.zip`**

The German Sol config is not shipped in this release; it stays maintained in the
repository and returns once the plugin UI is translatable as well.

### New in 0.6.1 — the Stations branch

- All station, base and depot chains moved into their own **Stations** category
  (station icon), shown between Robotic Explorers and Lifelines in the Campaign
  Atlas and as their own group in the Active Missions window.
- Expansion stages now sit in the epoch they belong to: the Earth station is
  founded in Orbital Roots and reaches its 12-seat final stage in Beltworks,
  the Mars base grows into Ringed Worlds, Kerbin's station upgrades stretch
  from Orbital Habits to Red Dust. Orbital Roots dropped from 45 to 21 missions
  — no more endless station chapter.
- Stations get their own pacing: up to 5 active missions and 3 visible per
  body, tunable via `activeStationen` / `visibleStationenPerSub`.
- Polished epoch intro panel (chapter kicker, large title, story text,
  progress bar) and a hard guarantee against clipped card texts.

### Highlights in 0.6.0

**The campaign tells its story**
- Every epoch opens with a narrative intro panel: chapter name, story text and
  your completion count.
- Completed missions show the in-game date of their first completion, and
  repeatables show how often you flew them — the atlas is now a chronicle of
  your space program.
- On-screen messages announce when a mission becomes claimable, what a claim
  paid and unlocked, and when you finish a whole epoch.
- New Sol finale: **Far Frontier Crown** — send three Kerbals through Neptune
  space and bring them home. Plus a new Venus relay network mission.

**Epochs rebalanced (Sol)**
- Every infrastructure chain lives in exactly one epoch now: Earth and Moon
  stations in *Orbital Roots*, Moon base and Earth fuel depot in *Inner Reach*,
  Mars station in *Red Horizon*, Mars base in *Beltworks*.
- Beltworks shrank from 51 to 37 missions; no epoch has backwards
  prerequisites anymore, and the uncrewed Eros flyby no longer hides behind a
  crewed milestone.

**Epochs rebalanced (Stock)**
- *Minty Operations* and *Deep-Space Lifeline* grew from 5/4 missions to 8/8;
  *The Purple Finale* grew from 2 to 6 — the Gilly fuel path and the Eve
  support station are now part of the finale chapter.
- Station, base and resupply chains moved from Lifelines to Pioneers, matching
  Sol. The crewed active-mission limit is now 4 (was 3) so a running station
  chain keeps a free slot.

**Quality of life & fixes**
- KSP-AVC support: a `.version` file ships with the mod, so AVC can announce
  updates.
- Skipping a mission now asks for an inline confirmation.
- Science rewards no longer book as "Cheating" in the R&D transaction log.
- Precision-landing markers cannot land in the ocean anymore (relevant for
  Kerbin/Laythe/Eve in the Stock campaign).
- Repeatables on cooldown render dimmed instead of green, cooldown texts and
  long descriptions no longer clip, and mission cards show clearer status
  lines throughout.
