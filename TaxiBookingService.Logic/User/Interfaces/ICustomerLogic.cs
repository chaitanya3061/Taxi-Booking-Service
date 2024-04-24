using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface ICustomerLogic<T>
    {
        Task<int> Register(CustomerRegisterDto request);
        Task<(string, string)> Login(CustomerLoginDto request);
        Task<string> RefreshToken();
        Task<int> BookRide(CustomerBookRideDto request);
        Task Logout();
        Task CancelRide(int rideId, string reason);
        Task<Driver> GetDriverAsync(int id);
        Task FeedBack(CustomerRatingDto rating);

        Task<List<CustomerRideDisplayDto>> RideHistory();

    }
}
