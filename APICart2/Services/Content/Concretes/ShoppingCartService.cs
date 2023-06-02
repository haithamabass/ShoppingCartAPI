using APICart2.Data;
using APICart2.DTOs;
using APICart2.Models;
using APICart2.Services.Content.Interfaces;
using APICart2.Services.Identity.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APICart2.Services.Content.Concretes
{
    public class ShoppingCartService : IShoppingCart
    {
        private readonly AppDbContext _context;
        private readonly IProduct _producttService;
        private readonly IAuthService _authService;
        private readonly ILogger<ShoppingCartService> _logger;

        public ShoppingCartService(AppDbContext context, ILogger<ShoppingCartService> logger, IAuthService authService, IProduct producttService)
        {
            _context = context;
            _logger = logger;
            _authService = authService;
            _producttService = producttService;
        }

        #region AddItem

        public async Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto, ClaimsPrincipal user)
        {

            await EnsureUserHasCart(user);

            var userId = await _authService.GetAuthenticatedUserByToken(user);

            var cart = await FindCartByUserId(userId);
            var cartId = cart.CartId;

            // Update the item quantity if it already exists in the cart
            var updatedItem = await UpdateExistItemQuantity(cartId, cartItemToAddDto.ProductId, cartItemToAddDto.Quantity);
            if (updatedItem != null)
            {
                return updatedItem;
            }

            // If the item does not exist in the cart, add it
            var item = new CartItem

            {
                CartId = cartId,
                ProductId = cartItemToAddDto.ProductId,
                Quantity = cartItemToAddDto.Quantity
            };

            if (item != null)
            {
                var result = await _context.CartItems.AddAsync(item);
                _context.SaveChanges();
                await _producttService.DecreaseProductQuantity(cartItemToAddDto.ProductId, cartItemToAddDto.Quantity);
                return result.Entity;
            }
            
            await _context.SaveChangesAsync();
            return null;
        }

        #endregion AddItem

        #region GetItem form cart

        public async Task<CartItem> GetItem(int id, ClaimsPrincipal user)
        {
            try
            {
                var userId = await _authService.GetAuthenticatedUserByToken(user);

                // Find CartId associated with UserId
                var cartuser = await FindCartByUserId(userId);

                var cartIdauth = cartuser.CartId;

                var item = await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartIdauth && ci.ItemId == id);

                if (item is null)
                    _logger.LogError("No item is found (via GetItem) ");

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while using GetItem method");

                throw;
            }
        }

        public async Task<CartItemDto> GetItemDto(int id, ClaimsPrincipal user)
        {
            try
            {
                var userId = await _authService.GetAuthenticatedUserByToken(user);

                // Find CartId associated with UserId
                var cartuser = await FindCartByUserId(userId);

                var cartIdauth = cartuser.CartId;

                var item = await (from cart in _context.Carts
                                  join CartItem in _context.CartItems on cart.CartId equals CartItem.CartId
                                  join Product in _context.Products on CartItem.ProductId equals Product.ProductId
                                  where CartItem.ItemId == id && cart.CartId == cartIdauth
                                  select new CartItemDto
                                  {
                                      CartId = CartItem.CartId,
                                      ItemIdDto = CartItem.ItemId,
                                      ProductId = CartItem.ProductId,
                                      ProductName = Product.Name,
                                      ProductDescription = Product.Description,
                                      ProductImageURL = Product.ImageURL,
                                      Price = Product.Price,
                                      Quantity = CartItem.Quantity,
                                      TotalPrice = Product.Price * CartItem.Quantity
                                  }).SingleOrDefaultAsync();

                if (item is null)
                    _logger.LogError("No item is found (via GetItemDto) ");

                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while using GetItemDto method");

                throw;
            }
        }

        #endregion GetItem form cart

        #region GetItems from cart

        public async Task<IEnumerable<CartItemDto>> GetItems(ClaimsPrincipal user)
        {
            try
            {
                var userId = await _authService.GetAuthenticatedUserByToken(user);

                var items = await (from CartItem in _context.CartItems
                                   join Cart in _context.Carts on CartItem.CartId equals Cart.CartId
                                   join Product in _context.Products on CartItem.ProductId equals Product.ProductId
                                   where Cart.UserId == userId
                                   select new CartItemDto
                                   {
                                       CartId = CartItem.CartId,
                                       ItemIdDto = CartItem.ItemId,
                                       ProductId = CartItem.ProductId,
                                       ProductName = Product.Name,
                                       ProductDescription = Product.Description,
                                       ProductImageURL = Product.ImageURL,
                                       Price = Product.Price,
                                       Quantity = CartItem.Quantity,
                                       TotalPrice = Product.Price * CartItem.Quantity
                                   }).AsNoTracking().ToListAsync();

                if (items is null)
                {
                    _logger.LogError("Empty Cart (via GetItems) ");
                    throw new ArgumentException("Empty Cart (via GetItems) ");
                }

                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while using GetItems method");

                throw;
            }
        }

        #endregion GetItems from cart

        #region UpdateQty

        public async Task<CartItem> UpdateQuantity(CartItemQtyUpdateDto cartItemQtyUpdateDto, ClaimsPrincipal user)
        {
            try
            {
                // Code that might throw an exception
                var item = await GetItem(cartItemQtyUpdateDto.CartItemId, user);
                int originalQuantity = item.Quantity;

                item.Quantity = cartItemQtyUpdateDto.Qty;

                if (cartItemQtyUpdateDto.Qty > item.Quantity)
                {
                    await _producttService.DecreaseProductQuantity(item.Product.ProductId, cartItemQtyUpdateDto.Qty - originalQuantity);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    await _producttService.IncreaseProductQuantity(item.Product.ProductId, originalQuantity - cartItemQtyUpdateDto.Qty);
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                return item;
            }
            catch (Exception ex)
            {
                // Log the exception or display it to the user
                _logger.LogError(ex, "An error occurred while using UpdateQty method");
            }
            return null;
        }

        #endregion UpdateQty

        #region DeleteItem

        public async Task<CartItem> DeleteItem(int itemId, ClaimsPrincipal user)
        {
            try
            {
                var item = await GetItem(itemId, user);

                _context.CartItems.Remove(item);
                await _context.SaveChangesAsync();
                await _producttService.IncreaseProductQuantity(item.Product.ProductId, item.Quantity);
                return item;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while using DeleteItem method");

                throw;
            }
        }

        #endregion DeleteItem

        #region ClearUserCart

        public async Task ClearUserCart(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        #endregion ClearUserCart

        #region Create New Cart

        public async Task<Cart> CreateCart(string userId)
        {
            // The user does not have a cart, so create a new one
            var newCart = new Cart
            {
                UserId = userId
            };

            if (newCart is null)
                _logger.LogError("Empty cart object (via CreateCart) ");

            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();

            return newCart;
        }

        #endregion Create New Cart

        #region FindCartByUserId

        public async Task<Cart> FindCartByUserId(string userId)
        {

            var cart = await _context.Carts
                       .Include(c => c.CartItems)
                           .ThenInclude(ci => ci.Product)
                       .FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart is null)
            {
                _logger.LogError("No Cart is found (via FindCartByUserId) ");
                throw new ArgumentException("NO Cart ");

            }

            return cart;
        }

        #endregion FindCartByUserId

        #region CartItemExists

        private async Task<bool> CartItemExists(int productId, int cartId)
        {
            return await _context.CartItems.AnyAsync(c => c.CartId == cartId && c.ProductId == productId);
        }

        #endregion CartItemExists

        #region Get Item by ProductId

        private async Task<CartItem> GetItembyProduct(int cartId, int productId)
        {

            var item = await (from cart in _context.Carts
                              join cartItem in _context.CartItems
                              on cart.CartId equals cartItem.CartId
                              where cartItem.ProductId == productId && cartItem.CartId == cartId
                              select cartItem).SingleOrDefaultAsync();

            return item;
        }

        #endregion Get Item by ProductId

        #region IncreaseExistItemQuantity
        public async Task<CartItem> IncreaseExistItemQuantity(CartItem item, int quantity)
        {
            item.Quantity += quantity;
            _context.SaveChanges();
            return item;
        }
        #endregion

        #region EnsureUserHasCart
        public async Task EnsureUserHasCart(ClaimsPrincipal user)
        {
            var userId = await _authService.GetAuthenticatedUserByToken(user);

            var userCart = await FindCartByUserId(userId);

            if (userCart == null)
            {
                await CreateCart(userId);
            }
        }
        #endregion

        #region UpdateExistItemQuantity
        public async Task<CartItem> UpdateExistItemQuantity(int cartId, int productId, int quantity)
        {
            // Check if the item already exists in the cart
            var exist = await CartItemExists(productId, cartId);

            if (exist is true)
            {
                // If the item exists, update its quantity
                var itemProduct = await GetItembyProduct(cartId, productId);
                var updated = await IncreaseExistItemQuantity(itemProduct, quantity);
                await _producttService.DecreaseProductQuantity(productId, quantity);
                return updated;
            }

            return null;
        }
        #endregion
    }
}