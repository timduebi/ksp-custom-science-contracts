# Development — one engine, three catalogs, one release

CustomScienceContracts has **one shared engine** (one compiled `CustomScienceContracts.dll`) and
three interchangeable contract catalogs. They ship in a **single GitHub release**: the main
download is the engine + the default catalog, and the other catalogs are optional config overlays
that replace only `GameData/CustomScienceContracts/Contracts/*.cfg`.

| Catalog | Bodies | Design source | Catalog in repo | In the release |
|---------|--------|---------------|-----------------|----------------|
| **SOL** (default) | real solar system | `custom_science_contracts_missionsdesign.md` | `GameData/CustomScienceContracts/Contracts/` (default) | main download |
| **SOL German** | real solar system | same (German variant) | `OptionalConfigs/SOL-German/` | optional overlay |
| **Stock** | stock KSP (Kerbin, Mun, …) | `custom_science_contracts_stock_missionsdesign.md` | `OptionalConfigs/Stock/` | optional overlay |

Everything else — the engine in `src/`, icons, UI, validators, generators — is shared and lives on
**`main`** (no per-edition branch). The same DLL handles real-solar-system and stock bodies, so an
engine change applies to every catalog at once.

## Generators

- `tools/gen_catalog_en.py` → default SOL catalog into `GameData/.../Contracts`.
- `tools/gen_catalog.py` → German SOL catalog into `OptionalConfigs/SOL-German/`.
- `tools/gen_catalog_stock.py` → Stock catalog into `OptionalConfigs/Stock/`.

Never edit generated `*.cfg` by hand — change the design `.md` and regenerate.

Generated catalogs can still contain curated logic that is easier to express in
the generator than in every mission block. Examples from the SOL campaign:

- generated return checks for crewed non-base missions,
- generated rover wheel-motion checks,
- relay-specific network checks,
- generated crewed-orbit apoapsis caps and minimum half-day holds,
- generated station/base/depot chains,
- generated support missions such as the first docking target,
- optional generated side missions that are deliberately not prerequisites,
- epoch placement through `EPOCH_EXACT` / `epoch_for_id`.

Treat those generator overrides as source. If a generated mission should move,
disappear or change checks, update the generator and regenerate every affected
catalog.

## Validators (keep green)

- `tools/validate_design.py [doc]` — auto-detects the `stock` profile from the filename.
- `tools/validate_catalog.py [dir] [sol|stock]` — pass the `stock` profile when validating the
  Stock catalog (its body set differs).

## Building the release

`tools/make_release.sh [--publish]` builds the shared DLL once, validates all three catalogs,
packages all assets, and (with `--publish`) creates/updates the **single** GitHub release:

```
CustomScienceContracts-vX.zip                # main download: engine + default SOL catalog
CustomScienceContracts-vX_Sol-German-Config.zip  # optional overlay: German SOL catalog
CustomScienceContracts-vX_Stock-Config.zip   # optional overlay: stock KSP catalog
```

The overlays contain only `GameData/CustomScienceContracts/Contracts/*.cfg`. The release notes come
from `tools/release_notes.md` (placement instructions). Version is read from
`src/CustomScienceContracts/Core/ModInfo.cs` (keep it in sync with `<Version>` in
`CustomScienceContracts.csproj`).

The main ZIP is assembled from `GameData/` plus player-facing docs and licenses.
`tools/make_release.sh` removes nested `*.zip` files from the copied `GameData`
folder before packaging so stale local archives do not ship accidentally.

Release notes and README must call out gameplay assumptions that are not enforced
by code. Since v0.4.0, the important assumption is a probe-first tech progression:
players should use a tech-tree/progression mod that unlocks probes before crewed
command pods or crew access.

## Adding an engine feature — do it for BOTH editions

When you add or change engine behavior (e.g. a new `CheckKind`), complete this checklist so the
feature lands in both editions, and **propose concrete mission placements for SOL and Stock**:

1. **Engine:** implement in `src/` (e.g. add the enum to `Model/Check.cs`, evaluation in
   `Conditions/CheckEvaluation.cs`, the UI label in `UI/ActiveMissionsWindow.cs`, and body
   visuals in `UI/BodyVisual.cs`).
2. **Validators:** add the new `kind`/field to `tools/validate_catalog.py` (`CHECK_KINDS` and any
   field checks) and to `tools/validate_design.py` so both catalogs still validate.
3. **Generators:** teach the relevant generator(s) to emit the new check fields.
4. **Design docs:** add real usages to **both** `custom_science_contracts_missionsdesign.md`
   (SOL) **and** `custom_science_contracts_stock_missionsdesign.md` (Stock), choosing
   edition-appropriate bodies (e.g. Mars/Titan for SOL, Duna/Laythe for Stock).
5. **Regenerate + validate** all catalogs (run all three generators, every validator green).
6. **Release** with `tools/make_release.sh --publish` (one release, all assets rebuilt from the
   shared DLL).

A change that touches `src/` but only one design doc is incomplete — it has shipped the engine to
every catalog but only wired the feature into one.

## UI and catalog learnings from 0.4

- The Mission Control atlas is a presentation layer over real prerequisites. Do
  not use visual placement to imply campaign order; only actual prerequisites
  should produce arrows.
- Completed repeatables must remain visible in the Campaign Atlas as well as in
  the Repeatables pool. Station/base/depot resupply cards are prerequisites in
  the infrastructure chain; filtering them out after claim makes the chain look
  broken even though the catalog is valid.
- Rounded Bezier arrows are easier to read than right-angle connectors when the
  atlas has several cross-branch dependencies.
- Epoch assignment controls pacing and readability. Moving a mission between
  epochs should not change its prerequisites unless the gameplay dependency
  really changes.
- Same-body missions that do not depend on each other should share a dependency
  column and use vertical lanes, so optional work looks parallel.
- Locked cards should show what unlocks them, not full mission objectives.
  Objectives are useful once the mission is unlocked or accepted.
- Icon keys should be unique bundled assets where possible. `icon_...` files are
  preferred over KSP tracking-station names, and legacy keys should map through
  `IconLibrary` rather than depending on `GameDatabase` lookup order.
- Vessel identity is explicit runtime state. Single-vessel missions store an
  assigned persistent id in mission progress, network missions store an assigned
  fleet and completed network fleets are copied to `FLEET_RECORD` persistence so
  follow-up networks can inherit them.
