using APICart2.Controllers;
using APICart2.DTOs;
using APICart2.Models;
using APICart2.Services.Content.Concretes;
using APICart2.Services.Content.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace APICartTest.ProductTests
{
    public class ProductBasicTests
    {
        private readonly Mock<IProduct> _productTest;
        private readonly Mock<ICategory> _categoryTest;
        private readonly ProductController _controller;
        private readonly ITestOutputHelper _output;

        public ProductBasicTests(ITestOutputHelper output)
        {
            _productTest = new Mock<IProduct>();
            _categoryTest = new Mock<ICategory>();
            _controller = new ProductController(_productTest.Object, _categoryTest.Object);
            _output = output;
        }


        private List<ProductDto> GetProductsData()
        {
            List<ProductDto> ProductsData = new()
            {
            new ProductDto
            {
                    ProductId = 1,
                    Name = "Test1",
                    Description = "Description Test1",
                    ImageURL = "/Images/Beauty/Beauty3.pngTest1",
                    BarCode = "PB-1",
                    Price = 1,
                    Qty = 1,
                    CategoryId = 1
            },
            new ProductDto
            {
                 ProductId = 2,
                    Name = "Test2",
                    Description = "Description Test2",
                    ImageURL = "/Images/Beauty/Beauty3.pngTest2",
                    BarCode = "PB-2",
                    Price = 2,
                    Qty = 2,
                    CategoryId =2
            },
            new ProductDto
            {
                    ProductId = 3,
                    Name = "Test3",
                    Description = "Description Test3",
                    ImageURL = "/Images/Beauty/Beauty3.pngTest3",
                    BarCode = "PB-3",
                    Price = 3,
                    Qty = 3,
                    CategoryId = 3
            }

            };

            return ProductsData;
        }
        private List<Product> GetProductsDataOriginal()
        {
            List<Product> ProductsData = new()
            {
            new Product
            {
                    ProductId = 1,
                    Name = "Test1",
                    Description = "Description Test1",
                    ImageURL = "/Images/Beauty/Beauty3.pngTest1",
                    BarCode = "PB-1",
                    Price = 1,
                    Quantity = 1,
                    CategoryId = 1
            },
            new Product
            {
                 ProductId = 2,
                    Name = "Test2",
                    Description = "Description Test2",
                    ImageURL = "/Images/Beauty/Beauty3.pngTest2",
                    BarCode = "PB-2",
                    Price = 2,
                    Quantity = 2,
                    CategoryId =2
            },
            new Product
            {
                    ProductId = 3,
                    Name = "Test3",
                    Description = "Description Test3",
                    ImageURL = "/Images/Beauty/Beauty3.pngTest3",
                    BarCode = "PB-3",
                    Price = 3,
                    Quantity = 3,
                    CategoryId = 3
            }

            };

            return ProductsData;
        }


        #region TestGetProducts

        [Fact]
        public async Task GetProducts_ReturnsOkResultWithListOfProducts()
        {
            //arrange
            var productlist = GetProductsData();
            _productTest.Setup(x => x.GetAll())
                .ReturnsAsync(productlist);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(productlist.Count, returnValue.Count);
        }



        [Fact]
        public async Task GetProducts_ReturnsNoContentResultWhenNoProducts()
        {
            //arrange

            _productTest.Setup(x => x.GetAll()).ReturnsAsync(new List<ProductDto>());

            // Act
            var result = await _controller.GetProducts();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);

        }

        [Fact]
        public async Task GetProducts_ThrowsException()
        {
            // Arrange
            _productTest.Setup(x => x.GetAll()).Throws(new Exception("Test Exception"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.GetProducts());
        }
        #endregion

        #region TestGetProduct

        [Fact]
        public async Task GetProduct_ReturnsCorrectProduct()
        {


            // Arrange
            var products = GetProductsDataOriginal();

            _productTest.Setup(service => service.GetById(It.IsAny<int>()))
        .ReturnsAsync((int id) => products.SingleOrDefault(p => p.ProductId == id));

            // Act
            var result = await _controller.GetProduct(2);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(2, product.ProductId);
        }



        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExistt()
        {


            // Arrange
            var products = GetProductsDataOriginal();

            _productTest.Setup(service => service.GetById(It.IsAny<int>()))
        .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProduct(2);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

        }





        #endregion

        #region TestGetProductByBarCode

        [Fact]
        public async Task GetProductByBarCode_ReturnsCorrectProduct()
        {
            // Arrange

            var products = GetProductsDataOriginal();

            _productTest.Setup(service => service.GetByBarCode(It.IsAny<string>()))
        .ReturnsAsync((string code) => products.SingleOrDefault(p => p.BarCode == code));

            // Act
            var result = await _controller.GetProductByBarCode("PB-2");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var product = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("PB-2", product.BarCode);
        }



        [Fact]
        public async Task GetProductByBarCode_ReturnsNotFound_WhenProductDoesNotExist()
        {


            // Arrange
            var productTestBarCode = "PB-5";
            ;

            var products = GetProductsDataOriginal();

            _productTest.Setup(service => service.GetByBarCode(productTestBarCode))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductByBarCode(productTestBarCode);


            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);

        }





        #endregion

        #region TestAddProduct

        [Fact]
        public async Task AddProduct_ReturnsOkResult()
        {
            // Arrange
            _productTest.Setup(x => x.ProductIsExist(It.IsAny<string>())).ReturnsAsync(false);

            var model = new ProductToAddDto
            {
                Name = "Test",
                BarCode = "PB-1",
                CategoryId = 1,
                Price = 1,
                Qty = 1,
                Description = "Description Test",
                ImageURL = "/Images/Beauty/Beauty3.pngTest"
            };

            // Act
            var result = await _controller.AddProduct(model);


            // Assert
            Assert.IsType<OkResult>(result);

            _productTest.Verify(s => s.Add(It.Is<Product>(p =>
                  p.Name == model.Name &&
                  p.BarCode == model.BarCode &&
                  p.CategoryId == model.CategoryId &&
                  p.Price == model.Price &&
                  p.Quantity == model.Qty &&
                  p.Description == model.Description &&
                  p.ImageURL == model.ImageURL
            )));
        }

        [Fact]
        public async Task AddProduct_IncreasesQuantity_WhenProductExists()
        {
            // Arrange
            _productTest.Setup(x => x.ProductIsExist(It.IsAny<string>())).ReturnsAsync(true);

            var existingProduct = new Product { ProductId = 1, BarCode = "PB-1", Quantity = 1 };

            _productTest.Setup(b => b.GetByBarCode(It.IsAny<string>())).ReturnsAsync(existingProduct);

            var model = new ProductToAddDto
            {
                Name = "Test",
                BarCode = "PB-1",
                CategoryId = 1,
                Price = 1,
                Qty = 1,
                Description = "Description Test",
                ImageURL = "/Images/Beauty/Beauty3.pngTest"
            };

            // Act
            var result = await _controller.AddProduct(model);


            // Assert
            Assert.IsType<OkResult>(result);

            _productTest.Verify(s => s.IncreaseProductQuantity(existingProduct.ProductId, model.Qty));
        }
        #endregion

        #region TestUpdateProduct

        [Fact]
        public async Task UpdateProduct_UpdatesProductAndReturnsOk()
        {
            // Arrange
            var productTestId = 2;

            var model = new ProductToUpdateDto
            {
                Name = "Test",
                BarCode = "PB-1",
                CategoryId = 1,
                Price = 5,
                Qty = 8,
                Description = "Description Test11",
                ImageURL = "/Images/Beauty/Beauty783.pngTest"
            };

            _productTest.Setup(x => x.GetById(productTestId))
                .ReturnsAsync(new Product { ProductId = productTestId });

            _categoryTest.Setup(x => x.GetById((int)model.CategoryId))
                .ReturnsAsync(new ProductCategory { CategoryId = (int)model.CategoryId });

            var existingProduct = new Product { ProductId = 1, BarCode = "PB-1", Quantity = 1 };

            _productTest.Setup(b => b.GetByBarCode(It.IsAny<string>())).ReturnsAsync(existingProduct);


            // Act
            var result = await _controller.UpdateProduct(productTestId,model);


            // Assert

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);
            var updatedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.NotNull(updatedProduct);

            Assert.Equal(model.Name, updatedProduct.Name);
            Assert.Equal(model.BarCode, updatedProduct.BarCode);
            Assert.Equal(model.CategoryId, updatedProduct.CategoryId);
            Assert.Equal(model.Description, updatedProduct.Description);
            Assert.Equal(model.Price, updatedProduct.Price);
            Assert.Equal(model.Qty, updatedProduct.Quantity);
            Assert.Equal(model.ImageURL, updatedProduct.ImageURL);
        }


        [Fact]
        public async Task UpdateProduct_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var productTestId = 2;

            var model = new ProductToUpdateDto();
           

            _productTest.Setup(x => x.GetById(productTestId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct(productTestId, model);


            // Assert

           Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

        #region TestDeleteProduct

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductIsDeleted()
        {
            // Arrange
            var productTestId = 2;
            var model = new Product
            {
                Name = "Test",
                BarCode = "PB-1",
                CategoryId = 1,
                Price = 5,
                Quantity = 8,
                Description = "Description Test11",
                ImageURL = "/Images/Beauty/Beauty783.pngTest"
            };

            _productTest.Setup(x => x.GetById(productTestId)).ReturnsAsync(model);

            // Act
            var result = await _controller.DeleteProduct(productTestId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task DeleteProduct__ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var productTestId = 2;


            _productTest.Setup(x => x.GetById(productTestId))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct(productTestId);


            // Assert

            Assert.IsType<NotFoundObjectResult>(result);
        }

        #endregion

    }
}
