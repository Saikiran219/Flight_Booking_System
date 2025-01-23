using Flight_Booking_project.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.IRepository
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAvailableSeatsAsync(int flightId, string seatClass);
        Task UpdateSeatAvailabilityAsync(int flightId, string seatNumber);
        Task<Seat> GetSeatNumberByFlightAsync(int flightId, string seatNumber);
        Task<Seat> GetSeatByIdAsync(int seatId);
    }

}
