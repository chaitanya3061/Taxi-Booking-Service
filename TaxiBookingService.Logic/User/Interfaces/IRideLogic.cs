using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IRideLogic
    {
        Task GetDriverAsync(int rideid);
        Task<decimal> CalculateFare(decimal pickUpLat, decimal pickUpLong, decimal dropOffLat, decimal dropOffLong);
    }
}
