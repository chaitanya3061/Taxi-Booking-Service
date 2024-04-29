using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TaxiBookingServiceDbContext _context;
        public UserRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByDriverId(int driverId)
        {
            var user = await _context.User
            .Include(u => u.Drivers)
            .FirstOrDefaultAsync(u => u.Drivers.Any(d => d.Id == driverId));
            return user;
        }

        public async Task<User> GetByToken(string token)
        {
            return await _context.User.FirstOrDefaultAsync(d => d.RefreshToken == token);
        }

        public async Task UpdateRefreshToken(User user, RefreshToken newRefreshToken)
        {
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        public async Task<Role> GetRoleById(int userId)
        {
            var user = await _context.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);
            return user.Role;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.User.FirstOrDefaultAsync(d => d.Email == email);
        }

        public async Task Logout(User user)
        {
            user.IsActive = false;
            user.RefreshToken = null;
            user.TokenExpires = DateTime.UtcNow;
            user.TokenExpires = DateTime.MinValue;
        }

        public async Task Login(User user)
        {
            user.IsActive = true;
        }
        public async Task<bool> IsEmail(string email)
        {
            return  _context.User.Any(x => x.Email == email);
        }
    }
}
