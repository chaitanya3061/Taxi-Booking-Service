using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Admin;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{

    public interface IAdminRepository
    {
        Task<Admin> GetByToken(string token);
        Task<Admin> GetById(int id);
    }
}
