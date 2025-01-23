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
    public class AirlineService : IAirlineService
    {
        private readonly IAirlineRepository _airlineRepository;

        public AirlineService(IAirlineRepository airlineRepository)
        {
            _airlineRepository = airlineRepository;
        }

        public async Task<IEnumerable<Airline>> GetAllAirlinesAsync()
        {
             return await _airlineRepository.GetAllAirlinesAsync();
            
        }
    }

}
