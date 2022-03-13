using ECom.Web.Models;

namespace WebApp.RESTClients
{
    public interface IProductCategoryHttpClient : IBaseHttpClient<ProductCategory,ProductCategory, ProductCategorySearchParameter, ProductCategorySearchResult>
    {
       
    }
}
