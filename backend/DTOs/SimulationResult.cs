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

    public double PassengerWeightKg { get; set; }

    public double CheckedBaggageWeightKg { get; set; }

    public int CargoLoadPercent { get; set; }

    public double CargoWeightKg { get; set; }

    public double MaximumCargoWeightKg { get; set; }

    public double PayloadWeightKg { get; set; }

    public double OperatingEmptyWeightKg { get; set; }

    public double ZeroFuelWeightKg { get; set; }

    public double MaximumZeroFuelWeightKg { get; set; }

    public int EstimatedFlightTimeMinutes { get; set; }

    public double TripFuelKg { get; set; }

    public double ReserveFuelKg { get; set; }

    public double ContingencyFuelKg { get; set; }

    public double RequiredFuelKg { get; set; }

    public double MaximumFuelKg { get; set; }

    public double FuelLoadPercent { get; set; }

    public double RampWeightKg { get; set; }

    public double TakeoffWeightKg { get; set; }

    public double MaximumTakeoffWeightKg { get; set; }

    public double LandingWeightKg { get; set; }

    public double MaximumLandingWeightKg { get; set; }
}