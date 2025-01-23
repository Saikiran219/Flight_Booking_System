using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IEmailRepository
    {
        void SendMailNotification(string toEmail, string subject, string body);
    }
}