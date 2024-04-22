//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Threading.Tasks;
//using TaxiBookingService.API.User.Admin;
//using TaxiBookingService.Common.Enums;
//using TaxiBookingService.Dal.Entities;
//using TaxiBookingService.Dal.Interfaces;

//namespace TaxiBookingService.Dal.Repositories
//{
//    public class AdminRepository : IAdminRepository<Admin>
//    {
//        private readonly TaxiBookingServiceDbContext _context;

//        public AdminRepository(TaxiBookingServiceDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<int> Register(AdminRegisterServiceContracts request, byte[] passwordHash, byte[] passwordSalt)
//        {
//            var user = new User
//            {
//                PasswordHash = passwordHash,
//                PasswordSalt = passwordSalt,
//                Email = request.Email,
//                Name = request.Name,
//                PhoneNumber = request.PhoneNumber,
//                Role = UserRole.Admin,
//            };

//            var Admin = new Admin
//            {
//                User = user,
//            };

//            _context.Admin.Add(Admin);
//            _context.SaveChanges();
//            return Admin.Id;
//        }

//        public async Task<Admin> GetById(int id)
//        {
//            return await _context.Admin.FindAsync(id);
//        }

//        public async Task<Admin> GetByEmail(string email)
//        {
//            return await _context.Admin.Include(d => d.User).FirstOrDefaultAsync(d => d.User.Email == email);
//        }

//        public async Task<Admin> GetByToken(string token)
//        {
//            return await _context.Admin.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
//        }

//        public async Task UpdateRefreshToken(Admin Admin, RefreshToken newRefreshToken)
//        {
//            Admin.User.RefreshToken = newRefreshToken.Token;
//            Admin.User.TokenCreated = newRefreshToken.Created;
//            Admin.User.TokenExpires = newRefreshToken.Expires;

//        }

//        public async Task Login(Admin Admin)
//        {
//            Admin.User.IsActive = true;
//        }

//        public async Task Logout(Admin Admin)
//        {
//            Admin.User.RefreshToken = null;
//            Admin.User.TokenCreated = DateTime.MinValue;
//            Admin.User.TokenExpires = DateTime.UtcNow;
//            Admin.User.IsActive = false;
//        }


//    }
//}
