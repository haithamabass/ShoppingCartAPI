namespace APICart2.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int InvoiceId { get; set; }
        public decimal Total { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserAddress { get; set; }

        public List<InvoiceItemDto> CartItems { get; set; }


    }
}
