using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class SeatDto
    {

        public string SeatNumber { get; set; }
        public string ClassType { get; set; } // e.g., Economy, Business
        public string Position { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
    }
}
