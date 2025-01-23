using Flight_Booking_project.Application.Interfaces;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Net.Mail;

using System.Net;

using System.Numerics;

using System.Text;

using System.Threading.Tasks;

using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;

using Flight_Booking_project.Application.IRepository;

using Flight_Booking_project.Domain.EntitiesDto.RequestDto;

namespace Flight_Booking_project.Application.Services

{

    public class EmailService : IEmailService

    {

        private readonly IFlightRepository _flightRepository;

        public EmailService(IFlightRepository flightRepository)

        {

            _flightRepository = flightRepository;

        }

        public void SendEmailNotification(string toEmail, string subject, string body)

        {

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.EnableSsl = true;

            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new NetworkCredential("burkashruthi001@gmail.com", GetPassword());

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("burkashruthi001@gmail.com");

            mailMessage.To.Add(toEmail);

            mailMessage.Subject = subject;

            mailMessage.Body = body;

            mailMessage.IsBodyHtml = true;

            smtpClient.Send(mailMessage);

        }

        public async Task<String> GenerateEmailBody(BookingRequestDto bookingRequestDto)

        {



            var passengers = bookingRequestDto.Passengers;

            var flightdetails = await _flightRepository.GetFlightByIdAsync(bookingRequestDto.FlightId);

            var emailBody = $@"
<html>
<body>
<h2>Flight Booking Confirmation</h2>
<p>Dear Customer,</p>
<p>Thank you for booking a flight with us. Here are your booking details:</p>
<h4>Flight Details:</h4>
<ul>
<li><strong>Flight Number:</strong> {bookingRequestDto.FlightId}</li>
<li><strong>Airline:</strong> {flightdetails.Airline.AirlineName}</li>
<li><strong>Departure Airport:</strong> {flightdetails.DepartureAirport.Name}</li>
<li><strong>Arrival Airport:</strong> {flightdetails.ArrivalAirport.Name}</li>
<li><strong>Departure Time:</strong> {flightdetails.DepartureTime}</li>
<li><strong>Arrival Time:</strong> {flightdetails.ArrivalTime}</li>
</ul>
<h4>Stop Details:</h4>
<ul>";

            // Add stop details if any
            emailBody += "<h4>Stop Details:</h4><ul>";
            if (flightdetails.Stops != null && flightdetails.Stops.Any())
            {
                foreach (var stop in flightdetails.Stops)
                {
                    emailBody += $@"
                        <li><strong>Stop:</strong> {stop.Airport?.Name ?? "Unknown"}</li>
                        <li><strong>Stop Duration:</strong> {stop.StopTime}</li>";
                }
            }
            else
            {
                emailBody += "<li>No Stops</li>";
            }

            emailBody += "</ul>";

            // Add passenger details

            emailBody += "<h4>Passenger Details:</h4><ul>";

            foreach (var passenger in passengers)

            {

                emailBody += $@"
<ul>
<li>Name: {passenger.FirstName} {passenger.LastName}</li>
<li>Age: {passenger.Age}</li>
<li>Gender: {passenger.Gender}</li>
<li>Phone Number: {passenger.PhoneNumber}</li>
</ul>";

            }

            emailBody += $@"
</ul>
<p>If you have any questions or need to reschedule, please contact us.</p>
<p>Best regards,<br/>Your Airline Team</p>
</body>
</html>";

            return emailBody;


        }

        private string GetPassword()

        {

            return "xvacuzdxgonhvvwq";

        }

    }

}

