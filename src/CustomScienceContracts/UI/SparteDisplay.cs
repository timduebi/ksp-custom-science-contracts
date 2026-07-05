using CustomScienceContracts.Model;

namespace CustomScienceContracts.UI
{
    /// <summary>Player-facing tab names. Icons and colors come from BodyVisual.</summary>
    public static class SparteDisplay
    {
        public static string Name(Sparte s)
        {
            switch (s)
            {
                case Sparte.Bemannt:             return "Pioneers";
                case Sparte.UnbemannteErkundung: return "Robotic Explorers";
                case Sparte.Stationen:           return "Stations";
                case Sparte.NetzwerkLogistik:    return "Lifelines";
                case Sparte.Wiederholbar:        return "Repeatable";
                default:                          return s.ToString();
            }
        }
    }
}
