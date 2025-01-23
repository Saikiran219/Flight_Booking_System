using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;

namespace Flight_Booking_project.Application.IRepository
{
    public interface IFlightRepository
    {
        Task<Flight> GetFlightByIdAsync(int flightId);
        Task<Airport> GetAirportByNameAsync(string airportName);
        Task<Airline> GetAirlineByNameAsync(string airlineName);
        Task<bool> CheckSeatAvailabilityAsync(int flightId, string classType, int passengerCount);
        Task<List<Flight>> SearchFlightsAsync(int departureAirportId, int arrivalAirportId, string DepartureAirportName, string ArrivalAirportName, string ClassType, DateTime DepartureDate, int NumberOfPassengers);
        Task<bool> DeleteFlightAsync(int flightId);
        Task AddFlightAsync(Flight flight);
        Task UpdateFlightAsync(Flight flight);
        Task<IEnumerable<Flight>> GetAllFlightsAsync();

        /*
        Task<List<Flight>> SearchFlightsByAdvanceFilterAsync(decimal? MinPrice,decimal? MaxPrice,string? AirlineName,int? NumberOfStops);

       */



    }

}
