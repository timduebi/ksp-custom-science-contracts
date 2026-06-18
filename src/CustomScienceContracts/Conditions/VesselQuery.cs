using System.Collections.Generic;
using CustomScienceContracts.Core;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Shared API-driven queries for vessels and bodies. No hardcoded body values:
    /// atmosphereDepth and day length come from the API.</summary>
    public static class VesselQuery
    {
        /// <summary>Situation filter from the condition string. Empty means any.</summary>
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

        /// <summary>The currently controlled vessel, or null in Tracking Station / without flight.</summary>
        public static Vessel Active => FlightGlobals.ActiveVessel;

        public static bool OnBody(Vessel v, CelestialBody body) =>
            body != null && v != null && v.mainBody == body;

        /// <summary>Great-circle surface distance in meters between two lat/lon positions on a body.</summary>
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

        /// <summary>Effective crew for crew checks: onboard crew plus nearby EVA Kerbals in the same
        /// SOI. This keeps a short EVA near a station from breaking a crew-duration timer.</summary>
        public static int EffectiveCrew(Vessel v)
        {
            if (v == null) return 0;
            int crew = v.GetCrewCount();
            var list = FlightGlobals.Vessels;
            if (list == null) return crew;
            const double nearM = 2500.0;
            Vector3d pos = v.GetWorldPos3D();
            foreach (var e in list)
            {
                if (e == null || !e.isEVA || ReferenceEquals(e, v)) continue;
                if (e.mainBody != v.mainBody) continue;
                if (Vector3d.Distance(e.GetWorldPos3D(), pos) <= nearM) crew++;
            }
            return crew;
        }

        /// <summary>Stable orbit above atmosphere: ORBITING + PeA &gt; atmosphereDepth.</summary>
        public static bool InOrbitAboveAtmo(Vessel v, CelestialBody body)
        {
            if (!OnBody(v, body) || v.situation != Vessel.Situations.ORBITING) return false;
            return v.orbit != null && v.orbit.PeA > body.atmosphereDepth;
        }

        /// <summary>Total amount of a resource on the vessel, e.g. "Ore".</summary>
        public static double Resource(Vessel v, string resourceName)
        {
            var def = PartResourceLibrary.Instance?.GetDefinition(resourceName);
            if (def == null) return 0.0;
            v.GetConnectedResourceTotals(def.id, out double amount, out _);
            return amount;
        }

        /// <summary>LiquidFuel + Oxidizer as fuel for FUEL_ORBIT.</summary>
        public static double Fuel(Vessel v) => Resource(v, "LiquidFuel") + Resource(v, "Oxidizer");

        /// <summary>Day length in seconds from the API.</summary>
        public static double SecondsPerDay()
        {
            int d = KSPUtil.dateTimeFormatter != null ? KSPUtil.dateTimeFormatter.Day : 21600;
            return d > 0 ? d : 21600;
        }

        /// <summary>Vessels that count as real craft, excluding debris, flags and asteroids.</summary>
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
