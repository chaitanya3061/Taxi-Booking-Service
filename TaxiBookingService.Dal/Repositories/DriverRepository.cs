using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class DriverRepository : Repository<Driver>,IDriverRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public DriverRepository(TaxiBookingServiceDbContext context): base(context) { 
        
            _context = context;
        }

        public async Task<Driver> GetById(int id)
        {
            return _context.Driver.Include(d => d.User).FirstOrDefault(x => x.Id == id);
        }

        public async Task<Location> GetLongLat(int Id)
        {
            var userLocation = _context.UserLocation.FirstOrDefault(x => x.UserId == Id);
            return new Location() {Latitude= userLocation.Latitude,Longitude= userLocation.Longitude };
        }


        public async Task<Driver> GetByToken(string token)
        {
            return await _context.Driver.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
        }
     
        public async Task<List<Driver>> GetAllDrivers()
        {
            return await _context.Driver.
                Where(x => x.DriverStatusId == 1).ToListAsync();
        }

        public async Task<List<Driver>> GetAllTaxiTypeDrivers(int rideId)
        {
            var ride = await _context.Ride.FindAsync(rideId);
            var taxis = await _context.Taxi
                   .Include(t => t.Driver)
                   .Where(t => t.TaxiTypeId == ride.TaxiTypeId)
                   .ToListAsync();

            var drivers = taxis.Select(t => t.Driver).ToList();
            drivers = drivers.Where(x => x.DriverStatusId == 1).ToList();
            return drivers;
        }

        public async Task<Driver> GetByEmail(string email)
        {
            return await _context.Driver.Include(d => d.User).FirstOrDefaultAsync(d => d.User.Email == email);
        }
    }
}
