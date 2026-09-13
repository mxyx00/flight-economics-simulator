using backend.DTOs;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulationController : ControllerBase
{
    private readonly FlightSimulationService _simulationService;

    public SimulationController(
        FlightSimulationService simulationService)
    {
        _simulationService = simulationService;
    }

    [HttpPost]
    public async Task<ActionResult<SimulationResult>> Simulate(
        SimulationRequest request)
    {
        string departureCode =
            request.DepartureAirport
                .Trim()
                .ToUpperInvariant();

        string arrivalCode =
            request.ArrivalAirport
                .Trim()
                .ToUpperInvariant();

        if (departureCode == arrivalCode)
        {
            return BadRequest(new
            {
                error =
                    "Departure and arrival airports must be different."
            });
        }

        try
        {
            var result =
                await _simulationService.SimulateAsync(
                    departureCode,
                    arrivalCode,
                    request.PassengerLoadPercent,
                    request.CargoLoadPercent
                );

            return Ok(result);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(new
            {
                error = exception.Message
            });
        }

        catch (InvalidOperationException exception)
        {
            return BadRequest(new
            {
                error = exception.Message
            });
        }
    }
}