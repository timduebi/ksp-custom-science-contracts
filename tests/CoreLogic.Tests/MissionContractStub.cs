namespace CustomScienceContracts.Model;

using System.Collections.Generic;

public sealed class MissionContract
{
    public string Id = "";
    public MissionStatus Status;
    public int TotalCompletions;
    public double FirstCompletedUT = -1.0;
    public List<string> Voraussetzungen = new();
}
