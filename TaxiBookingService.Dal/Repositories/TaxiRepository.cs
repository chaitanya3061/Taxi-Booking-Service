using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public class TaxiRepository:Repository<Taxi>,ITaxiRepository
    {
        private readonly TaxiBookingServiceDbContext _context;

        public TaxiRepository(TaxiBookingServiceDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<Taxi> GetTaxiByDriverId(int driverId)
        {
            return await _context.Taxi.FirstOrDefaultAsync(t => t.DriverId == driverId);
        }

        public async Task<int> AddTaxi(DriverTaxiDto request, int driverId)
        {
            return 1;
        }

        //public async Task UpdateTaxi(int taxiId, DriverTaxiDto taxi)
        //{
        //    var existingTaxi = await _context.Taxi.FindAsync(taxiId);
        //    if (existingTaxi == null)
        //    {
        //        throw new Exception("Taxi not found.");
        //    }
        //    var taxiType = await _context.TaxiType.FirstOrDefaultAsync(r => r.Name.ToLower() == taxi.TaxiType.ToLower());

        //    existingTaxi.Name = taxi.Name;
        //    existingTaxi.RegistrationNumber = taxi.RegistrationNumber;
        //    existingTaxi.Color = taxi.Color;
        //    existingTaxi.TaxiTypeId = taxiType.Id;
        //    _context.Entry(taxi).State = EntityState.Modified;

        //    await _context.SaveChangesAsync();
        //}
    }
}
