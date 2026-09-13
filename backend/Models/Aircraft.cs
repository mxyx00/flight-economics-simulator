namespace backend.Models;

public class Aircraft
{
    public string Name { get; set; } = "";

    public int MaximumPassengers { get; set; }

    public double CruiseSpeedKnots { get; set; }

    public double MaximumCargoWeightKg { get; set; }

    public double FlightTimeAllowanceHours { get; set; }

    public double ClimbAndDescentDistanceNm { get; set; }

    public double OperatingEmptyWeightKg { get; set; }

    public double MaximumTaxiWeightKg { get; set; }

    public double MaximumTakeoffWeightKg { get; set; }

    public double MaximumLandingWeightKg { get; set; }

    public double MaximumZeroFuelWeightKg { get; set; }

    public double MaximumFuelKg { get; set; }

    public double AveragePassengerWeightKg { get; set; }

    public double CheckedBagWeightKg { get; set; }

    public double TaxiFuelKg { get; set; }

    public double ClimbFuelKg { get; set; }

    public double CruiseFuelBurnKgPerHour { get; set; }

    public double DescentFuelKg { get; set; }

    public double ReserveFuelBurnKgPerHour { get; set; }

    public double ReserveTimeHours { get; set; }

    public double ReferenceCruiseWeightKg { get; set; }

    public double WeightFuelFlowExponent { get; set; }
}