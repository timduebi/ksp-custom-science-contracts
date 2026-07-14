using System;

namespace CustomScienceContracts.Conditions
{
    /// <summary>Operational relay detection for loaded and on-rails vessels, with optional stock
    /// CommNet connectivity and graceful compatibility fallback for replacement comm systems.</summary>
    public static class RelayCapability
    {
        public static bool IsOperational(Vessel vessel, out string reason)
        {
            reason = "no enabled relay antenna";
            if (vessel == null) return false;
            bool hardware = LoadedHardware(vessel) || ProtoHardware(vessel);
            if (!hardware) return false;
            if (!CommNetConnected(vessel))
            {
                reason = "relay has no active CommNet connection";
                return false;
            }
            reason = "operational relay connected";
            return true;
        }

        private static bool LoadedHardware(Vessel vessel)
        {
            if (!vessel.loaded || vessel.parts == null) return false;
            foreach (Part part in vessel.parts)
            {
                if (part?.Modules == null) continue;
                foreach (PartModule module in part.Modules)
                    if (IsRelay(module) && module.isEnabled && module.enabled && CanComm(module, null)) return true;
            }
            return false;
        }

        private static bool ProtoHardware(Vessel vessel)
        {
            if (vessel.loaded || vessel.protoVessel?.protoPartSnapshots == null) return false;
            foreach (ProtoPartSnapshot part in vessel.protoVessel.protoPartSnapshots)
            {
                Part prefab = part?.partInfo?.partPrefab;
                if (prefab?.Modules == null) continue;
                foreach (PartModule module in prefab.Modules)
                    if (IsRelay(module) && ProtoModuleEnabled(part, module.moduleName) && CanComm(module, ModuleSnapshot(part, module.moduleName)))
                        return true;
            }
            return false;
        }

        private static bool ProtoModuleEnabled(ProtoPartSnapshot part, string moduleName)
        {
            if (part?.modules == null) return true;
            foreach (ProtoPartModuleSnapshot snapshot in part.modules)
            {
                if (snapshot == null || !string.Equals(snapshot.moduleName, moduleName, StringComparison.OrdinalIgnoreCase))
                    continue;
                string raw = snapshot.moduleValues?.GetValue("isEnabled") ??
                             snapshot.moduleValues?.GetValue("moduleIsEnabled") ??
                             snapshot.moduleValues?.GetValue("enabled");
                return !bool.TryParse(raw, out bool enabled) || enabled;
            }
            return true;
        }

        private static ProtoPartModuleSnapshot ModuleSnapshot(ProtoPartSnapshot part, string moduleName)
        {
            if (part?.modules == null) return null;
            foreach (ProtoPartModuleSnapshot snapshot in part.modules)
                if (snapshot != null && string.Equals(snapshot.moduleName, moduleName, StringComparison.OrdinalIgnoreCase))
                    return snapshot;
            return null;
        }

        private static bool CanComm(PartModule module, ProtoPartModuleSnapshot snapshot)
        {
            if (!StockCommNetEnabled()) return true;
            try
            {
                object result;
                if (snapshot == null)
                    result = module.GetType().GetMethod("CanComm", Type.EmptyTypes)?.Invoke(module, null);
                else
                    result = module.GetType().GetMethod("CanCommUnloaded", new[] { typeof(ProtoPartModuleSnapshot) })
                        ?.Invoke(module, new object[] { snapshot });
                return !(result is bool canComm) || canComm;
            }
            catch (Exception) { return true; }
        }

        private static bool IsRelay(PartModule module)
        {
            if (module == null) return false;
            string name = module.moduleName ?? module.GetType().Name ?? "";
            if (!string.Equals(name, "ModuleDataTransmitter", StringComparison.OrdinalIgnoreCase)) return false;
            object antennaType = module.GetType().GetField("antennaType")?.GetValue(module);
            if (antennaType != null && antennaType.ToString().IndexOf("RELAY", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;
            string field = module.Fields?.GetValue("antennaType")?.ToString();
            return !string.IsNullOrEmpty(field) && field.IndexOf("RELAY", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool CommNetConnected(Vessel vessel)
        {
            if (!StockCommNetEnabled()) return true;
            try
            {
                Type type = vessel.GetType();
                object connection = type.GetField("connection")?.GetValue(vessel) ??
                                    type.GetProperty("Connection")?.GetValue(vessel, null);
                if (connection == null) return true;
                Type connectionType = connection.GetType();
                object value = connectionType.GetProperty("IsConnected")?.GetValue(connection, null) ??
                               connectionType.GetField("IsConnected")?.GetValue(connection);
                return !(value is bool connected) || connected;
            }
            catch (Exception) { return true; }
        }

        private static bool StockCommNetEnabled()
        {
            try
            {
                object difficulty = HighLogic.CurrentGame?.Parameters?.Difficulty;
                if (difficulty == null) return true;
                Type type = difficulty.GetType();
                object value = type.GetProperty("EnableCommNet")?.GetValue(difficulty, null) ??
                               type.GetField("EnableCommNet")?.GetValue(difficulty);
                return !(value is bool enabled) || enabled;
            }
            catch (Exception) { return true; }
        }
    }
}
