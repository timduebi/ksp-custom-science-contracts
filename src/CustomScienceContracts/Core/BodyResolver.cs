using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Loest interne Body-Namen (CelestialBody.name aus den configs) zur Laufzeit auf.
    /// Liefert NIE hardcodete Groessen/Atmosphaerenhoehen — die kommen aus dem CelestialBody.
    /// Standalone: ist ein referenzierter Body nicht vorhanden (kein SOL installiert),
    /// gibt Resolve null zurueck und der zugehoerige Contract bleibt inert.</summary>
    public static class BodyResolver
    {
        private static readonly Dictionary<string, CelestialBody> _cache =
            new Dictionary<string, CelestialBody>();

        /// <summary>Bei Szenenwechsel / nach Kopernicus-Load aufrufen.</summary>
        public static void RebuildCache()
        {
            _cache.Clear();
            if (FlightGlobals.Bodies == null) return;
            foreach (var b in FlightGlobals.Bodies)
            {
                if (b == null) continue;
                _cache[b.name] = b;            // interner Name (configs)
                if (!_cache.ContainsKey(b.bodyName)) _cache[b.bodyName] = b; // Display-Fallback
            }
        }

        public static CelestialBody Resolve(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_cache.Count == 0) RebuildCache();
            if (_cache.TryGetValue(name, out var b) && b != null) return b;

            // letzter Versuch direkt ueber FlightGlobals (Cache evtl. veraltet)
            b = FlightGlobals.Bodies?.FirstOrDefault(x => x != null && (x.name == name || x.bodyName == name));
            if (b != null) _cache[name] = b;
            else Debug.LogWarning($"[CSC] Body '{name}' nicht gefunden (Mod ohne passenden Planetenpack?).");
            return b;
        }

        public static bool Exists(string name) => Resolve(name) != null;
    }
}
