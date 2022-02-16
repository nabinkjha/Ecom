
namespace ECom.Contracts.Data.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Product { get; }
        IProductCategoryRepository ProductCategory { get; }
        void Commit();
    }
}