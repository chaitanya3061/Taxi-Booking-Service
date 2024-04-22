using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface ICustomerLogic<T>
    {
        Task<int> Register(CustomerRegisterServiceContracts request);
        Task<(string, string)> Login(CustomerLoginServiceContracts request);
        Task<string> RefreshToken();
        Task<List<TaxiType>> GetAllTaxiTypes();
        Task<int> BookRide(CustomerBookServiceContracts request);
        Task Logout();
        Task CancelRide(int rideId, string reason);
        Task<Driver> GetDriverAsync(int id);


    }
}
