using AutoMapper;
using Flight_Booking_project.Application.IRepository;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using Flight_Booking_project.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Flight_Booking_project.Infrastructure.Repository
{
    public class BookingRepository : IBookingRepository
    {

        private readonly FlightBookingContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(FlightBookingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            return await _context.Flights
                .Include(f => f.Seats)
                .SingleOrDefaultAsync(f => f.FlightId == flightId);
        }

        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking; // No transaction management here, handled in service layer
        }

        //public async Task<Seat> AddSeatAsync(Seat seat)
        //{
        //    await _context.Seats.AddAsync(seat);
        //    await _context.SaveChangesAsync();
        //    return seat;
        //}

        public async Task<Passenger> AddPassengerAsync(Passenger passenger)
        {
            await _context.Passengers.AddAsync(passenger);
            await _context.SaveChangesAsync();

            return passenger; // No transaction management here, handled in service layer
        }

        /* public async Task<Booking> GetBookingByIdAsync(int bookingId)
         {
             return await _context.Bookings
                 .Include(b => b.Passengers)
                 .SingleOrDefaultAsync(b => b.BookingId == bookingId);
         }
 */
        public async Task DeleteBookingAsync(int bookingId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    var booking = await _context.Bookings
                        .Include(b => b.Passengers)
                        .SingleOrDefaultAsync(b => b.BookingId == bookingId);

                    if (booking != null)
                    {
                        _context.Bookings.Remove(booking);

                        // Remove associated passengers
                        _context.Passengers.RemoveRange(booking.Passengers);

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                }
            }
        }




        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(string userId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Flight)
                    .ThenInclude(f => f.Airline)
                .Include(b => b.Flight.DepartureAirport)
                .Include(b => b.Flight.ArrivalAirport)
                .Include(b => b.Flight)
                .ThenInclude(f => f.Stops).ThenInclude(s => s.Airport)
                .Include(b => b.Flight.Seats)
                .Include(b => b.Passengers)
                    .ThenInclude(p => p.Seat)
                .ToListAsync();

           

            return bookings;
        


        // return await _context.Bookings.Include(b => b.Flight).ThenInclude(b=>b.Seats).Where (i=>i.UserId == userId).ToListAsync();
        /* .GroupBy(b => new
         {
             b.BookingId,
             b.BookingDate
         })
         .Select(g => new Booking
         {
             BookingId = g.Key.BookingId,
             BookingDate = g.Key.BookingDate
         })
         .ToListAsync();*/
    }


    public async Task<IEnumerable<Booking>> GetBookingsByFlightIdAsync(int flightId)
        {
            return await _context.Bookings
                .Where(b => b.FlightId == flightId)
                .Include(b => b.Passengers) // Include passengers if needed for the deletion
                .ToListAsync();
        }


        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Passengers)
                .ThenInclude(p => p.Seat)
                .Include(b => b.Flight)
                .ThenInclude(f => f.Airline)
                 .Include(b => b.Flight.DepartureAirport)
                .Include(b => b.Flight.ArrivalAirport)
                .Include(b => b.Flight)
                .ThenInclude(f=>f.Stops).ThenInclude(s=>s.Airport)
                .SingleOrDefaultAsync(b => b.BookingId == bookingId);
        }
        public async Task<decimal> CalculateTotalCostAsync(int bookingId)
        {
            var booking = await GetBookingByIdAsync(bookingId);
            if (booking == null || booking.Passengers == null || booking.Passengers.Count == 0)
            {
                return 0; // Or throw an exception if appropriate
            }

            // Calculate total cost
            decimal totalCost = booking.Passengers.Sum(p => p.Seat.Price);
            return totalCost;
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Fetch the booking and include passengers and flight
                    var booking = await _context.Bookings
                        .Include(b => b.Passengers)
                        .Include(b => b.Flight)
                        .Include(b => b.Flight.Seats)
                        .SingleOrDefaultAsync(b => b.BookingId == bookingId);

                    if (booking == null)
                    {
                        return false; // Booking not found
                    }

                    var departureTime = booking.Flight.DepartureTime;

                    if (booking.StatusId == -1)
                    {
                        // Check if the departure time has passed
                        if (departureTime < DateTime.Now)
                        {
                            booking.StatusId = 1; // Status for "flight already departed"

                            // Update the booking status in the database
                            _context.Bookings.Update(booking);
                            await _context.SaveChangesAsync();

                            // Exit the method without removing the booking
                            await transaction.CommitAsync();
                            return true;
                        }
                        //else
                        //{
                        //    booking.StatusId = 0; // Status for "flight yet to depart"
                        //                          // Update the booking status in the database



                        //    _context.Bookings.Update(booking);
                        //    await _context.SaveChangesAsync();

                        //    /* _context.Passengers.RemoveRange(booking.Passengers);
                        //     _context.Bookings.Remove(booking);
                        //     await _context.SaveChangesAsync();*/
                        //    await transaction.CommitAsync();
                        //    return true;

                        //}
                    }

                   
                    
                        booking.StatusId = 0; // Status for "flight yet to depart"
                                              // Update the booking status in the database


                        _context.Bookings.Update(booking);
                        await _context.SaveChangesAsync();

                        /* _context.Passengers.RemoveRange(booking.Passengers);
                         _context.Bookings.Remove(booking);
                         await _context.SaveChangesAsync();*/

                        foreach (var seat in booking.Flight.Seats)
                        {
                            seat.IsAvailable = true; // Set `IsAvailable` to true
                        }

                        _context.Flights.Update(booking.Flight); // Update the flight entity with seat changes
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();
                        return true;

                    

                   


                    // Remove the booking and associated passengers
                    // Commit the changes


                }
                catch (Exception ex)
                {
                    // Rollback in case of any failure
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }




    }
}
