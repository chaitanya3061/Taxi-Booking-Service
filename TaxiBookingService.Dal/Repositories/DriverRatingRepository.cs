using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class DriverRatingRepository
  : Repository<DriverRating>, IDriverRatingRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public DriverRatingRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DriverRating>> GetRatingsByCustomerId(int Id)
        {
            return await _context.DriverRating
              .Where(r => r.Ride.CustomerId == Id)
              .ToListAsync();
        }
    }
}
