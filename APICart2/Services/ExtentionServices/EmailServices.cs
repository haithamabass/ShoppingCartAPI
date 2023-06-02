using APICart2.Data;
using APICart2.Models;
using APICart2.Models.AuthModels;
using APICart2.Services.Content.Concretes;
using APICart2.Services.Identity.Interfaces;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Web;

namespace APICart2.Services.ExtentionServices
{
    public class EmailServices : IEmailService
    {
        private readonly IFluentEmail _email;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<ShoppingCartService> _logger;



        public EmailServices(IFluentEmail email, ILogger<ShoppingCartService> logger, UserManager<AppUser> userManager)
        {
            _email = email;
            _logger = logger;
            _userManager = userManager;
        }

        #region SendInvoiceEmail
        public async Task SendInvoiceEmail(Invoice invoice, AppUser appUser )
        {
            
            if (appUser is null)
            {
                throw new ArgumentException("Empty user (via SendInvoiceEmail) ");
            }
            await _email
                .To(appUser.Email)
                .Subject("Your Invoice")
                .Body("Here is your invoice.")
                .UsingTemplateFromFile("D:\\Visual Studio C# code Files\\APICart2\\APICart2\\Helpers\\EmailSettings\\Views\\EmailTemplate.cshtml", invoice)
                .SendAsync();
        }
        #endregion

        #region SendConfirmationEmailAsync
        public async Task SendConfirmationEmailAsync(AppUser user, string origin)
        {
            // Generate a confirmation token for the user
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            _logger.LogInformation($"Token: {token}");


            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            _logger.LogInformation($"EncodedToken: {encodedToken}");

            // Generate a confirmation link for the user
            var confirmationLink = $"{origin}/api/Identity/confirm-email?userId={user.Id}&code={encodedToken}";

            // Create the email message
            var email = _email
                .To(user.Email)
                .Subject("Confirm your email")
                .UsingTemplateFromFile("D:\\Visual Studio C# code Files\\APICart2\\APICart2\\Helpers\\EmailSettings\\Views\\ConfirmationEmail.cshtml", new { UserName = user.UserName, ConfirmationLink = confirmationLink });

            // Send the email
            await email.SendAsync();
        }
        #endregion

        #region ConfirmEmailAsync
        public async Task<string> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var isValid = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation", code);
            if (!isValid)
            {
                throw new Exception("The token is invalid or has expired.");
            }


            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return $"Account Confirmed for {user.Email}. ";
            }
            else
            {
                throw new Exception($"An error occured while confirming {user.Email}.");
            }
        }

        #endregion

    }
}
