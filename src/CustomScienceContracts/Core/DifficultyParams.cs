using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CustomScienceContracts.Core
{
    /// <summary>Difficulty preset shown natively in KSP's own Difficulty Options screen — both at
    /// new-game creation and via the in-game pause-menu settings — instead of only inside the
    /// mod's own settings window. <see cref="Tuning.ApplyDifficulty"/> and
    /// <see cref="Tuning.SyncFromGameParameters"/> keep this field and the mod's window in sync in
    /// both directions. <see cref="HasPresets"/>/<see cref="SetDifficultyPreset"/> also make it
    /// follow KSP's own overall Easy/Normal/Moderate/Hard preset slider.</summary>
    public class CscDifficultyParams : GameParameters.CustomParameterNode
    {
        public override string Title => "Custom Science Contracts";
        public override string Section => "Custom Science Contracts";
        public override string DisplaySection => "Custom Science Contracts";
        public override int SectionOrder => 1;
        public override GameParameters.GameMode GameMode => GameParameters.GameMode.SCIENCE;
        public override bool HasPresets => true;

        [GameParameters.CustomStringParameterUI("Difficulty preset", toolTip =
            "Sets the repeatable cooldown, active mission limits and the science multiplier. " +
            "\"custom\" leaves the values from settings.cfg untouched. Follows KSP's own overall " +
            "difficulty preset above unless set to custom.", autoPersistance = true)]
        public string difficulty = "normal";

        public override IList ValidValues(MemberInfo member)
        {
            if (member.Name == nameof(difficulty))
                return new List<string> { "normal", "casual", "hard", "custom" };
            return null;
        }

        /// <summary>Called by KSP whenever the overall Easy/Normal/Moderate/Hard slider at the
        /// top of Difficulty Options changes, so our preset tracks it automatically. Moderate and
        /// Hard both map to our "hard" tier since we only offer three levels.</summary>
        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            switch (preset)
            {
                case GameParameters.Preset.Easy: difficulty = "casual"; break;
                case GameParameters.Preset.Normal: difficulty = "normal"; break;
                case GameParameters.Preset.Moderate: difficulty = "hard"; break;
                case GameParameters.Preset.Hard: difficulty = "hard"; break;
                // Custom: leave whatever the player already picked for our own difficulty.
            }
        }
    }
}
