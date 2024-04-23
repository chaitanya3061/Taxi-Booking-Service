using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Profiles.RideMapping
{
    public class RideConfig : Profile
    {
        public RideConfig() {
            CreateMap<CancellationDto, RideCancellationReason>().ReverseMap();

        }

    }
}
