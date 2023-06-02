using APICart2.Data;
using APICart2.DTOs;
using APICart2.Models;
using APICart2.Services.Content.Interfaces;
using APICart2.Services.Identity.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace APICart2.Services.Content.Concretes
{
    public class OrderServices : IOrder
    {

        private readonly AppDbContext _context;
        private readonly AppUserDbContext _contextUser;
        private readonly IAuthService _authService;


        public OrderServices(AppDbContext context, IAuthService authService, AppUserDbContext contextUser)
        {
            _context = context;
            _authService = authService;
            _contextUser = contextUser;
        }

        public async Task<Order> AddOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<OrderDto>> GetLastOrdersForUser(string userId)
        {


            var orders =await GetOrdersByUserId(userId);

            var user = await _authService.GetUserDetails(userId);


            foreach (var order in orders)
            {
                order.UserName = user.FirstName + " " + user.LastName;
                order.UserEmail = user.Email;
                order.UserAddress = user.Address;
            }


            return orders;
        }



        public async Task<List<OrderDto>> GetOrdersByUserId(string userId)
        {
            var orders = await _context.Orders
                                .Where(o => o.UserId == userId)
                                .OrderByDescending(o => o.Date)
                                .Join(_context.Invoices,
                                    o => o.InvoiceId,
                                    i => i.InvoiceId,
                                    (o, i) => new OrderDto {
                                       OrderId =  o.OrderId,
                                       InvoiceId = o.InvoiceId,
                                       Total =  i.Total,
                                       Date = o.Date,
                                       UserId =o.UserId,
                                        CartItems = i.CartItems.Select(ci => new InvoiceItemDto
                                        {
                                            InvoiceItemId = ci.InvoiceItemId,
                                            ProductId = ci.ProductId,
                                            Quantity = ci.Quantity,
                                            ProductName = ci.Product.Name,
                                            //ProductDescription = ci.Product.Description,
                                            ProductImageURL = ci.Product.ImageURL,
                                            Price = ci.Product.Price,
                                            TotalPrice = ci.Quantity * ci.Product.Price
                                        }).ToList()
                                    })
                                .ToListAsync();


            return orders;
        }




        public async Task<Order> CreateOrder(string userId, int invoiceId)
        {
            var order = new Order
            {
                InvoiceId = invoiceId,
                UserId = userId,
                Date = DateTime.Now
            };

            await AddOrder(order);
            return order;
        }



    }
}


