using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public AdminRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> GetById(int id)
        {
            return await _context.Admin.FindAsync(id);
        }

        public async Task<Admin> GetByToken(string token)
        {
            return await _context.Admin.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
        }
    }
}
