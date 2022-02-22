using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Simple.OData.Client;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECom.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
        private ODataClient _client;
        private readonly JsonSerializerOptions _options;
        public ProductCategoryController()
        {
            _client = new ODataClient("https://localhost:44311/v1/");
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<IActionResult> Index()
        {
            //var product = await _client.For<Product>().Filter(x => x.Name == "Chai").FindEntriesAsync();
            //var category = await _client.For<ProductCategory>().Filter(x => x.Name == "Beverages").FindEntriesAsync();
            ////client.For<Product>().Key(product.ProductID).UnlinkEntry(x => x.ProductCategory);
            //var productCategories = await _client.FindEntriesAsync("ProductCategory?$filter=Id gt 3&$select=Id,Name&$skip=2&$top=100");
            //var result = _client.For<Product>().Filter(x => x.Name.StartsWith("A")).DeleteEntriesAsync();
            //return View(productCategories);
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetList([FromBody] ProductSearchParameter param)
        {
            var products = await GetProductWithCount(param.start,param.length, param.SortColumn, param.search?.value);
            var helper = new ProductSearchResult
            {
                draw = param.draw,
                Data = products.value.ToList(),
                RecordsFiltered = products.odatacount,
                RecordsTotal = products.odatacount
            };
            return Json(helper);

            //var searchResult = new List<Product>();
            //var annotations = new ODataFeedAnnotations();
            //var products = await _client
            //    .For<Product>()
            //    .FindEntriesAsync(annotations);
            //searchResult.AddRange(products);
            //while (annotations.NextPageLink != null)
            //{
            //    searchResult.AddRange(await _client
            //        .For<Product>()
            //        .FindEntriesAsync(annotations.NextPageLink, annotations));
            //}

            //var countCommand = _client.For<Product>();
            //var command = _client.For<Product>()
            //      .Top(param.Length)
            //      .Skip((param.Start - 1) * param.Length)
            //    ;
            //if (!string.IsNullOrEmpty(param.SearchText?.Text))
            //{
            //    command.Filter(x => param.SearchText.Text.Contains(x.Name));
            //}
            //var products = await command
            //    .FindEntriesAsync(annotations);
            //var productCount = await countCommand.FindScalarAsync<int>();
        }
        
        private async Task<ProductSearchResult> GetProductWithCount(int skipRecord,int pageSize,string sortBy,string searchText)
        {
            var baseURL = "https://localhost:44311/v1/product?$count=true&$top=" + pageSize;
            if (skipRecord > 0)
            {
                baseURL += $"&$skip={skipRecord}";
            }
            if(!string.IsNullOrEmpty(sortBy))
            {
                baseURL += $"&$orderby={sortBy}";
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                baseURL += $"&$filter=contains(Name,'{searchText}')";
            }
            var httpClient = new HttpClient();
            var result = await httpClient.GetStringAsync(baseURL);
            result = result.Replace("@odata.context", "odatacontext").Replace("@odata.count", "odatacount");
            var products = JsonSerializer.Deserialize<ProductSearchResult>(result, _options);
            return products;
        }
    }
}
