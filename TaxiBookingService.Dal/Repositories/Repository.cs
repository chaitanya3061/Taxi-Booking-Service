using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Interfaces;

namespace TaxiBookingService.Dal.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TaxiBookingServiceDbContext _dbContext;

        protected Repository(TaxiBookingServiceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
             _dbContext.Set<T>().Remove(entity);
        }

        public async Task Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task<bool> Exists(int id)
        {
           return  _dbContext.Set<T>().Find(id) != null;
        }
    }
}
