using CustomScienceContracts.Core;
using KSP.UI.Screens;
using ToolbarControl_NS;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Two toolbar buttons (active missions and mission control), registered through the
    /// vendored ToolbarControl library (see Vendor/ToolbarControl). This shows the buttons on
    /// Blizzy's Toolbar Continued when the player has it installed - freely repositionable, right
    /// next to any other mod's icon - and always falls back to the stock AppLauncher otherwise, so
    /// the mod works identically with or without the optional "ToolbarController" companion mod.</summary>
    public class CscUI : MonoBehaviour
    {
        private const string IconActive = "CustomScienceContracts/Icons/App/aktiv";
        private const string IconActiveAlert = "CustomScienceContracts/Icons/App/aktiv_alert";
        private const string IconAvail = "CustomScienceContracts/Icons/App/missionen";

        private ContractManager _mgr;
        private ToolbarControl _tcActive;
        private ToolbarControl _tcAvail;
        private bool _activeOpen, _availOpen;

        private readonly SelectionWindow _selection = new SelectionWindow();
        private readonly ActiveMissionsWindow _activeWin = new ActiveMissionsWindow();
        private readonly SettingsWindow _settings = new SettingsWindow();
        private bool _settingsOpen;

        private const float ActW = 480f, ActH = 620f, ConfW = 340f, ConfH = 178f;
        private const float SetW = 440f, SetH = 740f;
        private const float MinSelW = 760f, MinSelH = 520f;
        private Rect _selRect = new Rect(24, 24, 1200, 760);
        private Rect _actRect = new Rect(700, 80, ActW, ActH);
        private Rect _setRect = new Rect(180, 60, SetW, SetH);
        private bool _selRectInitialized;
        private bool _resizingSelection;   // Mission Control's own handle; Active Missions is fixed-size
        private Vector2 _resizeStartMouse;
        private Vector2 _resizeStartSize;
        private float _appliedMissionCenterScale = -1f;
        private bool _badgeOn;
        private float _nextBadgeCheck;

        /// <summary>Global window scale; IMGUI ignores KSP's UI scale, so the mod has its own.</summary>
        private static float UiScale => Mathf.Clamp(Tuning.UiScale, 0.8f, 1.6f);
        /// <summary>Screen size in scaled GUI coordinates.</summary>
        private static float VirtualW => Screen.width / UiScale;
        private static float VirtualH => Screen.height / UiScale;

        public void Bind(ContractManager mgr) => _mgr = mgr;

        private void Start()
        {
            var all = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT |
                      ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.TRACKSTATION |
                      ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH;
            var planning = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.TRACKSTATION |
                           ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH;

            _tcActive = gameObject.AddComponent<ToolbarControl>();
            _tcActive.AddToAllToolbars(() => _activeOpen = true, () => _activeOpen = false,
                all, ModInfo.Name, "activeMissions", IconActive, IconActive, "Active Missions");

            _tcAvail = gameObject.AddComponent<ToolbarControl>();
            _tcAvail.AddToAllToolbars(OpenAvail, CloseAvail,
                planning, ModInfo.Name, "missionControl", IconAvail, IconAvail, "Mission Control");
        }
        // ToolbarControl components are destroyed automatically along with this GameObject
        // (Object.Destroy(_ui.gameObject) in ContractsScenario.OnDestroy), which runs their own
        // cleanup - no manual button removal needed here.

        private static bool AvailableSceneAllowed =>
            HighLogic.LoadedScene == GameScenes.SPACECENTER ||
            HighLogic.LoadedScene == GameScenes.EDITOR ||
            HighLogic.LoadedScene == GameScenes.TRACKSTATION;

        private void OnGUI()
        {
            if (_mgr == null) return;
            GUISkin prevSkin = GUI.skin;
            Matrix4x4 prevMatrix = GUI.matrix;
            try
            {
                Theme.EnsureBuilt();
                GUI.skin = Theme.Skin;

                // Mission Control and Settings honor the UI scale slider.
                if (Mathf.Abs(UiScale - 1f) > 0.001f)
                    GUI.matrix = Matrix4x4.Scale(new Vector3(UiScale, UiScale, 1f)) * prevMatrix;

                if (_availOpen && AvailableSceneAllowed)
                {
                    EnsureMissionCenterRect();
                    _selRect = GUI.Window(GetInstanceID(), _selRect,
                        _ => _selection.Draw(_mgr, _selRect.width, _selRect.height, CloseAvail),
                        "Mission Control", Theme.Window);
                    _selRect = ClampRect(_selRect, MinSelW, MinSelH);
                    Theme.DrawWindowBorder(_selRect);
                    DrawSelectionResizeHandle();

                    if (_selection.SettingsToggleRequested)
                    { _settingsOpen = !_settingsOpen; _selection.SettingsToggleRequested = false; }
                }

                if (_settingsOpen && AvailableSceneAllowed)
                {
                    _setRect = GUILayout.Window(GetInstanceID() + 3, _setRect,
                        _ => _settings.Draw(_mgr, SetW, SetH, () => _settingsOpen = false), "Settings", Theme.Window,
                        GUILayout.Width(SetW), GUILayout.Height(SetH));
                    Theme.DrawWindowBorder(new Rect(_setRect.x, _setRect.y, SetW, SetH));
                }

                // Active Missions and its confirm dialog keep their original fixed size and
                // ignore the UI scale slider, so they never stretch along with Mission Control.
                GUI.matrix = prevMatrix;

                if (_activeOpen)
                {
                    _actRect = GUILayout.Window(GetInstanceID() + 1, _actRect,
                        _ => _activeWin.Draw(_mgr, ActW, ActH, CloseActive), "Active Missions", Theme.Window,
                        GUILayout.Width(ActW), GUILayout.Height(ActH));
                    Theme.DrawWindowBorder(new Rect(_actRect.x, _actRect.y, ActW, ActH));
                }

                if (_activeWin.PendingAbortId != null)
                {
                    if (_activeWin.ConfirmRect.x <= 1f)
                        _activeWin.ConfirmRect = new Rect(Screen.width / 2f - ConfW / 2f, Screen.height / 2f - ConfH / 2f, ConfW, ConfH);
                    _activeWin.ConfirmRect = GUILayout.Window(GetInstanceID() + 2, _activeWin.ConfirmRect,
                        _ => _activeWin.DrawConfirm(_mgr, ConfW), "Confirm", Theme.Window,
                        GUILayout.Width(ConfW), GUILayout.Height(ConfH));
                    Theme.DrawWindowBorder(new Rect(_activeWin.ConfirmRect.x, _activeWin.ConfirmRect.y, ConfW, ConfH));
                }
            }
            // Fully qualified: ToolbarControl_NS (vendored) also declares an internal Log class.
            catch (System.Exception e) { Core.Log.Ex("OnGUI", e); }
            finally { GUI.skin = prevSkin; GUI.matrix = prevMatrix; }
        }

        /// <summary>Swaps the active-missions launcher icon to the pre-baked badge variant while
        /// any mission is ready to claim, so the toolbar shows it without opening a window.</summary>
        private void Update()
        {
            if (_mgr == null || _tcActive == null) return;
            if (Time.realtimeSinceStartup < _nextBadgeCheck) return;
            _nextBadgeCheck = Time.realtimeSinceStartup + 1f;

            bool ready = false;
            var all = _mgr.Catalog.All;
            for (int i = 0; i < all.Count; i++)
                if (all[i].Status == Model.MissionStatus.ReadyToClaim) { ready = true; break; }
            if (ready == _badgeOn) return;
            _badgeOn = ready;
            string icon = ready ? IconActiveAlert : IconActive;
            try { _tcActive.SetTexture(icon, icon); }
            catch (System.Exception) { }
        }

        private void OpenAvail()
        {
            _availOpen = true;
            _selRect = MissionCenterRect();
            _selRectInitialized = true;
            _appliedMissionCenterScale = Tuning.MissionCenterScale;
            // Land on the epoch the campaign is currently at instead of always on epoch 1.
            if (_mgr != null) _selection.FocusRelevantEpoch(_mgr);
        }

        private void CloseAvail() { _availOpen = false; _resizingSelection = false; _tcAvail?.SetFalse(false); }
        private void CloseActive() { _activeOpen = false; _tcActive?.SetFalse(false); }

        private static Rect MissionCenterRect()
        {
            const float margin = 24f;
            float scale = Mathf.Clamp(Tuning.MissionCenterScale, 0.55f, 1.0f);
            float maxW = Mathf.Max(MinSelW, VirtualW - margin * 2f);
            float maxH = Mathf.Max(MinSelH, VirtualH - margin * 2f);
            float w = Mathf.Clamp(maxW * scale, MinSelW, maxW);
            float h = Mathf.Clamp(maxH * scale, MinSelH, maxH);
            return new Rect((VirtualW - w) * 0.5f, (VirtualH - h) * 0.5f, w, h);
        }

        private void EnsureMissionCenterRect()
        {
            bool scaleChanged = Mathf.Abs(_appliedMissionCenterScale - Tuning.MissionCenterScale) > 0.001f;
            if (!_selRectInitialized || scaleChanged)
            {
                _selRect = MissionCenterRect();
                _selRectInitialized = true;
                _appliedMissionCenterScale = Tuning.MissionCenterScale;
            }
            _selRect = ClampRect(_selRect, MinSelW, MinSelH);
        }

        /// <summary>Keeps a window rect on the (scaled) screen and above its minimum size.</summary>
        private static Rect ClampRect(Rect r, float minW, float minH)
        {
            float maxW = Mathf.Max(minW, VirtualW - 8f);
            float maxH = Mathf.Max(minH, VirtualH - 8f);
            r.width = Mathf.Clamp(r.width, minW, maxW);
            r.height = Mathf.Clamp(r.height, minH, maxH);
            r.x = Mathf.Clamp(r.x, 4f, Mathf.Max(4f, VirtualW - r.width - 4f));
            r.y = Mathf.Clamp(r.y, 4f, Mathf.Max(4f, VirtualH - r.height - 4f));
            return r;
        }

        /// <summary>Bottom-right drag handle for Mission Control.</summary>
        private void DrawSelectionResizeHandle()
        {
            Rect handle = new Rect(_selRect.xMax - 26f, _selRect.yMax - 26f, 20f, 20f);
            Event ev = Event.current;

            if (ev.type == EventType.Repaint)
            {
                Color line = new Color(0.32f, 0.80f, 1.00f, handle.Contains(ev.mousePosition) ? 0.82f : 0.46f);
                Theme.DrawRect(new Rect(handle.x + 13f, handle.y + 4f, 2f, 12f), line);
                Theme.DrawRect(new Rect(handle.x + 7f, handle.y + 10f, 8f, 2f), line);
                Theme.DrawRect(new Rect(handle.x + 10f, handle.y + 15f, 6f, 2f), line);
            }

            if (ev.type == EventType.MouseDown && ev.button == 0 && handle.Contains(ev.mousePosition))
            {
                _resizingSelection = true;
                _resizeStartMouse = ev.mousePosition;
                _resizeStartSize = new Vector2(_selRect.width, _selRect.height);
                ev.Use();
            }

            if (!_resizingSelection) return;
            if (ev.type == EventType.MouseDrag || ev.type == EventType.Repaint)
            {
                Vector2 delta = ev.mousePosition - _resizeStartMouse;
                _selRect.width = _resizeStartSize.x + delta.x;
                _selRect.height = _resizeStartSize.y + delta.y;
                _selRect = ClampRect(_selRect, MinSelW, MinSelH);
                if (ev.type == EventType.MouseDrag) ev.Use();
            }
            if (ev.type == EventType.MouseUp)
            {
                _resizingSelection = false;
                ev.Use();
            }
        }
    }
}
