using System.ComponentModel.DataAnnotations;

namespace APICart2.Models
{
    public class InvoiceItem
    {
        [Key]
        public int InvoiceItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }

    }
}
