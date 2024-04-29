using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDriverRepository _DriverRepository;
        private ICustomerRepository _CustomerRepository;
        private IRideRepository _RideRepository;
        private ILocationRepository _LocationRepository;
        private IAdminRepository _AdminRepository;
        private ITariffChargeRepository _TariffChargeRepository;
        private IRejectedRideRepository _RejectedRideRepository;
        private ITaxiRepository _TaxiRepository;
        private IRideCancellationReasonRepository _RideCancellationReasonRepository;
        private ICustomerRatingRepository _CustomerRatingRepository;
        private IDriverRatingRepository _DriverRatingRepository;
        private IPaymentRepository _PaymentRepository;
        private ITrustedContactRepository _TrustedContactRepository;
        private IUserRepository _UserRepository;
        private IPaymentMethodRepository _PaymentMethodRepository;
        private ITaxiTypeRepository _TaxiTypeRepository;

        private readonly TaxiBookingServiceDbContext _context;
        public UnitOfWork(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository => _UserRepository = new UserRepository(_context);

        public IDriverRepository DriverRepository => _DriverRepository = new DriverRepository(_context);

        public ICustomerRepository CustomerRepository => _CustomerRepository= new CustomerRepository(_context);

        public IRideRepository RideRepository => _RideRepository=new RideRepository(_context);

        public ILocationRepository LocationRepository => _LocationRepository = new LocationRepository(_context);

        public ITariffChargeRepository TariffChargeRepository => _TariffChargeRepository=new TariffChargeRepository(_context);

        public IRejectedRideRepository RejectedRideRepository => _RejectedRideRepository=new RejectedRideRepository(_context);

        public IRideCancellationReasonRepository RideCancellationReasonRepository => _RideCancellationReasonRepository=new RideCancellationReasonRepository(_context);

        public ITaxiRepository TaxiRepository => _TaxiRepository=new TaxiRepository(_context);
        public ICustomerRatingRepository CustomerRatingRepository => _CustomerRatingRepository = new CustomerRatingRepository(_context);
        public IDriverRatingRepository DriverRatingRepository => _DriverRatingRepository = new DriverRatingRepository(_context);
        public IPaymentRepository PaymentRepository => _PaymentRepository = new PaymentRepository(_context);

        public ITrustedContactRepository TrustedContactRepository => _TrustedContactRepository = new TrustedContactRepository(_context);

        public IAdminRepository AdminRepository => _AdminRepository ??= new AdminRepository(_context);

        public IPaymentMethodRepository PaymentMethodRepository => _PaymentMethodRepository ??= new PaymentMethodRepository(_context);
        public ITaxiTypeRepository TaxiTypeRepository => _TaxiTypeRepository ??= new TaxiTypeRepository(_context);

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }

}
