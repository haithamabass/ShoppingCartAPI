using APICart2.DTOs;
using APICart2.Models;
using APICart2.Models.AuthModels;
using System.Security.Claims;

namespace APICart2.Services.Content.Interfaces
{
    public interface IInvoice
    {
        Task<Invoice> AddInvoice(Invoice invoice);
        Task<InvoiceDto> GetInvoiceForUser(int id, string userId);
        Task<Invoice> CreateInvoice(string userId, Cart cart, AppUser appUser);


    }
}
