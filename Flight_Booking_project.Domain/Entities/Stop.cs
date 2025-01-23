using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class Stop
    {
        [Key]
        public int StopId { get; set; }
        public DateTime StopTime { get; set; }

        // A stop is related to one flight
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        // Each stop has an associated airport
        public int AirportId { get; set; }
        public Airport Airport { get; set; }
    }
}
