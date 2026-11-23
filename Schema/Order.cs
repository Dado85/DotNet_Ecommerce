namespace Ecommerce.Schema;
public class Order : BaseEntity
{
    public DateTime OrderDate{get;set;}
    public decimal? TotalAmmount{get;set;}
    public ICollection<OrderItem> OrderItems{get;set;}=new List<OrderItem>();
}