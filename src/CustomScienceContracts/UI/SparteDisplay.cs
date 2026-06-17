using CustomScienceContracts.Model;

namespace CustomScienceContracts.UI
{
    /// <summary>Kreative, beschreibende Anzeigenamen der Sparten. Icons/Farben kommen aus BodyVisual.</summary>
    public static class SparteDisplay
    {
        public static string Name(Sparte s)
        {
            switch (s)
            {
                case Sparte.Bemannt:             return "Pioniere";
                case Sparte.UnbemannteErkundung: return "Robotische Erkunder";
                case Sparte.NetzwerkLogistik:    return "Versorgungsnetz";
                case Sparte.Wiederholbar:        return "Daueraufträge";
                default:                          return s.ToString();
            }
        }
    }
}
