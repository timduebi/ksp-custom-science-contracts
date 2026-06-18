using System.Collections.Generic;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Stores the name and persistentId of the vessel that fulfilled a build station/base
    /// contract for each key. Repeatable resupply contracts reference the same key so they can name
    /// that exact station and check docking/rendezvous against that vessel. Persisted in the save
    /// folder as STATION nodes.</summary>
    public class StationRegistry
    {
        public class Entry { public string Name = ""; public uint PersistentId; }

        private readonly Dictionary<string, Entry> _map = new Dictionary<string, Entry>();

        /// <summary>Entry for a key, or null.</summary>
        public Entry Get(string key) =>
            !string.IsNullOrEmpty(key) && _map.TryGetValue(key, out var e) ? e : null;

        /// <summary>Display name of the station, or null.</summary>
        public string Name(string key) => Get(key)?.Name;

        /// <summary>Stores/updates the station for a key from a vessel.</summary>
        public void Record(string key, Vessel v)
        {
            if (string.IsNullOrEmpty(key) || v == null) return;
            _map[key] = new Entry { Name = v.vesselName ?? key, PersistentId = v.persistentId };
            Debug.Log($"[CSC] Recorded station '{key}' = \"{_map[key].Name}\" (id {_map[key].PersistentId}).");
        }

        public void Clear() => _map.Clear();

        // --- Persistence (STATION nodes below the state root) ---
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
