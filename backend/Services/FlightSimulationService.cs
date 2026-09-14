using backend.Data;
using backend.DTOs;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class FlightSimulationService
{

    private const double JetFuelDensityKgPerLiter =
        0.804;

    private const double LitersPerUsGallon =
        3.785411784;

    private const double CheckedBagFeePerPassenger =
        30.0;

    private static readonly Aircraft Boeing737800 = new()
    {
        Name = "Boeing 737-800",

        MaximumPassengers = 189,

        CruiseSpeedKnots = 450,

        MaximumCargoWeightKg = 5000,

        FlightTimeAllowanceHours = 0.6,

        ClimbAndDescentDistanceNm = 150,

        OperatingEmptyWeightKg = 41720,

        MaximumTaxiWeightKg = 79240,

        MaximumTakeoffWeightKg = 79010,

        MaximumLandingWeightKg = 66360,

        MaximumZeroFuelWeightKg = 62730,

        MaximumFuelKg = 20897,

        // Simulator assumptions
        AveragePassengerWeightKg = 86,

        CheckedBagWeightKg = 14,

        TaxiFuelKg = 250,

        ClimbFuelKg = 1100,

        CruiseFuelBurnKgPerHour = 2400,

        DescentFuelKg = 300,

        ReserveFuelBurnKgPerHour = 1800,

        ReserveTimeHours = 0.5,

        ReferenceCruiseWeightKg = 65000,

        WeightFuelFlowExponent = 0.7,



    };

    private readonly AppDbContext _context;
    private readonly FuelPriceService _fuelPriceService;

    public FlightSimulationService(
        AppDbContext context,
        FuelPriceService fuelPriceService)
    {
        _context = context;
        _fuelPriceService = fuelPriceService;
    }

    public async Task<SimulationResult> SimulateAsync(
        string departureCode,
        string arrivalCode,
        int passengerLoadPercent,
        int cargoLoadPercent)
    {
        var departure = await _context.Airports
            .AsNoTracking()
            .SingleOrDefaultAsync(
                airport => airport.Code == departureCode
            );

        var arrival = await _context.Airports
            .AsNoTracking()
            .SingleOrDefaultAsync(
                airport => airport.Code == arrivalCode
            );

        if (departure == null)
        {
            throw new KeyNotFoundException(
                $"Departure airport '{departureCode}' was not found."
            );
        }

        if (arrival == null)
        {
            throw new KeyNotFoundException(
                $"Arrival airport '{arrivalCode}' was not found."
            );
        }

        double distanceNauticalMiles =
            CalculateGreatCircleDistance(
                departure.Latitude,
                departure.Longitude,
                arrival.Latitude,
                arrival.Longitude
            );

        int passengerCount =
            (int)Math.Round(
                Boeing737800.MaximumPassengers
                * passengerLoadPercent
                / 100.0,
                MidpointRounding.AwayFromZero
            );

        double passengerWeightKg =
            passengerCount
            * Boeing737800.AveragePassengerWeightKg;

        double checkedBaggageWeightKg =
            passengerCount
            * Boeing737800.CheckedBagWeightKg;

        double cargoWeightKg =
            Boeing737800.MaximumCargoWeightKg
            * cargoLoadPercent
            / 100.0;

        double payloadWeightKg =
            passengerWeightKg
            + checkedBaggageWeightKg
            + cargoWeightKg;

        double zeroFuelWeightKg =
            Boeing737800.OperatingEmptyWeightKg
            + payloadWeightKg;

        if (
            zeroFuelWeightKg
            > Boeing737800.MaximumZeroFuelWeightKg
        )
        {
            throw new InvalidOperationException(
                "The passenger and cargo load exceeds the aircraft's maximum zero-fuel weight. Reduce passengers or cargo."
            );
        }

        double cruiseDistanceNm =
            Math.Max(
                0,
                distanceNauticalMiles
                - Boeing737800.ClimbAndDescentDistanceNm
            );

        double estimatedFlightTimeHours =
            cruiseDistanceNm
            / Boeing737800.CruiseSpeedKnots
            + Boeing737800.FlightTimeAllowanceHours;

        int estimatedFlightTimeMinutes =
            (int)Math.Round(
                estimatedFlightTimeHours * 60
            );

        var fuelPlan =
            CalculateFuelPlan(
                zeroFuelWeightKg,
                cruiseDistanceNm
            );

        if (
            fuelPlan.RequiredFuelKg
            > Boeing737800.MaximumFuelKg
        )
        {
            throw new InvalidOperationException(
                "The estimated fuel requirement exceeds the aircraft's usable fuel capacity."
            );
        }

        if (
            fuelPlan.RampWeightKg
            > Boeing737800.MaximumTaxiWeightKg
        )
        {
            throw new InvalidOperationException(
                "The aircraft exceeds maximum taxi weight."
            );
        }

        if (
            fuelPlan.TakeoffWeightKg
            > Boeing737800.MaximumTakeoffWeightKg
        )
        {
            throw new InvalidOperationException(
                "The aircraft exceeds maximum takeoff weight."
            );
        }

        if (
            fuelPlan.LandingWeightKg
            > Boeing737800.MaximumLandingWeightKg
        )
        {
            throw new InvalidOperationException(
                "The aircraft exceeds maximum landing weight."
            );
        }

        FuelPrice latestFuelPrice =
         await _fuelPriceService
        .GetLatestJetFuelPriceAsync();

        double tripFuelGallons =
            KilogramsToGallons(
                fuelPlan.TripFuelKg
            );

        double requiredFuelGallons =
            KilogramsToGallons(
                fuelPlan.RequiredFuelKg
            );

        double fuelBurnCost =
            tripFuelGallons
            * latestFuelPrice.PricePerGallon;

        double fuelLoadValue =
            requiredFuelGallons
            * latestFuelPrice.PricePerGallon;

        double checkedBagRevenue =
            passengerCount
            * CheckedBagFeePerPassenger;

        return new SimulationResult
        {
            AircraftName =
                Boeing737800.Name,

            DepartureAirport =
                departure.Code,

            ArrivalAirport =
                arrival.Code,

            DistanceNauticalMiles =
                Math.Round(distanceNauticalMiles),

            PassengerLoadPercent =
                passengerLoadPercent,

            PassengerCount =
                passengerCount,

            MaximumPassengers =
                Boeing737800.MaximumPassengers,

            PassengerWeightKg =
                Math.Round(passengerWeightKg),

            CheckedBaggageWeightKg =
                Math.Round(checkedBaggageWeightKg),

            CargoLoadPercent =
                cargoLoadPercent,

            CargoWeightKg =
                Math.Round(cargoWeightKg),

            MaximumCargoWeightKg =
                Boeing737800.MaximumCargoWeightKg,

            PayloadWeightKg =
                Math.Round(payloadWeightKg),

            OperatingEmptyWeightKg =
                Boeing737800.OperatingEmptyWeightKg,

            ZeroFuelWeightKg =
                Math.Round(zeroFuelWeightKg),

            MaximumZeroFuelWeightKg =
                Boeing737800.MaximumZeroFuelWeightKg,

            EstimatedFlightTimeMinutes =
                estimatedFlightTimeMinutes,

            TripFuelKg =
                Math.Round(fuelPlan.TripFuelKg),

            ReserveFuelKg =
                Math.Round(fuelPlan.ReserveFuelKg),

            ContingencyFuelKg =
                Math.Round(fuelPlan.ContingencyFuelKg),

            RequiredFuelKg =
                Math.Round(fuelPlan.RequiredFuelKg),

            MaximumFuelKg =
                Boeing737800.MaximumFuelKg,

            FuelLoadPercent =
                Math.Round(
                    fuelPlan.RequiredFuelKg
                    / Boeing737800.MaximumFuelKg
                    * 100,
                    1
                ),

            RampWeightKg =
                Math.Round(fuelPlan.RampWeightKg),

            TakeoffWeightKg =
                Math.Round(fuelPlan.TakeoffWeightKg),

            MaximumTakeoffWeightKg =
                Boeing737800.MaximumTakeoffWeightKg,

            LandingWeightKg =
                Math.Round(fuelPlan.LandingWeightKg),

            MaximumLandingWeightKg =
                Boeing737800.MaximumLandingWeightKg,

            TripFuelGallons =
                Math.Round(
                    tripFuelGallons,
                    1
                ),

            RequiredFuelGallons =
                Math.Round(
                    requiredFuelGallons,
                    1
                ),

            FuelPricePerGallon =
                Math.Round(
                    latestFuelPrice.PricePerGallon,
                    3
                ),

            FuelPriceDate =
                latestFuelPrice.Date,

            FuelPriceSource =
                latestFuelPrice.Source,

            FuelBurnCost =
                Math.Round(
                    fuelBurnCost,
                    2
                ),

            FuelLoadValue =
                Math.Round(
                    fuelLoadValue,
                    2
                ),

            CheckedBagRevenue =
                Math.Round(
                    checkedBagRevenue,
                    2
                )
        };
    }

    private static FuelPlan CalculateFuelPlan(
        double zeroFuelWeightKg,
        double cruiseDistanceNm)
    {
        double fuelGuessKg =
            2500
            + (
                cruiseDistanceNm
                / Boeing737800.CruiseSpeedKnots
                * Boeing737800.CruiseFuelBurnKgPerHour
            );

        for (int iteration = 0; iteration < 50; iteration++)
        {
            var calculation =
                CalculateFuelForGuess(
                    zeroFuelWeightKg,
                    cruiseDistanceNm,
                    fuelGuessKg
                );

            double difference =
                Math.Abs(
                    calculation.RequiredFuelKg
                    - fuelGuessKg
                );

            if (difference < 0.5)
            {
                fuelGuessKg =
                    calculation.RequiredFuelKg;

                break;
            }

            fuelGuessKg =
                (
                    fuelGuessKg
                    + calculation.RequiredFuelKg
                )
                / 2.0;
        }

        return CalculateFuelForGuess(
            zeroFuelWeightKg,
            cruiseDistanceNm,
            fuelGuessKg
        );
    }

    private static FuelPlan CalculateFuelForGuess(
        double zeroFuelWeightKg,
        double cruiseDistanceNm,
        double fuelGuessKg)
    {
        double rampWeightKg =
            zeroFuelWeightKg
            + fuelGuessKg;

        double taxiFuelKg =
            Boeing737800.TaxiFuelKg
            * CalculateWeightFactor(
                rampWeightKg,
                0.3
            );

        double takeoffWeightKg =
            rampWeightKg
            - taxiFuelKg;

        double climbFuelKg =
            Boeing737800.ClimbFuelKg
            * CalculateWeightFactor(
                takeoffWeightKg,
                0.6
            );

        double cruiseStartWeightKg =
            takeoffWeightKg
            - climbFuelKg;

        double cruiseFuelKg =
            CalculateCruiseFuel(
                cruiseStartWeightKg,
                cruiseDistanceNm
            );

        double weightAfterCruiseKg =
            cruiseStartWeightKg
            - cruiseFuelKg;

        double descentFuelKg =
            Boeing737800.DescentFuelKg
            * CalculateWeightFactor(
                weightAfterCruiseKg,
                0.4
            );

        double tripFuelKg =
            taxiFuelKg
            + climbFuelKg
            + cruiseFuelKg
            + descentFuelKg;

        double projectedLandingWeightKg =
            zeroFuelWeightKg
            + Math.Max(
                0,
                fuelGuessKg - tripFuelKg
            );

        double reserveFuelKg =
            Boeing737800.ReserveFuelBurnKgPerHour
            * Boeing737800.ReserveTimeHours
            * CalculateWeightFactor(
                projectedLandingWeightKg,
                Boeing737800.WeightFuelFlowExponent
            );

        double contingencyFuelKg =
            tripFuelKg * 0.05;

        double requiredFuelKg =
            tripFuelKg
            + reserveFuelKg
            + contingencyFuelKg;

        double landingWeightKg =
            zeroFuelWeightKg
            + reserveFuelKg
            + contingencyFuelKg;

        return new FuelPlan
        {
            TripFuelKg = tripFuelKg,

            ReserveFuelKg = reserveFuelKg,

            ContingencyFuelKg =
                contingencyFuelKg,

            RequiredFuelKg =
                requiredFuelKg,

            RampWeightKg =
                rampWeightKg,

            TakeoffWeightKg =
                takeoffWeightKg,

            LandingWeightKg =
                landingWeightKg
        };
    }

    private static double CalculateCruiseFuel(
        double startingWeightKg,
        double cruiseDistanceNm)
    {
        const int segments = 20;

        double cruiseTimeHours =
            cruiseDistanceNm
            / Boeing737800.CruiseSpeedKnots;

        double segmentTimeHours =
            cruiseTimeHours / segments;

        double currentWeightKg =
            startingWeightKg;

        double totalCruiseFuelKg = 0;

        for (int i = 0; i < segments; i++)
        {
            double weightFactor =
                CalculateWeightFactor(
                    currentWeightKg,
                    Boeing737800.WeightFuelFlowExponent
                );

            double fuelFlowKgPerHour =
                Boeing737800.CruiseFuelBurnKgPerHour
                * weightFactor;

            double segmentFuelKg =
                fuelFlowKgPerHour
                * segmentTimeHours;

            totalCruiseFuelKg +=
                segmentFuelKg;

            currentWeightKg -=
                segmentFuelKg;
        }

        return totalCruiseFuelKg;
    }

    private static double CalculateWeightFactor(
        double aircraftWeightKg,
        double exponent)
    {
        return Math.Pow(
            aircraftWeightKg
            / Boeing737800.ReferenceCruiseWeightKg,
            exponent
        );
    }

    private static double CalculateGreatCircleDistance(
        double latitude1,
        double longitude1,
        double latitude2,
        double longitude2)
    {
        const double earthRadiusNauticalMiles =
            3440.065;

        double lat1 =
            DegreesToRadians(latitude1);

        double lat2 =
            DegreesToRadians(latitude2);

        double deltaLatitude =
            DegreesToRadians(
                latitude2 - latitude1
            );

        double deltaLongitude =
            DegreesToRadians(
                longitude2 - longitude1
            );

        double a =
            Math.Pow(
                Math.Sin(deltaLatitude / 2),
                2
            )
            +
            Math.Cos(lat1)
            * Math.Cos(lat2)
            * Math.Pow(
                Math.Sin(deltaLongitude / 2),
                2
            );

        double c =
            2
            * Math.Atan2(
                Math.Sqrt(a),
                Math.Sqrt(1 - a)
            );

        return earthRadiusNauticalMiles * c;
    }

    private static double DegreesToRadians(
        double degrees)
    {
        return degrees * Math.PI / 180;
    }

    private class FuelPlan
    {
        public double TripFuelKg { get; set; }

        public double ReserveFuelKg { get; set; }

        public double ContingencyFuelKg { get; set; }

        public double RequiredFuelKg { get; set; }

        public double RampWeightKg { get; set; }

        public double TakeoffWeightKg { get; set; }

        public double LandingWeightKg { get; set; }

        

    }

    private static double KilogramsToGallons(
        double kilograms)
    {
        double liters =
            kilograms / JetFuelDensityKgPerLiter;

        return liters / LitersPerUsGallon;
    }    
}