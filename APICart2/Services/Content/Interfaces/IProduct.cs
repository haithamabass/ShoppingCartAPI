using APICart2.DTOs;
using APICart2.Models;
using Microsoft.EntityFrameworkCore;

namespace APICart2.Services.Content.Interfaces
{
    public interface IProduct
    {
        Task<List<ProductDto>> GetAll();

        Task<Product> GetById(int id);
        Task<List<ProductDto>> GetByCategory(int code);
        Task<Product> GetByBarCode(string code);

        Task<Product> Add(Product model);

        Product Update(Product model);

        Product Delete(Product model);

        Task<bool> ProductIsExist(string code);

        Task DecreaseProductQuantity(int productId, int quantityToDecrease);

        Task IncreaseProductQuantity(int productId, int quantityToIncrease);

        void SaveChanges();


    }
}
