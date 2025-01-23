using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Booking_project.Domain.Entities
{
    public class Airport
    {
        [Key]
        public int AirportId { get; set; }
        public string AirportCode { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        // A list of flights that depart from or arrive at this airport

        //one to many
        public ICollection<Flight> DepartureFlights { get; set; }
        public ICollection<Flight> ArrivalFlights { get; set; }
        //many to many
        public ICollection<Airline> Airlines { get; set; }

    }
}
