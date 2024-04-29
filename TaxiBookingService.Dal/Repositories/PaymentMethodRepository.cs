using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class PaymentMethodRepository : Repository<PaymentMethod>, IPaymentMethodRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public PaymentMethodRepository(TaxiBookingServiceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
