using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class TaxiTypeRepository : Repository<TaxiType>, ITaxiTypeRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public TaxiTypeRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TaxiType> GetByName(string name)
        {
           return await _context.TaxiType.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower());
        }
    }
}
