using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Profiles.DriverMapping
{
    public class DriverConfig : Profile
    {
        public DriverConfig()
        {

            CreateMap<DriverRegisterDto, User>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<User, Driver>()
            .ForMember(dest => dest.Driverearnings, opt => opt.MapFrom(src => 0.00m))
            .ForMember(dest => dest.DriverRating, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.DriverStatusId, opt => opt.MapFrom(src => AppConstant.Available))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

            CreateMap<DriverRegisterDto, Driver>()
            .ForMember(dest => dest.LicenceNumber, opt => opt.MapFrom(src => src.LicenseNumber)).ReverseMap();

            CreateMap<DriverTaxiDto, Taxi>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))
           .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
           .ForMember(dest => dest.TaxiTypeId, opt => opt.MapFrom(src => src.TaxiTypeId));

            CreateMap<DriverRatingDto, DriverRating>().ReverseMap();

            CreateMap<DriverDisplayDto, User>().ReverseMap();

            CreateMap<Ride,DriverGetRideDto>()
            .ForMember(dest => dest.RideId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TaxiType, opt => opt.MapFrom(src => src.TaxiType))
            .ForMember(dest => dest.EstimatedTimeArrival, opt => opt.Ignore())
            .ForMember(dest => dest.PickupLocation, opt => opt.Ignore())
            .ForMember(dest => dest.DropoffLocation, opt => opt.Ignore());

            CreateMap<Ride, DriverRideDisplayDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            .ForMember(dest => dest.DropoffLocation, opt=>opt.Ignore())
            .ForMember(dest => dest.PickupLocation, opt => opt.Ignore())
            .ForMember(dest => dest.TaxiType, opt => opt.MapFrom(src => src.TaxiType.Name))
            .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer.User.Name))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Payment.TotalFareAmount));

        }

    }
}
