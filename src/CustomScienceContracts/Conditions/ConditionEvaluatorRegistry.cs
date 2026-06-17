using System;
using System.Collections.Generic;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Map ConditionType -> Evaluator. Im Gerüst sind alle Typen mit einem Stub belegt,
    /// der false liefert. Schritt 4 (einfache) und Schritt 5 (FLYBY/MARKER_LANDING/RENDEZVOUS)
    /// ueberschreiben die Eintraege via Register().</summary>
    public class ConditionEvaluatorRegistry
    {
        private readonly Dictionary<ConditionType, IConditionEvaluator> _map =
            new Dictionary<ConditionType, IConditionEvaluator>();

        public ConditionEvaluatorRegistry()
        {
            foreach (ConditionType t in Enum.GetValues(typeof(ConditionType)))
                _map[t] = new StubEvaluator(t);
        }

        public void Register(IConditionEvaluator ev) => _map[ev.Type] = ev;

        public IConditionEvaluator Get(ConditionType t) =>
            _map.TryGetValue(t, out var e) ? e : _map[ConditionType.ORBIT];

        /// <summary>Alle Bedingungen eines Contracts erfuellt? (UND-Verknuepfung)</summary>
        public bool AllSatisfied(MissionContract c, EvaluationContext ctx)
        {
            foreach (var cond in c.Bedingungen)
                if (!Get(cond.Type).Evaluate(c, cond, ctx))
                    return false;
            return c.Bedingungen.Count > 0;
        }

        public void NotifyAccepted(MissionContract c)
        {
            foreach (var cond in c.Bedingungen) Get(cond.Type).OnAccepted(c, cond);
        }

        public void NotifyCleared(MissionContract c)
        {
            foreach (var cond in c.Bedingungen) Get(cond.Type).OnCleared(c, cond);
        }
    }

    /// <summary>Platzhalter bis die echte Logik implementiert ist (Schritt 4/5).</summary>
    internal sealed class StubEvaluator : ConditionEvaluatorBase
    {
        private bool _warned;
        public StubEvaluator(ConditionType t) { Type = t; }
        public override ConditionType Type { get; }
        public override bool Evaluate(MissionContract contract, Condition cond, EvaluationContext ctx)
        {
            if (!_warned)
            {
                Debug.Log($"[CSC] ConditionType {Type} noch nicht implementiert (Stub) — Contract '{contract.Id}'.");
                _warned = true;
            }
            return false;
        }
    }
}
