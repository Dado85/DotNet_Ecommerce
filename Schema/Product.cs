using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Schema
{
    public class Product : BaseEntity
    {
        // [Key]
        // public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Rating { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<Review> Reviews { get; set; }
            = new List<Review>();
    }
}