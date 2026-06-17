# Changelog

All notable changes to CustomScienceContracts are documented here.
This project uses simple `MAJOR.MINOR.PATCH` versioning.

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
