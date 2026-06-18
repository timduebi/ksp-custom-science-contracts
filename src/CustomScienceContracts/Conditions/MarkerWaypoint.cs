using System;
using System.Collections.Generic;
using CustomScienceContracts.Core;
using FinePrint;
using FinePrint.Utilities;
using UnityEngine;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Creates/removes a visible stock waypoint (FinePrint) for precision landings. Uses
    /// the typed ScenarioCustomWaypoints API. Waypoint object state only lives at runtime; after a
    /// reload it is recreated from persisted lat/lon values.</summary>
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
                Log.V($"Refreshing marker waypoint cache after scene change: {contractId}");
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
                    Log.V($"Marker waypoint visible only in Flight/Map/Tracking: {contractId}");
                    return;
                }

                Remove(contractId);
                var wp = new Waypoint
                {
                    celestialName = body.name,
                    latitude = lat,
                    longitude = lon,
                    altitude = 0.0,
                    name = string.IsNullOrEmpty(label) ? "Target" : label,
                    id = "report",          // icon
                    seed = seed,            // color
                    isOnSurface = true,
                    isNavigatable = true
                };
                ScenarioCustomWaypoints.AddWaypoint(wp);
                _active[contractId] = wp;
                NoteScene();
                Log.Info($"Marker set: {wp.name} @ {lat:0.00}/{lon:0.00} on {body.name}");
            }
            catch (Exception e) { Log.Warn($"Marker waypoint creation failed: {e.Message}"); }
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
            catch (Exception e) { Log.Warn($"Marker waypoint removal failed: {e.Message}"); }
        }

        /// <summary>Removes all waypoints of a contract (key == id or starts with "id#", e.g. several
        /// MARKER_LANDING checks). Used for claim/abandon of composite missions.</summary>
        public static void RemoveAll(string contractId)
        {
            var hit = new List<string>();
            foreach (var k in _active.Keys)
                if (k == contractId || k.StartsWith(contractId + "#", StringComparison.Ordinal)) hit.Add(k);
            foreach (var k in hit) Remove(k);
        }
    }
}
