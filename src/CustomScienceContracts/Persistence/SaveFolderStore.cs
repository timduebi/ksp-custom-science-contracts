using System;
using System.Globalization;
using System.IO;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Persistence
{
    /// <summary>Liest/schreibt den Laufzeit-State als editierbare Datei im Save-Ordner:
    /// saves/&lt;save&gt;/CustomScienceContracts/contracts_state.cfg. Diese Datei ist autoritativ;
    /// fehlt oder bricht sie, wird frisch aus dem Katalog neu geseedet (alles Locked/Available).</summary>
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
                var root = new ConfigNode(RootNode);
                root.AddValue("version", 1);
                root.AddValue("scienceMultiplier", mgr.ScienceMultiplier.ToString("R", CultureInfo.InvariantCulture));
                root.AddValue("unlockAll", Tuning.UnlockAll);
                foreach (var c in mgr.Catalog.All)
                {
                    var n = root.AddNode("STATE");
                    n.AddValue("id", c.Id);
                    n.AddValue("status", c.Status.ToString());
                    n.AddValue("totalCompletions", c.TotalCompletions);
                    n.AddValue("completionsSinceLastClaim", c.CompletionsSinceLastClaim);
                    if (c.Status == MissionStatus.Active && c.Progress != null && c.Progress.CountValues + c.Progress.CountNodes > 0)
                        n.AddNode(c.Progress.CreateCopy());
                }
                mgr.Stations.Save(root);
                root.Save(StateFilePath);
                Debug.Log($"[CSC] State gespeichert -> {StateFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[CSC] Speichern fehlgeschlagen: {e}");
            }
        }

        /// <summary>Wendet den gespeicherten State auf den bereits initialisierten Manager an.
        /// Gibt false zurueck, wenn keine (gueltige) Datei existiert -> frisch seeden.</summary>
        public static bool Load(ContractManager mgr)
        {
            try
            {
                if (!File.Exists(StateFilePath)) return false;
                ConfigNode root = ConfigNode.Load(StateFilePath);
                if (root == null) return false;
                if (root.name != RootNode && root.HasNode(RootNode)) root = root.GetNode(RootNode);

                if (double.TryParse(root.GetValue("scienceMultiplier"), NumberStyles.Float,
                        CultureInfo.InvariantCulture, out double mult))
                    mgr.ScienceMultiplier = Mathf.Clamp((float)mult, 0.1f, 3.0f);
                if (bool.TryParse(root.GetValue("unlockAll"), out bool ua)) Tuning.UnlockAll = ua;

                foreach (ConfigNode n in root.GetNodes("STATE"))
                {
                    string id = n.GetValue("id");
                    var c = mgr.Catalog.Get(id);
                    if (c == null) { Debug.LogWarning($"[CSC] State fuer unbekannte Id '{id}' ignoriert."); continue; }

                    if (Enum.TryParse(n.GetValue("status"), true, out MissionStatus st)) c.Status = st;
                    c.TotalCompletions = ParseInt(n.GetValue("totalCompletions"));
                    c.CompletionsSinceLastClaim = ParseInt(n.GetValue("completionsSinceLastClaim"));
                    var prog = n.GetNode("PROGRESS");
                    c.Progress = prog != null ? prog.CreateCopy() : new ConfigNode("PROGRESS");
                }

                mgr.Stations.Load(root);

                // Nach dem Laden Verfuegbarkeit neu ableiten (falls Katalog sich geaendert hat).
                mgr.RecomputeAvailability();
                Debug.Log($"[CSC] State geladen aus {StateFilePath}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[CSC] Laden fehlgeschlagen, seede frisch: {e}");
                return false;
            }
        }

        private static int ParseInt(string s) =>
            int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : 0;
    }
}
