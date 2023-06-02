using APICart2.Models;

namespace APICart2.Services.Content.Interfaces
{
    public interface ICategory
    {
        Task<List<ProductCategory>> GetAll();


        Task<ProductCategory> GetById(int id);


        Task<ProductCategory> AddCategory(ProductCategory model);



        ProductCategory UpdateCategory(ProductCategory model);



        ProductCategory DeleteCategory(ProductCategory model);



        Task<bool> CategoryIsExist(string categoryName);
    }
}
