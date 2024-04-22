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
    public class RideRepository :IRideRepository<Ride>
    {
       private readonly TaxiBookingServiceDbContext _context;

       public RideRepository(TaxiBookingServiceDbContext context)
       {
          _context = context;
       }

        public async Task<Ride> GetById(int id)
        {
            return await _context.Ride.Include(x=>x.PickupLocation).Include(x=>x.DropoffLocation).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<int> GetStatus(int rideId)
        {
            var ride= await _context.Ride.FindAsync(rideId);
            return ride.RideStatusId;
        }

        public async Task UpdateRideStatus(int rideId,int originalId,int previousId)
        {
            var ride=await GetById(rideId);
            if (ride.RideStatusId == previousId)
            {
                ride.RideStatusId = originalId;
            }
            _context.Entry(ride).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<(decimal latitude, decimal longitude)> GetRideLongLat(int Id)
        {
            var ride = await _context.Ride.FindAsync(Id);
            var result = await _context.Location.FindAsync(ride.PickupLocationId);
            return (result.Latitude, result.Longitude);
        }
    }
}
