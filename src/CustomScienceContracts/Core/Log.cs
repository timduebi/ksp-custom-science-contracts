using System;
using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Einheitliches Logging mit [CSC]-Prefix. Verbose-Zeilen nur wenn Tuning.VerboseLogging.</summary>
    public static class Log
    {
        public static void Info(string m) => Debug.Log("[CSC] " + m);
        public static void Warn(string m) => Debug.LogWarning("[CSC] " + m);
        public static void Error(string m) => Debug.LogError("[CSC] " + m);
        public static void Ex(string ctx, Exception e) => Debug.LogError($"[CSC] EX in {ctx}: {e}");
        public static void V(string m) { if (Tuning.VerboseLogging) Debug.Log("[CSC][v] " + m); }
    }
}
