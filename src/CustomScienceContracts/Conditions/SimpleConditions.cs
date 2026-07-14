using System.Globalization;
using System.Linq;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    // ============================================================================
    //  Simple conditions. Single-vessel types evaluate FlightGlobals.ActiveVessel.
    //  VESSEL_COUNT_ORBIT counts all real vessels; DOCK/ATMO_ENTRY are event-based.
    //  Bodies are resolved at runtime. Missing bodies make the condition inert.
    // ============================================================================

    /// <summary>ORBITING + orbit.PeA &gt; atmosphereDepth, optional crew &gt;= N.</summary>
    public sealed class OrbitEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.ORBIT;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null) return false;
            return VesselQuery.InOrbitAboveAtmo(v, body) && VesselQuery.CrewCount(v) >= cond.MinCrew;
        }
    }

    /// <summary>ORBITING + orbit.PeA &gt; minAltitudeKm, optional crew &gt;= N.</summary>
    public sealed class OrbitHighEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.ORBIT_HIGH;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null) return false;
            return VesselQuery.OnBody(v, body) &&
                   v.situation == Vessel.Situations.ORBITING &&
                   v.orbit != null && v.orbit.PeA > cond.MinAltitudeKm * 1000.0 &&
                   VesselQuery.CrewCount(v) >= cond.MinCrew;
        }
    }

    /// <summary>LANDED/SPLASHED at target body, optional crew &gt;= N.</summary>
    public sealed class LandedEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.LANDED;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null) return false;
            return VesselQuery.OnBody(v, body) &&
                   (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED) &&
                   VesselQuery.CrewCount(v) >= cond.MinCrew;
        }
    }

    /// <summary>Altitude between minFraction*atmoDepth and maxFraction*atmoDepth, situation FLYING.</summary>
    public sealed class AltFractionAtmoEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.ALT_FRACTION_ATMO;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null || !body.atmosphere) return false;
            double depth = body.atmosphereDepth;
            return VesselQuery.OnBody(v, body) &&
                   v.situation == Vessel.Situations.FLYING &&
                   v.altitude >= cond.MinFraction * depth && v.altitude <= cond.MaxFraction * depth &&
                   VesselQuery.CrewCount(v) >= cond.MinCrew;
        }
    }

    /// <summary>Altitude &gt; atmosphereDepth, situation SUB_ORBITAL.</summary>
    public sealed class AboveAtmoSuborbitalEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.ABOVE_ATMO_SUBORBITAL;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null) return false;
            return VesselQuery.OnBody(v, body) &&
                   v.situation == Vessel.Situations.SUB_ORBITAL &&
                   v.altitude > body.atmosphereDepth &&
                   VesselQuery.CrewCount(v) >= cond.MinCrew;
        }
    }

    /// <summary>Active vessel is an EVA in the target situation at the target body.</summary>
    public sealed class EvaEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.EVA;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || !v.isEVA) return false;
            return (body == null || VesselQuery.OnBody(v, body)) &&
                   VesselQuery.MatchesSituation(v, cond.Situation);
        }
    }

    /// <summary>Situation + crew &gt;= N + continuous UT span &gt;= T days. Tracks one vessel by
    /// persistentId, so time continues while the vessel is unfocused or unloaded. If the vessel
    /// loses situation/crew or disappears, the timer restarts with another matching vessel. Optional high orbit.</summary>
    public sealed class CrewDurationEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.CREW_DURATION;

        public override void OnAccepted(MissionContract c, Condition cond) => Reset(c);
        public override void OnCleared(MissionContract c, Condition cond) => Reset(c);

        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null) return false;
            double required = cond.DurationDays * VesselQuery.SecondsPerDay();
            double minM = cond.MinAltitudeKm * 1000.0;

            bool Predicate(Vessel v) =>
                v != null &&
                VesselQuery.OnBody(v, body) &&
                VesselQuery.MatchesSituation(v, cond.Situation) &&
                VesselQuery.CrewCount(v) >= cond.MinCrew &&
                (minM <= 0.0 || (v.orbit != null && v.orbit.PeA > minM));

            uint trackedId = GetUInt(c.Progress, "cd_vesselId");
            double startUT = GetDouble(c.Progress, "cd_startUT", -1.0);

            Vessel tracked = trackedId != 0
                ? ctx.FindVessel(trackedId)
                : null;

            if (!(tracked != null && Predicate(tracked) && startUT >= 0.0))
            {
                // Tracked vessel disappeared or no longer qualifies -> find a new matching vessel.
                var v0 = ctx.RealVessels.FirstOrDefault(Predicate);
                if (v0 == null) { Reset(c); return false; }
                startUT = ctx.UniversalTime;
                c.Progress.SetValue("cd_vesselId", v0.persistentId.ToString(), true);
                c.Progress.SetValue("cd_startUT", startUT.ToString("R", CultureInfo.InvariantCulture), true);
            }

            double elapsed = ctx.UniversalTime - startUT;
            c.Progress.SetValue("cd_remaining",
                System.Math.Max(0.0, required - elapsed).ToString("0", CultureInfo.InvariantCulture), true);
            return elapsed >= required;
        }

        private static void Reset(MissionContract c)
        {
            c.Progress.SetValue("cd_vesselId", "0", true);
            c.Progress.SetValue("cd_startUT", "-1", true);
            c.Progress.SetValue("cd_remaining", "0", true);
        }

        private static uint GetUInt(ConfigNode n, string k) =>
            uint.TryParse(n.GetValue(k), NumberStyles.Integer, CultureInfo.InvariantCulture, out uint r) ? r : 0u;
        private static double GetDouble(ConfigNode n, string k, double def) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : def;
    }

    /// <summary>Docking (onPartCouple) in target situation/body. Body/situation optional.</summary>
    public sealed class DockEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.DOCK;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            // If the contract targets a recorded station, that station must exist and be involved.
            var station = ctx.Stations?.Get(cond.StationKey);
            if (!string.IsNullOrEmpty(cond.StationKey) && station == null) return false;

            foreach (var ev in ctx.Events.Dockings)
            {
                var v = ev.Vessel;
                if (v == null) continue;
                if (body != null && !VesselQuery.OnBody(v, body)) continue;
                if (!VesselQuery.MatchesSituation(v, cond.Situation)) continue;
                if (station != null && v.persistentId != station.PersistentId) continue;
                return true;
            }
            return false;
        }
    }

    /// <summary>LANDED + Ore amount &gt; 0 at target body.</summary>
    public sealed class OreSurfaceEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.ORE_SURFACE;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null) return false;
            return VesselQuery.OnBody(v, body) &&
                   (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED) &&
                   VesselQuery.Resource(v, "Ore") > 0.0;
        }
    }

    /// <summary>&gt;= N real vessels ORBITING around body at once, optional PeA &gt; minAltitudeKm.</summary>
    public sealed class VesselCountOrbitEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.VESSEL_COUNT_ORBIT;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null) return false;
            double minM = cond.MinAltitudeKm * 1000.0;
            int count = ctx.RealVessels.Count(v =>
                VesselQuery.OnBody(v, body) &&
                v.situation == Vessel.Situations.ORBITING &&
                (minM <= 0.0 || (v.orbit != null && v.orbit.PeA > minM)));
            return count >= cond.VesselCount;
        }
    }

    /// <summary>ORBITING + fuel (LiquidFuel+Oxidizer) &gt; threshold.</summary>
    public sealed class FuelOrbitEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.FUEL_ORBIT;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active; var body = VesselQuery.Body(cond.Body);
            if (v == null || body == null) return false;
            double fuel = VesselQuery.Fuel(v);
            return VesselQuery.OnBody(v, body) &&
                   v.situation == Vessel.Situations.ORBITING &&
                   fuel > cond.FuelThreshold && fuel > 0.0;
        }
    }

    /// <summary>Change to FLYING over a body with atmosphere, event-based.</summary>
    public sealed class AtmoEntryEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.ATMO_ENTRY;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null || !body.atmosphere) return false;
            foreach (var ev in ctx.Events.SituationChanges)
                if (ev.To == Vessel.Situations.FLYING && ev.Vessel != null && VesselQuery.OnBody(ev.Vessel, body))
                    return true;
            return false;
        }
    }
}
