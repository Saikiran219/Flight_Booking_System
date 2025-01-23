using Flight_Booking_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class StopDetailsDto
    {
        public int StopId { get; set; }
        public string AirportName { get; set; }
        public DateTime StopDuration { get; set; }
    }
}
