using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public long PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public long? AlternativeContactNumber { get; set; }
        // A user can have many bookings
        public ICollection<Booking> Bookings { get; set; }
    }
}
