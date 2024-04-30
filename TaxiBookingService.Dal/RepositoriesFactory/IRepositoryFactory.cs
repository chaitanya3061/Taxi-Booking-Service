
namespace TaxiBookingService.Dal.RepositoriesFactory
{
    public interface IRepositoryFactory
    {
        TRepository CreateRepository<TRepository>() where TRepository : class;
    }
}
