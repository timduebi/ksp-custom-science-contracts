using System.Collections.Generic;

namespace CustomScienceContracts.Core
{
    /// <summary>Persists the satellite fleet that completed a network mission, keyed by mission id, so
    /// a follow-up network can inherit the predecessor's satellites. The active fleet lives in the
    /// mission Progress, but that is wiped on claim, so the completed fleet is copied here. Saved in
    /// the save folder as FLEET_RECORD nodes.</summary>
    public class FleetRegistry
    {
        public class Member { public uint Vid; public string Name = ""; }

        private readonly Dictionary<string, List<Member>> _map = new Dictionary<string, List<Member>>();

        /// <summary>Recorded fleet for a mission id, or null.</summary>
        public List<Member> Get(string missionId) =>
            !string.IsNullOrEmpty(missionId) && _map.TryGetValue(missionId, out var l) ? l : null;

        public void Set(string missionId, List<Member> members)
        {
            if (string.IsNullOrEmpty(missionId)) return;
            if (members == null || members.Count == 0) _map.Remove(missionId);
            else _map[missionId] = members;
        }

        public void Clear() => _map.Clear();

        // --- Persistence (FLEET_RECORD nodes below the state root) ---
        public void Save(ConfigNode root)
        {
            foreach (var kv in _map)
            {
                var n = root.AddNode("FLEET_RECORD");
                n.AddValue("mission", kv.Key);
                foreach (var m in kv.Value)
                {
                    var vn = n.AddNode("VESSEL");
                    vn.AddValue("vid", m.Vid);
                    vn.AddValue("name", m.Name);
                }
            }
        }

        public void Load(ConfigNode root)
        {
            _map.Clear();
            foreach (ConfigNode n in root.GetNodes("FLEET_RECORD"))
            {
                string mission = n.GetValue("mission");
                if (string.IsNullOrEmpty(mission)) continue;
                var list = new List<Member>();
                foreach (ConfigNode vn in n.GetNodes("VESSEL"))
                {
                    uint.TryParse(vn.GetValue("vid"), out uint vid);
                    if (vid == 0) continue;
                    list.Add(new Member { Vid = vid, Name = vn.GetValue("name") ?? "Vessel" });
                }
                if (list.Count > 0) _map[mission] = list;
            }
        }
    }
}
