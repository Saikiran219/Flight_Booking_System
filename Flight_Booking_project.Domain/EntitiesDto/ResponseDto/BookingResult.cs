using Flight_Booking_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class BookingResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Booking Booking { get; set; }
        // public decimal TotalPrice { get; set; }

        public Seat Seat { get; set; }

        public bool IsPaid { get; set; }
        public decimal TotalPrice {  get; set; }
    }
}
