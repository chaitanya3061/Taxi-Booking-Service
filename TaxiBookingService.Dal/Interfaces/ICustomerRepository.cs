using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{

    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByToken(string token);
        Task<Customer> GetById(int id);
        Task<Customer> GetByEmail(string email);
    }
}
