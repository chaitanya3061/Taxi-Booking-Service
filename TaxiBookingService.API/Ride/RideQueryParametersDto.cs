using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.API.Ride
{
    public class RideQueryParametersDto
    {
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public string SearchQuery { get; set; }
        public int Limit { get; set; }
        public int OffSet { get; set; }
        public int? FilterByStatus { get; set; }
    }
}
