namespace ECommerceRESTfulAPI.DTOs
{
    public class OrderCreateDto
    {
        public int CustomerId { get; set; }

        public List<OrderItemCreateDto> OrderItems { get; set; } = new();
    }
}
