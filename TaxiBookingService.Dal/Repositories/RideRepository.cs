using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class RideRepository :Repository<Ride>,IRideRepository
    {
       private readonly TaxiBookingServiceDbContext _context;

       public RideRepository(TaxiBookingServiceDbContext context) :base(context) 
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

        public async Task<int> GetDriverByRideId(int rideId)
        {
            var ride= await _context.Ride.FindAsync(rideId);
            return ride.DriverId.Value;
        }
        public async Task<int> GetCustomerByRideId(int rideId)
        {
            var ride = await _context.Ride.FindAsync(rideId);
            return ride.CustomerId;
        }

        public async Task<List<Ride>> GetAllDriverRides(int driverId)
        {
            return await _context.Ride.Where(x=>x.DriverId==driverId && x.RideStatusId==4).Include(x=>x.TaxiType).Include(x=>x.Customer.User).Include(x=>x.PickupLocation).Include(x=>x.DropoffLocation).ToListAsync();
        }

        public async Task<List<Ride>> GetAllPendingRides()
        {
            return await _context.Ride.Where(x => x.RideStatusId == 1).ToListAsync();
        }

        public async Task<List<Ride>> GetAllCustomerRides(int customerId)
        {
            return await _context.Ride.Where(x => x.CustomerId == customerId && x.RideStatusId == 4).Include(x => x.TaxiType).Include(x => x.Driver.User).Include(x => x.PickupLocation).Include(x => x.DropoffLocation).ToListAsync();
        }


        public async Task<int> BookRide(Location pickUp, Location dropOff, CustomerBookRideDto request, int customerId)//dto for paramaetrs
        {
            var taxiType = await _context.TaxiType.FirstOrDefaultAsync(r => r.Name.ToLower() == request.TaxiType.ToLower());
            var ride = new Ride
            {
                CustomerId = customerId,
                PickupLocation = new Location { Longitude = pickUp.Longitude, Latitude = pickUp.Latitude },
                DropoffLocation = new Location { Longitude = dropOff.Longitude, Latitude = dropOff.Latitude },
                TaxiTypeId = taxiType.Id,
                StartTime = DateTime.UtcNow,
                RideStatusId = 1
            };
            _context.Ride.Add(ride);
            await _context.SaveChangesAsync();//checking no of save chnages tracking records
            return ride.Id;
        }

        public async Task<Ride> GetRide(int driverId)
        {
           return await _context.Ride.Include(x=>x.PickupLocation).Include(x => x.DropoffLocation).Include(x => x.TaxiType).FirstOrDefaultAsync(x => x.DriverId == driverId && x.RideStatusId == 1 && x.Driver.DriverStatusId==2);
        }
    }
}
