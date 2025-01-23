using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class FlightWithDetailsDto
    {
        public int FlightId { get; set; }
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportName { get; set; }
        public string AirlineName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public List<Seatdto> Seats { get; set; }
        public List<StopDto> Stops { get; set; }
    }
}
