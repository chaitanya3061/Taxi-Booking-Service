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
        Task<List<Driver>> FindNearbyDrivers(string customerloc, double maxDistance);
    }
}
