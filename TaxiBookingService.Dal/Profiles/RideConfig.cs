using AutoMapper;
using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Profiles.RideMapping
{
    public class RideConfig : Profile
    {
        public RideConfig() {
            CreateMap<CancellationDto, RideCancellationReason>().ReverseMap();
            CreateMap<ScheduledRide, CustomerScheduleRideDTO>().ReverseMap();

        }

    }
}
