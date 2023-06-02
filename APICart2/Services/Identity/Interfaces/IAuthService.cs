using APICart2.DTOs;
using APICart2.Models.AuthModels;
using System.Security.Claims;

namespace APICart2.Services.Identity.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> SignUpAsync(SignUp model, string orgin);

        Task<AuthModel> LoginAsync(Login model);


        //for checking if the sent token is valid
        Task<AuthModel> RefreshTokenCheckAsync(string token);

        // for revoking refreshrokens
        Task<bool> RevokeTokenAsync(string token);

        Task<string> GetAuthenticatedUserByToken(ClaimsPrincipal user);

        Task<AppUser> GetUserDetails(string userId);
        Task<AppUser> GetUserByEmail(string email);
        Task<UserDTO> GetUserByEmailDto(string email);

        Task<AuthModel> RegisterAdminAsync(RegisterAdmin model);

        Task<List<UserDTO>> GetAllUsersWithRolesAsync();



    }
}
