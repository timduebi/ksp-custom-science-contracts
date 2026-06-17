using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Aktive Missionen: Bedingungsliste mit rot/gruen-Status, gruener Einloese-Haken sobald
    /// erfuellt, roter Abbruch-X (mit Bestaetigung) solange offen, transiente "+X"-Anzeige.</summary>
    public class ActiveMissionsWindow
    {
        private Vector2 _scroll;
        public string PendingAbortId;   // != null => Bestaetigungsdialog offen
        public Rect ConfirmRect = new Rect(0, 0, 340, 175);

        private static readonly Sparte[] Groups =
            { Sparte.Bemannt, Sparte.UnbemannteErkundung, Sparte.NetzwerkLogistik };
        private readonly HashSet<string> _collapsed = new HashSet<string>();

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            DrawClose(width, onClose);

            var active = mgr.ActiveContracts().ToList();
            GUILayout.Label($"Aktive Missionen: {active.Count}", Theme.Title);
            DrawClaimInfo(mgr);

            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Width(width - 8), GUILayout.Height(height - 96));
            if (active.Count == 0)
                GUILayout.Label("Du hast aktuell keine Mission angenommen.", Theme.Locked);

            // Nach Sparte gruppiert (gestapelt wie die Planeten), je Gruppe aufklappbar.
            foreach (var sparte in Groups)
            {
                var inSp = active.Where(c => c.HeimatSparte == sparte).ToList();
                if (inSp.Count == 0) continue;

                var sv = BodyVisual.ForSparte(sparte);
                string key = sparte.ToString();
                bool open = !_collapsed.Contains(key);
                string head = $"{(open ? "▼" : "▶")}  {SparteDisplay.Name(sparte)}   ({inSp.Count})";
                if (GUILayout.Button(head, Theme.GroupHeader, GUILayout.Height(28)))
                { if (!_collapsed.Remove(key)) _collapsed.Add(key); }
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawLeftAccent(GUILayoutUtility.GetLastRect(), sv.Color, sv.Icon);
                if (!open) continue;

                foreach (var c in inSp) DrawActiveItem(mgr, c);
                GUILayout.Space(4);
            }

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(0, 0, width - 34, 24));
        }

        /// <summary>Wissenschaftswert MIT Multiplikator + blaues Science-Symbol, rechtsbündig.
        /// Oeffentlich, damit auch das Auswahlfenster denselben Wert/Stil nutzt.</summary>
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

            // Titel + Wissenschafts-Belohnung
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

            // Teilziele — je eine Zeile in ihrer Farbe (gruen erfuellt / rot offen)
            GUILayout.Space(2);
            DrawChecklist(mgr, c, ready);
            string prog = ProgressLine(c);
            if (prog != null) GUILayout.Label(prog, Theme.Pill);

            GUILayout.EndVertical();
            Rect r = GUILayoutUtility.GetLastRect();
            // Linksbalken: planet-/mondfarbig (gruen sobald einlösbar), Mission-Icon.
            if (Event.current.type == EventType.Repaint)
                Theme.DrawLeftAccent(r, ready ? Theme.ClaimGreen : BodyVisual.ForBody(PrimaryBody(c)).Color,
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

        /// <summary>Bestaetigungs-Dialog (als eigenes Fenster von CscUI gezeichnet).</summary>
        public void DrawConfirm(ContractManager mgr, float width)
        {
            var c = mgr.Catalog.Get(PendingAbortId);
            GUILayout.Space(6);
            GUILayout.Label("Mission wirklich abbrechen?", Theme.Title);
            GUILayout.Label(c != null ? c.Titel : PendingAbortId, Theme.ItemSub);
            float pen = mgr.AbortPenalty(c);
            if (pen > 0f)
                GUILayout.Label($"⚠ Abbruch kostet dich {pen:0} Wissenschaftspunkte (halbe Belohnung).", Theme.Warn);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Ja, abbrechen", Theme.CloseBtn, GUILayout.Height(28)))
            { mgr.Abandon(PendingAbortId); PendingAbortId = null; }
            GUILayout.Space(8);
            if (GUILayout.Button("Nein", Theme.AcceptBtn, GUILayout.Height(28))) PendingAbortId = null;
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
            GUILayout.Label($"+{mgr.LastClaimAmount:0} Wissenschaft erhalten", Theme.ClaimInfo);
        }

        /// <summary>Zeichnet die Teilziel-Checkliste eines Auftrags (COMPOSITE: pro CHECK eine Zeile;
        /// sonst die Legacy-Bedingungen als je eine Zeile).</summary>
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

        /// <summary>Beschreibung mit ersetztem %station%-Platzhalter (echter Stationsname oder generisch).
        /// Achtung: KEINE geschweiften Klammern als Token nehmen — ConfigNode interpretiert { } als Node.</summary>
        public static string RenderDescription(MissionContract c, ContractManager mgr)
        {
            string d = c.Beschreibung ?? "";
            if (d.IndexOf("%station%", System.StringComparison.Ordinal) < 0) return d;
            string name = StationName(c, mgr);
            return d.Replace("%station%", name != null ? $"«{name}»" : "deiner Raumstation");
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

        /// <summary>Fortschrittszähler eines Timer-Checks, z. B. "  (3/15 Tage)" bzw. "  (4/10 s)".</summary>
        private static string TimerProgress(double remSeconds, Check chk)
        {
            if (chk.Kind == CheckKind.DURATION)
            {
                double total = chk.Days;
                double done = remSeconds < 0 ? 0.0 : System.Math.Max(0.0, total - remSeconds / Conditions.VesselQuery.SecondsPerDay());
                return $"  ({done:0.0}/{total:0} Tage)";
            }
            double tot = chk.Seconds;
            double d = remSeconds < 0 ? 0.0 : System.Math.Max(0.0, tot - remSeconds);
            return $"  ({d:0}/{tot:0} s)";
        }

        /// <summary>Anzeigetext eines Checks: handgeschriebenes Label, sonst generierter Fallback.</summary>
        public static string CheckLabel(Check chk, StationRegistry stations)
        {
            if (!string.IsNullOrEmpty(chk.Label)) return chk.Label;
            string body = BodyVisual.DisplayName(chk.Body);
            switch (chk.Kind)
            {
                case CheckKind.CREW_MIN:   return $"Bemannt mit mindestens {chk.Min} Kerbal{(chk.Min == 1 ? "" : "s")} an Bord";
                case CheckKind.CREW_NONE:  return "Unbemannt – kein Kerbal an Bord";
                case CheckKind.CREW_EXACT: return $"Genau {chk.Min} Kerbal{(chk.Min == 1 ? "" : "s")} an Bord";
                case CheckKind.ON_BODY:    return $"Beim Zielkörper {body}";
                case CheckKind.SITUATION:  return $"Zustand: {SituationText(chk.Situation)}";
                case CheckKind.LANDED:     return $"Auf {body} gelandet";
                case CheckKind.SUBORBITAL: return $"Suborbitaler Raumflug über {body}";
                case CheckKind.PERIAPSIS_MIN: return $"Periapsis oberhalb von {chk.Km:0} km";
                case CheckKind.ORBIT_ABOVE: return chk.Km > 0
                    ? $"Stabiler Orbit um {body}, Periapsis über {chk.Km:0} km"
                    : $"Stabiler Orbit um {body} oberhalb der Atmosphäre";
                case CheckKind.INCLINATION_MIN: return $"Orbit-Inklination mindestens {chk.InclinationMin:0} Grad";
                case CheckKind.ABOVE_ATMOSPHERE: return "Umlaufbahn vollständig oberhalb der Atmosphäre";
                case CheckKind.SUBORBITAL_ABOVE_ATMO: return "Scheitelpunkt oberhalb der Atmosphäre";
                case CheckKind.ATMO_FRACTION: return $"In {chk.FracMin * 100:0}–{chk.FracMax * 100:0} % der Atmosphärenhöhe";
                case CheckKind.ORE_PRESENT: return "Erz (Ore) an Bord gefördert";
                case CheckKind.ORE_SURFACE: return $"Erz (Ore) an der Oberfläche von {body} fördern";
                case CheckKind.FLYBY:       return chk.Km > 0
                    ? $"Vorbeiflug an {body} mit Annäherung unter {chk.Km:0} km"
                    : $"Vorbeiflug an {body}";
                case CheckKind.MARKER_LANDING: return $"Präzisionslandung bei {body} im Umkreis von {(chk.Km > 0 ? chk.Km : 15):0} km";
                case CheckKind.FUEL_MIN:    return $"Mehr als {chk.Amount:0} Einheiten Treibstoff an Bord";
                case CheckKind.RESOURCE_MIN:return $"Mehr als {chk.Amount:0} {chk.Resource} an Bord";
                case CheckKind.EVA:         return "Kerbal im Aussenbordeinsatz (EVA)";
                case CheckKind.DOCK_STATION:
                    string nm = stations?.Name(chk.StationKey);
                    return nm != null ? $"An Station «{nm}» angedockt" : "An der Zielstation angedockt";
                case CheckKind.DOCK_ANY:    return "Andockmanöver erfolgreich abgeschlossen";
                case CheckKind.VESSEL_COUNT:return $"{chk.Count} Satelliten gleichzeitig im Orbit um {body}";
                case CheckKind.VESSEL_COUNT_INCLINATION:
                    return $"{chk.Count} Satelliten gleichzeitig im Orbit um {body}, Inklination mindestens {chk.InclinationMin:0} Grad";
                case CheckKind.HOLD:        return $"Zustand {chk.Seconds:0} Sekunden stabil halten";
                case CheckKind.DURATION:    return $"{chk.Days:0} Tage ununterbrochen halten";
                default:                    return chk.Kind.ToString();
            }
        }

        /// <summary>"Ziel: Station …"-Zeile, wenn der Auftrag eine konkrete (benannte) Station ansteuert.</summary>
        public static string StationTarget(MissionContract c, ContractManager mgr)
        {
            if (!HasStationKey(c)) return null;
            string name = StationName(c, mgr);
            return name != null ? $"Ziel: Station «{name}»" : "Ziel: Station (noch nicht gebaut)";
        }

        // Eine Mission "zielt" auf eine Station (zeigt "Ziel: Station …"), wenn sie sie REFERENZIERT —
        // nicht beim Bau-Auftrag (recordStationKey), der die Station erst erschafft.
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
                return $"Restzeit: {secs / VesselQueryDays():0.0} Tage";
            if (TryD(p, "ml_dist", out double dist))
                return $"Distanz zum Marker: {dist / 1000.0:0.0} km";
            if (TryD(p, "fb_bestApproach", out double app) && app < 1e29)
                return $"Nächste Annäherung: {app / 1000.0:0} km";
            return null;
        }

        private static double VesselQueryDays() => Conditions.VesselQuery.SecondsPerDay();
        private static bool TryD(ConfigNode n, string k, out double v) =>
            double.TryParse(n.GetValue(k), NumberStyles.Float, CultureInfo.InvariantCulture, out v);

        private static string PrimaryBody(MissionContract c)
        {
            foreach (var b in c.Bedingungen)
            {
                if (!string.IsNullOrEmpty(b.Body)) return b.Body;
                foreach (var ck in b.Checks)
                    if (!string.IsNullOrEmpty(ck.Body)) return ck.Body;
            }
            return null;
        }

        private static string SituationText(string sit)
        {
            switch ((sit ?? "").Trim().ToUpperInvariant())
            {
                case "ORBIT": case "ORBITING": return "im Orbit";
                case "LANDED": case "SURFACE": return "gelandet";
                case "SPLASHED": return "gewassert";
                case "SUBORBITAL": case "SUB_ORBITAL": return "suborbital";
                case "FLYING": return "im Atmosphärenflug";
                default: return "";
            }
        }

        private static string StationSuffix(Condition b, StationRegistry stations)
        {
            if (string.IsNullOrEmpty(b.StationKey)) return "";
            string name = stations?.Name(b.StationKey);
            return name != null ? $" an Station «{name}»" : " an der Station";
        }

        /// <summary>Ausfuehrliche, gut lesbare Beschreibung einer Bedingung (eine Zeile in der Liste).</summary>
        public static string ConditionLabel(Condition b, StationRegistry stations = null)
        {
            string body = BodyVisual.DisplayName(b.Body);
            string crew = b.MinCrew > 0 ? $", mindestens {b.MinCrew} Crew" : "";
            string sit = SituationText(b.Situation);
            switch (b.Type)
            {
                case ConditionType.ORBIT:
                    return $"Stabilen Orbit um {body} erreichen{crew}";
                case ConditionType.ORBIT_HIGH:
                    return $"Hochorbit um {body} (Periapsis über {b.MinAltitudeKm:0} km){crew}";
                case ConditionType.LANDED:
                    return $"Sicher auf {body} landen{crew}";
                case ConditionType.ALT_FRACTION_ATMO:
                    return $"Atmosphärenflug über {body} in {b.MinFraction * 100:0}–{b.MaxFraction * 100:0} % der Atmosphärenhöhe{crew}";
                case ConditionType.ABOVE_ATMO_SUBORBITAL:
                    return $"Suborbital über die Atmosphäre von {body} hinaus{crew}";
                case ConditionType.EVA:
                    return $"Aussenbordeinsatz (EVA) bei {body}{(sit != "" ? " (" + sit + ")" : "")}";
                case ConditionType.CREW_DURATION:
                    return $"{b.DurationDays:0} Tage ununterbrochen mit mindestens {Mathf.Max(1, b.MinCrew)} Crew {(sit != "" ? sit + " " : "")}bei {body} halten";
                case ConditionType.DOCK:
                    return $"Andocken{StationSuffix(b, stations)}{(string.IsNullOrEmpty(b.Body) ? "" : $" im Orbit um {body}")}";
                case ConditionType.ORE_SURFACE:
                    return $"Erz (Ore) an der Oberfläche von {body} fördern";
                case ConditionType.VESSEL_COUNT_ORBIT:
                    return $"{b.VesselCount} Satelliten gleichzeitig im Orbit um {body}{(b.MinAltitudeKm > 0 ? $" (Periapsis über {b.MinAltitudeKm:0} km)" : "")}";
                case ConditionType.FUEL_ORBIT:
                    return $"Betanktes Treibstoffdepot (über {b.FuelThreshold:0} Einheiten) im Orbit um {body}";
                case ConditionType.FLYBY:
                    return $"Vorbeiflug an {body}{(b.FlybyAltitudeKm > 0 ? $" mit Annäherung unter {b.FlybyAltitudeKm:0} km" : "")}";
                case ConditionType.MARKER_LANDING:
                    return $"Präzisionslandung bei {body} im Umkreis von {(b.RadiusKm > 0 ? b.RadiusKm : 15):0} km{crew}";
                case ConditionType.RENDEZVOUS:
                    return $"Rendezvous{StationSuffix(b, stations)} bei {body} (unter {(b.RendezvousKm > 0 ? b.RendezvousKm : 2):0} km)";
                default:
                    return b.Type + " " + body;
            }
        }
    }
}
