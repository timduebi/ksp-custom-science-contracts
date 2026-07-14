using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Data;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Computes which Available contracts are visible in the selection window for home
    /// branches. Repeatables that have been completed once live in Wiederholbar and are no longer listed here.</summary>
    public static class VisibilityRules
    {
        public sealed class QueueInfo
        {
            public int Position;
            public int Waiting;
            public int FrontierLimit;
        }
        /// <summary>Contract is listable in its home branch: Available and not moved into the Wiederholbar pool.</summary>
        public static bool IsHomeAvailable(MissionContract c) =>
            c.Status == MissionStatus.Available && !c.IsRepeatableInPool;

        /// <summary>Returns the ids currently visible in home branches. Order follows the stable catalog order.</summary>
        public static HashSet<string> ComputeVisible(ContractCatalog cat)
        {
            var visible = new HashSet<string>();

            // Test switch: show every unlockable contract without limits.
            if (Tuning.UnlockAll)
            {
                foreach (var c in cat.All)
                    if (IsHomeAvailable(c)) visible.Add(c.Id);
                return visible;
            }

            // --- Crewed: global cap 3, then 5 once >= 50 % are CompletedOnce ---
            var bemannt = cat.InSparte(Sparte.Bemannt).ToList();
            int bemTotal = bemannt.Count;
            int bemDone = bemannt.Count(ContractManager.IsCompleted);
            int bemCap = (bemTotal > 0 && bemDone >= Tuning.BemanntBoostFraction * bemTotal)
                ? Tuning.VisibleBemanntBoosted
                : Tuning.VisibleBemanntBase;
            foreach (var c in bemannt.Where(IsHomeAvailable).Take(bemCap))
                visible.Add(c.Id);

            // --- Robotic exploration: 4 per subcategory; RevealAllAfter lifts the cap ---
            foreach (string sub in cat.Subcategories(Sparte.UnbemannteErkundung))
            {
                var inSub = cat.InSubcategory(Sparte.UnbemannteErkundung, sub).ToList();
                bool revealed = inSub.Any(c =>
                    !string.IsNullOrEmpty(c.RevealAllAfter) &&
                    ContractManager.IsCompleted(cat.Get(c.RevealAllAfter)));

                var avail = inSub.Where(IsHomeAvailable);
                var shown = revealed ? avail : avail.Take(Tuning.VisibleErkundungPerSub);
                foreach (var c in shown) visible.Add(c.Id);
            }

            // --- Stations: 3 per subcategory (chains are serial, so this shows the frontier) ---
            foreach (string sub in cat.Subcategories(Sparte.Stationen))
            {
                var inSub = cat.InSubcategory(Sparte.Stationen, sub);
                foreach (var c in inSub.Where(IsHomeAvailable).Take(Tuning.VisibleStationenPerSub))
                    visible.Add(c.Id);
            }

            // --- Network/logistics: 3 per subcategory ---
            foreach (string sub in cat.Subcategories(Sparte.NetzwerkLogistik))
            {
                var inSub = cat.InSubcategory(Sparte.NetzwerkLogistik, sub);
                foreach (var c in inSub.Where(IsHomeAvailable).Take(Tuning.VisibleNetzwerkPerSub))
                    visible.Add(c.Id);
            }

            return visible;
        }

        public static List<MissionContract> Queued(ContractCatalog catalog, HashSet<string> visible) =>
            catalog.All.Where(c => IsHomeAvailable(c) && !visible.Contains(c.Id)).ToList();

        /// <summary>Explains an unlocked-but-hidden mission in terms players can act on.</summary>
        public static QueueInfo QueueFor(ContractCatalog catalog, HashSet<string> visible, MissionContract mission)
        {
            if (mission == null || !IsHomeAvailable(mission) || visible.Contains(mission.Id)) return null;
            IEnumerable<MissionContract> scope = mission.HeimatSparte == Sparte.Bemannt
                ? catalog.InSparte(Sparte.Bemannt)
                : catalog.InSubcategory(mission.HeimatSparte, mission.Unterkategorie);
            var waiting = scope.Where(IsHomeAvailable).Where(c => !visible.Contains(c.Id)).ToList();
            int index = waiting.FindIndex(c => c.Id == mission.Id);
            if (index < 0) return null;
            int limit;
            switch (mission.HeimatSparte)
            {
                case Sparte.Bemannt:
                    int total = catalog.InSparte(Sparte.Bemannt).Count();
                    int done = catalog.InSparte(Sparte.Bemannt).Count(ContractManager.IsCompleted);
                    limit = total > 0 && done >= Tuning.BemanntBoostFraction * total
                        ? Tuning.VisibleBemanntBoosted : Tuning.VisibleBemanntBase;
                    break;
                case Sparte.UnbemannteErkundung: limit = Tuning.VisibleErkundungPerSub; break;
                case Sparte.Stationen: limit = Tuning.VisibleStationenPerSub; break;
                default: limit = Tuning.VisibleNetzwerkPerSub; break;
            }
            return new QueueInfo { Position = index + 1, Waiting = waiting.Count, FrontierLimit = limit };
        }
    }
}
