using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IPassengerRepository
    {

        Task<(bool IsSuccess, IEnumerable<PassengerDto> Passengers, string Message)> GetPassengersByBookingIdAsync(int bookingId);
        Task<bool> DeletePassengerAsync(int passengerId);
        Task<bool> DeletePassengersByBookingIdAsync(int bookingId);


    }
}
