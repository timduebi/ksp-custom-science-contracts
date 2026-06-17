using System.Globalization;
using System.Linq;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    // ============================================================================
    //  Einfache Bedingungen (Schritt 4). Einzel-Vessel-Typen werten das AKTIVE
    //  Fahrzeug aus (FlightGlobals.ActiveVessel). VESSEL_COUNT_ORBIT zaehlt alle
    //  realen Vessels; DOCK/ATMO_ENTRY sind ereignisbasiert. Body wird zur Laufzeit
    //  aufgeloest; fehlt er, ist die Bedingung inert. Atmo/Tageslaenge aus der API.
    // ============================================================================

    /// <summary>ORBITING + orbit.PeA &gt; atmosphereDepth, optional Crew &gt;= N (aktives Vessel).</summary>
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

    /// <summary>ORBITING + orbit.PeA &gt; minAltitudeKm, optional Crew &gt;= N (aktives Vessel).</summary>
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

    /// <summary>LANDED/SPLASHED auf Zielkoerper, optional Crew &gt;= N (aktives Vessel).</summary>
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

    /// <summary>Hoehe zwischen minFraction*atmoDepth und maxFraction*atmoDepth, Situation FLYING.</summary>
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

    /// <summary>Hoehe &gt; atmosphereDepth, Situation SUB_ORBITAL (aktives Vessel).</summary>
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

    /// <summary>Aktives Vessel ist im EVA-Zustand in der Ziel-Situation am Zielkoerper.</summary>
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

    /// <summary>Situation + Crew &gt;= N + zusammenhaengende UT-Spanne &gt;= T Tage. Verfolgt ein
    /// konkretes Vessel ueber persistentId — die Zeit laeuft weiter, auch wenn das Vessel nicht
    /// aktiv/fokussiert ist (auch unloaded). Verliert das Vessel Situation/Crew oder verschwindet
    /// es, startet der Timer mit einem anderen passenden Vessel neu. Optional Hochorbit (minAltitudeKm).</summary>
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
                ? ctx.Vessels.FirstOrDefault(v => v != null && v.persistentId == trackedId)
                : null;

            if (!(tracked != null && Predicate(tracked) && startUT >= 0.0))
            {
                // verfolgtes Vessel weg/ungeeignet -> neues passendes Vessel suchen
                var v0 = VesselQuery.RealVessels(ctx.Vessels).FirstOrDefault(Predicate);
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

    /// <summary>Andocken (onPartCouple) in Ziel-Situation/-Koerper. Body/Situation optional.</summary>
    public sealed class DockEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.DOCK;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            // Zielt der Auftrag auf eine konkrete gemerkte Station, muss diese existieren und am
            // Andocken beteiligt sein (sonst zaehlt jedes Andockmanoever).
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

    /// <summary>LANDED + Ore-Menge &gt; 0 auf Zielkoerper (aktives Vessel).</summary>
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

    /// <summary>&gt;= N reale Vessels gleichzeitig ORBITING um Koerper (optional PeA &gt; minAltitudeKm).</summary>
    public sealed class VesselCountOrbitEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.VESSEL_COUNT_ORBIT;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null) return false;
            double minM = cond.MinAltitudeKm * 1000.0;
            int count = VesselQuery.RealVessels(ctx.Vessels).Count(v =>
                VesselQuery.OnBody(v, body) &&
                v.situation == Vessel.Situations.ORBITING &&
                (minM <= 0.0 || (v.orbit != null && v.orbit.PeA > minM)));
            return count >= cond.VesselCount;
        }
    }

    /// <summary>ORBITING + Treibstoff (LiquidFuel+Oxidizer) &gt; Schwelle (aktives Vessel).</summary>
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

    /// <summary>Wechsel zu FLYING ueber einem Koerper mit Atmosphaere (Re-/Entry, ereignisbasiert).</summary>
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
