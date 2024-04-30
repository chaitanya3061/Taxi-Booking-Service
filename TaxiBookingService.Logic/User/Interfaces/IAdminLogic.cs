using TaxiBookingService.API.Ride;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IAdminLogic
    {
        Task<int> AddCancellationReason(AdminManageReasonDto request);
        Task<bool> DeleteCancellationReason(int Id);
        Task<int> UpdateCancellationReason(AdminManageReasonDto request,int Id);
        Task<bool> DeleteUser(int Id);
        Task<int> UpdateUser(AdminManageUserDto request, int Id);
        Task<List<Ride>> GetCustomerRides(int Id, RideQueryParametersDto request);
    }
}
