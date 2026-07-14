using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Pure schema decisions shared by persistence and unit tests.</summary>
    public static class StatePersistencePolicy
    {
        public const int CurrentVersion = 2;

        public static bool PersistProgress(MissionStatus status) =>
            status == MissionStatus.Active || status == MissionStatus.ReadyToClaim;

        public static bool ImportLegacyCompletion(MissionStatus status, int totalCompletions) =>
            totalCompletions > 0 || status == MissionStatus.CompletedOnce;
    }
}
