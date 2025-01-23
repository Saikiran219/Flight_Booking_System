using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class PassengerRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public long PhoneNumber { get; set; }
        public string Address { get; set; }
        public long? AlternativeContactNumber { get; set; }
        public int? SeatId { get; set; }



    }
}
