using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class BookingDetailsDto
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public decimal TotalPrice {  get; set; }    
        public List<PassengerDto> Passengers { get; set; }
      
        public bool IsPaid { get; set; }
    }

}
