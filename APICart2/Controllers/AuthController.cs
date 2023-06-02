using APICart2.DTOs;
using APICart2.Models.AuthModels;
using APICart2.Services.ExtentionServices;
using APICart2.Services.Identity.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APICart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;



        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        #region RefreshTokenCheck Endpoint

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshTokenCheckAsync()
        {
            var refreshToken = Request.Cookies["refreshTokenKey"];

            var result = await _authService.RefreshTokenCheckAsync(refreshToken);

            if (!result.ISAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }

        #endregion


        #region RevokeTokenAsync

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeTokenAsync(RevokeToken model)
        {
            var refreshToken = model.Token ?? Request.Cookies["refreshTokenKey"];

            //check if there is no token
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Token is required");

            var result = await _authService.RevokeTokenAsync(refreshToken);

            //check if there is a problem with "result"
            //if (!result)
            //    return BadRequest("Token is Invalid");

            return Ok("Done Revoke");
        }

        #endregion



    }
}
