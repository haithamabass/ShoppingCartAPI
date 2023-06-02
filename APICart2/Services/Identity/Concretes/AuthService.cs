using APICart2.Data;
using APICart2.Helpers;
using APICart2.Models;
using APICart2.Models.AuthModels;
using APICart2.Services.Content.Concretes;
using APICart2.Services.ExtentionServices;
using APICart2.Services.Identity.Interfaces;
using Azure.Core;
using FluentEmail.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using APICart2.DTOs;
using static APICart2.Data.SeedData.SeedDefaultData;

namespace APICart2.Services.Identity.Concretes
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _email;
        private readonly JWT _Jwt;
        private readonly ILogger<ShoppingCartService> _logger;
        private readonly AppUserDbContext _context;




        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, ILogger<ShoppingCartService> logger, IEmailService email)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _Jwt = jwt.Value;
            _logger = logger;
            _email = email;
        }

        #region create JWT

        // create JWT
        private async Task<JwtSecurityToken> CreateJwtAsync(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            //generate the symmetricSecurityKey by the s.key
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwt.Key));

            //generate the signingCredentials by symmetricSecurityKey
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            //define the  values that will be used to create JWT
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _Jwt.Issuer,
                audience: _Jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_Jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        #endregion create JWT

        #region Generate RefreshToken

        //Generate RefreshToken
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpireOn = DateTime.UtcNow.AddDays(10),
                CreateOn = DateTime.UtcNow
            };
        }

        #endregion Generate RefreshToken

        #region SignUp Method

        //SignUp
        public async Task<AuthModel> SignUpAsync(SignUp model, string orgin)
        {
            var auth = new AuthModel();

            var userEmail = await _userManager.FindByEmailAsync(model.Email);
            var userName = await _userManager.FindByNameAsync(model.Username);

            //checking the Email and username
            if (userEmail is not null)
                return new AuthModel { Message = "Email is Already used ! " };

            if (userName is not null)
                return new AuthModel { Message = "Username is Already used ! " };

            //fill
            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Address = model.Address,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            //check result
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }

                return new AuthModel { Message = errors };
            }

            //assign role to user by default
            await _userManager.AddToRoleAsync(user, "User");

            await _email.SendConfirmationEmailAsync(user, orgin);


            #region SendVerificationEmail

            //var verificationUri = await SendVerificationEmail(user, orgin);

            //await _emailSender.SendEmailAsync(new EmailRequest()
            //{
            //    ToEmail = user.Email,
            //    Body = $"Please confirm your account by visiting this URL {verificationUri}",
            //    Subject = "Confirm Registration"
            //});

            #endregion SendVerificationEmail

            var jwtSecurityToken = await CreateJwtAsync(user);

            auth.Email = user.Email;
            auth.Roles = new List<string> { "User" };
            auth.ISAuthenticated = true;
            auth.UserName = user.UserName;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            auth.TokenExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime();
            auth.Message = "SignUp Succeeded";

            // create new refresh token
            var newRefreshToken = GenerateRefreshToken();
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return auth;
        }

        #endregion SignUp Method

        #region Login Method

        //login
        public async Task<AuthModel> LoginAsync(Login model)
        {
            var auth = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

           
            var userpass = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !userpass)
            {
                auth.Message = "Email or Password is incorrect";
                return auth;
            }

            if (!user.EmailConfirmed)
            {
                auth.Message = "Email is incorrect or not confirmed";
                return auth;
            }

            var jwtSecurityToken = await CreateJwtAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            auth.Email = user.Email;
            auth.Roles = roles.ToList();
            auth.ISAuthenticated = true;
            auth.UserName = user.UserName;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            auth.TokenExpiresOn = jwtSecurityToken.ValidTo;
            auth.Message = "Login Succeeded ";

            //check if the user has any active refresh token
            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                auth.RefreshToken = activeRefreshToken.Token;
                auth.RefreshTokenExpiration = activeRefreshToken.ExpireOn;
            }
            else
            //in case user has no active refresh token
            {
                var newRefreshToken = GenerateRefreshToken();
                auth.RefreshToken = newRefreshToken.Token;
                auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;

                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
            }

            return auth;
        }

        #endregion Login Method

        #region check Refresh Tokens method

        //check Refresh Tokens
        public async Task<AuthModel> RefreshTokenCheckAsync(string token)
        {
            var auth = new AuthModel();

            //find the user that match the sent refresh token
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                auth.Message = "Invalid Token";
                return auth;
            }

            // check if the refreshtoken is active
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                auth.Message = "Inactive Token";
                return auth;
            }

            //revoke the sent Refresh Tokens
            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtSecurityToken = await CreateJwtAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            auth.Email = user.Email;
            auth.Roles = roles.ToList();
            auth.ISAuthenticated = true;
            auth.UserName = user.UserName;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            auth.TokenExpiresOn = jwtSecurityToken.ValidTo;
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;

            return auth;
        }

        #endregion check Refresh Tokens method

        #region revoke Refresh Tokens method

        //revoke Refresh token
        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            // check if the refreshtoken is active
            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
                return false;

            //revoke the sent Refresh Tokens
            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();

            await _userManager.UpdateAsync(user);

            return true;
        }

        #endregion revoke Refresh Tokens method

        #region GetAuthenticatedUser
        public async Task<string> GetAuthenticatedUserByToken(ClaimsPrincipal user)
        {


            var userIdString = user.FindFirstValue("userId");

            if (string.IsNullOrEmpty(userIdString))
            {
                _logger.LogError("User ID not found in JWT token");
                throw new ArgumentException("User ID not found in JWT token ex");

            }


            return userIdString;


        }

        #endregion


        #region GetUserDetails
        public async Task<AppUser> GetUserDetails(string userId)
        {

            try
            {
                var appUser  =  await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);


                if (appUser is null)
                {
                    _logger.LogError("No user is found (via GetUserDetails) ");

                }

                return appUser;

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred while using GetUserDetails method");

                throw;

            } 

        }
        #endregion

        #region GetUserByEmail
        public async Task<UserDTO> GetUserByEmailDto(string email)
        {
            var user =   await _userManager.FindByEmailAsync(email);
            // Find the user by email
            if(user is null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);


            var userDTO = new UserDTO
            {
                UserId = user.Id,
                Name = user.FirstName + user.LastName,
                Email = user.Email,
                Roles = roles.ToList(),
                Address = user.Address,
                PhoneNumber = user.PhoneNumber
            };

            return userDTO;
        }
        public async Task<AppUser> GetUserByEmail(string email)
        {
            var user =   await _userManager.FindByEmailAsync(email);
            // Find the user by email
            if(user is null)
            {
                return null;
            }

            return user;
        }
        #endregion

        #region RegisterAdmin
        public async Task<AuthModel> RegisterAdminAsync(RegisterAdmin model)
        {
            var auth = new AuthModel();

            
            var userEmail = await _userManager.FindByEmailAsync(model.Email);

            //checking the Email and username
            if (userEmail is not null)
                return new AuthModel { Message = "Email is Already used ! " };

            //fill
            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                EmailConfirmed = true, 
                UserName =model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            //check result
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    // Ignore the error related to the Address property being required
                    if (error.Code == "Required" && error.Description.Contains("Address"))
                        continue;
                    errors += $"{error.Description}, ";
                }

                return new AuthModel { Message = errors };
            }

            //assign admin role to user
            await _userManager.AddToRoleAsync(user, "Admin");


            var jwtSecurityToken = await CreateJwtAsync(user);

            auth.Email = user.Email;
            auth.Roles = new List<string> { "Admin" };
            auth.ISAuthenticated = true;
            auth.UserName = user.UserName;
            auth.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            auth.TokenExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime();
            auth.Message = "Admin Registration Succeeded";

            // create new refresh token
            var newRefreshToken = GenerateRefreshToken();
            auth.RefreshToken = newRefreshToken.Token;
            auth.RefreshTokenExpiration = newRefreshToken.ExpireOn;

            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            return auth;
        }
        #endregion

        #region GetAllUsersWithRolesAsync
        public async Task<List<UserDTO>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var userDTO = new UserDTO
                {

                    UserId = user.Id,
                    Name = user.FirstName + user.LastName ,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber
                };
                userDTOs.Add(userDTO);
            }

            return userDTOs;
        }
        #endregion
    }
}
