
using System.Threading.Tasks;

namespace ECom.Contracts.Data.Repositories
{
    public interface IUnitOfWork
    {
        IApiClientRepository ApiClient { get; }
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        Task<int> Commit();
    }
}