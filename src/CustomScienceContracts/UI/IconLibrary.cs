using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Laedt die mitgelieferten Icons direkt aus unserem Modordner. GameDatabase bleibt nur
    /// Fallback, damit gleichnamige Stock- oder Cache-Texturen nicht versehentlich gewinnen.</summary>
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
            try { tex = LoadPngFromDisk(url) ?? (GameDatabase.Instance != null ? GameDatabase.Instance.GetTexture(url, false) : null); }
            catch { tex = null; }
            _cache[url] = tex;
            return tex;
        }

        private static Texture2D LoadPngFromDisk(string url)
        {
            string rel = url.Replace('/', Path.DirectorySeparatorChar) + ".png";
            string path = Path.Combine(KSPUtil.ApplicationRootPath, "GameData", rel);
            if (!File.Exists(path)) return null;

            byte[] bytes = File.ReadAllBytes(path);
            var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false)
            {
                name = url,
                wrapMode = TextureWrapMode.Clamp,
                filterMode = FilterMode.Bilinear,
                hideFlags = HideFlags.HideAndDontSave
            };

            if (ImageConversion.LoadImage(tex, bytes, false)) return tex;
            Object.Destroy(tex);
            return null;
        }
    }
}
