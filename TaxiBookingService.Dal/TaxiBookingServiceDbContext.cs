using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

using System.Text;

//using TaxiBookingService.Common.Enums;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal
{
    public class TaxiBookingServiceDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public TaxiBookingServiceDbContext(DbContextOptions<TaxiBookingServiceDbContext> options, IConfiguration configuration)
          : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<UserLocation> UserLocation { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<TariffCharge> TariffCharge { get; set; }
        public DbSet<DriverStatus> DriverStatus { get; set; }
        public DbSet<TrustedContacts> TrustedContacts { get; set; }
        public DbSet<Taxi> Taxi { get; set; }
        DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<RejectedRide> RejectedRide { get; set; }
        public DbSet<Ride> Ride { get; set; }
        public DbSet<RideStatus> RideStatus { get; set; }
        public DbSet<ScheduledRide> ScheduledRide { get; set; }
        public DbSet<CustomerRating> CustomerRating { get; set; }
        public DbSet<DriverRating> DriverRating { get; set; }
        public DbSet<TaxiType> TaxiType { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<RideCancellationReason> RideCancellationReason { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Customer>()
            .HasOne(r => r.User)
            .WithMany(d => d.Customers)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Driver>()
            .HasOne(r => r.User)
            .WithMany(d => d.Drivers)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
            .HasOne(r => r.User)
            .WithMany(d => d.Admins)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
            .HasOne(r => r.User)
            .WithMany(d => d.Customers)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RejectedRide>()
           .HasOne(rr => rr.Ride)
           .WithMany(r => r.RejectedRides)
           .HasForeignKey(rr => rr.RideId)
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RejectedRide>()
           .HasOne(rr => rr.Driver)
           .WithMany(r => r.RejectedRides)
           .HasForeignKey(rr => rr.DriverId)
           .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Ride>()
            .HasOne(r => r.Driver)
            .WithMany(d => d.Rides)
            .HasForeignKey(r => r.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ride>()
            .HasOne(r => r.Customer)
            .WithMany(d => d.Rides)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TrustedContacts>()
            .HasOne(tc => tc.Customer)
            .WithMany(c => c.TrustedContacts)
            .HasForeignKey(tc => tc.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ride>()
            .HasOne(r => r.PickupLocation)
            .WithMany()
            .HasForeignKey(r => r.PickupLocationId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ride>()
            .HasOne(r => r.DropoffLocation)
            .WithMany()
            .HasForeignKey(r => r.DropoffLocationId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ride>()
            .HasOne(r => r.Stop1Location)
            .WithMany()
            .HasForeignKey(r => r.StopId1)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ride>()
             .HasOne(r => r.Stop2Location)
             .WithMany()
             .HasForeignKey(r => r.StopId2)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany()
            .HasForeignKey(u => u.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Driver>()
           .HasOne(u => u.DriverStatus)
           .WithMany()
           .HasForeignKey(u => u.DriverStatusId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
             .HasOne(u => u.PaymentMethod)
             .WithMany()
             .HasForeignKey(u => u.PaymentMethodId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
            .HasOne(u => u.PaymentStatus)
            .WithMany()
            .HasForeignKey(u => u.PaymentStatusId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Role>().HasData(
              new Role { Id = 1, Name = "Admin" },
              new Role { Id = 2, Name = "Customer" },
              new Role { Id = 3, Name = "Driver" }
          );
            modelBuilder.Entity<DriverStatus>().HasData(
             new DriverStatus { Id = 1, Name = "Availiable" },
             new DriverStatus { Id = 2, Name = "UnAvailiable" }
             );
            modelBuilder.Entity<TaxiType>().HasData(
             new TaxiType { Id = 1, Name = "2wheeler" },
             new TaxiType { Id = 2, Name = "3wheeler" },
             new TaxiType { Id = 3, Name = "4wheeler" }
             );
            modelBuilder.Entity<RideStatus>().HasData(
            new RideStatus { Id = 1, Status = "Searching" },
            new RideStatus { Id = 2, Status = "Accepted" },
            new RideStatus { Id = 3, Status = "Started" },
            new RideStatus { Id = 4, Status = "Completed" },
            new RideStatus { Id = 5, Status = "Cancelled" }
            );
            modelBuilder.Entity<PaymentStatus>().HasData(
            new PaymentStatus { Id = 1, Status = "Pending" },
            new PaymentStatus { Id = 2, Status = "Completed" }
            );
            modelBuilder.Entity<PaymentMethod>().HasData(
            new PaymentMethod { Id = 1, Name = "Wallet" },
            new PaymentMethod { Id = 2, Name = "Cash" }
            );
            modelBuilder.Entity<TariffCharge>().HasData(
            new TariffCharge { Id = 1, Name = "CancellationFee", Value = 5 },
            new TariffCharge { Id = 2, Name = "PerKm", Value = 3.20m },
            new TariffCharge { Id = 3, Name = "Basefare", Value = 32.0m },
            new TariffCharge { Id = 4, Name = "driverCommissionRate", Value = 10 }
            );
            modelBuilder.Entity<RideCancellationReason>().HasData(
            new RideCancellationReason { Id = 1, Name = "Customer changed mind", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 2, Name = "Driver unavailability", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 3, Name = "Emergency situation", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 4, Name = "Incorrect pickup location", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 5, Name = "Invalid payment method", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 6, Name = "Customer didn't show up", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 7, Name = "Driver took too long", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 8, Name = "Change in plans", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 9, Name = "Destination changed", IsValid = true, IsDeleted = false },
            new RideCancellationReason { Id = 10, Name = "Weather conditions", IsValid = false, IsDeleted = false },
            new RideCancellationReason { Id = 11, Name = "Traffic congestion", IsValid = false, IsDeleted = false },
            new RideCancellationReason { Id = 12, Name = "Change in plans", IsValid = false, IsDeleted = false }
        );
            var name = _configuration["AdminCredentials:Name"];
            var email = _configuration["AdminCredentials:Email"];
            var password = _configuration["AdminCredentials:Password"];
            var countryCode = _configuration["AdminCredentials:CountryCode"];
            var phoneNumber = _configuration["AdminCredentials:PhoneNumber"];
            using var hmac = new HMACSHA512();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 101,
                    Name = name,
                    Email = email,
                    PasswordSalt = hmac.Key,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                    CountryCode = countryCode,
                    PhoneNumber = phoneNumber,
                    RoleId = 1,
                    CreatedAt = DateTime.UtcNow,
                }
                );
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 3, UserId = 101 }
                );

        }
    }
}
