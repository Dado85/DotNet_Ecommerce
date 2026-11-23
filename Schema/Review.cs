namespace Ecommerce.Schema;

public class Review : BaseEntity
{
    public string? Message { get; set; }
    public int Rating { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
}