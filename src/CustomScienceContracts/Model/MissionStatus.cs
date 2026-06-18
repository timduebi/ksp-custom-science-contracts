namespace CustomScienceContracts.Model
{
    /// <summary>Runtime status of a mission in the flow Locked -> Available -> Active -> CompletedOnce.</summary>
    public enum MissionStatus
    {
        /// <summary>Not all prerequisites are complete.</summary>
        Locked,
        /// <summary>Prerequisites complete, visible in the selection window, not tracked yet.</summary>
        Available,
        /// <summary>Accepted by the player, tracked, counts against the active limit.</summary>
        Active,
        /// <summary>Conditions fulfilled, but not claimed yet. Reward is ready; follow-up missions
        /// unlock only when claimed. Still counts as active.</summary>
        ReadyToClaim,
        /// <summary>Claimed and reward paid.</summary>
        CompletedOnce
    }
}
