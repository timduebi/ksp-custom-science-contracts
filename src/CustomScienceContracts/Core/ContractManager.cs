using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Conditions;
using CustomScienceContracts.Data;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Steuert Status-Flow, Voraussetzungen, Annehmen/Abschluss, Reward-Auszahlung und
    /// die Repeatable-Cooldown-Buchhaltung. Wird vom ScenarioModule pro Tick getickt.</summary>
    public class ContractManager
    {
        public ContractCatalog Catalog { get; } = new ContractCatalog();
        public ConditionEvaluatorRegistry Evaluators { get; } = new ConditionEvaluatorRegistry();
        public GameEventBridge Events { get; } = new GameEventBridge();
        public StationRegistry Stations { get; } = new StationRegistry();

        /// <summary>Globaler Faktor auf alle ausgezahlten Wissenschaftspunkte (Einstellungen, 0.1–3.0).</summary>
        public double ScienceMultiplier = 1.0;

        /// <summary>Gilt der Contract als (mindestens einmal) abgeschlossen? Auch Repeatables im Pool.</summary>
        public static bool IsCompleted(MissionContract c) =>
            c != null && (c.TotalCompletions > 0 || c.Status == MissionStatus.CompletedOnce);

        public void Initialize(IEnumerable<MissionContract> contracts)
        {
            Catalog.Set(contracts);
            RecomputeAvailability();
        }

        // --- Verfuegbarkeit ---

        /// <summary>Setzt Locked-Contracts mit erfuellten Voraussetzungen auf Available.
        /// Beruehrt Active/CompletedOnce nicht.</summary>
        /// <summary>Letzte Einloesung (fuer die transiente "+X Wissenschaft"-Anzeige).</summary>
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
            // Freischalt-/Testmodus: keine Aktiv-Limits und kein Repeatable-Cooldown.
            if (Tuning.UnlockAll) return true;
            if (ActiveLimits.ActiveCount(Catalog.All, c.HeimatSparte) >= ActiveLimits.LimitFor(c.HeimatSparte))
                return false;
            // Repeatable im Pool: Cooldown muss abgelaufen sein.
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
            Debug.Log($"[CSC] Angenommen: {c.Id}");
            return true;
        }

        /// <summary>Wissenschaftsstrafe beim Abbrechen: halbe (multiplizierte) Belohnung, gerundet.</summary>
        public float AbortPenalty(MissionContract c) =>
            c == null ? 0f : Mathf.Round(c.ScienceReward * 0.5f * (float)ScienceMultiplier);

        public bool Abandon(string id)
        {
            var c = Catalog.Get(id);
            if (c == null || c.Status != MissionStatus.Active) return false;

            // Strafe: halbe Belohnung vom aktuellen Punktestand abziehen (nicht unter 0).
            float penalty = AbortPenalty(c);
            if (penalty > 0f && ResearchAndDevelopment.Instance != null)
            {
                float pen = Mathf.Min(penalty, ResearchAndDevelopment.Instance.Science);
                if (pen > 0f) ResearchAndDevelopment.Instance.AddScience(-pen, TransactionReasons.Cheating);
            }

            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");
            // zurueck in den richtigen Pool: Pool-Repeatable bleibt annehmbar, sonst Available/Locked
            c.Status = MissionStatus.Available;
            RecomputeAvailability();
            Debug.Log($"[CSC] Verworfen: {c.Id} (-{penalty} Wissenschaft)");
            return true;
        }

        /// <summary>Notausgang aus dem Einstellungsfenster: aktive Mission als erledigt markieren
        /// (schaltet Folgemissionen frei), aber OHNE Wissenschaftsauszahlung.</summary>
        public bool Skip(string id)
        {
            var c = Catalog.Get(id);
            if (c == null || (c.Status != MissionStatus.Active && c.Status != MissionStatus.ReadyToClaim)) return false;
            Evaluators.NotifyCleared(c);
            c.Progress = new ConfigNode("PROGRESS");
            c.TotalCompletions++;
            if (c.Repeatable) { c.CompletionsSinceLastClaim = 0; c.Status = MissionStatus.Available; }
            else c.Status = MissionStatus.CompletedOnce;
            Log.Info($"Übersprungen ohne Punkte: {c.Id}");
            RecomputeAvailability();
            return true;
        }

        // --- Pruef-Loop ---

        /// <summary>Prueft Active-Contracts; bei Erfuellung -> ReadyToClaim (gruen, aber noch nicht
        /// ausgezahlt). Speichert pro Bedingung den Erfuellt-Status fuer die UI.</summary>
        public void Tick(EvaluationContext ctx)
        {
            var active = Catalog.All.Where(c => c.Status == MissionStatus.Active).ToList();
            foreach (var c in active)
            {
                // Andock-Fusionen verarbeiten, bevor die Bindung gelesen wird (Timer ueberlebt Resupply).
                Conditions.CheckEvaluation.RemapDockedSubjects(c, ctx);
                if (EvaluateAll(c, ctx))
                {
                    c.Status = MissionStatus.ReadyToClaim;
                    // "Station/Basis bauen"-Auftrag: das erfuellende (aktive) Vessel als Station merken.
                    // Nur im Flug sinnvoll — im Space Center/Editor gibt es kein aktives Vessel.
                    if (!string.IsNullOrEmpty(c.RecordStationKey) && Conditions.VesselQuery.Active != null)
                        Stations.Record(c.RecordStationKey, Conditions.VesselQuery.Active);
                    Log.Info($"Bereit zum Einloesen: {c.Id}");
                }
            }
            Events.ClearFrameBuffer();
        }

        /// <summary>Wertet ALLE Bedingungen aus (ohne Kurzschluss), legt cond{i}_met in Progress ab
        /// und liefert true, wenn alle erfuellt sind.</summary>
        private bool EvaluateAll(MissionContract c, EvaluationContext ctx)
        {
            bool all = c.Bedingungen.Count > 0;
            for (int i = 0; i < c.Bedingungen.Count; i++)
            {
                var cond = c.Bedingungen[i];
                bool met = cond.Checks.Count > 0
                    ? Conditions.CheckEvaluation.Evaluate(c, i, cond, ctx)            // hand-komponierte Checkliste
                    : Evaluators.Get(cond.Type).Evaluate(c, cond, ctx);              // Legacy-Einzeltyp
                c.Progress.SetValue($"cond{i}_met", met ? "1" : "0", true);
                if (!met) all = false;
            }
            return all;
        }

        /// <summary>Ist Bedingung i (laut letztem Tick) erfuellt? Fuer die UI-Anzeige rot/gruen.</summary>
        public bool IsConditionMet(MissionContract c, int i) =>
            c.Status == MissionStatus.ReadyToClaim || c.Progress.GetValue($"cond{i}_met") == "1";

        /// <summary>Ist Teilziel j von Bedingung i erfuellt? (COMPOSITE-Checkliste, UI rot/gruen).</summary>
        public bool IsCheckMet(MissionContract c, int i, int j) =>
            c.Status == MissionStatus.ReadyToClaim || c.Progress.GetValue($"c{i}_{j}") == "1";

        /// <summary>Restzeit (Sekunden) eines Timer-Checks von Bedingung i (oder -1).</summary>
        public double CheckRemaining(MissionContract c, int i) =>
            double.TryParse(c.Progress.GetValue($"c{i}_rem"),
                System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture,
                out double r) ? r : -1.0;

        /// <summary>Loest eine bereitstehende Mission ein: zahlt Reward, schaltet Folgemissionen frei.</summary>
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

            // Cooldown-Buchhaltung: jeder ANDERE Pool-Repeatable bekommt +1.
            foreach (var other in Catalog.All)
                if (!ReferenceEquals(other, c) && other.IsRepeatableInPool)
                    other.CompletionsSinceLastClaim++;

            if (c.Repeatable)
            {
                c.CompletionsSinceLastClaim = 0;
                c.Status = MissionStatus.Available;
            }
            else c.Status = MissionStatus.CompletedOnce;

            Log.Info($"Eingeloest: {c.Id} (+{c.ScienceReward} Wissenschaft)");
            RecomputeAvailability();
            return true;
        }

        private static void PayReward(float amount)
        {
            if (amount <= 0f) return;
            if (ResearchAndDevelopment.Instance != null)
                ResearchAndDevelopment.Instance.AddScience(amount, TransactionReasons.Cheating);
            else
                Debug.LogWarning("[CSC] Kein ResearchAndDevelopment (kein Science/Career?) — Reward verfaellt.");
        }

        // --- Queries fuer die UI ---

        public IEnumerable<MissionContract> ActiveContracts() =>
            Catalog.All.Where(c => c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim);

        public IEnumerable<MissionContract> RepeatablePool() =>
            Catalog.All.Where(c => c.IsRepeatableInPool);

        public int RemainingCooldown(MissionContract c) =>
            Mathf.Max(0, Tuning.RepeatableCooldown - c.CompletionsSinceLastClaim);

        /// <summary>Ist die Vorschau gesperrter Missionen aktiv? (Trigger-Contract abgeschlossen.)</summary>
        public bool LockedPreviewActive =>
            IsCompleted(Catalog.Get(Tuning.LockedPreviewTrigger));

        /// <summary>Titel der noch nicht erfuellten Voraussetzungen eines Contracts (fuer die rote Zeile).</summary>
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
