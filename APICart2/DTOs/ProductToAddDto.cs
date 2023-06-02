namespace APICart2.DTOs
{
    public class ProductToAddDto
    {
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }
    }
}
