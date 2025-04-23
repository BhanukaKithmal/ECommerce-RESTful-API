namespace ECommerceRESTfulAPI.DTOs.Order
{
    public class OrderUpdateDto
    {
        public int CustomerId { get; set; }

        public List<OrderItemCreateDto> OrderItems { get; set; } = new();

    }
}
