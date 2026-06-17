namespace CustomScienceContracts.Model
{
    /// <summary>Die vier Hauptsparten. Wiederholbar ist dynamisch: ein Repeatable-Contract
    /// wandert nach Erstabschluss aus seiner Heimatsparte hierher.</summary>
    public enum Sparte
    {
        Bemannt,
        UnbemannteErkundung,
        NetzwerkLogistik,
        Wiederholbar
    }
}
