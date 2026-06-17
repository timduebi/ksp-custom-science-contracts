using System;
using System.Collections.Generic;
using CustomScienceContracts.Core;
using FinePrint;
using FinePrint.Utilities;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Setzt/entfernt einen sichtbaren Stock-Wegpunkt (FinePrint) fuer Praezisionslandungen.
    /// Nutzt die getypte ScenarioCustomWaypoints-API (zuverlaessiger als Reflection). Der Wegpunkt-
    /// Objektzustand lebt nur zur Laufzeit — nach einem Neuladen wird er aus den persistierten
    /// Lat/Lon neu erzeugt (siehe MarkerLandingEvaluator.EnsureMarker).</summary>
    public static class MarkerWaypoint
    {
        private static readonly Dictionary<string, Waypoint> _active = new Dictionary<string, Waypoint>();
        private static bool _sceneKnown;
        private static GameScenes _scene;

        private static bool WaypointScene =>
            HighLogic.LoadedSceneIsFlight ||
            HighLogic.LoadedScene == GameScenes.TRACKSTATION;

        private static bool SceneChanged =>
            _sceneKnown && _scene != HighLogic.LoadedScene;

        private static void NoteScene()
        {
            _scene = HighLogic.LoadedScene;
            _sceneKnown = true;
        }

        public static bool Has(string contractId)
        {
            if (!WaypointScene) return false;
            if (!_active.TryGetValue(contractId, out var wp) || wp == null)
            {
                _active.Remove(contractId);
                return false;
            }
            if (SceneChanged)
            {
                Log.V($"Marker-Waypoint-Cache nach Szenenwechsel erneuern: {contractId}");
                return false;
            }
            return true;
        }

        public static void Set(string contractId, CelestialBody body, double lat, double lon, string label, int seed)
        {
            try
            {
                if (!WaypointScene)
                {
                    Log.V($"Marker-Waypoint erst in Flight/Map/Tracking sichtbar: {contractId}");
                    return;
                }

                Remove(contractId);
                var wp = new Waypoint
                {
                    celestialName = body.name,
                    latitude = lat,
                    longitude = lon,
                    altitude = 0.0,
                    name = string.IsNullOrEmpty(label) ? "Ziel" : label,
                    id = "report",          // Icon
                    seed = seed,            // Farbe
                    isOnSurface = true,
                    isNavigatable = true
                };
                ScenarioCustomWaypoints.AddWaypoint(wp);
                _active[contractId] = wp;
                NoteScene();
                Log.Info($"Marker gesetzt: {wp.name} @ {lat:0.00}/{lon:0.00} auf {body.name}");
            }
            catch (Exception e) { Log.Warn($"Marker-Waypoint setzen fehlgeschlagen: {e.Message}"); }
        }

        public static void Remove(string contractId)
        {
            try
            {
                if (_active.TryGetValue(contractId, out var wp))
                {
                    ScenarioCustomWaypoints.RemoveWaypoint(wp);
                    _active.Remove(contractId);
                }
            }
            catch (Exception e) { Log.Warn($"Marker-Waypoint entfernen fehlgeschlagen: {e.Message}"); }
        }

        /// <summary>Entfernt alle Waypoints eines Contracts (Schluessel == id oder beginnt mit "id#",
        /// z.B. mehrere MARKER_LANDING-Checks). Fuer Claim/Abandon einer COMPOSITE-Mission.</summary>
        public static void RemoveAll(string contractId)
        {
            var hit = new List<string>();
            foreach (var k in _active.Keys)
                if (k == contractId || k.StartsWith(contractId + "#", StringComparison.Ordinal)) hit.Add(k);
            foreach (var k in hit) Remove(k);
        }
    }
}
