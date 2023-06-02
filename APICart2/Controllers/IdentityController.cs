using APICart2.Facades;
using APICart2.Models.AuthModels;
using APICart2.Services.Identity.Concretes;
using APICart2.Services.Identity.Interfaces;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APICart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IdentityFaçade _identityFacade;

        public IdentityController(IdentityFaçade identityFacade)
        {
            _identityFacade = identityFacade;
        }



        #region SignUp Endpoint

        [HttpPost("signUp")]
        public async Task<IActionResult> SignUpAsync([FromForm] SignUp model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var orgin = Request.Headers["origin"];
            var result = await _identityFacade.SignUp(model, orgin);

            if (!result.ISAuthenticated)
                return BadRequest(result.Message);

            //store the refresh token in a cookie
            SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        #endregion

        #region Login Endpoint

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromForm] Login model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _identityFacade.Login(model);

            if (!result.ISAuthenticated)
                return BadRequest(result.Message);

            //check if the user has a refresh token or not , to store it in a cookie
            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        #endregion

        #region ConfirmEmail
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {

            return Ok(await _identityFacade.ConfirmEmailAsync(userId, code));
        }
        #endregion

        #region ResendConfirmationEmail
        [HttpPost("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email)
        {
            // Find the user by email
            var user = await _identityFacade.GetUserByEmail(email);

            // Check if the user was found
            if (user == null)
            {
                return NotFound();
            }
            // Generate a new confirmation token and send a new confirmation email
            var origin = $"{Request.Scheme}://{Request.Host}";
            await _identityFacade.SendConfirmationEmailAsync(user, origin);

            return Ok();
        }
        #endregion

        #region SetRefreshTokenInCookies

        private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            //cookieOptionsExpires = DateTime.UtcNow.AddSeconds(cookieOptions.Timeout);

            Response.Cookies.Append("refreshTokenKey", refreshToken, cookieOptions);
        }

        #endregion



    }
}
