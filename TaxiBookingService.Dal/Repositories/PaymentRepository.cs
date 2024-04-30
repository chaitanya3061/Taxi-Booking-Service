using Microsoft.EntityFrameworkCore;
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
            return await _context.Payment.Include(x=>x.PaymentMethod).FirstOrDefaultAsync(p => p.RideId == rideId);
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
