using AutoMapper;
using TaxiBookingService.API.User.Customer;
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
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<User, Customer>()
            .ForMember(dest => dest.Customerwallet, opt => opt.MapFrom(src => 0.00m))
            .ForMember(dest => dest.CustomerRating, opt => opt.MapFrom(src => 0))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id)); 

            CreateMap<CustomerRegisterDto, Customer>(); 
            CreateMap<CustomerRatingDto, CustomerRating>().ReverseMap();
            CreateMap<CustomerTrustedContactDto, TrustedContacts>().ReverseMap();

            CreateMap<Ride, CustomerRideDisplayDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime))
            .ForMember(dest => dest.DropoffLocation, opt => opt.Ignore())
            .ForMember(dest => dest.PickupLocation, opt => opt.Ignore())
            .ForMember(dest => dest.TaxiType, opt => opt.MapFrom(src => src.TaxiType.Name))
            .ForMember(dest => dest.Driver, opt => opt.MapFrom(src => src.Driver.User.Name))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Payment.TotalFareAmount));
            ;
        }
    }
}
