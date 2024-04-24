using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRideRepository:IRepository<Ride>
    {
       Task UpdateRideStatus(int rideId,int originalId,int previousId);
       Task<int> GetStatus(int rideId);
       Task<(decimal latitude, decimal longitude)> GetRideLongLat(int Id);
       Task<int> GetDriverByRideId(int rideId);
       Task<int> GetCustomerByRideId(int rideId);
       Task<List<Ride>> GetAllDriverRides(int Id);
       Task<List<Ride>> GetAllCustomerRides(int Id);
       Task<int> BookRide(Location pickUp, Location dropOff, CustomerBookRideDto request, int customerId);
    }
}
