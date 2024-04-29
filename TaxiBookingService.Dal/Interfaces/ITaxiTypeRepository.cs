using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ITaxiTypeRepository : IRepository<TaxiType>
    {
        Task<TaxiType> GetByName(string name);
    }
}
