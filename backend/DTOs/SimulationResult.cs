namespace backend.DTOs;

public class SimulationResult
{
    public string DepartureAirport { get; set; } = "";

    public string ArrivalAirport { get; set; } = "";

    public double DistanceNauticalMiles { get; set; }

    public int PassengerLoadPercent { get; set; }

    public int CargoLoadPercent { get; set; }
}