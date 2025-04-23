using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.MySQL.Models { 

    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}


