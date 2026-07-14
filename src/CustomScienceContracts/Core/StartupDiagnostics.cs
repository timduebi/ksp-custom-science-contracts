using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CustomScienceContracts.Data;

namespace CustomScienceContracts.Core
{
    public enum DiagnosticSeverity { Info, Warning, Error }

    public sealed class DiagnosticEntry
    {
        public DiagnosticSeverity Severity;
        public string Message;
    }

    /// <summary>One compact startup report for profile, layout, bodies, companion mods and duplicate
    /// assemblies. It turns silent "all missions inert" states into an actionable log entry.</summary>
    public static class StartupDiagnostics
    {
        private static readonly List<DiagnosticEntry> _last = new List<DiagnosticEntry>();
        public static IReadOnlyList<DiagnosticEntry> Last => _last;
        public static string Profile { get; private set; } = "Unknown";
        public static int ErrorCount => _last.Count(x => x.Severity == DiagnosticSeverity.Error);
        public static int WarningCount => _last.Count(x => x.Severity == DiagnosticSeverity.Warning);

        public static void Run(ContractCatalog catalog)
        {
            _last.Clear();
            var bodies = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var mission in catalog.All)
                foreach (var condition in mission.Bedingungen)
                    foreach (var check in condition.Checks)
                    {
                        if (!string.IsNullOrEmpty(check.Body)) bodies.Add(check.Body);
                        if (!string.IsNullOrEmpty(check.ReturnBody)) bodies.Add(check.ReturnBody);
                    }

            bool sol = bodies.Contains("Earth") || bodies.Contains("Moon");
            bool stock = bodies.Contains("Kerbin") || bodies.Contains("Mun");
            Profile = sol && !stock ? "SOL" : stock && !sol ? "Stock" : "Mixed/Unknown";
            Add(DiagnosticSeverity.Info, $"Catalog profile: {Profile}; {catalog.All.Count} missions.");
            if (sol && stock) Add(DiagnosticSeverity.Error, "Catalog mixes SOL and Stock body names.");

            if (FlightGlobals.Bodies == null || FlightGlobals.Bodies.Count == 0)
                Add(DiagnosticSeverity.Warning, "Celestial bodies are not available yet; body validation deferred.");
            else
            {
                var missing = bodies.Where(name => !BodyResolver.Exists(name)).OrderBy(x => x).ToList();
                if (missing.Count > 0)
                    Add(DiagnosticSeverity.Error, $"{missing.Count} catalog bodies are missing: {string.Join(", ", missing.Take(8))}{(missing.Count > 8 ? " …" : "")}");
                else
                    Add(DiagnosticSeverity.Info, $"All {bodies.Count} referenced celestial bodies resolved.");
            }

            string root = KSPUtil.ApplicationRootPath ?? "";
            string gameData = Path.Combine(root, "GameData");
            string location = Assembly.GetExecutingAssembly().Location.Replace('\\', '/');
            if (location.IndexOf("/GameData/CustomScienceContracts/Plugins/", StringComparison.OrdinalIgnoreCase) < 0)
                Add(DiagnosticSeverity.Error, $"Assembly is outside GameData/CustomScienceContracts/Plugins: {location}");
            if (!Directory.Exists(Path.Combine(gameData, "CustomScienceContracts", "Icons")))
                Add(DiagnosticSeverity.Error, "Icons directory is missing from the installed mod.");

            int copies = AppDomain.CurrentDomain.GetAssemblies().Count(a =>
                string.Equals(a.GetName().Name, ModInfo.Name, StringComparison.OrdinalIgnoreCase));
            if (copies > 1) Add(DiagnosticSeverity.Error, $"Duplicate loaded assemblies detected: {copies} copies.");

            if (Profile == "SOL")
            {
                bool ctt = Directory.Exists(Path.Combine(gameData, "CommunityTechTree"));
                bool pbc = Directory.Exists(Path.Combine(gameData, "ProbesBeforeCrew"));
                Add(DiagnosticSeverity.Info,
                    $"Balance companions (optional): Community Tech Tree {(ctt ? "found" : "not found")}; Probes Before Crew {(pbc ? "found" : "not found")}.");
            }

            Log.Info($"Startup diagnostics: profile={Profile}, warnings={WarningCount}, errors={ErrorCount}");
            foreach (var entry in _last)
            {
                string line = $"Diagnostics [{entry.Severity}]: {entry.Message}";
                if (entry.Severity == DiagnosticSeverity.Error) Log.Error(line);
                else if (entry.Severity == DiagnosticSeverity.Warning) Log.Warn(line);
                else Log.Info(line);
            }
        }

        private static void Add(DiagnosticSeverity severity, string message) =>
            _last.Add(new DiagnosticEntry { Severity = severity, Message = message });
    }
}
