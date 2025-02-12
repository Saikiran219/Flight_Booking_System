using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Flight_Booking_project.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly FlightBookingContext _context;

        public UserRepository(FlightBookingContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

    }
}