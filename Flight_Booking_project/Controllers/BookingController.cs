﻿using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace Flight_Booking_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IPassengerService passengerService, IBookingService bookingService, IMapper mapper)
        {
            _passengerService = passengerService;
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("Passengers/{bookingId}")]
        public async Task<IActionResult> GetPassengersByBookingId(int bookingId)
        {
            var (isSuccess, passengers, message) = await _passengerService.GetPassengersByBookingIdAsync(bookingId);

            if (!isSuccess)
            {
                // Return a 404 Not Found response with a custom message
                return NotFound(new { Message = message });
            }
            return Ok(passengers);
        }


        //not in use
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("DeletePassengerList/{bookingId}")]
        public async Task<IActionResult> DeletePassengersByBookingId(int bookingId)
        {
            var result = await _passengerService.DeletePassengersByBookingIdAsync(bookingId);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = "No passengers found for the given booking ID." });
            }

            return Ok(new { Message = "Passengers deleted successfully." });
        }

        //not in use
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("DeletePassengers/{passengerId}")]
        public async Task<IActionResult> DeletePassenger(int passengerId)
        {
            var result = await _passengerService.DeletePassengerAsync(passengerId);

            if (!result.IsSuccess)
            {
                return NotFound(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }

        [Authorize(Roles ="Admin,User")]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] BookingRequestDto bookingRequestDto)
        {
            var bookingResult = await _bookingService.ConfirmBookingAsync(bookingRequestDto);

            if (!bookingResult.IsSuccess)
            {
                return BadRequest(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = bookingResult.Message
                });
            }

            var bookingDetailsDto = _mapper.Map<BookingDetailsDto>(bookingResult.Booking);
            bookingDetailsDto.TotalPrice = bookingResult.TotalPrice;

            var seatDetailsDto = _mapper.Map<SeatDto>(bookingResult.Seat);
           



            var response = new BookingResponseDto
            {
                IsSuccess = bookingResult.IsSuccess,
                Message = bookingResult.Message,
                BookingDetails = bookingDetailsDto,
                SeatDetails = seatDetailsDto
            };

            return Ok(response);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("user/{userId}/bookings")]
        public async Task<IActionResult> GetBookingsByUserId(string userId)
        {
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            if (bookings == null || !bookings.Any())
            {
                return NotFound("No bookings found for the user.");
            }
            return Ok(bookings);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpDelete("cancel/{bookingId}")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var bookingResult = await _bookingService.CancelBookingAsync(bookingId);

            if (!bookingResult.IsSuccess)
            {
                return BadRequest(new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = bookingResult.Message,
                    StatusId = bookingResult.StatusId
                });
            }

            return Ok(new BookingResponseDto
            {
                IsSuccess = true,
                Message = bookingResult.Message,
                StatusId = bookingResult.StatusId,
                BookingDetails = bookingResult.BookingDetails
            });
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("DownloadTicket/{bookingId}")]
        public async Task<IActionResult> DownloadTicket(int bookingId)
        {
            try
            {
                // Call the service method to generate the booking ticket
                var (totalCost, pdfStream) = await _bookingService.GenerateBookingTicket(bookingId);

                if (pdfStream == null)
                {
                    return NotFound("Booking not found.");
                }

                // Return the PDF as a downloadable file
               // pdfStream.Position = 0;
                var fileName = $"BookingTicket_{bookingId}.pdf";
                // Append the 'total-cost' header in lowercase
                Response.Headers.Append("total-cost", totalCost.ToString("F2"));
                return File(pdfStream, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                // Handle exceptions and return appropriate responses
                return BadRequest(ex.Message);
            }
        }


    }
}

