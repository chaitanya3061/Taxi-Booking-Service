using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Profiles.CustomerMapping
{
    public class CustomerConfig : Profile
    {
        public CustomerConfig()
        {
            CreateMap<CustomerRegisterDto, User>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
          .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
          
          .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber)).ReverseMap();

           CreateMap<CustomerRatingDto, CustomerRating>().ReverseMap();
        }
    }
}
