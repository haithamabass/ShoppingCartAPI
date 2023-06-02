using APICart2.DTOs;
using APICart2.Models.AuthModels;
using APICart2.Services.Content.Interfaces;
using APICart2.Services.Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace APICart2.Facades
{
    public class AdminFaçade
    {
        private readonly IAuthService _authService;
        private readonly IOrder _order;

        public AdminFaçade(IAuthService authService, IOrder cart)
        {
            _authService = authService;
            _order = cart;
        }

        #region RegisterAdmin
        public async Task<AuthModel> RegisterAdmin(RegisterAdmin model)
        {

            return await _authService.RegisterAdminAsync(model); ;
        }
        #endregion

        #region GetAllUsersWithRolesAsync
        public async Task<List<UserDTO>> GetAllUsers()
        {

            return await _authService.GetAllUsersWithRolesAsync(); ;
        }
        #endregion

        #region GetUserByEmail
        public async Task<UserDTO> GetUserByEmailDto(string email)
        {

            return await _authService.GetUserByEmailDto(email);
        }
        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _authService.GetUserByEmail(email);
        }
        #endregion

        #region GetLastOrdersForUser
        public async Task<List<OrderDto>> GetLastOrdersForUser(string userId)
        {


            return await _order.GetLastOrdersForUser(userId);
        }
        #endregion


    }
}
