using AutoMapper;
using Flight_Booking_project.Application.Interfaces;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Flight_Booking_project.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IMapper _mapper;
        private readonly ISeatRepository _seatRepository;


        /*private int _cachedDepartureAirportId;
        private int _cachedArrivalAirportId;
        private string _cachedClassType;
        private DateTime _cacheddepatureDate;*/
        public FlightService(IFlightRepository flightRepository, IMapper mapper, ISeatRepository seatRepository)
        {
            _flightRepository = flightRepository;
            _mapper = mapper;
            _seatRepository = seatRepository;
        }

        public async Task<List<FlightResponseDto>> SearchFlightsAsync(string DepartureAirportName, string ArrivalAirportName, string ClassType, DateTime DepartureDate, int NumberOfPassengers)
        {
            if (string.IsNullOrEmpty(DepartureAirportName) || string.IsNullOrEmpty(ArrivalAirportName))
            {
                throw new Exception("Both Departure and Arrival Airport Names are required.");
            }
            // Validation to ensure departure and arrival airports are not the same
            if (DepartureAirportName.Equals(ArrivalAirportName, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Departure and Arrival airports must be different.");
            }

            // Get the departure and arrival airports
            var departureAirport = await _flightRepository.GetAirportByNameAsync(DepartureAirportName);
            var arrivalAirport = await _flightRepository.GetAirportByNameAsync(ArrivalAirportName);



            if (departureAirport == null || arrivalAirport == null)
            {
                throw new Exception("Departure or Arrival airport not found");
            }
         
            var flights = await _flightRepository.SearchFlightsAsync(departureAirport.AirportId, arrivalAirport.AirportId, 
                DepartureAirportName, ArrivalAirportName, ClassType, 
                DepartureDate,  NumberOfPassengers);

            var availableFlights = new List<Flight>();
            foreach (var flight in flights)
            {
                var isAvailable = await _flightRepository.CheckSeatAvailabilityAsync(
                    flight.FlightId, ClassType, NumberOfPassengers);

                if (isAvailable)
                {
                   
                    availableFlights.Add(flight);
                }
            }

            if (availableFlights == null || !availableFlights.Any())
            {
                throw new Exception("No flights found based on the search criteria.");
            }

            // Map the flight data to FlightResponseDto
            var flightDtos = _mapper.Map<List<FlightResponseDto>>(availableFlights);
            foreach (var flightDto in flightDtos)
            {
                // Get the price based on ClassType for each flight
                var flight = availableFlights.FirstOrDefault(f => f.FlightId.ToString() == flightDto.FlightNumber);
                if (flight != null)
                {
                    // Find the seat with the correct ClassType and map the price
                    var seat = flight.Seats.FirstOrDefault(s => s.ClassType.Equals(ClassType, StringComparison.OrdinalIgnoreCase));
                    if (seat != null)
                    {
                        flightDto.Price = seat.Price;
                        flightDto.SeatClass = seat.ClassType?? "";
                    }
                }
            }

            return flightDtos;
           
        }

      /* public async Task<List<FlightResponseDto>> SearchFlightsByAdvanceFilterAsync(decimal? MinPrice, decimal? MaxPrice, string? AirlineName, int? NumberOfStops)
        {

            *//*if (_cachedDepartureAirportId == 0 || _cachedArrivalAirportId == 0 || string.IsNullOrEmpty(_cachedClassType) || _cacheddepatureDate == default(DateTime))
            {
                throw new Exception("Basic search must be completed before applying advanced filters.");
            }*//*
            var flights = await _flightRepository.SearchFlightsByAdvanceFilterAsync( MinPrice,  MaxPrice,  AirlineName, NumberOfStops);
            if (flights == null || !flights.Any())
            {
                throw new Exception("No flights found based on the search criteria.");
            }
            return _mapper.Map<List<FlightResponseDto>>(flights);

        }*/

        public async Task<FlightDetailsResponseDto> GetFlightByIdAsync(int flightId)
        {
            var flight = await _flightRepository.GetFlightByIdAsync(flightId);

            if (flight == null)
            {
                throw new Exception("Flight not found");
            }

            // Using AutoMapper to map the flight entity to FlightDetailsDto
            var flightDetailsDto = _mapper.Map<FlightDetailsResponseDto>(flight);

            return flightDetailsDto;
        }

        public async Task<List<Seat>> GetAvailableSeatsAsync(int flightId, string seatClass)
        {
            var availableSeats = await _seatRepository.GetAvailableSeatsAsync(flightId, seatClass);

            // Business logic: Validate if seats are available
            if (availableSeats == null || !availableSeats.Any())
            {
                throw new Exception("No available seats found for the selected class.");
            }

            return availableSeats;
        }
        public async Task<string> DeleteFlightByIdAsync(int flightId)

        {

            var isDeleted = await _flightRepository.DeleteFlightAsync(flightId);


            if (!isDeleted)

            {

                return "Flight not found or could not be deleted.";

            }


            return "Flight and its related details successfully deleted.";

        }
        /* public async Task<IEnumerable<FlightDetailsDtoByAdmin>> GetAllFlightsAsync()
         {
             // Fetch all flights from the repository
             var flights = await _flightRepository.GetAllFlightsAsync();

             // Use AutoMapper to map Flight entities to FlightDto objects
             var flightDtos = _mapper.Map<IEnumerable<FlightDetailsDtoByAdmin>>(flights);
             return flightDtos;
         }*/



        public async Task AddFlightAsync(AddFlightDto addFlightDto)
        {
            // Fetch DepartureAirportId and ArrivalAirportId using names
            var departureAirport = await _flightRepository.GetAirportByNameAsync(addFlightDto.DepartureAirportName);
            if (departureAirport == null)
            {
                throw new Exception("Invalid departure airport name");
            }

            var arrivalAirport = await _flightRepository.GetAirportByNameAsync(addFlightDto.ArrivalAirportName);
            if (arrivalAirport == null)
            {
                throw new Exception("Invalid arrival airport name");
            }

            // Check that the departure time is at least one hour ahead of the current time
            var currentTime = DateTime.UtcNow;
            if (!(addFlightDto.DepartureTime >= currentTime.AddHours(1)))
            {
                throw new Exception("Departure time must be at least one hour from now.");
            }

            // Ensure Departure and Arrival Airports are not the same
            if (departureAirport.AirportId == arrivalAirport.AirportId)
            {
                throw new Exception("Departure airport and arrival airport cannot be the same.");
            }

            // Fetch AirlineId using the name
            var airline = await _flightRepository.GetAirlineByNameAsync(addFlightDto.AirlineName);
            if (airline == null)
            {
                throw new Exception("Invalid airline name");
            }


            // Map DTO to Flight entity
            var flight = new Flight
            {
                DepartureAirportId = departureAirport.AirportId,  // Use fetched AirportId
                ArrivalAirportId = arrivalAirport.AirportId,      // Use fetched AirportId
                AirlineId = airline.AirlineId,                    // Use fetched AirlineId
                DepartureTime = addFlightDto.DepartureTime,
                ArrivalTime = addFlightDto.ArrivalTime,
                Stops = new List<Stop>(),
                Seats = new List<Seat>(),
                Bookings = new List<Booking>()
            };
            // Check that the arrival time is at least one hour ahead of the departure time
            if (!(flight.ArrivalTime > flight.DepartureTime.AddHours(1)))
            {
                throw new Exception("Arrival time must be at least one hour after the departure time.");
            }


            // Add Stops if any
            if (addFlightDto.Stops != null && addFlightDto.Stops.Any())
            {
                foreach (var stopDto in addFlightDto.Stops)
                {
                    // Fetch Stop AirportId using name
                    var stopAirport = await _flightRepository.GetAirportByNameAsync(stopDto.StopAirportName);
                    if (stopAirport == null)
                    {
                        throw new Exception($"Invalid stop airport name: {stopDto.StopAirportName}");
                    }

                    // Validate Stop
                    if (stopAirport.AirportId == departureAirport.AirportId || stopAirport.AirportId == arrivalAirport.AirportId)
                    {
                        throw new Exception($"Stop airport '{stopDto.StopAirportName}' cannot be the same as the departure or arrival airport.");
                    }

                    if (stopDto.StopTime <= flight.DepartureTime || stopDto.StopTime >= flight.ArrivalTime)
                    {
                        throw new Exception($"Stop time at '{stopDto.StopAirportName}' must be between departure and arrival times.");
                    }

                    var stop = new Stop
                    {
                        AirportId = stopAirport.AirportId,
                        StopTime = stopDto.StopTime,
                    };
                    flight.Stops.Add(stop);
                }
            }

            // Add Seats if any
            if (addFlightDto.Seats != null && addFlightDto.Seats.Any())
            {
                foreach (var seatDto in addFlightDto.Seats)
                {
                    var seat = new Seat
                    {
                        SeatNumber = seatDto.SeatNumber,
                        Position = seatDto.SeatPosition,
                        ClassType = seatDto.ClassType,
                        IsAvailable = seatDto.IsAvailable,
                        Price = seatDto.Price,
                    };
                    flight.Seats.Add(seat);
                }
            }

            // Save the flight to the repository
            await _flightRepository.AddFlightAsync(flight);
        }
        public async Task UpdateFlightDetailsAsync(UpdateFlightDetailsDto updateFlightDetailsDto)
        {
            // Fetch the flight
            var flight = await _flightRepository.GetFlightByIdAsync(updateFlightDetailsDto.Flight.Id);
            if (flight == null) throw new FileNotFoundException("Flight not found");

            // Validate that departure and arrival airports are not the same
            if (!string.IsNullOrEmpty(updateFlightDetailsDto.Flight.DepartureAirportName) &&
                !string.IsNullOrEmpty(updateFlightDetailsDto.Flight.ArrivalAirportName) &&
                updateFlightDetailsDto.Flight.DepartureAirportName == updateFlightDetailsDto.Flight.ArrivalAirportName)
            {
                throw new ArgumentException("Departure and arrival airports cannot be the same.");
            }
            // Update airline if provided
            if (!string.IsNullOrEmpty(updateFlightDetailsDto.Flight.AirlineName))
            {
                var airline = await _flightRepository.GetAirlineByNameAsync(updateFlightDetailsDto.Flight.AirlineName);
                if (airline == null) throw new FileNotFoundException("Airline not found");
                flight.AirlineId = airline.AirlineId; // Update the foreign key
            }

            // Update departure airport if provided
            if (!string.IsNullOrEmpty(updateFlightDetailsDto.Flight.DepartureAirportName))
            {
                var departureAirport = await _flightRepository.GetAirportByNameAsync(updateFlightDetailsDto.Flight.DepartureAirportName);
                if (departureAirport == null) throw new FileNotFoundException("Departure airport not found");
                flight.DepartureAirportId = departureAirport.AirportId; // Update the foreign key
            }

            // Update arrival airport if provided
            if (!string.IsNullOrEmpty(updateFlightDetailsDto.Flight.ArrivalAirportName))
            {
                var arrivalAirport = await _flightRepository.GetAirportByNameAsync(updateFlightDetailsDto.Flight.ArrivalAirportName);
                if (arrivalAirport == null) throw new FileNotFoundException("Arrival airport not found");
                flight.ArrivalAirportId = arrivalAirport.AirportId; // Update the foreign key
            }

            if (updateFlightDetailsDto.Flight.DepartureTime.HasValue && updateFlightDetailsDto.Flight.DepartureTime.Value <= DateTime.Now)
            {
                throw new ArgumentException("Departure time must be in the future.");
            }

            // Validate and update departure and arrival times
            if (updateFlightDetailsDto.Flight.DepartureTime.HasValue && updateFlightDetailsDto.Flight.ArrivalTime.HasValue)
            {
                if (updateFlightDetailsDto.Flight.DepartureTime.Value >= updateFlightDetailsDto.Flight.ArrivalTime.Value)
                    throw new ArgumentException("Departure time must be earlier than arrival time");

                flight.DepartureTime = updateFlightDetailsDto.Flight.DepartureTime.Value;
                flight.ArrivalTime = updateFlightDetailsDto.Flight.ArrivalTime.Value;
            }

            // Update seats if provided
            if (updateFlightDetailsDto.Seats != null)
            {
                foreach (var seat in updateFlightDetailsDto.Seats)
                {
                    var existingSeats = flight.Seats.Where(s => s.ClassType == seat.ClassType).ToList();  // Check by SeatId
                    if (existingSeats.Any()) // If there are any seats with this ClassType
                    {
                        foreach (var existingSeat in existingSeats)
                        {
                            if (seat.Price.HasValue) existingSeat.Price = seat.Price.Value;
                            if (seat.IsAvailable.HasValue) existingSeat.IsAvailable = seat.IsAvailable.Value;
                            if (!string.IsNullOrEmpty(seat.ClassType)) existingSeat.ClassType = seat.ClassType;
                            if (!string.IsNullOrEmpty(seat.Position)) existingSeat.Position = seat.Position;
                            if (!string.IsNullOrEmpty(seat.SeatNumber)) existingSeat.SeatNumber = seat.SeatNumber;
                        }

                    }
                    else
                    {
                        // Optionally handle adding new seats
                        throw new ArgumentException($"Seat with ID '{seat.SeatId}' does not exist for this flight.");
                    }
                }
            }

            // Update stops if provided
            if (updateFlightDetailsDto.Stops != null)
            {
                foreach (var stop in updateFlightDetailsDto.Stops)
                {
                    // Validate stop time is between departure and arrival times
                    if (stop.StopTime.HasValue &&
                        (stop.StopTime.Value <= flight.DepartureTime || stop.StopTime.Value >= flight.ArrivalTime))
                    {
                        throw new ArgumentException("Stop time must be between the departure and arrival times");
                    }
                    if (stop.StopAirportName == updateFlightDetailsDto.Flight.DepartureAirportName ||
                stop.StopAirportName == updateFlightDetailsDto.Flight.ArrivalAirportName)
                    {
                        throw new ArgumentException("Stop airport cannot be the same as departure or arrival airport.");
                    }


                    var stopAirport = await _flightRepository.GetAirportByNameAsync(stop.StopAirportName);
                    if (stopAirport == null)
                    {
                        // Throw an exception if the airport is not found
                        throw new FileNotFoundException($"Stop airport '{stop.StopAirportName}' not found.");
                    }

                    // Check if the stop already exists
                    var existingStop = flight.Stops.FirstOrDefault(s => s.StopId == stop.StopId); // Check by StopId
                    if (existingStop != null)
                    {
                        if (stop.StopTime.HasValue)
                        {
                            existingStop.StopTime = stop.StopTime.Value;
                        }// Update stop time
                        if (stopAirport.AirportId != existingStop.AirportId)
                        {
                            existingStop.AirportId = stopAirport.AirportId; // Update the airport ID
                        }
                    }
                    else
                    {
                        // Add new stop
                        throw new ArgumentException($"Stop with airport '{stop.StopAirportName}' does not exist for this flight.");
                    }
                }
            }

            // Save the changes to the repository
            await _flightRepository.UpdateFlightAsync(flight);
        }



        public async Task<List<FlightWithDetailsDto>> GetAllFlightsWithDetailsAsync()
        {
            var flights = await _flightRepository.GetAllFlightsAsync(); // Fetch all flights from the repository

            var flightDtos = flights.Select(flight => new FlightWithDetailsDto
            {
                FlightId = flight.FlightId,
                DepartureAirportName = flight.DepartureAirport.Name,
                ArrivalAirportName = flight.ArrivalAirport.Name,
                AirlineName = flight.Airline.AirlineName,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Seats = flight.Seats.Select(seat => new Seatdto
                {
                    SeatId = seat.SeatId,
                    SeatNumber = seat.SeatNumber,
                    ClassType = seat.ClassType,
                    Position = seat.Position,
                    Price = seat.Price,
                    IsAvailable = seat.IsAvailable
                }).ToList(),
                Stops = flight.Stops
                  .Where(stop => stop.Airport != null).
                Select(stop => new StopDto
                {
                    StopId = stop.StopId,
                    StopAirportName =  stop.Airport.Name , // Check if Airport is null
                    StopTime = stop.StopTime
                }).ToList()
            }).ToList();

            return flightDtos;
        }


        public async Task<Flight> GetflightByIdAsync(int flightId)
        {
            return await _flightRepository.GetFlightByIdAsync(flightId);
        }

    }

}
