using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRideRepository<T> 
    {
       Task<T> GetById(int id);
       Task UpdateRideStatus(int rideId,int originalId,int previousId);
       Task<int> GetStatus(int rideId);
       Task<(decimal latitude, decimal longitude)> GetRideLongLat(int Id);




    }
}
