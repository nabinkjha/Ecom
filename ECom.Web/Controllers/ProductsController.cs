//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using ECom.Web.Data;
//using ECom.Web.Models;

//namespace ECom.Web.Controllers
//{
//    public class ProductsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public ProductsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Products
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Product.Include(p => p.ProductCategory);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Products/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Product
//                .Include(p => p.ProductCategory)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        // GET: Products/Create
//        public IActionResult Create()
//        {
//            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Id");
//            return View();
//        }

//        // POST: Products/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Name,SKU,Slug,IsFeatured,ImageUrl,CreatedDate,Description,Price,Rating,Brand,ReviewCount,StockCount,ProductCategoryId")] Product product)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(product);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Id", product.ProductCategoryId);
//            return View(product);
//        }

//        // GET: Products/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Product.FindAsync(id);
//            if (product == null)
//            {
//                return NotFound();
//            }
//            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Id", product.ProductCategoryId);
//            return View(product);
//        }

//        // POST: Products/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SKU,Slug,IsFeatured,ImageUrl,CreatedDate,Description,Price,Rating,Brand,ReviewCount,StockCount,ProductCategoryId")] Product product)
//        {
//            if (id != product.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(product);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ProductExists(product.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Id", product.ProductCategoryId);
//            return View(product);
//        }

//        // GET: Products/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Product
//                .Include(p => p.ProductCategory)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        // POST: Products/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var product = await _context.Product.FindAsync(id);
//            _context.Product.Remove(product);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProductExists(int id)
//        {
//            return _context.Product.Any(e => e.Id == id);
//        }
//    }
//}
