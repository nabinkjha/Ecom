using ECom.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApp.RESTClients;

namespace ECom.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        public IProductHttpClient _productHttpClient;
        public IProductCategoryHttpClient _productCategoryHttpClient;
        public ProductController(ILogger<ProductController> logger, IProductHttpClient productHttpClient, IProductCategoryHttpClient productCategoryHttpClient)
        {
            _logger = logger;
            _productHttpClient = productHttpClient;
            _productCategoryHttpClient = productCategoryHttpClient;
        }
        public IActionResult Index()
        {
            return View();
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
            var param = new ProductCategorySearchParameter { };
            // var result = await _productCategoryHttpClient.GetSearchResult(param);
            return PartialView("_Edit");
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _productHttpClient.GetById(id.Value);
            return PartialView("_Edit", product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id, [Bind("Id,Name,SKU,Slug,IsFeatured,ImageUrl,CreatedDate,Description,Price,Rating,Brand,ReviewCount,StockCount,ProductCategoryId")] Product product)
        {
            bool success;
            if (!ModelState.IsValid)
            {
                return PartialView(product);
            }
            product = id == 0 ? await _productHttpClient.Create(product) : await _productHttpClient.Update(product);
            success = string.IsNullOrWhiteSpace(product.ErrorMessage);
            return Json(new { success, message = success ? product.SuccessMessage : product.ErrorMessage });
        }

        //// GET: Products/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Product
        //        .Include(p => p.ProductCategory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var product = await _context.Product.FindAsync(id);
        //    _context.Product.Remove(product);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ProductExists(int id)
        //{
        //    return _context.Product.Any(e => e.Id == id);
        //}

    }

}
