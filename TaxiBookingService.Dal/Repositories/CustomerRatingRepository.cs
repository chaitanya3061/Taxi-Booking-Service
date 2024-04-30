using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class CustomerRatingRepository
    : Repository<CustomerRating>, ICustomerRatingRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public CustomerRatingRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CustomerRating>> GetRatingsByDriverId(int Id)
        {
            return await _context.CustomerRating
               .Where(r => r.Ride.DriverId == Id)
               .ToListAsync();
        }
    }
}
