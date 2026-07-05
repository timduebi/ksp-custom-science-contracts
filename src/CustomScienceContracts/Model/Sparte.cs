namespace CustomScienceContracts.Model
{
    /// <summary>The main branches. Wiederholbar is dynamic: after its first completion a
    /// repeatable contract moves here from its home branch. Stationen (since 0.6.1) holds the
    /// station/base/depot infrastructure chains. Enum names are stable config/save keys.</summary>
    public enum Sparte
    {
        Bemannt,
        UnbemannteErkundung,
        NetzwerkLogistik,
        Wiederholbar,
        Stationen
    }
}
