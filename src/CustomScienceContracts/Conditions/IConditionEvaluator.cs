using CustomScienceContracts.Model;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Evaluates one condition type. Cross-frame state (flyby phase, crew-duration start,
    /// marker waypoint) lives in contract.Progress and is persisted. net48/Mono does not support
    /// default interface methods, so <see cref="ConditionEvaluatorBase"/> provides no-op defaults.</summary>
    public interface IConditionEvaluator
    {
        ConditionType Type { get; }

        /// <summary>true as soon as the condition counts as fulfilled.</summary>
        bool Evaluate(MissionContract contract, Condition cond, EvaluationContext ctx);

        /// <summary>Called when accepting, e.g. to create markers or clear timer starts.</summary>
        void OnAccepted(MissionContract contract, Condition cond);

        /// <summary>Called when claiming/abandoning, e.g. to remove marker waypoints.</summary>
        void OnCleared(MissionContract contract, Condition cond);
    }

    /// <summary>Base class with no-op defaults for OnAccepted/OnCleared.</summary>
    public abstract class ConditionEvaluatorBase : IConditionEvaluator
    {
        public abstract ConditionType Type { get; }
        public abstract bool Evaluate(MissionContract contract, Condition cond, EvaluationContext ctx);
        public virtual void OnAccepted(MissionContract contract, Condition cond) { }
        public virtual void OnCleared(MissionContract contract, Condition cond) { }
    }
}
