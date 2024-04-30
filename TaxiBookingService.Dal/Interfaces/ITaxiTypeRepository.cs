
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ITaxiTypeRepository : IRepository<TaxiType>
    {
        Task<TaxiType> GetByName(string name);
    }
}
