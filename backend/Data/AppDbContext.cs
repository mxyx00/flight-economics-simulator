using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Airport> Airports => Set<Airport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airport>()
            .HasIndex(airport => airport.Code)
            .IsUnique();

        modelBuilder.Entity<Airport>()
            .Property(airport => airport.Code)
            .HasMaxLength(3)
            .IsRequired();

        modelBuilder.Entity<Airport>()
            .Property(airport => airport.Name)
            .IsRequired();

        modelBuilder.Entity<Airport>()
            .Property(airport => airport.City)
            .IsRequired();

        modelBuilder.Entity<Airport>().HasData(
            new Airport
            {
                Id = 1,
                Code = "LAX",
                Name = "Los Angeles International Airport",
                City = "Los Angeles",
                Latitude = 33.942501,
                Longitude = -118.407997,
                LandingFeePer1000Lb = 7.50,
                DepartureFee = 1200
            },

            new Airport
            {
                Id = 2,
                Code = "JFK",
                Name = "John F. Kennedy International Airport",
                City = "New York",
                Latitude = 40.639447,
                Longitude = -73.779317,
                LandingFeePer1000Lb = 9.50,
                DepartureFee = 1600
            },

            new Airport
            {
                Id = 3,
                Code = "ORD",
                Name = "Chicago O'Hare International Airport",
                City = "Chicago",
                Latitude = 41.9786,
                Longitude = -87.9048,
                LandingFeePer1000Lb = 7.25,
                DepartureFee = 1100

            },

            new Airport
            {
                Id = 4,
                Code = "DFW",
                Name = "Dallas Fort Worth International Airport",
                City = "Dallas",
                Latitude = 32.896801,
                Longitude = -97.038002,
                LandingFeePer1000Lb = 5.50,
                DepartureFee = 900
            },

            new Airport
            {
                Id = 5,
                Code = "ATL",
                Name = "Hartsfield-Jackson Atlanta International Airport",
                City = "Atlanta",
                Latitude = 33.6367,
                Longitude = -84.428101,
                LandingFeePer1000Lb = 4.75,
                DepartureFee = 850
            },

            new Airport
            {
                Id = 6,
                Code = "SFO",
                Name = "San Francisco International Airport",
                City = "San Francisco",
                Latitude = 37.619806,
                Longitude = -122.374821,
                LandingFeePer1000Lb = 8.25,
                DepartureFee = 1400
            },

            new Airport
            {
                Id = 7,
                Code = "SEA",
                Name = "Seattle-Tacoma International Airport",
                City = "Seattle",
                Latitude = 47.449001,
                Longitude = -122.308998,
                LandingFeePer1000Lb = 6.00,
                DepartureFee = 950
            },

            new Airport
            {
                Id = 8,
                Code = "MIA",
                Name = "Miami International Airport",
                City = "Miami",
                Latitude = 25.7932,
                Longitude = -80.290604,
                LandingFeePer1000Lb = 6.25,
                DepartureFee = 1000
            },

            new Airport
            {
                Id = 9,
                Code = "BOS",
                Name = "Boston Logan International Airport",
                City = "Boston",
                Latitude = 42.3643,
                Longitude = -71.005203,
                LandingFeePer1000Lb = 8.00,
                DepartureFee = 1300
            },

            new Airport
            {
                Id = 10,
                Code = "DEN",
                Name = "Denver International Airport",
                City = "Denver",
                Latitude = 39.861698,
                Longitude = -104.672997,
                LandingFeePer1000Lb = 5.25,
                DepartureFee = 900
            }
        );
    }
}