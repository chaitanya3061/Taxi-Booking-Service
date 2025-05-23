﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class TariffChargeRepository : ITariffChargeRepository<TariffCharge>
    {
        private readonly TaxiBookingServiceDbContext _context;

        public TariffChargeRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }

        public async Task<List<TariffCharge>> GetTariffCharges()
        {
            return await _context.TariffCharge.ToListAsync();
        }
    }
}
