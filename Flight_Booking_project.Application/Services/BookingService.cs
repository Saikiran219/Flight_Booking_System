using AutoMapper;
using Microsoft.EntityFrameworkCore;

using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using System.Numerics;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml.Linq;


namespace FlightBookingSystem.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBookingByFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ISeatRepository _seatRepository;
       


        public BookingService(IBookingRepository bookingRepository, IBookingByFlightRepository flightRepository, IMapper mapper, IEmailService emailService,ISeatRepository seatRepository ,IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _flightRepository = flightRepository;
            _mapper = mapper;
            _emailService = emailService;
            _userRepository = userRepository;
            _seatRepository = seatRepository;
            
        }

        public async Task<BookingResult> ConfirmBookingAsync(BookingRequestDto bookingRequestDto)
        {
            // Get the flight details
            var flight = await _flightRepository.GetFlightByIdAsync(bookingRequestDto.FlightId);

            if (flight == null)
            {
                return new BookingResult { IsSuccess = false, Message = "Flight not found" };
            }

            // Map bookingRequestDto to booking entity
            var booking = _mapper.Map<Booking>(bookingRequestDto);
            booking.FlightId = flight.FlightId;
            booking.IsPaid = bookingRequestDto.IsPaid;
            booking.BookingDate = bookingRequestDto.BookingDate;


            // Check if there are seat bookings
            if (bookingRequestDto.SeatBookings != null && bookingRequestDto.SeatBookings.Any())
            {
                foreach (var seatBooking in bookingRequestDto.SeatBookings)
                {
                    // Get the seat by unique SeatId
                    var seat = await _seatRepository.GetSeatByIdAsync(seatBooking.seatId);

                    if (seat != null)
                    {
                        // Check if the seat is available
                        if (seat.IsAvailable)
                        {
                            // Update seat availability
                            await _seatRepository.UpdateSeatAvailabilityAsync(booking.FlightId, seatBooking.SeatNumber);

                            // Assign the unique SeatId to the corresponding passenger
                            var passenger = booking.Passengers.FirstOrDefault(p => p.SeatId == null); // Get a passenger without a SeatId
                            if (passenger != null)
                            {
                                passenger.SeatId = seat.SeatId; // Assign unique SeatId
                            }
                        }
                        else
                        {
                            return new BookingResult { IsSuccess = false, Message = $"Seat {seatBooking.seatId} is not available." };
                        }
                    }
                    else
                    {
                        return new BookingResult { IsSuccess = false, Message = $"Seat {seatBooking.seatId} does not exist." };
                    }
                }
            }
          //  booking.TotalPrice = booking.Passengers.Sum(p => p.Seat.Price);
            // Save the booking and associated passengers with seat assignments
            var savedBooking = await _bookingRepository.AddBookingAsync(booking);

            var bookingResult = new BookingResult
            {
                IsSuccess = true,
                Message = "Booking confirmed successfully",
                Booking = savedBooking,
                IsPaid = savedBooking.IsPaid,
                Seat = null ,// Optionally return the last processed seat
                TotalPrice = booking.Passengers.Sum(p => p.Seat.Price)
        };

            // If booking is successful, send email
            if (bookingResult.IsSuccess)
            {
                var user = await _userRepository.GetUserByIdAsync(bookingRequestDto?.UserId??"");
                var emailBody = await _emailService.GenerateEmailBody(bookingRequestDto);
                _emailService.SendEmailNotification(user.Email??"", "Flight Booking Confirmation", emailBody);
            }

            return bookingResult;
        }
        public async Task<IEnumerable<BookingsByUser>> GetBookingsByUserIdAsync(string userId)

        {
            var bookings = await _bookingRepository.GetBookingsByUserIdAsync(userId);
           
            // Map bookings to BookingsByUser DTO
            var bookingsByUser = new List<BookingsByUser>();
            foreach (var booking in bookings)
            {
                var bookingDto = _mapper.Map<BookingsByUser>(booking);

                // Populate the seats for the booking
                bookingDto.Seats = booking.Passengers  
                    .Where(p => p.Seat != null) // Ensure seat exists
                    .Select(p => new BookingSeatDto
                    {
                        Price = p?.Seat?.Price??0, // Assuming your Seat has a Price property
                        SeatNumber = p?.Seat?.SeatNumber?? "",
                        SeatPosition = p?.Seat?.Position ?? ""// Assuming your Seat has a SeatPosition property
                    }).ToList();
                bookingDto.TotalPrice = bookingDto.Seats.Sum(seat => seat.Price);

                bookingDto.PassengerCount = booking?.Passengers.Count ?? 0;
              //  bookingDto.TotalCost = await GetTotalCostAsync(booking.BookingId); // Assume you have BookingId in booking
                bookingsByUser.Add(bookingDto);
            }
            return bookingsByUser;

        }
        public async Task<BookingResponseDto> CancelBookingAsync(int bookingId)
        {
            var isCancelled = await _bookingRepository.CancelBookingAsync(bookingId);

            if (!isCancelled)
            {
                return new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = "Booking not found or cancellation failed.",
                    StatusId = -0

                };
            }

            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

            return new BookingResponseDto
            {
                IsSuccess = true,
                Message = "Booking canceled successfully and records deleted.",
                StatusId = booking.StatusId
            };
        }
        public async Task<(decimal TotalCost, byte[] PdfData)> GenerateBookingTicket(int bookingId)
        {
            // Get the booking from the repository
            var booking = await _bookingRepository.GetBookingByIdAsync(bookingId);

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            // Check if the booking is still pending
            if (booking.StatusId != -1)
            {
                throw new Exception("Ticket can only be downloaded for pending bookings.");
            }
            decimal totalCost = await GetTotalCostAsync(bookingId);

            // Generate PDF and use 'using' blocks for automatic disposal of resources
            var stream = new MemoryStream();

            using (var document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 30, 30)) // Page size A4 with margins
            {
                var writer = PdfWriter.GetInstance(document, stream);
                writer.CloseStream = false; // Prevent PdfWriter from closing the MemoryStream

                document.Open();

                // Define font styles
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, iTextSharp.text.BaseColor.BLACK);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, iTextSharp.text.BaseColor.BLACK);
                var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.BaseColor.BLACK);

                // Add a title
                document.Add(new Paragraph("Booking Ticket", titleFont) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph(" ")); // Add space

                // Flight Details Section
                document.Add(new Paragraph("Flight Details", headerFont));
                document.Add(new Paragraph($"Booking ID: {booking.BookingId}", regularFont));
                document.Add(new Paragraph($"Flight: {booking.Flight.FlightId}", regularFont));
                document.Add(new Paragraph($"Airline: {booking.Flight.Airline.AirlineName}", regularFont));
                document.Add(new Paragraph($"Departure: {booking.Flight.DepartureAirport.Name} at {booking.Flight.DepartureTime}", regularFont));
                document.Add(new Paragraph($"Arrival: {booking.Flight.ArrivalAirport.Name} at {booking.Flight.ArrivalTime}", regularFont));
                document.Add(new Paragraph(" ")); // Add space between sections

                // Add total cost to the document
                document.Add(new Paragraph($"Total Cost: ${totalCost:F2}", headerFont)); // Display total cost in USD format
                document.Add(new Paragraph(" ")); // Add space

                // Stop Details Section
                document.Add(new Paragraph("Stop Details", headerFont));
                if (booking.Flight.Stops != null && booking.Flight.Stops.Any())
                {
                   
                    document.Add(new Paragraph(" "));

                    // Create a table for stop details with 2 columns
                    PdfPTable stopTable = new PdfPTable(2);
                    stopTable.WidthPercentage = 100; // Table width is 100% of the page width
                    stopTable.SetWidths(new float[] { 3, 1 }); // Set the width proportions for each column

                    // Add table headers
                    stopTable.AddCell(new PdfPCell(new Phrase("Stop Airport", headerFont)));
                    stopTable.AddCell(new PdfPCell(new Phrase("Stop Time", headerFont)));

                    // Add stop details in the table
                    foreach (var stop in booking.Flight.Stops)
                    {
                        stopTable.AddCell(new PdfPCell(new Phrase(stop.Airport.Name, regularFont)));
                        stopTable.AddCell(new PdfPCell(new Phrase(stop.StopTime.ToString("g"), regularFont))); // Formatting to display date and time
                    }

                    // Add the table to the document
                    document.Add(stopTable);
                    document.Add(new Paragraph(" "));
                }
                else
                {
                    document.Add(new Paragraph("No Intermediate Airports Found.", regularFont));
                }

                document.Add(new Paragraph(" ")); // Add space between sections

                // Passenger Details Section
                document.Add(new Paragraph("Passenger Details", headerFont));
                document.Add(new Paragraph(" "));

                // Create a table with 4 columns for passenger information
                PdfPTable passengerTable = new PdfPTable(4);
                passengerTable.WidthPercentage = 100; // Table width is 100% of the page width
                passengerTable.SetWidths(new float[] { 3, 1, 1, 2 }); // Set the width proportions for each column

                // Add table headers
                passengerTable.AddCell(new PdfPCell(new Phrase("Passenger Name", headerFont)));
                passengerTable.AddCell(new PdfPCell(new Phrase("Age", headerFont)));
                passengerTable.AddCell(new PdfPCell(new Phrase("Phone", headerFont)));
                passengerTable.AddCell(new PdfPCell(new Phrase("Seat", headerFont)));

                // Add passenger details in the table
                foreach (var passenger in booking.Passengers)
                {
                    passengerTable.AddCell(new PdfPCell(new Phrase($"{passenger.FirstName} {passenger.LastName}", regularFont)));
                    passengerTable.AddCell(new PdfPCell(new Phrase(passenger.Age.ToString(), regularFont)));
                    passengerTable.AddCell(new PdfPCell(new Phrase(passenger.PhoneNumber.ToString(), regularFont)));
                    passengerTable.AddCell(new PdfPCell(new Phrase($"Seat: {passenger.Seat.SeatNumber} ({passenger.Seat.Position})", regularFont)));
                  
                }

                // Add the table to the document
                document.Add(passengerTable);
                document.Add(new Paragraph(" "));

                document.Close(); // Close the document
            }

            // Reset stream position before returning it
            stream.Position = 0;
            return (totalCost, stream.ToArray());
        }
        public async Task<decimal> GetTotalCostAsync(int bookingId)
        {
            return await _bookingRepository.CalculateTotalCostAsync(bookingId);
        }


    }
}
