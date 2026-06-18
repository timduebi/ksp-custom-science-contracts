# AGENTS.md - CustomScienceContracts

## Project Goal

CustomScienceContracts is a C# plugin for Kerbal Space Program 1.12.x. It adds a
standalone mission/objective system for Science Mode, running independently from
the stock career contract system.

Completed missions grant a science bonus through:

```csharp
ResearchAndDevelopment.AddScience(amount, TransactionReasons.Cheating)
```

## Mission Sources

The SOL mission design source is:

```text
custom_science_contracts_missionsdesign.md
```

Generated default SOL catalogs live in:

```text
GameData/CustomScienceContracts/Contracts/*.cfg
```

Generated German SOL replacement catalogs live in:

```text
OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts/*.cfg
```

Never hand-edit generated `.cfg` files. Change the mission design or generator,
then regenerate and validate.

## Folder Structure

- `src/CustomScienceContracts/` - C# plugin source.
- `GameData/CustomScienceContracts/` - deployable mod folder with generated
  default SOL contracts, icons, plugin DLL and settings.
- `OptionalConfigs/SOL-German/` - optional German SOL replacement contract pack.
- `tools/` - design/catalog generators and validators.
- `custom_science_contracts_missionsdesign.md` - German campaign flow and
  mission source.
- `customScienceContracts Logo.png` - project logo/artwork, intentionally kept.
- `DOKUMENTATION.md` - architecture and workflow documentation.

## Build and Validation

Use the local KSP managed assemblies. On this machine `dotnet` is available at:

```bash
/usr/local/share/dotnet/dotnet
```

Build without copying into a live KSP install:

```bash
/usr/local/share/dotnet/dotnet build -c Release \
  -p:KSPManaged="$HOME/Library/Application Support/Steam/steamapps/common/Kerbal Space Program/KSP.app/Contents/Resources/Data/Managed" \
  CustomScienceContracts.sln
```

Design/catalog workflow:

```bash
python3 tools/validate_design.py
python3 tools/gen_catalog.py
python3 tools/validate_catalog.py
python3 tools/validate_catalog.py OptionalConfigs/SOL-German/GameData/CustomScienceContracts/Contracts
```

Do not treat `bin/`, `obj/`, generated DLLs or release archives as source.

## Runtime Rules

- Progress is persisted through `ScenarioModule` into:
  `saves/<save>/CustomScienceContracts/contracts_state.cfg`.
- UI is built with KSP `ApplicationLauncher` plus IMGUI windows.
- The check loop runs roughly once per second over active missions.
- Body sizes, atmosphere heights and day length must always come from the KSP API.
- No hardcoded body dimensions.
- No invented body names.

## Contract Flow

- `Locked`: prerequisites are missing.
- `Available`: prerequisites are complete and visibility rules allow display.
- `Active`: accepted and tracked.
- `ReadyToClaim`: objectives are complete; reward is not paid yet.
- `CompletedOnce`: claimed and counted as completed.

Repeatable missions move into the `Wiederholbar` pool after their first
completion. They become available again after two other mission completions.

## Hard Rules

- No part requirements.
- No dependency on Kerbalism or Simplex APIs.
- No hardcoded body sizes or atmosphere heights.
- No hand-editing generated contract cfg files.
- Keep the default SOL catalog and optional German SOL config in sync.

## Text and Style

The default shipped SOL contract catalog and the optional German SOL catalog are
generated from the same mission source and use the same contract ids,
prerequisites and checks; only the player-facing text differs.

Write code comments and the plugin UI in English. Contract enum names such as `Bemannt` and
`NetzwerkLogistik` are stable technical keys and must not be renamed without a
migration plan.
