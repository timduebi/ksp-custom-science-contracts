// Declares a soft load-order dependency on the optional "ToolbarController" GameData mod, as
// required by the vendored ToolbarControl client library (see Vendor/ToolbarControl and
// THIRD_PARTY_NOTICES.md). This only affects load order and produces a load-time warning if
// missing/outdated; ToolbarControl itself falls back to the stock AppLauncher when the
// companion mod is not installed, so CustomScienceContracts works either way.
[assembly: KSPAssemblyDependency("ToolbarController", 1, 0)]
