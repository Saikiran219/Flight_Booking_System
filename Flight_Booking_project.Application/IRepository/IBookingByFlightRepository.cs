using Flight_Booking_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IBookingByFlightRepository
    {
        Task<Flight> GetFlightByIdAsync(int flightId);
    }
}
