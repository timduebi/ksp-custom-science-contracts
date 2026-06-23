using System.Collections.Generic;
using System.Globalization;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Player-set vessel binding for a mission, stored inside the mission Progress node so it
    /// persists with the save folder state automatically. Single-vessel missions (incl. flyby) use
    /// <c>assignedVid</c>; fleet/network missions use a <c>FLEET</c> node with multiple vessel ids
    /// (N2 hybrid: empty set means count fleet-wide as before).</summary>
    public static class MissionBinding
    {
        private const string KeyVid  = "assignedVid";
        private const string KeyName = "assignedName";
        private const string KeyLost = "assignedLost";
        private const string FleetNode = "FLEET";

        // ---- Single vessel ----
        public static uint AssignedVid(MissionContract c) => GetUInt(c?.Progress, KeyVid);
        public static string AssignedName(MissionContract c) => c?.Progress?.GetValue(KeyName) ?? "";
        public static bool IsLost(MissionContract c) => GetI(c?.Progress, KeyLost) == 1;

        public static void Assign(MissionContract c, uint vid, string name)
        {
            if (c?.Progress == null || vid == 0) return;
            c.Progress.SetValue(KeyVid, vid.ToString(), true);
            c.Progress.SetValue(KeyName, string.IsNullOrEmpty(name) ? "Vessel" : name, true);
            c.Progress.RemoveValue(KeyLost);
        }

        public static void Clear(MissionContract c)
        {
            if (c?.Progress == null) return;
            c.Progress.RemoveValue(KeyVid);
            c.Progress.RemoveValue(KeyName);
            c.Progress.RemoveValue(KeyLost);
        }

        public static void SetLost(MissionContract c, bool lost)
        {
            if (c?.Progress == null) return;
            if (lost) c.Progress.SetValue(KeyLost, "1", true);
            else c.Progress.RemoveValue(KeyLost);
        }

        /// <summary>Move the single-vessel binding to the docking survivor id.</summary>
        public static void RemapAssigned(MissionContract c, uint from, uint to)
        {
            if (c?.Progress == null || from == 0 || to == 0) return;
            if (GetUInt(c.Progress, KeyVid) == from)
                c.Progress.SetValue(KeyVid, to.ToString(), true);
        }

        // ---- Fleet (network) ----
        public static bool HasFleet(MissionContract c)
        {
            var node = c?.Progress?.GetNode(FleetNode);
            return node != null && node.CountNodes > 0;
        }

        public static List<FleetEntry> Fleet(MissionContract c)
        {
            var list = new List<FleetEntry>();
            var node = c?.Progress?.GetNode(FleetNode);
            if (node == null) return list;
            foreach (ConfigNode v in node.GetNodes("VESSEL"))
            {
                uint vid = GetUInt(v, "vid");
                if (vid == 0) continue;
                list.Add(new FleetEntry { Vid = vid, Name = v.GetValue("name") ?? "Vessel" });
            }
            return list;
        }

        public static bool FleetContains(MissionContract c, uint vid)
        {
            var node = c?.Progress?.GetNode(FleetNode);
            if (node == null || vid == 0) return false;
            foreach (ConfigNode v in node.GetNodes("VESSEL"))
                if (GetUInt(v, "vid") == vid) return true;
            return false;
        }

        public static void FleetAdd(MissionContract c, uint vid, string name)
        {
            if (c?.Progress == null || vid == 0 || FleetContains(c, vid)) return;
            var node = c.Progress.GetNode(FleetNode) ?? c.Progress.AddNode(FleetNode);
            var v = node.AddNode("VESSEL");
            v.AddValue("vid", vid.ToString());
            v.AddValue("name", string.IsNullOrEmpty(name) ? "Vessel" : name);
        }

        public static void FleetRemove(MissionContract c, uint vid)
        {
            var node = c?.Progress?.GetNode(FleetNode);
            if (node == null) return;
            var keep = new List<ConfigNode>();
            foreach (ConfigNode v in node.GetNodes("VESSEL"))
                if (GetUInt(v, "vid") != vid) keep.Add(v.CreateCopy());
            node.ClearNodes();
            foreach (var k in keep) node.AddNode(k);
            if (node.CountNodes == 0) c.Progress.RemoveNode(FleetNode);
        }

        /// <summary>Move a fleet member to the docking survivor id.</summary>
        public static void FleetRemap(MissionContract c, uint from, uint to)
        {
            var node = c?.Progress?.GetNode(FleetNode);
            if (node == null || from == 0 || to == 0) return;
            foreach (ConfigNode v in node.GetNodes("VESSEL"))
                if (GetUInt(v, "vid") == from) v.SetValue("vid", to.ToString(), true);
        }

        private static uint GetUInt(ConfigNode n, string k) =>
            n != null && uint.TryParse(n.GetValue(k), NumberStyles.Integer, CultureInfo.InvariantCulture, out uint r) ? r : 0u;
        private static int GetI(ConfigNode n, string k) =>
            n != null && int.TryParse(n.GetValue(k), NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : 0;
    }

    /// <summary>One assigned satellite of a network/fleet mission.</summary>
    public class FleetEntry
    {
        public uint Vid;
        public string Name;
    }
}
