using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Enums;

namespace TaxiBookingService.Common.Enums
{
    public enum RideStatus
    {
        Searching=1,
        Accepted,
        Started,
        Completed,
        Cancelled
    }
}
