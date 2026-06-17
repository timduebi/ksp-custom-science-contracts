using System;
using System.Globalization;
using UnityEngine;

namespace CustomScienceContracts.Model
{
    /// <summary>Atomares, einzeln bewertetes Teilziel innerhalb einer COMPOSITE-CONDITION.
    /// Wird pro Mission von Hand in der cfg formuliert (eigenes <c>label</c>), damit die
    /// Checkliste pro Mission individuell und gut lesbar ist (keine Massenware).</summary>
    public enum CheckKind
    {
        CREW_MIN,            // Besatzung >= min
        CREW_NONE,           // unbemannt (kein Kerbal an Bord)
        CREW_EXACT,          // Besatzung == min
        ON_BODY,             // am Zielkoerper (mainBody == body)
        SITUATION,           // Situation == situation (ORBITING/LANDED/...)
        PERIAPSIS_MIN,       // orbit.PeA > km
        ORBIT_ABOVE,         // ORBITING um body + orbit.PeA > km (praeziser Ein-Zeilen-Orbitcheck)
        INCLINATION_MIN,     // orbit.inclination >= inclinationMin
        ABOVE_ATMOSPHERE,    // orbit.PeA > atmosphereDepth (Orbit klar ueber der Atmosphaere)
        SUBORBITAL_ABOVE_ATMO, // altitude > atmosphereDepth (suborbitaler Scheitel ueber der Atmosphaere)
        SUBORBITAL,          // SUB_ORBITAL am body + altitude > atmosphereDepth (suborbitaler Raumflug)
        LANDED,              // LANDED/SPLASHED am body
        ATMO_FRACTION,       // altitude in [fracMin,fracMax] * atmosphereDepth
        ORE_PRESENT,         // Ore an Bord > 0
        ORE_SURFACE,         // LANDED/SPLASHED am body + Ore an Bord > 0 (Foerderung)
        FUEL_MIN,            // LiquidFuel+Oxidizer > amount
        RESOURCE_MIN,        // resource-Menge > amount
        EVA,                 // EVA (optional am body + Situation)
        DOCK_STATION,        // Andocken an die gemerkte Station (stationKey)
        DOCK_ANY,            // beliebiges Andocken
        VESSEL_COUNT,        // >= count reale Vessels ORBITING um body (optional PeA > km)
        VESSEL_COUNT_INCLINATION, // >= count reale Vessels ORBITING um body mit inclination >= inclinationMin (optional PeA > km)
        FLYBY,               // ein reales Vessel durchfliegt die SOI von body, orbitet nie (km = max. Annaeherung)
        MARKER_LANDING,      // aktives Vessel LANDED/SPLASHED am body, Distanz zum Zielpunkt <= km
        HOLD,                // alle uebrigen Checks zusammenhaengend seconds Sekunden halten
        DURATION             // alle uebrigen Checks zusammenhaengend days Tage halten
    }

    public class Check
    {
        public CheckKind Kind;
        public string Body = "";
        public string Situation = "";
        public string StationKey = "";
        public string Resource = "";
        public string Label = "";
        public int Min = 0;          // Crew-Schwelle
        public int Count = 1;        // VESSEL_COUNT
        public double Km = 0.0;      // PERIAPSIS_MIN / VESSEL_COUNT / FLYBY (max. Annaeherung) / MARKER_LANDING (Radius)
        public double InclinationMin = 0.0; // INCLINATION_MIN / VESSEL_COUNT_INCLINATION
        public double Seconds = 0.0; // HOLD
        public double Days = 0.0;    // DURATION
        public double Amount = 0.0;  // FUEL_MIN / RESOURCE_MIN
        public double FracMin = 0.0; // ATMO_FRACTION
        public double FracMax = 1.0; // ATMO_FRACTION

        /// <summary>Zeit-Check (HOLD/DURATION) — wird ueber den gemeinsamen Timer ausgewertet.</summary>
        public bool IsTimer => Kind == CheckKind.HOLD || Kind == CheckKind.DURATION;
        /// <summary>Ereignisbasiert (Andocken) — wird gegen den Event-Puffer ausgewertet.</summary>
        public bool IsEvent => Kind == CheckKind.DOCK_STATION || Kind == CheckKind.DOCK_ANY;
        /// <summary>Flotten-Check (zaehlt mehrere Vessels) statt eines einzelnen Subjekts.</summary>
        public bool IsFleet => Kind == CheckKind.VESSEL_COUNT || Kind == CheckKind.VESSEL_COUNT_INCLINATION;
        /// <summary>Vorbeiflug — eigener, ueber mehrere Ticks laufender Flotten-State (rastet ein).</summary>
        public bool IsFlyby => Kind == CheckKind.FLYBY;
        /// <summary>Praezisionslandung — setzt/prueft einen persistenten Zielpunkt (Waypoint).</summary>
        public bool IsMarker => Kind == CheckKind.MARKER_LANDING;
        /// <summary>Checks mit Eigen-State/Eigen-Subjekt, die NICHT gegen ein einzelnes Subjekt laufen.</summary>
        public bool IsSpecial => IsTimer || IsEvent || IsFleet || IsFlyby || IsMarker;

        public static Check Load(ConfigNode node)
        {
            var c = new Check();
            if (!Enum.TryParse(node.GetValue("kind"), true, out c.Kind))
            {
                Debug.LogWarning($"[CSC] Unbekannte CheckKind '{node.GetValue("kind")}' uebersprungen.");
                return null;
            }
            c.Body       = node.GetValue("body") ?? "";
            c.Situation  = node.GetValue("situation") ?? "";
            c.StationKey = node.GetValue("stationKey") ?? "";
            c.Resource   = node.GetValue("resource") ?? "";
            c.Label      = node.GetValue("label") ?? "";
            c.Min     = GetI(node, "min", 0);
            c.Count   = GetI(node, "count", 1);
            c.Km      = GetD(node, "km", 0.0);
            c.InclinationMin = GetD(node, "inclinationMin", 0.0);
            c.Seconds = GetD(node, "seconds", 0.0);
            c.Days    = GetD(node, "days", 0.0);
            c.Amount  = GetD(node, "amount", 0.0);
            c.FracMin = GetD(node, "fracMin", 0.0);
            c.FracMax = GetD(node, "fracMax", 1.0);
            return c;
        }

        private static int GetI(ConfigNode n, string k, int def) =>
            int.TryParse(n.GetValue(k), NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : def;
        private static double GetD(ConfigNode n, string k, double def) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : def;
    }
}
