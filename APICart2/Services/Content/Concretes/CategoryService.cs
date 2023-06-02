using APICart2.Data;
using APICart2.Models;
using APICart2.Services.Content.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICart2.Services.Content.Concretes
{
    public class CategoryService : ICategory
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        #region GetAll
        public async Task<List<ProductCategory>> GetAll()
        {
            return await _context.ProductCategories
                  .AsNoTracking()
                .ToListAsync();
        }
        #endregion

        #region GetById
        public async Task<ProductCategory> GetById(int id)
        {
            return await _context.ProductCategories
              .FindAsync(id);
        }
        #endregion

        #region AddCategory
        public async Task<ProductCategory> AddCategory(ProductCategory model)
        {
            _context.ProductCategories.Add(model);

            _context.SaveChanges();

            return model;
        }
        #endregion

        #region UpdateCategory
        public ProductCategory UpdateCategory(ProductCategory model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }
        #endregion

        #region DeleteCategory
        public ProductCategory DeleteCategory(ProductCategory model)
        {
            _context.Remove(model);
            _context.SaveChanges();
            return model;
        }
        #endregion

        #region CategoryIsExist
        public async Task<bool> CategoryIsExist(string categoryName)
        {
            return await _context.ProductCategories.AnyAsync(p => p.Name == categoryName);
        }
        #endregion

    }
}
