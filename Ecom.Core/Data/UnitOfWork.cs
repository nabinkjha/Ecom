using ECom.Contracts.Data.Repositories;

namespace ECom.Core.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        public IApiClientRepository ApiClient => new ApiClientRepository(_context);
        public IProductRepository Product => new ProductRepository(_context);

        public IProductCategoryRepository ProductCategory => new ProductCategoryRepository(_context);

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}