using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IUserLogic
    {
        Task<string> Login(UserLoginDto request);
        Task<string> RefreshToken();
        Task Logout();
    }
}
