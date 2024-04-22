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

        private IDriverRepository<Driver> _DriverRepository;
        private ICustomerRepository<Customer> _CustomerRepository;
        private IRideRepository<Ride> _RideRepository;
        private ILocationRepository<Location> _LocationRepository;
        //private IAdminRepository<Admin> _AdminRepository;
        private ITariffChargeRepository<TariffCharge> _TariffChargeRepository;
        private IRejectedRideRepository<RejectedRide> _RejectedRideRepository;
        private ITaxiRepository<Taxi> _TaxiRepository;
        private IRideCancellationReasonRepository<RideCancellationReason> _RideCancellationReasonRepository;
        private readonly TaxiBookingServiceDbContext _context;
        public UnitOfWork(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }


        public IDriverRepository<Driver> DriverRepository => _DriverRepository ??= new DriverRepository(_context);

        public ICustomerRepository<Customer> CustomerRepository => _CustomerRepository ??= new CustomerRepository(_context);

        public IRideRepository<Ride> RideRepository => _RideRepository??=new RideRepository(_context);

        public ILocationRepository<Location> LocationRepository => _LocationRepository ??= new LocationRepository(_context);

        public ITariffChargeRepository<TariffCharge> TariffChargeRepository => _TariffChargeRepository??=new TariffChargeRepository(_context);

        public IRejectedRideRepository<RejectedRide> RejectedRideRepository => _RejectedRideRepository??=new RejectedRideRepository(_context);

        public IRideCancellationReasonRepository<RideCancellationReason> RideCancellationReasonRepository => _RideCancellationReasonRepository??=new RideCancellationReasonRepository(_context);

        public ITaxiRepository<Taxi> TaxiRepository => _TaxiRepository??=new TaxiRepository(_context);

        //public IAdminRepository<Admin> AdminRepository => _AdminRepository ??= new AdminRepository(_context);

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }

}
