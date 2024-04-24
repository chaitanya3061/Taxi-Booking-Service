using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IUserLogic<T, TRegisterDto, TLoginDto> where T : class where TRegisterDto : class where TLoginDto : class
    {
        Task<int> Register(TRegisterDto request);
        Task<(string, string)> Login(TLoginDto request);
        Task<string> RefreshToken();
        Task Logout();
    }
}
