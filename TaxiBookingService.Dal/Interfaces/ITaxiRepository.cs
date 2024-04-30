
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
