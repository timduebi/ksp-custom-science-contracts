using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Dunkles Theme mit duennem hellblauem Rahmen. NUR das Fenster (und sein Rahmen) ist
    /// abgerundet; alle Innenflaechen (Kacheln, Buttons, Tabs, Header, Balken, Scrollbar) sind eckig.
    /// Hintergruende werden prozedural gebaut (volle Farbkontrolle, Hover-Zustaende). EIN Akzent
    /// (Hellblau); Farb-Codierung der Koerper/Sparten kommt aus BodyVisual.</summary>
    public static class Theme
    {
        public static readonly Color Accent     = new Color(0.32f, 0.80f, 1.00f);   // Hellblau
        public static readonly Color TextBright  = new Color(0.95f, 0.97f, 0.99f);
        public static readonly Color TextDim     = new Color(0.80f, 0.84f, 0.90f);   // heller = besser lesbar
        public static readonly Color TextGrey    = new Color(0.70f, 0.74f, 0.80f);
        public static readonly Color WinBg       = new Color(0.07f, 0.08f, 0.11f, 0.98f);
        public static readonly Color FieldBg     = new Color(0.13f, 0.16f, 0.26f, 1f); // Dunkelblau, etwas heller
        public static readonly Color FieldBgRdy  = new Color(0.10f, 0.20f, 0.14f, 1f); // gruenlich (ReadyToClaim)
        public static readonly Color Ok          = new Color(0.42f, 0.86f, 0.50f);   // gruen erfuellt
        public static readonly Color Bad         = new Color(0.93f, 0.46f, 0.42f);   // rot offen
        public static readonly Color ClaimGreen  = new Color(0.40f, 0.85f, 0.48f);
        public static readonly Color AbortRed    = new Color(0.90f, 0.30f, 0.28f);

        public static Texture2D White { get; private set; }
        /// <summary>Eigener Skin nur fuer die Scrollbalken (eigener Handle im Stil).</summary>
        public static GUISkin Skin { get; private set; }

        private static bool _built;
        private static Texture2D _barTex, _ringTex;
        public static GUIStyle Window, Title, TabActive, TabInactive, GroupHeader, GroupHeaderPlain, MoonHeader,
                               ItemBox, ItemBoxReady, ItemTitle, ItemSub, ItemDesc, Pill, Locked,
                               AcceptBtn, ClaimBtn, CloseBtn, SettingsBtn, TopIconButton, Border, CondOk, CondBad, CondNeutral,
                               ClaimInfo, Station, Bar, Warn, Reward, RewardIcon;

        public static void EnsureBuilt()
        {
            if (_built) return;
            White = Solid(Color.white);

            // Nur das Fenster bleibt rund (radius 12); alle Innenflaechen sind eckig (radius 0).
            Texture2D winTex   = Rounded(WinBg, 40, 12);
            Texture2D fieldTex = Rounded(FieldBg, 28, 0);
            Texture2D readyTex = Rounded(FieldBgRdy, 28, 0);
            Color btnCol = new Color(0.20f, 0.23f, 0.30f);
            Color tabOnCol = new Color(0.18f, 0.40f, 0.52f), tabOffCol = new Color(0.14f, 0.16f, 0.20f);
            Color headCol = new Color(0.16f, 0.19f, 0.27f);

            _barTex  = Rounded(Color.white, 16, 0);
            _ringTex = RoundedRing(Accent, 30, 12, 2);   // Fensterrahmen bleibt rund

            Window = new GUIStyle(GUI.skin.window);
            Bg(Window, winTex, 12);
            Window.fontSize = 17; Window.fontStyle = FontStyle.Bold;
            // Titel in JEDEM Zustand gut lesbar weiss (nie schwarz); fokussiertes Fenster hellblau.
            Window.normal.textColor = Window.onNormal.textColor =
                Window.hover.textColor = Window.onHover.textColor =
                Window.active.textColor = Window.onActive.textColor = TextBright;
            Window.focused.textColor = Window.onFocused.textColor = Accent;
            Window.padding = new RectOffset(14, 14, 28, 14);

            Border = new GUIStyle(); Bg(Border, _ringTex, 12);
            Bar = new GUIStyle(); Bg(Bar, _barTex, 6);

            Title    = Label(18, FontStyle.Bold, Accent, TextAnchor.MiddleLeft);
            ItemTitle= Label(17, FontStyle.Bold, TextBright, TextAnchor.UpperLeft); ItemTitle.wordWrap = true;
            ItemSub  = Label(14, FontStyle.Normal, TextDim, TextAnchor.UpperLeft); ItemSub.wordWrap = true;
            ItemDesc = Label(15, FontStyle.Normal, TextDim, TextAnchor.UpperLeft); ItemDesc.wordWrap = true;
            Pill     = Label(16, FontStyle.Bold, Accent, TextAnchor.MiddleRight);
            Locked   = Label(14, FontStyle.Normal, TextGrey, TextAnchor.UpperLeft); Locked.wordWrap = true;
            CondOk   = Label(14, FontStyle.Bold, Ok, TextAnchor.UpperLeft); CondOk.wordWrap = true;
            CondBad  = Label(14, FontStyle.Normal, Bad, TextAnchor.UpperLeft); CondBad.wordWrap = true;
            CondNeutral = Label(14, FontStyle.Normal, TextDim, TextAnchor.UpperLeft); CondNeutral.wordWrap = true;
            ClaimInfo= Label(15, FontStyle.Bold, Accent, TextAnchor.MiddleLeft);
            Station  = Label(14, FontStyle.Bold, Accent, TextAnchor.UpperLeft); Station.wordWrap = true;

            TabActive   = Btn(tabOnCol,  TextBright, 7); TabActive.fontSize = 14; TabActive.fontStyle = FontStyle.Bold; TabActive.padding = new RectOffset(28, 6, 7, 7);
            TabInactive = Btn(tabOffCol, TextGrey,   7); TabInactive.fontSize = 14; TabInactive.padding = new RectOffset(28, 6, 7, 7);

            GroupHeader = Btn(headCol, TextBright, 7); GroupHeader.fontSize = 15; GroupHeader.fontStyle = FontStyle.Bold;
            GroupHeader.alignment = TextAnchor.MiddleLeft; GroupHeader.padding = new RectOffset(38, 8, 7, 7);
            // Wie GroupHeader, aber ohne Icon-Einzug (Aktive-Fenster zeigt im Header nur den Farbbalken).
            GroupHeaderPlain = new GUIStyle(GroupHeader) { padding = new RectOffset(16, 8, 7, 7) };
            MoonHeader = new GUIStyle(GroupHeader) { fontStyle = FontStyle.Normal, fontSize = 14 };
            MoonHeader.padding = new RectOffset(54, 8, 6, 6);

            ItemBox = new GUIStyle(GUI.skin.box); Bg(ItemBox, fieldTex, 9); ItemBox.padding = new RectOffset(40, 44, 8, 8);
            ItemBoxReady = new GUIStyle(ItemBox); Bg(ItemBoxReady, readyTex, 9);

            AcceptBtn = Btn(Accent,     Color.black, 7); AcceptBtn.fontSize = 15; AcceptBtn.fontStyle = FontStyle.Bold; AcceptBtn.padding = new RectOffset(12, 12, 7, 7);
            ClaimBtn  = Btn(ClaimGreen, Color.black, 7); ClaimBtn.fontSize = 16; ClaimBtn.fontStyle = FontStyle.Bold; ClaimBtn.padding = new RectOffset(12, 12, 7, 7);
            CloseBtn  = Btn(AbortRed,   Color.white, 6); CloseBtn.fontSize = 16; CloseBtn.fontStyle = FontStyle.Bold; CloseBtn.alignment = TextAnchor.MiddleCenter;
            SettingsBtn = Btn(new Color(0.20f, 0.23f, 0.30f), TextBright, 6); SettingsBtn.fontSize = 15; SettingsBtn.fontStyle = FontStyle.Bold; SettingsBtn.alignment = TextAnchor.MiddleCenter;
            // Eigenstaendiger Icon-Button oben (Aktive Missionen): dunkel-blaue Kachel, heller Akzent, hellblauer Rand.
            TopIconButton = Btn(new Color(0.10f, 0.13f, 0.20f, 0.94f), TextBright, 6); TopIconButton.padding = new RectOffset(3, 3, 3, 3); TopIconButton.alignment = TextAnchor.MiddleCenter;

            Warn = Label(14, FontStyle.Bold, AbortRed, TextAnchor.UpperLeft); Warn.wordWrap = true;

            // Wissenschaftswert: weisser Text (wird zur Laufzeit via GUI.color hellblau getoent),
            // davor das Science-Symbol im selben Blau.
            Reward = Label(16, FontStyle.Bold, TextBright, TextAnchor.MiddleRight);
            RewardIcon = new GUIStyle { margin = new RectOffset(0, 2, 4, 0), padding = new RectOffset(0, 0, 0, 0) };
            RewardIcon.normal.background = null;

            BuildScrollbarSkin();
            _built = true;
        }

        private static void BuildScrollbarSkin()
        {
            Skin = Object.Instantiate(GUI.skin);
            Skin.hideFlags = HideFlags.HideAndDontSave;

            Texture2D track = Rounded(new Color(0.05f, 0.06f, 0.09f, 0.9f), 16, 0);
            Texture2D thumb = Rounded(new Color(0.24f, 0.46f, 0.60f), 16, 0);
            Texture2D thumbHi= Rounded(Accent, 16, 0);

            var sb = Skin.verticalScrollbar;
            sb.normal.background = track; sb.border = new RectOffset(6, 6, 6, 6);
            sb.fixedWidth = 11; sb.margin = new RectOffset(2, 0, 0, 0); sb.padding = new RectOffset(0, 0, 0, 0);

            var th = Skin.verticalScrollbarThumb;
            th.normal.background = thumb; th.hover.background = th.active.background = thumbHi;
            th.border = new RectOffset(6, 6, 6, 6); th.fixedWidth = 11;

            // Pfeil-Buttons unsichtbar (sauberer Look).
            Skin.verticalScrollbarUpButton = new GUIStyle();
            Skin.verticalScrollbarDownButton = new GUIStyle();
        }

        /// <summary>Duenner abgerundeter Akzentrahmen, ueber dem Fenster gezeichnet (alle 4 Seiten).</summary>
        public static void DrawWindowBorder(Rect r)
        {
            if (Event.current.type != EventType.Repaint) return;
            GUI.Box(r, GUIContent.none, Border);
        }

        /// <summary>Abgerundeter farbiger Linksbalken (eingerueckt) + eingefaerbtes Icon ueber eine Zeile.</summary>
        public static void DrawLeftAccent(Rect r, Color color, Texture2D icon, float barW = 6f, float iconSize = 24f)
        {
            var prev = GUI.color;
            GUI.color = color;
            GUI.Box(new Rect(r.x + 2f, r.y + 3f, barW, r.height - 6f), GUIContent.none, Bar);
            if (icon != null)
            {
                GUI.color = Color.Lerp(color, Color.white, 0.30f);
                GUI.DrawTexture(new Rect(r.x + barW + 7f, r.y + (r.height - iconSize) / 2f, iconSize, iconSize),
                    icon, ScaleMode.ScaleToFit, true);
            }
            GUI.color = prev;
        }

        /// <summary>Abgerundeter rechter Aktionsbalken (rot/gruen) mit Symbol (X / Haken), klickbar, Hover.</summary>
        public static bool DrawRightAction(Rect rowRect, Color color, string symbol, float w = 30f)
        {
            Rect rr = new Rect(rowRect.xMax - w - 2f, rowRect.y + 3f, w, rowRect.height - 6f);
            var prev = GUI.color;
            bool hover = rr.Contains(Event.current.mousePosition);
            GUI.color = hover ? Color.Lerp(color, Color.white, 0.18f) : color;
            GUI.Box(rr, GUIContent.none, Bar);
            GUI.color = prev;
            return GUI.Button(rr, symbol, RightSymbol);
        }

        private static GUIStyle _rightSymbol;
        private static GUIStyle RightSymbol
        {
            get
            {
                if (_rightSymbol == null)
                {
                    _rightSymbol = new GUIStyle(GUI.skin.label)
                    { fontSize = 18, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
                    _rightSymbol.normal.textColor = _rightSymbol.hover.textColor = Color.white;
                }
                return _rightSymbol;
            }
        }

        // ---- Helpers ----
        private static GUIStyle Label(int size, FontStyle fs, Color col, TextAnchor a)
        {
            var s = new GUIStyle(GUI.skin.label) { fontSize = size, fontStyle = fs, alignment = a };
            s.normal.textColor = col; return s;
        }

        /// <summary>Button-Style mit eigenem Hover-Hintergrund (heller). Eckig (radius 0); der
        /// border-Parameter dient nur noch dem 9-Slice-Rand der flachen Flaeche.</summary>
        private static GUIStyle Btn(Color col, Color text, int border)
        {
            int size = border * 2 + 4;
            Texture2D tex = Rounded(col, size, 0);
            Texture2D hov = Rounded(Color.Lerp(col, Color.white, 0.16f), size, 0);
            var s = new GUIStyle(GUI.skin.button);
            s.normal.background = s.onNormal.background = s.active.background = s.focused.background = tex;
            s.hover.background = s.onHover.background = hov;
            s.border = new RectOffset(border, border, border, border);
            s.normal.textColor = s.hover.textColor = s.active.textColor = s.onNormal.textColor = text;
            return s;
        }

        private static void Bg(GUIStyle s, Texture2D tex, int border)
        {
            s.normal.background = s.onNormal.background = s.hover.background =
                s.active.background = s.focused.background = tex;
            s.border = new RectOffset(border, border, border, border);
        }

        private static Texture2D Solid(Color c)
        {
            var t = new Texture2D(1, 1, TextureFormat.RGBA32, false) { hideFlags = HideFlags.HideAndDontSave };
            t.SetPixel(0, 0, c); t.Apply(); return t;
        }

        private static bool InsideRounded(int x, int y, int size, int radius, int inset)
        {
            int lo = inset, hi = size - 1 - inset;
            if (x < lo || x > hi || y < lo || y > hi) return false;
            int r = Mathf.Max(0, radius - inset);
            int cx = Mathf.Clamp(x, lo + r, hi - r);
            int cy = Mathf.Clamp(y, lo + r, hi - r);
            int dx = x - cx, dy = y - cy;
            return dx * dx + dy * dy <= r * r;
        }

        private static Texture2D Rounded(Color c, int size, int radius)
        {
            var t = new Texture2D(size, size, TextureFormat.RGBA32, false)
            { wrapMode = TextureWrapMode.Clamp, hideFlags = HideFlags.HideAndDontSave };
            var clear = new Color(0, 0, 0, 0);
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                    t.SetPixel(x, y, InsideRounded(x, y, size, radius, 0) ? c : clear);
            t.Apply(); return t;
        }

        /// <summary>Abgerundeter Ring (nur Rand, innen transparent) — als 9-Slice fuer den Fensterrahmen.</summary>
        private static Texture2D RoundedRing(Color c, int size, int radius, int thickness)
        {
            var t = new Texture2D(size, size, TextureFormat.RGBA32, false)
            { wrapMode = TextureWrapMode.Clamp, hideFlags = HideFlags.HideAndDontSave };
            var clear = new Color(0, 0, 0, 0);
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    bool ring = InsideRounded(x, y, size, radius, 0) &&
                                !InsideRounded(x, y, size, radius, thickness);
                    t.SetPixel(x, y, ring ? c : clear);
                }
            t.Apply(); return t;
        }
    }
}
