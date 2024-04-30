using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ICustomerRatingRepository : IRepository<CustomerRating>
    {
        Task<List<CustomerRating>> GetRatingsByDriverId(int Id);
    }
}
