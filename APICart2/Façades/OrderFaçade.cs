using APICart2.DTOs;
using APICart2.Models;
using APICart2.Models.AuthModels;
using APICart2.Services.Content.Interfaces;
using APICart2.Services.ExtentionServices;
using APICart2.Services.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APICart2.Facades
{
    public class OrderFaçade
    {
        private readonly IShoppingCart _shoppingCart;
        private readonly IInvoice _invoiceService;
        private readonly IOrder _orderServices;
        private readonly IEmailService _email;
        private readonly IAuthService _authService;



        public OrderFaçade(IShoppingCart shoppingCart, IInvoice invoiceService, IOrder orderServices, IEmailService email, IAuthService authService)
        {
            _shoppingCart = shoppingCart;
            _invoiceService = invoiceService;
            _orderServices = orderServices;
            _email = email;
            _authService = authService;
        }

        public OrderFaçade()
        {

        }

        #region Cart Methods
        #region AddItem

        public virtual async Task<CartItem> AddItemToCart(CartItemToAddDto cartItemToAddDto, ClaimsPrincipal user)
        {
           return await _shoppingCart.AddItem(cartItemToAddDto, user);
    
        }

        #endregion AddItem

        #region GetItem form cart

        public virtual async Task<CartItemDto> GetItemFromCart(int id, ClaimsPrincipal user)
        {
            return await _shoppingCart.GetItemDto(id, user);
        }

        #endregion GetItem form cart

        #region GetItems from cart

        public virtual async Task<IEnumerable<CartItemDto>> GetItems(ClaimsPrincipal user)
        {
            return await _shoppingCart.GetItems(user);
        }

        #endregion GetItems from cart

        #region UpdateQuantity

        public virtual async Task<CartItem> UpdateQuantity(CartItemQtyUpdateDto cartItemQtyUpdateDto, ClaimsPrincipal user)
        {
            return await _shoppingCart.UpdateQuantity(cartItemQtyUpdateDto, user);
        }

        #endregion UpdateQty

        #region DeleteItem

        public virtual async Task<CartItem> DeleteItem(int itemId, ClaimsPrincipal user)
        {
            return await _shoppingCart.DeleteItem(itemId, user);
        }

        #endregion DeleteItem
        public virtual async Task ClearUserCart(List<CartItem> cartItems)
        {
            await _shoppingCart.ClearUserCart(cartItems);
        }

        public virtual async Task<Cart> FindCartByUserId(string userId)
        {

            return await _shoppingCart.FindCartByUserId(userId);
        }

        #endregion

        public virtual async Task<Invoice> CreateInvoiceForUser(string userId, Cart cart, AppUser appUser)
        {
            return await _invoiceService.CreateInvoice(userId, cart, appUser);
        }

        public virtual async Task<InvoiceDto> GetInvoiceForUser(int id, string userId)
        {
            return await _invoiceService.GetInvoiceForUser(id,userId);
        }

        public virtual async Task<List<OrderDto>> GetLastOrders(string userId)
        {

            return await _orderServices.GetLastOrdersForUser(userId);
            
        }

        public virtual async Task<Order> CreateOrder(string userId, int invoiceId)
        {
          return await _orderServices.CreateOrder(userId, invoiceId);
        }

        #region SendInvoiceEmail
        public virtual async Task SendInvoiceEmail(Invoice invoice, AppUser appUser)
        {
            await _email.SendInvoiceEmail(invoice, appUser);
        }
        #endregion

        #region Auth Methods
        public virtual async Task<string> GetAuthenticatedUserByToken(ClaimsPrincipal user)
        {


            return await _authService.GetAuthenticatedUserByToken(user);


        }

        public virtual async Task<AppUser> GetUserDetails(string userId)
        {
            return await _authService.GetUserDetails(userId);

        }
        #endregion





    }
}
