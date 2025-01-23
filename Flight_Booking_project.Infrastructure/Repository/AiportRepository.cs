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
    public class AirportRepository:IAirportRepository
    {
        private readonly FlightBookingContext _context;

        public AirportRepository(FlightBookingContext context)
        {
            _context = context;
        }

        public async Task<List<Airport>> GetAllAirportsAsync()
        {
            // Fetch all airports from the database
            return await _context.Airports.ToListAsync(); 
        }
    }
}
