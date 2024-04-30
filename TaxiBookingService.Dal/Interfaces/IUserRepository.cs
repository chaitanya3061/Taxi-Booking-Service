using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IUserRepository :IRepository<User>
    {
        Task<User>GetByDriverId(int driverId);
        Task<User> GetByToken(string token);
        Task UpdateRefreshToken(User user, RefreshToken newRefreshToken);
        Task<Role> GetRoleById(int userId);
        Task<User> GetByEmail(string email);
        Task Logout(User user);
        Task Login(User user);
        Task<bool> IsEmail(string email);

    }
}
