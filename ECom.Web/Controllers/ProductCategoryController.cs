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
        // GET: Products/Create
        public IActionResult Create()
        {
            var productCategory =new ProductCategory();
            return PartialView("_Edit", productCategory);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var productCategory = await _productCategoryHttpClient.GetById(id.Value);
            return PartialView("_Edit", productCategory);
        }
       
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id, [Bind("Id,Name,Description,IsActive")] ProductCategory productCategory)
        {
            var action = id == 0 ? "Created" : "Updated";
            bool success;
            if (!ModelState.IsValid)
            {
                return PartialView(productCategory);
            }
            productCategory = id == 0 ? await _productCategoryHttpClient.Create(productCategory) : await _productCategoryHttpClient.Update(productCategory);
            success = string.IsNullOrWhiteSpace(productCategory.ErrorMessage);
            return Json(new { success, message = success ? $"The product category {action} successfully" : productCategory.ErrorMessage, title = "Product Category" + action });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productCategoryHttpClient.Delete(id);
            return Json(new { message = "Product Category deleted successfully.", title = "Product Category Deleted" });
        }

    }
}
