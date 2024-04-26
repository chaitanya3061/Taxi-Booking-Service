using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
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
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

            CreateMap<DriverTaxiDto, Taxi>()
               .ForMember(dest => dest.TaxiTypeId, opt => opt.Ignore())
               .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
               .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore())
               .ForMember(dest => dest.TaxiTypeId, opt => opt.Ignore())
               .ForMember(dest => dest.DriverId, opt => opt.Ignore())
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
               .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))
               .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

            CreateMap<DriverRatingDto, DriverRating>().ReverseMap();
            CreateMap<DriverDisplayDto, Driver>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => new User
                {
                    Name = src.Name,
                    Email = src.Email,
                    PhoneNumber = src.PhoneNumber
                }))
                .ForMember(dest => dest.DriverRating, opt => opt.MapFrom(src => src.DriverRating))
                .ReverseMap();
            CreateMap<DriverGetRideDto, Ride>().ReverseMap();
        }

    }
}
