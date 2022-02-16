using ECom.Contracts.Data.Repositories;
using ECom.Core.Entities;

namespace ECom.Core.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DatabaseContext context) : base(context)
        {
        }
    }
}