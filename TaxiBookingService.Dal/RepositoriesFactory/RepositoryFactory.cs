using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Dal.Repositories;

namespace TaxiBookingService.Dal.RepositoriesFactory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly TaxiBookingServiceDbContext _context;

        public RepositoryFactory(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }

        public TRepository CreateRepository<TRepository>() where TRepository : class
        {
            switch (typeof(TRepository))
            {
                case Type t when t == typeof(IUserRepository):
                    return new UserRepository(_context) as TRepository;

                case Type t when t == typeof(IDriverRepository):
                    return new DriverRepository(_context) as TRepository;

                case Type t when t == typeof(ICustomerRepository):
                    return new CustomerRepository(_context) as TRepository;

                case Type t when t == typeof(IRideRepository):
                    return new RideRepository(_context) as TRepository;

                case Type t when t == typeof(ILocationRepository):
                    return new LocationRepository(_context) as TRepository;

                case Type t when t == typeof(ITariffChargeRepository):
                    return new TariffChargeRepository(_context) as TRepository;

                case Type t when t == typeof(IRejectedRideRepository):
                    return new RejectedRideRepository(_context) as TRepository;

                case Type t when t == typeof(IRideCancellationReasonRepository):
                    return new RideCancellationReasonRepository(_context) as TRepository;

                case Type t when t == typeof(ITaxiRepository):
                    return new TaxiRepository(_context) as TRepository;

                case Type t when t == typeof(ICustomerRatingRepository):
                    return new CustomerRatingRepository(_context) as TRepository;

                case Type t when t == typeof(IDriverRatingRepository):
                    return new DriverRatingRepository(_context) as TRepository;

                case Type t when t == typeof(IPaymentRepository):
                    return new PaymentRepository(_context) as TRepository;

                case Type t when t == typeof(ITrustedContactRepository):
                    return new TrustedContactRepository(_context) as TRepository;

                case Type t when t == typeof(IAdminRepository):
                    return new AdminRepository(_context) as TRepository;

                case Type t when t == typeof(IPaymentMethodRepository):
                    return new PaymentMethodRepository(_context) as TRepository;

                case Type t when t == typeof(ITaxiTypeRepository):
                    return new TaxiTypeRepository(_context) as TRepository;

                case Type t when t == typeof(IScheduleRideRepository):
                    return new ScheduleRideRepository(_context) as TRepository;

                default:
                    throw new NotSupportedException($"Repository of type {typeof(TRepository).Name} is not supported.");
            }
        }

    }

}


