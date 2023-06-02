namespace APICart2.DTOs
{
    public class CartItemDto
    {
        public int ItemIdDto { get; set; }
        public int ProductId { get; set; }
        public int? CartId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductImageURL { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
