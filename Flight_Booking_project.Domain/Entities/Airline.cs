using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class Airline
    {
        [Key]
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public decimal BaggageAllowance { get; set; }

        // One-to-many relationship with flights
        public ICollection<Flight> Flights { get; set; }

        public ICollection<Airport> Airports { get; set; }
    }
}
