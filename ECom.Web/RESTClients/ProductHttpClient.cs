using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System;
using ECom.Web.Models;
using Simple.OData.Client;
using System.Linq;
using ECom.Web.RESTClients;
using Microsoft.Extensions.Options;
using ECom.Web.Common;
using System.Text.Json;

namespace WebApp.RESTClients
{
    public class ProductHttpClient : BaseHttpClient, IProductHttpClient
    {
        public ProductHttpClient(IOptions<ApplicationParameters> config, HttpClient httpClient) : base(config, httpClient)
        {

        }
        public async Task<ProductSearchResult> GetSearchResult(ProductSearchParameter param)
        {
            var annotations = new ODataFeedAnnotations();
            var productCommand = oDataClient.For<Product>().Top(param.length);
            if (!string.IsNullOrEmpty(param.search?.value))
            {
                productCommand.Filter(x => param.search.value.Contains(x.Name));
            }
            if (param.start > 0)
            {
                productCommand.Skip(param.start);
            }
            if (param.order.Length > 0)
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
            var annotations = new ODataFeedAnnotations();
            var product = await GetProductById(id);
          //  var product = await oDataClient.For<Product>().Filter(x => x.Id == id).FindEntryAsync(annotations);
          //        var product = await oDataClient
          //                       .For<Product>()
          //.FindEntryAsync();
            return product;
        }
        private async Task<Product> GetProductById(int id)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var baseURL = $"https://localhost:44311/v1/product?$filter=Id eq {id}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("api-key", "12345abecdf516d0f6472d5199999");
            var result = await httpClient.GetStringAsync(baseURL);
            var product = JsonSerializer.Deserialize<ProductSearchResult>(result, options);
            return product.value.FirstOrDefault();
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

        public async Task<Product> Delete(int id)
        {
            var product = await oDataClient.For<Product>().Filter(x => x.Id == id).ExecuteAsSingleAsync();
            await oDataClient.For<Product>()
            .Key(id)
            .DeleteEntryAsync();
            return product;
        }
    }
}
