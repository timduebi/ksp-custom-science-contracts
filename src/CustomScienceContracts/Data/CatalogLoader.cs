using System;
using System.Collections.Generic;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Data
{
    /// <summary>Loads mission definitions from all CUSTOM_CONTRACT_CATALOG nodes
    /// (GameData/CustomScienceContracts/Contracts/*.cfg). Uses GameDatabase so any number of
    /// catalog files can be merged. Not hardcoded.</summary>
    public static class CatalogLoader
    {
        public const string CatalogNodeName = "CUSTOM_CONTRACT_CATALOG";

        public static List<MissionContract> LoadAll()
        {
            var result = new List<MissionContract>();
            if (GameDatabase.Instance == null)
            {
                Debug.LogError("[CSC] GameDatabase unavailable, catalog is empty.");
                return result;
            }

            ConfigNode[] catalogs = GameDatabase.Instance.GetConfigNodes(CatalogNodeName);
            foreach (ConfigNode catalog in catalogs)
            {
                foreach (ConfigNode cNode in catalog.GetNodes("CONTRACT"))
                {
                    MissionContract mc = ParseContract(cNode);
                    if (mc != null) result.Add(mc);
                }
            }

            DetectDuplicateIds(result);
            Debug.Log($"[CSC] Catalog loaded: {result.Count} contracts from {catalogs.Length} catalog node(s).");
            return result;
        }

        /// <summary>Loads optional EPOCH metadata nodes (number/name/description) from all
        /// catalogs. Missing metadata is fine; the UI falls back to built-in epoch names.</summary>
        public static List<ContractCatalog.EpochInfo> LoadEpochs()
        {
            var result = new List<ContractCatalog.EpochInfo>();
            if (GameDatabase.Instance == null) return result;
            foreach (ConfigNode catalog in GameDatabase.Instance.GetConfigNodes(CatalogNodeName))
            {
                foreach (ConfigNode e in catalog.GetNodes("EPOCH"))
                {
                    if (!int.TryParse(e.GetValue("number"), out int number) || number < 1)
                    {
                        Debug.LogWarning("[CSC] Skipped EPOCH node without valid 'number'.");
                        continue;
                    }
                    result.Add(new ContractCatalog.EpochInfo
                    {
                        Number = number,
                        Name = (e.GetValue("name") ?? "").Trim(),
                        Description = (e.GetValue("description") ?? "").Trim()
                    });
                }
            }
            return result;
        }

        private static MissionContract ParseContract(ConfigNode node)
        {
            string id = node.GetValue("id");
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("[CSC] Skipped CONTRACT without 'id'.");
                return null;
            }

            var mc = new MissionContract
            {
                Id = id,
                Titel = node.GetValue("title") ?? id,
                Beschreibung = node.GetValue("description") ?? "",
                Unterkategorie = node.GetValue("subcategory") ?? "",
                IconKey = (node.GetValue("icon") ?? "").Trim(),
                EpochTitle = (node.GetValue("epochName") ?? "").Trim(),
                RevealAllAfter = node.GetValue("revealAllAfter") ?? "",
                RecordStationKey = node.GetValue("recordStationKey") ?? "",
                StationRef = node.GetValue("stationRef") ?? ""
            };

            string sparteStr = node.GetValue("sparte");
            if (!Enum.TryParse(sparteStr, true, out mc.HeimatSparte))
            {
                Debug.LogWarning($"[CSC] Contract '{id}': unknown branch '{sparteStr}', skipped.");
                return null;
            }
            if (mc.HeimatSparte == Sparte.Wiederholbar)
            {
                Debug.LogWarning($"[CSC] Contract '{id}': home branch cannot be Wiederholbar. " +
                                 "Use repeatable=true instead of sparte=Wiederholbar.");
                return null;
            }

            float reward;
            float.TryParse(node.GetValue("reward"), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out reward);
            mc.ScienceReward = reward;

            if (int.TryParse(node.GetValue("epoch"), out int epoch))
                mc.Epoch = Math.Max(1, epoch);

            bool rep;
            bool.TryParse(node.GetValue("repeatable"), out rep);
            mc.Repeatable = rep;

            foreach (string pre in node.GetValues("prerequisite"))
                if (!string.IsNullOrEmpty(pre)) mc.Voraussetzungen.Add(pre.Trim());

            foreach (ConfigNode condNode in node.GetNodes("CONDITION"))
            {
                Condition c = Condition.Load(condNode);
                if (c != null) mc.Bedingungen.Add(c);
            }

            if (mc.Bedingungen.Count == 0)
                Debug.LogWarning($"[CSC] Contract '{id}' has no valid CONDITION.");

            return mc;
        }

        private static void DetectDuplicateIds(List<MissionContract> list)
        {
            var seen = new HashSet<string>();
            foreach (var mc in list)
                if (!seen.Add(mc.Id))
                    Debug.LogError($"[CSC] Duplicate contract id '{mc.Id}' in catalog.");
        }
    }
}
