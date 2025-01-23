using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.ResponseDto
{
    public class LoginResultDto
    {
        public string Token { get; set; }
        public int UserId { get; set; } // Adjust type as necessary
    }
}
