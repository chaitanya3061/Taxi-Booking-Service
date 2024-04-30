using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IDriverRepository :IRepository<Driver>
    {
        Task<Driver> GetByToken(string token);
        Task<Driver> GetById(int id);
        Task<List<Driver>> GetAllDrivers();
        Task<Location> GetLongLat(int driverId);
        Task<List<Driver>> GetAllTaxiTypeDrivers(int rideId);
        Task<Driver> GetByEmail(string email);
    }
}
