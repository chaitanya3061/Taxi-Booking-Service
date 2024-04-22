using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class RejectedRideRepository : IRejectedRideRepository<RejectedRide>
    {
        private readonly TaxiBookingServiceDbContext _context;

        public RejectedRideRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }
        public async Task<int> Add(int driverId, int rideId)
        {
            var rejectedride = new RejectedRide
            {
                DriverId= driverId,
                RideId= rideId
            };
           await _context.RejectedRide.AddAsync(rejectedride);
           await _context.SaveChangesAsync();
            return rejectedride.Id;
        }
        public async Task<bool> HasDriverRejectedRide(int driverId, int rideId)
        {
            return await _context.RejectedRide
                .AnyAsync(rr => rr.DriverId == driverId && rr.RideId == rideId);
        }
    }
}
