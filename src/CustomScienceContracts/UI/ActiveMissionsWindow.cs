using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Active missions window with objective status, claim/abort actions and transient reward feedback.</summary>
    public class ActiveMissionsWindow
    {
        private Vector2 _scroll;
        public string PendingAbortId;
        public Rect ConfirmRect = new Rect(0, 0, 340, 175);

        private static readonly Sparte[] Groups =
            { Sparte.Bemannt, Sparte.UnbemannteErkundung, Sparte.NetzwerkLogistik };
        private readonly HashSet<string> _collapsed = new HashSet<string>();

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            DrawClose(width, onClose);

            var active = mgr.ActiveContracts().ToList();
            GUILayout.Label($"Active Missions: {active.Count}", Theme.Title);
            DrawClaimInfo(mgr);

            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Width(width - 8), GUILayout.Height(height - 96));
            if (active.Count == 0)
                GUILayout.Label("No missions are currently active.", Theme.Locked);

            // Grouped by branch, each branch collapsible.
            foreach (var sparte in Groups)
            {
                var inSp = active.Where(c => c.HeimatSparte == sparte).ToList();
                if (inSp.Count == 0) continue;

                var sv = BodyVisual.ForSparte(sparte);
                string key = sparte.ToString();
                bool open = !_collapsed.Contains(key);
                string head = $"{(open ? "▼" : "▶")}  {SparteDisplay.Name(sparte)}   ({inSp.Count})";
                if (GUILayout.Button(head, Theme.GroupHeaderPlain, GUILayout.Height(28)))
                { if (!_collapsed.Remove(key)) _collapsed.Add(key); }
                // The group header only shows the colored branch bar; mission icons live on mission cards.
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawLeftAccent(GUILayoutUtility.GetLastRect(), sv.Color, null);
                if (!open) continue;

                foreach (var c in inSp) DrawActiveItem(mgr, c);
                GUILayout.Space(4);
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(0, 0, width - 34, 24));
        }

        /// <summary>Science reward with multiplier and science icon, right-aligned.</summary>
        public static void DrawReward(float baseReward, ContractManager mgr)
        {
            float val = baseReward * (float)mgr.ScienceMultiplier;
            GUILayout.BeginHorizontal(GUILayout.Width(86));
            GUILayout.FlexibleSpace();
            var prev = GUI.color; GUI.color = Theme.Accent;
            var sci = IconLibrary.UI("science symbol");
            if (sci != null) GUILayout.Label(sci, Theme.RewardIcon, GUILayout.Width(15), GUILayout.Height(15));
            GUILayout.Label($"{val:0}", Theme.Reward);
            GUI.color = prev;
            GUILayout.EndHorizontal();
        }

        private void DrawActiveItem(ContractManager mgr, MissionContract c)
        {
            bool ready = c.Status == MissionStatus.ReadyToClaim;
            GUILayout.BeginVertical(ready ? Theme.ItemBoxReady : Theme.ItemBox);

            // Title + science reward.
            GUILayout.BeginHorizontal();
            GUILayout.Label(c.Titel, ready ? Theme.CondOk : Theme.ItemTitle);
            GUILayout.FlexibleSpace();
            DrawReward(c.ScienceReward, mgr);
            GUILayout.EndHorizontal();

            GUILayout.Label(BodyVisual.DisplayName(c.Unterkategorie), Theme.ItemSub);
            string desc = RenderDescription(c, mgr);
            if (!string.IsNullOrEmpty(desc)) GUILayout.Label(desc, Theme.ItemDesc);

            string stn = StationTarget(c, mgr);
            if (stn != null) GUILayout.Label(stn, Theme.Station);

            // Objectives, one per line and colored by status.
            GUILayout.Space(2);
            DrawChecklist(mgr, c, ready);
            string prog = ProgressLine(c);
            if (prog != null) GUILayout.Label(prog, Theme.Pill);

            // Vessel assignment block (which craft this mission tracks). Hidden once claimable.
            if (!ready) DrawAssignment(mgr, c);

            GUILayout.EndVertical();
            Rect r = GUILayoutUtility.GetLastRect();
            // Left bar: body-colored, green when claimable, with mission icon.
            if (Event.current.type == EventType.Repaint)
                Theme.DrawLeftAccent(r, ready ? Theme.ClaimGreen : BodyVisual.ForBody(BodyVisual.PrimaryBody(c)).Color,
                    BodyVisual.MissionIcon(c), 6f, 22f);

            if (ready)
            {
                if (Theme.DrawRightAction(r, Theme.ClaimGreen, "✓")) mgr.Claim(c.Id);
            }
            else
            {
                if (Theme.DrawRightAction(r, Theme.AbortRed, "✕")) PendingAbortId = c.Id;
            }
            GUILayout.Space(5);
        }

        /// <summary>Vessel-to-mission binding controls: one assigned craft for single-vessel missions,
        /// or a satellite list for network missions. Assigning needs flight; other scenes show status.</summary>
        private void DrawAssignment(ContractManager mgr, MissionContract c)
        {
            bool fleet = ContractManager.IsFleetMission(c);
            if (!fleet && !ContractManager.IsSingleBindable(c)) return;

            bool inFlight = HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null;
            GUILayout.Space(3);
            GUILayout.BeginVertical(Theme.DetailBox);
            if (fleet) DrawFleetAssignment(mgr, c, inFlight);
            else DrawSingleAssignment(mgr, c, inFlight);
            GUILayout.EndVertical();
        }

        private static void DrawSingleAssignment(ContractManager mgr, MissionContract c, bool inFlight)
        {
            if (MissionBinding.IsLost(c))
            {
                GUILayout.Label("Assigned vessel lost - reassign", Theme.Warn);
                if (inFlight && GUILayout.Button("Assign current vessel", Theme.FoldoutBtn, GUILayout.Height(24)))
                    mgr.AssignActiveVessel(c.Id);
                return;
            }
            uint vid = MissionBinding.AssignedVid(c);
            if (vid != 0)
            {
                bool present = FlightGlobals.Vessels != null &&
                               FlightGlobals.Vessels.Any(v => v != null && v.persistentId == vid);
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Vessel: {MissionBinding.AssignedName(c)}{(present ? "" : "  (not visible)")}", Theme.Station);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Detach", Theme.FoldoutBtn, GUILayout.Width(74), GUILayout.Height(22)))
                    mgr.ClearAssignment(c.Id);
                GUILayout.EndHorizontal();
            }
            else
            {
                if (ContractManager.RequiresAssignment(c))
                    GUILayout.Label("Assign the station vessel so this mission can finish.", Theme.Warn);
                if (inFlight)
                {
                    if (GUILayout.Button("Assign current vessel", Theme.FoldoutBtn, GUILayout.Height(24)))
                        mgr.AssignActiveVessel(c.Id);
                    GUILayout.Label("Tip: assign once the craft is in its final stage configuration.", Theme.Locked);
                }
                else
                {
                    GUILayout.Label("Tracks the active vessel. Assign a craft in flight to pin it.", Theme.Locked);
                }
            }
        }

        private static void DrawFleetAssignment(ContractManager mgr, MissionContract c, bool inFlight)
        {
            var members = mgr.FleetMembers(c);
            if (members.Count == 0)
            {
                GUILayout.Label("No satellites assigned yet - add this network's satellites to count them.", Theme.Warn);
            }
            else
            {
                int ok = 0;
                foreach (var m in members) if (m.Qualifies) ok++;
                GUILayout.Label($"Fleet: {ok}/{members.Count} qualifying", Theme.ItemSub);
                foreach (var m in members)
                {
                    GUILayout.BeginHorizontal();
                    string line = (m.Qualifies ? "✓  " : "✗  ") + m.Name + (m.Qualifies ? "" : $"  - {m.Reason}");
                    GUILayout.Label(line, m.Qualifies ? Theme.CondOk : Theme.CondBad);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("✕", Theme.FoldoutBtn, GUILayout.Width(26), GUILayout.Height(20)))
                        mgr.RemoveFleetVessel(c.Id, m.Vid);
                    GUILayout.EndHorizontal();
                }
            }
            if (inFlight && GUILayout.Button("Add current vessel", Theme.FoldoutBtn, GUILayout.Height(24)))
                mgr.AssignActiveVessel(c.Id);
        }

        /// <summary>Abort confirmation dialog drawn as its own CscUI window.</summary>
        public void DrawConfirm(ContractManager mgr, float width)
        {
            var c = mgr.Catalog.Get(PendingAbortId);
            GUILayout.Space(6);
            GUILayout.Label("Abort this mission?", Theme.Title);
            GUILayout.Label(c != null ? c.Titel : PendingAbortId, Theme.ItemSub);
            float pen = mgr.AbortPenalty(c);
            if (pen > 0f)
                GUILayout.Label($"Abort penalty: {pen:0} science points (half reward).", Theme.Warn);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes, abort", Theme.CloseBtn, GUILayout.Height(28)))
            { mgr.Abandon(PendingAbortId); PendingAbortId = null; }
            GUILayout.Space(8);
            if (GUILayout.Button("No", Theme.AcceptBtn, GUILayout.Height(28))) PendingAbortId = null;
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
            GUI.DragWindow(new Rect(0, 0, width, 24));
        }

        private static void DrawClose(float width, System.Action onClose)
        {
            if (GUI.Button(new Rect(width - 30, 4, 22, 22), "✕", Theme.CloseBtn)) onClose();
        }

        private void DrawClaimInfo(ContractManager mgr)
        {
            if (string.IsNullOrEmpty(mgr.LastClaimId)) return;
            if (Time.realtimeSinceStartup - mgr.LastClaimRealtime > 6f) return;
            GUILayout.Label($"+{mgr.LastClaimAmount:0} science received", Theme.ClaimInfo);
        }

        /// <summary>Draws a mission objective checklist.</summary>
        private static void DrawChecklist(ContractManager mgr, MissionContract c, bool ready)
        {
            for (int i = 0; i < c.Bedingungen.Count; i++)
            {
                var cond = c.Bedingungen[i];
                if (cond.Checks.Count > 0)
                {
                    for (int j = 0; j < cond.Checks.Count; j++)
                    {
                        bool met = mgr.IsCheckMet(c, i, j);
                        string label = CheckLabel(cond.Checks[j], mgr.Stations);
                        if (!ready && cond.Checks[j].IsTimer)
                            label += TimerProgress(mgr.CheckRemaining(c, i), cond.Checks[j]);
                        GUILayout.Label((met ? "✓  " : "✗  ") + label, met ? Theme.CondOk : Theme.CondBad);
                    }
                }
                else
                {
                    bool met = mgr.IsConditionMet(c, i);
                    GUILayout.Label((met ? "✓  " : "✗  ") + ConditionLabel(cond, mgr.Stations),
                        met ? Theme.CondOk : Theme.CondBad);
                }
            }
        }

        /// <summary>Description with the %station% placeholder resolved to a vessel name or generic text.</summary>
        public static string RenderDescription(MissionContract c, ContractManager mgr)
        {
            string d = c.Beschreibung ?? "";
            if (d.IndexOf("%station%", System.StringComparison.Ordinal) < 0) return d;
            string name = StationName(c, mgr);
            return d.Replace("%station%", name != null ? $"\"{name}\"" : "your station");
        }

        private static string StationName(MissionContract c, ContractManager mgr)
        {
            string key = !string.IsNullOrEmpty(c.RecordStationKey) ? c.RecordStationKey : c.StationRef;
            if (string.IsNullOrEmpty(key))
                foreach (var b in c.Bedingungen)
                {
                    if (!string.IsNullOrEmpty(b.StationKey)) { key = b.StationKey; break; }
                    foreach (var ck in b.Checks)
                        if (!string.IsNullOrEmpty(ck.StationKey)) { key = ck.StationKey; break; }
                    if (!string.IsNullOrEmpty(key)) break;
                }
            return string.IsNullOrEmpty(key) ? null : mgr.Stations.Name(key);
        }

        /// <summary>Timer progress, for example "  (3/15 days)" or "  (4/10 s)".</summary>
        private static string TimerProgress(double remSeconds, Check chk)
        {
            if (chk.Kind == CheckKind.DURATION)
            {
                double total = chk.Days;
                double done = remSeconds < 0 ? 0.0 : System.Math.Max(0.0, total - remSeconds / Conditions.VesselQuery.SecondsPerDay());
                return $"  ({done:0.0}/{total:0} days)";
            }
            double tot = chk.Seconds;
            double d = remSeconds < 0 ? 0.0 : System.Math.Max(0.0, tot - remSeconds);
            return $"  ({d:0}/{tot:0} s)";
        }

        /// <summary>Display text for a check: catalog label first, generated fallback otherwise.</summary>
        public static string CheckLabel(Check chk, StationRegistry stations)
        {
            if (!string.IsNullOrEmpty(chk.Label)) return chk.Label;
            string body = BodyVisual.DisplayName(chk.Body);
            switch (chk.Kind)
            {
                case CheckKind.CREW_MIN:   return $"Crewed with at least {chk.Min} Kerbal{(chk.Min == 1 ? "" : "s")} aboard";
                case CheckKind.CREW_NONE:  return "Uncrewed - no Kerbal aboard";
                case CheckKind.CREW_EXACT: return $"Exactly {chk.Min} Kerbal{(chk.Min == 1 ? "" : "s")} aboard";
                case CheckKind.CREW_CAPACITY_MIN: return $"Vessel has at least {chk.Min} crew seat{(chk.Min == 1 ? "" : "s")}";
                case CheckKind.ON_BODY:    return $"At target body {body}";
                case CheckKind.SITUATION:  return $"Situation: {SituationText(chk.Situation)}";
                case CheckKind.LANDED:     return $"Landed on {body}";
                case CheckKind.SUBORBITAL: return $"Suborbital spaceflight over {body}";
                case CheckKind.PERIAPSIS_MIN: return $"Periapsis above {chk.Km:0} km";
                case CheckKind.ORBIT_ABOVE: return chk.Km > 0
                    ? $"Stable orbit around {body}, periapsis above {chk.Km:0} km"
                    : $"Stable orbit around {body} above the atmosphere";
                case CheckKind.APOAPSIS_MAX: return $"Apoapsis below {chk.Km:0} km";
                case CheckKind.INCLINATION_MIN: return $"Orbital inclination at least {chk.InclinationMin:0} degrees";
                case CheckKind.ABOVE_ATMOSPHERE: return "Orbit entirely above the atmosphere";
                case CheckKind.SUBORBITAL_ABOVE_ATMO: return "Apoapsis above the atmosphere";
                case CheckKind.ATMO_FRACTION: return $"Within {chk.FracMin * 100:0}-{chk.FracMax * 100:0}% of atmosphere height";
                case CheckKind.ORE_PRESENT: return "Ore mined aboard";
                case CheckKind.ORE_SURFACE: return $"Mine Ore on the surface of {body}";
                case CheckKind.WHEEL_MOTION: return $"Drive a wheeled rover on {body} at {chk.SpeedMin:0.0} m/s or faster";
                case CheckKind.FLYBY:       return chk.Km > 0
                    ? $"Fly by {body} with closest approach below {chk.Km:0} km"
                    : $"Fly by {body}";
                case CheckKind.MARKER_LANDING: return $"Precision landing at {body} within {(chk.Km > 0 ? chk.Km : 15):0} km";
                case CheckKind.RETURN_FROM_BODY:
                    string home = BodyVisual.DisplayName(string.IsNullOrEmpty(chk.ReturnBody) ? "Kerbin" : chk.ReturnBody);
                    return string.Equals(chk.ReturnMode, "flyby", System.StringComparison.OrdinalIgnoreCase)
                        ? $"Fly by {body}, then return to {home}"
                        : string.Equals(chk.ReturnMode, "visit", System.StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(chk.ReturnMode, "home", System.StringComparison.OrdinalIgnoreCase)
                            ? $"Complete the visit, then return to {home}"
                        : $"Return from {body} to {home}";
                case CheckKind.FUEL_MIN:    return $"More than {chk.Amount:0} units of fuel aboard";
                case CheckKind.RESOURCE_MIN:return $"More than {chk.Amount:0} {chk.Resource} aboard";
                case CheckKind.EVA:         return "Kerbal on EVA";
                case CheckKind.DOCK_STATION:
                    string nm = stations?.Name(chk.StationKey);
                    return nm != null ? $"Docked to station \"{nm}\"" : "Docked to the target station";
                case CheckKind.DOCK_ANY:    return "Docking maneuver completed";
                case CheckKind.VESSEL_COUNT:return $"{chk.Count} satellites simultaneously in orbit around {body}";
                case CheckKind.VESSEL_COUNT_INCLINATION:
                    return $"{chk.Count} satellites simultaneously in orbit around {body}, inclination at least {chk.InclinationMin:0} degrees";
                case CheckKind.RELAY_VESSEL_COUNT:
                    return $"{chk.Count} relay satellites simultaneously in orbit around {body}";
                case CheckKind.RELAY_VESSEL_COUNT_INCLINATION:
                    return $"{chk.Count} relay satellites simultaneously in orbit around {body}, inclination at least {chk.InclinationMin:0} degrees";
                case CheckKind.HOLD:        return $"Hold state for {chk.Seconds:0} seconds";
                case CheckKind.DURATION:    return $"Hold continuously for {chk.Days:0} days";
                default:                    return chk.Kind.ToString();
            }
        }

        /// <summary>"Target: Station ..." line for missions aimed at a named station.</summary>
        public static string StationTarget(MissionContract c, ContractManager mgr)
        {
            if (!HasStationKey(c)) return null;
            string name = StationName(c, mgr);
            return name != null ? $"Target: Station \"{name}\"" : "Target: Station (not built yet)";
        }

        // A mission targets a station when it references it, not when it creates it.
        public static bool HasStationKey(MissionContract c)
        {
            if (!string.IsNullOrEmpty(c.StationRef)) return true;
            foreach (var b in c.Bedingungen)
            {
                if (!string.IsNullOrEmpty(b.StationKey)) return true;
                foreach (var ck in b.Checks) if (!string.IsNullOrEmpty(ck.StationKey)) return true;
            }
            return false;
        }

        private static string ProgressLine(MissionContract c)
        {
            var p = c.Progress; if (p == null) return null;
            if (TryD(p, "cd_remaining", out double secs) && secs > 0)
                return $"Time left: {secs / VesselQueryDays():0.0} days";
            if (TryD(p, "ml_dist", out double dist))
                return $"Distance to marker: {dist / 1000.0:0.0} km";
            if (p.GetValue("fb_seen") == "1" && TryD(p, "fb_bestApproach", out double app) && app < 1e29)
                return $"Closest approach: {System.Math.Max(0.0, app / 1000.0):0} km";
            return null;
        }

        private static double VesselQueryDays() => Conditions.VesselQuery.SecondsPerDay();
        private static bool TryD(ConfigNode n, string k, out double v) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out v);

        private static string SituationText(string sit)
        {
            switch ((sit ?? "").Trim().ToUpperInvariant())
            {
                case "ORBIT": case "ORBITING": return "in orbit";
                case "LANDED": case "SURFACE": return "landed";
                case "SPLASHED": return "splashed";
                case "SUBORBITAL": case "SUB_ORBITAL": return "suborbital";
                case "FLYING": return "flying in atmosphere";
                default: return "";
            }
        }

        private static string StationSuffix(Condition b, StationRegistry stations)
        {
            if (string.IsNullOrEmpty(b.StationKey)) return "";
            string name = stations?.Name(b.StationKey);
            return name != null ? $" to station \"{name}\"" : " to the station";
        }

        /// <summary>Readable description of a legacy condition.</summary>
        public static string ConditionLabel(Condition b, StationRegistry stations = null)
        {
            string body = BodyVisual.DisplayName(b.Body);
            string crew = b.MinCrew > 0 ? $", at least {b.MinCrew} crew" : "";
            string sit = SituationText(b.Situation);
            switch (b.Type)
            {
                case ConditionType.ORBIT:
                    return $"Reach stable orbit around {body}{crew}";
                case ConditionType.ORBIT_HIGH:
                    return $"High orbit around {body} (periapsis above {b.MinAltitudeKm:0} km){crew}";
                case ConditionType.LANDED:
                    return $"Land safely on {body}{crew}";
                case ConditionType.ALT_FRACTION_ATMO:
                    return $"Atmospheric flight over {body} at {b.MinFraction * 100:0}-{b.MaxFraction * 100:0}% of atmosphere height{crew}";
                case ConditionType.ABOVE_ATMO_SUBORBITAL:
                    return $"Suborbital above the atmosphere of {body}{crew}";
                case ConditionType.EVA:
                    return $"EVA at {body}{(sit != "" ? " (" + sit + ")" : "")}";
                case ConditionType.CREW_DURATION:
                    return $"Hold for {b.DurationDays:0} days with at least {Mathf.Max(1, b.MinCrew)} crew {(sit != "" ? sit + " " : "")}at {body}";
                case ConditionType.DOCK:
                    return $"Dock{StationSuffix(b, stations)}{(string.IsNullOrEmpty(b.Body) ? "" : $" in orbit around {body}")}";
                case ConditionType.ORE_SURFACE:
                    return $"Mine Ore on the surface of {body}";
                case ConditionType.VESSEL_COUNT_ORBIT:
                    return $"{b.VesselCount} satellites simultaneously in orbit around {body}{(b.MinAltitudeKm > 0 ? $" (periapsis above {b.MinAltitudeKm:0} km)" : "")}";
                case ConditionType.FUEL_ORBIT:
                    return $"Fueled depot (above {b.FuelThreshold:0} units) in orbit around {body}";
                case ConditionType.FLYBY:
                    return $"Fly by {body}{(b.FlybyAltitudeKm > 0 ? $" with closest approach below {b.FlybyAltitudeKm:0} km" : "")}";
                case ConditionType.MARKER_LANDING:
                    return $"Precision landing at {body} within {(b.RadiusKm > 0 ? b.RadiusKm : 15):0} km{crew}";
                case ConditionType.RENDEZVOUS:
                    return $"Rendezvous{StationSuffix(b, stations)} at {body} (below {(b.RendezvousKm > 0 ? b.RendezvousKm : 2):0} km)";
                default:
                    return b.Type + " " + body;
            }
        }
    }
}
