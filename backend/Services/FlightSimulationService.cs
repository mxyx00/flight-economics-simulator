using backend.Data;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;

// using haversine formula to calculate distance
// using nautical miles

namespace backend.Services;

public class FlightSimulationService
{
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

        return new SimulationResult
        {
            DepartureAirport = departure.Code,
            ArrivalAirport = arrival.Code,
            DistanceNauticalMiles =
                Math.Round(distanceNauticalMiles),
            PassengerLoadPercent = passengerLoadPercent,
            CargoLoadPercent = cargoLoadPercent
        };
    }

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