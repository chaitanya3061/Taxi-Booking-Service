using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IPaymentRepository:IRepository<Payment>
    {
        Task<int> CreatePayment(int rideId, string type, decimal fare);
        Task<PaymentMethod> GetPaymentMethod(string paymentMethodName);
        Task<decimal> GetFareByRide(int rideId);
    }
}
