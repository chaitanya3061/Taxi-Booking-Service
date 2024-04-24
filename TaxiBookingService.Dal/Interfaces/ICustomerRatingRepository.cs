using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Dal.Interfaces
{
    public interface ICustomerRatingRepository : IRepository<CustomerRating>
    {
        Task<List<CustomerRating>> GetRatingsByDriverId(int Id);
    }
}
