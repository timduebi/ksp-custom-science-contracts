using System;
using System.Globalization;
using System.IO;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Persistence
{
    /// <summary>Reads/writes runtime state as an editable file in the save folder:
    /// saves/&lt;save&gt;/CustomScienceContracts/contracts_state.cfg. This file is authoritative;
    /// if it is missing or broken, state is seeded fresh from the catalog.</summary>
    public static class SaveFolderStore
    {
        private const string RootNode = "CUSTOM_SCIENCE_CONTRACTS_STATE";
        private const string FileName = "contracts_state.cfg";
        private const string SubDir = "CustomScienceContracts";
        public const int CurrentVersion = StatePersistencePolicy.CurrentVersion;

        public static string SaveDir =>
            Path.Combine(KSPUtil.ApplicationRootPath, "saves", HighLogic.SaveFolder ?? "default", SubDir);

        public static string StateFilePath => Path.Combine(SaveDir, FileName);

        public static void Save(ContractManager mgr)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                var root = new ConfigNode(RootNode);
                root.AddValue("version", CurrentVersion);
                root.AddValue("scienceMultiplier", mgr.ScienceMultiplier.ToString("R", CultureInfo.InvariantCulture));
                root.AddValue("unlockAll", Tuning.UnlockAll);
                root.AddValue("showToasts", Tuning.ShowToasts);
                root.AddValue("playSounds", Tuning.PlaySounds);
                root.AddValue("uiScale", Tuning.UiScale.ToString("R", CultureInfo.InvariantCulture));
                root.AddValue("missionCenterScale", Tuning.MissionCenterScale.ToString("R", CultureInfo.InvariantCulture));
                root.AddValue("difficulty", Tuning.Difficulty);
                root.AddValue("economyDifficulty", Tuning.EconomyDifficulty);
                root.AddValue("pacingDifficulty", Tuning.PacingDifficulty);
                root.AddValue("operationsDifficulty", Tuning.OperationsDifficulty);
                foreach (var c in mgr.Catalog.All)
                {
                    var n = root.AddNode("STATE");
                    n.AddValue("id", c.Id);
                    n.AddValue("status", c.Status.ToString());
                    n.AddValue("totalCompletions", c.TotalCompletions);
                    n.AddValue("completionsSinceLastClaim", c.CompletionsSinceLastClaim);
                    if (c.FirstCompletedUT >= 0.0)
                        n.AddValue("firstCompletedUT", c.FirstCompletedUT.ToString("R", CultureInfo.InvariantCulture));
                    // ReadyToClaim is evaluated state too: dropping its progress used to lose
                    // return/flyby evidence and vessel bindings across a save/reload.
                    if (StatePersistencePolicy.PersistProgress(c.Status) &&
                        c.Progress != null && c.Progress.CountValues + c.Progress.CountNodes > 0)
                        n.AddNode(c.Progress.CreateCopy());
                }
                foreach (var entry in mgr.CompletionLog.Entries)
                {
                    var n = root.AddNode("COMPLETION");
                    n.AddValue("sequence", entry.Sequence);
                    n.AddValue("id", entry.MissionId);
                    n.AddValue("ut", entry.UniversalTime.ToString("R", CultureInfo.InvariantCulture));
                    n.AddValue("action", entry.Action);
                    n.AddValue("hasScience", entry.HasScience);
                    if (entry.HasScience) n.AddValue("science", entry.Science.ToString("R", CultureInfo.InvariantCulture));
                    if (entry.Imported) n.AddValue("imported", true);
                    if (!string.IsNullOrEmpty(entry.VesselName)) n.AddValue("vessel", entry.VesselName);
                    if (!string.IsNullOrEmpty(entry.Crew)) n.AddValue("crew", entry.Crew);
                }
                mgr.Stations.Save(root);
                mgr.Fleets.Save(root);

                // Write, parse and validate a sibling temporary file first. File.Replace then
                // swaps it atomically and makes the old valid state the backup in one operation.
                string temp = StateFilePath + ".tmp";
                if (File.Exists(temp)) File.Delete(temp);
                root.Save(temp);
                if (!TryReadRoot(temp, out ConfigNode written, out int writtenVersion) ||
                    written == null || writtenVersion != CurrentVersion)
                    throw new InvalidDataException("Temporary state failed post-write validation.");
                if (File.Exists(StateFilePath))
                    File.Replace(temp, StateFilePath, StateFilePath + ".bak", true);
                else
                    File.Move(temp, StateFilePath);
                Debug.Log($"[CSC] State saved -> {StateFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[CSC] Save failed: {e}");
            }
        }

        /// <summary>Applies saved state to the initialized manager. Tries the main file first and
        /// falls back to the .bak backup. Returns false when neither exists/parses, which causes
        /// a fresh seed.</summary>
        public static bool Load(ContractManager mgr)
        {
            if (TryLoadFile(StateFilePath, mgr)) return true;
            if (TryLoadFile(StateFilePath + ".bak", mgr))
            {
                Debug.LogWarning("[CSC] State restored from backup file (main state file missing or broken).");
                return true;
            }
            return false;
        }

        private static bool TryLoadFile(string path, ContractManager mgr)
        {
            try
            {
                if (!File.Exists(path)) return false;
                if (!TryReadRoot(path, out ConfigNode root, out int version)) return false;
                if (version < 1 || version > CurrentVersion)
                {
                    Debug.LogError($"[CSC] Unsupported state version {version} in '{path}'.");
                    return false;
                }

                if (double.TryParse(root.GetValue("scienceMultiplier"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double mult))
                    mgr.ScienceMultiplier = Mathf.Clamp((float)mult, 0.1f, 3.0f);
                if (bool.TryParse(root.GetValue("unlockAll"), out bool ua)) Tuning.UnlockAll = ua;
                if (bool.TryParse(root.GetValue("showToasts"), out bool toasts)) Tuning.ShowToasts = toasts;
                if (bool.TryParse(root.GetValue("playSounds"), out bool sounds)) Tuning.PlaySounds = sounds;
                if (double.TryParse(root.GetValue("uiScale"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double us))
                    Tuning.UiScale = Mathf.Clamp((float)us, 0.8f, 1.6f);
                if (double.TryParse(root.GetValue("missionCenterScale"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double mcs))
                    Tuning.MissionCenterScale = Mathf.Clamp((float)mcs, 0.55f, 1.0f);
                string economy = root.GetValue("economyDifficulty");
                string pacing = root.GetValue("pacingDifficulty");
                string operations = root.GetValue("operationsDifficulty");
                if (!string.IsNullOrEmpty(economy) || !string.IsNullOrEmpty(pacing) || !string.IsNullOrEmpty(operations))
                {
                    if (!string.IsNullOrEmpty(economy)) Tuning.ApplyEconomy(economy, mgr);
                    if (!string.IsNullOrEmpty(pacing)) Tuning.ApplyPacing(pacing);
                    if (!string.IsNullOrEmpty(operations)) Tuning.ApplyOperations(operations);
                }
                else
                {
                    string diff = root.GetValue("difficulty");
                    if (!string.IsNullOrEmpty(diff)) Tuning.ApplyDifficulty(diff);
                }

                foreach (ConfigNode n in root.GetNodes("STATE"))
                {
                    string id = n.GetValue("id");
                    var c = mgr.Catalog.Get(id);
                    if (c == null) { Debug.LogWarning($"[CSC] Ignored state for unknown id '{id}'."); continue; }

                    if (Enum.TryParse(n.GetValue("status"), true, out MissionStatus st)) c.Status = st;
                    c.TotalCompletions = ParseInt(n.GetValue("totalCompletions"));
                    c.CompletionsSinceLastClaim = ParseInt(n.GetValue("completionsSinceLastClaim"));
                    c.FirstCompletedUT = double.TryParse(n.GetValue("firstCompletedUT"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double fut) ? fut : -1.0;
                    var prog = n.GetNode("PROGRESS");
                    c.Progress = prog != null ? prog.CreateCopy() : new ConfigNode("PROGRESS");
                }

                mgr.Stations.Load(root);
                mgr.Fleets.Load(root);

                mgr.CompletionLog.Clear();
                foreach (ConfigNode n in root.GetNodes("COMPLETION"))
                {
                    var entry = new CompletionRecord
                    {
                        Sequence = ParseLong(n.GetValue("sequence")),
                        MissionId = n.GetValue("id") ?? "",
                        UniversalTime = ParseDouble(n.GetValue("ut"), -1.0),
                        Action = n.GetValue("action") ?? "claim",
                        HasScience = ParseBool(n.GetValue("hasScience")),
                        Science = (float)ParseDouble(n.GetValue("science"), 0.0),
                        Imported = ParseBool(n.GetValue("imported")),
                        VesselName = n.GetValue("vessel") ?? "",
                        Crew = n.GetValue("crew") ?? ""
                    };
                    mgr.CompletionLog.AddLoaded(entry);
                }
                if (mgr.CompletionLog.Count == 0)
                {
                    int imported = mgr.CompletionLog.ImportLegacy(mgr.Catalog.All);
                    if (imported > 0)
                        Debug.Log($"[CSC] Migrated {imported} legacy completion(s) into Program Log v2.");
                }

                // Recompute availability after load in case the catalog changed.
                mgr.RecomputeAvailability();
                Debug.Log($"[CSC] State loaded from {path}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CSC] Load of '{path}' failed: {e}");
                return false;
            }
        }

        private static int ParseInt(string s) =>
            int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : 0;

        private static long ParseLong(string s) =>
            long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out long r) ? r : 0L;

        private static double ParseDouble(string s, double fallback) =>
            double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : fallback;

        private static bool ParseBool(string s) => bool.TryParse(s, out bool r) && r;

        private static bool TryReadRoot(string path, out ConfigNode root, out int version)
        {
            root = null;
            version = 0;
            ConfigNode loaded = ConfigNode.Load(path);
            if (loaded == null) return false;
            if (loaded.name == RootNode) root = loaded;
            else if (loaded.HasNode(RootNode)) root = loaded.GetNode(RootNode);
            if (root == null || root.name != RootNode)
            {
                Debug.LogError($"[CSC] Invalid state root in '{path}'.");
                return false;
            }
            version = ParseInt(root.GetValue("version"));
            return version > 0;
        }
    }
}
