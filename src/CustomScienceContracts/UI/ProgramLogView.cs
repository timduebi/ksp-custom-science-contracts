using System;
using CustomScienceContracts.Core;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Renders the append-only completion history independently from the campaign atlas.</summary>
    internal sealed class ProgramLogView
    {
        private Vector2 _scroll;

        public void Draw(ContractManager mgr, Rect viewport, Func<MissionContract, string> epochName)
        {
            var entries = mgr.CompletionLog.Entries;
            float contentW = Mathf.Max(viewport.width - 16f, 620f);
            float rowW = contentW - 24f;
            float contentH = Mathf.Max(26f + entries.Count * 56f, 220f);

            _scroll = GUI.BeginScrollView(viewport, _scroll,
                new Rect(0f, 0f, contentW, contentH), true, true);
            if (entries.Count == 0)
            {
                GUI.Label(new Rect(18f, 16f, 560f, 48f),
                    "No missions completed yet.\nYour program's story starts with the first claim.", Theme.Locked);
                GUI.EndScrollView();
                return;
            }

            float y = 12f;
            for (int i = 0; i < entries.Count; i++)
            {
                CompletionRecord entry = entries[i];
                MissionContract c = mgr.Catalog.Get(entry.MissionId);
                Rect row = new Rect(12f, y, rowW, 50f);
                GUI.Box(row, GUIContent.none, Theme.EpochPanel);
                if (Event.current.type == EventType.Repaint)
                    Theme.DrawLeftAccent(row,
                        c != null ? BodyVisual.ForSparte(c.HeimatSparte).Color : Theme.Ok, null, 5f);

                GUI.Label(new Rect(row.x + 14f, row.y + 8f, 92f, 18f),
                    FormatUT(entry.UniversalTime) ?? "—", Theme.EpochKicker);
                string title = c?.Titel ?? entry.MissionId;
                GUI.Label(new Rect(row.x + 110f, row.y + 6f, row.width - 420f, 20f), title, Theme.CardTitle);

                string details;
                if (entry.Imported) details = "Imported from a pre-0.7 save; payout was not stored.";
                else if (entry.Action == "skip") details = "Skipped without reward";
                else details = !string.IsNullOrEmpty(entry.VesselName) ? entry.VesselName : "Mission claimed";
                if (!string.IsNullOrEmpty(entry.Crew)) details += " · " + entry.Crew;
                GUI.Label(new Rect(row.x + 110f, row.y + 27f, row.width - 420f, 17f), details, Theme.ItemSub);

                GUI.Label(new Rect(row.xMax - 296f, row.y + 9f, 178f, 18f),
                    c != null ? epochName(c) : "Catalog entry unavailable", Theme.SectionCount);
                Color previous = GUI.color;
                GUI.color = entry.HasScience ? Theme.Accent : new Color(0.65f, 0.67f, 0.72f);
                GUI.Label(new Rect(row.xMax - 108f, row.y + 8f, 94f, 20f),
                    entry.HasScience ? $"+{entry.Science:0.##}" : "—", Theme.Pill);
                GUI.color = previous;
                y += 56f;
            }
            GUI.EndScrollView();
        }

        private static string FormatUT(double ut)
        {
            if (ut < 0.0 || double.IsNaN(ut) || double.IsInfinity(ut)) return null;
            int day = Math.Max(1, (int)Math.Floor(ut / 21600.0) + 1);
            int year = (day - 1) / 426 + 1;
            int dayOfYear = (day - 1) % 426 + 1;
            return $"Y{year}, D{dayOfYear}";
        }
    }
}
