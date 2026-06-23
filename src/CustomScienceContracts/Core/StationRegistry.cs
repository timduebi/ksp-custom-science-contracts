using System.Collections.Generic;
using System.Linq;
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
            Record(key, v.persistentId, v.vesselName);
        }

        /// <summary>Stores/updates the station for a key from a known id and name. Used to record the
        /// assigned station vessel even when it is not the active/loaded vessel.</summary>
        public void Record(string key, uint persistentId, string name)
        {
            if (string.IsNullOrEmpty(key) || persistentId == 0) return;
            _map[key] = new Entry { Name = string.IsNullOrEmpty(name) ? key : name, PersistentId = persistentId };
            Debug.Log($"[CSC] Recorded station '{key}' = \"{_map[key].Name}\" (id {_map[key].PersistentId}).");
        }

        /// <summary>Follows recorded stations across docking merges, so the id stays valid after a
        /// resupply ship docks (otherwise DOCK_STATION matching and station tracking would break).</summary>
        public void Remap(IReadOnlyList<GameEventBridge.MergeEvent> merges, IReadOnlyList<Vessel> vessels)
        {
            if (merges == null || vessels == null) return;
            foreach (var e in _map.Values)
            {
                if (e.PersistentId == 0 || vessels.Any(v => v != null && v.persistentId == e.PersistentId)) continue;
                foreach (var m in merges)
                {
                    uint other = e.PersistentId == m.IdA ? m.IdB : (e.PersistentId == m.IdB ? m.IdA : 0u);
                    if (other != 0u && vessels.Any(v => v != null && v.persistentId == other))
                    { e.PersistentId = other; break; }
                }
            }
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
