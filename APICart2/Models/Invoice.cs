using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICart2.Models
{
    public class Invoice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvoiceId { get; set; }
        public int CartId { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceItem> CartItems { get; set; }


        // Additional properties for the user's information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Address { get; set; }

    }
}
