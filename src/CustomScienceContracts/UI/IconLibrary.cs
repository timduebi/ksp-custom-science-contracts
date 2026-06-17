using System.Collections.Generic;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Laedt die mitgelieferten Ztheme-/Planet-Icons aus GameData ueber die GameDatabase
    /// (URL = Pfad ohne Endung). Gecacht; fehlt eine Textur, wird null geliefert und die UI
    /// faellt auf einfache Farbflaechen zurueck.</summary>
    public static class IconLibrary
    {
        private const string Base = "CustomScienceContracts/Icons";
        private static readonly Dictionary<string, Texture2D> _cache = new Dictionary<string, Texture2D>();

        public static Texture2D UI(string name) => Load(Base + "/UI/" + name);
        public static Texture2D Body(string name) => Load(Base + "/Bodies/" + name);
        public static Texture2D App(string name) => Load(Base + "/App/" + name);

        private static Texture2D Load(string url)
        {
            if (_cache.TryGetValue(url, out var t)) return t;
            Texture2D tex = null;
            try { tex = GameDatabase.Instance != null ? GameDatabase.Instance.GetTexture(url, false) : null; }
            catch { tex = null; }
            _cache[url] = tex;
            return tex;
        }
    }
}
