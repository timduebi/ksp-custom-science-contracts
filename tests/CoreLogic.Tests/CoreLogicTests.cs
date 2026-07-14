using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using System.Linq;
using Xunit;

namespace CoreLogic.Tests;

public class CoreLogicTests
{
    [Fact]
    public void HashIsStableAndSensitive()
    {
        Assert.Equal(1335831723, DeterministicHash.Of("hello"));
        Assert.Equal(DeterministicHash.Of("mission#1"), DeterministicHash.Of("mission#1"));
        Assert.NotEqual(DeterministicHash.Of("mission#1"), DeterministicHash.Of("mission#2"));
    }

    [Theory]
    [InlineData(MissionStatus.Active, true)]
    [InlineData(MissionStatus.ReadyToClaim, true)]
    [InlineData(MissionStatus.Locked, false)]
    [InlineData(MissionStatus.Available, false)]
    [InlineData(MissionStatus.CompletedOnce, false)]
    public void OnlyLiveEvaluationStateIsPersisted(MissionStatus status, bool expected) =>
        Assert.Equal(expected, StatePersistencePolicy.PersistProgress(status));

    [Fact]
    public void LegacyCompletionsCanBeImported()
    {
        Assert.True(StatePersistencePolicy.ImportLegacyCompletion(MissionStatus.CompletedOnce, 0));
        Assert.True(StatePersistencePolicy.ImportLegacyCompletion(MissionStatus.Available, 2));
        Assert.False(StatePersistencePolicy.ImportLegacyCompletion(MissionStatus.Available, 0));
    }

    [Fact]
    public void LegacyLogMigrationDoesNotInventPayoutsOrRepeatRuns()
    {
        var log = new CompletionLog();
        int imported = log.ImportLegacy(new[]
        {
            new MissionContract { Id = "repeat", Status = MissionStatus.Available,
                TotalCompletions = 4, FirstCompletedUT = 200 },
            new MissionContract { Id = "once", Status = MissionStatus.CompletedOnce,
                TotalCompletions = 1, FirstCompletedUT = 100 },
            new MissionContract { Id = "open", Status = MissionStatus.Available },
        });
        Assert.Equal(2, imported);
        Assert.Equal(new[] { "once", "repeat" }, log.Entries.Select(entry => entry.MissionId));
        Assert.All(log.Entries, entry =>
        {
            Assert.True(entry.Imported);
            Assert.False(entry.HasScience);
            Assert.Equal("import", entry.Action);
        });
    }
}
