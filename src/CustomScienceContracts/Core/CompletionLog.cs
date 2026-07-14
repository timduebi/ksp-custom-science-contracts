using System;
using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Model;

namespace CustomScienceContracts.Core
{
    /// <summary>One immutable campaign-history event. Unlike MissionContract.TotalCompletions this
    /// preserves every run of a repeatable mission and the science that was actually paid.</summary>
    public sealed class CompletionRecord
    {
        public long Sequence;
        public string MissionId = "";
        public double UniversalTime = -1.0;
        public string Action = "claim"; // claim, skip, import
        public bool HasScience;
        public float Science;
        public bool Imported;
        public string VesselName = "";
        public string Crew = "";
    }

    /// <summary>Append-only program history persisted independently of current mission state.</summary>
    public sealed class CompletionLog
    {
        private readonly List<CompletionRecord> _entries = new List<CompletionRecord>();
        private long _nextSequence = 1;

        public IReadOnlyList<CompletionRecord> Entries => _entries;
        public int Count => _entries.Count;

        public CompletionRecord Add(string missionId, double ut, string action, float? science,
            string vesselName = "", string crew = "", bool imported = false)
        {
            var entry = new CompletionRecord
            {
                Sequence = _nextSequence++,
                MissionId = missionId ?? "",
                UniversalTime = ut,
                Action = string.IsNullOrEmpty(action) ? "claim" : action,
                HasScience = science.HasValue,
                Science = science ?? 0f,
                Imported = imported,
                VesselName = vesselName ?? "",
                Crew = crew ?? ""
            };
            _entries.Add(entry);
            return entry;
        }

        public void AddLoaded(CompletionRecord entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.MissionId)) return;
            if (entry.Sequence <= 0) entry.Sequence = _nextSequence;
            _entries.Add(entry);
            _nextSequence = Math.Max(_nextSequence, entry.Sequence + 1);
        }

        public void Clear()
        {
            _entries.Clear();
            _nextSequence = 1;
        }

        /// <summary>Migrates v1 saves without inventing payouts. There is one historical row per
        /// mission (the old format only knew first-completion UT and aggregate completion count).</summary>
        public int ImportLegacy(IEnumerable<MissionContract> contracts)
        {
            if (_entries.Count != 0 || contracts == null) return 0;
            var completed = contracts.Where(c =>
                    StatePersistencePolicy.ImportLegacyCompletion(c.Status, c.TotalCompletions))
                .OrderBy(c => c.FirstCompletedUT < 0.0 ? double.MaxValue : c.FirstCompletedUT)
                .ThenBy(c => c.Id, StringComparer.Ordinal);
            int count = 0;
            foreach (var c in completed)
            {
                Add(c.Id, c.FirstCompletedUT, "import", null, imported: true);
                count++;
            }
            return count;
        }
    }
}
