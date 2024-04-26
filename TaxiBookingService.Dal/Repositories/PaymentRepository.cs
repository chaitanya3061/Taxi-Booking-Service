using Azure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public PaymentRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PaymentMethod> GetPaymentMethod(string paymentMethodName)
        {
            return await _context.PaymentMethod.FirstOrDefaultAsync(p => p.Name.ToLower() == paymentMethodName.ToLower());
        }

        public async Task<Payment> GetByRide(int rideId)
        {
            return await _context.Payment.FirstOrDefaultAsync(p => p.RideId == rideId);
        }

        public async Task<int> CreatePayment(int rideId, string type , decimal fare)
        {
            var paymentMethod = await GetPaymentMethod(type);

            var payment = new Payment
            {
                RideId = rideId,
                PaymentMethodId = paymentMethod.Id,
                PaymentStatusId = 1,
                PaymentDate = DateTime.UtcNow,
                TotalFareAmount = fare
            };
            await _context.Payment.AddAsync(payment);
            return payment.Id;
        }
    

    }
}
