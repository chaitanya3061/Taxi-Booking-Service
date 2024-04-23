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
    public class DriverConfig:Profile
    {
        public DriverConfig() {

            CreateMap<DriverRegisterDto, User>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

            CreateMap<DriverTaxiDto, Taxi>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.RegistrationNumber))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color)).ReverseMap();

            CreateMap<DriverRatingDto, DriverRating>().ReverseMap();

        }

    }
}
