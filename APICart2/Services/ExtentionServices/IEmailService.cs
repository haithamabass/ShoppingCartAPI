using APICart2.Models.AuthModels;
using APICart2.Models;

namespace APICart2.Services.ExtentionServices
{
    public interface IEmailService
    {
        Task<string> ConfirmEmailAsync(string userId, string code);
        Task SendInvoiceEmail(Invoice invoice, AppUser appUser);
        Task SendConfirmationEmailAsync(AppUser user, string origin);

    }
}
