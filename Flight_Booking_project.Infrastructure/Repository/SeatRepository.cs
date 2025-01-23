using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Infrastructure.Repository
{
    public class SeatRepository : ISeatRepository
    {
        private readonly FlightBookingContext _context;

        public SeatRepository(FlightBookingContext context)
        {
            _context = context;
        }

        // This method fetches available seats for a flight by flightId and seatClass.
        public async Task<List<Seat>> GetAvailableSeatsAsync(int flightId, string seatClass)
        {
            return await _context.Seats
                .Where(s => s.FlightId == flightId && s.ClassType == seatClass && s.IsAvailable)
                .ToListAsync();
        }

        public async Task UpdateSeatAvailabilityAsync(int flightId, string seatNumber)
        {
            var seat = await _context.Seats.FirstOrDefaultAsync(s => s.FlightId == flightId && s.SeatNumber == seatNumber);
            if (seat != null)
            {
                seat.IsAvailable = false;
                await _context.SaveChangesAsync();

            }
        }


        public async Task<Seat> GetSeatNumberByFlightAsync(int flightId, string seatNumber)
        {
            return await _context.Seats.FirstOrDefaultAsync(s => s.FlightId == flightId && s.SeatNumber == seatNumber);
        }

        public async Task<Seat> GetSeatByIdAsync(int seatId)
        {
            // Fetch the seat from the database using the SeatId
            return await _context.Seats
                .Include(s => s.Flight)  // Optionally include flight details if needed
                .Include(s => s.Passenger) // Optionally include passenger details if needed
                .FirstOrDefaultAsync(s => s.SeatId == seatId);
        }


    }

}
