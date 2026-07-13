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
        private const float SetW = 440f, SetH = 740f;
        private const float MinSelW = 760f, MinSelH = 520f;
        private const float MinActW = 400f, MinActH = 420f;
        private Rect _selRect = new Rect(24, 24, 1200, 760);
        private Rect _actRect = new Rect(700, 80, ActW, ActH);
        private Rect _setRect = new Rect(180, 60, SetW, SetH);
        private bool _selRectInitialized;
        private int _resizing;   // 0 = none, 1 = mission control, 2 = active missions
        private Vector2 _resizeStartMouse;
        private Vector2 _resizeStartSize;
        private float _appliedMissionCenterScale = -1f;
        private Texture2D _iconActiveAlert;
        private bool _badgeOn;
        private float _nextBadgeCheck;

        /// <summary>Global window scale; IMGUI ignores KSP's UI scale, so the mod has its own.</summary>
        private static float UiScale => Mathf.Clamp(Tuning.UiScale, 0.8f, 1.6f);
        /// <summary>Screen size in scaled GUI coordinates.</summary>
        private static float VirtualW => Screen.width / UiScale;
        private static float VirtualH => Screen.height / UiScale;

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
            Matrix4x4 prevMatrix = GUI.matrix;
            try
            {
                Theme.EnsureBuilt();
                GUI.skin = Theme.Skin;
                if (Mathf.Abs(UiScale - 1f) > 0.001f)
                    GUI.matrix = Matrix4x4.Scale(new Vector3(UiScale, UiScale, 1f)) * GUI.matrix;

                if (_availOpen && AvailableSceneAllowed)
                {
                    EnsureMissionCenterRect();
                    _selRect = GUI.Window(GetInstanceID(), _selRect,
                        _ => _selection.Draw(_mgr, _selRect.width, _selRect.height, CloseAvail),
                        "Mission Control", Theme.Window);
                    _selRect = ClampRect(_selRect, MinSelW, MinSelH);
                    Theme.DrawWindowBorder(_selRect);
                    DrawResizeHandle(1, ref _selRect, MinSelW, MinSelH);

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
                        _ => _activeWin.Draw(_mgr, _actRect.width, _actRect.height, CloseActive), "Active Missions", Theme.Window,
                        GUILayout.Width(_actRect.width), GUILayout.Height(_actRect.height));
                    _actRect = ClampRect(_actRect, MinActW, MinActH);
                    Theme.DrawWindowBorder(_actRect);
                    DrawResizeHandle(2, ref _actRect, MinActW, MinActH);
                }

                if (_activeWin.PendingAbortId != null)
                {
                    if (_activeWin.ConfirmRect.x <= 1f)
                        _activeWin.ConfirmRect = new Rect(VirtualW / 2f - ConfW / 2f, VirtualH / 2f - ConfH / 2f, ConfW, ConfH);
                    _activeWin.ConfirmRect = GUILayout.Window(GetInstanceID() + 2, _activeWin.ConfirmRect,
                        _ => _activeWin.DrawConfirm(_mgr, ConfW), "Confirm", Theme.Window,
                        GUILayout.Width(ConfW), GUILayout.Height(ConfH));
                    Theme.DrawWindowBorder(new Rect(_activeWin.ConfirmRect.x, _activeWin.ConfirmRect.y, ConfW, ConfH));
                }
            }
            catch (System.Exception e) { Log.Ex("OnGUI", e); }
            finally { GUI.skin = prevSkin; GUI.matrix = prevMatrix; }
        }

        /// <summary>Swaps the active-missions launcher icon to the badge variant while any
        /// mission is ready to claim, so the toolbar shows it without opening a window.</summary>
        private void Update()
        {
            if (_mgr == null || _btnActive == null) return;
            if (Time.realtimeSinceStartup < _nextBadgeCheck) return;
            _nextBadgeCheck = Time.realtimeSinceStartup + 1f;

            bool ready = false;
            var all = _mgr.Catalog.All;
            for (int i = 0; i < all.Count; i++)
                if (all[i].Status == Model.MissionStatus.ReadyToClaim) { ready = true; break; }
            if (ready == _badgeOn) return;
            _badgeOn = ready;
            try { _btnActive.SetTexture(ready ? AlertIcon() : IconApp("aktiv")); }
            catch (System.Exception) { }
        }

        /// <summary>The active-missions icon with a green claim dot in the top-right corner.</summary>
        private Texture2D AlertIcon()
        {
            if (_iconActiveAlert != null) return _iconActiveAlert;
            Texture2D baseTex = IconApp("aktiv");
            try
            {
                int w = baseTex.width, h = baseTex.height;
                var tex = new Texture2D(w, h, TextureFormat.RGBA32, false) { hideFlags = HideFlags.HideAndDontSave };
                tex.SetPixels32(baseTex.GetPixels32());
                float radius = w * 0.17f;
                float cx = w - radius - 2f, cy = h - radius - 2f;   // texture origin bottom-left -> top-right corner
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                    {
                        float d = Mathf.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                        if (d <= radius) tex.SetPixel(x, y, new Color(0.28f, 0.85f, 0.36f, 1f));
                        else if (d <= radius + 1.6f) tex.SetPixel(x, y, new Color(0.04f, 0.09f, 0.05f, 1f));
                    }
                tex.Apply();
                _iconActiveAlert = tex;
            }
            catch (System.Exception) { _iconActiveAlert = baseTex; }
            return _iconActiveAlert;
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

        private void CloseAvail() { _availOpen = false; _resizing = 0; _btnAvail?.SetFalse(false); }
        private void CloseActive() { _activeOpen = false; _btnActive?.SetFalse(false); }

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

        /// <summary>Bottom-right drag handle shared by the resizable windows.</summary>
        private void DrawResizeHandle(int id, ref Rect rect, float minW, float minH)
        {
            Rect handle = new Rect(rect.xMax - 26f, rect.yMax - 26f, 20f, 20f);
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
                _resizing = id;
                _resizeStartMouse = ev.mousePosition;
                _resizeStartSize = new Vector2(rect.width, rect.height);
                ev.Use();
            }

            if (_resizing != id) return;
            if (ev.type == EventType.MouseDrag || ev.type == EventType.Repaint)
            {
                Vector2 delta = ev.mousePosition - _resizeStartMouse;
                rect.width = _resizeStartSize.x + delta.x;
                rect.height = _resizeStartSize.y + delta.y;
                rect = ClampRect(rect, minW, minH);
                if (ev.type == EventType.MouseDrag) ev.Use();
            }
            if (ev.type == EventType.MouseUp)
            {
                _resizing = 0;
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
