using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using System.Linq;
using System.Collections.Generic;
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

    [Theory]
    [InlineData(1, 246, true)]
    [InlineData(2, 246, true)]
    [InlineData(0, 246, false)]
    [InlineData(2, 0, false)]
    public void BareConfigNodeSaveLayoutIsRecognized(int version, int states, bool expected) =>
        Assert.Equal(expected, StatePersistencePolicy.LooksLikeBareState(version, states));

    [Fact]
    public void UnlockPathIsTopologicalMinimalAndSkipsCompletedAncestors()
    {
        var a = new MissionContract { Id = "a", Status = MissionStatus.CompletedOnce };
        var b = new MissionContract { Id = "b", Voraussetzungen = { "a" } };
        var c = new MissionContract { Id = "c", Voraussetzungen = { "a" } };
        var d = new MissionContract { Id = "d", Voraussetzungen = { "b", "c" } };
        var byId = new Dictionary<string, MissionContract>
            { ["a"] = a, ["b"] = b, ["c"] = c, ["d"] = d };

        var path = UnlockPath.Build(d, id => byId.TryGetValue(id, out var m) ? m : null,
            mission => mission.TotalCompletions > 0 || mission.Status == MissionStatus.CompletedOnce);

        Assert.Equal(new[] { "b", "c" }, path.Select(mission => mission.Id));
    }

    [Fact]
    public void RelayTopologyRequiresReserveAndUsefulPhasing()
    {
        Assert.True(NetworkTopologyPolicy.Meets(new[] { 0.0, 90.0, 180.0, 270.0 },
            primaries: 3, redundancy: 1, separationMin: 20, maxGap: 150));
        Assert.False(NetworkTopologyPolicy.Meets(new[] { 0.0, 120.0, 240.0 },
            primaries: 3, redundancy: 1, separationMin: 20, maxGap: 150));
        Assert.False(NetworkTopologyPolicy.Meets(new[] { 0.0, 5.0, 180.0, 270.0 },
            primaries: 3, redundancy: 1, separationMin: 20, maxGap: 150));
    }

    [Fact]
    public void DeliveryTracksOnlyPositiveDeltaAndNeverLosesObservedProgress()
    {
        Assert.Equal(0, ResourceDeliveryPolicy.Accumulate(500, 450, 0));
        Assert.Equal(175, ResourceDeliveryPolicy.Accumulate(500, 675, 0));
        Assert.Equal(175, ResourceDeliveryPolicy.Accumulate(500, 530, 175));
        Assert.True(ResourceDeliveryPolicy.Reached(200, 200));
        Assert.False(ResourceDeliveryPolicy.Reached(199, 200));
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
