# Changelog

All notable changes to CustomScienceContracts are documented here.
This project uses simple `MAJOR.MINOR.PATCH` versioning.

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
