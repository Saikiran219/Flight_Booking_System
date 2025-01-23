using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class BookingResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } // Additional message or error information
        public int StatusId { get; set; }
        public BookingDetailsDto BookingDetails { get; set; } // Booking details if successful

        public SeatDto SeatDetails { get; set; }
    }
}
