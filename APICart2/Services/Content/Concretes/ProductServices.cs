using APICart2.Data;
using APICart2.DTOs;
using APICart2.Models;
using APICart2.Services.Content.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICart2.Services.Content.Concretes
{
    public class ProductServices : IProduct
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductServices> _logger;


        public ProductServices(AppDbContext context, ILogger<ProductServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region GetAll
        public async Task<List<ProductDto>> GetAll()
        {
            var product = await _context.Products
                 .Include(r => r.ProductCategory)
                 .Select(x => new ProductDto
                 {
                     ProductId = x.ProductId,
                     BarCode = x.BarCode,
                     Name = x.Name,
                     CategoryName = x.ProductCategory.Name,
                     CategoryId = x.CategoryId,
                     Price = x.Price,
                     Qty = x.Quantity,
                     Description = x.Description,
                     ImageURL = x.ImageURL,
                 })
                 .AsNoTracking()
                 .ToListAsync();

            return product;
        }
        #endregion

        # region GetByCategory
        public async Task<List<ProductDto>> GetByCategory(int code)
        {
            var product = await _context.Products
                 .Include(r => r.ProductCategory)
                .Where(c => c.CategoryId == code)
                 .Select(x => new ProductDto
                 {
                     ProductId = x.ProductId,
                     Name = x.Name,
                     CategoryName = x.ProductCategory.Name,
                     CategoryId = x.CategoryId,
                     Price = x.Price,
                     Qty = x.Quantity,
                     Description = x.Description,
                     ImageURL = x.ImageURL,
                 })
                 .ToListAsync();

            return product;
        }
        #endregion

        #region GetById
        public async Task<Product> GetById(int id)
        {

            try
            {
                var product = await _context.Products
                     .Include(r => r.ProductCategory)
                    .SingleOrDefaultAsync(p => p.ProductId == id);

                if(product == null)
                    throw new ArgumentException("NO PRODUCT HAS FOUND (via GetById Product) "); 



                return product;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "NO PRODUCT HAS FOUND (via GetById Product)");
                throw;
            }
            

        }
        #endregion


        #region GetByBarCode
        public async Task<Product> GetByBarCode(string code)
        {
            var product = await _context.Products
                     .Include(r => r.ProductCategory)
                    .SingleOrDefaultAsync(p => p.BarCode == code);


            return product;
        }
        #endregion


        #region Add 
        public async Task<Product> Add(Product model)
        {
            await _context.Products.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }
        #endregion

        #region Update
        public Product Update(Product model)
        {
            _context.Update(model);
            _context.SaveChanges();
            return model;
        }
        #endregion

        #region Delete
        public Product Delete(Product model)
        {
            _context.Products.Remove(model);
            _context.SaveChangesAsync();

            return model;
        }
        #endregion

        #region ProductIsExist
        public async Task<bool> ProductIsExist(string code)
        {
            return await _context.Products.AnyAsync(p => p.BarCode == code);
        }
        #endregion

        #region DecreaseProductQuantity
        public async Task DecreaseProductQuantity(int productId, int quantityToDecrease)
        {
            //var product = await _context.Products.FindAsync(productId);
            var product = await GetById(productId);

            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            if (product.Quantity < quantityToDecrease)
            {
                throw new ArgumentException("Insufficient product quantity");
            }

            product.Quantity -= quantityToDecrease;
            await _context.SaveChangesAsync();
        }
        #endregion

        #region IncreaseProductQuantity
        public async Task IncreaseProductQuantity(int productId, int quantityToIncrease)
        {
            //var product = await _context.Products.FindAsync(productId);
            var product = await GetById(productId);

            if (product == null)
            {
                throw new ArgumentException("Product not found");
            }

            if (product.Quantity < quantityToIncrease)
            {
                throw new ArgumentException("Insufficient product quantity");
            }

            product.Quantity += quantityToIncrease;
            await _context.SaveChangesAsync();
        }


        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion

    }
}
