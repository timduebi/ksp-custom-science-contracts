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
        public static int VisibleStationenPerSub = 3;      // stations: 3 per subcategory (body)
        public static int VisibleNetzwerkPerSub = 3;       // network/logistics: 3 per subcategory

        // --- Active limits ---
        public static int ActiveBemannt = 4;   // 4 since 0.6: keeps a crewed slot free besides ops
        public static int ActiveErkundung = 10;
        public static int ActiveStationen = 5; // station/base/depot chains incl. 150-day operations
        public static int ActiveNetzwerk = 5;

        // --- Repeatable ---
        public static int RepeatableCooldown = 2;          // >= 2 other completions before repeatable again

        // --- Markers (default; individual contracts may override through radiusKm) ---
        public static double MarkerRadiusKmDefault = 15.0;

        // --- Check loop ---
        public static float CheckIntervalSeconds = 1.0f;

        // --- Mission Control window ---
        public static float MissionCenterScale = 0.96f; // fraction of the screen used when opened
        /// <summary>Global scale for all mod windows (0.8-1.6); helps on 4K displays where
        /// legacy IMGUI ignores KSP's UI scale setting.</summary>
        public static float UiScale = 1.0f;

        // --- Notifications ---
        public static bool ShowToasts = true;   // on-screen messages (ready/claim/unlock/epoch)
        public static bool PlaySounds = true;   // soft chime when a mission becomes claimable

        // --- Difficulty preset ("custom" keeps the settings.cfg values) ---
        public static string Difficulty = "normal";

        // --- Diagnostics ---
        public static bool VerboseLogging = false;
        /// <summary>Test switch: unlocks all missions and ignores visibility limits.</summary>
        public static bool UnlockAll = false;
        /// <summary>Runs the logic self-test once at startup and logs PASS/FAIL lines.</summary>
        public static bool SelfTest = false;

        /// <summary>Applies a difficulty preset to cooldown and active limits, and mirrors the
        /// choice into KSP's native Difficulty Options (see <see cref="CscDifficultyParams"/>) so
        /// the mod's own settings window and the native screen never disagree. "custom" leaves
        /// cooldown/limits untouched (settings.cfg values keep applying); a name that is neither a
        /// known preset nor "custom" is ignored entirely.</summary>
        public static void ApplyDifficulty(string name)
        {
            switch (name)
            {
                case "casual":
                    RepeatableCooldown = 1;
                    ActiveBemannt = 5; ActiveErkundung = 12; ActiveStationen = 6; ActiveNetzwerk = 6;
                    break;
                case "normal":
                    RepeatableCooldown = 2;
                    ActiveBemannt = 4; ActiveErkundung = 10; ActiveStationen = 5; ActiveNetzwerk = 5;
                    break;
                case "hard":
                    RepeatableCooldown = 3;
                    ActiveBemannt = 3; ActiveErkundung = 8; ActiveStationen = 4; ActiveNetzwerk = 4;
                    break;
                case "custom":
                    break;
                default:
                    return;
            }
            Difficulty = name;
            try
            {
                var p = HighLogic.CurrentGame?.Parameters?.CustomParams<CscDifficultyParams>();
                if (p != null) p.difficulty = name;
            }
            catch (System.Exception) { }
        }

        /// <summary>Science multiplier for a difficulty preset, or null for "custom"/unknown
        /// (meaning: leave whatever multiplier is currently set).</summary>
        public static float? ScienceMultiplierFor(string name)
        {
            switch (name)
            {
                case "casual": return 1.3f;
                case "normal": return 1.0f;
                case "hard": return 0.4f;
                default: return null;
            }
        }

        /// <summary>Re-applies the difficulty preset when it was changed through KSP's native
        /// Difficulty Options (new-game creation or the in-game pause-menu settings) since we
        /// last checked. Cheap enough to poll every check-loop tick.</summary>
        public static void SyncFromGameParameters(ContractManager mgr)
        {
            if (HighLogic.CurrentGame == null) return;
            var p = HighLogic.CurrentGame.Parameters?.CustomParams<CscDifficultyParams>();
            if (p == null || p.difficulty == Difficulty) return;
            ApplyDifficulty(p.difficulty);
            float? mult = ScienceMultiplierFor(p.difficulty);
            if (mult.HasValue && mgr != null) mgr.ScienceMultiplier = mult.Value;
        }

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
                VisibleStationenPerSub= GetI(n, "visibleStationenPerSub", VisibleStationenPerSub);
                VisibleNetzwerkPerSub = GetI(n, "visibleNetzwerkPerSub", VisibleNetzwerkPerSub);
                ActiveBemannt         = GetI(n, "activeBemannt", ActiveBemannt);
                ActiveErkundung       = GetI(n, "activeErkundung", ActiveErkundung);
                ActiveStationen       = GetI(n, "activeStationen", ActiveStationen);
                ActiveNetzwerk        = GetI(n, "activeNetzwerk", ActiveNetzwerk);
                RepeatableCooldown    = GetI(n, "repeatableCooldown", RepeatableCooldown);
                MarkerRadiusKmDefault = GetD(n, "markerRadiusKmDefault", MarkerRadiusKmDefault);
                CheckIntervalSeconds  = GetF(n, "checkIntervalSeconds", CheckIntervalSeconds);
                MissionCenterScale    = Mathf.Clamp(GetF(n, "missionCenterScale", MissionCenterScale), 0.55f, 1.0f);
                UiScale               = Mathf.Clamp(GetF(n, "uiScale", UiScale), 0.8f, 1.6f);
                ShowToasts            = GetB(n, "showToasts", ShowToasts);
                PlaySounds            = GetB(n, "playSounds", PlaySounds);
                VerboseLogging        = GetB(n, "verboseLogging", VerboseLogging);
                UnlockAll             = GetB(n, "unlockAll", UnlockAll);
                SelfTest              = GetB(n, "selfTest", SelfTest);
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
