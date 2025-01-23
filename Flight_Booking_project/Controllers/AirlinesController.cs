using Microsoft.AspNetCore.Mvc;

namespace Flight_Booking_project.Controllers
{
    using Flight_Booking_project.Application.Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class AirlinesController : ControllerBase
    {
        private readonly IAirlineService _airlineService;

        public AirlinesController(IAirlineService airlineService)
        {
            _airlineService = airlineService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAirlines()
        {
            var airlines = await _airlineService.GetAllAirlinesAsync();
            return Ok(airlines);
        }
    }

}
