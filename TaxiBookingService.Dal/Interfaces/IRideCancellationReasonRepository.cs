using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRideCancellationReasonRepository :IRepository<RideCancellationReason>
    {
        Task<List<RideCancellationReason>> GetAllValidReasons();
        Task<RideCancellationReason> GetByName(string name);
    }
}
