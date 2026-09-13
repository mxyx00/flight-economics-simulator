using Microsoft.AspNetCore.Mvc;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController : ControllerBase
{
    private static readonly List<Airport> Airports =
    [
        new Airport
        {
            Code = "LAX",
            Name = "Los Angeles International Airport",
            City = "Los Angeles"
        },

        new Airport
        {
            Code = "JFK",
            Name = "John F. Kennedy International Airport",
            City = "New York"
        },

        new Airport
        {
            Code = "ORD",
            Name = "O'Hare International Airport",
            City = "Chicago"
        },

        new Airport
        {
            Code = "DFW",
            Name = "Dallas Fort Worth International Airport",
            City = "Dallas"
        },

        new Airport
        {
            Code = "ATL",
            Name = "Hartsfield-Jackson Atlanta International Airport",
            City = "Atlanta"
        },

        new Airport
        {
            Code = "SFO",
            Name = "San Francisco International Airport",
            City = "San Francisco"
        },

        new Airport
        {
            Code = "SEA",
            Name = "Seattle-Tacoma International Airport",
            City = "Seattle"
        },

        new Airport
        {
            Code = "MIA",
            Name = "Miami International Airport",
            City = "Miami"
        },

        new Airport
        {
            Code = "BOS",
            Name = "Boston Logan International Airport",
            City = "Boston"
        },

        new Airport
        {
            Code = "DEN",
            Name = "Denver International Airport",
            City = "Denver"
        }
    ];

    [HttpGet]
    public ActionResult<IEnumerable<Airport>> GetAirports()
    {
        return Ok(Airports);
    }
}