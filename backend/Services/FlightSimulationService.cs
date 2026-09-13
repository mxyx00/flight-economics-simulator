using backend.Data;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;
using backend.Models;

// using haversine formula to calculate distance
// using nautical miles

namespace backend.Services;

public class FlightSimulationService
{
    private static readonly Aircraft Boeing737800 = new()
        {
            Name = "Boeing 737-800",
            MaximumPassengers = 189,
            CruiseSpeedKnots = 450,
            MaximumCargoWeightKg = 5000,
            FlightTimeAllowanceHours = 0.6 // includes taxi takeoff and landing
        };
    private readonly AppDbContext _context;

    public FlightSimulationService(AppDbContext context)
    {
        _context = context;
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

        double cargoWeightKg =
            Boeing737800.MaximumCargoWeightKg
            * cargoLoadPercent
            / 100.0;

        double estimatedFlightTimeHours =
            distanceNauticalMiles
            / Boeing737800.CruiseSpeedKnots
            + Boeing737800.FlightTimeAllowanceHours;

        int estimatedFlightTimeMinutes =
            (int)Math.Round(
                estimatedFlightTimeHours * 60
            );

       return new SimulationResult
        {
            AircraftName = Boeing737800.Name,

            DepartureAirport = departure.Code,
            ArrivalAirport = arrival.Code,

            DistanceNauticalMiles = Math.Round(distanceNauticalMiles),

            PassengerLoadPercent = passengerLoadPercent,

            PassengerCount = passengerCount,

            MaximumPassengers = Boeing737800.MaximumPassengers,

            CargoLoadPercent =cargoLoadPercent,

            CargoWeightKg = Math.Round(cargoWeightKg),

            MaximumCargoWeightKg =  Boeing737800.MaximumCargoWeightKg,

            EstimatedFlightTimeMinutes = estimatedFlightTimeMinutes };}

    private static double CalculateGreatCircleDistance(
        double latitude1,
        double longitude1,
        double latitude2,
        double longitude2)
    {
        const double earthRadiusNauticalMiles = 3440.065;

        double lat1 = DegreesToRadians(latitude1);
        double lat2 = DegreesToRadians(latitude2);

        double deltaLatitude =
            DegreesToRadians(latitude2 - latitude1);

        double deltaLongitude =
            DegreesToRadians(longitude2 - longitude1);

        double a =
            Math.Pow(Math.Sin(deltaLatitude / 2), 2)
            +
            Math.Cos(lat1)
            * Math.Cos(lat2)
            * Math.Pow(Math.Sin(deltaLongitude / 2), 2);

        double c =
            2 * Math.Atan2(
                Math.Sqrt(a),
                Math.Sqrt(1 - a)
            );

        return earthRadiusNauticalMiles * c;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
