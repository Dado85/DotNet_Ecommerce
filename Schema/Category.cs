using Ecommerce.Schema;

public class Category : BaseEntity
{
    // public int Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Product> Products { get; set; }
        = new List<Product>();
}