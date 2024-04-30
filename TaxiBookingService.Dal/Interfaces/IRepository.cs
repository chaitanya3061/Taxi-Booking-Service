
namespace TaxiBookingService.Dal.Interfaces
{
    public interface IRepository<T> :IFactoryRepository where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> Add(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity);
        Task<bool> Exists(int id);
    }
}
