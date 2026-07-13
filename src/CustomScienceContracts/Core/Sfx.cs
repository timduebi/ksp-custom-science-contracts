using UnityEngine;

namespace CustomScienceContracts.Core
{
    /// <summary>Plays the bundled notification chime. The clip is loaded lazily from
    /// GameData/CustomScienceContracts/Sounds/; a missing clip degrades to silence.</summary>
    public static class Sfx
    {
        private const string ReadyClipUrl = "CustomScienceContracts/Sounds/csc_ready";

        private static AudioSource _source;
        private static AudioClip _ready;
        private static bool _loadTried;

        /// <summary>Soft chime when a mission becomes claimable.</summary>
        public static void PlayReady()
        {
            if (!Tuning.PlaySounds) return;
            try
            {
                if (!_loadTried)
                {
                    _loadTried = true;
                    _ready = GameDatabase.Instance != null
                        ? GameDatabase.Instance.GetAudioClip(ReadyClipUrl)
                        : null;
                    if (_ready == null) Log.V($"Notification clip not found: {ReadyClipUrl}");
                }
                if (_ready == null) return;

                if (_source == null)
                {
                    var go = new GameObject("CSC_Sfx");
                    Object.DontDestroyOnLoad(go);
                    _source = go.AddComponent<AudioSource>();
                    _source.spatialBlend = 0f;   // plain UI sound, not positional
                }
                _source.PlayOneShot(_ready, GameSettings.UI_VOLUME);
            }
            catch (System.Exception) { }
        }
    }
}
