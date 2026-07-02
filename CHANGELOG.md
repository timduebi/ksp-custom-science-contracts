# Changelog

All notable changes to CustomScienceContracts are documented here.
This project uses simple `MAJOR.MINOR.PATCH` versioning.

The mod ships as one download with the SOL (real solar system) campaign, plus optional config
packs on the same release that swap the catalog to the stock KSP system or to German.

## [Unreleased]

### Changed
- SOL: the entire Earth orbital station chain (3-seat build through the 12-seat
  operations, including supplies and long stays) now lives in the Orbital Roots
  epoch instead of being spread across Moonrise, Inner Reach and Beltworks.

### Fixed
- Repeatables view: missions waiting for their cooldown (or a free branch slot)
  are now rendered dimmed instead of green, so "waiting" and "ready" are
  distinguishable at a glance. The Campaign Atlas keeps them green as completed
  history.
- Mission card texts are no longer cut off: wrapped text heights get a safety
  margin against IMGUI's wrap measurement, status lines size themselves to
  their content, and the body label no longer truncates long names.

## [0.5.0] - 2026-07-02

First stable release — the alpha label is gone.

### Added
- Repeatable cards now always show their cooldown state directly on the card in
  the Repeatables view: "Available after N more missions" with a progress bar,
  plus clear active/ready/slot-limit states.
- Repeatable missions carry a ↻ badge everywhere so they are recognizable at a
  glance.
- Epoch tabs show a per-epoch completion progress bar, a checkmark once an
  epoch is fully completed, and the count of currently acceptable missions.
- Mission Control now opens on the epoch where the campaign currently is
  (first epoch with claimable, active or acceptable missions).
- Mission Control can be dragged by its title bar (in addition to resizing).

### Changed
- The Campaign Atlas now doubles as a campaign timeline: repeatable missions
  stay permanently visible there after their first completion and are shown as
  completed (green), including while they cycle through the repeatable pool.
  Accepting them again happens exclusively from the Repeatables view.
- The Repeatables view dropped the epoch split entirely: one page lists the
  whole pool grouped by target body in atlas order, so every repeatable is
  findable without paging through epochs.
- Expanded mission cards show their objectives directly instead of behind a
  second "Objectives" foldout click.
- Epoch tabs wrap into a second row on narrow windows instead of running off
  the edge, so later epochs stay reachable at every window size.
- Campaign counts (mode tab, epoch tabs, branch headers) no longer include
  pool repeatables; those are counted in the Repeatables tab.
- Skipping a mission from the settings window now advances repeatable
  cooldowns exactly like claiming it, since a skip also counts as a completion.
- Unified close-button glyphs and cooldown wording across all windows.

### Fixed
- The check loop no longer risks a NullReferenceException when the vessel list
  is unavailable in a scene.
- Removed dead settings (`activeButtonX/Y/Size`, `lockedPreviewTrigger`,
  `markerRadiusKmResupply`) and unused code paths; per-frame UI work was
  reduced by caching catalog indexes and epoch names.

## [0.4.4] - 2026-06-23

### Added
- Added explicit vessel assignment for active single-vessel missions and
  assigned satellite fleets for network missions.
- Added completed network fleet persistence so follow-up network missions can
  inherit the predecessor satellites.

### Fixed
- Repeatable station, base and depot resupply missions now remain visible in the
  Campaign Atlas after first completion instead of disappearing from every
  infrastructure chain.
- Registered station ids, assigned vessels and fleet members now follow docking
  merge survivors, keeping station resupply and long-duration tracking attached
  after docking.
- Crew capacity and relay checks now work against unloaded/on-rails vessels more
  reliably.

## [0.4.1] - 2026-06-22

### Added
- Added two optional lunar base-site survey landings during the Earth station
  phase.

### Changed
- Moved the Moon base gate behind the 150-day, 3-Kerbal lunar station
  operation so surface bases follow sustained orbital infrastructure, while the
  early site-survey landings stay optional.
- Refined station progression so Earth and Moon orbital stations start at three
  seats, build/expansion checks require an empty uncrewed station with enough
  seats, and crew is required from resupply onward.
- Added generated apoapsis caps and minimum 0.5-day holds for crewed orbit
  missions.
- Removed the crewed Phobos and Deimos orbit missions.
- Split early asteroid-belt scouting into Red Horizon and kept Beltworks focused
  on later belt landings, industry and crewed operations.
- Removed unnecessary crewed-orbit prerequisites from the Phobos depot and Ceres
  landing path.
- Replaced hard-corner atlas dependency lines with rounded arrow connectors for
  clearer crossings.

## [0.4.0] - 2026-06-22

### Added
- Added the fullscreen-style Mission Control atlas with epoch pages, branch rows,
  dependency arrows, cross-epoch unlock hints and expandable objectives.
- Added a resizable Mission Control window and a settings slider for its initial
  size.
- Added assigned docking-target flow for the first docking maneuver.
- Added stricter objective checks for rover movement, relay satellite networks
  and crew returns on non-base crewed missions.

### Changed
- Rebalanced the Sol campaign around a probe-first progression. Use a tech-tree
  or progression mod that unlocks probes before crewed command pods/crew access.
- Updated the Moon crew requirements, Moon mission titles, Mars/Phobos epoch
  placement and Phobos fuel-depot flow.
- Made Mission Control backgrounds more opaque.
- Mission icons now prefer the bundled `icon_...` files to avoid stock/cached
  tracking-station icon mixups.

### Removed
- Removed the Deimos fuel-depot mission from the generated Sol catalogs.

## [0.3.0] - 2026-06-18

### Added
- Added crew-return objectives for SOL crewed landings on the Moon, Mars,
  Phobos, Deimos, Ceres, Ganymede and Titan. Explicit base-building missions are
  excluded so long-running base progression stays manageable.
- Added crew-return objectives for the crewed Venus and Mars flybys.
- Added a stateful `RETURN_FROM_BODY` check that can track a destination landing
  or flyby first, then require safe crew recovery on Earth.
- Added the **Stock KSP** config pack (`OptionalConfigs/Stock/`): the full campaign
  rebuilt for the stock system — 76 missions across Pioneers (crewed), Robotic Explorers
  and Lifelines, with a Laythe-then-Eve landing-and-return finale.
- Added the optional **German** SOL config pack under `OptionalConfigs/SOL-German/`.

### Changed
- The mod now ships as a single release: the main download is the SOL campaign, and the
  Stock and German catalogs are optional config packs that replace only the four
  `Contracts/*.cfg`.
- Planet and moon body icons are color-tinted per body in the UI, so shared base
  icons can still appear in the correct body colors.

## [0.2.0] - 2026-06-17

### Added
- Added an optional English contract catalog pack. It replaces mission titles,
  descriptions, subcategories and checklist labels while keeping the same
  contract ids, prerequisites, rewards and objective logic.
- Added generation and validation support for the English catalog under
  `OptionalConfigs/English/`.

### Changed
- Translated the plugin UI, repository documentation and release-facing text to
  English.
- The main release still ships with the original German contract catalog. The
  English contract catalog is distributed as a separate optional download.
- Bumped the mod version to 0.2.0.

## [0.1.4] - 2026-06-17

### Fixed
- Mission icons are now chosen more reliably in the mission list and active
  mission window. Station, crewed-vessel, relay, EVA and atmospheric missions
  should no longer unexpectedly show the generic probe icon.
- Icon names from the mission catalog are cleaned up while loading, so small
  formatting mistakes in config values do not break the displayed icon.

### Notes
- The mod continues to use the square UI icon files bundled in
  `GameData/CustomScienceContracts/Icons/UI`.

## [0.1.3] - 2026-06-17

### Fixed
- Moved the active-missions button back into the stock app-launcher column so it
  no longer floats near the top middle of the screen.
- Precision-landing waypoints are now only created in Flight/Map/Tracking and
  are recreated after scene changes, while the v0.1.2 all-scene timer evaluation
  remains intact.
- Mission icons no longer permanently fall back to probe icons when a texture is
  temporarily null or destroyed during scene changes; icon textures are reloaded
  from disk as needed and missing icon keys are reported via verbose logging.

## [0.1.2] - 2026-06-17

### Added
- Robotic **rover precision landings** for the Moon (`un_luna_rover`) and Mars
  (`un_mars_rover`). Completing them is now required before the crewed landing on
  that body.
- Robotic **polar precision landings** for the Moon, Mars and Titan
  (`un_luna_polar_landing`, `un_mars_polar_landing`, `un_titan_polar_landing`),
  unlocked after the matching polar mapping (optional side branches).
- The mod version is embedded in the build and shown in the log and the settings
  window.

### Changed
- Precision-landing targets are now randomized per save (no longer the same spot
  every playthrough) and constrained to within ±15° of the equator; polar landings
  target latitudes ≥70°.
- The "active missions" button now sits at the top of the screen (near the resource
  display) instead of in the stock app-launcher cluster; position is configurable in
  `settings.cfg`.
- The crewed Mars landing is now gated behind the rover landing (`un_mars_rover`)
  instead of `un_mars_precision_landing` (which remains as a separate robotic
  precision landing).

## [0.1.1] - 2026-06-17

### Fixed
- The active-missions window showed the same icon twice per mission (the sparte
  glyph in the group header plus the mission-card icon). The group header now
  shows only the colored sparte bar, so each mission displays a single
  descriptive icon (rover / kerbal / probe / station …).
- Duration and hold timers (`DURATION` / `HOLD`) no longer reset when you leave
  the tracked vessel:
  - Going on EVA near the vessel keeps counting — nearby EVA crew now counts
    toward crew checks.
  - Switching focus to another vessel or changing scene no longer resets the
    timer; the bound vessel is tracked in the background by its persistent ID.
  - Docking (e.g. resupplying a station) re-binds the timer to the merged
    vessel instead of stalling it.

### Changed
- Timers are now evaluated in all mod scenes (Space Center, Editor, Flight,
  Tracking Station) using in-game time. Time does not advance in the editor
  (the universe clock is frozen there), as expected.

### Added
- The release archive now bundles `LICENSE`, `THIRD_PARTY_NOTICES.md` and the
  `LICENSES/` folder (GPL-3.0 and the Kerbal Planet Emblems MIT text), as
  required for the bundled third-party image assets.

## [0.1.0] - 2026-06

- First packaged release of CustomScienceContracts.
- `GameData/CustomScienceContracts` with the compiled plugin DLL.
- Generated mission catalogs, UI / app / body icons, `settings.cfg`.
- README with installation notes and dependencies.
- Runtime dependencies: KSP 1.12.x and a SOL Quarter-Scale environment with
  matching internal body names. No Contract Configurator, Kerbalism, Simplex or
  part-pack dependency required.
