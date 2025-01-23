using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;


namespace Flight_Booking_project.Application.Services
{
    public class PassengerService : IPassengerService
    {
        private readonly IPassengerRepository _repository;

        public PassengerService(IPassengerRepository repository)
        {
            _repository = repository;
        }

        public async Task<(bool IsSuccess, IEnumerable<PassengerDto> Passengers, string Message)> GetPassengersByBookingIdAsync(int bookingId)
        {
            return await _repository.GetPassengersByBookingIdAsync(bookingId);
        }

        public async Task<BookingResponseDto> DeletePassengerAsync(int passengerId)
        {
            var success = await _repository.DeletePassengerAsync(passengerId);
            if (!success)
            {
                return new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = "Passenger not found."
                };
            }

            return new BookingResponseDto
            {
                IsSuccess = true,
                Message = "Passenger deleted successfully."
            };
        }

        public async Task<BookingResponseDto> DeletePassengersByBookingIdAsync(int bookingId)
        {
            var success = await _repository.DeletePassengersByBookingIdAsync(bookingId);
            if (!success)
            {
                return new BookingResponseDto
                {
                    IsSuccess = false,
                    Message = "Passenger not found."
                };
            }

            return new BookingResponseDto
            {
                IsSuccess = true,
                Message = "Passenger deleted successfully."
            };
        }
    }
}
