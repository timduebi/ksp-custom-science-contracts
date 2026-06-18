using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace CustomScienceContracts.Model
{
    /// <summary>A single condition parsed from a CONDITION ConfigNode. All fields are optional;
    /// only the ones relevant to the condition type are used. Body values such as atmosphereDepth
    /// are never stored here and are resolved from the API at runtime.</summary>
    public class Condition
    {
        public ConditionType Type;
        public string Body = "";            // internal CelestialBody.name, e.g. "Earth", "Moon"
        public int MinCrew = 0;
        public double DurationDays = 0.0;   // CREW_DURATION
        public double MinFraction = 0.0;    // ALT_FRACTION_ATMO
        public double MaxFraction = 1.0;    // ALT_FRACTION_ATMO
        public double MinAltitudeKm = 0.0;  // ORBIT_HIGH / high-orbit networks
        public double RadiusKm = 0.0;       // MARKER_LANDING (15 default, 5 for resupply/rotation)
        public int VesselCount = 1;         // VESSEL_COUNT_ORBIT
        public double FuelThreshold = 0.0;  // FUEL_ORBIT (LiquidFuel+Oxidizer units)
        public double RendezvousKm = 0.0;   // RENDEZVOUS
        public double FlybyAltitudeKm = 0.0;// FLYBY: max closest approach (PeA), 0 = SOI passage only
        public string Situation = "";       // optionaler expliziter Situations-Filter
        public string StationKey = "";      // DOCK/RENDEZVOUS: target is the recorded station with this key

        /// <summary>Hand-composed atomic goals for COMPOSITE conditions. When this list is not
        /// empty, the condition is evaluated through these checks rather than the legacy type.</summary>
        public List<Check> Checks = new List<Check>();

        public static Condition Load(ConfigNode node)
        {
            var c = new Condition();
            string typeStr = node.GetValue("type");
            if (!Enum.TryParse(typeStr, true, out c.Type))
            {
                Debug.LogWarning($"[CSC] Unknown ConditionType '{typeStr}' skipped.");
                return null;
            }

            c.Body          = node.GetValue("body") ?? "";
            c.Situation     = node.GetValue("situation") ?? "";
            c.StationKey    = node.GetValue("stationKey") ?? "";
            c.MinCrew       = GetInt(node, "minCrew", 0);
            c.VesselCount   = GetInt(node, "vesselCount", 1);
            c.DurationDays  = GetDouble(node, "durationDays", 0.0);
            c.MinFraction   = GetDouble(node, "minFraction", 0.0);
            c.MaxFraction   = GetDouble(node, "maxFraction", 1.0);
            c.MinAltitudeKm = GetDouble(node, "minAltitudeKm", 0.0);
            c.RadiusKm      = GetDouble(node, "radiusKm", 0.0);
            c.FuelThreshold = GetDouble(node, "fuelThreshold", 0.0);
            c.RendezvousKm  = GetDouble(node, "rendezvousKm", 0.0);
            c.FlybyAltitudeKm = GetDouble(node, "flybyAltitudeKm", 0.0);

            foreach (ConfigNode chk in node.GetNodes("CHECK"))
            {
                Check ck = Check.Load(chk);
                if (ck != null) c.Checks.Add(ck);
            }
            return c;
        }

        private static int GetInt(ConfigNode n, string key, int def)
        {
            string v = n.GetValue(key);
            return int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : def;
        }

        private static double GetDouble(ConfigNode n, string key, double def)
        {
            string v = n.GetValue(key);
            return double.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : def;
        }
    }
}
