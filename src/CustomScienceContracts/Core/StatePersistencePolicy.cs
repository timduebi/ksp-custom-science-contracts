using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>Pure schema decisions shared by persistence and unit tests.</summary>
    public static class StatePersistencePolicy
    {
        public const int CurrentVersion = 3;
        public const int CurrentEvaluationSchema = 3;

        public static bool PersistProgress(MissionStatus status) =>
            status == MissionStatus.Active || status == MissionStatus.ReadyToClaim;

        public static bool ImportLegacyCompletion(MissionStatus status, int totalCompletions) =>
            totalCompletions > 0 || status == MissionStatus.CompletedOnce;

        /// <summary>KSP's ConfigNode.Save writes the node contents without the node's own name.
        /// Versions 0.1-0.7 therefore produced a valid bare state document whose top-level node
        /// directly contains version and STATE entries. Keep accepting that layout permanently.</summary>
        public static bool LooksLikeBareState(int version, int stateCount) =>
            version > 0 && stateCount > 0;
    }
}
