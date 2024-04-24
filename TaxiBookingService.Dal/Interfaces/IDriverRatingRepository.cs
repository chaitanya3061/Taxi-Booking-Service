using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface IDriverRatingRepository : IRepository<DriverRating>
    {
        Task<List<DriverRating>> GetRatingsByCustomerId(int Id);
    }
}
