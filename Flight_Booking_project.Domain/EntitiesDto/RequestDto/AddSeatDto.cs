using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class AddSeatDto
    {
        public string SeatNumber { get; set; }
        public string SeatPosition { get; set; }
        public string ClassType { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; } = true;
        //public int? PassengerId { get; set; } // Optional: passenger id, nullable
    }
}
