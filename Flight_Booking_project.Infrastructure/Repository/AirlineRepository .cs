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
    public class AirlineRepository : IAirlineRepository
    {
        private readonly FlightBookingContext _context;

        public AirlineRepository(FlightBookingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
        {
            return await _context.Airlines.ToListAsync();
        }

       
    }

}
