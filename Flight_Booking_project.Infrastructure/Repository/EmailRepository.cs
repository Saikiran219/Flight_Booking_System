using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Flight_Booking_project.Application.IRepository;

namespace EStore.Infrastructure.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;
        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void SendMailNotification(string toEmail, string subject, string body)
        {
            // Validate the email address
            if (string.IsNullOrWhiteSpace(toEmail) || !IsValidEmail(toEmail))
            {
                throw new ArgumentException("Invalid email address.", nameof(toEmail));
            }

            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("tierenvsp01@gmail.com", GetPassword());

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("tierenvsp01@gmail.com");
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    smtpClient.Send(mailMessage);
                }
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string GetPassword()
        {
            return "nvxpiyyknivljgtn"; // Consider securing your password!
        }
    }

}