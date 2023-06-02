using APICart2.DTOs;
using APICart2.Models;
using APICart2.Services.Content.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProduct _productServcie;
        private readonly ICategory _categoryService;

        public ProductController(IProduct productServcie, ICategory categoryService)
        {
            _productServcie = productServcie;
            _categoryService = categoryService;
        }


        #region GetAllProducts Endpoint
        // GET: api/Products
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("GetAll")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts()
        {
            var products = await _productServcie.GetAll();

            if (products is null|| !products.Any())
                return NoContent();

            return Ok(products);

        }
        #endregion

        #region GetProduct Endpoint
        // GET: api/Products/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            
            var product = await _productServcie.GetById(id);

            if (product == null)
            {
                return NotFound($"no Product with {id} was found");
            }

            return Ok(product);
        }
        #endregion

        #region GetProductByBarCode Endpoint
        // GET: api/Products/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("barcode/{barCode}")]
        public async Task<ActionResult<Product>> GetProductByBarCode(string barCode)
        {
           
            var product = await _productServcie.GetByBarCode(barCode);

            if (product == null)
            {
                return NotFound($"no Product with {barCode} was found");
            }

            return Ok(product);
        }
        #endregion

        #region Create Product Endpoint
        // POST: api/Products
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> AddProduct([FromQuery] ProductToAddDto model)
        {

            var isExist = await _productServcie.ProductIsExist(model.BarCode);

            if (isExist is true)
            {
                var productByBarcode = await _productServcie.GetByBarCode(model.BarCode);
                await _productServcie.IncreaseProductQuantity(productByBarcode.ProductId, (int)model.Qty);
                _productServcie.SaveChanges();
            }
            else
            {
                var product = new Product
                {
                    Name = model.Name,
                    BarCode = model.BarCode,
                    CategoryId = (int)model.CategoryId,
                    Price = (int)model.Price,
                    Quantity = (int)model.Qty,
                    Description = model.Description,
                    ImageURL = model.ImageURL,
                };

                await _productServcie.Add(product);
            }
            _productServcie.SaveChanges();

            return Ok();
        }
        #endregion


        #region Update Product Endpoint
        // PUT: api/Products/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProduct([FromQuery] int id, [FromQuery] ProductToUpdateDto model)
        {
            var product = await _productServcie.GetById(id);

            if (product is null)
            {
                return NotFound($"product with Id:{id} has not found");
            }


            product.Name = model.Name ?? product.Name;
            product.BarCode = model.BarCode ?? product.BarCode;
            product.Description = model.Description ?? product.Description;
            product.ImageURL = model.ImageURL ?? product.ImageURL;

            if (model.CategoryId != null)
            {
                var category = await _categoryService.GetById((int)model.CategoryId);
                if (category != null)
                    product.CategoryId = (int)model.CategoryId;
            }

            if (model.Price != null)
                product.Price = (int)model.Price;
            if (model.Qty != null)
                product.Quantity = (int)model.Qty;

            _productServcie.Update(product);

            return Ok(product);
        }

        #endregion

        #region Delete Product Endpoint
        // DELETE: api/Products/5
        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productServcie.GetById(id);
            if (product is null)
            {
                return NotFound($"product with Id:{id} has not found");
            }

            _productServcie.Delete(product);

            return Ok("Deleted");
        }
        #endregion

    }
}
