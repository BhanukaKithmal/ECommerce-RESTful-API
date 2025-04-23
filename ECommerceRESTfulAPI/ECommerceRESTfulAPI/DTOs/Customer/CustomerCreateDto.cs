using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerceRESTfulAPI.DTOs.Customer
{
    public class CustomerCreateDto
    {
      
        [Required]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "First name cannot contain numbers or special characters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Last name cannot contain numbers or special characters.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^\+?[0-9]{7,15}$", ErrorMessage = "Phone number must be between 7 to 15 digits and may start with '+'.")]
        public string? Phone { get; set; }
    }
}
    
