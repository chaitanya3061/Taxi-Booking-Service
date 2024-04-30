
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRejectedRideRepository : IRepository<RejectedRide>
    {
        Task<bool> HasDriverRejectedRide(int driverId, int rideId);
    }
}
