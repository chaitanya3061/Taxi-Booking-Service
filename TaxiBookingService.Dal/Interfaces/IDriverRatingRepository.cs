
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IDriverRatingRepository : IRepository<DriverRating>
    {
        Task<List<DriverRating>> GetRatingsByCustomerId(int Id);
    }
}
