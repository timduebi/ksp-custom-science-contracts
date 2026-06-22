using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Mission center: Campaign Atlas / Repeatables -> epoch page -> branch rows.</summary>
    public class SelectionWindow
    {
        private enum CenterMode { Campaign, Repeatable }

        private static readonly Sparte[] Branches =
            { Sparte.Bemannt, Sparte.UnbemannteErkundung, Sparte.NetzwerkLogistik };

        private const float CardW = 250f;
        private const float CardBaseH = 104f;
        private const float CardGap = 64f;
        private const float BodyLabelW = 132f;
        private const float BranchHeaderH = 30f;
        private const float RowGap = 22f;
        private const float LaneGap = 14f;
        private const float SectionGap = 34f;

        private CenterMode _mode = CenterMode.Campaign;
        private int _selectedEpoch = 1;
        private Vector2 _scroll;
        private readonly HashSet<string> _expandedCards = new HashSet<string>();
        private readonly HashSet<string> _expandedObjectives = new HashSet<string>();
        private readonly Dictionary<string, Rect> _cardRects = new Dictionary<string, Rect>();

        /// <summary>Set by the gear button; CscUI reads it and toggles the settings window.</summary>
        public bool SettingsToggleRequested;

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            if (GUI.Button(new Rect(width - 30f, 4f, 22f, 22f), "X", Theme.CloseBtn))
            {
                onClose();
                return;
            }
            DrawGear(new Rect(width - 58f, 5f, 22f, 22f));

            HashSet<string> visible = VisibilityRules.ComputeVisible(mgr.Catalog);

            DrawModeTabs(mgr, visible, new Rect(14f, 30f, width - 86f, 32f));
            DrawEpochTabs(mgr, visible, new Rect(14f, 70f, width - 28f, 46f));

            Rect viewport = new Rect(14f, 124f, width - 28f, height - 140f);
            var layout = BuildLayout(mgr, visible, viewport.width);
            DrawMap(mgr, visible, viewport, layout);
        }

        private void DrawModeTabs(ContractManager mgr, HashSet<string> visible, Rect r)
        {
            float gap = 8f;
            float w = (r.width - gap) * 0.5f;
            DrawModeTab(mgr, visible, CenterMode.Campaign, "Campaign Atlas", new Rect(r.x, r.y, w, r.height));
            DrawModeTab(mgr, visible, CenterMode.Repeatable, "Repeatables", new Rect(r.x + w + gap, r.y, w, r.height));
        }

        private void DrawModeTab(ContractManager mgr, HashSet<string> visible, CenterMode mode, string title, Rect r)
        {
            int count = ContractsForMode(mgr, mode).Count(c => CanAcceptFromCenter(mgr, c, visible));
            bool active = _mode == mode;
            if (GUI.Button(r, $"{title} ({count})", active ? Theme.TabActive : Theme.TabInactive))
            {
                _mode = mode;
                if (_selectedEpoch < 1 || _selectedEpoch > 9) _selectedEpoch = 1;
            }

            if (Event.current.type == EventType.Repaint)
            {
                Color col = mode == CenterMode.Campaign ? Theme.Accent : BodyVisual.ForSparte(Sparte.Wiederholbar).Color;
                Theme.DrawLeftAccent(r, col, mode == CenterMode.Campaign
                    ? IconLibrary.UI("TrackingStation_ButtonMapShips")
                    : BodyVisual.ForSparte(Sparte.Wiederholbar).Icon, 4f, 17f);
            }
        }

        private void DrawEpochTabs(ContractManager mgr, HashSet<string> visible, Rect r)
        {
            float tabW = Mathf.Max(148f, r.width / 9f);
            for (int epoch = 1; epoch <= 9; epoch++)
            {
                Rect er = new Rect(r.x + (epoch - 1) * tabW, r.y, tabW - 6f, r.height);
                if (er.x > r.xMax) break;
                int count = ContractsForMode(mgr, _mode)
                    .Count(c => EpochOf(c) == epoch && CanAcceptFromCenter(mgr, c, visible));
                bool active = _selectedEpoch == epoch;
                if (GUI.Button(er, $"{EpochName(epoch)} ({count})",
                        active ? Theme.EpochTabActive : Theme.EpochTabInactive))
                    _selectedEpoch = epoch;

                if (active && Event.current.type == EventType.Repaint)
                    Theme.DrawRect(new Rect(er.x + 10f, er.yMax - 4f, er.width - 20f, 3f), Theme.TextBright);
            }
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
            else if (GUI.Button(r, "S", Theme.SettingsBtn)) SettingsToggleRequested = true;
        }

        private MapLayout BuildLayout(ContractManager mgr, HashSet<string> visible, float viewportWidth)
        {
            _cardRects.Clear();
            var layout = new MapLayout { ContentWidth = Mathf.Max(viewportWidth - 16f, 900f), ContentHeight = 80f };
            var epochContracts = ContractsForMode(mgr, _mode)
                .Where(c => EpochOf(c) == _selectedEpoch)
                .ToList();
            var columns = ComputeDisplayColumns(mgr, epochContracts);
            float y = 12f;

            foreach (var branch in Branches)
            {
                var branchContracts = epochContracts
                    .Where(c => c.HeimatSparte == branch)
                    .OrderBy(c => BodyRank(PrimaryBody(c)))
                    .ThenBy(c => ColumnOf(c, columns))
                    .ThenBy(c => CatalogIndex(mgr, c))
                    .ToList();
                if (branchContracts.Count == 0) continue;

                Rect header = new Rect(12f, y, layout.ContentWidth - 24f, BranchHeaderH);
                layout.BranchHeaders.Add(new BranchHeader(branch, header, branchContracts));
                y += BranchHeaderH + 8f;

                foreach (var group in branchContracts.GroupBy(c => PrimaryBody(c)).OrderBy(g => BodyRank(g.Key)))
                {
                    var rowContracts = group
                        .OrderBy(c => ColumnOf(c, columns))
                        .ThenBy(c => CatalogIndex(mgr, c))
                        .ToList();

                    var laneColumns = new List<HashSet<int>>();
                    var pending = new List<PendingCard>();
                    foreach (var c in rowContracts)
                    {
                        int column = ColumnOf(c, columns);
                        int lane = 0;
                        while (lane < laneColumns.Count && laneColumns[lane].Contains(column))
                            lane++;
                        if (lane == laneColumns.Count)
                            laneColumns.Add(new HashSet<int>());
                        laneColumns[lane].Add(column);
                        pending.Add(new PendingCard(c, column, lane, CardHeight(mgr, c, visible)));
                    }

                    var laneHeights = new float[Mathf.Max(1, laneColumns.Count)];
                    foreach (var p in pending)
                        laneHeights[p.Lane] = Mathf.Max(laneHeights[p.Lane], p.Height);

                    var laneOffsets = new float[laneHeights.Length];
                    float rowH = 0f;
                    for (int i = 0; i < laneHeights.Length; i++)
                    {
                        laneOffsets[i] = rowH;
                        rowH += Mathf.Max(CardBaseH, laneHeights[i]);
                        if (i < laneHeights.Length - 1) rowH += LaneGap;
                    }
                    rowH = Mathf.Max(CardBaseH, rowH);

                    layout.BodyRows.Add(new BodyRow(group.Key, new Rect(16f, y, BodyLabelW - 12f, rowH)));
                    foreach (var p in pending)
                    {
                        float x = 16f + BodyLabelW + p.Column * (CardW + CardGap);
                        Rect card = new Rect(x, y + laneOffsets[p.Lane], CardW, p.Height);
                        layout.Cards.Add(new CardEntry(p.Contract, card));
                        _cardRects[p.Contract.Id] = card;
                        layout.ContentWidth = Mathf.Max(layout.ContentWidth, card.xMax + 36f);
                    }
                    y += rowH + RowGap;
                }

                y += SectionGap;
            }

            if (layout.Cards.Count == 0)
                layout.EmptyText = _mode == CenterMode.Repeatable
                    ? "No repeatable missions in this epoch yet."
                    : "No campaign missions in this epoch.";

            layout.ContentHeight = Mathf.Max(y + 20f, 220f);
            foreach (var i in Enumerable.Range(0, layout.BranchHeaders.Count))
            {
                var h = layout.BranchHeaders[i];
                h.Rect.width = layout.ContentWidth - 24f;
                layout.BranchHeaders[i] = h;
            }
            return layout;
        }

        private static Dictionary<string, int> ComputeDisplayColumns(ContractManager mgr, List<MissionContract> contracts)
        {
            return ComputeDependencyColumns(contracts);
        }

        private static string LayoutRowKey(MissionContract c) =>
            ((int)c.HeimatSparte).ToString() + "|" + PrimaryBody(c);

        private static Dictionary<string, int> ComputeDependencyColumns(List<MissionContract> contracts)
        {
            var byId = contracts.ToDictionary(c => c.Id);
            var memo = new Dictionary<string, int>();
            var visiting = new HashSet<string>();
            foreach (var c in contracts)
                ResolveColumn(c, byId, memo, visiting);
            return memo;
        }

        private static int ResolveColumn(MissionContract c, Dictionary<string, MissionContract> byId,
            Dictionary<string, int> memo, HashSet<string> visiting)
        {
            if (c == null) return 0;
            if (memo.TryGetValue(c.Id, out int cached)) return cached;
            if (!visiting.Add(c.Id)) return 0;

            int column = 0;
            foreach (string preId in c.Voraussetzungen)
                if (byId.TryGetValue(preId, out var pre))
                    column = Mathf.Max(column, ResolveColumn(pre, byId, memo, visiting) + 1);

            visiting.Remove(c.Id);
            memo[c.Id] = column;
            return column;
        }

        private static int ColumnOf(MissionContract c, Dictionary<string, int> columns) =>
            c != null && columns.TryGetValue(c.Id, out int column) ? column : 0;

        private void DrawMap(ContractManager mgr, HashSet<string> visible, Rect viewport, MapLayout layout)
        {
            _scroll = GUI.BeginScrollView(viewport, _scroll,
                new Rect(0f, 0f, layout.ContentWidth, layout.ContentHeight), true, true);

            if (!string.IsNullOrEmpty(layout.EmptyText))
            {
                GUI.Label(new Rect(18f, 16f, 420f, 28f), layout.EmptyText, Theme.Locked);
                GUI.EndScrollView();
                return;
            }

            DrawBranchHeaders(mgr, visible, layout);
            DrawBodyRows(layout);
            DrawConnections(mgr, layout.Cards);
            foreach (var entry in layout.Cards)
                DrawMissionCard(mgr, visible, entry.Contract, entry.Rect);

            GUI.EndScrollView();
        }

        private void DrawBranchHeaders(ContractManager mgr, HashSet<string> visible, MapLayout layout)
        {
            foreach (var h in layout.BranchHeaders)
            {
                GUI.Box(h.Rect, GUIContent.none, Theme.EpochPanel);
                var sv = BodyVisual.ForSparte(h.Branch);
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawLeftAccent(h.Rect, sv.Color, sv.Icon, 5f, 19f);

                int ready = h.Contracts.Count(c => CanAcceptFromCenter(mgr, c, visible));
                GUI.Label(new Rect(h.Rect.x + 36f, h.Rect.y + 5f, 360f, 22f),
                    $"{SparteDisplay.Name(h.Branch)} ({ready})", Theme.ItemTitle);
                GUI.Label(new Rect(h.Rect.xMax - 120f, h.Rect.y + 7f, 100f, 20f),
                    $"{h.Contracts.Count} missions", Theme.ItemSub);
            }
        }

        private void DrawBodyRows(MapLayout layout)
        {
            foreach (var row in layout.BodyRows)
            {
                var bv = BodyVisual.ForBody(row.Body);
                Rect icon = new Rect(row.Rect.x, row.Rect.y + 4f, 22f, 22f);
                if (bv.Icon != null)
                {
                    var prev = GUI.color;
                    GUI.color = Color.white;
                    GUI.DrawTexture(icon, bv.Icon, ScaleMode.ScaleToFit, true);
                    GUI.color = prev;
                }
                GUI.Label(new Rect(row.Rect.x + 28f, row.Rect.y + 3f, row.Rect.width - 28f, 22f),
                    BodyVisual.DisplayName(row.Body), Theme.BodyRowLabel);
            }
        }

        private void DrawMissionCard(ContractManager mgr, HashSet<string> visible, MissionContract c, Rect r)
        {
            bool canAccept = CanAcceptFromCenter(mgr, c, visible);
            bool lockedForDetails = IsLockedForDetails(c, canAccept);
            bool expanded = _expandedCards.Contains(c.Id);

            GUI.Box(r, GUIContent.none, CardStyle(c, canAccept));
            if (Event.current.type == EventType.Repaint)
            {
                if (lockedForDetails)
                {
                    Theme.DrawRect(new Rect(r.x + 1f, r.y + 1f, r.width - 2f, r.height - 2f),
                        new Color(0f, 0f, 0f, 0.16f));
                    Theme.DrawRect(new Rect(r.x + 1f, r.yMax - 5f, r.width - 2f, 4f),
                        new Color(0.90f, 0.34f, 0.30f, 0.46f));
                }
                Theme.DrawRect(new Rect(r.x + 1f, r.y + 2f, 4f, r.height - 4f), StatusColor(c, canAccept));
                var missionIcon = BodyVisual.MissionIcon(c);
                if (lockedForDetails)
                {
                    GUI.Label(new Rect(r.xMax - 58f, r.y + 8f, 48f, 16f), "LOCKED", Theme.LockBadge);
                }
                else if (missionIcon != null)
                {
                    var prev = GUI.color;
                    GUI.color = Color.Lerp(StatusColor(c, canAccept), Color.white, 0.35f);
                    GUI.DrawTexture(new Rect(r.xMax - 28f, r.y + 8f, 18f, 18f), missionIcon, ScaleMode.ScaleToFit, true);
                    GUI.color = prev;
                }
                DrawCrossEpochTag(mgr, c, r);
            }

            Rect arrow = new Rect(r.x + 10f, r.y + 8f, 22f, 22f);
            if (GUI.Button(arrow, expanded ? "v" : ">", Theme.TopIconButton))
                Toggle(_expandedCards, c.Id);
            if (GUI.Button(new Rect(r.x + 34f, r.y + 6f, r.width - 116f, 58f), GUIContent.none, GUIStyle.none))
                Toggle(_expandedCards, c.Id);
            GUI.Label(new Rect(r.x + 36f, r.y + 8f, r.width - 118f, 58f), c.Titel, Theme.CardTitle);

            DrawPlanetLine(r, c, mgr);
            if (expanded)
                DrawMissionDetails(mgr, visible, c, r, canAccept);
        }

        private void DrawCrossEpochTag(ContractManager mgr, MissionContract c, Rect r)
        {
            var unlocks = CrossEpochUnlocks(mgr, c);
            if (unlocks.Count == 0) return;
            int firstEpoch = unlocks.Min(EpochOf);
            string label = "-> " + EpochName(firstEpoch);
            Rect tag = new Rect(r.xMax - 88f, r.y + 31f, 76f, 16f);
            Color tagColor = unlocks.All(IsCrossEpochUnlocked) ? Theme.Ok : Theme.Accent;
            Theme.DrawRect(tag, WithAlpha(tagColor, 0.24f));
            var prev = GUI.color;
            GUI.color = tagColor;
            GUI.Label(tag, label, Theme.UnlockTag);
            GUI.color = prev;
        }

        private void DrawPlanetLine(Rect r, MissionContract c, ContractManager mgr)
        {
            string body = PrimaryBody(c);
            var bv = BodyVisual.ForBody(body);
            Rect icon = new Rect(r.x + 12f, r.y + 76f, 18f, 18f);
            if (bv.Icon != null)
            {
                var prev = GUI.color;
                GUI.color = Color.white;
                GUI.DrawTexture(icon, bv.Icon, ScaleMode.ScaleToFit, true);
                GUI.color = prev;
            }
            GUI.Label(new Rect(r.x + 34f, r.y + 75f, 116f, 20f), BodyVisual.DisplayName(body), Theme.CardMeta);

            var sci = IconLibrary.UI("science symbol");
            var prevColor = GUI.color;
            GUI.color = Theme.Accent;
            if (sci != null)
                GUI.DrawTexture(new Rect(r.xMax - 58f, r.y + 78f, 13f, 13f), sci, ScaleMode.ScaleToFit, true);
            GUI.Label(new Rect(r.xMax - 42f, r.y + 74f, 38f, 20f),
                $"{c.ScienceReward * (float)mgr.ScienceMultiplier:0}", Theme.CardMeta);
            GUI.color = prevColor;
        }

        private void DrawMissionDetails(ContractManager mgr, HashSet<string> visible, MissionContract c, Rect card, bool canAccept)
        {
            float x = card.x + 10f;
            float y = card.y + CardBaseH + 8f;
            float w = card.width - 20f;

            GUI.Label(new Rect(x, y, w, 18f), StatusText(mgr, visible, c, canAccept), Theme.CardMeta);
            y += 20f;

            if (IsLockedForDetails(c, canAccept))
            {
                DrawUnlockRequirements(mgr, visible, c, x, y, w, canAccept);
                return;
            }

            string desc = ActiveMissionsWindow.RenderDescription(c, mgr);
            if (!string.IsNullOrEmpty(desc))
            {
                float dh = TextHeight(Theme.ItemDesc, desc, w);
                GUI.Label(new Rect(x, y, w, dh), desc, Theme.ItemDesc);
                y += dh + 4f;
            }

            string stn = ActiveMissionsWindow.StationTarget(c, mgr);
            if (stn != null)
            {
                GUI.Label(new Rect(x, y, w, 20f), stn, Theme.Station);
                y += 22f;
            }

            y = DrawMissingRequirements(mgr, c, x, y, w);
            y = DrawAction(mgr, c, x, y, w, canAccept);
            y = DrawCrossEpochUnlocks(mgr, c, x, y, w);

            bool objectivesOpen = _expandedObjectives.Contains(c.Id);
            if (GUI.Button(new Rect(x, y, w, 25f), (objectivesOpen ? "v " : "> ") + "Objectives", Theme.FoldoutBtn))
                Toggle(_expandedObjectives, c.Id);
            y += 29f;

            if (objectivesOpen)
                DrawObjectives(mgr, c, x, y, w);
        }

        private float DrawMissingRequirements(ContractManager mgr, MissionContract c, float x, float y, float w)
        {
            var unmet = c.Voraussetzungen.Select(id => mgr.Catalog.Get(id))
                .Where(pre => !ContractManager.IsCompleted(pre))
                .ToList();
            if (unmet.Count == 0) return y;

            GUI.Label(new Rect(x, y, w, 18f), "Requires:", Theme.Locked);
            y += 18f;
            foreach (var pre in unmet)
            {
                string title = pre != null ? pre.Titel : "Unknown mission";
                string epoch = pre != null ? EpochName(pre.Epoch) : "Unknown epoch";
                string line = "- " + title + " (" + epoch + ")";
                float h = TextHeight(Theme.CondBad, line, w);
                GUI.Label(new Rect(x, y, w, h), line, Theme.CondBad);
                y += h;
            }
            return y + 4f;
        }

        private float DrawUnlockRequirements(ContractManager mgr, HashSet<string> visible, MissionContract c,
            float x, float y, float w, bool canAccept)
        {
            GUI.Label(new Rect(x, y, w, 18f), "Unlock requirements:", Theme.UnlockHeader);
            y += 22f;

            foreach (string line in UnlockRequirementLines(mgr, visible, c, canAccept))
            {
                string text = "- " + line;
                float h = TextHeight(Theme.CondBad, text, w - 14f);
                Rect box = new Rect(x, y, w, h + 8f);
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawRect(box, new Color(0.20f, 0.10f, 0.11f, 0.58f));
                GUI.Label(new Rect(x + 7f, y + 4f, w - 14f, h), text, Theme.CondBad);
                y += h + 12f;
            }
            return y + 2f;
        }

        private List<string> UnlockRequirementLines(ContractManager mgr, HashSet<string> visible,
            MissionContract c, bool canAccept)
        {
            var lines = new List<string>();
            foreach (string id in c.Voraussetzungen)
            {
                var pre = mgr.Catalog.Get(id);
                if (ContractManager.IsCompleted(pre)) continue;
                string title = pre != null ? pre.Titel : id;
                string epoch = pre != null ? EpochName(pre.Epoch) : "Unknown epoch";
                lines.Add("Complete: " + title + " (" + epoch + ")");
            }

            if (lines.Count > 0) return lines;

            if (c.IsRepeatableInPool)
            {
                int cooldown = mgr.RemainingCooldown(c);
                if (cooldown > 0)
                    lines.Add($"Complete {cooldown} other mission(s) to refresh this repeatable.");
            }
            else if (c.Status == MissionStatus.Available && !visible.Contains(c.Id))
            {
                lines.Add("Waiting behind the branch visibility limit.");
            }

            if (lines.Count == 0 && c.Status == MissionStatus.Available && !canAccept)
                lines.Add($"Free an active {SparteDisplay.Name(c.HeimatSparte)} mission slot.");

            if (lines.Count == 0)
                lines.Add("Complete prerequisite missions first.");

            return lines;
        }

        private float DrawAction(ContractManager mgr, MissionContract c, float x, float y, float w, bool canAccept)
        {
            Rect btn = new Rect(x + w - 108f, y, 108f, 26f);
            if (c.Status == MissionStatus.ReadyToClaim)
            {
                if (GUI.Button(btn, "Claim", Theme.ClaimBtn))
                    mgr.Claim(c.Id);
                return y + 31f;
            }
            if (canAccept)
            {
                if (GUI.Button(btn, "Accept", Theme.AcceptBtn))
                    mgr.Accept(c.Id);
                return y + 31f;
            }
            return y;
        }

        private float DrawCrossEpochUnlocks(ContractManager mgr, MissionContract c, float x, float y, float w)
        {
            var unlocks = CrossEpochUnlocks(mgr, c);
            if (unlocks.Count == 0) return y;

            GUI.Label(new Rect(x, y, w, 18f), "Unlocks in another epoch:", Theme.Station);
            y += 21f;

            int shown = 0;
            foreach (var next in unlocks.Take(3))
            {
                string line = "- " + next.Titel + " (" + EpochName(next.Epoch) + ")";
                bool unlocked = IsCrossEpochUnlocked(next);
                var style = unlocked ? Theme.CondOk : Theme.Station;
                float h = TextHeight(style, line, w - 14f);
                Rect box = new Rect(x, y, w, h + 8f);
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawRect(box, unlocked
                        ? new Color(0.08f, 0.20f, 0.12f, 0.62f)
                        : new Color(0.08f, 0.17f, 0.24f, 0.62f));
                GUI.Label(new Rect(x + 7f, y + 4f, w - 14f, h), line, style);
                y += h + 10f;
                shown++;
            }
            if (unlocks.Count > shown)
            {
                string more = $"+ {unlocks.Count - shown} more";
                GUI.Label(new Rect(x + 7f, y, w - 14f, 18f), more, Theme.CardMeta);
                y += 20f;
            }
            return y + 2f;
        }

        private static bool IsCrossEpochUnlocked(MissionContract c)
        {
            return c != null && c.Status != MissionStatus.Locked;
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private List<MissionContract> CrossEpochUnlocks(ContractManager mgr, MissionContract c)
        {
            if (c == null) return new List<MissionContract>();
            return mgr.Catalog.All
                .Where(next => next.Voraussetzungen.Contains(c.Id) && EpochOf(next) != EpochOf(c))
                .OrderBy(EpochOf)
                .ThenBy(next => CatalogIndex(mgr, next))
                .ToList();
        }

        private void DrawObjectives(ContractManager mgr, MissionContract c, float x, float y, float w)
        {
            bool live = c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim;
            for (int i = 0; i < c.Bedingungen.Count; i++)
            {
                var cond = c.Bedingungen[i];
                if (cond.Checks.Count > 0)
                {
                    for (int j = 0; j < cond.Checks.Count; j++)
                    {
                        bool met = mgr.IsCheckMet(c, i, j);
                        string label = ActiveMissionsWindow.CheckLabel(cond.Checks[j], mgr.Stations);
                        string prefix = live && met ? "✓  " : "•  ";
                        var style = live ? (met ? Theme.CondOk : Theme.CondBad) : Theme.CondNeutral;
                        DrawObjectiveLine(prefix + label, style, x, ref y, w, live && met);
                    }
                }
                else
                {
                    bool met = mgr.IsConditionMet(c, i);
                    string label = ActiveMissionsWindow.ConditionLabel(cond, mgr.Stations);
                    string prefix = live && met ? "✓  " : "•  ";
                    var style = live ? (met ? Theme.CondOk : Theme.CondBad) : Theme.CondNeutral;
                    DrawObjectiveLine(prefix + label, style, x, ref y, w, live && met);
                }
            }
        }

        private static void DrawObjectiveLine(string text, GUIStyle style, float x, ref float y, float w, bool met)
        {
            float h = TextHeight(style, text, w - 16f);
            Rect box = new Rect(x, y, w, h + 8f);
            if (Event.current.type == EventType.Repaint)
            {
                Color bg = met
                    ? new Color(0.09f, 0.18f, 0.12f, 0.72f)
                    : new Color(0.12f, 0.15f, 0.21f, 0.78f);
                Theme.DrawRect(box, bg);
            }
            GUI.Label(new Rect(x + 8f, y + 4f, w - 16f, h), text, style);
            y += h + 12f;
        }

        private void DrawConnections(ContractManager mgr, List<CardEntry> cards)
        {
            if (Event.current.type != EventType.Repaint) return;
            var ids = new HashSet<string>(cards.Select(e => e.Contract.Id));
            foreach (var entry in cards)
            {
                Rect to = entry.Rect;
                foreach (string preId in entry.Contract.Voraussetzungen)
                {
                    if (!ids.Contains(preId) || !_cardRects.TryGetValue(preId, out Rect from)) continue;
                    var pre = mgr.Catalog.Get(preId);
                    bool done = ContractManager.IsCompleted(pre);
                    Color col = done ? new Color(0.42f, 0.86f, 0.50f, 0.34f)
                                     : new Color(0.72f, 0.78f, 0.86f, 0.25f);
                    DrawConnection(from, to, col);
                }
            }
        }

        private static void DrawConnection(Rect from, Rect to, Color color)
        {
            float x1 = from.xMax;
            float y1 = from.y + CardBaseH * 0.5f;
            float x2 = to.x;
            float y2 = to.y + CardBaseH * 0.5f;
            float mid = x2 > x1 ? (x1 + x2) * 0.5f : x1 + 42f;
            float endX = x2 - 8f;

            Vector2 p0 = new Vector2(x1, y1);
            Vector2 p1 = new Vector2(mid, y1);
            Vector2 p2 = new Vector2(mid, y2);
            Vector2 p3 = new Vector2(endX, y2);

            DrawBezier(p0, p1, p2, p3, color, 2.2f);
            DrawArrowHead(new Vector2(x2 - 6f, y2), (p3 - p2).normalized, color);
        }

        private static void DrawBezier(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float thickness)
        {
            Vector2 prev = p0;
            const int steps = 24;
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                Vector2 next = Cubic(p0, p1, p2, p3, t);
                DrawSegment(prev, next, color, thickness);
                prev = next;
            }
        }

        private static Vector2 Cubic(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            float u = 1f - t;
            return u * u * u * p0 + 3f * u * u * t * p1 + 3f * u * t * t * p2 + t * t * t * p3;
        }

        private static void DrawSegment(Vector2 a, Vector2 b, Color color, float thickness)
        {
            Vector2 d = b - a;
            float len = d.magnitude;
            if (len < 0.5f) return;

            Matrix4x4 old = GUI.matrix;
            float angle = Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg;
            GUIUtility.RotateAroundPivot(angle, a);
            Theme.DrawRect(new Rect(a.x, a.y - thickness * 0.5f, len, thickness), color);
            GUI.matrix = old;
        }

        private static void DrawArrowHead(Vector2 tip, Vector2 direction, Color color)
        {
            if (direction.sqrMagnitude < 0.01f) direction = Vector2.right;
            direction.Normalize();
            Vector2 back = -direction;
            Vector2 side = new Vector2(-direction.y, direction.x);
            DrawSegment(tip, tip + back * 10f + side * 5f, color, 2.2f);
            DrawSegment(tip, tip + back * 10f - side * 5f, color, 2.2f);
        }

        private float CardHeight(ContractManager mgr, MissionContract c, HashSet<string> visible)
        {
            if (!_expandedCards.Contains(c.Id)) return CardBaseH;

            bool canAccept = CanAcceptFromCenter(mgr, c, visible);
            float h = CardBaseH + 34f;
            if (IsLockedForDetails(c, canAccept))
                return h + UnlockRequirementsHeight(mgr, visible, c, canAccept, CardW - 20f) + 8f;

            string desc = ActiveMissionsWindow.RenderDescription(c, mgr);
            if (!string.IsNullOrEmpty(desc))
                h += TextHeight(Theme.ItemDesc, desc, CardW - 20f) + 4f;

            if (ActiveMissionsWindow.StationTarget(c, mgr) != null)
                h += 22f;

            var unmet = c.Voraussetzungen.Select(id => mgr.Catalog.Get(id))
                .Where(pre => !ContractManager.IsCompleted(pre))
                .ToList();
            if (unmet.Count > 0)
            {
                h += 18f;
                foreach (var pre in unmet)
                {
                    string title = pre != null ? pre.Titel : "Unknown mission";
                    string epoch = pre != null ? EpochName(pre.Epoch) : "Unknown epoch";
                    h += TextHeight(Theme.CondBad, "- " + title + " (" + epoch + ")", CardW - 20f);
                }
                h += 4f;
            }

            if (c.Status == MissionStatus.ReadyToClaim || canAccept)
                h += 31f;

            h += CrossEpochUnlocksHeight(mgr, c, CardW - 20f);

            h += 29f;
            if (_expandedObjectives.Contains(c.Id))
                h += ObjectiveHeight(mgr, c, CardW - 20f);
            return h + 8f;
        }

        private float CrossEpochUnlocksHeight(ContractManager mgr, MissionContract c, float width)
        {
            var unlocks = CrossEpochUnlocks(mgr, c);
            if (unlocks.Count == 0) return 0f;

            float h = 21f;
            int shown = 0;
            foreach (var next in unlocks.Take(3))
            {
                var style = IsCrossEpochUnlocked(next) ? Theme.CondOk : Theme.Station;
                h += TextHeight(style, "- " + next.Titel + " (" + EpochName(next.Epoch) + ")", width - 14f) + 10f;
                shown++;
            }
            if (unlocks.Count > shown) h += 20f;
            return h + 2f;
        }

        private float UnlockRequirementsHeight(ContractManager mgr, HashSet<string> visible,
            MissionContract c, bool canAccept, float width)
        {
            float h = 22f;
            foreach (string line in UnlockRequirementLines(mgr, visible, c, canAccept))
                h += TextHeight(Theme.CondBad, "- " + line, width - 14f) + 12f;
            return h + 2f;
        }

        private float ObjectiveHeight(ContractManager mgr, MissionContract c, float width)
        {
            float h = 0f;
            bool live = c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim;
            for (int i = 0; i < c.Bedingungen.Count; i++)
            {
                var cond = c.Bedingungen[i];
                if (cond.Checks.Count > 0)
                {
                    for (int j = 0; j < cond.Checks.Count; j++)
                    {
                        bool met = mgr.IsCheckMet(c, i, j);
                        var style = live ? (met ? Theme.CondOk : Theme.CondBad) : Theme.CondNeutral;
                        string prefix = live && met ? "✓  " : "•  ";
                        h += TextHeight(style, prefix + ActiveMissionsWindow.CheckLabel(cond.Checks[j], mgr.Stations), width - 16f) + 12f;
                    }
                }
                else
                {
                    bool met = mgr.IsConditionMet(c, i);
                    var style = live ? (met ? Theme.CondOk : Theme.CondBad) : Theme.CondNeutral;
                    string prefix = live && met ? "✓  " : "•  ";
                    h += TextHeight(style, prefix + ActiveMissionsWindow.ConditionLabel(cond, mgr.Stations), width - 16f) + 12f;
                }
            }
            return h;
        }

        private static float TextHeight(GUIStyle style, string text, float width)
        {
            return Mathf.Max(18f, style.CalcHeight(new GUIContent(text), width));
        }

        private static IEnumerable<MissionContract> ContractsForMode(ContractManager mgr, CenterMode mode)
        {
            if (mode == CenterMode.Repeatable)
                return mgr.RepeatablePool();
            return mgr.Catalog.All.Where(c => c.HeimatSparte != Sparte.Wiederholbar && !c.IsRepeatableInPool);
        }

        private static bool CanAcceptFromCenter(ContractManager mgr, MissionContract c, HashSet<string> visible)
        {
            if (!mgr.CanAccept(c)) return false;
            if (c.IsRepeatableInPool) return true;
            return Tuning.UnlockAll || visible.Contains(c.Id);
        }

        private static GUIStyle CardStyle(MissionContract c, bool canAccept)
        {
            if (c.Status == MissionStatus.ReadyToClaim) return Theme.CardReady;
            if (c.Status == MissionStatus.Active) return Theme.CardActive;
            if (canAccept) return Theme.CardAvailable;
            if (ContractManager.IsCompleted(c)) return Theme.CardCompleted;
            return Theme.CardLocked;
        }

        private static Color StatusColor(MissionContract c, bool canAccept)
        {
            if (c.Status == MissionStatus.ReadyToClaim) return Theme.ClaimGreen;
            if (c.Status == MissionStatus.Active) return new Color(0.95f, 0.65f, 0.20f);
            if (canAccept) return Theme.Accent;
            if (ContractManager.IsCompleted(c)) return Theme.Ok;
            return new Color(0.48f, 0.50f, 0.56f);
        }

        private static bool IsLockedForDetails(MissionContract c, bool canAccept)
        {
            if (c == null) return true;
            if (c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim) return false;
            if (ContractManager.IsCompleted(c)) return false;
            return !canAccept;
        }

        private static string StatusText(ContractManager mgr, HashSet<string> visible, MissionContract c, bool canAccept)
        {
            if (c.Status == MissionStatus.ReadyToClaim) return "Ready to claim";
            if (c.Status == MissionStatus.Active) return "Active";
            if (ContractManager.IsCompleted(c) && c.IsRepeatableInPool)
            {
                int cd = mgr.RemainingCooldown(c);
                return cd > 0 ? $"Repeatable cooldown: {cd} mission(s) left" : "Repeatable mission ready";
            }
            if (ContractManager.IsCompleted(c)) return "Completed";
            if (canAccept) return "Available";
            if (c.Status == MissionStatus.Available && !c.IsRepeatableInPool && !visible.Contains(c.Id))
                return "Prerequisites met, waiting behind the branch visibility limit";
            return "Locked";
        }

        private static int EpochOf(MissionContract c) => Mathf.Max(1, c.Epoch);

        private static string EpochName(int epoch)
        {
            switch (epoch)
            {
                case 1: return "Ignition";
                case 2: return "Moonrise";
                case 3: return "Orbital Roots";
                case 4: return "Inner Reach";
                case 5: return "Red Horizon";
                case 6: return "Beltworks";
                case 7: return "Jovian Gate";
                case 8: return "Ringed Worlds";
                case 9: return "Far Frontier";
                default: return "Campaign";
            }
        }

        private static string PrimaryBody(MissionContract c)
        {
            foreach (var b in c.Bedingungen)
            {
                if (!string.IsNullOrEmpty(b.Body)) return b.Body;
                foreach (var ck in b.Checks)
                    if (!string.IsNullOrEmpty(ck.Body)) return ck.Body;
            }
            return BodyVisual.SubcatBodyName(c.Unterkategorie) ?? c.Unterkategorie;
        }

        private static int BodyRank(string body)
        {
            switch (BodyVisual.DisplayName(body))
            {
                case "Earth": return 10;
                case "Moon": return 20;
                case "Venus": return 30;
                case "Mercury": return 35;
                case "Sun": return 40;
                case "Mars": return 50;
                case "Phobos": return 52;
                case "Deimos": return 54;
                case "Eros": return 60;
                case "Vesta": return 62;
                case "Ceres": return 64;
                case "Pallas": return 66;
                case "Psyche": return 68;
                case "Ryugu": return 70;
                case "Ida": return 72;
                case "Dactyl": return 74;
                case "Jupiter": return 80;
                case "Io": return 82;
                case "Europa": return 84;
                case "Ganymede": return 86;
                case "Callisto": return 88;
                case "Saturn": return 100;
                case "Titan": return 102;
                case "Enceladus": return 104;
                case "Rhea": return 106;
                case "Iapetus": return 108;
                case "Dione": return 110;
                case "Tethys": return 112;
                case "Mimas": return 114;
                case "Hyperion": return 116;
                case "Phoebe": return 118;
                case "Uranus": return 130;
                case "Titania": return 132;
                case "Oberon": return 134;
                case "Ariel": return 136;
                case "Umbriel": return 138;
                case "Miranda": return 140;
                case "Puck": return 142;
                case "Neptune": return 150;
                case "Triton": return 152;
                case "Nereid": return 154;
                case "Proteus": return 156;
                case "Pluto": return 170;
                case "Charon": return 172;
                case "Arrokoth": return 180;
                default: return 1000;
            }
        }

        private static int CatalogIndex(ContractManager mgr, MissionContract c)
        {
            for (int i = 0; i < mgr.Catalog.All.Count; i++)
                if (ReferenceEquals(mgr.Catalog.All[i], c)) return i;
            return int.MaxValue;
        }

        private static void Toggle(HashSet<string> set, string key)
        {
            if (!set.Remove(key)) set.Add(key);
        }

        private struct BranchHeader
        {
            public Sparte Branch;
            public Rect Rect;
            public List<MissionContract> Contracts;

            public BranchHeader(Sparte branch, Rect rect, List<MissionContract> contracts)
            {
                Branch = branch;
                Rect = rect;
                Contracts = contracts;
            }
        }

        private struct BodyRow
        {
            public string Body;
            public Rect Rect;

            public BodyRow(string body, Rect rect)
            {
                Body = body;
                Rect = rect;
            }
        }

        private struct CardEntry
        {
            public MissionContract Contract;
            public Rect Rect;

            public CardEntry(MissionContract contract, Rect rect)
            {
                Contract = contract;
                Rect = rect;
            }
        }

        private struct PendingCard
        {
            public MissionContract Contract;
            public int Column;
            public int Lane;
            public float Height;

            public PendingCard(MissionContract contract, int column, int lane, float height)
            {
                Contract = contract;
                Column = column;
                Lane = lane;
                Height = height;
            }
        }

        private class MapLayout
        {
            public readonly List<BranchHeader> BranchHeaders = new List<BranchHeader>();
            public readonly List<BodyRow> BodyRows = new List<BodyRow>();
            public readonly List<CardEntry> Cards = new List<CardEntry>();
            public float ContentWidth;
            public float ContentHeight;
            public string EmptyText;
        }
    }
}
