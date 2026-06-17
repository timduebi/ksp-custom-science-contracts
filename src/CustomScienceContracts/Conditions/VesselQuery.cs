using System.Collections.Generic;
using CustomScienceContracts.Core;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Geteilte, API-getriebene Abfragen ueber Vessels und Koerper.
    /// Keine hardcodeten Body-Werte: atmosphereDepth/Tageslaenge kommen aus der API.</summary>
    public static class VesselQuery
    {
        /// <summary>Situations-Filter aus dem Condition-String. Leer = beliebig.</summary>
        public static bool MatchesSituation(Vessel v, string sit)
        {
            if (string.IsNullOrEmpty(sit)) return true;
            switch (sit.Trim().ToUpperInvariant())
            {
                case "ORBIT":
                case "ORBITING":   return v.situation == Vessel.Situations.ORBITING;
                case "LANDED":
                case "SURFACE":    return v.situation == Vessel.Situations.LANDED ||
                                          v.situation == Vessel.Situations.SPLASHED;
                case "SPLASHED":   return v.situation == Vessel.Situations.SPLASHED;
                case "SUBORBITAL":
                case "SUB_ORBITAL":return v.situation == Vessel.Situations.SUB_ORBITAL;
                case "FLYING":     return v.situation == Vessel.Situations.FLYING;
                case "PRELAUNCH":  return v.situation == Vessel.Situations.PRELAUNCH;
                default:           return true;
            }
        }

        /// <summary>Das aktuell gesteuerte Fahrzeug (null in der Tracking Station / ohne Flug).</summary>
        public static Vessel Active => FlightGlobals.ActiveVessel;

        public static bool OnBody(Vessel v, CelestialBody body) =>
            body != null && v != null && v.mainBody == body;

        /// <summary>Grosskreis-Oberflaechendistanz (m) zwischen zwei Lat/Lon auf einem Koerper.</summary>
        public static double SurfaceDistance(CelestialBody body, double lat1, double lon1, double lat2, double lon2)
        {
            double r = body.Radius;
            double dLat = (lat2 - lat1) * Mathf.Deg2Rad;
            double dLon = (lon2 - lon1) * Mathf.Deg2Rad;
            double s1 = System.Math.Sin(dLat / 2.0);
            double s2 = System.Math.Sin(dLon / 2.0);
            double a = s1 * s1 +
                       System.Math.Cos(lat1 * Mathf.Deg2Rad) * System.Math.Cos(lat2 * Mathf.Deg2Rad) * s2 * s2;
            return r * 2.0 * System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1.0 - a));
        }

        public static int CrewCount(Vessel v) => v.GetCrewCount();

        /// <summary>Stabiler Orbit oberhalb der Atmosphaere: ORBITING + PeA &gt; atmosphereDepth.</summary>
        public static bool InOrbitAboveAtmo(Vessel v, CelestialBody body)
        {
            if (!OnBody(v, body) || v.situation != Vessel.Situations.ORBITING) return false;
            return v.orbit != null && v.orbit.PeA > body.atmosphereDepth;
        }

        /// <summary>Summe einer Ressource auf dem Vessel (z.B. "Ore").</summary>
        public static double Resource(Vessel v, string resourceName)
        {
            var def = PartResourceLibrary.Instance?.GetDefinition(resourceName);
            if (def == null) return 0.0;
            v.GetConnectedResourceTotals(def.id, out double amount, out _);
            return amount;
        }

        /// <summary>LiquidFuel + Oxidizer als "Treibstoff" fuer FUEL_ORBIT.</summary>
        public static double Fuel(Vessel v) => Resource(v, "LiquidFuel") + Resource(v, "Oxidizer");

        /// <summary>Tageslaenge in Sekunden aus der API (respektiert Kronometer/Heimatkoerper).</summary>
        public static double SecondsPerDay()
        {
            int d = KSPUtil.dateTimeFormatter != null ? KSPUtil.dateTimeFormatter.Day : 21600;
            return d > 0 ? d : 21600;
        }

        /// <summary>Vessels, die als "echte" Fahrzeuge zaehlen (kein Debris/Flag/Asteroid).</summary>
        public static IEnumerable<Vessel> RealVessels(IReadOnlyList<Vessel> vessels)
        {
            foreach (var v in vessels)
            {
                if (v == null) continue;
                switch (v.vesselType)
                {
                    case VesselType.Debris:
                    case VesselType.Flag:
                    case VesselType.SpaceObject:
                    case VesselType.Unknown:
                    case VesselType.DeployedScienceController:
                    case VesselType.DeployedSciencePart:
                        continue;
                    default:
                        yield return v;
                        break;
                }
            }
        }

        public static CelestialBody Body(string name) => BodyResolver.Resolve(name);
    }
}
