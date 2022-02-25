using ECom.Contracts.Data.Repositories;
using System.Threading.Tasks;

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

        public async Task<int> Commit()
        {
            return await _context.SaveChangesAsync();
        }
    }
}