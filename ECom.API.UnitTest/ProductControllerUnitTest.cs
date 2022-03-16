using ECom.API.Controllers.OData.v2;
using ECom.Contracts.Data.Repositories;
using ECom.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Results;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECom.API.UnitTest
{
    public class ProductControllerUnitTest
    {
        private Mock<IUnitOfWork> uowMoq;
        private Product singleProduct;
        private IQueryable<Product> productCollectionQueryable;
        public ProductControllerUnitTest()
        {
            uowMoq = new Mock<IUnitOfWork>();
            singleProduct = new Product { Id =1 };
            productCollectionQueryable = new List<Product> { singleProduct, new Product { Id=2 } }.AsQueryable();
        }

        [Fact]
        public void GetProducts_Returns_The_Correct_Number_Of_Products()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.GetAll()).Returns(productCollectionQueryable);
            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = controller.Get();
            //Assert
            var okResult = actionResult as OkObjectResult;
            var returnProducts = okResult.Value as IEnumerable<Product>;
            Assert.Equal(2, returnProducts.Count());
            uowMoq.Verify();
        }

        [Fact]
        public void GetProduct_Returns_The_Correct_Product()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Get(It.IsAny<int>())).Returns(singleProduct);
            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = controller.GetProduct(It.IsAny<int>());
            //Assert
            var okResult = actionResult as OkObjectResult;
            var returnProduct = okResult.Value as Product;
            Assert.NotNull(returnProduct);
            uowMoq.Verify();
        }

        [Fact]
        public async Task CreateProduct_Returns_The_CreatedODataResult()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Add(It.IsAny<Product>()));
            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = controller.Create(singleProduct);
            //Assert
            var okResult = await actionResult as CreatedODataResult<Product>;
            var returnProduct = okResult.Entity;
            Assert.NotNull(returnProduct);
            uowMoq.Verify();
        }

        [Fact]
        public void Update_Action_Calls_UpdateMethod_Return_BadRequest_When_Id_Does_Not_Match_EntityId()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Update(It.IsAny<Product>()));
            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = controller.Update(0, singleProduct);
            //Assert
            var result = actionResult.Result as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
            uowMoq.Verify();
        }

        [Fact]
        public async Task Update_Action_Return_NotFoundObjectResult_When_Product_DoesNot_Exist()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Update(It.IsAny<Product>()));

            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = await controller.Update(1, singleProduct);
            //Assert
            var result = actionResult as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            uowMoq.Verify();
        }

        [Fact]
        public async Task Update_Action_Calls_UpdateMethod_Of_Repository_Returns_The_UpdatedODataResult()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Get(It.IsAny<int>())).Returns(singleProduct);
            uowMoq.Setup(x => x.Product.Update(It.IsAny<Product>()));
            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = controller.Update(1, singleProduct);
            //Assert
            var result = await actionResult as UpdatedODataResult<Product>;
            var returnProduct = result.Entity;
            Assert.NotNull(returnProduct);
            uowMoq.Verify();
        }
        [Fact]
        public async Task Patch_Action_Return_NotFoundObjectResult_When_Product_DoesNot_Exist()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Update(It.IsAny<Product>()));
            var deltaProduct = new Mock<Delta<Product>>().Object;
            var controller = new ProductController(uowMoq.Object);
            //Act
            var actionResult = await controller.Patch(1, deltaProduct);
            //Assert
            var result = actionResult as NotFoundObjectResult;
            Assert.Equal(404, result.StatusCode);
            uowMoq.Verify();
        }
        [Fact]
        public async Task Patch_Action_Calls_UpdateMethod_Of_Repository_And_Returns_The_UpdatedODataResult()
        {
            //Arrange
            uowMoq.Setup(x => x.Product.Get(It.IsAny<int>())).Returns(singleProduct);
            uowMoq.Setup(x => x.Product.Update(It.IsAny<Product>()));
            var controller = new ProductController(uowMoq.Object);
            var deltaProduct = new Mock<Delta<Product>>().Object;
            //Act
            var actionResult = controller.Patch(1, deltaProduct);
            //Assert
            var okResult = await actionResult as UpdatedODataResult<Product>;
            var returnProduct = okResult.Entity;
            Assert.NotNull(returnProduct);
            uowMoq.Verify();
        }
    }
}
