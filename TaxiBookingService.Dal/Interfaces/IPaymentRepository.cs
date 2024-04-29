using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IPaymentRepository:IRepository<Payment>
    {
        Task<int> CreatePayment(int rideId, string type, decimal fare);
        Task<PaymentMethod> GetPaymentMethod(string paymentMethodName);
        Task<Payment> GetByRide(int rideId);
    }
}
