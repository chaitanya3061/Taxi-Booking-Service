using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Admin;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IAdminLogic
    {
        Task<int> AddCancellationReason(AdminManageReasonDto request);
        Task<bool> DeleteCancellationReason(int Id);
        Task<int> UpdateCancellationReason(AdminManageReasonDto request,int Id);
        Task<bool> DeleteUser(int Id);
        Task<int> UpdateUser(AdminManageUserDto request, int Id);
    }
}
