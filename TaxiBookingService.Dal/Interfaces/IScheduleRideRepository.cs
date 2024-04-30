
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IScheduleRideRepository :IRepository<ScheduledRide>
    {
        Task<int> CreateScheduleRide(int rideId,DateTime ScheduledDate);
        Task<List<ScheduledRide>> GetAllActiveSchedulerides();
    }
}
