using System;
using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Conditions
{
    /// <summary>State machine for crewed source visits and safe returns. Crew identity, rather than
    /// vessel id, survives staging/docking and is intentionally not exposed as an easy checklist row.</summary>
    public static class ReturnEvaluation
    {
        public static bool Evaluate(MissionContract contract, int conditionIndex, int checkIndex,
            Check check, EvaluationContext context)
        {
            ConfigNode state = StateNode(contract.Progress, $"ret{conditionIndex}_{checkIndex}");
            if (Int(state, "done") == 1) return true;

            CelestialBody source = BodyResolver.Resolve(check.Body);
            CelestialBody home = BodyResolver.Resolve(string.IsNullOrEmpty(check.ReturnBody) ? "Kerbin" : check.ReturnBody);
            if (source == null || home == null) return false;
            bool flyby = string.Equals(check.ReturnMode, "flyby", StringComparison.OrdinalIgnoreCase);
            bool visit = flyby || string.Equals(check.ReturnMode, "visit", StringComparison.OrdinalIgnoreCase);
            bool homeMode = string.Equals(check.ReturnMode, "home", StringComparison.OrdinalIgnoreCase);
            bool seen = Int(state, "seenSource") == 1;
            bool loggedThisTick = false;

            foreach (Vessel vessel in context.RealVessels)
            {
                if (!Crewed(vessel)) continue;
                bool atSource = homeMode
                    ? vessel.mainBody == source && !Surface(vessel) && vessel.situation != Vessel.Situations.PRELAUNCH
                    : vessel.mainBody == source && (visit || Surface(vessel));
                bool atHome = vessel.mainBody == home && Surface(vessel);
                if (!seen && atSource)
                {
                    RememberHomeVessels(state, context, home);
                    RememberSourceCrew(state, vessel);
                    seen = true;
                    loggedThisTick = true;
                    state.SetValue("seenSource", "1", true);
                    state.SetValue("sourceVessel", vessel.persistentId.ToString(), true);
                    contract.Progress.SetValue("ret_status", visit || homeMode ? "visit_logged" : "source_logged", true);
                }
                if (seen && !loggedThisTick && atHome && !WasAtHomeBefore(state, vessel.persistentId) &&
                    CrewMatches(state, CrewNames(vessel)))
                {
                    state.SetValue("done", "1", true);
                    state.SetValue("returnVessel", vessel.persistentId.ToString(), true);
                    contract.Progress.SetValue("ret_status", "returned", true);
                    return true;
                }
            }
            contract.Progress.SetValue("ret_status", seen ? "awaiting_return" :
                (visit || homeMode ? "awaiting_visit" : "awaiting_source"), true);
            return false;
        }

        /// <summary>At least one source Kerbal must return. Missing version is the explicit v1
        /// compatibility path for already-active 0.6 missions.</summary>
        public static bool CrewMatches(ConfigNode returnState, IEnumerable<string> crew)
        {
            if (returnState == null) return false;
            if (returnState.GetValue("crewIdentityVersion") != "1") return true;
            ConfigNode source = returnState.GetNode("SOURCE_CREW");
            if (source == null || crew == null) return false;
            var expected = new HashSet<string>(source.GetValues("name"), StringComparer.Ordinal);
            return crew.Any(name => !string.IsNullOrEmpty(name) && expected.Contains(name));
        }

        private static bool Crewed(Vessel vessel) => vessel != null && VesselQuery.EffectiveCrew(vessel) > 0;
        private static bool Surface(Vessel vessel) => vessel != null &&
            (vessel.situation == Vessel.Situations.LANDED || vessel.situation == Vessel.Situations.SPLASHED);

        private static void RememberHomeVessels(ConfigNode state, EvaluationContext context, CelestialBody home)
        {
            ConfigNode baseline = StateNode(state, "HOME_BASELINE");
            foreach (Vessel vessel in context.RealVessels)
                if (Crewed(vessel) && Surface(vessel) && vessel.mainBody == home)
                    VesselNode(baseline, vessel.persistentId, true);
        }

        private static bool WasAtHomeBefore(ConfigNode state, uint id)
        {
            ConfigNode baseline = state.GetNode("HOME_BASELINE");
            return baseline != null && VesselNode(baseline, id, false) != null;
        }

        private static void RememberSourceCrew(ConfigNode state, Vessel vessel)
        {
            ConfigNode source = state.GetNode("SOURCE_CREW") ?? state.AddNode("SOURCE_CREW");
            source.ClearValues();
            foreach (string name in CrewNames(vessel)) source.AddValue("name", name);
            state.SetValue("crewIdentityVersion", "1", true);
        }

        private static string[] CrewNames(Vessel vessel)
        {
            try
            {
                return vessel?.GetVesselCrew().Where(k => k != null && !string.IsNullOrEmpty(k.name))
                    .Select(k => k.name).Distinct(StringComparer.Ordinal).ToArray() ?? Array.Empty<string>();
            }
            catch (Exception) { return Array.Empty<string>(); }
        }

        private static ConfigNode StateNode(ConfigNode parent, string name) =>
            parent.GetNode(name) ?? parent.AddNode(name);

        private static ConfigNode VesselNode(ConfigNode parent, uint id, bool create)
        {
            string text = id.ToString();
            foreach (ConfigNode node in parent.GetNodes("VESSEL"))
                if (node.GetValue("id") == text) return node;
            if (!create) return null;
            ConfigNode added = parent.AddNode("VESSEL");
            added.AddValue("id", text);
            return added;
        }

        private static int Int(ConfigNode node, string key) =>
            node != null && int.TryParse(node.GetValue(key), out int value) ? value : 0;
    }
}
