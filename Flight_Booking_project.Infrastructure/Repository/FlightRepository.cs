using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Infrastructure.Repository
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightBookingContext _context;

        public FlightRepository(FlightBookingContext context)
        {
            _context = context;
        }

        public async Task<Airport> GetAirportByNameAsync(string airportName)
        {
            if (string.IsNullOrEmpty(airportName))
            {
                throw new ArgumentNullException(nameof(airportName), "Airport name cannot be null or empty");
            }

            return await _context.Airports.FirstOrDefaultAsync(a => a.Name.ToLower() == airportName.ToLower())
                   ?? throw new FileNotFoundException($"Airport with name '{airportName}' not found.");
        }

       /* public async Task<Airport> GetAirportByNameAsync(string airportName)
        {
            return await _context.Airports
                .FirstOrDefaultAsync(a => a.Name.ToLower() == airportName.ToLower());
        }*/
        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
            .Include(f => f.Stops)
           .ThenInclude(s => s.Airport)  // Ensure that the airport for each stop is included
           .Include(f => f.Seats)
           .Include(f => f.Airline)
           .Include(f => f.DepartureAirport)
           .Include(f => f.ArrivalAirport)
           .FirstOrDefaultAsync(f => f.FlightId == flightId);

        }

        public async Task<List<Flight>> SearchFlightsAsync(int departureAirportId, int arrivalAirportId, string DepartureAirportName, string ArrivalAirportName, string ClassType, DateTime DepartureDate, int NumberOfPassengers)
        {
            var query = _context.Flights
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.Airline)
                .Include(f => f.Seats)
                .Include(f => f.Stops)
                .AsQueryable();

            query = query.Where(f => f.DepartureAirportId == departureAirportId && f.ArrivalAirportId == arrivalAirportId);

            // Filter by class type and availability of seat
            if (!string.IsNullOrEmpty(ClassType))
            {
                query = query.Where(f => f.Seats.Any(s => s.ClassType ==ClassType && s.IsAvailable));
            }
            if (DepartureDate < DateTime.Now)
            {
                throw new Exception("Date sholud not be less than todays date");
            }
            else
            {
                if (DepartureDate != null)
                {
                    query = query.Where(f => f.DepartureTime.Date == DepartureDate.Date);
                }
            }
            return await query.ToListAsync();
        }


        public async Task<bool> CheckSeatAvailabilityAsync(int flightId, string classType, int passengerCount)
        {
            var flight = await _context.Flights
                .Include(f => f.Seats)
                .FirstOrDefaultAsync(f => f.FlightId == flightId);

            var availableSeats = flight.Seats.Count(s => s.ClassType == classType && s.IsAvailable);

            return availableSeats >= passengerCount;
        }

        /* public async Task<List<Flight>> SearchFlightsByAdvanceFilterAsync( decimal? MinPrice, decimal? MaxPrice, string? AirlineName, int? NumberOfStops)
         {
             var query = _context.Flights
         .Include(f => f.Seats)
         .Include(f => f.Airline)
         .Include(f => f.Stops)
         .AsQueryable();

         *//*
             query = query.Where(f => f.DepartureAirportId == departureAirportId && f.ArrivalAirportId == arrivalAirportId);

             // Filter by class type and availability of seats
             if (!string.IsNullOrEmpty(classType))
             {
                 query = query.Where(f => f.Seats.Any(s => s.ClassType == classType && s.IsAvailable));
             }

           *//*


             // Filter by minimum price
             if (MinPrice.HasValue)
             {
                 query = query.Where(f => f.Seats.Any(s => s.Price >= MinPrice.Value));
             }

             // Filter by maximum price
             if (MaxPrice.HasValue)
             {
                 query = query.Where(f => f.Seats.Any(s => s.Price <= MaxPrice.Value));
             }

             // Filter by airline name
             if (!string.IsNullOrEmpty(AirlineName))
             {
                 query = query.Where(f => f.Airline.AirlineName.ToLower() == AirlineName.ToLower());
             }

             // Filter by number of stops
             if (NumberOfStops.HasValue)
             {
                 query = query.Where(f => f.Stops.Count == NumberOfStops.Value);
             }



             return await query.ToListAsync();

         }*/



        public async Task<bool> DeleteFlightAsync(int flightId)

        {

            // Include related entities (Seats, Stops, and Bookings)

            var flight = await _context.Flights

                                       .Include(f => f.Seats)

                                       .Include(f => f.Stops)

                                       .Include(f => f.Bookings)

                                       .ThenInclude(b => b.Passengers)  // Include passengers for each booking

                                       .FirstOrDefaultAsync(f => f.FlightId == flightId);


            if (flight == null)

            {

                return false; // Flight not found

            }


            // Remove related bookings explicitly if cascading deletes are not configured

            _context.Bookings.RemoveRange(flight.Bookings);


            // Remove the flight (cascade should handle Seats and Stops)

            _context.Flights.Remove(flight);

            await _context.SaveChangesAsync(); // Saves all changes (Bookings, Seats, Stops, and Flight deletions)


            return true;

        }
        public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
        {
            return await _context.Flights
                                 .Include(f => f.Airline)  // Include related entities to fetch necessary data
                                 .Include(f => f.DepartureAirport)
                                 .Include(f => f.ArrivalAirport)
                                 .Include(f => f.Stops)
                                 .Include(f => f.Seats)
                                 .ToListAsync();
        }
        public async Task AddFlightAsync(Flight flight)
        {
            await _context.Flights.AddAsync(flight);
            await _context.SaveChangesAsync();
        }
        public async Task<Airline> GetAirlineByNameAsync(string airlineName)
        {
            return await _context.Airlines
                .FirstOrDefaultAsync(a => a.AirlineName.ToLower() == airlineName.ToLower());
        }
        public async Task UpdateFlightAsync(Flight flight)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Use the Update method to automatically track changes to the flight and its related entities
                    _context.Flights.Update(flight);

                    // Save the changes
                    await _context.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback the transaction in case of an error
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }



    }

}
