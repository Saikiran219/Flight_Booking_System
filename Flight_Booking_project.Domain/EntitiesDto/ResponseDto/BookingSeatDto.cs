using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class BookingSeatDto
    {
        public decimal Price { get; set; }
        public string SeatNumber { get; set; }
        public string SeatPosition { get; set; }
    }
}
