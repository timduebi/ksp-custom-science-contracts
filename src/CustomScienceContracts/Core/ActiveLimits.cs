using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Active limits per branch: 3 / 10 / 5. Repeatables count against the limit of their home branch.</summary>
    public static class ActiveLimits
    {
        public static int LimitFor(Sparte s)
        {
            switch (s)
            {
                case Sparte.Bemannt: return Tuning.ActiveBemannt;
                case Sparte.UnbemannteErkundung: return Tuning.ActiveErkundung;
                case Sparte.NetzwerkLogistik: return Tuning.ActiveNetzwerk;
                default: return Tuning.ActiveErkundung; // Wiederholbar -> home branch counts
            }
        }

        public static int ActiveCount(IEnumerable<MissionContract> all, Sparte heimat) =>
            all.Count(c => (c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim)
                           && c.HeimatSparte == heimat);
    }
}
