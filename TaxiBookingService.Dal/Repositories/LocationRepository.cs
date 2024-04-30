using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public LocationRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
