
namespace ECom.Contracts.Data.Repositories
{
    public interface IUnitOfWork
    {
        IApiClientRepository ApiClient { get; }
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        void Commit();
    }
}