using Microsoft.EntityFrameworkCore;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class RejectedRideRepository : Repository<RejectedRide>,IRejectedRideRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public RejectedRideRepository(TaxiBookingServiceDbContext context): base(context) 
        {
            _context = context;
        }
        public async Task<bool> HasDriverRejectedRide(int driverId, int rideId)
        {
            return await _context.RejectedRide
                .AnyAsync(rr => rr.DriverId == driverId && rr.RideId == rideId);
        }
    }
}
