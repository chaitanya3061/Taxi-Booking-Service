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
        IAdminRepository<Admin> AdminRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IRideRepository RideRepository { get; }
        ILocationRepository LocationRepository { get; }
        ITariffChargeRepository TariffChargeRepository { get; }
        ITrustedContactRepository TrustedContactRepository { get; }
        IRejectedRideRepository RejectedRideRepository { get; }
        IRideCancellationReasonRepository RideCancellationReasonRepository { get; }
        ITaxiRepository TaxiRepository { get; }
        ICustomerRatingRepository CustomerRatingRepository { get; }
        IDriverRatingRepository DriverRatingRepository { get; }
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
