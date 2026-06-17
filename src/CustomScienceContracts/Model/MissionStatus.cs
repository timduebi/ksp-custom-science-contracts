namespace CustomScienceContracts.Model
{
    /// <summary>Laufzeit-Status einer Mission im Flow Locked -> Available -> Active -> CompletedOnce.</summary>
    public enum MissionStatus
    {
        /// <summary>Nicht alle Voraussetzungen erfuellt.</summary>
        Locked,
        /// <summary>Alle Voraussetzungen erfuellt, im Auswahlfenster sichtbar (Sichtbarkeitsregeln), nicht getrackt.</summary>
        Available,
        /// <summary>Vom Spieler angenommen, wird getrackt, zaehlt gegen das Aktiv-Limit.</summary>
        Active,
        /// <summary>Bedingungen erfuellt (gruen), aber noch NICHT eingeloest. Reward steht zum Abholen
        /// bereit; Folgemissionen werden erst beim Einloesen freigeschaltet. Zaehlt noch als aktiv.</summary>
        ReadyToClaim,
        /// <summary>Eingeloest, Reward ausgezahlt.</summary>
        CompletedOnce
    }
}
