namespace APICart2.DTOs
{
    public class InvoiceItemDto
    {
        public int InvoiceItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
    }
}