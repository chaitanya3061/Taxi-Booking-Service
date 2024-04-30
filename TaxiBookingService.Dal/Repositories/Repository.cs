using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Exists(int id)
        {
           return  _dbContext.Set<T>().Find(id) != null;
        }
    }
}
