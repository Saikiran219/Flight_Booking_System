using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class AddStopDto
    {
        public string StopAirportName { get; set; }
        public DateTime StopTime { get; set; }
    }
}
