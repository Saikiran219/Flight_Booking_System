using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class Flight
    {
        [Key]
        public int FlightId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        // Navigation properties
        public int DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }

        public int ArrivalAirportId { get; set; }
        public Airport ArrivalAirport { get; set; }

        // A flight is associated with one airline
        public int AirlineId { get; set; }
        public Airline Airline { get; set; }

        // A flight can have multiple stops
        public ICollection<Stop> Stops { get; set; }

        // A flight has many seats
        public ICollection<Seat> Seats { get; set; }

        // A flight can have many bookings
        public ICollection<Booking> Bookings { get; set; }
    }
}
