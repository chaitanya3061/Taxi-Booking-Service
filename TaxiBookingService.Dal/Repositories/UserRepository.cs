using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TaxiBookingServiceDbContext _context;
        public UserRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
