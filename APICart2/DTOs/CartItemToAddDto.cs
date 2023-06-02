namespace APICart2.DTOs
{
    public class CartItemToAddDto
    {
        //[NotMapped]
        //public int ItemId { get; set; }

        public int CartId { get; set; }
        //public string UserId { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
