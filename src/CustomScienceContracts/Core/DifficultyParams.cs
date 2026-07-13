using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CustomScienceContracts.Core
{
    /// <summary>Difficulty preset shown natively in KSP's own Difficulty Options screen — both at
    /// new-game creation and via the in-game pause-menu settings — instead of only inside the
    /// mod's own settings window. <see cref="Tuning.ApplyDifficulty"/> and
    /// <see cref="Tuning.SyncFromGameParameters"/> keep this field and the mod's window in sync in
    /// both directions.</summary>
    public class CscDifficultyParams : GameParameters.CustomParameterNode
    {
        public override string Title => "Custom Science Contracts";
        public override string Section => "Custom Science Contracts";
        public override string DisplaySection => "Custom Science Contracts";
        public override int SectionOrder => 1;
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.SCIENCE;
        public override bool HasPresets => false;

        [GameParameters.CustomStringParameterUI("Difficulty preset", toolTip =
            "Sets the repeatable cooldown, active mission limits and the science multiplier. " +
            "\"custom\" leaves the values from settings.cfg untouched.", autoPersistance = true)]
        public string difficulty = "custom";

        public override IList ValidValues(MemberInfo member)
        {
            if (member.Name == nameof(difficulty))
                return new List<string> { "custom", "casual", "normal", "hard" };
            return null;
        }
    }
}
