//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.OData.Query;
//using Microsoft.AspNetCore.OData.Routing.Controllers;
//using ECom.Contracts.Data.Repositories;
//using System.Linq;
//using ECom.Core.Entities;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Net.Mime;

//namespace ECom.API.Controllers.OData.v1
//{
//    [Produces(MediaTypeNames.Application.Json)]
//    [Consumes(MediaTypeNames.Application.Json)]
//    public class ProductCategoryController : ODataController
//    {
//        private readonly IUnitOfWork _uow;
//        public ProductCategoryController(IUnitOfWork uow)
//        {
//            _uow = uow;
//        }

//        /// <summary>
//        /// Gets all Product
//        /// Use the GET http verb
//        /// Request for v1/ProductCategory
//        /// </summary>
//        /// <returns>List of ProductCategory</returns>
//        [EnableQuery]
//        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductCategory>))]
//        [ProducesResponseType(404)]
//        [HttpGet("v1/ProductCategory")]
//        public IActionResult Get()
//        {
//            var items = _uow.ProductCategory.GetAll().AsQueryable();
//            return Ok(items);
//        }
//        ///// <summary>
//        ///// Gets single Product
//        ///// Use the GET http verb
//        ///// Request for v1/ProductCategory(3)
//        ///// </summary>
//        ///// <param name="id"></param>
//        ///// <returns>Single ProductCategory</returns>
//        //[HttpGet("v1/ProductCategory({id})")]
//        //[HttpGet("v1/ProductCategory/{id}")]
//        //[ProducesResponseType(200, Type = typeof(ProductCategory))]
//        //[ProducesResponseType(404)]
//        //public IActionResult Get(int id)
//        //{
//        //    var entity = _uow.ProductCategory.Get(id);
//        //    if (entity == null)
//        //    {
//        //        return NotFound();
//        //    }
//        //    return Ok(entity);
//        //}

//        /// <summary>
//        /// Creates a ProductCategory.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     POST v1/ProductCategory
//        ///     {
//        ///        "name": "Category 1",
//        ///        
//        ///     }
//        ///
//        /// </remarks>
//        /// <param name="item"></param>
//        /// <returns>A newly created Product</returns>
//        /// <response code="201">Returns the newly created ProductCategory</response>
//        /// <response code="400">If the item is null</response>            
//        [HttpPost("v1/ProductCategory")]
//        [ProducesResponseType(StatusCodes.Status201Created)]
//        [ProducesResponseType(StatusCodes.Status400BadRequest)]
//        public ActionResult<ProductCategory> Create(ProductCategory item)
//        {
//            _uow.ProductCategory.Add(item);
//            _uow.Commit();
//            return Ok(item);
//        }
//        /// <summary>
//        /// Update a specific ProductCategory.
//        /// </summary>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     PUT v1/ProductCategory/1
//        ///     {
//        ///        "id": 1,
//        ///        "name": "Category Update",
//        ///        
//        ///     }
//        ///
//        /// </remarks>
//        /// <param name="id"></param>
//        /// <param name="item"></param>
//        /// <returns></returns>
//        [HttpPut("v1/ProductCategory")]
//        public IActionResult Update(int id, ProductCategory item)
//        {
//            if (item == null || item.Id != id)
//            {
//                return BadRequest();
//            }
//            var productType = _uow.ProductCategory.Get(id);
//            if (productType == null)
//            {
//                return NotFound();
//            }
//            productType.Name = item.Name;
//            _uow.ProductCategory.Update(productType);
//            _uow.Commit();
//            return NoContent();
//        }
//        /// <summary>
//        /// Deletes a specific ProductCategory.
//        /// </summary>
//        /// <param name="id"></param>        
//        [HttpDelete("v1/ProductCategory")]
//        public IActionResult Delete(int id)
//        {
//            var item = _uow.Product.Get(id);
//            if (item == null)
//            {
//                return NotFound();
//            }
//            _uow.Product.Delete(item.Id);
//            _uow.Commit();
//            return NoContent();
//        }
//    }
//}