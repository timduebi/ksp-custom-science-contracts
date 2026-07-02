using System.Collections.Generic;
using UnityEngine;

namespace CustomScienceContracts.Model
{
    /// <summary>A mission. Definition comes from the .cfg catalog in GameData; runtime state is
    /// stored in the save folder.</summary>
    public class MissionContract
    {
        // --- Definition from the catalog, immutable at runtime ---
        public string Id = "";
        public string Titel = "";
        public string Beschreibung = "";
        public Sparte HeimatSparte;            // Bemannt / UnbemannteErkundung / NetzwerkLogistik
        public string Unterkategorie = "";     // body/category label
        public int Epoch = 1;                   // campaign chapter used by Mission Control UI
        public string EpochTitle = "";          // optional chapter title supplied by the active catalog
        /// <summary>Optional UI icon selected per mission, as filename in Icons/UI without
        /// extension, e.g. "TrackingStation_ButtonMapLander". Empty means fallback by mission shape.</summary>
        public string IconKey = "";
        public List<string> Voraussetzungen = new List<string>();  // ids that must all be completed once
        public List<Condition> Bedingungen = new List<Condition>();
        public float ScienceReward = 0f;
        public bool Repeatable = false;
        /// <summary>Exploration/outer system only: when this contract id is CompletedOnce, all
        /// contracts in the subcategory become visible and the 4-item cap is lifted.</summary>
        public string RevealAllAfter = "";
        /// <summary>When set, the mod records the fulfilling vessel as a station/base under this
        /// key. Resupply contracts reference it through CONDITION.stationKey.</summary>
        public string RecordStationKey = "";
        /// <summary>Display-only reference to a recorded station/base without recording it again, so
        /// follow-up contracts can show its name in titles/%station%.</summary>
        public string StationRef = "";

        // --- Runtime state, persisted in the save folder ---
        public MissionStatus Status = MissionStatus.Locked;
        /// <summary>Repeatable cooldown: reset to 0 when claimed, +1 on every other completion,
        /// accept again when &gt;= Tuning.RepeatableCooldown.</summary>
        public int CompletionsSinceLastClaim = 0;
        /// <summary>Total completion count for statistics/UI.</summary>
        public int TotalCompletions = 0;
        /// <summary>In-game UT of the first completion (claim or skip), -1 if never completed.
        /// Shown in the Campaign Atlas so the timeline doubles as a mission chronicle.</summary>
        public double FirstCompletedUT = -1.0;
        /// <summary>Scratch state for condition evaluators (flyby phase, crew-duration start time,
        /// marker waypoint state, ...). Relevant for active contracts and persisted with the save.</summary>
        public ConfigNode Progress = new ConfigNode("PROGRESS");

        public bool IsRepeatableInPool => Repeatable && TotalCompletions > 0;
    }
}
