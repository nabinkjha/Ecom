using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ECom.Contracts.Data.Repositories;
using System.Linq;
using Microsoft.AspNetCore.Http;
using ECom.Core.Entities;
using Microsoft.AspNetCore.OData.Formatter;
using System.Collections.Generic;

namespace ECom.API.Controllers.OData.v2
{
    [Produces("application/json")]
    public class ProductController : ODataController
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
        [EnableQuery(PageSize = 10, MaxExpansionDepth = 5)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(404)]
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
        /// <param name="key"></param>
        /// <returns>Single Product</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public IActionResult Get([FromODataUri] int key)
        {
            var entity = _uow.Product.Get(key);
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
        public IActionResult Create([FromBody] Product item)
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
            _uow.Commit();
            return Ok(item);
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
        public IActionResult Update(int id, Product item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }
            var product = _uow.Product.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            product.Name = item.Name;
            _uow.Product.Update(product);
            _uow.Commit();
            return NoContent();
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
        public IActionResult Delete([FromODataUri] int id)
        {
            var item = _uow.Product.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            _uow.Product.Delete(item.Id);
            _uow.Commit();
            return NoContent();
        }
    }
}