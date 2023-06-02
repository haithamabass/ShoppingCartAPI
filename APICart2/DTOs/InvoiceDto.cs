namespace APICart2.DTOs
{
    public class InvoiceDto
    {
        public DateTime Date { get; set; }
        public int CartId { get; set; }
        public int InvoiceId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceItemDto> CartItems { get; set; }

    }
}
