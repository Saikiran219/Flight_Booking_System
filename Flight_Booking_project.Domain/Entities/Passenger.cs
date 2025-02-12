using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class Passenger
    {
        [Key]
        public int PassengerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public int Age { get; set; }
        public long PhoneNumber { get; set; }
        public string? Address { get; set; }
        public long? AlternativeContactNumber { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        // Seat association (One-to-One)
        public int? SeatId { get; set; } // Foreign key
        public Seat? Seat { get; set; }
    }
}
