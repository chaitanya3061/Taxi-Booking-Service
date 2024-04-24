using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ITaxiRepository :IRepository<Taxi>
    {
        Task<Taxi> GetTaxiByDriverId(int DriverId);
        Task<int> AddTaxi(DriverTaxiDto request, int driverId);
    }
}
