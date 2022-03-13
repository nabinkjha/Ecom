using AutoMapper;
using ECom.Web.Common;
using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.RESTClients;

namespace ECom.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly ILogger<ProductController> _logger;
        public IProductHttpClient _productHttpClient;
        public IProductCategoryHttpClient _productCategoryHttpClient;
        private readonly IMapper _mapper;
        public ProductController(ILogger<ProductController> logger, IProductHttpClient productHttpClient, IProductCategoryHttpClient productCategoryHttpClient, IMapper mapper)
        {
            _logger = logger;
            _productHttpClient = productHttpClient;
            _productCategoryHttpClient = productCategoryHttpClient;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            AddPageHeader("Products", "");
            AddBreadcrumb("Product", "/Product");
            var searchParameter = new ProductSearchParameter
            {
                ProductCategorySelectList = await PopulateProductCategorySelectList(0, true)
            };
            return View(searchParameter);
        }

        [HttpPost]
        public async Task<JsonResult> GetProductList([FromBody] ProductSearchParameter param)
        {
            var result = await _productHttpClient.GetSearchResult(param);
            return Json(result);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
            var product = new ProductViewModel { CreatedDate = System.DateTime.Now };
            product.ProductCategorySelectList = await PopulateProductCategorySelectList();
            return PartialView("_Edit", product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _productHttpClient.GetById(id.Value);
            product.ProductCategorySelectList = await PopulateProductCategorySelectList(product.ProductCategoryId);
            return PartialView("_Edit", product);
        }
        private async Task<List<SelectListItem>> PopulateProductCategorySelectList(int productCategoryId = 0, bool addPleaseDefault = false)
        {
            var param = new ProductCategorySearchParameter { length = 100 };
            var productCategories = await _productCategoryHttpClient.GetSearchResult(param);
            return SelectedListHelper.GetProductCategorySelectList(productCategories.Data, productCategoryId, addPleaseDefault);
        }
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id, [Bind("Id,Name,SKU,Slug,IsFeatured,ImageUrl,CreatedDate,Description,Price,Rating,Brand,ReviewCount,StockCount,ProductCategoryId")] ProductViewModel productViewModel)
        {
            var action = id == 0 ? "Created" : "Updated";
            if (!ModelState.IsValid)
            {
                productViewModel.ProductCategorySelectList = await PopulateProductCategorySelectList(productViewModel.ProductCategoryId, id == 0);
                return PartialView("_Edit", productViewModel);
            }
            var product = _mapper.Map<Product>(productViewModel);
            productViewModel = id == 0 ? await _productHttpClient.Create(product) : await _productHttpClient.Update(product);
            return Json(new { success = !productViewModel.HasError, message = $"The product {productViewModel.Name + " " + action} successfully", title = "Product " + action });
        }


        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productHttpClient.Delete(id);
            return Json(new { message = "Product deleted successfully.", title = "Product Deleted" });
        }
    }

}
