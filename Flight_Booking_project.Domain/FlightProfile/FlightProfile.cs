using AutoMapper;
using Flight_Booking_project.Domain.Entities;
using Flight_Booking_project.Domain.EntitiesDto.RequestDto;
using Flight_Booking_project.Domain.EntitiesDto.ResponseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Flight_Booking_project.Domain.EntitiesDto
{

    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<Flight, FlightResponseDto>()
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightId))
                .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline.AirlineName))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport.Name))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport.Name))
                .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
                .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
                .ForMember(dest => dest.NumberOfStops, opt => opt.MapFrom(src => src.Stops.Count))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Seats.FirstOrDefault().Price))
                .ForMember(dest => dest.SeatClass, opt => opt.MapFrom(src => src.Seats.FirstOrDefault().ClassType));

            CreateMap<Flight, FlightDetailsResponseDto>()
            .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightId))
            .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline.AirlineName))
            .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport.Name))
            .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport.Name))
            .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
            .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
            .ForMember(dest => dest.StopCount, opt => opt.MapFrom(src => src.Stops != null ? src.Stops.Count : 0))
            .ForMember(dest => dest.BaggageAllowance, opt => opt.MapFrom(src => src.Airline.BaggageAllowance))
            .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops != null ? src.Stops.Select(s => new StopDetailsDto
            {
                StopId=s.StopId,
                AirportName = s.Airport != null ? s.Airport.Name : string.Empty,
                StopDuration = s.StopTime
            }).ToList() : new List<StopDetailsDto>()))
            .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats != null ? src.Seats.Select(seat => new SeatDetailsDto
            {
                SeatId=seat.SeatId,
                SeatNumber = seat.SeatNumber,
                SeatClass = seat.ClassType,
                SeatPosition = seat.Position,
                Price = seat.Price,
                IsAvailable = seat.IsAvailable, 
            }).ToList() : new List<SeatDetailsDto>()));

            CreateMap<Stop, StopDetailsDto>()
                 .ForMember(dest => dest.StopId, opt => opt.MapFrom(src => src.StopId))
               .ForMember(dest => dest.AirportName, opt => opt.MapFrom(src => src.Airport != null ? src.Airport.Name : string.Empty))
               .ForMember(dest => dest.StopDuration, opt => opt.MapFrom(src => src.StopTime));
                


            // Mapping from FlightSearchRequestDto to Flight entity
            CreateMap<FlightBasicSearchRequestDto, Flight>()
                // We don't directly map these string fields to the Flight entity
                // because they're used for searching, not setting.
                .ForMember(dest => dest.DepartureAirport, opt => opt.Ignore()) // Airport is mapped in query, not directly
                .ForMember(dest => dest.ArrivalAirport, opt => opt.Ignore()) // Same as above
                .ForMember(dest => dest.Seats, opt => opt.Ignore()) // Seats are not mapped directly in search
                .ForMember(dest => dest.Airline, opt => opt.Ignore()); // Airline is handled similarly in the service



            CreateMap<RegisterDto, User>()

          .ForMember(dest => dest.AlternativeContactNumber, opt => opt.AllowNull()); // Allow null values

            CreateMap<User, UserDto>();
            CreateMap<User, LoginResultDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore()); // Token is generated separately

            CreateMap<BookingRequestDto, Booking>()
              .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers));

            CreateMap<PassengerRequestDto, Passenger>();
            CreateMap<SeatBookingDto, Seat>()
                .ForMember(dest=>dest.SeatId,opt => opt.MapFrom(src => src.seatId))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.SeatNumber))
                .ForMember(dest => dest.ClassType, opt => opt.MapFrom(src => src.ClassType))
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.SeatPosition));

            // Domain Model to DTO
            CreateMap<Booking, BookingDetailsDto>()
                .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers))
                 .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            CreateMap<Passenger, PassengerDto>();
            CreateMap<Seat, SeatDto>();

            CreateMap<Booking, BookingsByUser>()
            .ForMember(dest => dest.Flight, opt => opt.MapFrom(src => src.Flight))
            
            .ForMember(dest => dest.Passengers, opt => opt.MapFrom(src => src.Passengers));

            CreateMap<Flight, FlightBookingBYUserDto>()
          .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.FlightId))
          .ForMember(dest => dest.Airline, opt => opt.MapFrom(src => src.Airline.AirlineName))
          .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport.Name))
          .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport.Name))
          .ForMember(dest => dest.DepartureTime, opt => opt.MapFrom(src => src.DepartureTime))
          .ForMember(dest => dest.ArrivalTime, opt => opt.MapFrom(src => src.ArrivalTime))
          .ForMember(dest => dest.StopCount, opt => opt.MapFrom(src => src.Stops != null ? src.Stops.Count : 0))
          .ForMember(dest => dest.BaggageAllowance, opt => opt.MapFrom(src => src.Airline.BaggageAllowance))
            .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops));
            
            CreateMap<Seat,BookingSeatDto>()
               
                .ForMember(dest=>dest.SeatPosition,opt=>opt.MapFrom(src=>src.Position))
                .ForMember(dest => dest.SeatNumber, opt => opt.MapFrom(src => src.SeatNumber))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));


            CreateMap<Flight, FlightDetailsDtoByAdmin>()
.ForMember(dest => dest.AirlineName, opt => opt.MapFrom(src => src.Airline.AirlineName)) // Assuming Airline has a Name property
.ForMember(dest => dest.DepartureAirportName, opt => opt.MapFrom(src => src.DepartureAirport.Name)) // Assuming Airport has a Name property
.ForMember(dest => dest.ArrivalAirportName, opt => opt.MapFrom(src => src.ArrivalAirport.Name))
//.ForMember(dest => dest.StopNames, opt => opt.MapFrom(src => src.Stops.Select(s => s.Airport.Name).ToList())); // Assuming Stop has AirportStopName
.ForMember(dest => dest.StopNames, opt => opt.MapFrom(src => src.Stops != null ? src.Stops.Select(s => s.Airport.Name).ToList() : new List<string>()))
.ForMember(dest => dest.SeatDetails, opt => opt.MapFrom(src => src.Seats));  // Map Seats to SeatDetails in DTO
        }
    }
}