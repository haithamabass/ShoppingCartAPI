using APICart2.Models.AuthModels;
using System.ComponentModel.DataAnnotations;

namespace APICart2.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public DateTime Date { get; set; }

        // Foreign key property for the relationship with the AppUser class
        public string UserId { get; set; }
        public AppUser User { get; set; } 
    }
}
