using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class TrustedContactRepository: Repository<TrustedContacts>, ITrustedContactRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public TrustedContactRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
