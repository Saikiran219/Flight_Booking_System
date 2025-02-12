using AutoMapper;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flight_Booking_project.Infrastructure.Repository
{
    public class PassengerRepository : IPassengerRepository
    {
        private readonly FlightBookingContext _context;
        private readonly IMapper _mapper;


        public PassengerRepository(FlightBookingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(bool IsSuccess, IEnumerable<PassengerDto> Passengers, string Message)> GetPassengersByBookingIdAsync(int bookingId)
        {
            var booking = await _context.Bookings
                .Where(b => b.BookingId == bookingId)
                .Include(b => b.Passengers)
                .FirstOrDefaultAsync();

            if (booking == null)
            {
                return (false, Enumerable.Empty<PassengerDto>(), $"Booking with ID {bookingId} not found.");
            }

            var passengers = _mapper.Map<IEnumerable<PassengerDto>>(booking.Passengers);
            return (true, passengers, string.Empty);
        }

        public async Task<bool> DeletePassengerAsync(int passengerId)
        {
            var passenger = await _context.Passengers.FindAsync(passengerId);

            if (passenger == null)
            {
                return false;
            }

            _context.Passengers.Remove(passenger);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePassengersByBookingIdAsync(int bookingId)
        {
            var passengers = _context.Passengers.Where(p => p.BookingId == bookingId).ToList();

            if (passengers == null || !passengers.Any())
            {
                return false; // No passengers found for the given booking ID
            }

            _context.Passengers.RemoveRange(passengers);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
