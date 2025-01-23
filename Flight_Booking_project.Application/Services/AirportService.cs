using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Services
{
    public class AirportService:IAirportService
    {
        private readonly IAirportRepository _airportRepository;

        public AirportService(IAirportRepository airportRepository)
        {
            _airportRepository = airportRepository;
        }

        public async Task<List<Airport>> GetAllAirportsAsync()
        {
            // Business logic can be applied here (e.g., filtering inactive airports, etc.)
            return await _airportRepository.GetAllAirportsAsync();
        }
    }
}
