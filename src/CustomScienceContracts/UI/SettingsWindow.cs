using System.Linq;
using CustomScienceContracts.Core;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Settings window: science multiplier, unlock-all toggle and emergency mission skip.</summary>
    public class SettingsWindow
    {
        private Vector2 _scroll;
        // Two-step skip confirmation: first click arms the button for a few seconds.
        private string _pendingSkipId;
        private float _pendingSkipUntil;

        public void Draw(ContractManager mgr, float width, float height, System.Action onClose)
        {
            if (GUI.Button(new Rect(width - 30, 4, 22, 22), "✕", Theme.CloseBtn)) onClose();

            GUILayout.Space(4);
            GUILayout.Label("Settings", Theme.Title);
            GUILayout.Label($"{ModInfo.Name} v{ModInfo.Version}", Theme.ItemSub);

            // --- Science multiplier (x0.1 - x3.0, 0.1 steps) ---
            GUILayout.Space(8);
            GUILayout.Label($"Science multiplier:  x{mgr.ScienceMultiplier:0.0}", Theme.ItemTitle);
            float m = GUILayout.HorizontalSlider((float)mgr.ScienceMultiplier, 0.1f, 3.0f, GUILayout.Height(20));
            mgr.ScienceMultiplier = Mathf.Clamp(Mathf.Round(m * 10f) / 10f, 0.1f, 3.0f);
            GUILayout.Label("Applies to all future claimed rewards.", Theme.ItemSub);

            // --- Mission Control size ---
            GUILayout.Space(12);
            GUILayout.Label($"Mission Control size:  {Tuning.MissionCenterScale * 100f:0}%", Theme.ItemTitle);
            float size = GUILayout.HorizontalSlider(Tuning.MissionCenterScale, 0.55f, 1.0f, GUILayout.Height(20));
            Tuning.MissionCenterScale = Mathf.Clamp(Mathf.Round(size * 100f) / 100f, 0.55f, 1.0f);
            GUILayout.Label("You can also drag the lower-right corner of Mission Control.", Theme.ItemSub);

            // --- Unlock all ---
            GUILayout.Space(12);
            bool unlock = Tuning.UnlockAll;
            string lbl = unlock ? "All missions unlocked: ON" : "Unlock all missions: OFF";
            if (GUILayout.Button(lbl, unlock ? Theme.ClaimBtn : Theme.AcceptBtn, GUILayout.Height(30)))
            {
                Tuning.UnlockAll = !unlock;
                mgr.RecomputeAvailability();
            }
            GUILayout.Label("Ignores prerequisites and visibility limits.", Theme.ItemSub);

            // --- Skip active mission ---
            GUILayout.Space(12);
            GUILayout.Label("Skip Active Mission", Theme.ItemTitle);
            GUILayout.Label("Emergency exit for a broken or too difficult mission: counts as completed "
                          + "and unlocks follow-ups, but pays NO science points.", Theme.ItemSub);

            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Width(width - 8), GUILayout.Height(height - 330));
            var active = mgr.ActiveContracts().ToList();
            if (active.Count == 0)
                GUILayout.Label("No active missions.", Theme.Locked);
            if (_pendingSkipId != null && Time.realtimeSinceStartup > _pendingSkipUntil)
                _pendingSkipId = null;
            foreach (var c in active)
            {
                GUILayout.BeginHorizontal(Theme.ItemBox);
                GUILayout.Label(c.Titel, Theme.ItemSub);
                GUILayout.FlexibleSpace();
                bool armed = _pendingSkipId == c.Id;
                if (GUILayout.Button(armed ? "Really skip? No reward" : "Skip",
                        armed ? Theme.CloseBtn : Theme.SettingsBtn, GUILayout.Width(armed ? 196 : 132), GUILayout.Height(26)))
                {
                    if (armed)
                    {
                        mgr.Skip(c.Id);
                        _pendingSkipId = null;
                    }
                    else
                    {
                        _pendingSkipId = c.Id;
                        _pendingSkipUntil = Time.realtimeSinceStartup + 4f;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(3);
            }
            GUILayout.EndScrollView();

            GUI.DragWindow(new Rect(0, 0, width - 34, 24));
        }
    }
}
