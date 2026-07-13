using System;
using UnityEngine;
using KSP.UI.Screens;
using System.Linq;

namespace ToolbarControl_NS
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    public class RegisterToolbarBlizzyOptions : MonoBehaviour
    {

        void Start()
        {
            ToolbarControl.RegisterMod(BlizzyOptions.MODID, BlizzyOptions.MODNAME, false, true, false);
        }
    }
}
