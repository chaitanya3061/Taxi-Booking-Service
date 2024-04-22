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
    public class TaxiRepository:ITaxiRepository<Taxi>
    {
        private readonly TaxiBookingServiceDbContext _context;

        public TaxiRepository(TaxiBookingServiceDbContext context)
        {
            _context = context;
        }
        public async Task<Taxi> GetTaxiByDriverId(int driverId)
        {
            return await _context.Taxi.FirstOrDefaultAsync(t => t.DriverId == driverId);
        }

        public async Task<int> AddTaxi(DriverTaxiServiceContracts request, int driverId)
        {
            var taxiType = await _context.TaxiType.FirstOrDefaultAsync(r => r.Name.ToLower() == request.TaxiType.ToLower());
            var Taxi = new Taxi
            {
                DriverId = driverId,
                Name = request.Name,
                RegistrationNumber = request.RegistrationNumber,
                Color = request.Color,
                TaxiTypeId = taxiType.Id,
            };
            _context.Taxi.Add(Taxi);
            await _context.SaveChangesAsync();
            return Taxi.Id;
        }

        public async Task<Taxi> GetTaxiById(int taxiId)
        {
            return await _context.Taxi.FindAsync(taxiId);
        }

        public async Task UpdateTaxi(int taxiId, DriverTaxiServiceContracts taxi)
        {
            var existingTaxi = await _context.Taxi.FindAsync(taxiId);
            if (existingTaxi == null)
            {
                throw new Exception("Taxi not found.");
            }
            var taxiType = await _context.TaxiType.FirstOrDefaultAsync(r => r.Name.ToLower() == taxi.TaxiType.ToLower());

            existingTaxi.Name = taxi.Name;
            existingTaxi.RegistrationNumber = taxi.RegistrationNumber;
            existingTaxi.Color = taxi.Color;
            existingTaxi.TaxiTypeId = taxiType.Id;
            _context.Entry(taxi).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
