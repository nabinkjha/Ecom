using ECom.Web.Models;

namespace WebApp.RESTClients
{
    public interface IProductHttpClient : IBaseHttpClient<Product, ProductSearchParameter, ProductSearchResult>
    {
       
    }
}
