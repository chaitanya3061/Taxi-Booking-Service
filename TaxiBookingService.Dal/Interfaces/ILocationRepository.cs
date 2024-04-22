using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ILocationRepository<T>
    {
        Task<T> GetById(int id);

    }
}
