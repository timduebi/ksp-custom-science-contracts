namespace CustomScienceContracts.Conditions
{
    /// <summary>Zentrale Stelle, an der die konkreten Evaluatoren registriert werden.
    /// Im Gerüst leer (alle Typen laufen ueber StubEvaluator).
    /// Schritt 4: einfache Bedingungen. Schritt 5: FLYBY / MARKER_LANDING / RENDEZVOUS.</summary>
    public static class ConditionRegistration
    {
        public static void RegisterAll(ConditionEvaluatorRegistry reg)
        {
            // --- Schritt 4 (einfach) ---
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

            // --- Schritt 5 (knifflig) ---
            reg.Register(new FlybyEvaluator());
            reg.Register(new MarkerLandingEvaluator());
            reg.Register(new RendezvousEvaluator());

            // --- COMPOSITE: Auswertung via CheckEvaluation; hier nur der Marker-Lifecycle-Haken ---
            reg.Register(new CompositeEvaluator());
        }
    }
}
