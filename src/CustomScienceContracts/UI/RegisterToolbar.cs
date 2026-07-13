using CustomScienceContracts.Core;
using ToolbarControl_NS;
using UnityEngine;

namespace CustomScienceContracts.UI
{
    /// <summary>Registers the mod with ToolbarControl as early as the game process allows (once,
    /// at the main menu), independent of any save being loaded. This only reserves the mod's
    /// namespace/display name for ToolbarControl's own settings page; the actual buttons are
    /// created per Science-mode game by <see cref="CscUI"/>, matching the registration pattern
    /// documented at https://github.com/linuxgurugamer/ToolbarControl.</summary>
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        private void Start() => ToolbarControl.RegisterMod(ModInfo.Name, "Custom Science Contracts");
    }
}
