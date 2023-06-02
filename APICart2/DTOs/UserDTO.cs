namespace APICart2.DTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}
