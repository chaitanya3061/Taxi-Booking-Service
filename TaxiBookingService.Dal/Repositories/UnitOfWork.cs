//using AutoMapper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaxiBookingService.Dal.Entities;
//using TaxiBookingService.Dal.Interfaces;
//using TaxiBookingService.Dal.RepositoriesFactory;

//namespace TaxiBookingService.Dal.Repositories
//{
//    public class UnitOfWork : IUnitOfWork
//    {

//        private IDriverRepository _DriverRepository;
//        private ICustomerRepository _CustomerRepository;
//        private IRideRepository _RideRepository;
//        private ILocationRepository _LocationRepository;
//        private IAdminRepository _AdminRepository;
//        private ITariffChargeRepository _TariffChargeRepository;
//        private IRejectedRideRepository _RejectedRideRepository;
//        private ITaxiRepository _TaxiRepository;
//        private IRideCancellationReasonRepository _RideCancellationReasonRepository;
//        private ICustomerRatingRepository _CustomerRatingRepository;
//        private IDriverRatingRepository _DriverRatingRepository;
//        private IPaymentRepository _PaymentRepository;
//        private ITrustedContactRepository _TrustedContactRepository;
//        private IUserRepository _UserRepository;
//        private IPaymentMethodRepository _PaymentMethodRepository;
//        private ITaxiTypeRepository _TaxiTypeRepository;
//        private IScheduleRideRepository _ScheduleRideRepository;


//        private readonly TaxiBookingServiceDbContext _context;
//        public UnitOfWork(TaxiBookingServiceDbContext context)
//        {
//            _context = context;
//        }

//        public IUserRepository UserRepository => _UserRepository = new UserRepository(_context);
//        public IDriverRepository DriverRepository => _DriverRepository = new DriverRepository(_context);
//        public ICustomerRepository CustomerRepository => _CustomerRepository= new CustomerRepository(_context);
//        public IRideRepository RideRepository => _RideRepository=new RideRepository(_context);
//        public ILocationRepository LocationRepository => _LocationRepository = new LocationRepository(_context);
//        public ITariffChargeRepository TariffChargeRepository => _TariffChargeRepository=new TariffChargeRepository(_context);
//        public IRejectedRideRepository RejectedRideRepository => _RejectedRideRepository=new RejectedRideRepository(_context);
//        public IRideCancellationReasonRepository RideCancellationReasonRepository => _RideCancellationReasonRepository=new RideCancellationReasonRepository(_context);
//        public ITaxiRepository TaxiRepository => _TaxiRepository=new TaxiRepository(_context);
//        public ICustomerRatingRepository CustomerRatingRepository => _CustomerRatingRepository = new CustomerRatingRepository(_context);
//        public IDriverRatingRepository DriverRatingRepository => _DriverRatingRepository = new DriverRatingRepository(_context);
//        public IPaymentRepository PaymentRepository => _PaymentRepository = new PaymentRepository(_context);
//        public ITrustedContactRepository TrustedContactRepository => _TrustedContactRepository = new TrustedContactRepository(_context);
//        public IAdminRepository AdminRepository => _AdminRepository = new AdminRepository(_context);
//        public IPaymentMethodRepository PaymentMethodRepository => _PaymentMethodRepository = new PaymentMethodRepository(_context);
//        public ITaxiTypeRepository TaxiTypeRepository => _TaxiTypeRepository = new TaxiTypeRepository(_context);
//        public IScheduleRideRepository ScheduleRideRepository  => _ScheduleRideRepository = new ScheduleRideRepository(_context);

//        public Task<int> SaveChangesAsync()
//        {
//            return _context.SaveChangesAsync();
//        }
//    }

//}
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Dal.RepositoriesFactory;

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
        private IScheduleRideRepository _ScheduleRideRepository;

        private readonly TaxiBookingServiceDbContext _context;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(TaxiBookingServiceDbContext context, IRepositoryFactory repositoryFactory)
        {
            _context = context;
            _repositoryFactory = repositoryFactory;
        }

        public IUserRepository UserRepository => _UserRepository = _repositoryFactory.CreateRepository<IUserRepository>();

        //public IUserRepository UserRepository => _UserRepository = new UserRepository(_context);
        public IDriverRepository DriverRepository => _DriverRepository =_repositoryFactory.CreateRepository<IDriverRepository>();
        public ICustomerRepository CustomerRepository => _CustomerRepository = _repositoryFactory.CreateRepository<ICustomerRepository>();
        public IRideRepository RideRepository => _RideRepository = _repositoryFactory.CreateRepository<IRideRepository>();
        public ILocationRepository LocationRepository => _LocationRepository = _repositoryFactory.CreateRepository<ILocationRepository>();
        public ITariffChargeRepository TariffChargeRepository => _TariffChargeRepository = _repositoryFactory.CreateRepository<ITariffChargeRepository>();
        public IRejectedRideRepository RejectedRideRepository => _repositoryFactory.CreateRepository<IRejectedRideRepository>();
        public IRideCancellationReasonRepository RideCancellationReasonRepository => _RideCancellationReasonRepository = _repositoryFactory.CreateRepository<IRideCancellationReasonRepository>();
        public ITaxiRepository TaxiRepository => _TaxiRepository = _repositoryFactory.CreateRepository<ITaxiRepository>();
        public ICustomerRatingRepository CustomerRatingRepository => _repositoryFactory.CreateRepository<ICustomerRatingRepository>();
        public IDriverRatingRepository DriverRatingRepository => _DriverRatingRepository = _repositoryFactory.CreateRepository<IDriverRatingRepository>();
        public IPaymentRepository PaymentRepository => _PaymentRepository = _repositoryFactory.CreateRepository<IPaymentRepository>();
        public ITrustedContactRepository TrustedContactRepository => _TrustedContactRepository = _repositoryFactory.CreateRepository<ITrustedContactRepository>();
        public IAdminRepository AdminRepository => _AdminRepository = _repositoryFactory.CreateRepository<IAdminRepository>();
        public IPaymentMethodRepository PaymentMethodRepository => _PaymentMethodRepository = _repositoryFactory.CreateRepository<IPaymentMethodRepository>();
        public ITaxiTypeRepository TaxiTypeRepository => _TaxiTypeRepository = _repositoryFactory.CreateRepository<ITaxiTypeRepository>();
        public IScheduleRideRepository ScheduleRideRepository => _ScheduleRideRepository = _repositoryFactory.CreateRepository<IScheduleRideRepository>();

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }

}
