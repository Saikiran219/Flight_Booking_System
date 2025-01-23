using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Microsoft.AspNetCore.Mvc;

namespace Flight_Booking_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet("Basicsearch")]
        public async Task<IActionResult> SearchFlights([FromQuery] string DepartureAirportName,[FromQuery] string ArrivalAirportName, [FromQuery] string ClassType, [FromQuery] DateTime DepartureDate, [FromQuery] int NumberOfPassengers)
        {
            try
            {
                var flights = await _flightService.SearchFlightsAsync(DepartureAirportName,ArrivalAirportName,ClassType,DepartureDate,NumberOfPassengers);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        /* [HttpGet("AdvanceFilterSearch")]
         public async Task<IActionResult> SearchFlightsByAdvanceFilter( [FromQuery]decimal? MinPrice, [FromQuery] decimal? MaxPrice, [FromQuery] string? AirlineName, [FromQuery] int? NumberOfStops)
         {
             try
             {
                 var flights = await _flightService.SearchFlightsByAdvanceFilterAsync( MinPrice,  MaxPrice, AirlineName, NumberOfStops);
                 return Ok(flights);
             }
             catch (Exception ex)
             {
                 return BadRequest(new { Message = ex.Message });
             }
         }*/

        //[HttpGet("details/{id}")]
        //public async Task<IActionResult> GetFlightById(int id)
        //{
        //    var flight = await _flightService.GetflightByIdAsync(id);
        //    if (flight == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(flight);
        //}

        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetFlightDetails(int flightId)
        {
            try
            {
                var flightDetails = await _flightService.GetFlightByIdAsync(flightId);
                if (flightDetails == null)
                {
                    return NotFound();
                }
                return Ok(flightDetails);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpGet("{flightId}/available-seats")]
        public async Task<IActionResult> GetAvailableSeats(int flightId, [FromQuery] string seatClass)
        {
            try
            {
                var availableSeats = await _flightService.GetAvailableSeatsAsync(flightId, seatClass);
                return Ok(availableSeats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpDelete("delete-flight/{flightId}")]

        public async Task<IActionResult> DeleteFlight(int flightId)

        {

            var result = await _flightService.DeleteFlightByIdAsync(flightId);


            if (result == "Flight and its related details successfully deleted.")

            {

                return Ok(new { Message = result });

            }


            return NotFound(new { Message = result });

        }

        [HttpGet("GetAllFlights")]
        public async Task<IActionResult> GetAllFlights()
        {
            var flights = await _flightService.GetAllFlightsWithDetailsAsync(); // Fetch flights with all details
            return Ok(flights);
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddFlight([FromBody] AddFlightDto addFlightDto)
        {
            if (addFlightDto == null)
            {
                return BadRequest();
            }

            try
            {
                await _flightService.AddFlightAsync(addFlightDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPut("UpdateFlightDetails")]
        public async Task<IActionResult> UpdateFlightDetails([FromBody] UpdateFlightDetailsDto updateFlightDetailsDto)
        {
            // Check if the flight object is provided
            if (updateFlightDetailsDto.Flight == null)
            {
                return BadRequest("Flight details must be provided.");
            }

            // Proceed with the update logic
            await _flightService.UpdateFlightDetailsAsync(updateFlightDetailsDto);
            return Ok("Flight details updated successfully");
        }


    }

}
