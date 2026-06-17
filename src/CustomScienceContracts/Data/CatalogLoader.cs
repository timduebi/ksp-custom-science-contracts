using System;
using System.Collections.Generic;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Data
{
    /// <summary>Laedt die Missionsdefinitionen aus allen CUSTOM_CONTRACT_CATALOG-Nodes
    /// (GameData/CustomScienceContracts/Contracts/*.cfg). Nutzt GameDatabase, damit beliebig
    /// viele .cfg-Dateien zusammengefuehrt werden. NICHT hardcoded.</summary>
    public static class CatalogLoader
    {
        public const string CatalogNodeName = "CUSTOM_CONTRACT_CATALOG";

        public static List<MissionContract> LoadAll()
        {
            var result = new List<MissionContract>();
            if (GameDatabase.Instance == null)
            {
                Debug.LogError("[CSC] GameDatabase nicht verfuegbar, Katalog leer.");
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
            Debug.Log($"[CSC] Katalog geladen: {result.Count} Contracts aus {catalogs.Length} Katalog-Node(s).");
            return result;
        }

        private static MissionContract ParseContract(ConfigNode node)
        {
            string id = node.GetValue("id");
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("[CSC] CONTRACT ohne 'id' uebersprungen.");
                return null;
            }

            var mc = new MissionContract
            {
                Id = id,
                Titel = node.GetValue("title") ?? id,
                Beschreibung = node.GetValue("description") ?? "",
                Unterkategorie = node.GetValue("subcategory") ?? "",
                IconKey = (node.GetValue("icon") ?? "").Trim(),
                RevealAllAfter = node.GetValue("revealAllAfter") ?? "",
                RecordStationKey = node.GetValue("recordStationKey") ?? "",
                StationRef = node.GetValue("stationRef") ?? ""
            };

            string sparteStr = node.GetValue("sparte");
            if (!Enum.TryParse(sparteStr, true, out mc.HeimatSparte))
            {
                Debug.LogWarning($"[CSC] Contract '{id}': unbekannte Sparte '{sparteStr}', uebersprungen.");
                return null;
            }
            if (mc.HeimatSparte == Sparte.Wiederholbar)
            {
                Debug.LogWarning($"[CSC] Contract '{id}': Heimatsparte darf nicht Wiederholbar sein. " +
                                 "Repeatable=true setzen statt sparte=Wiederholbar.");
                return null;
            }

            float reward;
            float.TryParse(node.GetValue("reward"), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out reward);
            mc.ScienceReward = reward;

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
                Debug.LogWarning($"[CSC] Contract '{id}' hat keine gueltige CONDITION.");

            return mc;
        }

        private static void DetectDuplicateIds(List<MissionContract> list)
        {
            var seen = new HashSet<string>();
            foreach (var mc in list)
                if (!seen.Add(mc.Id))
                    Debug.LogError($"[CSC] Doppelte Contract-Id '{mc.Id}' im Katalog!");
        }
    }
}
