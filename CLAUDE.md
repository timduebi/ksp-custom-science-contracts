# CLAUDE.md — working rules for this repo

CustomScienceContracts is a KSP science-mode contract mod with **two editions sharing one
engine**: **SOL** (real solar system) and **Stock** (stock KSP). Both live on `main`; there is
no per-edition branch. Read `DEVELOPMENT.md` for the full architecture.

## Always treat both editions together

- **The engine in `src/` is shared.** Any engine/feature change affects SOL and Stock at once.
- **When you add or change an engine feature, proactively propose how to use it in BOTH
  editions** — name concrete missions/bodies for SOL (e.g. Mars, Titan, Ganymede) *and* for
  Stock (e.g. Duna, Laythe, Eve) in the same response, before implementing. Do not wire a new
  feature into only one design doc.
- Follow the "Adding an engine feature" checklist in `DEVELOPMENT.md` (engine → validators →
  generators → both design docs → regenerate+validate both → release both).

## Catalogs are generated

- Never hand-edit `GameData/.../Contracts/*.cfg` or `OptionalConfigs/**/*.cfg`. Edit the design
  `.md` and run the generator:
  - SOL EN → `tools/gen_catalog_en.py` (`GameData/.../Contracts`)
  - SOL DE → `tools/gen_catalog.py` (`OptionalConfigs/SOL-German/`)
  - Stock EN → `tools/gen_catalog_stock.py` (`OptionalConfigs/Stock/`)
- Keep `tools/validate_design.py` and `tools/validate_catalog.py <dir> <sol|stock>` green.

## Building and releasing

- Build/package via **`tools/make_release.sh <sol|stock> [--publish]`** — it builds the shared
  DLL once and is the only supported release path. SOL publishes with `make_latest=true`, Stock
  with `make_latest=false` (SOL keeps GitHub's "Latest" badge).
- To compile without touching the live KSP install use the plain
  `dotnet build -c Release -p:KSPManaged=…`; `build.sh` additionally copies into the KSP
  GameData and is only for local in-game testing.
- The mod version lives in `src/CustomScienceContracts/Core/ModInfo.cs` and
  `CustomScienceContracts.csproj` `<Version>` — bump both together.

## ConfigNode trap

`{` and `}` are node delimiters in `.cfg`; never put them in `description=` values (truncates
the text). Use `%station%` as the station-name placeholder, not `{…}`.
