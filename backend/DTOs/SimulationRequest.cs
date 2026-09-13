using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public class SimulationRequest
{
    [Required]
    public string DepartureAirport { get; set; } = "";

    [Required]
    public string ArrivalAirport { get; set; } = "";

    [Range(0, 100)]
    public int PassengerLoadPercent { get; set; }

    [Range(0, 100)]
    public int CargoLoadPercent { get; set; }
}