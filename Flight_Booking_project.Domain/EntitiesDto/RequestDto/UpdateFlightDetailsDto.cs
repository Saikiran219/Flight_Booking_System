using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class UpdateFlightDetailsDto
    {
        public UpdateFlightDto? Flight { get; set; }
        public List<UpdateSeatDto>? Seats { get; set; }
        public List<UpdateStopDto>? Stops { get; set; }
    }
}
