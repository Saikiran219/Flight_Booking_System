using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Interfaces
{
    public interface IPassengerService
    {
        Task<(bool IsSuccess, IEnumerable<PassengerDto> Passengers, string Message)> GetPassengersByBookingIdAsync(int bookingId);
        Task<BookingResponseDto> DeletePassengerAsync(int passengerId);
        Task<BookingResponseDto> DeletePassengersByBookingIdAsync(int bookingId);
    }
}
