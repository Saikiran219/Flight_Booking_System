using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IBookingRepository
    {

        Task<Booking> AddBookingAsync(Booking booking);
        Task<Passenger> AddPassengerAsync(Passenger passenger);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task DeleteBookingAsync(int bookingId);

        Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId);

        Task<bool> CancelBookingAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<decimal> CalculateTotalCostAsync(int bookingId);






    }
}
