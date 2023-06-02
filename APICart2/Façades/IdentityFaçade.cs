using APICart2.Models.AuthModels;
using APICart2.Services.ExtentionServices;
using APICart2.Services.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace APICart2.Facades
{
    public class IdentityFaçade
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _email;


        public IdentityFaçade(IEmailService email, IAuthService authService)
        {
            _email = email;
            _authService = authService;
        }

        #region SignUp Method
        public async Task<AuthModel> SignUp(SignUp model, string orgin)
        {


            return await _authService.SignUpAsync(model, orgin);
        }

        #endregion SignUp Method

        #region Login Method

        //login
        public async Task<AuthModel> Login(Login model)
        {
            return await _authService.LoginAsync(model);
        }

        #endregion Login Method

        #region GetUserByEmail
        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _authService.GetUserByEmail(email) ;
        }
        #endregion

        #region ConfirmEmailAsync
        public async Task<string> ConfirmEmailAsync(string userId, string code)
        {
           
            return await _email.ConfirmEmailAsync(userId, code);
        }

        #endregion

        #region SendConfirmationEmailAsync
        public async Task SendConfirmationEmailAsync(AppUser user, string origin)
        {

            await _email.SendConfirmationEmailAsync(user, origin);
        }
        #endregion

    }
}
