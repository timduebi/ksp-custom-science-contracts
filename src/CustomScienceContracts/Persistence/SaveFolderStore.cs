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

        public static string SaveDir =>
            Path.Combine(KSPUtil.ApplicationRootPath, "saves", HighLogic.SaveFolder ?? "default", SubDir);

        public static string StateFilePath => Path.Combine(SaveDir, FileName);

        public static void Save(ContractManager mgr)
        {
            try
            {
                Directory.CreateDirectory(SaveDir);
                // Keep the previous state as .bak: this file is the whole campaign progress, and
                // Load falls back to the backup when the main file is missing or corrupt.
                if (File.Exists(StateFilePath))
                    File.Copy(StateFilePath, StateFilePath + ".bak", true);
                var root = new ConfigNode(RootNode);
                root.AddValue("version", 1);
                root.AddValue("scienceMultiplier", mgr.ScienceMultiplier.ToString("R", CultureInfo.InvariantCulture));
                root.AddValue("unlockAll", Tuning.UnlockAll);
                root.AddValue("showToasts", Tuning.ShowToasts);
                root.AddValue("playSounds", Tuning.PlaySounds);
                root.AddValue("uiScale", Tuning.UiScale.ToString("R", CultureInfo.InvariantCulture));
                root.AddValue("difficulty", Tuning.Difficulty);
                foreach (var c in mgr.Catalog.All)
                {
                    var n = root.AddNode("STATE");
                    n.AddValue("id", c.Id);
                    n.AddValue("status", c.Status.ToString());
                    n.AddValue("totalCompletions", c.TotalCompletions);
                    n.AddValue("completionsSinceLastClaim", c.CompletionsSinceLastClaim);
                    if (c.FirstCompletedUT >= 0.0)
                        n.AddValue("firstCompletedUT", c.FirstCompletedUT.ToString("R", CultureInfo.InvariantCulture));
                    if (c.Status == MissionStatus.Active && c.Progress != null && c.Progress.CountValues + c.Progress.CountNodes > 0)
                        n.AddNode(c.Progress.CreateCopy());
                }
                mgr.Stations.Save(root);
                mgr.Fleets.Save(root);
                root.Save(StateFilePath);
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
                ConfigNode root = ConfigNode.Load(path);
                if (root == null) return false;
                if (root.name != RootNode && root.HasNode(RootNode)) root = root.GetNode(RootNode);

                if (double.TryParse(root.GetValue("scienceMultiplier"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double mult))
                    mgr.ScienceMultiplier = Mathf.Clamp((float)mult, 0.1f, 3.0f);
                if (bool.TryParse(root.GetValue("unlockAll"), out bool ua)) Tuning.UnlockAll = ua;
                if (bool.TryParse(root.GetValue("showToasts"), out bool toasts)) Tuning.ShowToasts = toasts;
                if (bool.TryParse(root.GetValue("playSounds"), out bool sounds)) Tuning.PlaySounds = sounds;
                if (double.TryParse(root.GetValue("uiScale"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double us))
                    Tuning.UiScale = Mathf.Clamp((float)us, 0.8f, 1.6f);
                string diff = root.GetValue("difficulty");
                if (!string.IsNullOrEmpty(diff)) Tuning.ApplyDifficulty(diff);

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
    }
}
