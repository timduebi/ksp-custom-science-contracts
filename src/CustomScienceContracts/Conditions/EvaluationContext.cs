using System.Collections.Generic;
using CustomScienceContracts.Core;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Schnappschuss des Spielzustands fuer einen Pruef-Tick, plus die gepufferten
    /// diskreten Events. Wird an alle Condition-Evaluatoren durchgereicht.</summary>
    public class EvaluationContext
    {
        public double UniversalTime;
        public IReadOnlyList<Vessel> Vessels;   // FlightGlobals.Vessels-Schnappschuss
        public GameEventBridge Events;          // Andocken / SOI / Situation seit letztem Tick
        public StationRegistry Stations;        // gemerkte Stationen (fuer stationKey-Bedingungen)
    }
}
