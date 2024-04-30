using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class RideCancellationReasonRepository : Repository<RideCancellationReason>,IRideCancellationReasonRepository
    {
        private readonly TaxiBookingServiceDbContext _context;
        public RideCancellationReasonRepository(TaxiBookingServiceDbContext context) : base(context) 
        {
                    _context = context;
        }

        public async Task<List<RideCancellationReason>> GetAllValidReasons()
        {
            return await _context.RideCancellationReason.Where(r => r.IsValid ).ToListAsync();
        }

        public async Task<RideCancellationReason> GetByName(string name)
        {
            return await _context.RideCancellationReason
                .FirstOrDefaultAsync(r => r.Name == name && !r.IsDeleted);
        }
    }
}
