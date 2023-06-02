using APICart2.DTOs;
using APICart2.Models;
using System.Security.Claims;

namespace APICart2.Services.Content.Interfaces
{
    public interface IOrder
    {
        Task<Order> AddOrder(Order order);
        Task<List<OrderDto>> GetLastOrdersForUser(string userId);
        Task<Order> CreateOrder(string userId, int invoiceId);
        Task<List<OrderDto>> GetOrdersByUserId(string userId);
    }
}
