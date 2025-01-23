using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class UpdateFlightDto
    {
        public int Id { get; set; }
        public string? AirlineName { get; set; }
        public string? DepartureAirportName { get; set; }
        public string? ArrivalAirportName { get; set; }
        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
    }
}
