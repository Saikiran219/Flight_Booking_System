using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class BookingRequestDto
    {
        public string? UserId { get; set; }
        public int FlightId { get; set; }
        public ICollection<SeatBookingDto> SeatBookings { get; set; } // Seats selected by the user
        public ICollection<PassengerRequestDto> Passengers { get; set; } // Passengers details
        public bool IsPaid { get; set; } = false; // Default to false

        public DateTime BookingDate { get; set; }
    }
}
