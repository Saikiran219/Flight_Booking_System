using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.EntitiesDto.RequestDto
{
    public class ResetPasswordReq
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string Token { get; set; }
    }
}