using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class ScheduleRideRepository : Repository<ScheduledRide>, IScheduleRideRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public ScheduleRideRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CreateScheduleRide(int rideId, DateTime ScheduledDate)
        {
            var ScheduleRide = new ScheduledRide
            {
                RideId = rideId,
                ScheduledDate = ScheduledDate
            };
            await _context.ScheduledRide.AddAsync(ScheduleRide);
            return ScheduleRide.Id;
        }

        public async Task<List<ScheduledRide>> GetAllActiveSchedulerides()
        {
            return _context.ScheduledRide.Where(x=>x.ScheduledDate==DateTime.Now).ToList(); 
        }
    }
}
