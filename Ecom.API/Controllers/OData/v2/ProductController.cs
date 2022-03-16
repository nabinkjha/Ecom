using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ECom.Contracts.Data.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ECom.Core.Entities;
using Microsoft.AspNetCore.OData.Formatter;
using System.Collections.Generic;
using ECom.API.Controllers.OData.Login;

using Microsoft.AspNetCore.OData.Deltas;
using System.Threading.Tasks;

namespace ECom.API.Controllers.OData.v2
{
    public class ProductController : BaseODataController
    {
        private readonly IUnitOfWork _uow;

        public ProductController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Gets all Product
        /// Use the GET http verb
        /// Request for v2/Product
        /// </summary>
        /// <returns>List of Products</returns>
        [EnableQuery]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(404)]
        [HttpGet(template: "v2/Product")]
        public IActionResult Get()
        {
            var items = _uow.Product.GetAll().AsQueryable();
            return Ok(items);
        }
        /// <summary>
        /// Gets single Product
        /// Use the GET http verb
        /// Request for v2/Product(3)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single Product</returns>
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        [EnableQuery]
        [HttpGet("v2/Product/{id:int}")]
        [HttpGet("v2/Product({id:int})")]
        public IActionResult GetProduct(int id)
        {
            var entity = _uow.Product.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        /// <summary>
        /// Creates a Product.
        /// Use the POST http verb.
        /// Request for v2/product
        /// Set Content-Type:Application/Json
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Product
        ///     {
        ///        "id": 0, To create a new product pass id as 0.
        ///        "name": "Product 1 ",
        ///        "producttypeid": 3
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created Product</returns>
        /// <response code="201">Returns the newly created Product</response>
        /// <response code="400">If the item is null</response>            
        [HttpPost("v2/Product")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Product item)
        {
            if (item == null)
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _uow.Product.Add(item);
            await _uow.Commit();
            return Created(item);
        }
        /// <summary>
        /// Update a specific Product.
        /// Saves the entire Product object to the object specified by key (id). Is supposed to overwrite all properties.
        /// Use the PUT http verb
        /// Request for v2/product/1 or v2/product(1)
        /// Set Content-Type:Application/Json
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Product/1
        ///     {
        ///        "id": 1,
        ///        "name": "Updated Value",
        ///        
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpPut("v2/Product({id})")]
        [HttpPut("v2/Product/{id}")]
        public async Task<IActionResult> Update(int id, Product item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }
            var product = _uow.Product.Get(id);
            if (product == null)
            {
                return NotFound($"Not found product with id = {id}");
            }
            product.Name = item.Name;
            _uow.Product.Update(product);
            await _uow.Commit();
            return Updated(product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpPatch("v2/Product({id})")]
        [HttpPatch("v2/Product/{id}")]
        public async Task<IActionResult> Patch(int id, Delta<Product> product)
        {
            if (product != null)
            {
                var original = _uow.Product.Get(id);
                if (original == null)
                {
                    return NotFound($"Not found product with id = {id}");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                product.Patch(original);
                await _uow.Commit();
                return Updated(original);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        /// <summary>
        /// Use the DELETE http verb
        /// Request for v2/product(5) or  v2/product/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <summary>
        /// Deletes a specific Product.
        /// </summary>
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [HttpDelete("v2/Product({id})")]
        [HttpDelete("v2/Product/{id}")]
        public async Task<IActionResult> Delete([FromODataUri] int id)
        {
            var item = _uow.Product.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            _uow.Product.Delete(item.Id);
            await _uow.Commit();
            return NoContent();
        }
    }
}