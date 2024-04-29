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
        IDriverRepository DriverRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        IAdminRepository AdminRepository { get; }
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
        IPaymentMethodRepository PaymentMethodRepository { get; }
        ITaxiTypeRepository TaxiTypeRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
