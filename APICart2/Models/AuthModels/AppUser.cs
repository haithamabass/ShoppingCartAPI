using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APICart2.Models.AuthModels
{
    public class AppUser: IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? Address { get; set; }


        public List<RefreshToken>? RefreshTokens { get; set; }


        public List<Order>? Orders { get; set; }


    }
}
