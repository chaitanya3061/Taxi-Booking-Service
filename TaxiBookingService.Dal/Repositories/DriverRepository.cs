using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class DriverRepository : IDriverRepository<Driver>
    {
        private readonly TaxiBookingServiceDbContext _context;

        public DriverRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }

        public async Task<int> Register(DriverRegisterDto request, byte[] passwordHash, byte[] passwordSalt)
        {
            var user = new User
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = request.Email,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                CountryCode = request.CountryCode,
                RoleId = 3,
            };

            var driver = new Driver
            {
                User = user,
                LicenceNumber = request.LicenseNumber,
                DriverRating = 0,
                Driverearnings = 0.0m,
                DriverStatusId = 1
            };

            _context.Driver.Add(driver);
            _context.SaveChanges();
            return driver.Id;
        }

        public async Task<Driver> GetById(int id)
        {
            return _context.Driver.Include(d => d.User).FirstOrDefault(x => x.Id == id);
        }

        public async Task<(decimal latitude, decimal longitude)> GetLongLat(int Id)
        {
            var userLocation = _context.UserLocation.FirstOrDefault(x => x.UserId == Id);
            return (userLocation.Latitude, userLocation.Longitude);
        }
        public async Task<Driver> GetByEmail(string email)
        {
            return await _context.Driver.Include(d => d.User).FirstOrDefaultAsync(d => d.User.Email == email);
        }

        public async Task<Driver> GetByToken(string token)
        {
            return await _context.Driver.Include(d => d.User).FirstOrDefaultAsync(d => d.User.RefreshToken == token);
        }

        public async Task UpdateRefreshToken(Driver driver, RefreshToken newRefreshToken)
        {
            driver.User.RefreshToken = newRefreshToken.Token;
            driver.User.TokenCreated = newRefreshToken.Created;
            driver.User.TokenExpires = newRefreshToken.Expires;

        }

        public async Task Login(Driver driver)
        {
        }

        public async Task Logout(Driver driver)
        {
            driver.User.RefreshToken = null;
            driver.User.TokenCreated = DateTime.MinValue;
            driver.User.TokenExpires = DateTime.UtcNow;
        }

        public async Task Update(Driver driver)
        {
            _context.Entry(driver).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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

        public async Task UpdateStatus(int Id, int statusId)
        {
            var driver = await GetById(Id);
            driver.DriverStatusId = statusId;
            _context.Entry(driver).State = EntityState.Modified;
        }

        public async Task UpdateRideStatus(int driverId, int rideId)
        {
            var ride = await _context.Ride.FindAsync(rideId);
            ride.DriverId = rideId;
            if (ride.RideStatusId == 1)
                ride.RideStatusId = 2;
            _context.Entry(ride).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
