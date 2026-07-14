using System;

namespace CustomScienceContracts.Core
{
    /// <summary>Pure resource-delta policy. Consumption never erases delivery already observed.</summary>
    public static class ResourceDeliveryPolicy
    {
        public static double Accumulate(double baseline, double current, double previous) =>
            Math.Max(0.0, Math.Max(previous, current - baseline));

        public static bool Reached(double delivered, double required) =>
            delivered + 1e-6 >= Math.Max(0.0, required);
    }
}
