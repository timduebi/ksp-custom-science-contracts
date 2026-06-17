namespace CustomScienceContracts.Model
{
    /// <summary>Alle unterstuetzten Bedingungstypen. Einfache zuerst, drei knifflige zuletzt
    /// (FLYBY, MARKER_LANDING, RENDEZVOUS, jeweils mit State-Tracking ueber mehrere Frames).</summary>
    public enum ConditionType
    {
        // --- Einfach ---
        ORBIT,                  // ORBITING + orbit.PeA > atmosphereDepth
        ORBIT_HIGH,             // ORBITING + orbit.PeA > minAltitudeKm
        LANDED,                 // LANDED/SPLASHED auf Zielkoerper
        ATMO_ENTRY,             // Wechsel zu FLYING ueber Koerper mit Atmosphaere
        ALT_FRACTION_ATMO,      // Hoehe zwischen f1*atmoDepth und f2*atmoDepth, Situation FLYING
        ABOVE_ATMO_SUBORBITAL,  // Hoehe > atmoDepth, Situation SUB_ORBITAL
        EVA,                    // aktiver Kerbal im EVA-Zustand in Ziel-Situation
        CREW_DURATION,          // Situation + Crew >= N + MET-Spanne >= T Tage
        DOCK,                   // onPartCouple in Ziel-Situation
        ORE_SURFACE,            // LANDED + GetResourceAmount("Ore") > 0
        VESSEL_COUNT_ORBIT,     // >= N Vessels gleichzeitig ORBITING um Koerper
        FUEL_ORBIT,             // ORBITING + Treibstoff > Schwelle

        // --- Knifflig (State-Tracking) ---
        FLYBY,                  // SOI betreten, nie georbited, SOI verlassen
        MARKER_LANDING,         // LANDED + Distanz zu Waypoint <= R
        RENDEZVOUS,             // zwei Vessels < D km in Ziel-Situation

        // --- Hand-komponiert: enthaelt CHECK-Subnodes (atomare, einzeln bewertete Teilziele) ---
        COMPOSITE
    }
}
