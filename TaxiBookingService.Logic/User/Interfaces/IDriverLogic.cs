using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IDriverLogic<T>
    {
        Task<int> Register(DriverRegisterDto request);
        Task<string> Login(DriverLoginDto request);
        Task<string> RefreshToken();
        Task Logout();
        Task<int> AddTaxi(DriverTaxiDto taxi);
        Task<string>Accept(int DriverId,int rideId);
        Task Decline(DriverDeclineDto request);
        Task StartRide(int rideId);
        Task<decimal> EndRide(int rideId);
        Task CancelRide(int rideId,string reason);   
        
        Task FeedBack(DriverRatingDto rating);

        Task<List<DriverRideDisplayDto>> RideHistory();
    }
}
