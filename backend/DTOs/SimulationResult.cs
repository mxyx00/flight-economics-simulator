namespace backend.DTOs;

public class SimulationResult
{
    public string AircraftName { get; set; } = "";

    public string DepartureAirport { get; set; } = "";

    public string ArrivalAirport { get; set; } = "";

    public double DistanceNauticalMiles { get; set; }

    public int PassengerLoadPercent { get; set; }

    public int PassengerCount { get; set; }

    public int MaximumPassengers { get; set; }

    public int CargoLoadPercent { get; set; }

    public double CargoWeightKg { get; set; }

    public double MaximumCargoWeightKg { get; set; }

    public int EstimatedFlightTimeMinutes { get; set; }
}