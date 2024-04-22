using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class LocationRepository :ILocationRepository<Location>
    {
        private readonly TaxiBookingServiceDbContext _context;

        public LocationRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Location> GetById(int id)
        {
            return await _context.Location.FindAsync(id);
        }
    }
}
