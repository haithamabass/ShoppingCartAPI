using APICart2.DTOs;
using APICart2.Models;
using System.Security.Claims;

namespace APICart2.Services.Content.Interfaces
{
    public interface IShoppingCart
    {
        Task<CartItem> GetItem(int id, ClaimsPrincipal user);
        Task<CartItemDto> GetItemDto(int id, ClaimsPrincipal user);
        Task<IEnumerable<CartItemDto>> GetItems(ClaimsPrincipal user);
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAddDto, ClaimsPrincipal user);
        Task<CartItem> UpdateQuantity(CartItemQtyUpdateDto cartItemQtyUpdateDto, ClaimsPrincipal user);
        Task<CartItem> DeleteItem(int itemId, ClaimsPrincipal user);
        Task<Cart> CreateCart(string userId);
        Task<Cart> FindCartByUserId(string userId);
        Task ClearUserCart (List<CartItem>cartItems);
        Task<CartItem> IncreaseExistItemQuantity(CartItem item, int quantity);
        Task EnsureUserHasCart(ClaimsPrincipal user);
        Task<CartItem> UpdateExistItemQuantity(int cartId, int productId, int quantity);


    }
}
