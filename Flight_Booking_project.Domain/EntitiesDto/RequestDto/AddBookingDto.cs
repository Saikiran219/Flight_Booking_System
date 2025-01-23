using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class AddBookingDto
    {
        public int PassengerId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
