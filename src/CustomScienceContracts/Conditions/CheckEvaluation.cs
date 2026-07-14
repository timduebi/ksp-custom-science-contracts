using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Evaluates a COMPOSITE condition through its hand-composed CHECK goals. Each check is
    /// evaluated individually so the UI can show per-goal state. HOLD/DURATION use one shared timer
    /// that starts once all other checks are fulfilled, even while unfocused/unloaded. Flyby and
    /// marker checks keep their own multi-tick state.</summary>
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
            bool hasReturn = false;
            for (int j = 0; j < n; j++) if (checks[j].IsReturn) { hasReturn = true; break; }

            // A subject vessel is relevant only when the condition has single-vessel checks.
            bool hasVesselChecks = false;
            for (int j = 0; j < n; j++) if (!checks[j].IsSpecial) { hasVesselChecks = true; break; }

            double t0 = GetD(c.Progress, $"c{ci}_t0", -1.0);
            bool timerRunning = hasTimer && t0 >= 0.0;

            // Subject vessel: once a timer is running, the binding is locked to the recorded vessel
            // so focus or scene changes do not break the timer.
            Vessel subject = ResolveSubject(c, ci, cond, ctx, hasTimer, hasVesselChecks, timerRunning);
            bool legacyEvaluation = GetI(c.Progress, "evaluationSchema", 0) <
                                    StatePersistencePolicy.CurrentEvaluationSchema;

            bool allInstant = true;
            bool allTimerPrereqs = true;
            for (int j = 0; j < n; j++)
            {
                var chk = checks[j];
                if (chk.IsTimer) continue;
                bool m;
                string latchKey = $"c{ci}_{j}_latched";
                if (hasReturn && !chk.IsReturn && GetI(c.Progress, latchKey, 0) == 1)
                {
                    m = true;
                }
                else
                {
                    if (chk.IsFlyby)      m = EvalFlyby(c, ci, j, chk, ctx);
                    else if (chk.IsMarker) m = EvalMarker(c, ci, j, chk, ctx);
                    else if (chk.IsReturn) m = EvalReturn(c, ci, j, chk, ctx);
                    else if (chk.IsEvent)  m = EvalEvent(chk, ctx);
                    else if (chk.IsFleet)  m = EvalFleet(c, chk, ctx, legacyEvaluation);
                    else if (chk.IsDelivery) m = legacyEvaluation
                        ? EvalLegacyDelivery(chk, subject)
                        : EvalDelivery(c, ci, j, chk, ctx, subject);
                    else if (chk.Kind == CheckKind.EVA) m = EvalEvaForMission(chk, ctx, subject);
                    else                   m = subject != null && EvalVessel(chk, subject);
                    if (hasReturn && !chk.IsReturn && m)
                        c.Progress.SetValue(latchKey, "1", true);
                }
                met[j] = m;
                if (!m) allInstant = false;
                if (!chk.IsReturn && !m) allTimerPrereqs = false;
            }

            bool overall;
            if (hasTimer)
            {
                var t = checks[timerIdx];
                double required = t.Kind == CheckKind.HOLD ? t.Seconds : t.Days * VesselQuery.SecondsPerDay();
                bool timerDone = hasReturn && GetI(c.Progress, $"c{ci}_timerDone", 0) == 1;

                // Bound vessel only transiently missing during scene/load changes: hold t0 instead
                // of resetting; elapsed time is recovered from absolute UT.
                bool subjectMissing = timerRunning && hasVesselChecks && subject == null;

                if (timerDone)
                {
                    c.Progress.SetValue($"c{ci}_rem", "0", true);
                }
                else if (timerRunning)
                {
                    if (subjectMissing)  { /* hold t0 */ }
                    else if (allTimerPrereqs) { /* keep counting with unchanged t0 */ }
                    else { t0 = -1.0; c.Progress.SetValue($"c{ci}_t0", "-1", true); } // real interruption -> restart
                }
                else if (allTimerPrereqs)
                {
                    t0 = ctx.UniversalTime; c.Progress.SetValue($"c{ci}_t0", t0.ToString("R", Inv), true); // start
                }

                double elapsed = t0 >= 0.0 ? ctx.UniversalTime - t0 : 0.0;
                double rem = Math.Max(0.0, required - elapsed);
                c.Progress.SetValue($"c{ci}_rem", rem.ToString("0", Inv), true);
                bool holdMet = timerDone || (!subjectMissing && allTimerPrereqs && t0 >= 0.0 && elapsed >= required);
                if (hasReturn && holdMet) c.Progress.SetValue($"c{ci}_timerDone", "1", true);
                met[timerIdx] = holdMet;
                overall = holdMet && allInstant;
            }
            else overall = allInstant;

            for (int j = 0; j < n; j++)
                c.Progress.SetValue($"c{ci}_{j}", met[j] ? "1" : "0", true);

            return overall;
        }

        /// <summary>Removes marker waypoints for all checks of this contract.</summary>
        public static void ClearMarkers(MissionContract c) => MarkerWaypoint.RemoveAll(c.Id);

        /// <summary>Stamps the ruleset used by a newly accepted mission and snapshots every
        /// delivery target before the supply craft can be merged into it. Loaded pre-0.8 active
        /// missions have no stamp and deliberately retain their former, less strict behavior.</summary>
        public static void InitializeAcceptedState(MissionContract c, StationRegistry stations)
        {
            if (c?.Progress == null) return;
            c.Progress.SetValue("evaluationSchema",
                StatePersistencePolicy.CurrentEvaluationSchema.ToString(Inv), true);
            for (int ci = 0; ci < c.Bedingungen.Count; ci++)
                for (int j = 0; j < c.Bedingungen[ci].Checks.Count; j++)
                {
                    Check check = c.Bedingungen[ci].Checks[j];
                    if (!check.IsDelivery) continue;
                    var station = stations?.Get(check.StationKey);
                    Vessel target = FindLiveVessel(station?.PersistentId ?? 0u);
                    if (target == null) continue;
                    ConfigNode state = StateNode(c.Progress, $"delivery{ci}_{j}");
                    state.SetValue("baseline", DeliveryAmount(target, check).ToString("R", Inv), true);
                    state.SetValue("targetId", target.persistentId.ToString(), true);
                }
        }

        public static double DeliveryProgress(MissionContract c, int conditionIndex, int checkIndex)
        {
            ConfigNode state = c?.Progress?.GetNode($"delivery{conditionIndex}_{checkIndex}");
            return NDouble(state, "delivered", 0.0);
        }

        private static bool EvalLegacyDelivery(Check check, Vessel subject)
        {
            // Checks introduced from nothing (new SOL/base delivery goals) are ignored for missions
            // already active during the upgrade. A converted FUEL_MIN keeps its original snapshot
            // semantics, including its strict greater-than comparison.
            if (!string.Equals(check.LegacyKind, "FUEL_MIN", StringComparison.OrdinalIgnoreCase))
                return true;
            return subject != null && VesselQuery.Fuel(subject) > check.Amount;
        }

        private static bool EvalDelivery(MissionContract c, int ci, int j, Check chk,
            EvaluationContext ctx, Vessel subject)
        {
            ConfigNode state = StateNode(c.Progress, $"delivery{ci}_{j}");
            if (NInt(state, "done") == 1) return true;
            var station = ctx.Stations?.Get(chk.StationKey);
            Vessel target = station == null ? null : ctx.FindVessel(station.PersistentId);
            if (target == null) return false;

            // Surface/base delivery: the separately landed supply vessel must physically reach
            // the recorded base. Its carried amount is the delivery, never the base's old stock.
            if (subject != null && subject.persistentId != target.persistentId &&
                subject.mainBody == target.mainBody)
            {
                double radius = (chk.Km > 0.0 ? chk.Km : 2.0) * 1000.0;
                if (Vector3d.Distance(subject.GetWorldPos3D(), target.GetWorldPos3D()) <= radius)
                {
                    double carried = DeliveryAmount(subject, chk);
                    state.SetValue("delivered", carried.ToString("R", Inv), true);
                    if (carried >= chk.Amount)
                    {
                        state.SetValue("done", "1", true);
                        return true;
                    }
                }
            }

            if (state.GetValue("baseline") == null)
            {
                state.SetValue("baseline", DeliveryAmount(target, chk).ToString("R", Inv), true);
                state.SetValue("targetId", target.persistentId.ToString(), true);
                return false;
            }

            uint acceptedTarget = GetUInt(state, "targetId");
            bool dockedNow = ctx.Events?.Dockings != null &&
                ctx.Events.Dockings.Any(ev => ev.Vessel != null && ev.Vessel.persistentId == target.persistentId);
            if (!dockedNow && ctx.Events?.Merges != null)
                dockedNow = ctx.Events.Merges.Any(merge =>
                    merge.IdA == acceptedTarget || merge.IdB == acceptedTarget ||
                    merge.IdA == target.persistentId || merge.IdB == target.persistentId);
            if (dockedNow) state.SetValue("docked", "1", true);
            if (NInt(state, "docked") != 1) return false;

            double baseline = NDouble(state, "baseline", DeliveryAmount(target, chk));
            double delivered = ResourceDeliveryPolicy.Accumulate(baseline, DeliveryAmount(target, chk),
                NDouble(state, "delivered", 0.0));
            state.SetValue("delivered", delivered.ToString("R", Inv), true);
            if (!ResourceDeliveryPolicy.Reached(delivered, chk.Amount)) return false;
            state.SetValue("done", "1", true);
            return true;
        }

        private static double DeliveryAmount(Vessel vessel, Check check) =>
            string.Equals(check.Resource, "Fuel", StringComparison.OrdinalIgnoreCase)
                ? VesselQuery.Fuel(vessel)
                : VesselQuery.Resource(vessel, check.Resource);

        private static Vessel FindLiveVessel(uint id)
        {
            if (id == 0 || FlightGlobals.Vessels == null) return null;
            return FlightGlobals.Vessels.FirstOrDefault(v => v != null && v.persistentId == id);
        }

        /// <summary>Remaps timer binding (c{ci}_vid) to the merged vessel when the bound vessel was
        /// absorbed by docking, avoiding a permanently paused timer with an obsolete persistentId.</summary>
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
                if (ctx.FindVessel(vid) != null) continue; // still exists
                foreach (var m in merges)
                {
                    uint other = vid == m.IdA ? m.IdB : (vid == m.IdB ? m.IdA : 0u);
                    if (ctx.FindVessel(other) != null)
                    {
                        c.Progress.SetValue(key, other.ToString(), true);
                        break;
                    }
                }
            }

            // Player-set bindings follow the same docking-survivor remap: a resupply docking a bound
            // station keeps the mission attached to the merged vessel.
            foreach (var m in merges)
            {
                uint av = MissionBinding.AssignedVid(c);
                if (av != 0 && ctx.FindVessel(av) == null)
                {
                    uint other = av == m.IdA ? m.IdB : (av == m.IdB ? m.IdA : 0u);
                    if (ctx.FindVessel(other) != null)
                        MissionBinding.RemapAssigned(c, av, other);
                }
                foreach (var fe in MissionBinding.Fleet(c))
                {
                    if (ctx.FindVessel(fe.Vid) != null) continue;
                    uint other = fe.Vid == m.IdA ? m.IdB : (fe.Vid == m.IdB ? m.IdA : 0u);
                    if (ctx.FindVessel(other) != null)
                        MissionBinding.FleetRemap(c, fe.Vid, other);
                }
            }
        }

        private static Vessel ResolveSubject(MissionContract c, int ci, Condition cond, EvaluationContext ctx,
                                             bool hasTimer, bool hasVesselChecks, bool timerRunning)
        {
            // Explicit single-vessel assignment overrides everything: only the bound vessel counts,
            // loaded or unloaded. Transient missing (scene load) returns null and the caller holds t0.
            uint assigned = MissionBinding.AssignedVid(c);
            if (assigned != 0)
                return ctx.FindVessel(assigned);

            // Fleet/network mission: use the first present assigned satellite as the representative
            // subject for the shared CREW_NONE / DURATION checks instead of the active vessel.
            if (MissionBinding.HasFleet(c))
            {
                foreach (var fe in MissionBinding.Fleet(c))
                {
                    var fv = ctx.FindVessel(fe.Vid);
                    if (fv != null) return fv;
                }
                return null;
            }

            // Without a timer or single-vessel checks, evaluate the active vessel.
            if (!hasTimer || !hasVesselChecks) return VesselQuery.Active;

            uint vid = GetUInt(c.Progress, $"c{ci}_vid");
            Vessel bound = vid != 0
                ? ctx.FindVessel(vid)
                : null;

            // Running timer means locked binding: only the bound vessel counts, even unfocused or
            // unloaded. No fallback or rebinding; transient missing returns null and the caller holds t0.
            if (timerRunning) return bound;

            // Timer not running yet: keep the bound vessel while it still qualifies; otherwise check
            // and bind the active vessel if it qualifies.
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

        /// <summary>Single-vessel check, public so subject tracking can reuse it.</summary>
        public static bool EvalVessel(Check chk, Vessel v)
        {
            if (v == null) return false;
            CelestialBody body = string.IsNullOrEmpty(chk.Body) ? v.mainBody : BodyResolver.Resolve(chk.Body);
            switch (chk.Kind)
            {
                case CheckKind.CREW_MIN:   return VesselQuery.EffectiveCrew(v) >= chk.Min;
                case CheckKind.CREW_NONE:  return v.GetCrewCount() == 0;
                case CheckKind.CREW_EXACT: return VesselQuery.EffectiveCrew(v) == chk.Min;
                case CheckKind.CREW_CAPACITY_MIN:
                    return VesselQuery.CrewCapacity(v) >= chk.Min;
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
                    // km > 0: fixed value; km <= 0: atmosphere height from API, no hardcode.
                    double minPe = chk.Km > 0 ? chk.Km * 1000.0 : body.atmosphereDepth;
                    return v.orbit.PeA > minPe;
                }
                case CheckKind.APOAPSIS_MAX:
                    return body != null && v.mainBody == body &&
                           v.situation == Vessel.Situations.ORBITING &&
                           v.orbit != null &&
                           chk.Km > 0 &&
                           v.orbit.ApA < chk.Km * 1000.0;
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
                case CheckKind.MASS_MIN:     return VesselQuery.Mass(v) >= chk.Amount;
                case CheckKind.MODULE_COUNT: return VesselQuery.ModuleCount(v, chk.Module) >= chk.Count;
                case CheckKind.POWER_CAPACITY_MIN:
                    return VesselQuery.ResourceCapacity(v, "ElectricCharge") >= chk.Amount;
                case CheckKind.DOCKING_PORT_COUNT:
                    return VesselQuery.ModuleCount(v, "ModuleDockingNode") >= chk.Count;
                case CheckKind.WHEEL_MOTION:
                    return body != null && v.mainBody == body &&
                           v.situation == Vessel.Situations.LANDED &&
                           HasWheelModule(v) &&
                           v.srfSpeed >= chk.SpeedMin;
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

            // StationRegistry is remapped to the docking survivor before evaluation. The merge
            // buffer therefore provides the most reliable match even when onPartCouple retained a
            // reference to the absorbed vessel.
            if (station != null && ctx.Events?.Merges != null && ctx.Events.Merges.Any(merge =>
                    merge.IdA == station.PersistentId || merge.IdB == station.PersistentId))
            {
                Vessel target = ctx.FindVessel(station.PersistentId);
                if (target != null && (body == null || target.mainBody == body) &&
                    VesselQuery.MatchesSituation(target, chk.Situation)) return true;
            }

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

        /// <summary>EVA check that survives an assigned subject: the EVA kerbal is its own vessel with
        /// a different persistentId, so the bound capsule never matches. Accept any EVA kerbal that is
        /// the active vessel, or one within ~2500 m of the mission anchor (assigned/representative
        /// vessel). Mirrors the proximity in <see cref="VesselQuery.EffectiveCrew"/>.</summary>
        private static bool EvalEvaForMission(Check chk, EvaluationContext ctx, Vessel anchor)
        {
            CelestialBody body = string.IsNullOrEmpty(chk.Body) ? null : BodyResolver.Resolve(chk.Body);
            Vessel active = VesselQuery.Active;

            // No anchor (unassigned): the active vessel is the subject, as before. When you go EVA the
            // active vessel becomes the kerbal, so this matches directly.
            if (anchor == null)
                return active != null && EvaMatches(chk, body, active);

            // Assigned: the EVA kerbal must belong to the anchor vessel, i.e. same SOI and within
            // ~2500 m, so an unrelated kerbal on another mission cannot complete this one.
            const double nearM = 2500.0;
            Vector3d pos = anchor.GetWorldPos3D();
            foreach (var v in ctx.Vessels)
            {
                if (v == null || ReferenceEquals(v, anchor) || !EvaMatches(chk, body, v)) continue;
                if (v.mainBody != anchor.mainBody) continue;
                if (Vector3d.Distance(v.GetWorldPos3D(), pos) <= nearM) return true;
            }
            return false;
        }

        private static bool EvaMatches(Check chk, CelestialBody body, Vessel v)
        {
            if (v == null || !v.isEVA) return false;
            if (body != null && v.mainBody != body) return false;
            return VesselQuery.MatchesSituation(v, chk.Situation);
        }

        private static bool EvalFleet(MissionContract c, Check chk, EvaluationContext ctx, bool legacyEvaluation)
        {
            CelestialBody body = BodyResolver.Resolve(chk.Body);
            if (body == null) return false;
            double minM = chk.Km * 1000.0;
            bool relayRequired = chk.Kind == CheckKind.RELAY_VESSEL_COUNT ||
                                 chk.Kind == CheckKind.RELAY_VESSEL_COUNT_INCLINATION ||
                                 chk.Kind == CheckKind.RELAY_NETWORK_TOPOLOGY;
            bool inclinationRequired = chk.Kind == CheckKind.VESSEL_COUNT_INCLINATION ||
                                       chk.Kind == CheckKind.RELAY_VESSEL_COUNT_INCLINATION ||
                                       (chk.Kind == CheckKind.RELAY_NETWORK_TOPOLOGY && chk.InclinationMin > 0.0);
            // Networks require active assignment: only assigned satellites count, so the player must
            // assign the constellation and foreign craft are never mistaken for the network.
            var members = ctx.RealVessels.Where(v =>
                MissionBinding.FleetContains(c, v.persistentId) &&
                v.mainBody == body &&
                v.situation == Vessel.Situations.ORBITING &&
                v.orbit != null &&
                (minM <= 0.0 || v.orbit.PeA > minM) &&
                (!inclinationRequired || v.orbit.inclination >= chk.InclinationMin) &&
                (!relayRequired || RelayCapability.IsOperational(v, out _))).ToList();
            int required = chk.Count + (chk.Kind == CheckKind.RELAY_NETWORK_TOPOLOGY && !legacyEvaluation
                ? Math.Max(0, chk.Redundancy) : 0);
            if (members.Count < required) return false;
            if (chk.Kind != CheckKind.RELAY_NETWORK_TOPOLOGY || legacyEvaluation) return true;
            return NetworkTopologyPolicy.Meets(members.Select(v => OrbitalPhase(v, ctx.UniversalTime)),
                chk.Count, chk.Redundancy, chk.SeparationMin, chk.MaxGap);
        }

        private static double OrbitalPhase(Vessel vessel, double ut)
        {
            try
            {
                double phase = vessel.orbit.getMeanAnomalyAtUT(ut) * Mathf.Rad2Deg;
                return NormalizeDegrees(phase);
            }
            catch (Exception) { }
            return NormalizeDegrees(vessel.longitude);
        }

        private static double NormalizeDegrees(double value)
        {
            value %= 360.0;
            return value < 0.0 ? value + 360.0 : value;
        }

        /// <summary>Whether a single assigned satellite currently qualifies for a fleet check, plus a
        /// short reason for the UI when it does not. Uses the base body/orbit/altitude/relay criteria.</summary>
        public static bool FleetMemberQualifies(Check chk, Vessel v, out string reason)
        {
            reason = "";
            if (chk == null) { reason = "no fleet check"; return false; }
            if (v == null) { reason = "not visible"; return false; }
            CelestialBody body = BodyResolver.Resolve(chk.Body);
            if (body == null) { reason = "unknown body"; return false; }
            if (v.mainBody != body) { reason = $"not at {chk.Body}"; return false; }
            if (v.situation != Vessel.Situations.ORBITING || v.orbit == null) { reason = "not in orbit"; return false; }
            double minM = chk.Km * 1000.0;
            if (minM > 0.0 && v.orbit.PeA <= minM) { reason = "periapsis too low"; return false; }
            bool inclinationRequired = chk.Kind == CheckKind.VESSEL_COUNT_INCLINATION ||
                                       chk.Kind == CheckKind.RELAY_VESSEL_COUNT_INCLINATION ||
                                       (chk.Kind == CheckKind.RELAY_NETWORK_TOPOLOGY && chk.InclinationMin > 0.0);
            if (inclinationRequired && v.orbit.inclination < chk.InclinationMin) { reason = "inclination too low"; return false; }
            bool relayRequired = chk.Kind == CheckKind.RELAY_VESSEL_COUNT ||
                                 chk.Kind == CheckKind.RELAY_VESSEL_COUNT_INCLINATION ||
                                 chk.Kind == CheckKind.RELAY_NETWORK_TOPOLOGY;
            if (relayRequired && !RelayCapability.IsOperational(v, out reason)) return false;
            reason = "in orbit";
            return true;
        }

        /// <summary>Flyby check: a real vessel enters the body's SOI, never orbits/lands there and
        /// leaves again. Optional closest PeA must be &lt;= km. State is per check and latches after success.</summary>
        private static bool EvalFlyby(MissionContract c, int ci, int j, Check chk, EvaluationContext ctx)
        {
            ConfigNode node = StateNode(c.Progress, $"fb{ci}_{j}");
            if (NInt(node, "done") == 1) return true;

            var body = BodyResolver.Resolve(chk.Body);
            if (body == null) return false;
            double thr = chk.Km > 0 ? chk.Km * 1000.0 : 0.0;
            bool completed = false;
            double bestApproach = Huge;

            // A flyby is flown by a single probe: when a vessel is assigned, only that probe (and
            // anything docked into it) counts, so a different craft passing the body does not finish it.
            uint assigned = MissionBinding.AssignedVid(c);

            foreach (var v in ctx.RealVessels)
            {
                if (assigned != 0 && v.persistentId != assigned) continue;
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
                    c.Progress.SetValue("fb_seen", "1", true);
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

        /// <summary>Marker landing check: choose a target once, ensure a visible waypoint after
        /// reloads, and complete once the active vessel is landed/splashed within km.</summary>
        private static bool EvalMarker(MissionContract c, int ci, int j, Check chk, EvaluationContext ctx)
        {
            var body = BodyResolver.Resolve(chk.Body);
            if (body == null) return false;
            string pfx = $"ml{ci}_{j}_";
            string wpId = $"{c.Id}#{ci}_{j}";

            // 1) Pick target once and persist it. Random per game seed but stable inside a save.
            //    Absolute latitude band defaults to near-equator; polar landings use high bands.
            if (GetI(c.Progress, pfx + "set", 0) != 1)
            {
                int gameSeed = HighLogic.CurrentGame != null ? HighLogic.CurrentGame.Seed : 0;
                var rng = new System.Random(gameSeed ^ DeterministicHash.Of($"{c.Id}#{ci}_{j}"));
                double aMin = Math.Min(chk.LatAbsMin, chk.LatAbsMax);
                double aMax = Math.Max(chk.LatAbsMin, chk.LatAbsMax);
                double absLat = rng.NextDouble() * (aMax - aMin) + aMin;
                double lat = rng.Next(2) == 0 ? absLat : -absLat;   // northern or southern hemisphere
                double lon = rng.NextDouble() * 360.0 - 180.0;
                // Ocean bodies: redraw deterministically until the target is on land, so a
                // precision landing never asks for a splashdown site.
                for (int attempt = 0; attempt < 25 && MarkerWaypoint.IsWater(body, lat, lon); attempt++)
                {
                    absLat = rng.NextDouble() * (aMax - aMin) + aMin;
                    lat = rng.Next(2) == 0 ? absLat : -absLat;
                    lon = rng.NextDouble() * 360.0 - 180.0;
                }
                c.Progress.SetValue(pfx + "lat", lat.ToString("R", Inv), true);
                c.Progress.SetValue(pfx + "lon", lon.ToString("R", Inv), true);
                c.Progress.SetValue(pfx + "set", "1", true);
            }
            double mLat = GetD(c.Progress, pfx + "lat", 0.0);
            double mLon = GetD(c.Progress, pfx + "lon", 0.0);

            // 2) Ensure visible waypoint; object state only lives at runtime.
            if (!MarkerWaypoint.Has(wpId))
                MarkerWaypoint.Set(wpId, body, mLat, mLon, c.Titel, DeterministicHash.Of(wpId) % 10000);

            // 3) Landed + distance.
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

        /// <summary>Sequential return check: first log a crewed landing/splashdown on the source
        /// body, or a crewed SOI visit in flyby mode, then complete once a crewed vessel lands or
        /// splashes down on the return body. The return is intentionally forgiving across staging
        /// and docking, because the crew capsule often is not the same vessel object that reached
        /// the destination.</summary>
        private static bool EvalReturn(MissionContract c, int ci, int j, Check chk, EvaluationContext ctx)
        {
            return ReturnEvaluation.Evaluate(c, ci, j, chk, ctx);
        }
        private static bool HasWheelModule(Vessel v)
        {
            if (v == null || v.parts == null) return false;
            foreach (var p in v.parts)
            {
                if (p == null || p.Modules == null) continue;
                foreach (PartModule module in p.Modules)
                {
                    string name = module?.moduleName ?? module?.GetType().Name ?? "";
                    if (name.IndexOf("ModuleWheel", StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }
            return false;
        }

        // ---- ConfigNode state helpers ----
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

    /// <summary>Lifecycle hooks for COMPOSITE conditions. Actual evaluation runs through
    /// <see cref="CheckEvaluation"/>; claim/abandon needs to remove marker waypoints.</summary>
    public sealed class CompositeEvaluator : ConditionEvaluatorBase
    {
        public override ConditionType Type => ConditionType.COMPOSITE;
        public override bool Evaluate(MissionContract c, Condition cond, EvaluationContext ctx) => false;
        public override void OnCleared(MissionContract c, Condition cond) => CheckEvaluation.ClearMarkers(c);
    }
}
