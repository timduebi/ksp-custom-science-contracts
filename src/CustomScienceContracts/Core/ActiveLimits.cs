using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Aktiv-Limits pro Sparte (gleichzeitig angenommene Missionen): 3 / 10 / 5.
    /// Wiederholbare zaehlen gegen das Limit ihrer Heimatsparte.</summary>
    public static class ActiveLimits
    {
        public static int LimitFor(Sparte s)
        {
            switch (s)
            {
                case Sparte.Bemannt: return Tuning.ActiveBemannt;
                case Sparte.UnbemannteErkundung: return Tuning.ActiveErkundung;
                case Sparte.NetzwerkLogistik: return Tuning.ActiveNetzwerk;
                default: return Tuning.ActiveErkundung; // Wiederholbar -> Heimatsparte zaehlt
            }
        }

        public static int ActiveCount(IEnumerable<MissionContract> all, Sparte heimat) =>
            all.Count(c => (c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim)
                           && c.HeimatSparte == heimat);

        /// <summary>Darf ein Available-Contract angenommen werden, ohne das Limit zu sprengen?</summary>
        public static bool CanAccept(IReadOnlyList<MissionContract> all, MissionContract candidate)
        {
            if (candidate.Status != MissionStatus.Available) return false;
            return ActiveCount(all, candidate.HeimatSparte) < LimitFor(candidate.HeimatSparte);
        }
    }
}
