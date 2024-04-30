using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRideRepository:IRepository<Ride>
    {
       Task UpdateRideStatus(int rideId,int originalId,int previousId);
       Task<int> GetStatus(int rideId);
       Task<Location> GetRideLongLat(int Id);
       Task<int> GetDriverByRideId(int rideId);
       Task<int> GetCustomerByRideId(int rideId);
       Task<List<Ride>> GetAllDriverRides(int Id);
       Task<List<Ride>> GetAllCustomerRides(int Id);
        Task<List<Ride>> GetAllPendingRides();
       Task<int> BookRide(Location pickUp, Location dropOff, CustomerBookRideDto request, int customerId);
       Task<Ride> GetRide(int driverId);
       Task<List<Ride>> GetCustomerRides(int Id, RideQueryParametersDto request);
       Task<bool>IsDriverInRide(int driverId);
       Task<bool> HasActiveRideRequest(int customerId);
    }
}
