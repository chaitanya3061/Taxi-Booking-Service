using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class CustomerRepository : ICustomerRepository<Customer>
    {
        private readonly TaxiBookingServiceDbContext _context;

        public CustomerRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;

        }

        public async Task<int> Register(User request, byte[] passwordHash, byte[] passwordSalt)
        {
            var user = new User
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.Email,
                Name = request.Name,
                CountryCode = request.CountryCode,
                PhoneNumber = request.PhoneNumber,
                RoleId=2
            };

            var Customer = new Customer
            {
                User = user,
                Customerwallet= 0.00m,
                CustomerRating=0,
            };

            _context.Customer.Add(Customer);
            _context.SaveChanges();
            return Customer.Id;
        }
        public async Task<Customer> GetById(int id)
        {
            return await _context.Customer.Include(x=>x.User).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await _context.Customer.Include(d => d.User).FirstOrDefaultAsync(d => d.User.Email == email);
        }

        public async Task<Customer> GetByToken(string token)
        {
            return await _context.Customer.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
        }

        public async Task UpdateRefreshToken(Customer Customer, RefreshToken newRefreshToken)
        {
            Customer.User.RefreshToken = newRefreshToken.Token;
            Customer.User.TokenCreated = newRefreshToken.Created;
            Customer.User.TokenExpires = newRefreshToken.Expires;

        }

        public async Task Login(Customer Customer)
        {
           
        }

        public async Task Logout(Customer Customer)
        {
            Customer.User.RefreshToken = null;
            Customer.User.TokenCreated = DateTime.MinValue;
            Customer.User.TokenExpires = DateTime.UtcNow;
        }
       
        public async Task Update(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
