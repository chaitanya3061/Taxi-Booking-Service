using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IRideLogic
    {
        Task<string> GetDriverAsync(int rideid);
        Task<decimal> CalculateFare(Location pickUpLocation, Location dropOffLocation);
        Task<decimal> CalculateCancellationFee(int rideId);
        Task<decimal> CalculateFareWithStop(Location pickupLocation, Location stopLocation, Location dropoffLocation);

    }
}
