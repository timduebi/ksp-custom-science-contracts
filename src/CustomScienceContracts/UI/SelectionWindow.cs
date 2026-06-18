using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Mission selection window: tabs -> expandable body groups -> moon subgroups -> missions.</summary>
    public class SelectionWindow
    {
        private static readonly Sparte[] Tabs =
            { Sparte.Bemannt, Sparte.UnbemannteErkundung, Sparte.NetzwerkLogistik, Sparte.Wiederholbar };

        private int _tab;
        private Vector2 _scroll;
        private readonly HashSet<string> _expanded = new HashSet<string>();

        /// <summary>Set by the gear button; CscUI reads it and toggles the settings window.</summary>
        public bool SettingsToggleRequested;

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            if (GUI.Button(new Rect(width - 30, 4, 22, 22), "✕", Theme.CloseBtn)) onClose();
            DrawGear(new Rect(width - 58, 5, 22, 22));

            GUILayout.BeginHorizontal();
            for (int i = 0; i < Tabs.Length; i++)
            {
                var sv = BodyVisual.ForSparte(Tabs[i]);
                if (GUILayout.Button(SparteDisplay.Name(Tabs[i]),
                        _tab == i ? Theme.TabActive : Theme.TabInactive, GUILayout.Height(30)))
                    _tab = i;
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawLeftAccent(GUILayoutUtility.GetLastRect(), sv.Color, sv.Icon, 4f, 17f);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(6);
            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Width(width - 8), GUILayout.Height(height - 84));

            if (Tabs[_tab] == Sparte.Wiederholbar) DrawRepeatable(mgr);
            else DrawSparte(mgr, Tabs[_tab]);

            GUILayout.EndScrollView();
            GUI.DragWindow(new Rect(8, 0, width - 70, 24));
        }

        /// <summary>Gear icon button in the top-right corner.</summary>
        private void DrawGear(Rect r)
        {
            var gear = IconLibrary.UI("settings");
            if (gear != null)
            {
                var prev = GUI.color;
                GUI.color = r.Contains(Event.current.mousePosition)
                    ? new Color(0.85f, 0.87f, 0.90f) : new Color(0.58f, 0.61f, 0.66f);
                GUI.DrawTexture(r, gear, ScaleMode.ScaleToFit, true);
                GUI.color = prev;
                if (GUI.Button(r, GUIContent.none, GUIStyle.none)) SettingsToggleRequested = true;
            }
            else if (GUI.Button(r, "⚙", Theme.SettingsBtn)) SettingsToggleRequested = true;
        }

        private void DrawSparte(ContractManager mgr, Sparte sparte)
        {
            var visible = VisibilityRules.ComputeVisible(mgr.Catalog);
            GUILayout.Label($"Active {ActiveLimits.ActiveCount(mgr.Catalog.All, sparte)}/{ActiveLimits.LimitFor(sparte)}",
                Theme.ItemSub);

            foreach (string sub in mgr.Catalog.Subcategories(sparte))
            {
                var inSub = mgr.Catalog.InSubcategory(sparte, sub).ToList();
                var shown = inSub.Where(c => visible.Contains(c.Id)).ToList();
                int locked = inSub.Count(c => c.Status == MissionStatus.Locked);
                var sv = BodyVisual.ForSubcategory(sub);

                string key = sparte + "/" + sub;
                bool open = _expanded.Contains(key);
                string head = $"{(open ? "▼" : "▶")}  {BodyVisual.DisplayName(sub)}   ({shown.Count} open, {locked} locked)";
                if (GUILayout.Button(head, Theme.GroupHeader, GUILayout.Height(28))) Toggle(key);
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawLeftAccent(GUILayoutUtility.GetLastRect(), sv.Color, sv.Icon);
                if (!open) continue;

                // Split direct planet contracts from moon subgroups.
                CelestialBody subBody = BodyResolver.Resolve(BodyVisual.SubcatBodyName(sub));
                var direct = new List<MissionContract>();
                var moonOrder = new List<string>();
                var moons = new Dictionary<string, List<MissionContract>>();
                foreach (var c in shown)
                {
                    string moon = MoonOf(c, subBody);
                    if (moon == null) { direct.Add(c); continue; }
                    if (!moons.TryGetValue(moon, out var list))
                    { list = new List<MissionContract>(); moons[moon] = list; moonOrder.Add(moon); }
                    list.Add(c);
                }

                if (shown.Count == 0)
                    GUILayout.Label("   - no visible contract -", Theme.Locked);

                foreach (var c in direct) DrawItem(mgr, c, false);

                foreach (string moon in moonOrder)
                {
                    var mv = BodyVisual.ForBody(moon);
                    string mkey = key + "/" + moon;
                    bool mopen = _expanded.Contains(mkey);
                    string mhead = $"{(mopen ? "▼" : "▶")}  {BodyVisual.DisplayName(moon)}   ({moons[moon].Count})";
                    if (GUILayout.Button(mhead, Theme.MoonHeader, GUILayout.Height(24))) Toggle(mkey);
                    if (Event.current.type == EventType.Repaint)
                        Theme.DrawLeftAccent(GUILayoutUtility.GetLastRect(), mv.Color, mv.Icon, 5f, 18f);
                    if (!mopen) continue;
                    foreach (var c in moons[moon]) DrawItem(mgr, c, true);
                }

                // Preview: next locked mission for this body.
                if (mgr.LockedPreviewActive)
                {
                    var next = inSub.FirstOrDefault(x => x.Status == MissionStatus.Locked &&
                                                         !visible.Contains(x.Id));
                    if (next != null) DrawLockedPreview(mgr, next);
                }
            }
        }

        private void DrawLockedPreview(ContractManager mgr, MissionContract c)
        {
            GUILayout.BeginVertical(Theme.ItemBox);
            GUILayout.Label("🔒 " + c.Titel, Theme.Locked);
            var unmet = mgr.UnmetPrerequisiteTitles(c);
            if (unmet.Count > 0)
                GUILayout.Label("Required first: " + string.Join(", ", unmet.ToArray()), Theme.CondBad);
            GUILayout.EndVertical();
            if (Event.current.type == EventType.Repaint)
                Theme.DrawLeftAccent(GUILayoutUtility.GetLastRect(),
                    new Color(0.5f, 0.5f, 0.55f), BodyVisual.MissionIcon(c), 5f, 20f);
            GUILayout.Space(3);
        }

        private void DrawRepeatable(ContractManager mgr)
        {
            var pool = mgr.RepeatablePool().ToList();
            if (pool.Count == 0)
            { GUILayout.Label("No repeatable missions unlocked yet.", Theme.Locked); return; }
            foreach (var c in pool) DrawItem(mgr, c, false, isPool: true);
        }

        private void DrawItem(ContractManager mgr, MissionContract c, bool indent, bool isPool = false)
        {
            GUILayout.BeginHorizontal();
            if (indent) GUILayout.Space(16);
            GUILayout.BeginVertical(Theme.ItemBox);

            GUILayout.BeginHorizontal();
            GUILayout.Label(c.Titel, Theme.ItemTitle);
            GUILayout.FlexibleSpace();
            ActiveMissionsWindow.DrawReward(c.ScienceReward, mgr);
            GUILayout.EndHorizontal();

            string desc = ActiveMissionsWindow.RenderDescription(c, mgr);
            if (!string.IsNullOrEmpty(desc))
                GUILayout.Label(desc, Theme.ItemDesc);

            string stn = ActiveMissionsWindow.StationTarget(c, mgr);
            if (stn != null) GUILayout.Label(stn, Theme.Station);

            // Objectives, one per line.
            if (c.Bedingungen.Count > 0)
            {
                GUILayout.Space(2);
                foreach (var b in c.Bedingungen)
                {
                    if (b.Checks.Count > 0)
                        foreach (var ck in b.Checks)
                            GUILayout.Label("•  " + ActiveMissionsWindow.CheckLabel(ck, mgr.Stations), Theme.CondNeutral);
                    else
                        GUILayout.Label("•  " + ActiveMissionsWindow.ConditionLabel(b, mgr.Stations), Theme.CondNeutral);
                }
            }

            GUILayout.BeginHorizontal();
            if (isPool)
            {
                int cd = mgr.RemainingCooldown(c);
                GUILayout.Label(cd > 0 ? $"Cooldown: {cd} mission(s) left" : $"ready - completed {c.TotalCompletions}x",
                    Theme.ItemSub);
            }
            GUILayout.FlexibleSpace();
            GUI.enabled = mgr.CanAccept(c);
            if (GUILayout.Button("Accept", Theme.AcceptBtn, GUILayout.Height(30), GUILayout.MinWidth(120))) mgr.Accept(c.Id);
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            // Left accent and mission icon over the row.
            if (Event.current.type == EventType.Repaint)
            {
                Rect r = GUILayoutUtility.GetLastRect();
                var bodyCol = BodyVisual.ForBody(PrimaryBody(c)).Color;
                Theme.DrawLeftAccent(r, bodyCol, BodyVisual.MissionIcon(c), 5f, 20f);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);
        }

        // --- Helpers ---
        private void Toggle(string key) { if (!_expanded.Remove(key)) _expanded.Add(key); }

        /// <summary>Moon name if the mission body is a moon of the subgroup body.</summary>
        private static string MoonOf(MissionContract c, CelestialBody subBody)
        {
            if (subBody == null) return null;
            var pb = BodyResolver.Resolve(PrimaryBody(c));
            if (pb == null || pb == subBody) return null;
            return pb.referenceBody == subBody ? pb.name : null;
        }

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
    }
}
