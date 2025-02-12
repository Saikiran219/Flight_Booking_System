using Flight_Booking_project.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Flight_Booking_project.Infrastructure.Data
{
    public class FlightBookingContext : IdentityDbContext<User>
    {
        public FlightBookingContext(DbContextOptions<FlightBookingContext> options) : base(options) { }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many between Airline and Flight
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Airline)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AirlineId);

            // One-to-many between Flight and Seats
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Flight)
                .WithMany(f => f.Seats)
                .HasForeignKey(s => s.FlightId);

            //one to one between stop and aiport
            modelBuilder.Entity<Stop>()
                   .HasOne(s => s.Airport)
                   .WithOne()
                   .HasForeignKey<Stop>(s => s.AirportId);

            // One-to-many between Flight and Stops
            modelBuilder.Entity<Stop>()
            .HasOne(s => s.Flight)
            .WithMany(f => f.Stops)
            .HasForeignKey(s => s.FlightId);


            // One-to-many between Airport and Flight (Departure and Arrival)

            modelBuilder.Entity<Flight>()
            .HasOne(f => f.DepartureAirport)
            .WithMany(a => a.DepartureFlights)
            .HasForeignKey(f => f.DepartureAirportId);

            modelBuilder.Entity<Flight>()
           .HasOne(f => f.ArrivalAirport)
           .WithMany(a => a.ArrivalFlights)
           .HasForeignKey(f => f.ArrivalAirportId);

            // One-to-many between Booking and Passenger
            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.Booking)
                .WithMany(b => b.Passengers)
                .HasForeignKey(p => p.BookingId);

            // One-to-many between Flight and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Flight)
                .WithMany(f => f.Bookings)
                .HasForeignKey(b => b.FlightId);


            // One-to-many between User and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Passenger>()
            .HasOne(p => p.Seat)
            .WithOne(s => s.Passenger)
            .HasForeignKey<Passenger>(p => p.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        }
       /* public async Task BeginTransactionAsync()
        {
            await Database.BeginTransactionAsync();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public DatabaseFacade Database => base.Database;*/

    } 
}
