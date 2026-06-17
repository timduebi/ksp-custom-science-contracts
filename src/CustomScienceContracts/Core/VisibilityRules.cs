using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Data;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Berechnet, welche Available-Contracts im Auswahlfenster (Heimatsparten) sichtbar sind.
    /// Repeatable-Contracts, die schon einmal abgeschlossen wurden, leben in der Sparte Wiederholbar
    /// und werden hier NICHT mehr gelistet.</summary>
    public static class VisibilityRules
    {
        /// <summary>Contract ist in seiner Heimatsparte listbar (Available und noch nicht in den
        /// Wiederholbar-Pool gewandert).</summary>
        public static bool IsHomeAvailable(MissionContract c) =>
            c.Status == MissionStatus.Available && !c.IsRepeatableInPool;

        /// <summary>Liefert die Menge der Ids, die in den Heimatsparten aktuell sichtbar sind.
        /// Reihenfolge = Katalog-Reihenfolge (stabil).</summary>
        public static HashSet<string> ComputeVisible(ContractCatalog cat)
        {
            var visible = new HashSet<string>();

            // Test-Schalter: alles freischaltbare ohne Limits zeigen.
            if (Tuning.UnlockAll)
            {
                foreach (var c in cat.All)
                    if (IsHomeAvailable(c)) visible.Add(c.Id);
                return visible;
            }

            // --- Bemannt: globales Limit 3, ab >= 50 % CompletedOnce -> 5 ---
            var bemannt = cat.InSparte(Sparte.Bemannt).ToList();
            int bemTotal = bemannt.Count;
            int bemDone = bemannt.Count(ContractManager.IsCompleted);
            int bemCap = (bemTotal > 0 && bemDone >= Tuning.BemanntBoostFraction * bemTotal)
                ? Tuning.VisibleBemanntBoosted
                : Tuning.VisibleBemanntBase;
            foreach (var c in bemannt.Where(IsHomeAvailable).Take(bemCap))
                visible.Add(c.Id);

            // --- Unbemannte Erkundung: 4 pro Unterkategorie; RevealAllAfter hebt das Limit auf ---
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

            // --- Netzwerk/Logistik: 3 pro Unterkategorie ---
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
