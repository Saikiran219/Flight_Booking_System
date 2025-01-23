using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class BookingsByUser
    {
        public int BookingId { get; set; }
        public DateTime BookingDate { get; set; }
        public int StatusId { get; set; }
        public decimal TotalPrice {  get; set; }    
        public FlightBookingBYUserDto Flight {  get; set; }
        public List<PassengerDto> Passengers { get; set; }  

        public List<BookingSeatDto> Seats { get; set; }
        public int PassengerCount { get; set; }
       // public decimal TotalCost { get; set; }


    }
}
