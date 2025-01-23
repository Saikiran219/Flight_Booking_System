using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class FlightDetailsDtoByAdmin
    {
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string AirlineName { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportName { get; set; }
        public List<string> StopNames { get; set; }
        public ICollection<SeatBookingDto> SeatDetails { get; set; } = new List<SeatBookingDto>(); // Include Seat details
    }
}
