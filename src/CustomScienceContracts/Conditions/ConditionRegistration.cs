namespace CustomScienceContracts.Conditions
{
    /// <summary>Central place where the concrete evaluators are registered.</summary>
    public static class ConditionRegistration
    {
        public static void RegisterAll(ConditionEvaluatorRegistry reg)
        {
            // --- Simple evaluators ---
            reg.Register(new OrbitEvaluator());
            reg.Register(new OrbitHighEvaluator());
            reg.Register(new LandedEvaluator());
            reg.Register(new AltFractionAtmoEvaluator());
            reg.Register(new AboveAtmoSuborbitalEvaluator());
            reg.Register(new EvaEvaluator());
            reg.Register(new CrewDurationEvaluator());
            reg.Register(new DockEvaluator());
            reg.Register(new OreSurfaceEvaluator());
            reg.Register(new VesselCountOrbitEvaluator());
            reg.Register(new FuelOrbitEvaluator());
            reg.Register(new AtmoEntryEvaluator());

            // --- Stateful evaluators ---
            reg.Register(new FlybyEvaluator());
            reg.Register(new MarkerLandingEvaluator());
            reg.Register(new RendezvousEvaluator());

            // --- COMPOSITE: evaluated via CheckEvaluation; this only provides lifecycle hooks ---
            reg.Register(new CompositeEvaluator());
        }
    }
}
