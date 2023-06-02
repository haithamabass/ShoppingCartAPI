using System.ComponentModel.DataAnnotations;

namespace APICart2.Models.AuthModels
{
    public class SignUp
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(128)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string Password { get; set; }
    }
}
