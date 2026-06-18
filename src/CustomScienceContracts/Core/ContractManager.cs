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
                if (pen > 0f) ResearchAndDevelopment.Instance.AddScience(-pen, TransactionReasons.Cheating);
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
            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");
            c.TotalCompletions++;
            if (c.Repeatable) { c.CompletionsSinceLastClaim = 0; c.Status = MissionStatus.Available; }
            else c.Status = MissionStatus.CompletedOnce;
            Log.Info($"Skipped without reward: {c.Id}");
            RecomputeAvailability();
            return true;
        }

        // --- Check loop ---

        /// <summary>Checks active contracts; fulfilled contracts become ReadyToClaim, but are not
        /// paid yet. Stores the fulfilled state per condition for the UI.</summary>
        public void Tick(EvaluationContext ctx)
        {
            var active = Catalog.All.Where(c => c.Status == MissionStatus.Active).ToList();
            foreach (var c in active)
            {
                // Process docking merges before reading bindings, so timers survive resupply docking.
                Conditions.CheckEvaluation.RemapDockedSubjects(c, ctx);
                if (EvaluateAll(c, ctx))
                {
                    c.Status = MissionStatus.ReadyToClaim;
                    // "Build station/base" contract: remember the active vessel that fulfilled it.
                    // Only meaningful in flight; Space Center/Editor have no active vessel.
                    if (!string.IsNullOrEmpty(c.RecordStationKey) && Conditions.VesselQuery.Active != null)
                        Stations.Record(c.RecordStationKey, Conditions.VesselQuery.Active);
                    Log.Info($"Ready to claim: {c.Id}");
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

            float amount = c.ScienceReward * (float)ScienceMultiplier;
            PayReward(amount);
            LastClaimId = c.Id; LastClaimAmount = amount; LastClaimRealtime = Time.realtimeSinceStartup;
            c.TotalCompletions++;
            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");

            // Cooldown bookkeeping: every other pool repeatable gets +1.
            foreach (var other in Catalog.All)
                if (!ReferenceEquals(other, c) && other.IsRepeatableInPool)
                    other.CompletionsSinceLastClaim++;

            if (c.Repeatable)
            {
                c.CompletionsSinceLastClaim = 0;
                c.Status = MissionStatus.Available;
            }
            else c.Status = MissionStatus.CompletedOnce;

            Log.Info($"Claimed: {c.Id} (+{c.ScienceReward} science)");
            RecomputeAvailability();
            return true;
        }

        private static void PayReward(float amount)
        {
            if (amount <= 0f) return;
            if (ResearchAndDevelopment.Instance != null)
                ResearchAndDevelopment.Instance.AddScience(amount, TransactionReasons.Cheating);
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

        /// <summary>Whether locked mission previews are active because the trigger contract is completed.</summary>
        public bool LockedPreviewActive =>
            IsCompleted(Catalog.Get(Tuning.LockedPreviewTrigger));

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
}
