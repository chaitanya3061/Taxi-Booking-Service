using Microsoft.EntityFrameworkCore;
using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Common.AssetManagement.Common;
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
            return await _context.Ride.Include(x=>x.PickupLocation).Include(x=>x.DropoffLocation).Include(x => x.Payment).FirstOrDefaultAsync(x=>x.Id==id);
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
        public async Task<Location> GetRideLongLat(int Id)
        {
            var ride = await _context.Ride.FindAsync(Id);
            var result = await _context.Location.FindAsync(ride.PickupLocationId);
            return new Location() {Latitude= result.Latitude,Longitude= result.Longitude };
        }

        public async Task<int> GetDriverByRideId(int rideId)
        {
            var driverId= await _context.Ride
               .Where(r => r.Id == rideId)
               .Select(r => r.DriverId)
               .FirstOrDefaultAsync();
            return driverId.Value;
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
            return await _context.Ride.Where(x => x.RideStatusId == (int)Common.Enums.RideStatus.Searching).ToListAsync();
        }

        public async Task<List<Ride>> GetAllCustomerRides(int customerId)
        {
            return await _context.Ride
        .Where(x => x.CustomerId == customerId &&
                   (x.RideStatusId == (int)Common.Enums.RideStatus.Completed || x.RideStatusId == (int)Common.Enums.RideStatus.Cancelled))
        .Include(x => x.TaxiType)
        .Include(x => x.Driver.User)
        .Include(x => x.PickupLocation)
        .Include(x => x.DropoffLocation)
        .ToListAsync();
        }


        public async Task<int> BookRide(Location pickUp, Location dropOff, CustomerBookRideDto request, int customerId)
        {
            var taxiType = await _context.TaxiType.FirstOrDefaultAsync(r => r.Name.ToLower() == request.TaxiType.ToLower());
            var ride = new Ride
            {
                CustomerId = customerId,
                PickupLocation = new Location { Longitude = pickUp.Longitude, Latitude = pickUp.Latitude },
                DropoffLocation = new Location { Longitude = dropOff.Longitude, Latitude = dropOff.Latitude },
                TaxiTypeId = taxiType.Id,
                StartTime = DateTime.UtcNow,
                RideStatusId = (int)Common.Enums.RideStatus.Searching,
            };
            _context.Ride.Add(ride);
            await _context.SaveChangesAsync();
            return ride.Id;
        }

        public async Task<Ride> GetRide(int driverId)
        {
           return await _context.Ride.Include(x=>x.PickupLocation).Include(x => x.DropoffLocation).Include(x => x.TaxiType).FirstOrDefaultAsync(x => x.DriverId == driverId && x.RideStatusId == 1 && x.Driver.DriverStatusId==2);
        }

        public Task<List<Ride>> GetCustomerRides(int Customerid, RideQueryParametersDto parameters)
        {
            var query = _context.Ride.Where(r => r.CustomerId == Customerid);

            if (!string.IsNullOrEmpty(parameters.SortField))
            {
                if (parameters.SortOrder == "desc")
                {
                    query = query.OrderByDescending(r => EF.Property<object>(r, parameters.SortField));
                }
                else
                {
                    query = query.OrderBy(r => EF.Property<object>(r, parameters.SortField));
                }
            }

            if (!string.IsNullOrEmpty(parameters.SearchQuery))
            {
                query = query.Where(r => r.Driver.User.Name.Contains(parameters.SearchQuery));                            
            }

            query = query.Skip((parameters.Limit - 1) * parameters.OffSet)
                         .Take(parameters.OffSet);

            //filter 
            if (parameters.FilterByStatus.HasValue)
            {
                query = query.Where(r => r.RideStatusId == parameters.FilterByStatus);
            }

            var result = query.ToListAsync();
            return result;
        }

        public async Task<bool> IsDriverInRide(int driverId)
        {
             return await _context.Ride.AnyAsync(r => r.DriverId == driverId && (r.RideStatusId == (int)Common.Enums.RideStatus.Accepted || r.RideStatusId == (int)Common.Enums.RideStatus.Started));
        }

        public async Task<bool> HasActiveRideRequest(int customerId)
        {
            return await _context.Ride.AnyAsync(r => r.CustomerId == customerId && (r.RideStatusId == (int)Common.Enums.RideStatus.Searching));
        }
    }
}
