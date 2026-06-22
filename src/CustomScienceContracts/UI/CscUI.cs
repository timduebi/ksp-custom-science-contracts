using CustomScienceContracts.Core;
using KSP.UI.Screens;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Two app-launcher buttons: active missions and mission control.</summary>
    public class CscUI : MonoBehaviour
    {
        private ContractManager _mgr;
        private ApplicationLauncherButton _btnActive;
        private ApplicationLauncherButton _btnAvail;
        private bool _activeOpen, _availOpen;

        private readonly SelectionWindow _selection = new SelectionWindow();
        private readonly ActiveMissionsWindow _activeWin = new ActiveMissionsWindow();
        private readonly SettingsWindow _settings = new SettingsWindow();
        private bool _settingsOpen;

        private const float ActW = 480f, ActH = 620f, ConfW = 340f, ConfH = 178f;
        private const float SetW = 440f, SetH = 560f;
        private const float MinSelW = 760f, MinSelH = 520f;
        private Rect _selRect = new Rect(24, 24, 1200, 760);
        private Rect _actRect = new Rect(700, 80, ActW, ActH);
        private Rect _setRect = new Rect(180, 120, SetW, SetH);
        private bool _selRectInitialized;
        private bool _resizingSelection;
        private Vector2 _resizeStartMouse;
        private Vector2 _resizeStartSize;
        private float _appliedMissionCenterScale = -1f;

        public void Bind(ContractManager mgr) => _mgr = mgr;

        private void Awake() => GameEvents.onGUIApplicationLauncherReady.Add(AddButtons);
        private void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(AddButtons);
            Remove(ref _btnActive);
            Remove(ref _btnAvail);
        }

        private void AddButtons()
        {
            if (ApplicationLauncher.Instance == null) return;
            var all = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.FLIGHT |
                      ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.TRACKSTATION |
                      ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH;
            var planning = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.TRACKSTATION |
                           ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH;

            if (_btnActive == null)
                _btnActive = ApplicationLauncher.Instance.AddModApplication(
                    () => _activeOpen = true, () => _activeOpen = false, null, null, null, null,
                    all, IconApp("aktiv"));

            if (_btnAvail == null)
                _btnAvail = ApplicationLauncher.Instance.AddModApplication(
                    OpenAvail, CloseAvail, null, null, null, null,
                    planning, IconApp("missionen"));
        }

        private void Remove(ref ApplicationLauncherButton b)
        {
            if (b != null) { ApplicationLauncher.Instance?.RemoveModApplication(b); b = null; }
        }

        private static bool AvailableSceneAllowed =>
            HighLogic.LoadedScene == GameScenes.SPACECENTER ||
            HighLogic.LoadedScene == GameScenes.EDITOR ||
            HighLogic.LoadedScene == GameScenes.TRACKSTATION;

        private void OnGUI()
        {
            if (_mgr == null) return;
            GUISkin prevSkin = GUI.skin;
            try
            {
                Theme.EnsureBuilt();
                GUI.skin = Theme.Skin;

                if (_availOpen && AvailableSceneAllowed)
                {
                    EnsureMissionCenterRect();
                    _selRect = GUI.Window(GetInstanceID(), _selRect,
                        _ => _selection.Draw(_mgr, _selRect.width, _selRect.height, CloseAvail),
                        "Mission Control", Theme.Window);
                    _selRect = ClampSelectionRect(_selRect);
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
            catch (System.Exception e) { Log.Ex("OnGUI", e); }
            finally { GUI.skin = prevSkin; }
        }

        private void OpenAvail()
        {
            _availOpen = true;
            _selRect = MissionCenterRect();
            _selRectInitialized = true;
            _appliedMissionCenterScale = Tuning.MissionCenterScale;
        }

        private void CloseAvail() { _availOpen = false; _resizingSelection = false; _btnAvail?.SetFalse(false); }
        private void CloseActive() { _activeOpen = false; _btnActive?.SetFalse(false); }

        private static Rect MissionCenterRect()
        {
            const float margin = 24f;
            float scale = Mathf.Clamp(Tuning.MissionCenterScale, 0.55f, 1.0f);
            float maxW = Mathf.Max(MinSelW, Screen.width - margin * 2f);
            float maxH = Mathf.Max(MinSelH, Screen.height - margin * 2f);
            float w = Mathf.Clamp(maxW * scale, MinSelW, maxW);
            float h = Mathf.Clamp(maxH * scale, MinSelH, maxH);
            return new Rect((Screen.width - w) * 0.5f, (Screen.height - h) * 0.5f, w, h);
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
            _selRect = ClampSelectionRect(_selRect);
        }

        private static Rect ClampSelectionRect(Rect r)
        {
            float maxW = Mathf.Max(MinSelW, Screen.width - 8f);
            float maxH = Mathf.Max(MinSelH, Screen.height - 8f);
            r.width = Mathf.Clamp(r.width, MinSelW, maxW);
            r.height = Mathf.Clamp(r.height, MinSelH, maxH);
            r.x = Mathf.Clamp(r.x, 4f, Mathf.Max(4f, Screen.width - r.width - 4f));
            r.y = Mathf.Clamp(r.y, 4f, Mathf.Max(4f, Screen.height - r.height - 4f));
            return r;
        }

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
                _selRect = ClampSelectionRect(_selRect);
                if (ev.type == EventType.MouseDrag) ev.Use();
            }
            if (ev.type == EventType.MouseUp)
            {
                _resizingSelection = false;
                ev.Use();
            }
        }

        private static Texture2D IconApp(string name)
        {
            var t = IconLibrary.App(name);
            return t != null ? t : Icon(name);
        }

        private static Texture2D Icon(string name)
        {
            var t = IconLibrary.UI(name);
            if (t != null) return t;
            const int s = 38;
            var x = new Texture2D(s, s, TextureFormat.RGBA32, false);
            for (int yy = 0; yy < s; yy++)
                for (int xx = 0; xx < s; xx++)
                    x.SetPixel(xx, yy, (xx < 2 || yy < 2 || xx >= s - 2 || yy >= s - 2) ? Theme.Accent : Theme.WinBg);
            x.Apply();
            return x;
        }
    }
}
