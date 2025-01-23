using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Flight_Booking_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly IAirportService _airportService;

        public AirportsController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAirports()
        {
            try
            {
                List<Airport> airports = await _airportService.GetAllAirportsAsync();

                if (airports == null || airports.Count == 0)
                {
                    return NotFound("No airports found.");
                }

                return Ok(airports);
            }
            catch (Exception ex)
            {
                // Log the error if necessary
                return StatusCode(500, "An error occurred while retrieving airports.");
            }
        }
    }
}
