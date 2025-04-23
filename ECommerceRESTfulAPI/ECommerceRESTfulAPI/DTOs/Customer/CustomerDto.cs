namespace EntityFrameworkCore.MySQL.DTOs
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}
