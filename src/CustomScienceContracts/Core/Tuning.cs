using System.Globalization;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Zentrale Stellschrauben aus AGENTS.md / Designplan. Defaults hier; zur Laufzeit aus
    /// GameData/CustomScienceContracts/settings.cfg (Node CUSTOM_SCIENCE_CONTRACTS_SETTINGS)
    /// ueberschreibbar. Fehlt die Datei/ein Wert, gilt der Default.</summary>
    public static class Tuning
    {
        public const string SettingsNode = "CUSTOM_SCIENCE_CONTRACTS_SETTINGS";

        // --- Sichtbarkeit (Available im Auswahlfenster) ---
        public static int VisibleBemanntBase = 3;          // bemannt: 3 sichtbar ...
        public static int VisibleBemanntBoosted = 5;       // ... 5 sobald >= 50 % aller bemannten erledigt
        public static float BemanntBoostFraction = 0.5f;
        public static int VisibleErkundungPerSub = 4;      // Erkundung: 4 pro Unterkategorie
        public static int VisibleNetzwerkPerSub = 3;       // Netzwerk: 3 pro Unterkategorie

        // --- Aktiv-Limits (gleichzeitig angenommen) ---
        public static int ActiveBemannt = 3;
        public static int ActiveErkundung = 10;
        public static int ActiveNetzwerk = 5;

        // --- Repeatable ---
        public static int RepeatableCooldown = 2;          // >= 2 andere Abschluesse bis wieder annehmbar

        // --- Marker (Default; einzelne Contracts ueberschreiben via radiusKm) ---
        public static double MarkerRadiusKmDefault = 15.0;
        public static double MarkerRadiusKmResupply = 5.0;  // Referenz; Contracts setzen radiusKm direkt

        // --- Pruef-Loop ---
        public static float CheckIntervalSeconds = 1.0f;

        // --- Vorschau gesperrter Missionen ---
        /// <summary>Ab Abschluss dieses Contracts wird je Koerper die naechste gesperrte Mission als
        /// ausgegraute Vorschau (mit roter Voraussetzungs-Zeile) angezeigt.</summary>
        public static string LockedPreviewTrigger = "cr_luna_flyby_crewed";

        // --- Diagnose ---
        public static bool VerboseLogging = false;
        /// <summary>Test-Schalter: schaltet alle Missionen frei und zeigt sie ohne Sichtbarkeitslimits.</summary>
        public static bool UnlockAll = false;

        private static bool _loaded;

        /// <summary>Laedt settings.cfg (einmalig). Werte, die fehlen, behalten ihren Default.</summary>
        public static void Load()
        {
            if (_loaded) return;
            _loaded = true;
            if (GameDatabase.Instance == null) return;

            ConfigNode[] nodes = GameDatabase.Instance.GetConfigNodes(SettingsNode);
            if (nodes == null || nodes.Length == 0) { Debug.Log("[CSC] Keine settings.cfg — Defaults aktiv."); return; }

            // Mehrere Nodes (mehrere Dateien): der Reihe nach anwenden, letzter gewinnt.
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
                MarkerRadiusKmResupply= GetD(n, "markerRadiusKmResupply", MarkerRadiusKmResupply);
                CheckIntervalSeconds  = GetF(n, "checkIntervalSeconds", CheckIntervalSeconds);
                LockedPreviewTrigger  = n.GetValue("lockedPreviewTrigger") ?? LockedPreviewTrigger;
                VerboseLogging        = GetB(n, "verboseLogging", VerboseLogging);
                UnlockAll             = GetB(n, "unlockAll", UnlockAll);
            }
            Debug.Log("[CSC] settings.cfg geladen.");
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
