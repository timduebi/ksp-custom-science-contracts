using System.Collections.Generic;
using System.Linq;
using CustomScienceContracts.Core;
using CustomScienceContracts.Data;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Mission center with two views. The Campaign Atlas shows one epoch at a time as
    /// branch/body rows with prerequisite arrows and doubles as the campaign timeline: repeatable
    /// missions stay visible there permanently once completed, marked green. The Repeatables view
    /// drops the epoch split entirely and lists the whole pool grouped by target body, with the
    /// cooldown progress always visible on each card.</summary>
    public class SelectionWindow
    {
        private enum CenterMode { Campaign, Repeatable }

        // Fixed display order: Stations sit below Robotic Explorers and above Lifelines.
        private static readonly Sparte[] Branches =
            { Sparte.Bemannt, Sparte.UnbemannteErkundung, Sparte.Stationen, Sparte.NetzwerkLogistik };

        private const float CardW = 280f;
        private const float CardBaseH = 104f;
        private const float CooldownStripH = 26f;   // extra card face height in the Repeatables view
        private const float CardGap = 64f;
        private const float RepCardGap = 20f;
        private const float BodyLabelW = 132f;
        private const float BranchHeaderH = 30f;
        private const float SectionHeaderH = 34f;
        private const float RowGap = 22f;
        private const float LaneGap = 14f;
        private const float SectionGap = 34f;
        private const float EpochTabH = 40f;
        private const float EpochTabGap = 6f;

        private static readonly Color ActiveOrange = new Color(0.95f, 0.65f, 0.20f);

        private CenterMode _mode = CenterMode.Campaign;
        private int _selectedEpoch = 1;
        private Vector2 _scroll;
        private readonly HashSet<string> _expandedCards = new HashSet<string>();
        private readonly Dictionary<string, Rect> _cardRects = new Dictionary<string, Rect>();

        // Per-catalog display cache (epoch names, epoch count, stock detection). The catalog is
        // immutable after load, so this is computed once instead of every OnGUI pass.
        private ContractCatalog _cachedCatalog;
        private string[] _epochNames;
        private int _maxEpoch = 1;

        /// <summary>Set by the gear button; CscUI reads it and toggles the settings window.</summary>
        public bool SettingsToggleRequested;

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            if (GUI.Button(new Rect(width - 30f, 4f, 22f, 22f), "✕", Theme.CloseBtn))
            {
                onClose();
                return;
            }
            DrawGear(new Rect(width - 58f, 5f, 22f, 22f));

            EnsureCatalogCache(mgr);
            HashSet<string> visible = VisibilityRules.ComputeVisible(mgr.Catalog);

            DrawModeTabs(mgr, visible, new Rect(14f, 30f, width - 86f, 32f));

            float mapTop = 70f;
            MapLayout layout;
            if (_mode == CenterMode.Campaign)
            {
                ComputeEpochStats(mgr, visible, out int[] acceptable, out int[] done, out int[] total);
                mapTop += DrawEpochTabs(mgr, acceptable, done, total, new Rect(14f, 70f, width - 28f, 0f)) + 8f;
                float introH = DrawEpochIntro(mgr, done, total, new Rect(14f, mapTop, width - 28f, 0f));
                if (introH > 0f) mapTop += introH + 8f;
                Rect viewport = new Rect(14f, mapTop, width - 28f, height - mapTop - 16f);
                layout = BuildCampaignLayout(mgr, visible, viewport.width);
                DrawMap(mgr, visible, viewport, layout);
            }
            else
            {
                Rect viewport = new Rect(14f, mapTop, width - 28f, height - mapTop - 16f);
                layout = BuildRepeatableLayout(mgr, visible, viewport.width);
                DrawMap(mgr, visible, viewport, layout);
            }

            GUI.DragWindow(new Rect(0f, 0f, width - 64f, 26f));
        }

        /// <summary>Selects the first epoch that has work (claimable, active or acceptable
        /// missions). Called when the window opens, so the player lands where the campaign is.</summary>
        public void FocusRelevantEpoch(ContractManager mgr)
        {
            EnsureCatalogCache(mgr);
            HashSet<string> visible = VisibilityRules.ComputeVisible(mgr.Catalog);
            for (int epoch = 1; epoch <= _maxEpoch; epoch++)
            {
                foreach (var c in mgr.Catalog.All)
                {
                    if (EpochOf(c) != epoch) continue;
                    if (c.Status == MissionStatus.Active || c.Status == MissionStatus.ReadyToClaim ||
                        CampaignAcceptable(mgr, c, visible))
                    {
                        _selectedEpoch = epoch;
                        return;
                    }
                }
            }
        }

        // --- Header ---

        private void DrawModeTabs(ContractManager mgr, HashSet<string> visible, Rect r)
        {
            float gap = 8f;
            float w = (r.width - gap) * 0.5f;
            int campaignCount = mgr.Catalog.All.Count(c => CampaignAcceptable(mgr, c, visible));
            int repeatableCount = mgr.Catalog.All.Count(c => RepeatableAcceptable(mgr, c));
            DrawModeTab(CenterMode.Campaign, $"Campaign Atlas ({campaignCount})", new Rect(r.x, r.y, w, r.height));
            DrawModeTab(CenterMode.Repeatable, $"Repeatables ({repeatableCount})", new Rect(r.x + w + gap, r.y, w, r.height));
        }

        private void DrawModeTab(CenterMode mode, string title, Rect r)
        {
            bool active = _mode == mode;
            if (GUI.Button(r, title, active ? Theme.TabActive : Theme.TabInactive) && _mode != mode)
            {
                _mode = mode;
                _scroll = Vector2.zero;
            }

            if (Event.current.type == EventType.Repaint)
            {
                Color col = mode == CenterMode.Campaign ? Theme.Accent : BodyVisual.ForSparte(Sparte.Wiederholbar).Color;
                Theme.DrawLeftAccent(r, col, mode == CenterMode.Campaign
                    ? IconLibrary.UI("TrackingStation_ButtonMapShips")
                    : BodyVisual.ForSparte(Sparte.Wiederholbar).Icon, 4f, 17f);
            }
        }

        /// <summary>Epoch tabs sized to the window: they wrap into extra rows instead of running
        /// off-screen, so every epoch stays reachable. Each tab shows the epoch name, the number of
        /// currently acceptable missions and a completion progress bar. Returns the height used.</summary>
        private float DrawEpochTabs(ContractManager mgr, int[] acceptable, int[] done, int[] total, Rect r)
        {
            const float minTabW = 128f;
            int perRow = Mathf.Clamp(Mathf.FloorToInt((r.width + EpochTabGap) / (minTabW + EpochTabGap)), 1, _maxEpoch);
            int rows = Mathf.CeilToInt(_maxEpoch / (float)perRow);
            float tabW = (r.width - (perRow - 1) * EpochTabGap) / perRow;

            for (int epoch = 1; epoch <= _maxEpoch; epoch++)
            {
                int idx = epoch - 1;
                Rect er = new Rect(
                    r.x + (idx % perRow) * (tabW + EpochTabGap),
                    r.y + (idx / perRow) * (EpochTabH + 4f),
                    tabW, EpochTabH);

                bool epochDone = total[epoch] > 0 && done[epoch] == total[epoch];
                string label = (epochDone ? "✓ " : "") + EpochName(epoch)
                             + (acceptable[epoch] > 0 ? $" ({acceptable[epoch]})" : "");
                if (GUI.Button(er, label, _selectedEpoch == epoch ? Theme.EpochTabActive : Theme.EpochTabInactive))
                    _selectedEpoch = epoch;

                if (Event.current.type == EventType.Repaint)
                {
                    // Completion progress along the bottom edge; accent-colored while in progress,
                    // green once the epoch is fully completed.
                    float frac = total[epoch] > 0 ? done[epoch] / (float)total[epoch] : 0f;
                    Rect bar = new Rect(er.x + 8f, er.yMax - 5f, er.width - 16f, 3f);
                    Theme.DrawRect(bar, new Color(1f, 1f, 1f, 0.08f));
                    if (frac > 0f)
                        Theme.DrawRect(new Rect(bar.x, bar.y, bar.width * frac, bar.height),
                            epochDone ? Theme.Ok : Theme.Accent);
                    if (_selectedEpoch == epoch)
                        Theme.DrawRect(new Rect(er.x + 8f, er.y + 2f, er.width - 16f, 2f), Theme.TextBright);
                }
            }
            return rows * EpochTabH + (rows - 1) * 4f;
        }

        private void ComputeEpochStats(ContractManager mgr, HashSet<string> visible,
            out int[] acceptable, out int[] done, out int[] total)
        {
            acceptable = new int[_maxEpoch + 1];
            done = new int[_maxEpoch + 1];
            total = new int[_maxEpoch + 1];
            foreach (var c in mgr.Catalog.All)
            {
                int epoch = EpochOf(c);
                if (epoch > _maxEpoch) continue;
                total[epoch]++;
                if (ContractManager.IsCompleted(c)) done[epoch]++;
                if (CampaignAcceptable(mgr, c, visible)) acceptable[epoch]++;
            }
        }

        /// <summary>Narrative intro panel for the selected epoch, fed by the catalog's EPOCH
        /// metadata: kicker line (epoch position + completion), large chapter title, story text
        /// and a completion bar. Returns the height used, or 0 without epoch metadata.</summary>
        private float DrawEpochIntro(ContractManager mgr, int[] done, int[] total, Rect r)
        {
            var info = mgr.Catalog.Epoch(_selectedEpoch);
            if (info == null || string.IsNullOrEmpty(info.Description)) return 0f;

            float textW = r.width - 40f;
            float dh = TextHeight(Theme.EpochIntroText, info.Description, textW);
            float h = 12f + 15f + 28f + dh + 15f;

            Rect panel = new Rect(r.x, r.y, r.width, h);
            GUI.Box(panel, GUIContent.none, Theme.EpochPanel);
            if (Event.current.type == EventType.Repaint)
                Theme.DrawLeftAccent(panel, Theme.Accent, null, 5f);

            int doneCount = _selectedEpoch < done.Length ? done[_selectedEpoch] : 0;
            int totalCount = _selectedEpoch < total.Length ? total[_selectedEpoch] : 0;
            bool epochDone = totalCount > 0 && doneCount == totalCount;
            string kicker = totalCount > 0
                ? $"EPOCH {_selectedEpoch} OF {_maxEpoch}   ·   {doneCount} OF {totalCount} MISSIONS COMPLETED{(epochDone ? "  ✓" : "")}"
                : $"EPOCH {_selectedEpoch} OF {_maxEpoch}";
            GUI.Label(new Rect(r.x + 20f, r.y + 12f, textW, 14f), kicker, Theme.EpochKicker);
            GUI.Label(new Rect(r.x + 20f, r.y + 27f, textW, 26f), EpochName(_selectedEpoch), Theme.EpochTitle);
            GUI.Label(new Rect(r.x + 20f, r.y + 56f, textW, dh), info.Description, Theme.EpochIntroText);

            if (Event.current.type == EventType.Repaint)
            {
                // Completion progress along the bottom edge of the panel.
                Rect bar = new Rect(r.x + 20f, panel.yMax - 9f, r.width - 40f, 3f);
                Theme.DrawRect(bar, new Color(1f, 1f, 1f, 0.08f));
                float frac = totalCount > 0 ? doneCount / (float)totalCount : 0f;
                if (frac > 0f)
                    Theme.DrawRect(new Rect(bar.x, bar.y, bar.width * frac, bar.height),
                        epochDone ? Theme.Ok : Theme.Accent);
            }
            return h;
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

        // --- Acceptance rules per view ---

        /// <summary>Acceptable from the Campaign Atlas: pool repeatables are excluded — the atlas
        /// shows them as completed history and they are re-accepted from the Repeatables view.</summary>
        private static bool CampaignAcceptable(ContractManager mgr, MissionContract c, HashSet<string> visible)
        {
            if (c.IsRepeatableInPool) return false;
            return mgr.CanAccept(c) && (Tuning.UnlockAll || visible.Contains(c.Id));
        }

        /// <summary>Acceptable from the Repeatables view: pool membership plus manager rules
        /// (cooldown over, branch slot free).</summary>
        private static bool RepeatableAcceptable(ContractManager mgr, MissionContract c) =>
            c.IsRepeatableInPool && mgr.CanAccept(c);

        private bool CanAcceptHere(ContractManager mgr, MissionContract c, HashSet<string> visible) =>
            _mode == CenterMode.Campaign ? CampaignAcceptable(mgr, c, visible) : RepeatableAcceptable(mgr, c);

        // --- Layout: Campaign Atlas ---

        private MapLayout BuildCampaignLayout(ContractManager mgr, HashSet<string> visible, float viewportWidth)
        {
            _cardRects.Clear();
            var layout = new MapLayout { ContentWidth = Mathf.Max(viewportWidth - 16f, 900f), ContentHeight = 80f };
            var epochContracts = mgr.Catalog.All
                .Where(c => EpochOf(c) == _selectedEpoch)
                .ToList();
            var columns = ComputeDependencyColumns(epochContracts);
            float y = 12f;

            foreach (var branch in Branches)
            {
                var branchContracts = epochContracts
                    .Where(c => c.HeimatSparte == branch)
                    .OrderBy(c => BodyRank(BodyVisual.PrimaryBody(c)))
                    .ThenBy(c => ColumnOf(c, columns))
                    .ThenBy(c => mgr.Catalog.IndexOf(c))
                    .ToList();
                if (branchContracts.Count == 0) continue;

                Rect header = new Rect(12f, y, layout.ContentWidth - 24f, BranchHeaderH);
                layout.BranchHeaders.Add(new BranchHeader(branch, header, branchContracts));
                y += BranchHeaderH + 8f;

                foreach (var group in branchContracts.GroupBy(c => BodyVisual.PrimaryBody(c)).OrderBy(g => BodyRank(g.Key)))
                {
                    var rowContracts = group
                        .OrderBy(c => ColumnOf(c, columns))
                        .ThenBy(c => mgr.Catalog.IndexOf(c))
                        .ToList();

                    // Cards that share a dependency column stack into vertical lanes, so optional
                    // parallel missions do not look like a sequential chain.
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
                layout.EmptyText = "No campaign missions in this epoch.";

            layout.ContentHeight = Mathf.Max(y + 20f, 220f);
            foreach (var i in Enumerable.Range(0, layout.BranchHeaders.Count))
            {
                var h = layout.BranchHeaders[i];
                h.Rect.width = layout.ContentWidth - 24f;
                layout.BranchHeaders[i] = h;
            }
            return layout;
        }

        // --- Layout: Repeatables grouped by body ---

        /// <summary>One flat page for the whole repeatable pool: a section per target body in
        /// atlas order, cards flowing left-to-right inside each section. No epochs, no arrows.</summary>
        private MapLayout BuildRepeatableLayout(ContractManager mgr, HashSet<string> visible, float viewportWidth)
        {
            _cardRects.Clear();
            var layout = new MapLayout { ContentWidth = Mathf.Max(viewportWidth - 16f, 620f) };

            var pool = mgr.RepeatablePool()
                .OrderBy(c => BodyRank(BodyVisual.PrimaryBody(c)))
                .ThenBy(c => mgr.Catalog.IndexOf(c))
                .ToList();

            if (pool.Count == 0)
            {
                layout.EmptyText = "No repeatable missions unlocked yet.\n"
                                 + "Complete a repeatable mission (marked ↻ in the Campaign Atlas) once to add it to this pool.";
                layout.ContentHeight = 220f;
                return layout;
            }

            float cw = layout.ContentWidth;
            int cols = Mathf.Max(1, Mathf.FloorToInt((cw - 32f + RepCardGap) / (CardW + RepCardGap)));
            float y = 12f;

            foreach (var group in pool.GroupBy(c => BodyVisual.PrimaryBody(c)))
            {
                var cards = group.ToList();
                int ready = cards.Count(c => RepeatableAcceptable(mgr, c));
                layout.BodySections.Add(new BodySection(group.Key, new Rect(12f, y, cw - 24f, SectionHeaderH), ready, cards.Count));
                y += SectionHeaderH + 10f;

                int col = 0;
                float rowMaxH = 0f;
                foreach (var c in cards)
                {
                    float h = CardHeight(mgr, c, visible);
                    if (col == cols)
                    {
                        col = 0;
                        y += rowMaxH + 14f;
                        rowMaxH = 0f;
                    }
                    Rect card = new Rect(16f + col * (CardW + RepCardGap), y, CardW, h);
                    layout.Cards.Add(new CardEntry(c, card));
                    _cardRects[c.Id] = card;
                    rowMaxH = Mathf.Max(rowMaxH, h);
                    col++;
                }
                y += rowMaxH + SectionGap;
            }

            layout.ContentHeight = Mathf.Max(y + 20f, 220f);
            return layout;
        }

        // --- Shared drawing ---

        private void DrawMap(ContractManager mgr, HashSet<string> visible, Rect viewport, MapLayout layout)
        {
            _scroll = GUI.BeginScrollView(viewport, _scroll,
                new Rect(0f, 0f, layout.ContentWidth, layout.ContentHeight), true, true);

            if (!string.IsNullOrEmpty(layout.EmptyText))
            {
                GUI.Label(new Rect(18f, 16f, 560f, 48f), layout.EmptyText, Theme.Locked);
                GUI.EndScrollView();
                return;
            }

            DrawBranchHeaders(mgr, visible, layout);
            DrawBodyRows(layout);
            DrawBodySections(layout);
            if (_mode == CenterMode.Campaign)
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

                int ready = h.Contracts.Count(c => CampaignAcceptable(mgr, c, visible));
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

        /// <summary>Body section headers of the Repeatables view: icon, name and how many of the
        /// section's missions can be accepted right now.</summary>
        private void DrawBodySections(MapLayout layout)
        {
            foreach (var s in layout.BodySections)
            {
                GUI.Box(s.Rect, GUIContent.none, Theme.EpochPanel);
                var bv = BodyVisual.ForBody(s.Body);
                if (Event.current.type == EventType.Repaint)
                {
                    Theme.DrawLeftAccent(s.Rect, bv.Color, null, 5f);
                    if (bv.Icon != null)
                    {
                        var prev = GUI.color;
                        GUI.color = Color.white;
                        GUI.DrawTexture(new Rect(s.Rect.x + 14f, s.Rect.y + 6f, 22f, 22f), bv.Icon, ScaleMode.ScaleToFit, true);
                        GUI.color = prev;
                    }
                }
                GUI.Label(new Rect(s.Rect.x + 42f, s.Rect.y + 6f, 320f, 22f),
                    BodyVisual.DisplayName(s.Body), Theme.ItemTitle);
                GUI.Label(new Rect(s.Rect.xMax - 190f, s.Rect.y + 8f, 176f, 20f),
                    $"{s.Ready} of {s.Total} ready", Theme.SectionCount);
            }
        }

        private float FaceHeight() =>
            _mode == CenterMode.Repeatable ? CardBaseH + CooldownStripH : CardBaseH;

        private void DrawMissionCard(ContractManager mgr, HashSet<string> visible, MissionContract c, Rect r)
        {
            bool canAccept = CanAcceptHere(mgr, c, visible);
            bool lockedForDetails = IsLockedForDetails(c, canAccept);
            bool expanded = _expandedCards.Contains(c.Id);
            float faceH = FaceHeight();

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
                else
                {
                    if (missionIcon != null)
                    {
                        var prev = GUI.color;
                        GUI.color = Color.Lerp(StatusColor(c, canAccept), Color.white, 0.35f);
                        GUI.DrawTexture(new Rect(r.xMax - 28f, r.y + 8f, 18f, 18f), missionIcon, ScaleMode.ScaleToFit, true);
                        GUI.color = prev;
                    }
                    // Repeat glyph so repeatable missions are recognizable at a glance.
                    if (c.Repeatable)
                        GUI.Label(new Rect(r.xMax - 48f, r.y + 7f, 18f, 18f), "↻", Theme.RepeatBadge);
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
            if (_mode == CenterMode.Repeatable)
                DrawCooldownStrip(mgr, c, r, canAccept);
            if (expanded)
                DrawMissionDetails(mgr, visible, c, r, canAccept, faceH);
        }

        /// <summary>Always-visible cooldown/status strip on repeatable cards: shows why a mission
        /// is not available yet and how far the refresh has progressed.</summary>
        private void DrawCooldownStrip(ContractManager mgr, MissionContract c, Rect r, bool canAccept)
        {
            float x = r.x + 12f;
            float w = r.width - 24f;
            int cooldown = Mathf.Max(1, Tuning.RepeatableCooldown);
            int cd = mgr.RemainingCooldown(c);

            string text;
            GUIStyle style;
            Color barColor;
            float frac;
            if (c.Status == MissionStatus.ReadyToClaim)
            {
                text = "Ready to claim"; style = Theme.CondOk; barColor = Theme.ClaimGreen; frac = 1f;
            }
            else if (c.Status == MissionStatus.Active)
            {
                text = "Active — tracked in Active Missions"; style = Theme.CardMeta; barColor = ActiveOrange; frac = 1f;
            }
            else if (cd > 0)
            {
                int doneCount = cooldown - cd;
                text = $"Available after {cd} more mission{(cd == 1 ? "" : "s")}  ({doneCount}/{cooldown})";
                style = Theme.CardMeta; barColor = Theme.Accent; frac = doneCount / (float)cooldown;
            }
            else if (canAccept)
            {
                text = $"Ready — completed {c.TotalCompletions}×"; style = Theme.CondOk; barColor = Theme.Ok; frac = 1f;
            }
            else
            {
                // Short form; the expanded status line names the exact branch.
                text = "Waiting for a free mission slot";
                style = Theme.CardMeta; barColor = new Color(0.55f, 0.58f, 0.64f); frac = 1f;
            }

            GUI.Label(new Rect(x, r.y + CardBaseH - 3f, w, 18f), text, style);
            if (Event.current.type == EventType.Repaint)
            {
                Rect bar = new Rect(x, r.y + CardBaseH + 17f, w, 4f);
                Theme.DrawRect(bar, new Color(1f, 1f, 1f, 0.10f));
                if (frac > 0f)
                    Theme.DrawRect(new Rect(bar.x, bar.y, bar.width * frac, bar.height), barColor);
            }
        }

        private void DrawCrossEpochTag(ContractManager mgr, MissionContract c, Rect r)
        {
            if (_mode != CenterMode.Campaign) return;
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
            string body = BodyVisual.PrimaryBody(c);
            var bv = BodyVisual.ForBody(body);
            Rect icon = new Rect(r.x + 12f, r.y + 76f, 18f, 18f);
            if (bv.Icon != null)
            {
                var prev = GUI.color;
                GUI.color = Color.white;
                GUI.DrawTexture(icon, bv.Icon, ScaleMode.ScaleToFit, true);
                GUI.color = prev;
            }
            GUI.Label(new Rect(r.x + 34f, r.y + 75f, r.width - 100f, 20f), BodyVisual.DisplayName(body), Theme.CardMeta);

            var sci = IconLibrary.UI("science symbol");
            var prevColor = GUI.color;
            GUI.color = Theme.Accent;
            if (sci != null)
                GUI.DrawTexture(new Rect(r.xMax - 58f, r.y + 78f, 13f, 13f), sci, ScaleMode.ScaleToFit, true);
            GUI.Label(new Rect(r.xMax - 42f, r.y + 74f, 38f, 20f),
                $"{c.ScienceReward * (float)mgr.ScienceMultiplier:0}", Theme.CardMeta);
            GUI.color = prevColor;
        }

        private void DrawMissionDetails(ContractManager mgr, HashSet<string> visible, MissionContract c,
            Rect card, bool canAccept, float faceH)
        {
            float x = card.x + 10f;
            float y = card.y + faceH + 8f;
            float w = card.width - 20f;

            // Status lines can wrap (e.g. cooldown texts), so their height is measured too.
            string status = StatusText(mgr, visible, c, canAccept);
            float statusH = TextHeight(Theme.CardMeta, status, w);
            GUI.Label(new Rect(x, y, w, statusH), status, Theme.CardMeta);
            y += statusH + 4f;

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

            GUI.Label(new Rect(x, y, w, 18f), "Objectives:", Theme.Station);
            y += 20f;
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
                y += h + 3f;
            }
            return y + 6f;
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
                Rect box = new Rect(x, y, w, h + 12f);
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawRect(box, new Color(0.20f, 0.10f, 0.11f, 0.58f));
                GUI.Label(new Rect(x + 7f, y + 4f, w - 14f, h), text, Theme.CondBad);
                y += h + 16f;
            }
            return y + 6f;
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
                    lines.Add($"Complete {cooldown} more mission{(cooldown == 1 ? "" : "s")} to refresh this repeatable.");
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
            if (_mode != CenterMode.Campaign) return y;
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
                .ThenBy(next => mgr.Catalog.IndexOf(next))
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

        // --- Dependency arrows (Campaign Atlas only) ---

        private void DrawConnections(ContractManager mgr, List<CardEntry> cards)
        {
            if (Event.current.type != EventType.Repaint) return;
            var ids = new HashSet<string>(cards.Select(e => e.Contract.Id));
            var sourceSlots = BuildSourceConnectionSlots(cards, ids);
            var sourceCounts = BuildSourceConnectionCounts(cards, ids);
            foreach (var entry in cards)
            {
                Rect to = entry.Rect;
                var prereqs = entry.Contract.Voraussetzungen
                    .Where(id => ids.Contains(id) && _cardRects.ContainsKey(id))
                    .OrderBy(id => _cardRects[id].y)
                    .ThenBy(id => _cardRects[id].x)
                    .ToList();

                for (int i = 0; i < prereqs.Count; i++)
                {
                    string preId = prereqs[i];
                    if (!_cardRects.TryGetValue(preId, out Rect from)) continue;
                    var pre = mgr.Catalog.Get(preId);
                    bool done = ContractManager.IsCompleted(pre);
                    Color col = done ? new Color(0.42f, 0.86f, 0.50f, 0.34f)
                                     : new Color(0.72f, 0.78f, 0.86f, 0.25f);
                    string key = EdgeKey(preId, entry.Contract.Id);
                    int sourceSlot = sourceSlots.TryGetValue(key, out int slot) ? slot : 0;
                    int sourceCount = sourceCounts.TryGetValue(preId, out int count) ? count : 1;
                    DrawConnection(from, to, col, sourceSlot, sourceCount, i, prereqs.Count);
                }
            }
        }

        private static Dictionary<string, int> BuildSourceConnectionSlots(List<CardEntry> cards, HashSet<string> visibleIds)
        {
            var slots = new Dictionary<string, int>();
            foreach (string sourceId in visibleIds)
            {
                var targets = cards
                    .Where(e => e.Contract.Voraussetzungen.Contains(sourceId))
                    .OrderBy(e => e.Rect.y)
                    .ThenBy(e => e.Rect.x)
                    .ToList();
                for (int i = 0; i < targets.Count; i++)
                    slots[EdgeKey(sourceId, targets[i].Contract.Id)] = i;
            }
            return slots;
        }

        private static Dictionary<string, int> BuildSourceConnectionCounts(List<CardEntry> cards, HashSet<string> visibleIds)
        {
            var counts = new Dictionary<string, int>();
            foreach (string sourceId in visibleIds)
                counts[sourceId] = cards.Count(e => e.Contract.Voraussetzungen.Contains(sourceId));
            return counts;
        }

        private static string EdgeKey(string sourceId, string targetId) => sourceId + "->" + targetId;

        private static void DrawConnection(Rect from, Rect to, Color color,
            int sourceSlot, int sourceCount, int targetSlot, int targetCount)
        {
            float x1 = from.xMax;
            float y1 = ConnectionAnchorY(from, sourceSlot, sourceCount);
            float x2 = to.x;
            float y2 = ConnectionAnchorY(to, targetSlot, targetCount);
            float mid;
            if (x2 > x1)
            {
                float gap = x2 - x1;
                float bend = Mathf.Clamp(Mathf.Abs(y2 - y1) * 0.08f, 0f, 34f);
                mid = x1 + gap * 0.52f + (y2 >= y1 ? bend : -bend);
                mid = Mathf.Clamp(mid, x1 + 24f, x2 - 24f);
            }
            else
            {
                mid = x1 + 58f;
            }
            float endX = x2 - 8f;

            Vector2 p0 = new Vector2(x1, y1);
            Vector2 p1 = new Vector2(mid, y1);
            Vector2 p2 = new Vector2(mid, y2);
            Vector2 p3 = new Vector2(endX, y2);

            DrawBezier(p0, p1, p2, p3, color, 2.2f);
            DrawArrowHead(new Vector2(x2 - 6f, y2), (p3 - p2).normalized, color);
        }

        private static float ConnectionAnchorY(Rect r, int slot, int count)
        {
            if (count <= 1) return r.y + CardBaseH * 0.5f;
            float band = Mathf.Min(62f, CardBaseH - 34f);
            float top = r.y + (CardBaseH - band) * 0.5f;
            return top + band * (slot + 1f) / (count + 1f);
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

        // --- Card sizing (must stay in sync with the drawing code above) ---

        private float CardHeight(ContractManager mgr, MissionContract c, HashSet<string> visible)
        {
            float faceH = FaceHeight();
            if (!_expandedCards.Contains(c.Id)) return faceH;

            bool canAccept = CanAcceptHere(mgr, c, visible);
            float h = faceH + 8f;   // details top padding
            h += TextHeight(Theme.CardMeta, StatusText(mgr, visible, c, canAccept), CardW - 20f) + 4f;
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
                    h += TextHeight(Theme.CondBad, "- " + title + " (" + epoch + ")", CardW - 20f) + 3f;
                }
                h += 6f;
            }

            if (c.Status == MissionStatus.ReadyToClaim || canAccept)
                h += 31f;

            h += CrossEpochUnlocksHeight(mgr, c, CardW - 20f);

            h += 20f;   // "Objectives:" header
            h += ObjectiveHeight(mgr, c, CardW - 20f);
            return h + 8f;
        }

        private float CrossEpochUnlocksHeight(ContractManager mgr, MissionContract c, float width)
        {
            if (_mode != CenterMode.Campaign) return 0f;
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
                h += TextHeight(Theme.CondBad, "- " + line, width - 14f) + 16f;
            return h + 6f;
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
            // IMGUI's CalcHeight and the actual render pass can disagree by one wrap point
            // (rounding/kerning), which clipped the last line of descriptions, requirement lists
            // and unlock hints. Measure slightly narrow AND reserve a full extra line whenever
            // the text wraps at all — spare air never clips, a missing line always does.
            float h = style.CalcHeight(new GUIContent(text), width - 8f);
            float singleLine = style.CalcHeight(new GUIContent("X"), width);
            if (h > singleLine + 1f) h += style.lineHeight;
            return Mathf.Max(18f, h);
        }

        // --- Card state presentation ---

        private GUIStyle CardStyle(MissionContract c, bool canAccept)
        {
            if (c.Status == MissionStatus.ReadyToClaim) return Theme.CardReady;
            // Timeline history: completed repeatables stay green in the Campaign Atlas even while
            // they cycle through the Repeatables pool again.
            if (_mode == CenterMode.Campaign && c.IsRepeatableInPool) return Theme.CardCompleted;
            if (c.Status == MissionStatus.Active) return Theme.CardActive;
            if (canAccept) return Theme.CardAvailable;
            // Repeatables view: a pool mission that cannot be accepted is cooling down or blocked
            // by the branch slot limit — render it dimmed, not green, so "waiting" and "ready"
            // are distinguishable at a glance.
            if (_mode == CenterMode.Repeatable && c.IsRepeatableInPool) return Theme.CardLocked;
            if (ContractManager.IsCompleted(c)) return Theme.CardCompleted;
            return Theme.CardLocked;
        }

        private Color StatusColor(MissionContract c, bool canAccept)
        {
            if (c.Status == MissionStatus.ReadyToClaim) return Theme.ClaimGreen;
            if (_mode == CenterMode.Campaign && c.IsRepeatableInPool) return Theme.Ok;
            if (c.Status == MissionStatus.Active) return ActiveOrange;
            if (canAccept) return Theme.Accent;
            if (_mode == CenterMode.Repeatable && c.IsRepeatableInPool)
                return new Color(0.48f, 0.50f, 0.56f);
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

        private string StatusText(ContractManager mgr, HashSet<string> visible, MissionContract c, bool canAccept)
        {
            if (c.Status == MissionStatus.ReadyToClaim) return "Ready to claim";

            if (c.IsRepeatableInPool)
            {
                string times = $"completed {c.TotalCompletions}×";
                if (_mode == CenterMode.Campaign)
                {
                    if (c.Status == MissionStatus.Active) return $"Active (repeatable, {times})";
                    int cd = mgr.RemainingCooldown(c);
                    return cd > 0
                        ? $"Completed {c.TotalCompletions}× — repeatable again after {cd} more mission{(cd == 1 ? "" : "s")}"
                        : $"Completed {c.TotalCompletions}× — accept again from the Repeatables tab";
                }
                if (c.Status == MissionStatus.Active) return "Active";
                int cooldown = mgr.RemainingCooldown(c);
                if (cooldown > 0) return $"On cooldown — available after {cooldown} more mission{(cooldown == 1 ? "" : "s")}";
                return canAccept
                    ? "Ready — can be accepted again"
                    : $"Waiting for a free {SparteDisplay.Name(c.HeimatSparte)} slot";
            }

            if (c.Status == MissionStatus.Active) return "Active";
            if (ContractManager.IsCompleted(c))
            {
                string date = FormatUT(c.FirstCompletedUT);
                return date != null ? $"Completed — {date}" : "Completed";
            }
            if (canAccept) return "Available";
            if (c.Status == MissionStatus.Available && !visible.Contains(c.Id))
                return "Prerequisites met, waiting behind the branch visibility limit";
            return "Locked";
        }

        /// <summary>In-game date of a UT, e.g. "Y2, D113", or null when unknown.</summary>
        private static string FormatUT(double ut)
        {
            if (ut < 0.0) return null;
            try
            {
                return KSPUtil.PrintDateCompact(ut, false);
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        // --- Ordering helpers ---

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

        // --- Epoch metadata (cached per catalog) ---

        private void EnsureCatalogCache(ContractManager mgr)
        {
            if (ReferenceEquals(_cachedCatalog, mgr.Catalog) && _epochNames != null) return;
            _cachedCatalog = mgr.Catalog;

            _maxEpoch = 1;
            foreach (var c in mgr.Catalog.All)
                _maxEpoch = Mathf.Max(_maxEpoch, EpochOf(c));

            bool stock = mgr.Catalog.All.Any(UsesStockBody);
            _epochNames = new string[_maxEpoch + 1];
            for (int epoch = 1; epoch <= _maxEpoch; epoch++)
            {
                // Name priority: EPOCH metadata node, per-contract epochName, built-in fallback.
                string catalogName = mgr.Catalog.Epoch(epoch)?.Name;
                if (string.IsNullOrEmpty(catalogName))
                    catalogName = mgr.Catalog.All
                        .Where(c => EpochOf(c) == epoch && !string.IsNullOrEmpty(c.EpochTitle))
                        .Select(c => c.EpochTitle)
                        .FirstOrDefault();
                _epochNames[epoch] = !string.IsNullOrEmpty(catalogName)
                    ? catalogName
                    : (stock ? StockEpochName(epoch) : SolEpochName(epoch));
            }

            _selectedEpoch = Mathf.Clamp(_selectedEpoch, 1, _maxEpoch);
        }

        private string EpochName(int epoch) =>
            _epochNames != null && epoch >= 1 && epoch < _epochNames.Length ? _epochNames[epoch] : "Campaign";

        private static int EpochOf(MissionContract c) => Mathf.Max(1, c.Epoch);

        private static string SolEpochName(int epoch)
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

        private static string StockEpochName(int epoch)
        {
            switch (epoch)
            {
                case 1: return "First Sparks";
                case 2: return "Orbital Habits";
                case 3: return "Mun or Bust";
                case 4: return "Minty Operations";
                case 5: return "Inner Mischief";
                case 6: return "Red Dust";
                case 7: return "Deep-Space Lifeline";
                case 8: return "Jool Frontier";
                case 9: return "The Purple Finale";
                default: return "Campaign";
            }
        }

        private static bool UsesStockBody(MissionContract c)
        {
            switch (BodyVisual.PrimaryBody(c))
            {
                case "Kerbin":
                case "Mun":
                case "Minmus":
                case "Moho":
                case "Eve":
                case "Gilly":
                case "Duna":
                case "Ike":
                case "Dres":
                case "Jool":
                case "Laythe":
                case "Vall":
                case "Tylo":
                case "Bop":
                case "Pol":
                case "Eeloo":
                    return true;
                default:
                    return false;
            }
        }

        private static int BodyRank(string body)
        {
            switch (BodyVisual.DisplayName(body))
            {
                case "Kerbin": return 5;
                case "Mun": return 15;
                case "Minmus": return 18;
                case "Earth": return 10;
                case "Moon": return 20;
                case "Eve": return 28;
                case "Gilly": return 29;
                case "Venus": return 30;
                case "Moho": return 33;
                case "Mercury": return 35;
                case "Sun": return 40;
                case "Duna": return 45;
                case "Ike": return 46;
                case "Mars": return 50;
                case "Phobos": return 52;
                case "Deimos": return 54;
                case "Dres": return 58;
                case "Eros": return 60;
                case "Vesta": return 62;
                case "Ceres": return 64;
                case "Pallas": return 66;
                case "Psyche": return 68;
                case "Ryugu": return 70;
                case "Ida": return 72;
                case "Dactyl": return 74;
                case "Jool": return 78;
                case "Jupiter": return 80;
                case "Laythe": return 81;
                case "Io": return 82;
                case "Vall": return 83;
                case "Europa": return 84;
                case "Tylo": return 85;
                case "Ganymede": return 86;
                case "Bop": return 87;
                case "Callisto": return 88;
                case "Pol": return 89;
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
                case "Eeloo": return 176;
                case "Arrokoth": return 180;
                default: return 1000;
            }
        }

        private static void Toggle(HashSet<string> set, string key)
        {
            if (!set.Remove(key)) set.Add(key);
        }

        // --- Layout data ---

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

        /// <summary>Full-width body group header of the Repeatables view.</summary>
        private struct BodySection
        {
            public string Body;
            public Rect Rect;
            public int Ready;
            public int Total;

            public BodySection(string body, Rect rect, int ready, int total)
            {
                Body = body;
                Rect = rect;
                Ready = ready;
                Total = total;
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
            public readonly List<BodySection> BodySections = new List<BodySection>();
            public readonly List<CardEntry> Cards = new List<CardEntry>();
            public float ContentWidth;
            public float ContentHeight;
            public string EmptyText;
        }
    }
}
