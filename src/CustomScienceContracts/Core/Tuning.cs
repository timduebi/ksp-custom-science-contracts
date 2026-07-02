using System.Globalization;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Central tuning values from the mission design. Defaults live here and can
    /// be overridden at runtime through GameData/CustomScienceContracts/settings.cfg
    /// (CUSTOM_SCIENCE_CONTRACTS_SETTINGS node). Missing files or values keep the defaults.</summary>
    public static class Tuning
    {
        public const string SettingsNode = "CUSTOM_SCIENCE_CONTRACTS_SETTINGS";

        // --- Visibility (available in the selection window) ---
        public static int VisibleBemanntBase = 3;          // crewed: 3 visible initially
        public static int VisibleBemanntBoosted = 5;       // 5 once >= 50 % of crewed missions are done
        public static float BemanntBoostFraction = 0.5f;
        public static int VisibleErkundungPerSub = 4;      // exploration: 4 per subcategory
        public static int VisibleNetzwerkPerSub = 3;       // network/logistics: 3 per subcategory

        // --- Active limits ---
        public static int ActiveBemannt = 4;   // 4 since 0.6: station chains need a slot besides missions
        public static int ActiveErkundung = 10;
        public static int ActiveNetzwerk = 5;

        // --- Repeatable ---
        public static int RepeatableCooldown = 2;          // >= 2 other completions before repeatable again

        // --- Markers (default; individual contracts may override through radiusKm) ---
        public static double MarkerRadiusKmDefault = 15.0;

        // --- Check loop ---
        public static float CheckIntervalSeconds = 1.0f;

        // --- Mission Control window ---
        public static float MissionCenterScale = 0.96f; // fraction of the screen used when opened

        // --- Diagnostics ---
        public static bool VerboseLogging = false;
        /// <summary>Test switch: unlocks all missions and ignores visibility limits.</summary>
        public static bool UnlockAll = false;

        private static bool _loaded;

        /// <summary>Loads settings.cfg once. Missing values keep their defaults.</summary>
        public static void Load()
        {
            if (_loaded) return;
            _loaded = true;
            if (GameDatabase.Instance == null) return;

            ConfigNode[] nodes = GameDatabase.Instance.GetConfigNodes(SettingsNode);
            if (nodes == null || nodes.Length == 0) { Debug.Log("[CSC] No settings.cfg found, using defaults."); return; }

            // Multiple nodes/files: apply in order, last value wins.
            foreach (var n in nodes)
            {
                VisibleBemanntBase    = GetI(n, "visibleBemanntBase", VisibleBemanntBase);
                VisibleBemanntBoosted = GetI(n, "visibleBemanntBoosted", VisibleBemanntBoosted);
                BemanntBoostFraction  = GetF(n, "bemanntBoostFraction", BemanntBoostFraction);
                VisibleErkundungPerSub= GetI(n, "visibleErkundungPerSub", VisibleErkundungPerSub);
                VisibleNetzwerkPerSub = GetI(n, "visibleNetzwerkPerSub", VisibleNetzwerkPerSub);
                ActiveBemannt         = GetI(n, "activeBemannt", ActiveBemannt);
                ActiveErkundung       = GetI(n, "activeErkundung", ActiveErkundung);
                ActiveNetzwerk        = GetI(n, "activeNetzwerk", ActiveNetzwerk);
                RepeatableCooldown    = GetI(n, "repeatableCooldown", RepeatableCooldown);
                MarkerRadiusKmDefault = GetD(n, "markerRadiusKmDefault", MarkerRadiusKmDefault);
                CheckIntervalSeconds  = GetF(n, "checkIntervalSeconds", CheckIntervalSeconds);
                MissionCenterScale    = Mathf.Clamp(GetF(n, "missionCenterScale", MissionCenterScale), 0.55f, 1.0f);
                VerboseLogging        = GetB(n, "verboseLogging", VerboseLogging);
                UnlockAll             = GetB(n, "unlockAll", UnlockAll);
            }
            Debug.Log("[CSC] settings.cfg loaded.");
        }

        private static int GetI(ConfigNode n, string k, int def) =>
            int.TryParse(n.GetValue(k), NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : def;
        private static float GetF(ConfigNode n, string k, float def) =>
            float.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out float r) ? r : def;
        private static double GetD(ConfigNode n, string k, double def) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : def;
        private static bool GetB(ConfigNode n, string k, bool def) =>
            bool.TryParse(n.GetValue(k), out bool r) ? r : def;
    }
}
