using System;
using System.Collections.Generic;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Builds the shortest required completion sequence for a locked mission. CSC
    /// prerequisites are conjunctive, so the unique minimum is the de-duplicated union of every
    /// unfinished ancestor, returned in executable topological order.</summary>
    public static class UnlockPath
    {
        public static List<MissionContract> Build(MissionContract target,
            Func<string, MissionContract> lookup, Func<MissionContract, bool> completed)
        {
            var result = new List<MissionContract>();
            var emitted = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var visiting = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (target == null || lookup == null || completed == null) return result;
            foreach (string id in target.Voraussetzungen)
                Visit(lookup(id), lookup, completed, visiting, emitted, result);
            return result;
        }

        private static void Visit(MissionContract mission, Func<string, MissionContract> lookup,
            Func<MissionContract, bool> completed, HashSet<string> visiting,
            HashSet<string> emitted, List<MissionContract> result)
        {
            if (mission == null || completed(mission) || emitted.Contains(mission.Id)) return;
            if (!visiting.Add(mission.Id)) return; // validator reports cycles; UI remains safe.
            foreach (string id in mission.Voraussetzungen)
                Visit(lookup(id), lookup, completed, visiting, emitted, result);
            visiting.Remove(mission.Id);
            if (emitted.Add(mission.Id)) result.Add(mission);
        }
    }
}
