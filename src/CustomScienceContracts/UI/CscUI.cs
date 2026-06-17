using CustomScienceContracts.Core;
using KSP.UI.Screens;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Zwei AppLauncher-Buttons: Aktive Missionen (ueberall) und Verfuegbare Missionen
    /// (nur SpaceCenter/Editor/Ortungsstation — im Flug/Karte ausgeblendet). Zeichnet beide Fenster
    /// mit abgerundetem, hellblau umrandetem Rahmen sowie den Abbruch-Bestaetigungsdialog.</summary>
    public class CscUI : MonoBehaviour
    {
        private ContractManager _mgr;
        private ApplicationLauncherButton _btnAvail;   // nur "Verfuegbare Missionen" im Stock-AppLauncher
        private bool _activeOpen, _availOpen;

        private readonly SelectionWindow _selection = new SelectionWindow();
        private readonly ActiveMissionsWindow _activeWin = new ActiveMissionsWindow();
        private readonly SettingsWindow _settings = new SettingsWindow();
        private bool _settingsOpen;

        private const float SelW = 560f, SelH = 690f, ActW = 480f, ActH = 620f, ConfW = 340f, ConfH = 178f;
        private const float SetW = 440f, SetH = 520f;
        private Rect _selRect = new Rect(110, 80, SelW, SelH);
        private Rect _actRect = new Rect(700, 80, ActW, ActH);
        private Rect _setRect = new Rect(180, 120, SetW, SetH);

        public void Bind(ContractManager mgr) => _mgr = mgr;

        private void Awake() => GameEvents.onGUIApplicationLauncherReady.Add(AddButtons);
        private void OnDestroy()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(AddButtons);
            Remove(ref _btnAvail);
        }

        private void AddButtons()
        {
            if (ApplicationLauncher.Instance == null) return;
            var planning = ApplicationLauncher.AppScenes.SPACECENTER | ApplicationLauncher.AppScenes.TRACKSTATION |
                           ApplicationLauncher.AppScenes.VAB | ApplicationLauncher.AppScenes.SPH;

            // "Aktive Missionen" steht bewusst NICHT im Stock-AppLauncher, sondern als eigener Button oben
            // (siehe DrawActiveButton). Hier nur der Planungs-Button "Verfuegbare Missionen".
            if (_btnAvail == null)
                _btnAvail = ApplicationLauncher.Instance.AddModApplication(
                    () => _availOpen = true, () => _availOpen = false, null, null, null, null,
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
                GUI.skin = Theme.Skin;   // eigener Scrollbalken-Stil fuer unsere Fenster

                DrawActiveButton();

                if (_availOpen && AvailableSceneAllowed)
                {
                    _selRect = GUILayout.Window(GetInstanceID(), _selRect,
                        _ => _selection.Draw(_mgr, SelW, SelH, CloseAvail), "Missionskontrolle", Theme.Window,
                        GUILayout.Width(SelW), GUILayout.Height(SelH));
                    Theme.DrawWindowBorder(new Rect(_selRect.x, _selRect.y, SelW, SelH));

                    if (_selection.SettingsToggleRequested)
                    { _settingsOpen = !_settingsOpen; _selection.SettingsToggleRequested = false; }
                }

                if (_settingsOpen && AvailableSceneAllowed)
                {
                    _setRect = GUILayout.Window(GetInstanceID() + 3, _setRect,
                        _ => _settings.Draw(_mgr, SetW, SetH, () => _settingsOpen = false), "Einstellungen", Theme.Window,
                        GUILayout.Width(SetW), GUILayout.Height(SetH));
                    Theme.DrawWindowBorder(new Rect(_setRect.x, _setRect.y, SetW, SetH));
                }

                if (_activeOpen)
                {
                    _actRect = GUILayout.Window(GetInstanceID() + 1, _actRect,
                        _ => _activeWin.Draw(_mgr, ActW, ActH, CloseActive), "Aktive Missionen", Theme.Window,
                        GUILayout.Width(ActW), GUILayout.Height(ActH));
                    Theme.DrawWindowBorder(new Rect(_actRect.x, _actRect.y, ActW, ActH));
                }

                if (_activeWin.PendingAbortId != null)
                {
                    if (_activeWin.ConfirmRect.x <= 1f)
                        _activeWin.ConfirmRect = new Rect(Screen.width / 2f - ConfW / 2f, Screen.height / 2f - ConfH / 2f, ConfW, ConfH);
                    _activeWin.ConfirmRect = GUILayout.Window(GetInstanceID() + 2, _activeWin.ConfirmRect,
                        _ => _activeWin.DrawConfirm(_mgr, ConfW), "Bestätigen", Theme.Window,
                        GUILayout.Width(ConfW), GUILayout.Height(ConfH));
                    Theme.DrawWindowBorder(new Rect(_activeWin.ConfirmRect.x, _activeWin.ConfirmRect.y, ConfW, ConfH));
                }
            }
            catch (System.Exception e) { Log.Ex("OnGUI", e); }
            finally { GUI.skin = prevSkin; }
        }

        private void CloseAvail() { _availOpen = false; _btnAvail?.SetFalse(false); }
        private void CloseActive() { _activeOpen = false; }

        /// <summary>Eigener Aktive-Missionen-Button oben (nahe der Ressourcenanzeige), bewusst ausserhalb
        /// des Stock-AppLaunchers. Position/Groesse aus settings.cfg; X=-1 => automatisch oben mittig.</summary>
        private void DrawActiveButton()
        {
            float size = Tuning.ActiveButtonSize;
            float x = Tuning.ActiveButtonX >= 0f ? Tuning.ActiveButtonX : (Screen.width * 0.5f - 260f);
            float y = Tuning.ActiveButtonY;
            var rect = new Rect(x, y, size, size);
            var icon = IconLibrary.App("aktiv");
            var content = icon != null
                ? new GUIContent(icon, "Aktive Missionen")
                : new GUIContent("AM", "Aktive Missionen");
            if (GUI.Button(rect, content, Theme.TopIconButton))
                _activeOpen = !_activeOpen;
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
