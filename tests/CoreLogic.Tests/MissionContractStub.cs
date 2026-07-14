namespace CustomScienceContracts.Model;

public sealed class MissionContract
{
    public string Id = "";
    public MissionStatus Status;
    public int TotalCompletions;
    public double FirstCompletedUT = -1.0;
}
