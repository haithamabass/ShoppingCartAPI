using APICart2.DTOs;
using APICart2.Models;

namespace APICart2.Extentions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id = productCategory.CategoryId,
                        Name = productCategory.Name,
                    }).ToList();
        }

        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products
                    select new ProductDto
                    {
                        ProductId = product.ProductId,
                        Name = product.Name,
                        Description = product.Description,
                        ImageURL = product.ImageURL,
                        Price = product.Price,
                        Qty = product.Quantity,
                        CategoryId = product.ProductCategory.CategoryId,
                        CategoryName = product.ProductCategory.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product)

        {
            return new ProductDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Quantity,
                CategoryId = product.ProductCategory.CategoryId,
                CategoryName = product.ProductCategory.Name
            };
        }

        public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
                                                            IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.ProductId
                    select new CartItemDto
                    {
                        ItemIdDto = cartItem.ItemId,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        ProductDescription = product.Description,
                        ProductImageURL = product.ImageURL,
                        Price = product.Price,
                        CartId = cartItem.CartId,
                        Quantity = cartItem.Quantity,
                        TotalPrice = product.Price * cartItem.Quantity
                    }).ToList();
        }

        public static CartItemDto ConvertToDto(this CartItem cartItem,
                                                    Product product)
        {
            return new CartItemDto
            {
                ItemIdDto = cartItem.ItemId,
                ProductId = cartItem.ProductId,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                CartId = cartItem.CartId,
                Quantity = cartItem.Quantity,
                TotalPrice = product.Price * cartItem.Quantity
            };
        }

    }
}
