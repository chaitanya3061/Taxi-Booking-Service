
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRideCancellationReasonRepository :IRepository<RideCancellationReason>
    {
        Task<List<RideCancellationReason>> GetAllValidReasons();
        Task<RideCancellationReason> GetByName(string name);
    }
}
