using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApp.RESTClients;

namespace ECom.Web.Controllers
{
    public class ProductCategoryController : Controller
    {
        private readonly ILogger<ProductCategoryController> _logger;
        public IProductCategoryHttpClient _productCategoryHttpClient;

        public ProductCategoryController(ILogger<ProductCategoryController> logger, IProductCategoryHttpClient productCategoryHttpClient)
        {
            _logger = logger;
            _productCategoryHttpClient = productCategoryHttpClient;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> GetSearchResult([FromBody] ProductCategorySearchParameter param)
        {
            var result = await _productCategoryHttpClient.GetSearchResult(param);
            return Json(result);
        }
        
     
    }
}
