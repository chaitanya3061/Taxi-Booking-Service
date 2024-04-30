using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{

    public interface IAdminRepository
    {
        Task<Admin> GetByToken(string token);
        Task<Admin> GetById(int id);
    }
}
