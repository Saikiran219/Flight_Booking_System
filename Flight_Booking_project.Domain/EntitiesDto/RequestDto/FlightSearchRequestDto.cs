using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class FlightBasicSearchRequestDto
    {
        public string DepartureAirportName { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ClassType { get; set; }
        public DateTime DepartureDate { get; set; }
        public int NumberOfPassengers { get; set;}



    }
}
