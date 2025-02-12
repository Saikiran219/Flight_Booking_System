using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class Seat
    {
        [Key]
        public int SeatId { get; set; }
        public string?SeatNumber { get; set; }
        public string? ClassType { get; set; } // e.g., Economy, Business
        public bool IsAvailable { get; set; }
        public string? Position { get; set; }
        public decimal Price { get; set; }


        // A seat is related to a specific flight
        public int FlightId { get; set; }
        public Flight? Flight { get; set; }
        // Passenger association (One-to-One)
        public Passenger? Passenger { get; set; }

    }
}
