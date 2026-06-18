using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Resolves internal body names (CelestialBody.name from configs) at runtime. Never
    /// provides hardcoded body sizes or atmosphere heights; those come from CelestialBody. If a
    /// referenced body is missing, Resolve returns null and the contract stays inert.</summary>
    public static class BodyResolver
    {
        private static readonly Dictionary<string, CelestialBody> _cache =
            new Dictionary<string, CelestialBody>();

        /// <summary>Call on scene change or after Kopernicus load.</summary>
        public static void RebuildCache()
        {
            _cache.Clear();
            if (FlightGlobals.Bodies == null) return;
            foreach (var b in FlightGlobals.Bodies)
            {
                if (b == null) continue;
                _cache[b.name] = b;            // internal config name
                if (!_cache.ContainsKey(b.bodyName)) _cache[b.bodyName] = b; // Display-Fallback
            }
        }

        public static CelestialBody Resolve(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_cache.Count == 0) RebuildCache();
            if (_cache.TryGetValue(name, out var b) && b != null) return b;

            // Last attempt directly through FlightGlobals in case the cache is stale.
            b = FlightGlobals.Bodies?.FirstOrDefault(x => x != null && (x.name == name || x.bodyName == name));
            if (b != null) _cache[name] = b;
            else Debug.LogWarning($"[CSC] Body '{name}' not found (missing or incompatible planet pack?).");
            return b;
        }

        public static bool Exists(string name) => Resolve(name) != null;
    }
}
