using System.ComponentModel.DataAnnotations;

namespace APICart2.Models.AuthModels
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }



        [Required]
        public string Password { get; set; }
    }
}
