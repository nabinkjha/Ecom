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

        public async Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> CreateProduct(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> UpdateProduct(Product entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
