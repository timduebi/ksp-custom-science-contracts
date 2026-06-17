using System.Linq;
using CustomScienceContracts.Core;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Einstellungsfenster (Zahnrad in der Missionskontrolle): Wissenschafts-Multiplikator,
    /// "alle Missionen freischalten" und ein Notausgang zum Ueberspringen einer aktiven Mission
    /// (erledigt, aber ohne Punkte). Wird von CscUI als eigenes Fenster gezeichnet.</summary>
    public class SettingsWindow
    {
        private Vector2 _scroll;

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            if (GUI.Button(new Rect(width - 30, 4, 22, 22), "✕", Theme.CloseBtn)) onClose();

            GUILayout.Space(4);
            GUILayout.Label("Einstellungen", Theme.Title);

            // --- Wissenschafts-Multiplikator (x0.1 – x3.0, 0.1er-Schritte) ---
            GUILayout.Space(8);
            GUILayout.Label($"Wissenschafts-Multiplikator:  x{mgr.ScienceMultiplier:0.0}", Theme.ItemTitle);
            float m = GUILayout.HorizontalSlider((float)mgr.ScienceMultiplier, 0.1f, 3.0f, GUILayout.Height(20));
            mgr.ScienceMultiplier = Mathf.Clamp(Mathf.Round(m * 10f) / 10f, 0.1f, 3.0f);
            GUILayout.Label("Gilt auf alle künftig eingelösten Belohnungen.", Theme.ItemSub);

            // --- Alles freischalten ---
            GUILayout.Space(12);
            bool unlock = Tuning.UnlockAll;
            string lbl = unlock ? "Alle Missionen freigeschaltet: AN" : "Alle Missionen freischalten: AUS";
            if (GUILayout.Button(lbl, unlock ? Theme.ClaimBtn : Theme.AcceptBtn, GUILayout.Height(30)))
            {
                Tuning.UnlockAll = !unlock;
                mgr.RecomputeAvailability();
            }
            GUILayout.Label("Hebt alle Voraussetzungen und Sichtbarkeitslimits auf.", Theme.ItemSub);

            // --- Aktive Mission ueberspringen ---
            GUILayout.Space(12);
            GUILayout.Label("Aktive Mission überspringen", Theme.ItemTitle);
            GUILayout.Label("Notausgang bei kaputter/zu schwerer Mission: zählt als erledigt (schaltet "
                          + "Folgemissionen frei), zahlt aber KEINE Wissenschaftspunkte.", Theme.ItemSub);

            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Width(width - 8), GUILayout.Height(height - 270));
            var active = mgr.ActiveContracts().ToList();
            if (active.Count == 0)
                GUILayout.Label("Keine aktiven Missionen.", Theme.Locked);
            foreach (var c in active)
            {
                GUILayout.BeginHorizontal(Theme.ItemBox);
                GUILayout.Label(c.Titel, Theme.ItemSub);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Überspringen", Theme.SettingsBtn, GUILayout.Width(132), GUILayout.Height(26)))
                    mgr.Skip(c.Id);
                GUILayout.EndHorizontal();
                GUILayout.Space(3);
            }
            GUILayout.EndScrollView();

            GUI.DragWindow(new Rect(0, 0, width - 34, 24));
        }
    }
}
