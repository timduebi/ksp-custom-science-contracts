using System;
using System.Globalization;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    // ============================================================================
    //  Stateful conditions. State lives in contract.Progress and is persisted.
    // ============================================================================

    /// <summary>FLYBY: any real vessel enters the target body's SOI, never orbits/lands there, and
    /// leaves again. Optional closest approach must be &lt;= flybyAltitudeKm. Per-vessel state lives
    /// in Progress so multiple probes can be tracked while unfocused/unloaded.</summary>
    public sealed class FlybyEvaluator : ConditionEvaluatorBase
    {
        private const double Huge = 1e30;
        public override ConditionType Type => ConditionType.FLYBY;

        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null) return false;
            double thr = cond.FlybyAltitudeKm > 0 ? cond.FlybyAltitudeKm * 1000.0 : 0.0;

            bool completed = false;
            double bestApproach = Huge;

            foreach (var v in VesselQuery.RealVessels(ctx.Vessels))
            {
                bool atTarget = v.mainBody == body;
                var node = GetVesselNode(c.Progress, v.persistentId, create: atTarget);

                if (atTarget)
                {
                    int inSOI = NInt(node, "inSOI");
                    int orbited = NInt(node, "orbited");
                    double minPeA = NDouble(node, "minPeA", Huge);

                    if (inSOI == 0) { inSOI = 1; orbited = 0; minPeA = Huge; }
                    if (v.orbit != null) minPeA = System.Math.Min(minPeA, v.orbit.PeA);
                    if (v.situation == Vessel.Situations.ORBITING ||
                        v.situation == Vessel.Situations.LANDED ||
                        v.situation == Vessel.Situations.SPLASHED ||
                        v.situation == Vessel.Situations.DOCKED)
                        orbited = 1;

                    bestApproach = System.Math.Min(bestApproach, minPeA);
                    Write(node, inSOI, orbited, minPeA);
                }
                else if (node != null && NInt(node, "inSOI") == 1)
                {
                    // Left the SOI -> evaluate.
                    bool altOk = thr <= 0.0 || NDouble(node, "minPeA", Huge) <= thr;
                    bool success = NInt(node, "orbited") == 0 && altOk;
                    Write(node, 0, 0, Huge);
                    if (success) completed = true;
                }
            }

            if (bestApproach < Huge)
                c.Progress.SetValue("fb_bestApproach",
                    bestApproach.ToString("0", CultureInfo.InvariantCulture), true);
            return completed;
        }

        private static ConfigNode GetVesselNode(ConfigNode prog, uint id, bool create)
        {
            string ids = id.ToString();
            foreach (ConfigNode n in prog.GetNodes("VESSEL"))
                if (n.GetValue("id") == ids) return n;
            if (!create) return null;
            var node = prog.AddNode("VESSEL");
            node.AddValue("id", ids);
            return node;
        }

        private static void Write(ConfigNode n, int inSOI, int orbited, double minPeA)
        {
            n.SetValue("inSOI", inSOI.ToString(), true);
            n.SetValue("orbited", orbited.ToString(), true);
            n.SetValue("minPeA", minPeA.ToString("R", CultureInfo.InvariantCulture), true);
        }

        private static int NInt(ConfigNode n, string k) =>
            n != null && int.TryParse(n.GetValue(k), out int r) ? r : 0;
        private static double NDouble(ConfigNode n, string k, double def) =>
            n != null && double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : def;
    }

    /// <summary>MARKER_LANDING: sets a target point when accepted. Fulfilled once landed/splashed
    /// within the target radius.</summary>
    public sealed class MarkerLandingEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.MARKER_LANDING;

        public override void OnAccepted(MissionContract c, Condition cond) => EnsureMarker(c, cond);
        public override void OnCleared(MissionContract c, Condition cond) => MarkerWaypoint.Remove(c.Id);

        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null) return false;

            EnsureMarker(c, cond);                       // create/recreate marker, even without active vessel
            var v = VesselQuery.Active;
            if (v == null) return false;                 // landed state requires an active vessel

            double mLat = GetD(c.Progress, "ml_lat");
            double mLon = GetD(c.Progress, "ml_lon");
            double rMeters = (cond.RadiusKm > 0 ? cond.RadiusKm : Tuning.MarkerRadiusKmDefault) * 1000.0;

            bool landed = VesselQuery.OnBody(v, body) &&
                          (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED) &&
                          VesselQuery.CrewCount(v) >= cond.MinCrew;
            if (!landed) return false;

            double dist = VesselQuery.SurfaceDistance(body, v.latitude, v.longitude, mLat, mLon);
            c.Progress.SetValue("ml_dist", dist.ToString("0", CultureInfo.InvariantCulture), true);
            return dist <= rMeters;
        }

        private static void EnsureMarker(MissionContract c, Condition cond)
        {
            var body = VesselQuery.Body(cond.Body);
            if (body == null) return;

            // 1) Pick target once and persist it in ml_lat/ml_lon/ml_set.
            if (GetInt(c.Progress, "ml_set") != 1)
            {
                double lat, lon;
                var v = VesselQuery.Active;
                bool atBase = v != null && VesselQuery.OnBody(v, body) &&
                              (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED);
                if (atBase)
                {
                    // Resupply/rotation: marker at the current base location.
                    lat = v.latitude; lon = v.longitude;
                }
                else
                {
                    // Fresh precision landing: deterministic random point, stable per contract id.
                    var rng = new System.Random(c.Id.GetHashCode());
                    lat = rng.NextDouble() * 140.0 - 70.0;     // -70..70, avoid poles
                    lon = rng.NextDouble() * 360.0 - 180.0;
                }
                c.Progress.SetValue("ml_lat", lat.ToString("R", CultureInfo.InvariantCulture), true);
                c.Progress.SetValue("ml_lon", lon.ToString("R", CultureInfo.InvariantCulture), true);
                c.Progress.SetValue("ml_set", "1", true);
            }

            // 2) Ensure visible waypoint after reloads; object state only lives at runtime.
            if (!MarkerWaypoint.Has(c.Id))
            {
                double lat = GetD(c.Progress, "ml_lat");
                double lon = GetD(c.Progress, "ml_lon");
                MarkerWaypoint.Set(c.Id, body, lat, lon, c.Titel, Math.Abs(c.Id.GetHashCode()) % 10000);
            }
        }

        private static int GetInt(ConfigNode n, string k) => int.TryParse(n.GetValue(k), out int r) ? r : 0;
        private static double GetD(ConfigNode n, string k) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : 0.0;
    }

    /// <summary>RENDEZVOUS: active vessel and another real vessel are both in the target situation
    /// at the target body with distance &lt; D km.</summary>
    public sealed class RendezvousEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.RENDEZVOUS;

        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx)
        {
            var v = VesselQuery.Active;
            if (v == null) return false;
            var body = VesselQuery.Body(cond.Body);   // optional
            if (body != null && !VesselQuery.OnBody(v, body)) return false;
            if (!VesselQuery.MatchesSituation(v, cond.Situation)) return false;

            double dMeters = (cond.RendezvousKm > 0 ? cond.RendezvousKm : 2.0) * 1000.0;
            Vector3d posA = v.GetWorldPos3D();

            // Concrete station target? Then only approach to that exact vessel counts.
            var station = ctx.Stations?.Get(cond.StationKey);
            if (!string.IsNullOrEmpty(cond.StationKey) && station == null) return false;

            foreach (var other in VesselQuery.RealVessels(ctx.Vessels))
            {
                if (other == v) continue;
                if (station != null && other.persistentId != station.PersistentId) continue;
                if (body != null && !VesselQuery.OnBody(other, body)) continue;
                if (!VesselQuery.MatchesSituation(other, cond.Situation)) continue;
                if ((posA - other.GetWorldPos3D()).magnitude <= dMeters) return true;
            }
            return false;
        }
    }
}
