using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Interfaces
{
     public interface IRejectedRideRepository<T>
    {
        Task<int> Add(int driverId,int rideId);
        Task<bool> HasDriverRejectedRide(int driverId, int rideId);

    }
}
