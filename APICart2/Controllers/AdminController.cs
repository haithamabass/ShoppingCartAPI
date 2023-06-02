using APICart2.DTOs;
using APICart2.Facades;
using APICart2.Models.AuthModels;
using APICart2.Services.Identity.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static APICart2.Data.SeedData.SeedDefaultData;

namespace APICart2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly AdminFaçade _admin;

        public AdminController(AdminFaçade admin)
        {
            _admin = admin;
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromQuery] RegisterAdmin model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _admin.RegisterAdmin(model);

            if (!result.ISAuthenticated)
                return BadRequest(result.Message);

            //store the refresh token in a cookie
            SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }


        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("User/{email}")]
        public async Task<ActionResult<UserDTO>> GetUserByRegisteredEmail([FromQuery] string email)
        {
            var user = await _admin.GetUserByEmailDto(email);

            if (user == null)
            {
                return NotFound("No user has found");
            }

            return Ok(user);
        }

        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet("Users")]
        public async Task<ActionResult<UserDTO>> GetAllRegisteredUsers()
        {
            var users = await _admin.GetAllUsers();

            if (users == null || !users.Any())
            {
                return NoContent();
            }

            return Ok(users);
        }



        [Authorize(Roles = "SuperAdmin, Admin")]
        [HttpGet]
        [Route("LastOrders/{email}")]
        public async Task<ActionResult<List<OrderDto>>> GetLastOrders([FromQuery] string email)
        {

            var user = await _admin.GetUserByEmail(email);

            var orders = await _admin.GetLastOrdersForUser(user.Id);

            return Ok(orders);
        }



        #region SetRefreshTokenInCookies

        private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime()
            };

            Response.Cookies.Append("refreshTokenKey", refreshToken, cookieOptions);
        }

        #endregion


    }
}
