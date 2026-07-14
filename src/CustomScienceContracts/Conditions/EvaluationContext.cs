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

        private readonly Dictionary<uint, Vessel> _byId = new Dictionary<uint, Vessel>();
        private readonly List<Vessel> _real = new List<Vessel>();
        public IReadOnlyList<Vessel> RealVessels => _real;

        /// <summary>Builds the per-tick indexes once. Active checks used to rescan every vessel for
        /// every mission/check, which became quadratic in mature saves with large constellations.</summary>
        public void BuildIndexes()
        {
            _byId.Clear();
            _real.Clear();
            if (Vessels == null) return;
            for (int i = 0; i < Vessels.Count; i++)
            {
                Vessel vessel = Vessels[i];
                if (vessel == null) continue;
                _byId[vessel.persistentId] = vessel;
                if (VesselQuery.IsReal(vessel)) _real.Add(vessel);
            }
        }

        public Vessel FindVessel(uint persistentId) =>
            persistentId != 0 && _byId.TryGetValue(persistentId, out Vessel vessel) ? vessel : null;
    }
}
