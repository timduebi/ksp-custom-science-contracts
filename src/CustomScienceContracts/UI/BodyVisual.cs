using System.Collections.Generic;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Visual mapping (icon + color) for bodies, branches, subcategories and condition
    /// types. Planet icons are stock/OPM-like icons tinted toward the real body color. Mission
    /// icons come from the TrackingStation_ButtonMap set.</summary>
    public static class BodyVisual
    {
        public struct Visual { public Texture2D Icon; public Color Color; }

        private static Color RGB(int r, int g, int b) => new Color(r / 255f, g / 255f, b / 255f);
        private static readonly Dictionary<string, Texture2D> _bodyTintCache = new Dictionary<string, Texture2D>();

        // body name -> (planet icon key, color)
        private static readonly Dictionary<string, (string icon, Color col)> _bodies =
            new Dictionary<string, (string, Color)>
        {
            // Star
            { "Sun",     ("pol",    RGB(255,210,80)) },
            // Stock KSP
            { "Kerbin",  ("kerbin", RGB(46,111,176)) },
            { "Mun",     ("mun",    RGB(158,158,158)) },
            { "Minmus",  ("ovok",   RGB(149,218,200)) },
            { "Moho",    ("moho",   RGB(140,123,107)) },
            { "Eve",     ("eve",    RGB(139,88,186)) },
            { "Gilly",   ("gilly",  RGB(134,122,105)) },
            { "Duna",    ("duna",   RGB(193,80,46)) },
            { "Ike",     ("mun",    RGB(118,112,105)) },
            { "Dres",    ("ovok",   RGB(150,142,126)) },
            { "Jool",    ("jool",   RGB(107,185,80)) },
            { "Laythe",  ("kerbin", RGB(70,139,198)) },
            { "Vall",    ("vall",   RGB(185,212,224)) },
            { "Tylo",    ("tylo",   RGB(158,150,133)) },
            { "Bop",     ("gilly",  RGB(111,98,82)) },
            { "Pol",     ("pol",    RGB(213,190,75)) },
            { "Eeloo",   ("plock",  RGB(213,213,201)) },
            // Planets
            { "Earth",   ("kerbin", RGB(46,111,176)) },
            { "Moon",    ("mun",    RGB(158,158,158)) },
            { "Mars",    ("duna",   RGB(193,80,46)) },
            { "Mercury", ("moho",   RGB(140,123,107)) },
            { "Venus",   ("eve",    RGB(217,180,90)) },
            { "Jupiter", ("jool",   RGB(200,150,75)) },
            { "Saturn",  ("sarnus", RGB(217,198,138)) },
            { "Uranus",  ("urlum",  RGB(143,217,212)) },
            { "Neptune", ("neidon", RGB(63,95,192)) },
            { "Pluto",   ("plock",  RGB(181,158,132)) },
            // Martian moons
            { "Phobos", ("gilly", RGB(110,102,90)) },
            { "Deimos", ("gilly", RGB(124,114,100)) },
            // Jovian moons
            { "Io",       ("pol",  RGB(215,194,75)) },
            { "Europa",   ("vall", RGB(207,216,224)) },
            { "Ganymede", ("tylo", RGB(142,133,118)) },
            { "Callisto", ("tylo", RGB(111,106,100)) },
            { "Amalthea", ("gilly",RGB(160,85,64)) },
            { "Thebe",    ("gilly",RGB(124,116,104)) },
            // Saturnian moons
            { "Titan",     ("tekto", RGB(215,154,60)) },
            { "Enceladus", ("hale",  RGB(230,238,242)) },
            { "Rhea",      ("slate", RGB(194,200,204)) },
            { "Iapetus",   ("slate", RGB(154,139,112)) },
            { "Dione",     ("vall",  RGB(200,204,208)) },
            { "Tethys",    ("vall",  RGB(212,218,222)) },
            { "Mimas",     ("ovok",  RGB(178,178,178)) },
            { "Hyperion",  ("gilly", RGB(168,152,120)) },
            { "Phoebe",    ("gilly", RGB(92,88,79)) },
            // Uranian moons
            { "Titania", ("vall",  RGB(182,174,164)) },
            { "Oberon",  ("slate", RGB(153,142,130)) },
            { "Ariel",   ("vall",  RGB(200,200,200)) },
            { "Umbriel", ("mun",   RGB(110,106,100)) },
            { "Miranda", ("ovok",  RGB(174,178,182)) },
            { "Puck",    ("gilly", RGB(106,100,92)) },
            // Neptunian moons
            { "Triton",  ("vall",  RGB(214,200,194)) },
            { "Nereid",  ("gilly", RGB(138,134,126)) },
            { "Proteus", ("gilly", RGB(94,90,82)) },
            // Pluto's moon
            { "Charon",  ("mun",   RGB(154,147,138)) },
            // Asteroids
            { "Ceres",    ("ovok",  RGB(156,150,140)) },
            { "Pallas",   ("gilly", RGB(122,118,108)) },
            { "Vesta",    ("ovok",  RGB(168,154,128)) },
            { "Eros",     ("gilly", RGB(168,142,110)) },
            { "Ryugu",    ("gilly", RGB(76,72,68)) },
            { "Psyche",   ("gilly", RGB(142,138,132)) },
            { "Ida",      ("gilly", RGB(138,130,112)) },
            { "Dactyl",   ("gilly", RGB(122,114,102)) },
            { "Arrokoth", ("gilly", RGB(156,90,70)) },
        };

        // Subcategory label (UI) -> body name for icon/color. German labels are kept for
        // backwards compatibility with the default German contract catalog.
        private static readonly Dictionary<string, string> _subToBody = new Dictionary<string, string>
        {
            { "Earth", "Earth" }, { "Erde", "Earth" },
            { "Moon", "Moon" }, { "Luna", "Moon" },
            { "Kerbin", "Kerbin" }, { "Mun", "Mun" }, { "Minmus", "Minmus" },
            { "Moho", "Moho" }, { "Eve", "Eve" }, { "Gilly", "Gilly" },
            { "Duna", "Duna" }, { "Ike", "Ike" }, { "Dres", "Dres" },
            { "Jool", "Jool" }, { "Laythe", "Laythe" }, { "Vall", "Vall" },
            { "Tylo", "Tylo" }, { "Bop", "Bop" }, { "Pol", "Pol" },
            { "Eeloo", "Eeloo" }, { "Kerbin System", "Kerbin" },
            { "Jool System", "Jool" }, { "Inner Planets", "Moho" },
            { "Outer System", "Eeloo" },
            { "Mars", "Mars" }, { "Venus", "Venus" },
            { "Mercury", "Mercury" }, { "Merkur", "Mercury" },
            { "Jupiter", "Jupiter" }, { "Saturn", "Saturn" },
            { "Uranus", "Uranus" },
            { "Neptune", "Neptune" }, { "Neptun", "Neptune" },
            { "Pluto", "Pluto" },
            { "Asteroids", "Ceres" }, { "Asteroiden", "Ceres" },
        };

        /// <summary>Body name behind a subcategory label, or null.</summary>
        public static string SubcatBodyName(string label) =>
            _subToBody.TryGetValue(label, out string b) ? b : null;

        // Internal CelestialBody.name or legacy label -> English display name.
        private static readonly Dictionary<string, string> _display = new Dictionary<string, string>
        {
            { "Earth", "Earth" }, { "Erde", "Earth" },
            { "Moon", "Moon" }, { "Luna", "Moon" },
            { "Kerbin", "Kerbin" }, { "Mun", "Mun" }, { "Minmus", "Minmus" },
            { "Moho", "Moho" }, { "Eve", "Eve" }, { "Gilly", "Gilly" },
            { "Duna", "Duna" }, { "Ike", "Ike" }, { "Dres", "Dres" },
            { "Jool", "Jool" }, { "Laythe", "Laythe" }, { "Vall", "Vall" },
            { "Tylo", "Tylo" }, { "Bop", "Bop" }, { "Pol", "Pol" },
            { "Eeloo", "Eeloo" }, { "Kerbin System", "Kerbin System" },
            { "Jool System", "Jool System" }, { "Inner Planets", "Inner Planets" },
            { "Outer System", "Outer System" },
            { "Mercury", "Mercury" }, { "Merkur", "Mercury" }, { "Venus", "Venus" },
            { "Mars", "Mars" }, { "Jupiter", "Jupiter" }, { "Saturn", "Saturn" }, { "Uranus", "Uranus" },
            { "Neptune", "Neptune" }, { "Neptun", "Neptune" }, { "Pluto", "Pluto" },
            { "Sun", "Sun" }, { "Sonne", "Sun" },
            { "Ganymede", "Ganymede" }, { "Ganymed", "Ganymede" },
            { "Callisto", "Callisto" }, { "Kallisto", "Callisto" }, { "Europa", "Europa" }, { "Io", "Io" },
            { "Titan", "Titan" }, { "Charon", "Charon" }, { "Triton", "Triton" },
            { "Asteroiden", "Asteroids" }, { "Asteroids", "Asteroids" },
            { "Interplanetar", "Interplanetary" }, { "Interplanetary", "Interplanetary" },
            { "Logistik", "Logistics" }, { "Logistics", "Logistics" },
        };

        /// <summary>English display name for an internal body or subcategory name. Falls back to
        /// the input when no mapping is registered.</summary>
        public static string DisplayName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return _display.TryGetValue(name, out string d) ? d : name;
        }

        public static Visual ForBody(string bodyName)
        {
            if (bodyName != null && _bodies.TryGetValue(bodyName, out var v))
                return new Visual { Icon = TintedBodyIcon(v.icon, v.col), Color = v.col };
            return new Visual { Icon = IconLibrary.Body("mun"), Color = RGB(150,150,150) };
        }

        private static Texture2D TintedBodyIcon(string iconKey, Color tint)
        {
            string cacheKey = $"{iconKey}:{ColorUtility.ToHtmlStringRGB(tint)}";
            if (_bodyTintCache.TryGetValue(cacheKey, out var cached))
            {
                if (cached != null) return cached;
                _bodyTintCache.Remove(cacheKey);
            }

            var src = IconLibrary.Body(iconKey);
            if (src == null) return null;
            try
            {
                var px = src.GetPixels32();
                var outPx = new Color32[px.Length];
                for (int i = 0; i < px.Length; i++)
                {
                    float shade = (px[i].r + px[i].g + px[i].b) / (255f * 3f);
                    float lift = Mathf.Clamp01(0.38f + shade * 0.82f);
                    outPx[i] = new Color32(
                        (byte)Mathf.Clamp(Mathf.RoundToInt(tint.r * 255f * lift), 0, 255),
                        (byte)Mathf.Clamp(Mathf.RoundToInt(tint.g * 255f * lift), 0, 255),
                        (byte)Mathf.Clamp(Mathf.RoundToInt(tint.b * 255f * lift), 0, 255),
                        px[i].a);
                }
                var tex = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false)
                {
                    name = "CSC_TintedBody_" + cacheKey,
                    wrapMode = TextureWrapMode.Clamp,
                    filterMode = src.filterMode,
                    hideFlags = HideFlags.HideAndDontSave
                };
                tex.SetPixels32(outPx);
                tex.Apply(false, false);
                _bodyTintCache[cacheKey] = tex;
                return tex;
            }
            catch (System.Exception)
            {
                return src;
            }
        }

        public static Visual ForSubcategory(string label)
        {
            if (_subToBody.TryGetValue(label, out string body)) return ForBody(body);
            // Special categories without one exact body.
            if (label == "Interplanetar" || label == "Interplanetary")
                return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapShips"), Color = RGB(224,184,77) };
            if (label == "Logistik" || label == "Logistics")
                return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapBase"), Color = RGB(102,187,106) };
            return new Visual { Icon = null, Color = RGB(150,150,150) };
        }

        // Branches: color + tab icon.
        public static Visual ForSparte(Sparte s)
        {
            switch (s)
            {
                case Sparte.Bemannt:
                    return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapShips"), Color = RGB(224,163,60) };
                case Sparte.UnbemannteErkundung:
                    return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapRover"), Color = RGB(79,195,247) };
                case Sparte.NetzwerkLogistik:
                    return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapCommunicationsRelay"), Color = RGB(102,187,106) };
                case Sparte.Wiederholbar:
                    return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapStation"), Color = RGB(186,104,200) };
                default:
                    return new Visual { Icon = null, Color = RGB(150,150,150) };
            }
        }

        /// <summary>Mission icon: first the icon selected by the catalog (icon=...), otherwise a
        /// fallback derived from all conditions/checks. This keeps composite missions from
        /// collapsing to the probe icon.</summary>
        public static Texture2D MissionIcon(MissionContract c)
        {
            if (c != null && !string.IsNullOrEmpty(c.IconKey))
            {
                var t = IconLibrary.UI(c.IconKey);
                if (t != null) return t;
            }
            return ForMissionShape(c);
        }

        private static Texture2D ForMissionShape(MissionContract c)
        {
            if (c == null || c.Bedingungen.Count == 0)
                return IconLibrary.UI("TrackingStation_ButtonMapProbe");

            bool hasMarker = false;
            bool hasBaseResource = false;
            bool hasDock = false;
            bool hasEva = false;
            bool hasFlyby = false;
            bool hasFleet = false;
            bool hasAtmo = false;
            bool hasDuration = false;
            bool hasCrew = c.HeimatSparte == Sparte.Bemannt;
            bool hasLanded = false;
            bool hasOrbit = false;

            foreach (var cond in c.Bedingungen)
            {
                if (cond.Type == ConditionType.COMPOSITE && cond.Checks.Count > 0)
                {
                    foreach (var ck in cond.Checks)
                    {
                        switch (ck.Kind)
                        {
                            case CheckKind.MARKER_LANDING:
                                hasMarker = true;
                                break;
                            case CheckKind.FUEL_MIN:
                            case CheckKind.RESOURCE_MIN:
                            case CheckKind.ORE_SURFACE:
                                hasBaseResource = true;
                                break;
                            case CheckKind.DOCK_ANY:
                            case CheckKind.DOCK_STATION:
                                hasDock = true;
                                break;
                            case CheckKind.EVA:
                                hasEva = true;
                                break;
                            case CheckKind.FLYBY:
                                hasFlyby = true;
                                break;
                            case CheckKind.RETURN_FROM_BODY:
                                hasLanded = true;
                                hasCrew = true;
                                break;
                            case CheckKind.VESSEL_COUNT:
                            case CheckKind.VESSEL_COUNT_INCLINATION:
                                hasFleet = true;
                                break;
                            case CheckKind.ATMO_FRACTION:
                            case CheckKind.SUBORBITAL:
                            case CheckKind.SUBORBITAL_ABOVE_ATMO:
                                hasAtmo = true;
                                break;
                            case CheckKind.LANDED:
                                hasLanded = true;
                                break;
                            case CheckKind.ORBIT_ABOVE:
                            case CheckKind.SITUATION:
                                hasOrbit = hasOrbit || ck.Situation == "ORBITING" || ck.Kind == CheckKind.ORBIT_ABOVE;
                                break;
                            case CheckKind.DURATION:
                            case CheckKind.HOLD:
                                hasDuration = true;
                                break;
                            case CheckKind.CREW_MIN:
                            case CheckKind.CREW_EXACT:
                                hasCrew = true;
                                break;
                        }
                    }
                    continue;
                }

                switch (cond.Type)
                {
                    case ConditionType.MARKER_LANDING:
                        hasMarker = true;
                        break;
                    case ConditionType.FUEL_ORBIT:
                    case ConditionType.ORE_SURFACE:
                        hasBaseResource = true;
                        break;
                    case ConditionType.DOCK:
                    case ConditionType.CREW_DURATION:
                        hasDock = true;
                        break;
                    case ConditionType.EVA:
                        hasEva = true;
                        break;
                    case ConditionType.FLYBY:
                        hasFlyby = true;
                        break;
                    case ConditionType.VESSEL_COUNT_ORBIT:
                        hasFleet = true;
                        break;
                    case ConditionType.ATMO_ENTRY:
                    case ConditionType.ALT_FRACTION_ATMO:
                    case ConditionType.ABOVE_ATMO_SUBORBITAL:
                        hasAtmo = true;
                        break;
                    case ConditionType.LANDED:
                        hasLanded = true;
                        break;
                    case ConditionType.ORBIT:
                    case ConditionType.ORBIT_HIGH:
                    case ConditionType.RENDEZVOUS:
                        hasOrbit = true;
                        break;
                }
            }

            if (hasMarker)
                return IconLibrary.UI("TrackingStation_ButtonMapFlag");
            if (hasBaseResource)
                return IconLibrary.UI("TrackingStation_ButtonMapBase");
            if (hasDock)
                return IconLibrary.UI("TrackingStation_ButtonMapStation");
            if (hasEva && !hasLanded)
                return IconLibrary.UI("TrackingStation_ButtonMapEVA");
            if (hasLanded)
                return IconLibrary.UI(hasDuration ? "TrackingStation_ButtonMapBase" : "TrackingStation_ButtonMapLander");
            if (hasFlyby)
                return IconLibrary.UI("TrackingStation_ButtonMapProbe");
            if (hasFleet)
                return IconLibrary.UI("TrackingStation_ButtonMapCommunicationsRelay");
            if (hasAtmo)
                return IconLibrary.UI("TrackingStation_ButtonMapAircraft");
            if (hasOrbit)
                return IconLibrary.UI(hasDuration && hasCrew
                    ? "TrackingStation_ButtonMapStation"
                    : hasCrew ? "TrackingStation_ButtonMapShips" : "TrackingStation_ButtonMapCommunicationsRelay");
            return IconLibrary.UI("TrackingStation_ButtonMapProbe");
        }

        // Legacy fallback for a single condition type.
        public static Texture2D ForCondition(ConditionType t)
        {
            switch (t)
            {
                case ConditionType.FLYBY:                return IconLibrary.UI("TrackingStation_ButtonMapProbe");
                case ConditionType.ORBIT:
                case ConditionType.ORBIT_HIGH:
                case ConditionType.FUEL_ORBIT:
                case ConditionType.RENDEZVOUS:           return IconLibrary.UI("TrackingStation_ButtonMapShips");
                case ConditionType.LANDED:               return IconLibrary.UI("TrackingStation_ButtonMapLander");
                case ConditionType.MARKER_LANDING:       return IconLibrary.UI("TrackingStation_ButtonMapFlag");
                case ConditionType.CREW_DURATION:
                case ConditionType.DOCK:                 return IconLibrary.UI("TrackingStation_ButtonMapStation");
                case ConditionType.EVA:                  return IconLibrary.UI("TrackingStation_ButtonMapEVA");
                case ConditionType.ORE_SURFACE:          return IconLibrary.UI("TrackingStation_ButtonMapBase");
                case ConditionType.VESSEL_COUNT_ORBIT:   return IconLibrary.UI("TrackingStation_ButtonMapCommunicationsRelay");
                case ConditionType.ALT_FRACTION_ATMO:
                case ConditionType.ABOVE_ATMO_SUBORBITAL:
                case ConditionType.ATMO_ENTRY:           return IconLibrary.UI("TrackingStation_ButtonMapAircraft");
                default:                                  return IconLibrary.UI("TrackingStation_ButtonMapProbe");
            }
        }
    }
}
