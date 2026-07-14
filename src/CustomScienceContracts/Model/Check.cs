using System;
using System.Globalization;
using UnityEngine;

namespace CustomScienceContracts.Model
{
    /// <summary>Atomic, individually evaluated goal inside a COMPOSITE condition. Each mission can
    /// provide its own cfg label, keeping the checklist readable and mission-specific.</summary>
    public enum CheckKind
    {
        CREW_MIN,            // crew >= min
        CREW_NONE,           // uncrewed, no Kerbals aboard
        CREW_EXACT,          // crew == min
        CREW_CAPACITY_MIN,   // vessel seats >= min
        ON_BODY,             // at target body (mainBody == body)
        SITUATION,           // Situation == situation (ORBITING/LANDED/...)
        PERIAPSIS_MIN,       // orbit.PeA > km
        ORBIT_ABOVE,         // ORBITING around body + orbit.PeA > km
        APOAPSIS_MAX,        // ORBITING around body + orbit.ApA < km
        INCLINATION_MIN,     // orbit.inclination >= inclinationMin
        ABOVE_ATMOSPHERE,    // orbit.PeA > atmosphereDepth
        SUBORBITAL_ABOVE_ATMO, // altitude > atmosphereDepth
        SUBORBITAL,          // SUB_ORBITAL at body + altitude > atmosphereDepth
        LANDED,              // LANDED/SPLASHED at body
        ATMO_FRACTION,       // altitude in [fracMin,fracMax] * atmosphereDepth
        ORE_PRESENT,         // Ore an Bord > 0
        ORE_SURFACE,         // LANDED/SPLASHED at body + Ore aboard > 0
        FUEL_MIN,            // LiquidFuel+Oxidizer > amount
        RESOURCE_MIN,        // resource amount > amount
        WHEEL_MOTION,        // landed rover with wheel module and surface speed >= speed
        EVA,                 // EVA, optionally at body/situation
        DOCK_STATION,        // dock with recorded station (stationKey)
        DOCK_ANY,            // any docking event
        VESSEL_COUNT,        // >= count real vessels ORBITING around body, optional PeA > km
        VESSEL_COUNT_INCLINATION, // >= count reale Vessels ORBITING um body mit inclination >= inclinationMin (optional PeA > km)
        RELAY_VESSEL_COUNT,  // like VESSEL_COUNT, but each vessel needs a relay transmitter
        RELAY_VESSEL_COUNT_INCLINATION, // like VESSEL_COUNT_INCLINATION with relay transmitters
        RELAY_NETWORK_TOPOLOGY, // operational, phased relay network with spare capacity
        RESOURCE_DELIVERY,   // positive resource delivery to a recorded station/base
        MASS_MIN,            // vessel mass in tonnes
        MODULE_COUNT,        // number of matching PartModules
        POWER_CAPACITY_MIN,  // ElectricCharge capacity
        DOCKING_PORT_COUNT,  // number of ModuleDockingNode modules
        FLYBY,               // a real vessel passes through the body's SOI without orbiting
        MARKER_LANDING,      // active vessel landed/splashed at body within km of target point
        RETURN_FROM_BODY,    // crewed landing/visit at body, then crewed landing/splashdown on returnBody
        HOLD,                // hold all other checks continuously for seconds
        DURATION             // hold all other checks continuously for days
    }

    public class Check
    {
        public CheckKind Kind;
        public string Body = "";
        public string Situation = "";
        public string StationKey = "";
        public string Resource = "";
        public string Module = "";
        public string LegacyKind = "";
        public string Label = "";
        public string ReturnBody = "";
        public string ReturnMode = ""; // empty/surface = land first, flyby = visit SOI first
        public int Min = 0;          // crew/capacity threshold
        public int Count = 1;        // VESSEL_COUNT
        public int Redundancy = 0;   // additional operational relay satellites beyond Count
        public double Km = 0.0;      // PERIAPSIS_MIN / VESSEL_COUNT / FLYBY closest approach / MARKER radius
        public double InclinationMin = 0.0; // INCLINATION_MIN / VESSEL_COUNT_INCLINATION
        public double SeparationMin = 0.0; // RELAY_NETWORK_TOPOLOGY minimum phase separation
        public double MaxGap = 360.0; // RELAY_NETWORK_TOPOLOGY largest allowed phase gap
        public double Seconds = 0.0; // HOLD
        public double Days = 0.0;    // DURATION
        public double Amount = 0.0;  // fuel/resource/delivery/mass/power threshold
        public double SpeedMin = 0.0; // WHEEL_MOTION
        public double FracMin = 0.0; // ATMO_FRACTION
        public double FracMax = 1.0; // ATMO_FRACTION
        public double LatAbsMin = 0.0;  // MARKER_LANDING: lower absolute latitude band
        public double LatAbsMax = 15.0; // MARKER_LANDING: upper absolute latitude band

        /// <summary>Timer check (HOLD/DURATION), evaluated through the shared timer.</summary>
        public bool IsTimer => Kind == CheckKind.HOLD || Kind == CheckKind.DURATION;
        /// <summary>Event-based docking check, evaluated against the event buffer.</summary>
        public bool IsEvent => Kind == CheckKind.DOCK_STATION || Kind == CheckKind.DOCK_ANY;
        /// <summary>Fleet check that counts multiple vessels instead of one subject vessel.</summary>
        public bool IsFleet => Kind == CheckKind.VESSEL_COUNT || Kind == CheckKind.VESSEL_COUNT_INCLINATION ||
                               Kind == CheckKind.RELAY_VESSEL_COUNT || Kind == CheckKind.RELAY_VESSEL_COUNT_INCLINATION ||
                               Kind == CheckKind.RELAY_NETWORK_TOPOLOGY;
        public bool IsDelivery => Kind == CheckKind.RESOURCE_DELIVERY;
        /// <summary>Flyby check with its own multi-tick fleet state.</summary>
        public bool IsFlyby => Kind == CheckKind.FLYBY;
        /// <summary>Precision landing that creates/checks a persistent target point.</summary>
        public bool IsMarker => Kind == CheckKind.MARKER_LANDING;
        /// <summary>Sequential crewed return check with its own persisted state.</summary>
        public bool IsReturn => Kind == CheckKind.RETURN_FROM_BODY;
        /// <summary>Checks with their own state/subject that do not run against one subject vessel.</summary>
        public bool IsSpecial => IsTimer || IsEvent || IsFleet || IsFlyby || IsMarker || IsReturn || IsDelivery;

        public static Check Load(ConfigNode node)
        {
            var c = new Check();
            if (!Enum.TryParse(node.GetValue("kind"), true, out c.Kind))
            {
                Debug.LogWarning($"[CSC] Unknown CheckKind '{node.GetValue("kind")}' skipped.");
                return null;
            }
            c.Body       = node.GetValue("body") ?? "";
            c.Situation  = node.GetValue("situation") ?? "";
            c.StationKey = node.GetValue("stationKey") ?? "";
            c.Resource   = node.GetValue("resource") ?? "";
            c.Module     = node.GetValue("module") ?? "";
            c.LegacyKind = node.GetValue("legacyKind") ?? "";
            c.Label      = node.GetValue("label") ?? "";
            c.ReturnBody = node.GetValue("returnBody") ?? "";
            c.ReturnMode = node.GetValue("returnMode") ?? "";
            c.Min     = GetI(node, "min", 0);
            c.Count   = GetI(node, "count", 1);
            c.Redundancy = GetI(node, "redundancy", 0);
            c.Km      = GetD(node, "km", 0.0);
            c.InclinationMin = GetD(node, "inclinationMin", 0.0);
            c.SeparationMin = GetD(node, "separationMin", 0.0);
            c.MaxGap = GetD(node, "maxGap", 360.0);
            c.Seconds = GetD(node, "seconds", 0.0);
            c.Days    = GetD(node, "days", 0.0);
            c.Amount  = GetD(node, "amount", 0.0);
            c.SpeedMin = GetD(node, "speed", 0.0);
            c.FracMin = GetD(node, "fracMin", 0.0);
            c.FracMax = GetD(node, "fracMax", 1.0);
            c.LatAbsMin = GetD(node, "latMin", 0.0);
            c.LatAbsMax = GetD(node, "latMax", 15.0);
            return c;
        }

        private static int GetI(ConfigNode n, string k, int def) =>
            int.TryParse(n.GetValue(k), NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : def;
        private static double GetD(ConfigNode n, string k, double def) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : def;
    }
}
