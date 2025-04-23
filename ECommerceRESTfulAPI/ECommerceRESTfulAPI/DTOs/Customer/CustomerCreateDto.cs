using System.ComponentModel.DataAnnotations;

namespace ECommerceRESTfulAPI.DTOs.Customer
{
    public class CustomerCreateDto
    {

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public string? Phone { get; set; }
    }
}
