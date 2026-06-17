using CustomScienceContracts.Model;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Wertet einen Bedingungstyp aus. Cross-Frame-State (FLYBY-Phase, CREW_DURATION-Start,
    /// Marker-Waypoint) wird in contract.Progress (ConfigNode) gehalten und mitpersistiert.
    /// Hinweis: net48/Mono unterstuetzt KEINE Default-Interface-Methoden -> Basisklasse
    /// <see cref="ConditionEvaluatorBase"/> liefert die No-op-Defaults.</summary>
    public interface IConditionEvaluator
    {
        ConditionType Type { get; }

        /// <summary>true, sobald die Bedingung als erfuellt gilt.</summary>
        bool Evaluate(MissionContract contract, Condition cond, EvaluationContext ctx);

        /// <summary>Beim Annehmen (z.B. Marker-Waypoint setzen, CREW_DURATION-Startzeit nullen).</summary>
        void OnAccepted(MissionContract contract, Condition cond);

        /// <summary>Beim Abschluss/Verwerfen (z.B. Marker-Waypoint entfernen).</summary>
        void OnCleared(MissionContract contract, Condition cond);
    }

    /// <summary>Basisklasse mit No-op-Defaults fuer OnAccepted/OnCleared.</summary>
    public abstract class ConditionEvaluatorBase : IConditionEvaluator
    {
        public abstract ConditionType Type { get; }
        public abstract bool Evaluate(MissionContract contract, Condition cond, EvaluationContext ctx);
        public virtual void OnAccepted(MissionContract contract, Condition cond) { }
        public virtual void OnCleared(MissionContract contract, Condition cond) { }
    }
}
