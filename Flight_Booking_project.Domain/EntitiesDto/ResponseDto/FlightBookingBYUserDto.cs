using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class FlightBookingBYUserDto
    {
        public string FlightNumber { get; set; }
        public string Airline { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int StopCount { get; set; }
        public decimal BaggageAllowance { get; set; }
        public List<StopDetailsDto> Stops { get; set; }

        
       
    }
}
