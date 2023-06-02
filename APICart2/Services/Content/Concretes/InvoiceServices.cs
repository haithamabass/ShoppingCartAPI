using APICart2.Data;
using APICart2.DTOs;
using APICart2.Models;
using APICart2.Models.AuthModels;
using APICart2.Services.Content.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APICart2.Services.Content.Concretes
{
    public class InvoiceServices : IInvoice
    {

        private readonly AppDbContext _context;

        public InvoiceServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> AddInvoice(Invoice invoice)
        {
             _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<InvoiceDto> GetInvoiceForUser(int id, string userId)
        {
            var invoiceDto = await _context.Invoices
                .Where(i => i.UserId == userId && i.InvoiceId == id)
                .Select(i => new InvoiceDto
                {
                    InvoiceId = i.InvoiceId,
                    CartId = i.CartId,
                    UserId = i.UserId,
                    Date = i.Date,
                    FullName = i.FirstName +   i.LastName,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber,
                    Address = i.Address,
                    Total = i.Total,
                    CartItems = i.CartItems.Select(ci => new InvoiceItemDto
                    {
                        InvoiceItemId = ci.InvoiceItemId,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        ProductCategory = ci.Product.ProductCategory.Name,
                        ProductImageURL = ci.Product.ImageURL,
                        Quantity = ci.Quantity,
                        Price = ci.Product.Price,
                        TotalPrice = ci.Quantity * ci.Product.Price
                    }).ToList(),
                })
                .FirstOrDefaultAsync();

            return invoiceDto;
        }


        public async Task<Invoice> CreateInvoice(string userId, Cart cart, AppUser appUser)
        {
            var invoice = new Invoice
            {
                CartId = cart.CartId,
                UserId = userId,
                Date = DateTime.Now,
                Total = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                CartItems = cart.CartItems.Select(ci => new InvoiceItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                }).ToList(),

                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                PhoneNumber = appUser.PhoneNumber,
                Address = appUser.Address
            };

            await AddInvoice(invoice);
            return invoice;
        }





    }
}
