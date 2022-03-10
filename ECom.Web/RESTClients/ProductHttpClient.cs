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
    public class ProductHttpClient : BaseHttpClient, IProductHttpClient
    {
        public ProductHttpClient(IOptions<ApplicationParameters> config, HttpClient httpClient, IMemoryCache memoryCache) : base(config, httpClient, memoryCache)
        {

        }
        public async Task<ProductSearchResult> GetSearchResult(ProductSearchParameter param)
        {
            var annotations = new ODataFeedAnnotations();
            var productCommand = oDataClient.For<Product>()
                .Expand(x => x.ProductCategory)
                .Select(x => new { x.Id, x.Name, x.ProductCategory, x.Price, x.SKU, x.Slug, x.StockCount })
                .Top(param.length)
                .Skip(param.start);
            if (param.FilterBy.Length >0)
            {
                if (param.FilterBy.Any(x=>x.PropertyName == "ProductCategoryId"))
                {
                    int productCategoryId = 0;
                    int.TryParse(param.FilterBy.First(x => x.PropertyName == "ProductCategoryId").PropertyValue, out productCategoryId);
                    if (productCategoryId > 0)
                    {
                        productCommand.Filter(x => x.ProductCategoryId == productCategoryId);
                    }
                }
            }
            if (!string.IsNullOrEmpty(param.search?.value))
            {
                productCommand.Filter(x => param.search.value.Contains(x.Name));
            }
            if (param.order?.Length > 0)
            {
                var sortColumn = param.SortColumn.Split(" ");
                if (sortColumn.Length > 1)
                {
                    if (sortColumn[1] == "desc")
                        productCommand = productCommand.OrderByDescending(sortColumn[0]);
                    else
                        productCommand = productCommand.OrderBy(sortColumn[0]);
                }
            }
            var products = await productCommand.FindEntriesAsync(annotations);
            var result = new ProductSearchResult
            {
                draw = param.draw,
                Data = products.ToList(),
                RecordsFiltered = annotations.Count ?? 0,
                RecordsTotal = annotations.Count ?? 0
            };
            return result;
        }

        public async Task<Product> GetById(int id)
        {
            var product = await oDataClient.For<Product>("Product").Filter(x => x.Id == id).FindEntryAsync();
            return product;
        }

        public async Task<Product> Create(Product entity)
        {
            var product = await oDataClient.For<Product>()
             .Set(entity)
             .InsertEntryAsync();
            return product;
        }

        public async Task<Product> Update(Product entity)
        {
            var product = await oDataClient.For<Product>()
             .Key(entity.Id)
             .Set(entity)
             .UpdateEntryAsync();
            return product;
        }

        public async Task Delete(int id)
        {
            await oDataClient.For<Product>()
            .Key(id)
            .DeleteEntryAsync();
        }
    }
}
