using System.Collections.Generic;
using CustomScienceContracts.Model;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Visuelle Zuordnung (Icon + Farbe) fuer Koerper, Sparten, Unterkategorien und
    /// Bedingungstypen. Planet-Icons sind optisch passende Stock/OPM-Icons, eingefaerbt auf die
    /// echte Koerperfarbe. Mission-Icons stammen aus dem TrackingStation_ButtonMap-Set.</summary>
    public static class BodyVisual
    {
        public struct Visual { public Texture2D Icon; public Color Color; }

        private static Color RGB(int r, int g, int b) => new Color(r / 255f, g / 255f, b / 255f);

        // body-name -> (planet-icon-key, farbe)
        private static readonly Dictionary<string, (string icon, Color col)> _bodies =
            new Dictionary<string, (string, Color)>
        {
            // Stern
            { "Sun",     ("pol",    RGB(255,210,80)) },
            // Planeten
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
            // Mars-Monde
            { "Phobos", ("gilly", RGB(110,102,90)) },
            { "Deimos", ("gilly", RGB(124,114,100)) },
            // Jupiter-Monde
            { "Io",       ("pol",  RGB(215,194,75)) },
            { "Europa",   ("vall", RGB(207,216,224)) },
            { "Ganymede", ("tylo", RGB(142,133,118)) },
            { "Callisto", ("tylo", RGB(111,106,100)) },
            { "Amalthea", ("gilly",RGB(160,85,64)) },
            { "Thebe",    ("gilly",RGB(124,116,104)) },
            // Saturn-Monde
            { "Titan",     ("tekto", RGB(215,154,60)) },
            { "Enceladus", ("hale",  RGB(230,238,242)) },
            { "Rhea",      ("slate", RGB(194,200,204)) },
            { "Iapetus",   ("slate", RGB(154,139,112)) },
            { "Dione",     ("vall",  RGB(200,204,208)) },
            { "Tethys",    ("vall",  RGB(212,218,222)) },
            { "Mimas",     ("ovok",  RGB(178,178,178)) },
            { "Hyperion",  ("gilly", RGB(168,152,120)) },
            { "Phoebe",    ("gilly", RGB(92,88,79)) },
            // Uranus-Monde
            { "Titania", ("vall",  RGB(182,174,164)) },
            { "Oberon",  ("slate", RGB(153,142,130)) },
            { "Ariel",   ("vall",  RGB(200,200,200)) },
            { "Umbriel", ("mun",   RGB(110,106,100)) },
            { "Miranda", ("ovok",  RGB(174,178,182)) },
            { "Puck",    ("gilly", RGB(106,100,92)) },
            // Neptun-Monde
            { "Triton",  ("vall",  RGB(214,200,194)) },
            { "Nereid",  ("gilly", RGB(138,134,126)) },
            { "Proteus", ("gilly", RGB(94,90,82)) },
            // Pluto-Mond
            { "Charon",  ("mun",   RGB(154,147,138)) },
            // Asteroiden
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

        // Unterkategorie-Label (UI) -> Koerpername fuer Icon/Farbe
        private static readonly Dictionary<string, string> _subToBody = new Dictionary<string, string>
        {
            { "Erde", "Earth" }, { "Luna", "Moon" }, { "Mars", "Mars" }, { "Venus", "Venus" },
            { "Merkur", "Mercury" }, { "Jupiter", "Jupiter" }, { "Saturn", "Saturn" },
            { "Uranus", "Uranus" }, { "Neptun", "Neptune" }, { "Pluto", "Pluto" },
            { "Asteroiden", "Ceres" },
        };

        /// <summary>Koerpername hinter einem Unterkategorie-Label (oder null).</summary>
        public static string SubcatBodyName(string label) =>
            _subToBody.TryGetValue(label, out string b) ? b : null;

        // interner CelestialBody.name / Label -> deutscher Anzeigename
        private static readonly Dictionary<string, string> _display = new Dictionary<string, string>
        {
            { "Earth", "Erde" }, { "Moon", "Luna" }, { "Mercury", "Merkur" }, { "Venus", "Venus" },
            { "Mars", "Mars" }, { "Jupiter", "Jupiter" }, { "Saturn", "Saturn" }, { "Uranus", "Uranus" },
            { "Neptune", "Neptun" }, { "Pluto", "Pluto" }, { "Sun", "Sonne" },
            { "Ganymede", "Ganymed" }, { "Callisto", "Kallisto" }, { "Europa", "Europa" }, { "Io", "Io" },
            { "Titan", "Titan" }, { "Charon", "Charon" }, { "Triton", "Triton" },
        };

        /// <summary>Deutscher Anzeigename eines (internen) Koerper- oder Unterkategorie-Namens.
        /// Faellt auf die Eingabe zurueck, wenn nichts hinterlegt ist.</summary>
        public static string DisplayName(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return _display.TryGetValue(name, out string d) ? d : name;
        }

        public static Visual ForBody(string bodyName)
        {
            if (bodyName != null && _bodies.TryGetValue(bodyName, out var v))
                return new Visual { Icon = IconLibrary.Body(v.icon), Color = v.col };
            return new Visual { Icon = IconLibrary.Body("mun"), Color = RGB(150,150,150) };
        }

        public static Visual ForSubcategory(string label)
        {
            if (_subToBody.TryGetValue(label, out string body)) return ForBody(body);
            // Sonderkategorien ohne Koerper
            if (label == "Interplanetar")
                return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapShips"), Color = RGB(224,184,77) };
            if (label == "Logistik")
                return new Visual { Icon = IconLibrary.UI("TrackingStation_ButtonMapBase"), Color = RGB(102,187,106) };
            return new Visual { Icon = null, Color = RGB(150,150,150) };
        }

        // Sparten: Farbe (frei gewaehlt) + Tab-Icon
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

        /// <summary>Icon einer Mission: das pro Mission gewaehlte (icon=...), sonst Fallback nach
        /// erstem Bedingungstyp. So zeigt jede Mission ein passendes Symbol statt nur "Sonde".</summary>
        public static Texture2D MissionIcon(MissionContract c)
        {
            if (c != null && !string.IsNullOrEmpty(c.IconKey))
            {
                var t = IconLibrary.UI(c.IconKey);
                if (t != null) return t;
            }
            return ForCondition(c != null && c.Bedingungen.Count > 0 ? c.Bedingungen[0].Type : ConditionType.ORBIT);
        }

        // Mission-Icon nach (erstem) Bedingungstyp
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
