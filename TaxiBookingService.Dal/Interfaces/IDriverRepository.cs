using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IDriverRepository<T>
    {
        Task UpdateRefreshToken(T user, RefreshToken refreshToken);
        Task<int> Register(DriverRegisterServiceContracts request, byte[] passwordHash, byte[] passwordSalt);
        Task<T> GetByEmail(string email);
        Task<T> GetByToken(string token);
        Task<T> GetById(int id);
        Task Login(T entity);
        Task Logout(T entity);
        Task<List<Driver>> GetAllDrivers();
        Task<(decimal latitude, decimal longitude)> GetLongLat(int Id);
        Task<List<Driver>> GetAllTaxiTypeDrivers(int rideId);
        Task UpdateStatus(int Id,int statusId);
        Task UpdateRideStatus(int driverId, int rideId);
        Task Update(Driver driver);
    }
}
