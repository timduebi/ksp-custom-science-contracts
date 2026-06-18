# Development — two editions, one engine

CustomScienceContracts ships in **two editions that share a single engine** (one compiled
`CustomScienceContracts.dll`). They differ only in the contract catalog:

| Edition | Bodies | Design source | Catalog in repo | Downloads |
|---------|--------|---------------|-----------------|-----------|
| **SOL** | real solar system | `custom_science_contracts_missionsdesign.md` | `GameData/CustomScienceContracts/Contracts/` (default) + `OptionalConfigs/SOL-German/` | English (main) + German (optional config) |
| **Stock** | stock KSP (Kerbin, Mun, …) | `custom_science_contracts_stock_missionsdesign.md` | `OptionalConfigs/Stock/` | English only |

Everything else — the engine in `src/`, icons, UI, validators, generators — is shared and
lives on **`main`**. There is intentionally no per-edition branch: a single branch means an
engine change is automatically part of both editions.

## Generators

- `tools/gen_catalog_en.py` → SOL English catalog into `GameData/.../Contracts` (the default).
- `tools/gen_catalog.py` → SOL German catalog into `OptionalConfigs/SOL-German/`.
- `tools/gen_catalog_stock.py` → Stock English catalog into `OptionalConfigs/Stock/`.

Never edit generated `*.cfg` by hand — change the design `.md` and regenerate.

## Validators (keep green)

- `tools/validate_design.py [doc]` — auto-detects the `stock` profile from the filename.
- `tools/validate_catalog.py [dir] [sol|stock]` — pass the `stock` profile when validating the
  Stock catalog (its body set differs).

## Building a release

`tools/make_release.sh <sol|stock> [--publish]` builds the shared DLL once, validates, packages
the edition's download(s), and optionally creates/updates the GitHub release (SOL is published
with `make_latest=true`, Stock with `make_latest=false`, so SOL keeps the "Latest" badge).

```
tools/make_release.sh sol      # CustomScienceContracts-SOL-vX.zip + -SOL-GermanConfig-vX.zip
tools/make_release.sh stock    # CustomScienceContracts-Stock-vX.zip
```

The version is read from `src/CustomScienceContracts/Core/ModInfo.cs` (keep it in sync with
`<Version>` in `CustomScienceContracts.csproj`).

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
5. **Regenerate + validate** both catalogs (run all three generators, both validators green).
6. **Release** both editions with `tools/make_release.sh sol` and `tools/make_release.sh stock`.

A change that touches `src/` but only one design doc is incomplete — it has shipped the engine
to both editions but only wired it into one.
