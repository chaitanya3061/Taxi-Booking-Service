using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class AdminRepository : IAdminRepository<Admin>
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

        public async Task<Admin> GetByEmail(string email)
        {
            return await _context.Admin.Include(d => d.User).FirstOrDefaultAsync(d => d.User.Email == email);
        }

        public async Task<Admin> GetByToken(string token)
        {
            return await _context.Admin.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
        }

        public async Task UpdateRefreshToken(Admin Admin, RefreshToken newRefreshToken)
        {
            Admin.User.RefreshToken = newRefreshToken.Token;
            Admin.User.TokenCreated = newRefreshToken.Created;
            Admin.User.TokenExpires = newRefreshToken.Expires;

        }

        public async Task Login(Admin Admin)
        {

        }

        public async Task Logout(Admin Admin)
        {
            Admin.User.RefreshToken = null;
            Admin.User.TokenCreated = DateTime.MinValue;
            Admin.User.TokenExpires = DateTime.UtcNow;
        }


    }
}
