using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class AddFlightDto
    {
       // public string FlightNumber { get; set; }
        public string DepartureAirportName { get; set; } // Use name in UI
        public string ArrivalAirportName { get; set; }   // Use name in UI
        public string AirlineName { get; set; }          // Use name in UI
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        // Optional: List of stops
        public List<AddStopDto> Stops { get; set; } = new List<AddStopDto>();

        // Optional: List of seats
        public List<AddSeatDto> Seats { get; set; } = new List<AddSeatDto>();

        // Optional: List of bookings
       // public List<AddBookingDto> Bookings { get; set; } = new List<AddBookingDto>();
    }

}
