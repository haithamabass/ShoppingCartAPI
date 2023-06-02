using APICart2.Data;
using APICart2.DTOs;
using APICart2.Extentions;
using APICart2.Facades;
using APICart2.Models;
using APICart2.Services.Content.Concretes;
using APICart2.Services.Content.Interfaces;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APICart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderFaçade _orderFacade;

        public OrderController(OrderFaçade orderFacade)
        {
            _orderFacade = orderFacade;
        }



        #region GetItems from cart
        [Authorize]
        [HttpGet]
        [Route("GetItems")]
        public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems()
        {
            try
            {

                var cartItems = await this._orderFacade.GetItems(User);

                if (cartItems == null)
                {
                    return NoContent();
                }


                return Ok(cartItems);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
        #endregion


        #region Get one Item from cart
        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartItemDto>> GetItem(int id)
        {
           
            try
            {
                var cartItem = await this._orderFacade.GetItemFromCart(id,User);
                if (cartItem == null)
                {
                    return NotFound("No Item Found");
                }

                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion


        #region Additems in cart
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CartItemDto>> AddItem(CartItemToAddDto cartItemToAddDto)
        {
           
            try
            {

                var newCartItem = await _orderFacade.AddItemToCart(cartItemToAddDto, User);

                    if (newCartItem == null)
                    {
                        return BadRequest("Empty Input ..... !");
                    }

                    return CreatedAtAction(nameof(GetItem), new { id = newCartItem.ItemId }, newCartItem);
                //return Ok($"Created {newCartItem}");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        #endregion

        #region Update ItemsQty from cart
        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<CartItem>> UpdateQuantity([FromForm]CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await this._orderFacade.UpdateQuantity(cartItemQtyUpdateDto, User);
               
                return CreatedAtAction(nameof(GetItem), new { id = cartItem.ItemId }, cartItem);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }
        #endregion



        #region DeleteItems
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartItemDto>> DeleteItem([FromForm] int itemId)
        {
            try
            {

                return Ok(await _orderFacade.DeleteItem(itemId, User));

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        #endregion


        [Authorize]
        [HttpPost]
        [Route("CreateInvoice")]
        public async Task<ActionResult<InvoiceDto>> CreateInvoice()
        {
            var userId = await _orderFacade.GetAuthenticatedUserByToken(User);

            // Find the user's cart
            var cart = await _orderFacade.FindCartByUserId(userId);

            if (cart.CartItems == null || !cart.CartItems.Any())
            {
                // Return an error message indicating that the cart is empty
                return BadRequest("The cart is empty. Cannot create an invoice.");
            }

            // Retrieve the user's information from the database
            var appUser = await _orderFacade.GetUserDetails(userId);

            var invoice = await _orderFacade.CreateInvoiceForUser(userId, cart, appUser);
            await _orderFacade.SendInvoiceEmail(invoice, appUser);

            await _orderFacade.CreateOrder(userId, invoice.InvoiceId);

            // Clear the user's cart
            await _orderFacade.ClearUserCart(cart.CartItems);

            return await GetInvoice(invoice.InvoiceId);
        }

        [Authorize]
        [HttpGet]
        [Route("GetInvoice")]
        public async Task<ActionResult<InvoiceDto>> GetInvoice([FromForm] int invoiceId)
        {
            var userId = await _orderFacade.GetAuthenticatedUserByToken(User);
            var invoiceDto = await _orderFacade.GetInvoiceForUser(invoiceId, userId);

            return Ok(invoiceDto);
        }

        [Authorize]
        [HttpGet]
        [Route("LastOrders")]
        public async Task<ActionResult<List<OrderDto>>> GetLastOrders()
        {
            var userId = await _orderFacade.GetAuthenticatedUserByToken(User);

            var orders = await _orderFacade.GetLastOrders(userId);

            return Ok(orders);
        }






    }
}
