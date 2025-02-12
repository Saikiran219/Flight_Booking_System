using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Flight_Booking_project.Domain.Entities
{
    public class User:IdentityUser
    {
  
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public long? AlternativeContactNumber { get; set; }
        // A user can have many bookings
        public ICollection<Booking>? Bookings { get; set; }
    }
}
