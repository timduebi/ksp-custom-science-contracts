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

            // --- Network/logistics: 3 per subcategory ---
            foreach (string sub in cat.Subcategories(Sparte.NetzwerkLogistik))
            {
                var inSub = cat.InSubcategory(Sparte.NetzwerkLogistik, sub);
                foreach (var c in inSub.Where(IsHomeAvailable).Take(Tuning.VisibleNetzwerkPerSub))
                    visible.Add(c.Id);
            }

            return visible;
        }
    }
}
