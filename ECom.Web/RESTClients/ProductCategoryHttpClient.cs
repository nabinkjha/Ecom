using System.Threading.Tasks;
using System.Net.Http;
using ECom.Web.Models;
using Simple.OData.Client;
using System.Linq;
using ECom.Web.RESTClients;
using Microsoft.Extensions.Options;
using ECom.Web.Common;
using Microsoft.Extensions.Caching.Memory;

namespace WebApp.RESTClients
{
    public class ProductCategoryHttpClient : BaseHttpClient, IProductCategoryHttpClient
    {
        public ProductCategoryHttpClient(IOptions<ApplicationParameters> config, HttpClient httpClient, IMemoryCache memoryCache) : base(config, httpClient, memoryCache)
        {

        }
        public async Task<ProductCategorySearchResult> GetSearchResult(ProductCategorySearchParameter param)
        {
            var annotations = new ODataFeedAnnotations();
            var productCategoriesCommand = oDataClient.For<ProductCategory>().Top(param.length);
            if (!string.IsNullOrEmpty(param.search?.value))
            {
                productCategoriesCommand.Filter(x => param.search.value.Contains(x.Name));
            }
            if (param.start > 0)
            {
                productCategoriesCommand.Skip(param.start);
            }
            if (param.order?.Length > 0)
            {
                var sortColumn = param.SortColumn.Split(" ");
                if (sortColumn.Length > 1)
                {
                    if (sortColumn[1] == "desc")
                        productCategoriesCommand = productCategoriesCommand.OrderByDescending(sortColumn[0]);
                    else
                        productCategoriesCommand = productCategoriesCommand.OrderBy(sortColumn[0]);
                }
            }
            else
            {
                productCategoriesCommand = productCategoriesCommand.OrderBy("Name");
            }
            var productCategories = await productCategoriesCommand.FindEntriesAsync(annotations);
            var result = new ProductCategorySearchResult
            {
                draw = param.draw,
                Data = productCategories.ToList(),
                RecordsFiltered = annotations.Count ?? 0,
                RecordsTotal = annotations.Count ?? 0
            };
            return result;
        }

        public async Task<ProductCategory> GetById(int id)
        {
            var productCategory = await oDataClient.For<ProductCategory>("ProductCategory").Filter(x => x.Id == id).FindEntryAsync();
            return productCategory;
        }

        public async Task<ProductCategory> Create(ProductCategory entity)
        {
            var productCategory = await oDataClient.For<ProductCategory>()
             .Set(entity)
             .InsertEntryAsync();
            return productCategory;
        }

        public async Task<ProductCategory> Update(ProductCategory entity)
        {
            var productCategory = await oDataClient.For<ProductCategory>()
             .Key(entity.Id)
             .Set(entity)
             .UpdateEntryAsync();
            return productCategory;
        }

        public async Task Delete(int id)
        {
            await oDataClient.For<ProductCategory>()
            .Key(id)
            .DeleteEntryAsync();
        }
    }

}
