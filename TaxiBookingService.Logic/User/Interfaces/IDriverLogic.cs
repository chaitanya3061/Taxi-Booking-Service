using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IDriverLogic<T>
    {
        Task<int> Register(DriverRegisterServiceContracts request);
        Task<string> Login(DriverLoginServiceContracts request);
        Task<string> RefreshToken();
        Task Logout();
        Task<int> AddTaxi(DriverTaxiServiceContracts taxi);
        Task UpdateTaxi(int taxiId, DriverTaxiServiceContracts taxi);
        Task<string>Accept(int DriverId,int rideId);
        Task Decline(int DriverId, int rideId);
        Task StartRide(int rideId);
        Task<decimal> EndRide(int rideId);
        Task CancelRide(int rideId,string reason);    
    }
}
