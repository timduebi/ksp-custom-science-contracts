using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomScienceContracts.Core
{
    /// <summary>Pure policy for a redundant, evenly phased orbital relay network.</summary>
    public static class NetworkTopologyPolicy
    {
        public static bool Meets(IEnumerable<double> orbitalPhases, int primaries, int redundancy,
            double separationMin, double maxGap)
        {
            if (orbitalPhases == null) return false;
            var phases = orbitalPhases.Select(Normalize).OrderBy(value => value).ToList();
            int required = Math.Max(0, primaries) + Math.Max(0, redundancy);
            if (phases.Count < required || phases.Count < 2) return false;

            double smallest = 360.0;
            double largest = 0.0;
            for (int i = 0; i < phases.Count; i++)
            {
                double next = i + 1 < phases.Count ? phases[i + 1] : phases[0] + 360.0;
                double gap = next - phases[i];
                smallest = Math.Min(smallest, gap);
                largest = Math.Max(largest, gap);
            }

            return smallest + 1e-6 >= Math.Max(0.0, separationMin) &&
                   largest <= Math.Min(360.0, Math.Max(0.0, maxGap)) + 1e-6;
        }

        private static double Normalize(double phase)
        {
            phase %= 360.0;
            return phase < 0.0 ? phase + 360.0 : phase;
        }
    }
}
