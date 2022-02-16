using ECom.Contracts.Data.Repositories;
using ECom.Core.Entities;

namespace ECom.Core.Data.Repositories
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(DatabaseContext context) : base(context)
        {
        }
    }
}