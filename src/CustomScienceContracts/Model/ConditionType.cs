namespace CustomScienceContracts.Model
{
    /// <summary>All supported condition types. The last stateful types keep state across ticks.</summary>
    public enum ConditionType
    {
        // --- Einfach ---
        ORBIT,                  // ORBITING + orbit.PeA > atmosphereDepth
        ORBIT_HIGH,             // ORBITING + orbit.PeA > minAltitudeKm
        LANDED,                 // LANDED/SPLASHED auf Zielkoerper
        ATMO_ENTRY,             // change to FLYING over a body with atmosphere
        ALT_FRACTION_ATMO,      // altitude between f1*atmoDepth and f2*atmoDepth, situation FLYING
        ABOVE_ATMO_SUBORBITAL,  // altitude > atmoDepth, situation SUB_ORBITAL
        EVA,                    // aktiver Kerbal im EVA-Zustand in Ziel-Situation
        CREW_DURATION,          // situation + crew >= N + MET span >= T days
        DOCK,                   // onPartCouple in Ziel-Situation
        ORE_SURFACE,            // LANDED + GetResourceAmount("Ore") > 0
        VESSEL_COUNT_ORBIT,     // >= N vessels ORBITING around body at once
        FUEL_ORBIT,             // ORBITING + Treibstoff > Schwelle

        // --- Knifflig (State-Tracking) ---
        FLYBY,                  // SOI betreten, nie georbited, SOI verlassen
        MARKER_LANDING,         // LANDED + Distanz zu Waypoint <= R
        RENDEZVOUS,             // zwei Vessels < D km in Ziel-Situation

        // --- Hand-composed: contains CHECK subnodes (atomic, individually evaluated goals) ---
        COMPOSITE
    }
}
