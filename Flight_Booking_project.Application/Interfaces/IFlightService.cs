using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Application.Interfaces
{
    public interface IFlightService
    {
        Task<FlightDetailsResponseDto> GetFlightByIdAsync(int flightId);
        Task<List<FlightResponseDto>> SearchFlightsAsync(string DepartureAirportName, string ArrivalAirportName, string ClassType, DateTime DepartureDate, int NumberOfPassengers);
        /* Task<List<FlightResponseDto>> SearchFlightsByAdvanceFilterAsync(decimal? MinPrice, decimal? MaxPrice, string? AirlineName, int? NumberOfStops);*/
        Task<List<Seat>> GetAvailableSeatsAsync(int flightId, string seatClass);
        Task<string> DeleteFlightByIdAsync(int flightId);
        Task<List<FlightWithDetailsDto>> GetAllFlightsWithDetailsAsync();
        Task<Flight> GetflightByIdAsync(int flightId);

        Task AddFlightAsync(AddFlightDto addFlightDto);
        Task UpdateFlightDetailsAsync(UpdateFlightDetailsDto updateFlightDetailsDto);
    }

}
