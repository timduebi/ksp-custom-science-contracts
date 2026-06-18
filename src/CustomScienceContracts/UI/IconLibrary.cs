using System.Collections.Generic;
using System.IO;
using CustomScienceContracts.Core;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Loads bundled icons directly from the mod folder. GameDatabase is only a
    /// fallback so stock or cached textures with the same name do not accidentally win.</summary>
    public static class IconLibrary
    {
        private const string Base = "CustomScienceContracts/Icons";
        private static readonly Dictionary<string, Texture2D> _cache = new Dictionary<string, Texture2D>();
        private static readonly HashSet<string> _missingLogged = new HashSet<string>();

        public static Texture2D UI(string name) => Load(Base + "/UI/" + name);
        public static Texture2D Body(string name) => Load(Base + "/Bodies/" + name);
        public static Texture2D App(string name) => Load(Base + "/App/" + name);

        private static Texture2D Load(string url)
        {
            if (_cache.TryGetValue(url, out var t))
            {
                if (t != null) return t;
                _cache.Remove(url);
                Log.V($"Icon cache refreshed because texture was null/destroyed: {url}");
            }

            Texture2D tex = null;
            try
            {
                tex = LoadPngFromDisk(url) ??
                      (GameDatabase.Instance != null ? GameDatabase.Instance.GetTexture(url, false) : null);
            }
            catch (System.Exception e)
            {
                Log.V($"Icon load failed: {url} ({e.Message})");
                tex = null;
            }

            if (tex != null)
            {
                _cache[url] = tex;
                _missingLogged.Remove(url);
                return tex;
            }

            if (_missingLogged.Add(url))
                Log.V($"Icon key returned no texture: {url}");
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
