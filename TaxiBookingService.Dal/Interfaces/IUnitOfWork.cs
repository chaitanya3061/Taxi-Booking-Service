using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IUnitOfWork
    {
        IDriverRepository<Driver> DriverRepository { get; }
        ICustomerRepository<Customer> CustomerRepository { get; }

        IRideRepository<Ride> RideRepository { get; }

        ILocationRepository<Location> LocationRepository { get; }
        ITariffChargeRepository<TariffCharge> TariffChargeRepository { get; }

        IRejectedRideRepository<RejectedRide> RejectedRideRepository { get; }

        IRideCancellationReasonRepository<RideCancellationReason> RideCancellationReasonRepository { get; }

       ITaxiRepository<Taxi> TaxiRepository { get; }


        //IAdminRepository<Admin> AdminRepository { get; }


        Task<int> SaveChangesAsync();
    }
}
