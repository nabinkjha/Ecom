using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ECom.Contracts.Data.Repositories;
using System.Linq;
using ECom.Core.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using ECom.API.Controllers.OData.Login;

namespace ECom.API.Controllers.OData.v2
{
    public class ProductCategoryController : BaseODataController
    {
        private readonly IUnitOfWork _uow;
        public ProductCategoryController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        /// <summary>
        /// Gets all Product
        /// Use the GET http verb
        /// Request for v2/ProductCategory
        /// </summary>
        /// <returns>List of ProductCategory</returns>
        /// <response code="200">Returns IEnumerable of ProductCategory </response>
        /// <response code="401">If the user is not authorize or JWT token expired</response>   
        [EnableQuery(PageSize = 10, MaxExpansionDepth = 5)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductCategory>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("v2/ProductCategory")]
        public IActionResult Get()
        {
            var items = _uow.ProductCategory.GetAll().AsQueryable();
            return Ok(items);
        }
        /// <summary>
        /// Gets single Product
        /// Use the GET http verb
        /// Request for v2/ProductCategory(3)
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Single ProductCategory</returns>
        /// <response code="200">Returns a single Product that matches the Id </response>
        /// <response code="404">Returns a 404 NotFound if the product category does not exist </response>
        /// <response code="401">If the user is not authorize or JWT token expired</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(200, Type = typeof(ProductCategory))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("v2/ProductCategory({id})")]
        [HttpGet("v2/ProductCategory/{id}")]
        public IActionResult Get(int id)
        {
            var entity = _uow.ProductCategory.Get(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        /// <summary>
        /// Creates a ProductCategory.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /ProductCategory
        ///     {
        ///        "name": "ProductCategory 1 ",
        ///        
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created ProductCategory</returns>
        /// <response code="201">Returns the newly created ProductCategory</response>
        /// <response code="400">If the item is null</response>            
        /// <response code="401">If the user is not authorize or JWT token expired</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("v2/ProductCategory")]
        public ActionResult<ProductCategory> Create(ProductCategory item)
        {
            _uow.ProductCategory.Add(item);
            _uow.Commit();
            return Ok(item);
        }
        /// <summary>
        /// Update a specific ProductCategory.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /ProductCategory/1
        ///     {
        ///        "id": 1,
        ///        "name": "ProductCategory Update",
        ///        
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <response code="401">If the user is not authorize or JWT token expired</response>
        /// <response code="400">Returns a 404 NotFound if the product category does not exist </response>
        /// <response code="404">Returns a 400 BadRequest if the product category parameter is null or param id is not matched with id in the ProductCategory </response>
        /// <response code="204">Returns a 204 NoContent if the request is successfuly completed. </response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut("v2/ProductCategory")]
        public IActionResult Update(int id, ProductCategory item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }
            var productType = _uow.ProductCategory.Get(id);
            if (productType == null)
            {
                return NotFound();
            }
            productType.Name = item.Name;
            _uow.ProductCategory.Update(productType);
            _uow.Commit();
            return NoContent();
        }
        /// <summary>
        /// Use the DELETE http verb
        /// Request for v2/productcategory(5) or v2/productcategory/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <summary>
        /// Deletes a specific ProductCategory.
        /// </summary>      
        /// <response code="401">If the user is not authorize or JWT token expired</response>
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("v2/ProductCategory")]
        public IActionResult Delete(int id)
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