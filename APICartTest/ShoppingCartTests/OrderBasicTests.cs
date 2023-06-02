using APICart2.Controllers;
using APICart2.DTOs;
using APICart2.Facades;
using APICart2.Models;
using APICart2.Services.Content.Concretes;
using APICart2.Services.Content.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APICartTest.ShoppingCartTests
{
    public class OrderBasicTests
    {
        private readonly Mock<OrderFaçade> _productTest;
        private readonly Mock<OrderFaçade> _cartTest;
        private readonly OrderController _controller;


        public OrderBasicTests()
        {
            _productTest = new Mock<OrderFaçade>();
            _cartTest = new Mock<OrderFaçade>();
            _controller = new OrderController(_cartTest.Object);
        }

        #region TestGetItems

        [Fact]
        public async Task GetItems_ReturnsOk_WhenItemsExist()
        {
            // Arrange
            var cartItems = new List<CartItemDto> { new CartItemDto { ItemIdDto = 1, Quantity = 2 } };

            _cartTest.Setup(x => x.GetItems(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((cartItems));

            // Act
            var result = await _controller.GetItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<CartItemDto>>(okResult.Value);
            Assert.Equal(cartItems, returnValue);

        }



        [Fact]
        public async Task GetItems_ReturnsNoContent_WhenNoItemsExist()
        {
            // Arrange
            _cartTest.Setup(x => x.GetItems(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((List<CartItemDto>)null);

            // Act
            var result = await _controller.GetItems();

            // Assert
            Assert.IsType<NoContentResult>(result.Result);

        }


        [Fact]
        public async Task GetItems_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _cartTest.Setup(x => x.GetItems(It.IsAny<ClaimsPrincipal>())).Throws(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetItems();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Test Exception", statusCodeResult.Value);

        }

        #endregion

        #region TestGetItem
        [Fact]
        public async Task GetItem_ReturnsOk_WhenItemExist()
        {
            // Arrange
            var itemId = 1;
            var item = new CartItemDto { ItemIdDto = 1, Quantity = 2 };

            _cartTest.Setup(x => x.GetItemFromCart(itemId, It.IsAny<ClaimsPrincipal>())).ReturnsAsync((item));

            // Act
            var result = await _controller.GetItem(itemId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CartItemDto>(okResult.Value);
            Assert.Equal(item, returnValue);

        }


        [Fact]
        public async Task GetItem_ReturnsNoContent_WhenNoItemExist()
        {
            // Arrange
            var itemId = 1;
            

            _cartTest.Setup(x => x.GetItemFromCart(itemId, It.IsAny<ClaimsPrincipal>())).ReturnsAsync((CartItemDto)null);

            // Act
            var result = await _controller.GetItem(itemId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
           

        }


        [Fact]
        public async Task GetItem_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var itemId = 1;
            

            _cartTest.Setup(x => x.GetItemFromCart(itemId, It.IsAny<ClaimsPrincipal>())).Throws(new Exception("Test Exception"));

            // Act
            var result = await _controller.GetItem(itemId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Test Exception", statusCodeResult.Value);


        }

        #endregion

        #region TestAddItem

        [Fact]
        public async Task AddItem_ReturnCreatedResult_WhenItemIsAdded()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var cartItemToAddDto = new CartItemToAddDto 
            {
              CartId =  1,
              ProductId = 5,
              Quantity = 5

            };

            var cartItem = new CartItem();
            _cartTest.Setup(x => x.AddItemToCart(cartItemToAddDto, mockUser)).ReturnsAsync(cartItem);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.AddItem(cartItemToAddDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var value = Assert.IsType<CartItem>(createdAtActionResult.Value);



        }


        [Fact]
        public async Task AddItem_ReturnBadRequestResult_WhenItemIsNotAdded()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var cartItemToAddDto = new CartItemToAddDto 
            {
              CartId =  1,
              ProductId = 5,
              Quantity = 5

            };

            var cartItem = new CartItem();
            _cartTest.Setup(x => x.AddItemToCart(cartItemToAddDto, mockUser)).ReturnsAsync((CartItem)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.AddItem(cartItemToAddDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);

        }


        [Fact]
        public async Task AddItem_ReturnInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var cartItemToAddDto = new CartItemToAddDto
            {
                CartId = 1,
                ProductId = 5,
                Quantity = 5

            };

            var cartItem = new CartItem();
            _cartTest.Setup(x => x.AddItemToCart(cartItemToAddDto, mockUser)).Throws(new Exception("Test Exception"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.AddItem(cartItemToAddDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Test Exception", statusCodeResult.Value);

        }

        #endregion

        #region  TestUpdateQuantity
        [Fact]
        public async Task UpdateQuantity_ReturnCreatedResult_WhenQuantityIsUpdated()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var cartItemToUpdateDto = new CartItemQtyUpdateDto();

            var cartItem = new CartItem();
            _cartTest.Setup(x => x.UpdateQuantity(cartItemToUpdateDto, mockUser)).ReturnsAsync(cartItem);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.UpdateQuantity(cartItemToUpdateDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetItem), createdResult.ActionName);
            Assert.Equal(cartItem.ItemId, createdResult.RouteValues["id"]);



        }



        [Fact]
        public async Task UpdateQuantity_ReturnInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var cartItemToUpdateDto = new CartItemQtyUpdateDto();

            var cartItem = new CartItem();
            _cartTest.Setup(x => x.UpdateQuantity(cartItemToUpdateDto, mockUser)).Throws(new Exception("Test Exception"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.UpdateQuantity(cartItemToUpdateDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Test Exception", statusCodeResult.Value);



        }

        #endregion

        #region TestDeleteItem
        [Fact]
        public async Task DeleteItem_ReturnCreatedResult_WhenItemIsDeleted()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var itemId = 1;
            var cartItem = new CartItem();
            _cartTest.Setup(x => x.DeleteItem(itemId, mockUser)).ReturnsAsync(cartItem);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.DeleteItem(itemId);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

        }


        [Fact]
        public async Task DeleteItem_ReturnInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            var mockUser = new ClaimsPrincipal();
            var itemId = 1;
            var cartItem = new CartItem();

            _cartTest.Setup(x => x.DeleteItem(itemId, mockUser)).Throws(new Exception("Test Exception"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = mockUser }
            };

            // Act
            var result = await _controller.DeleteItem(itemId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("Test Exception", statusCodeResult.Value);



        }

        #endregion

    }
}
