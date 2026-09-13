using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AirportsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AirportsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Airport>>> GetAirports()
    {
        var airports = await _context.Airports
            .AsNoTracking()
            .OrderBy(airport => airport.Code)
            .ToListAsync();

        return Ok(airports);
    }
}