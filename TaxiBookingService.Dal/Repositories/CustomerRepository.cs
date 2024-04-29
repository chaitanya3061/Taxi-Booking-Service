using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class CustomerRepository : Repository<Customer>,ICustomerRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public CustomerRepository(TaxiBookingServiceDbContext context) : base(context) { 
        
            _context = context;
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customer.Include(x=>x.User).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<Customer> GetByToken(string token)
        {
            return await _context.Customer.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
        }
    }
}
