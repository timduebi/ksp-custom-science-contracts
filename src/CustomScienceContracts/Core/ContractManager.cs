using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Conditions;
using CustomScienceContracts.Data;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Controls status flow, prerequisites, acceptance/completion, reward payout and
    /// repeatable cooldown bookkeeping. Ticked by the ScenarioModule.</summary>
    public class ContractManager
    {
        public ContractCatalog Catalog { get; } = new ContractCatalog();
        public ConditionEvaluatorRegistry Evaluators { get; } = new ConditionEvaluatorRegistry();
        public GameEventBridge Events { get; } = new GameEventBridge();
        public StationRegistry Stations { get; } = new StationRegistry();
        public FleetRegistry Fleets { get; } = new FleetRegistry();

        /// <summary>Global factor for all paid science rewards (settings, 0.1-3.0).</summary>
        public double ScienceMultiplier = 1.0;

        /// <summary>Whether a contract counts as completed at least once, including repeatables in the pool.</summary>
        public static bool IsCompleted(MissionContract c) =>
            c != null && (c.TotalCompletions > 0 || c.Status == MissionStatus.CompletedOnce);

        public void Initialize(IEnumerable<MissionContract> contracts)
        {
            Catalog.Set(contracts);
            RecomputeAvailability();
        }

        // --- Availability ---

        /// <summary>Sets locked contracts with fulfilled prerequisites to Available. Does not touch Active/CompletedOnce.</summary>
        /// <summary>Last claim, used for the transient "+X science" display.</summary>
        public string LastClaimId { get; private set; }
        public float LastClaimAmount { get; private set; }
        public float LastClaimRealtime { get; private set; }

        public void RecomputeAvailability()
        {
            foreach (var c in Catalog.All)
            {
                if (c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim ||
                    c.Status == MissionStatus.CompletedOnce)
                    continue;
                bool prereqsMet = Tuning.UnlockAll || c.Voraussetzungen.All(id => IsCompleted(Catalog.Get(id)));
                c.Status = prereqsMet ? MissionStatus.Available : MissionStatus.Locked;
            }
        }

        public bool CanAccept(MissionContract c)
        {
            if (c == null || c.Status != MissionStatus.Available) return false;
            // Unlock/test mode: no active limits and no repeatable cooldown.
            if (Tuning.UnlockAll) return true;
            if (ActiveLimits.ActiveCount(Catalog.All, c.HeimatSparte) >= ActiveLimits.LimitFor(c.HeimatSparte))
                return false;
            // Repeatable in the pool: cooldown must be over.
            if (c.IsRepeatableInPool && c.CompletionsSinceLastClaim < Tuning.RepeatableCooldown)
                return false;
            return true;
        }

        public bool Accept(string id)
        {
            var c = Catalog.Get(id);
            if (!CanAccept(c)) return false;
            c.Status = MissionStatus.Active;
            c.Progress = new ConfigNode("PROGRESS");
            InheritFleetFromPrereqs(c);   // follow-up networks pre-fill the predecessor's satellites
            AutoAssignStationOnAccept(c);  // longstay/expand pin the registered station automatically
            Evaluators.NotifyAccepted(c);
            Debug.Log($"[CSC] Accepted: {c.Id}");
            return true;
        }

        /// <summary>Abort penalty: half of the multiplied reward, rounded.</summary>
        public float AbortPenalty(MissionContract c) =>
            c == null ? 0f : Mathf.Round(c.ScienceReward * 0.5f * (float)ScienceMultiplier);

        public bool Abandon(string id)
        {
            var c = Catalog.Get(id);
            if (c == null || c.Status != MissionStatus.Active) return false;

            // Penalty: subtract half the reward from the current science balance, never below zero.
            float penalty = AbortPenalty(c);
            if (penalty > 0f && ResearchAndDevelopment.Instance != null)
            {
                float pen = Mathf.Min(penalty, ResearchAndDevelopment.Instance.Science);
                if (pen > 0f) ResearchAndDevelopment.Instance.AddScience(-pen, TransactionReasons.ScienceTransmission);
            }

            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");
            // Back into the correct pool: pool repeatables remain available, others return to Available/Locked.
            c.Status = MissionStatus.Available;
            RecomputeAvailability();
            Debug.Log($"[CSC] Abandoned: {c.Id} (-{penalty} science)");
            return true;
        }

        /// <summary>Emergency exit from the settings window: mark an active mission as complete
        /// to unlock follow-ups, but without paying science.</summary>
        public bool Skip(string id)
        {
            var c = Catalog.Get(id);
            if (c == null || (c.Status != MissionStatus.Active && c.Status != MissionStatus.ReadyToClaim)) return false;
            StoreFleetIfNetwork(c);
            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");
            c.TotalCompletions++;
            RecordFirstCompletion(c);
            AdvanceRepeatableCooldowns(c);   // a skip is still a completion for cooldown purposes
            if (c.Repeatable) { c.CompletionsSinceLastClaim = 0; c.Status = MissionStatus.Available; }
            else c.Status = MissionStatus.CompletedOnce;
            Log.Info($"Skipped without reward: {c.Id}");
            RecomputeAvailability();
            if (c.TotalCompletions == 1) AnnounceEpochIfComplete(c);
            return true;
        }

        // --- Vessel binding ---

        /// <summary>A network/fleet mission counts multiple vessels (VESSEL_COUNT / RELAY_...).</summary>
        public static bool IsFleetMission(MissionContract c)
        {
            if (c == null) return false;
            foreach (var cond in c.Bedingungen)
                foreach (var chk in cond.Checks)
                    if (chk.IsFleet) return true;
            return false;
        }

        /// <summary>A single-vessel mission (incl. flyby) that has at least one real single-vessel
        /// check and can be pinned to one assigned vessel.</summary>
        public static bool IsSingleBindable(MissionContract c)
        {
            if (c == null || IsFleetMission(c)) return false;
            foreach (var cond in c.Bedingungen)
                foreach (var chk in cond.Checks)
                    if (!chk.IsSpecial) return true;
            return false;
        }

        public static bool IsBindable(MissionContract c) => IsFleetMission(c) || IsSingleBindable(c);

        /// <summary>Identity-establishing missions (those that record a station) must have a vessel
        /// assigned before they complete, so the recorded station is correct for all follow-ups.</summary>
        public static bool RequiresAssignment(MissionContract c) =>
            c != null && !string.IsNullOrEmpty(c.RecordStationKey);

        /// <summary>Longstay/expand missions track the existing station: they reference it (stationRef)
        /// but neither record it nor dock a new craft, so the registered station is auto-assigned on
        /// accept. Supply missions (with DOCK_STATION) keep the player's supply ship instead.</summary>
        private static bool AutoAssignsStation(MissionContract c)
        {
            if (c == null || string.IsNullOrEmpty(c.StationRef) || !string.IsNullOrEmpty(c.RecordStationKey))
                return false;
            foreach (var cond in c.Bedingungen)
                foreach (var chk in cond.Checks)
                    if (chk.Kind == CheckKind.DOCK_STATION) return false;
            return true;
        }

        private void AutoAssignStationOnAccept(MissionContract c)
        {
            if (!AutoAssignsStation(c)) return;
            var entry = Stations.Get(c.StationRef);
            if (entry == null || entry.PersistentId == 0) return;
            MissionBinding.Assign(c, entry.PersistentId, entry.Name);
            for (int i = 0; i < c.Bedingungen.Count; i++)
                c.Progress.SetValue($"c{i}_vid", entry.PersistentId.ToString(), true);
            Log.Info($"Auto-assigned station '{c.StationRef}' (id {entry.PersistentId}) to {c.Id}");
        }

        private void InheritFleetFromPrereqs(MissionContract c)
        {
            if (!IsFleetMission(c)) return;
            foreach (string preId in c.Voraussetzungen)
            {
                var rec = Fleets.Get(preId);
                if (rec == null) continue;
                foreach (var m in rec) MissionBinding.FleetAdd(c, m.Vid, m.Name);
            }
        }

        private void StoreFleetIfNetwork(MissionContract c)
        {
            if (!IsFleetMission(c)) return;
            var members = MissionBinding.Fleet(c)
                .Select(fe => new FleetRegistry.Member { Vid = fe.Vid, Name = fe.Name })
                .ToList();
            Fleets.Set(c.Id, members);
        }

        /// <summary>Pin the active vessel to a mission: a single binding, or a fleet member for
        /// network missions. Only valid in flight on an active mission.</summary>
        public bool AssignActiveVessel(string id)
        {
            var c = Catalog.Get(id);
            if (c == null || c.Status != MissionStatus.Active) return false;
            var v = Conditions.VesselQuery.Active;
            if (v == null) return false;

            if (IsFleetMission(c))
            {
                MissionBinding.FleetAdd(c, v.persistentId, v.vesselName);
            }
            else
            {
                MissionBinding.Assign(c, v.persistentId, v.vesselName);
                // Re-lock per-condition timers/bindings onto the new subject so they restart cleanly.
                for (int i = 0; i < c.Bedingungen.Count; i++)
                {
                    c.Progress.SetValue($"c{i}_vid", v.persistentId.ToString(), true);
                    c.Progress.RemoveValue($"c{i}_t0");
                }
            }
            Log.Info($"Assigned vessel {v.persistentId} ({v.vesselName}) to {c.Id}");
            return true;
        }

        /// <summary>Drop the single-vessel binding (mission falls back to the active vessel).</summary>
        public bool ClearAssignment(string id)
        {
            var c = Catalog.Get(id);
            if (c == null) return false;
            MissionBinding.Clear(c);
            for (int i = 0; i < c.Bedingungen.Count; i++)
            {
                c.Progress.RemoveValue($"c{i}_vid");
                c.Progress.RemoveValue($"c{i}_t0");
            }
            return true;
        }

        public bool RemoveFleetVessel(string id, uint vid)
        {
            var c = Catalog.Get(id);
            if (c == null) return false;
            MissionBinding.FleetRemove(c, vid);
            return true;
        }

        /// <summary>Assigned satellites of a network mission with their current qualifying status.</summary>
        public List<FleetMemberInfo> FleetMembers(MissionContract c)
        {
            var result = new List<FleetMemberInfo>();
            if (c == null) return result;
            Model.Check fleetChk = null;
            foreach (var cond in c.Bedingungen)
            {
                foreach (var chk in cond.Checks) if (chk.IsFleet) { fleetChk = chk; break; }
                if (fleetChk != null) break;
            }
            foreach (var fe in MissionBinding.Fleet(c))
            {
                var v = FlightGlobals.Vessels?.FirstOrDefault(x => x != null && x.persistentId == fe.Vid);
                var info = new FleetMemberInfo { Vid = fe.Vid, Name = fe.Name, Present = v != null };
                if (v == null) { info.Qualifies = false; info.Reason = "not visible"; }
                else info.Qualifies = Conditions.CheckEvaluation.FleetMemberQualifies(fleetChk, v, out info.Reason);
                result.Add(info);
            }
            return result;
        }

        /// <summary>Reacts to recovery/destruction of an assigned vessel. Recovery at home completes
        /// an open crewed return; otherwise the binding is marked lost (timer pauses, not resets).
        /// A destroy is debounced one tick so a scene-load drop is not mistaken for a real loss.</summary>
        private void ProcessAssignedLifecycle(MissionContract c, EvaluationContext ctx)
        {
            uint vid = MissionBinding.AssignedVid(c);
            if (vid == 0 || c.Progress == null) return;

            bool recovered = false, destroyed = false;
            var events = ctx.Events?.Lifecycles;
            if (events != null)
                for (int i = 0; i < events.Count; i++)
                    if (events[i].Id == vid) { if (events[i].Recovered) recovered = true; else destroyed = true; }

            bool present = ctx.Vessels != null && ctx.Vessels.Any(v => v != null && v.persistentId == vid);

            if (recovered)
            {
                // Recovered on the home body: complete an open crewed return, else the craft is gone.
                MissionBinding.SetLost(c, !TryCompleteReturnOnRecovery(c));
                c.Progress.RemoveValue("assignedDestroyPending");
                return;
            }
            if (present)
            {
                c.Progress.RemoveValue("assignedDestroyPending");
                return;
            }
            if (destroyed)
            {
                if (c.Progress.GetValue("assignedDestroyPending") == "1") MissionBinding.SetLost(c, true);
                else c.Progress.SetValue("assignedDestroyPending", "1", true);
                return;
            }
            // No event and gone: confirm a pending destroy (it did not reappear), else hold (transient).
            if (c.Progress.GetValue("assignedDestroyPending") == "1") MissionBinding.SetLost(c, true);
        }

        /// <summary>Marks an open crewed RETURN check as done when its source visit was already
        /// logged, so a vessel recovered at home still pays out. Returns false when the mission has no
        /// such pending return.</summary>
        private static bool TryCompleteReturnOnRecovery(MissionContract c)
        {
            bool any = false;
            for (int ci = 0; ci < c.Bedingungen.Count; ci++)
            {
                var cond = c.Bedingungen[ci];
                for (int j = 0; j < cond.Checks.Count; j++)
                {
                    if (!cond.Checks[j].IsReturn) continue;
                    var node = c.Progress.GetNode($"ret{ci}_{j}");
                    if (node == null) continue;
                    if (node.GetValue("done") == "1") { any = true; continue; }
                    if (node.GetValue("seenSource") == "1")
                    {
                        node.SetValue("done", "1", true);
                        c.Progress.SetValue("ret_status", "returned", true);
                        any = true;
                    }
                }
            }
            return any;
        }

        // --- Check loop ---

        /// <summary>Checks active contracts; fulfilled contracts become ReadyToClaim, but are not
        /// paid yet. Stores the fulfilled state per condition for the UI.</summary>
        public void Tick(EvaluationContext ctx)
        {
            // Follow recorded stations across docking merges once per tick, so a resupply docking does
            // not leave DOCK_STATION matching or station tracking pointing at a vanished id.
            if (Events.Merges.Count > 0) Stations.Remap(Events.Merges, ctx.Vessels);

            var active = Catalog.All.Where(c => c.Status == MissionStatus.Active).ToList();
            foreach (var c in active)
            {
                // Recovery/destruction of an assigned vessel before reading bindings, so a returned
                // crew can complete and a lost craft pauses instead of resetting.
                ProcessAssignedLifecycle(c, ctx);
                // Process docking merges before reading bindings, so timers survive resupply docking.
                Conditions.CheckEvaluation.RemapDockedSubjects(c, ctx);
                if (EvaluateAll(c, ctx))
                {
                    // Identity-establishing missions only complete once a vessel is assigned, so the
                    // recorded station is the intended one, not whatever happens to be active.
                    if (RequiresAssignment(c) && MissionBinding.AssignedVid(c) == 0)
                        continue;
                    if (!string.IsNullOrEmpty(c.RecordStationKey))
                    {
                        uint vid = MissionBinding.AssignedVid(c);
                        if (vid != 0) Stations.Record(c.RecordStationKey, vid, MissionBinding.AssignedName(c));
                        else if (Conditions.VesselQuery.Active != null)
                            Stations.Record(c.RecordStationKey, Conditions.VesselQuery.Active);
                    }
                    c.Status = MissionStatus.ReadyToClaim;
                    Log.Info($"Ready to claim: {c.Id}");
                    Toast($"Mission ready to claim: {c.Titel}");
                }
            }
            Events.ClearFrameBuffer();
        }

        /// <summary>Evaluates all conditions without short-circuiting, writes cond{i}_met to
        /// progress and returns true when all are fulfilled.</summary>
        private bool EvaluateAll(MissionContract c, EvaluationContext ctx)
        {
            bool all = c.Bedingungen.Count > 0;
            for (int i = 0; i < c.Bedingungen.Count; i++)
            {
                var cond = c.Bedingungen[i];
                bool met = cond.Checks.Count > 0
                    ? Conditions.CheckEvaluation.Evaluate(c, i, cond, ctx)            // hand-composed checklist
                    : Evaluators.Get(cond.Type).Evaluate(c, cond, ctx);              // legacy single condition
                c.Progress.SetValue($"cond{i}_met", met ? "1" : "0", true);
                if (!met) all = false;
            }
            return all;
        }

        /// <summary>Whether condition i was fulfilled in the last tick. Used for red/green UI state.</summary>
        public bool IsConditionMet(MissionContract c, int i) =>
            c.Status == MissionStatus.ReadyToClaim || c.Progress.GetValue($"cond{i}_met") == "1";

        /// <summary>Whether checklist item j of condition i is fulfilled. Used for red/green UI state.</summary>
        public bool IsCheckMet(MissionContract c, int i, int j) =>
            c.Status == MissionStatus.ReadyToClaim || c.Progress.GetValue($"c{i}_{j}") == "1";

        /// <summary>Remaining seconds for a timer check of condition i, or -1.</summary>
        public double CheckRemaining(MissionContract c, int i) =>
            double.TryParse(c.Progress.GetValue($"c{i}_rem"),
                System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture,
                out double r) ? r : -1.0;

        /// <summary>Claims a ready mission: pays reward and unlocks follow-up missions.</summary>
        public bool Claim(string id)
        {
            var c = Catalog.Get(id);
            if (c == null || c.Status != MissionStatus.ReadyToClaim) return false;

            // Snapshot for the "Unlocked: ..." toast after availability is recomputed.
            var availableBefore = new HashSet<string>();
            foreach (var x in Catalog.All)
                if (x.Status == MissionStatus.Available) availableBefore.Add(x.Id);

            float amount = c.ScienceReward * (float)ScienceMultiplier;
            PayReward(amount);
            LastClaimId = c.Id; LastClaimAmount = amount; LastClaimRealtime = Time.realtimeSinceStartup;
            c.TotalCompletions++;
            RecordFirstCompletion(c);
            StoreFleetIfNetwork(c);   // keep the constellation so a follow-up network can inherit it
            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");
            AdvanceRepeatableCooldowns(c);

            if (c.Repeatable)
            {
                c.CompletionsSinceLastClaim = 0;
                c.Status = MissionStatus.Available;
            }
            else c.Status = MissionStatus.CompletedOnce;

            Log.Info($"Claimed: {c.Id} (+{c.ScienceReward} science)");
            RecomputeAvailability();

            Toast($"+{amount:0} Science — {c.Titel}");
            AnnounceUnlocked(c, availableBefore);
            if (c.TotalCompletions == 1) AnnounceEpochIfComplete(c);
            return true;
        }

        /// <summary>Stamps the in-game time of the first completion for the atlas chronicle.</summary>
        private static void RecordFirstCompletion(MissionContract c)
        {
            if (c.FirstCompletedUT < 0.0)
                c.FirstCompletedUT = Planetarium.GetUniversalTime();
        }

        /// <summary>Toast listing missions that just became available, capped at three titles.</summary>
        private void AnnounceUnlocked(MissionContract claimed, HashSet<string> availableBefore)
        {
            var unlocked = new List<string>();
            foreach (var x in Catalog.All)
                if (x.Status == MissionStatus.Available && !availableBefore.Contains(x.Id) &&
                    !ReferenceEquals(x, claimed))
                    unlocked.Add(x.Titel);
            if (unlocked.Count == 0) return;

            string titles = string.Join(", ", unlocked.Take(3));
            if (unlocked.Count > 3) titles += $" (+{unlocked.Count - 3} more)";
            Toast("Unlocked: " + titles);
        }

        /// <summary>Toast when the first completion of a mission finishes its whole epoch.</summary>
        private void AnnounceEpochIfComplete(MissionContract c)
        {
            int epoch = Mathf.Max(1, c.Epoch);
            foreach (var other in Catalog.All)
                if (Mathf.Max(1, other.Epoch) == epoch && !IsCompleted(other)) return;
            string name = Catalog.Epoch(epoch)?.Name;
            Toast($"Epoch complete: {(string.IsNullOrEmpty(name) ? "Epoch " + epoch : name)}");
        }

        /// <summary>On-screen message; missions complete in scenes without ScreenMessages too,
        /// so failures are ignored.</summary>
        private static void Toast(string message)
        {
            try
            {
                if (ScreenMessages.Instance != null)
                    ScreenMessages.PostScreenMessage(message, 6f, ScreenMessageStyle.UPPER_CENTER);
            }
            catch (System.Exception) { }
        }

        /// <summary>Cooldown bookkeeping: every pool repeatable except the completed one gets +1.
        /// Runs on every completion (claim or skip), so cooldowns advance consistently.</summary>
        private void AdvanceRepeatableCooldowns(MissionContract completed)
        {
            foreach (var other in Catalog.All)
                if (!ReferenceEquals(other, completed) && other.IsRepeatableInPool)
                    other.CompletionsSinceLastClaim++;
        }

        private static void PayReward(float amount)
        {
            if (amount <= 0f) return;
            if (ResearchAndDevelopment.Instance != null)
                ResearchAndDevelopment.Instance.AddScience(amount, TransactionReasons.ScienceTransmission);
            else
                Debug.LogWarning("[CSC] No ResearchAndDevelopment instance (not Science/Career?) - reward lost.");
        }

        // --- UI queries ---

        public IEnumerable<MissionContract> ActiveContracts() =>
            Catalog.All.Where(c => c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim);

        public IEnumerable<MissionContract> RepeatablePool() =>
            Catalog.All.Where(c => c.IsRepeatableInPool);

        public int RemainingCooldown(MissionContract c) =>
            Mathf.Max(0, Tuning.RepeatableCooldown - c.CompletionsSinceLastClaim);

        /// <summary>Titles of unmet prerequisites for the red UI line.</summary>
        public List<string> UnmetPrerequisiteTitles(MissionContract c)
        {
            var list = new List<string>();
            foreach (string id in c.Voraussetzungen)
            {
                var pre = Catalog.Get(id);
                if (!IsCompleted(pre)) list.Add(pre != null ? pre.Titel : id);
            }
            return list;
        }
    }

    /// <summary>UI row for one assigned satellite of a network mission.</summary>
    public class FleetMemberInfo
    {
        public uint Vid;
        public string Name;
        public bool Present;
        public bool Qualifies;
        public string Reason;
    }
}
