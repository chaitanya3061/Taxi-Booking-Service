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
