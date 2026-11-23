using Ecommerce.Schema;

public class OrderReqDto
{
    public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
}
public class OrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
public class OrderResponseDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal? TotalAmmount { get; set; }
    public List<OrderItemResponseDto> OrderItems { get; set; } = new();
}

public class OrderItemResponseDto
{
    public int ProductId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal SubTotal { get; set; }
}