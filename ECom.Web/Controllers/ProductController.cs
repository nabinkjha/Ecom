using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECom.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly JsonSerializerOptions _options;
        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<IActionResult> Index()
        {
            var user = await GetTokenToAccessOdataAPI();
            var setting = GetSettingWithToken("https://localhost:44311/v2/", user.Token);
            var client = new ODataClient(setting);
            var products = await client.For<Product>()// Name of the entity must match the API endpoint controller name
                   .Select(x=> (new { x.Id , x.Name,x.SKU,x.Slug, x.ProductCategory}))
                   .Expand(p => p.ProductCategory)
                   .Top(10).Skip(100)
                   .FindEntriesAsync();
            return View(products);
        }

        private async Task<ApplicationLoginViewModel> GetTokenToAccessOdataAPI()
        {
            var httpClient = new HttpClient();
            var result = await httpClient.PostAsJsonAsync("https://localhost:44311/v2/Authenticate", new
            {
                UserName = "odata",
                Password = "odata123"
            }, _options);
            result.EnsureSuccessStatusCode();
            var content = await result.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<ApplicationLoginViewModel>(content);
            return user;
        }

        private ODataClientSettings GetSettingWithToken(string url, string accessToken)
        {
            var clientSettings = new ODataClientSettings(new Uri(url));
            clientSettings.BeforeRequest += delegate (HttpRequestMessage message)
            {
                message.Headers.Add("Authorization", "Bearer " + accessToken);
            };
            return clientSettings;
        }
    }
  
}
