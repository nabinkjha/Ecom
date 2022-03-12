using System.Threading.Tasks;
using System.Net.Http;
using ECom.Web.Models;
using Simple.OData.Client;
using System.Linq;
using ECom.Web.RESTClients;
using Microsoft.Extensions.Options;
using ECom.Web.Common;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using System.Collections.Generic;

namespace WebApp.RESTClients
{
    public class ProductHttpClient : BaseHttpClient, IProductHttpClient
    {
        private IMapper _mapper;
        public ProductHttpClient(IMapper mapper, IOptions<ApplicationParameters> config, HttpClient httpClient, IMemoryCache memoryCache) : base(config, httpClient, memoryCache)
        {
            _mapper = mapper;
        }
        public async Task<ProductSearchResult> GetSearchResult(ProductSearchParameter param)
        {
            var annotations = new ODataFeedAnnotations();
            var productCommand = oDataClient.For<Product>()
                .Expand(x => x.ProductCategory)
                .Select(x => new { x.Id, x.Name, x.ProductCategory, x.Price, x.SKU, x.Slug, x.StockCount })
                .Top(param.length)
                .Skip(param.start);
            AddCustomFilterByCondtion(param, productCommand);
            if (!string.IsNullOrEmpty(param.search?.value))
            {
                productCommand.Filter(x => x.Name.Contains(param.search.value) || x.Description.Contains(param.search.value));
            }
            productCommand = AddSortingCondition(param, productCommand);
            var products = await productCommand.FindEntriesAsync(annotations);
            var result = new ProductSearchResult
            {
                draw = param.draw,
                Data = _mapper.Map<List<ProductViewModel>>(products.ToList()),
                RecordsFiltered = annotations.Count ?? 0,
                RecordsTotal = annotations.Count ?? 0
            };
            return result;
        }

        private static void AddCustomFilterByCondtion(ProductSearchParameter param, IBoundClient<Product> productCommand)
        {
            if (param.FilterBy.Length > 0)
            {
                if (param.FilterBy.Any(x => x.PropertyName == "ProductCategoryId"))
                {
                    int.TryParse(param.FilterBy.First(x => x.PropertyName == "ProductCategoryId").PropertyValue, out int productCategoryId);
                    if (productCategoryId > 0)
                    {
                        productCommand.Filter(x => x.ProductCategoryId == productCategoryId);
                    }
                }
            }
        }

        private static IBoundClient<Product> AddSortingCondition(ProductSearchParameter param, IBoundClient<Product> productCommand)
        {
            if (param.order?.Length > 0)
            {
                var sortColumn = param.SortColumn.Split(" ");
                if (sortColumn.Length > 1)
                {
                    var sortColumnName = sortColumn[0];
                    if (sortColumnName.Equals("category"))
                    {
                        if (sortColumn[1] == "desc")
                            productCommand = productCommand.OrderByDescending(x => x.ProductCategory.Name);
                        else
                            productCommand = productCommand.OrderBy(x => x.ProductCategory.Name);
                    }
                    else
                    {
                        if (sortColumn[1] == "desc")
                            productCommand = productCommand.OrderByDescending(sortColumnName);
                        else
                            productCommand = productCommand.OrderBy(sortColumnName);
                    }
                }
            }
            return productCommand;
        }

        public async Task<ProductViewModel> GetById(int id)
        {
            var product = await oDataClient.For<Product>("Product").Filter(x => x.Id == id).FindEntryAsync();
            var result = _mapper.Map<ProductViewModel>(product);
            return result;
        }

        public async Task<ProductViewModel> Create(Product entity)
        {
            var product = await oDataClient.For<Product>("Product")// This name "Product" must match with API entity name.
             .Set(entity)
             .InsertEntryAsync();
            var result = _mapper.Map<ProductViewModel>(product);
            return result;
        }

        public async Task<ProductViewModel> Update(Product entity)
        {
            var product = await oDataClient.For<Product>("Product")
             .Key(entity.Id)
             .Set(entity)
             .UpdateEntryAsync();
            var result = _mapper.Map<ProductViewModel>(product);
            return result;
        }

        public async Task Delete(int id)
        {
            await oDataClient.For<Product>()
            .Key(id)
            .DeleteEntryAsync();
        }
    }
}
