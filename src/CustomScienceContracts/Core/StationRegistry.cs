using System.Collections.Generic;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Merkt sich pro Schluessel (z.B. "earth_station") den Namen + persistentId des
    /// Vessels, das einen "Station/Basis bauen"-Auftrag erfuellt hat. Wiederhol-Versorgungs-
    /// auftraege referenzieren denselben Schluessel und koennen so gezielt diese Station nennen
    /// und ihr Andocken/Rendezvous gegen genau dieses Vessel pruefen. Wird im Save-Ordner
    /// mitpersistiert (STATION-Nodes).</summary>
    public class StationRegistry
    {
        public class Entry { public string Name = ""; public uint PersistentId; }

        private readonly Dictionary<string, Entry> _map = new Dictionary<string, Entry>();

        /// <summary>Eintrag fuer einen Schluessel (oder null).</summary>
        public Entry Get(string key) =>
            !string.IsNullOrEmpty(key) && _map.TryGetValue(key, out var e) ? e : null;

        /// <summary>Anzeigename der Station (oder null).</summary>
        public string Name(string key) => Get(key)?.Name;

        /// <summary>Speichert/aktualisiert die Station eines Schluessels aus einem Vessel.</summary>
        public void Record(string key, Vessel v)
        {
            if (string.IsNullOrEmpty(key) || v == null) return;
            _map[key] = new Entry { Name = v.vesselName ?? key, PersistentId = v.persistentId };
            Debug.Log($"[CSC] Station '{key}' = \"{_map[key].Name}\" (id {_map[key].PersistentId}) gemerkt.");
        }

        public void Clear() => _map.Clear();

        // --- Persistenz (STATION-Nodes unter dem State-Root) ---
        public void Save(ConfigNode root)
        {
            foreach (var kv in _map)
            {
                var n = root.AddNode("STATION");
                n.AddValue("key", kv.Key);
                n.AddValue("name", kv.Value.Name);
                n.AddValue("persistentId", kv.Value.PersistentId);
            }
        }

        public void Load(ConfigNode root)
        {
            _map.Clear();
            foreach (ConfigNode n in root.GetNodes("STATION"))
            {
                string key = n.GetValue("key");
                if (string.IsNullOrEmpty(key)) continue;
                uint.TryParse(n.GetValue("persistentId"), out uint id);
                _map[key] = new Entry { Name = n.GetValue("name") ?? key, PersistentId = id };
            }
        }
    }
}
