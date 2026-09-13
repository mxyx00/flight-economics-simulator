namespace backend.Models;

public class Aircraft
{
    public string Name { get; set; } = "";

    public int MaximumPassengers { get; set; }

    public double CruiseSpeedKnots { get; set; }

    public double MaximumCargoWeightKg { get; set; }

    public double FlightTimeAllowanceHours { get; set; }
}