using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ITaxiRepository<T>
    {
        Task<int> AddTaxi(DriverTaxiServiceContracts asset, int Id);
        Task UpdateTaxi(int taxiId, DriverTaxiServiceContracts taxi);
        Task<T> GetTaxiById(int TaxiId);
        Task<T> GetTaxiByDriverId(int DriverId);
    }
}
