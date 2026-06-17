using System;
using System.Globalization;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Wertet eine COMPOSITE-CONDITION ueber ihre hand-komponierten CHECK-Teilziele aus.
    /// Jeder Check wird einzeln bewertet (Ergebnis in contract.Progress als c{ci}_{j}), damit die UI
    /// pro Teilziel gruen/rot anzeigen kann. HOLD/DURATION laufen ueber einen gemeinsamen Timer, der
    /// startet, sobald alle uebrigen Checks gleichzeitig erfuellt sind (auch unfokussiert/unloaded).
    /// FLYBY/MARKER_LANDING tragen eigenen, ueber mehrere Ticks laufenden State (pro Check ci_j).</summary>
    public static class CheckEvaluation
    {
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;
        private const double Huge = 1e30;

        public static bool Evaluate(MissionContract c, int ci, Condition cond, EvaluationContext ctx)
        {
            var checks = cond.Checks;
            int n = checks.Count;
            var met = new bool[n];

            int timerIdx = -1;
            for (int j = 0; j < n; j++) if (checks[j].IsTimer) timerIdx = j;
            bool hasTimer = timerIdx >= 0;

            // Gibt es ueberhaupt Einzel-Vessel-Checks? Nur dann ist ein Subjekt-Vessel relevant
            // (reine FLEET/FLYBY/EVENT + Timer brauchen keins).
            bool hasVesselChecks = false;
            for (int j = 0; j < n; j++) if (!checks[j].IsSpecial) { hasVesselChecks = true; break; }

            double t0 = GetD(c.Progress, $"c{ci}_t0", -1.0);
            bool timerRunning = hasTimer && t0 >= 0.0;

            // Subjekt-Vessel: bei laufendem Timer ist die Bindung GESPERRT (nur das gemerkte Schiff,
            // kein Wechsel aufs aktive Vessel), damit EVA/Fokus-/Szenenwechsel den Timer nicht abbrechen.
            Vessel subject = ResolveSubject(c, ci, cond, ctx, hasTimer, hasVesselChecks, timerRunning);

            bool allInstant = true;
            for (int j = 0; j < n; j++)
            {
                var chk = checks[j];
                if (chk.IsTimer) continue;
                bool m;
                if (chk.IsFlyby)      m = EvalFlyby(c, ci, j, chk, ctx);
                else if (chk.IsMarker) m = EvalMarker(c, ci, j, chk, ctx);
                else if (chk.IsEvent)  m = EvalEvent(chk, ctx);
                else if (chk.IsFleet)  m = EvalFleet(chk, ctx);
                else                   m = subject != null && EvalVessel(chk, subject);
                met[j] = m;
                if (!m) allInstant = false;
            }

            bool overall;
            if (hasTimer)
            {
                var t = checks[timerIdx];
                double required = t.Kind == CheckKind.HOLD ? t.Seconds : t.Days * VesselQuery.SecondsPerDay();

                // Gebundenes Schiff nur transient nicht auffindbar (Szenenwechsel/Ladevorgang):
                // t0 halten (Pause), NICHT zuruecksetzen — elapsed wird aus absoluter UT nachgeholt.
                bool subjectMissing = timerRunning && hasVesselChecks && subject == null;

                if (timerRunning)
                {
                    if (subjectMissing)  { /* t0 halten */ }
                    else if (allInstant) { /* weiterzaehlen, t0 unveraendert */ }
                    else { t0 = -1.0; c.Progress.SetValue($"c{ci}_t0", "-1", true); } // echte Unterbrechung -> neu
                }
                else if (allInstant)
                {
                    t0 = ctx.UniversalTime; c.Progress.SetValue($"c{ci}_t0", t0.ToString("R", Inv), true); // Start
                }

                double elapsed = t0 >= 0.0 ? ctx.UniversalTime - t0 : 0.0;
                double rem = Math.Max(0.0, required - elapsed);
                c.Progress.SetValue($"c{ci}_rem", rem.ToString("0", Inv), true);
                bool holdMet = !subjectMissing && allInstant && t0 >= 0.0 && elapsed >= required;
                met[timerIdx] = holdMet;
                overall = holdMet;
            }
            else overall = allInstant;

            for (int j = 0; j < n; j++)
                c.Progress.SetValue($"c{ci}_{j}", met[j] ? "1" : "0", true);

            return overall;
        }

        /// <summary>Marker-Waypoints aller Checks dieses Contracts entfernen (Abschluss/Verwerfen).</summary>
        public static void ClearMarkers(MissionContract c) => MarkerWaypoint.RemoveAll(c.Id);

        /// <summary>Haengt die Timer-Bindung (c{ci}_vid) auf das fusionierte Schiff um, wenn das gebundene
        /// Schiff durch Andocken in einem anderen Vessel aufgegangen ist — sonst pausierte der Timer
        /// dauerhaft, weil die alte persistentId nach dem Docking nicht mehr existiert.</summary>
        public static void RemapDockedSubjects(MissionContract c, EvaluationContext ctx)
        {
            if (c.Progress == null || ctx.Events == null) return;
            var merges = ctx.Events.Merges;
            if (merges == null || merges.Count == 0) return;
            for (int ci = 0; ci < c.Bedingungen.Count; ci++)
            {
                string key = $"c{ci}_vid";
                uint vid = GetUInt(c.Progress, key);
                if (vid == 0) continue;
                if (ctx.Vessels.Any(v => v != null && v.persistentId == vid)) continue; // existiert noch
                foreach (var m in merges)
                {
                    uint other = vid == m.IdA ? m.IdB : (vid == m.IdB ? m.IdA : 0u);
                    if (other != 0u && ctx.Vessels.Any(v => v != null && v.persistentId == other))
                    {
                        c.Progress.SetValue(key, other.ToString(), true);
                        break;
                    }
                }
            }
        }

        private static Vessel ResolveSubject(MissionContract c, int ci, Condition cond, EvaluationContext ctx,
                                             bool hasTimer, bool hasVesselChecks, bool timerRunning)
        {
            // Ohne Timer (oder ohne Einzel-Vessel-Checks): immer das aktive Vessel bewerten.
            if (!hasTimer || !hasVesselChecks) return VesselQuery.Active;

            uint vid = GetUInt(c.Progress, $"c{ci}_vid");
            Vessel bound = vid != 0
                ? ctx.Vessels.FirstOrDefault(v => v != null && v.persistentId == vid)
                : null;

            // Laeuft der Timer, ist die Bindung gesperrt: nur das gebundene Schiff zaehlt — auch
            // unfokussiert/unloaded. Kein Fallback aufs aktive Vessel, kein Neubinden. Transient nicht
            // gefunden -> null (der Aufrufer haelt dann t0, statt zurueckzusetzen).
            if (timerRunning) return bound;

            // Timer laeuft noch nicht: gebundenes Schiff behalten, solange es noch erfuellt; sonst das
            // aktive Vessel pruefen und (falls erfuellend) neu binden.
            if (bound != null && SatisfiesVesselChecks(cond, bound, ctx)) return bound;

            var act = VesselQuery.Active;
            if (act != null && SatisfiesVesselChecks(cond, act, ctx))
            {
                c.Progress.SetValue($"c{ci}_vid", act.persistentId.ToString(), true);
                return act;
            }
            return act;
        }

        private static bool SatisfiesVesselChecks(Condition cond, Vessel v, EvaluationContext ctx)
        {
            foreach (var chk in cond.Checks)
                if (!chk.IsSpecial && !EvalVessel(chk, v))
                    return false;
            return true;
        }

        /// <summary>Einzel-Vessel-Check (oeffentlich, damit auch die Subjekt-Verfolgung ihn nutzt).</summary>
        public static bool EvalVessel(Check chk, Vessel v)
        {
            if (v == null) return false;
            CelestialBody body = string.IsNullOrEmpty(chk.Body) ? v.mainBody : BodyResolver.Resolve(chk.Body);
            switch (chk.Kind)
            {
                case CheckKind.CREW_MIN:   return VesselQuery.EffectiveCrew(v) >= chk.Min;
                case CheckKind.CREW_NONE:  return v.GetCrewCount() == 0;
                case CheckKind.CREW_EXACT: return VesselQuery.EffectiveCrew(v) == chk.Min;
                case CheckKind.ON_BODY:    return body != null && v.mainBody == body;
                case CheckKind.SITUATION:  return VesselQuery.MatchesSituation(v, chk.Situation);
                case CheckKind.LANDED:
                    return body != null && v.mainBody == body &&
                           (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED);
                case CheckKind.PERIAPSIS_MIN:
                    return v.orbit != null && v.orbit.PeA > chk.Km * 1000.0;
                case CheckKind.ORBIT_ABOVE:
                {
                    if (body == null || v.mainBody != body ||
                        v.situation != Vessel.Situations.ORBITING || v.orbit == null) return false;
                    // km > 0: fester Wert; km <= 0: API-Atmosphaerenhoehe (kein Hardcode).
                    double minPe = chk.Km > 0 ? chk.Km * 1000.0 : body.atmosphereDepth;
                    return v.orbit.PeA > minPe;
                }
                case CheckKind.INCLINATION_MIN:
                    return v.orbit != null && v.orbit.inclination >= chk.InclinationMin;
                case CheckKind.ABOVE_ATMOSPHERE:
                    return body != null && v.orbit != null && v.orbit.PeA > body.atmosphereDepth;
                case CheckKind.SUBORBITAL_ABOVE_ATMO:
                    return body != null && v.mainBody == body && v.altitude > body.atmosphereDepth;
                case CheckKind.SUBORBITAL:
                    return body != null && v.mainBody == body &&
                           v.situation == Vessel.Situations.SUB_ORBITAL && v.altitude > body.atmosphereDepth;
                case CheckKind.ATMO_FRACTION:
                    if (body == null || !body.atmosphere || v.mainBody != body) return false;
                    double d = body.atmosphereDepth;
                    return v.altitude >= chk.FracMin * d && v.altitude <= chk.FracMax * d;
                case CheckKind.ORE_PRESENT:  return VesselQuery.Resource(v, "Ore") > 0.0;
                case CheckKind.ORE_SURFACE:
                    return body != null && v.mainBody == body &&
                           (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED) &&
                           VesselQuery.Resource(v, "Ore") > 0.0;
                case CheckKind.FUEL_MIN:     return VesselQuery.Fuel(v) > chk.Amount;
                case CheckKind.RESOURCE_MIN: return VesselQuery.Resource(v, chk.Resource) > chk.Amount;
                case CheckKind.EVA:
                    if (!v.isEVA) return false;
                    if (body != null && v.mainBody != body) return false;
                    return VesselQuery.MatchesSituation(v, chk.Situation);
                default:                     return false;
            }
        }

        private static bool EvalEvent(Check chk, EvaluationContext ctx)
        {
            CelestialBody body = BodyResolver.Resolve(chk.Body);
            StationRegistry.Entry station = chk.Kind == CheckKind.DOCK_STATION ? ctx.Stations?.Get(chk.StationKey) : null;
            if (chk.Kind == CheckKind.DOCK_STATION && station == null) return false;

            foreach (var ev in ctx.Events.Dockings)
            {
                var v = ev.Vessel;
                if (v == null) continue;
                if (body != null && v.mainBody != body) continue;
                if (!VesselQuery.MatchesSituation(v, chk.Situation)) continue;
                if (station != null && v.persistentId != station.PersistentId) continue;
                return true;
            }
            return false;
        }

        private static bool EvalFleet(Check chk, EvaluationContext ctx)
        {
            CelestialBody body = BodyResolver.Resolve(chk.Body);
            if (body == null) return false;
            double minM = chk.Km * 1000.0;
            int count = VesselQuery.RealVessels(ctx.Vessels).Count(v =>
                v.mainBody == body &&
                v.situation == Vessel.Situations.ORBITING &&
                v.orbit != null &&
                (minM <= 0.0 || v.orbit.PeA > minM) &&
                (chk.Kind != CheckKind.VESSEL_COUNT_INCLINATION || v.orbit.inclination >= chk.InclinationMin));
            return count >= chk.Count;
        }

        /// <summary>FLYBY-Check: irgendein reales Vessel betritt die SOI von body, orbitet/landet dort NIE
        /// und verlaesst sie wieder; optional muss die kleinste PeA &lt;= km liegen. State pro Check (ci_j),
        /// laeuft unbeaufsichtigt/unloaded; rastet nach Erfolg dauerhaft ein (done=1).</summary>
        private static bool EvalFlyby(MissionContract c, int ci, int j, Check chk, EvaluationContext ctx)
        {
            ConfigNode node = StateNode(c.Progress, $"fb{ci}_{j}");
            if (NInt(node, "done") == 1) return true;

            var body = BodyResolver.Resolve(chk.Body);
            if (body == null) return false;
            double thr = chk.Km > 0 ? chk.Km * 1000.0 : 0.0;
            bool completed = false;
            double bestApproach = Huge;

            foreach (var v in VesselQuery.RealVessels(ctx.Vessels))
            {
                bool atTarget = v.mainBody == body;
                ConfigNode vn = GetVesselNode(node, v.persistentId, create: atTarget);
                if (atTarget)
                {
                    int inSOI = NInt(vn, "inSOI");
                    int orbited = NInt(vn, "orbited");
                    double minPeA = NDouble(vn, "minPeA", Huge);
                    if (inSOI == 0) { inSOI = 1; orbited = 0; minPeA = Huge; }
                    if (v.orbit != null) minPeA = Math.Min(minPeA, v.orbit.PeA);
                    if (v.situation == Vessel.Situations.ORBITING ||
                        v.situation == Vessel.Situations.LANDED ||
                        v.situation == Vessel.Situations.SPLASHED ||
                        v.situation == Vessel.Situations.DOCKED)
                        orbited = 1;
                    bestApproach = Math.Min(bestApproach, minPeA);
                    WriteVessel(vn, inSOI, orbited, minPeA);
                }
                else if (vn != null && NInt(vn, "inSOI") == 1)
                {
                    bool altOk = thr <= 0.0 || NDouble(vn, "minPeA", Huge) <= thr;
                    bool success = NInt(vn, "orbited") == 0 && altOk;
                    WriteVessel(vn, 0, 0, Huge);
                    if (success) completed = true;
                }
            }

            if (bestApproach < Huge)
                c.Progress.SetValue("fb_bestApproach", bestApproach.ToString("0", Inv), true);
            if (completed) { node.SetValue("done", "1", true); return true; }
            return false;
        }

        /// <summary>MARKER_LANDING-Check: Zielpunkt einmalig festlegen (Basisstandort, falls beim ersten Tick
        /// gelandet — sonst deterministischer Zufallspunkt), sichtbaren Waypoint sicherstellen (auch nach
        /// Neuladen), erfuellt sobald das aktive Vessel LANDED/SPLASHED und Grosskreisdistanz &lt;= km.</summary>
        private static bool EvalMarker(MissionContract c, int ci, int j, Check chk, EvaluationContext ctx)
        {
            var body = BodyResolver.Resolve(chk.Body);
            if (body == null) return false;
            string pfx = $"ml{ci}_{j}_";
            string wpId = $"{c.Id}#{ci}_{j}";

            // 1) Zielort einmal festlegen (persistiert).
            if (GetI(c.Progress, pfx + "set", 0) != 1)
            {
                double lat, lon;
                var av = VesselQuery.Active;
                bool atBase = av != null && VesselQuery.OnBody(av, body) &&
                              (av.situation == Vessel.Situations.LANDED || av.situation == Vessel.Situations.SPLASHED);
                if (atBase) { lat = av.latitude; lon = av.longitude; }
                else
                {
                    var rng = new System.Random(($"{c.Id}#{ci}_{j}").GetHashCode());
                    lat = rng.NextDouble() * 140.0 - 70.0;   // -70..70, Pole vermeiden
                    lon = rng.NextDouble() * 360.0 - 180.0;
                }
                c.Progress.SetValue(pfx + "lat", lat.ToString("R", Inv), true);
                c.Progress.SetValue(pfx + "lon", lon.ToString("R", Inv), true);
                c.Progress.SetValue(pfx + "set", "1", true);
            }
            double mLat = GetD(c.Progress, pfx + "lat", 0.0);
            double mLon = GetD(c.Progress, pfx + "lon", 0.0);

            // 2) Sichtbaren Waypoint sicherstellen (Objekt lebt nur zur Laufzeit).
            if (!MarkerWaypoint.Has(wpId))
                MarkerWaypoint.Set(wpId, body, mLat, mLon, c.Titel, Math.Abs(wpId.GetHashCode()) % 10000);

            // 3) Gelandet + Distanz.
            var v = VesselQuery.Active;
            if (v == null) return false;
            bool landed = VesselQuery.OnBody(v, body) &&
                          (v.situation == Vessel.Situations.LANDED || v.situation == Vessel.Situations.SPLASHED);
            if (!landed) return false;
            double rMeters = (chk.Km > 0 ? chk.Km : Tuning.MarkerRadiusKmDefault) * 1000.0;
            double dist = VesselQuery.SurfaceDistance(body, v.latitude, v.longitude, mLat, mLon);
            c.Progress.SetValue("ml_dist", dist.ToString("0", Inv), true);
            return dist <= rMeters;
        }

        // ---- ConfigNode-State-Helfer ----
        private static ConfigNode StateNode(ConfigNode prog, string name)
        {
            var n = prog.GetNode(name);
            return n ?? prog.AddNode(name);
        }
        private static ConfigNode GetVesselNode(ConfigNode parent, uint id, bool create)
        {
            string ids = id.ToString();
            foreach (ConfigNode n in parent.GetNodes("VESSEL"))
                if (n.GetValue("id") == ids) return n;
            if (!create) return null;
            var node = parent.AddNode("VESSEL");
            node.AddValue("id", ids);
            return node;
        }
        private static void WriteVessel(ConfigNode n, int inSOI, int orbited, double minPeA)
        {
            n.SetValue("inSOI", inSOI.ToString(), true);
            n.SetValue("orbited", orbited.ToString(), true);
            n.SetValue("minPeA", minPeA.ToString("R", Inv), true);
        }

        private static uint GetUInt(ConfigNode n, string k) =>
            uint.TryParse(n.GetValue(k), NumberStyles.Integer, Inv, out uint r) ? r : 0u;
        private static int GetI(ConfigNode n, string k, int def) =>
            int.TryParse(n.GetValue(k), NumberStyles.Integer, Inv, out int r) ? r : def;
        private static int NInt(ConfigNode n, string k) =>
            n != null && int.TryParse(n.GetValue(k), NumberStyles.Integer, Inv, out int r) ? r : 0;
        private static double GetD(ConfigNode n, string k, double def) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, Inv, out double r) ? r : def;
        private static double NDouble(ConfigNode n, string k, double def) =>
            n != null && double.TryParse(n.GetValue(k), NumberStyles.Float, Inv, out double r) ? r : def;
    }

    /// <summary>Lifecycle-Haken fuer COMPOSITE-CONDITIONs: die Auswertung selbst laeuft ueber
    /// <see cref="CheckEvaluation"/> (direkt aus dem ContractManager), aber beim Abschluss/Verwerfen
    /// muessen gesetzte Marker-Waypoints entfernt werden.</summary>
    public sealed class CompositeEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.COMPOSITE;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx) => false;
        public override void OnCleared(MissionContract c, Condition cond) => CheckEvaluation.ClearMarkers(c);
    }
}
