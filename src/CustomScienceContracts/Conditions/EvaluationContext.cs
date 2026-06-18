using System.Collections.Generic;
using CustomScienceContracts.Core;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Snapshot of game state for one check tick plus buffered discrete events.</summary>
    public class EvaluationContext
    {
        public double UniversalTime;
        public IReadOnlyList<Vessel> Vessels;   // FlightGlobals.Vessels snapshot
        public GameEventBridge Events;          // docking / SOI / situation since last tick
        public StationRegistry Stations;        // recorded stations for stationKey conditions
    }
}
