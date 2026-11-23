using Riok.Mapperly.Abstractions;
using Ecommerce.Schema;

[Mapper]
public partial class OrderMapper
{
    // Request DTO -> Order Entity
    public partial Order ToEntity(OrderReqDto dto);

    // Request Item DTO -> OrderItem Entity
    public partial OrderItem ToEntity(OrderItemDto dto);

    // Order Entity -> Order Response DTO
    public partial OrderResponseDto ToDto(Order order);

    // OrderItem Entity -> OrderItem Response DTO
    [MapProperty("Product.Title", nameof(OrderItemResponseDto.Title))]
    public partial OrderItemResponseDto ToDto(OrderItem orderItem);

    // Wrapper method
    [UserMapping(Default = true)]
    public OrderItemResponseDto _ToDto(OrderItem orderItem)
    {
        // Step 1: Execute generated mapping
        var dto = ToDto(orderItem);

        // Step 2: Custom calculation
        dto.SubTotal = orderItem.UnitPrice * orderItem.Quantity;

        // Step 3: Return completed DTO
        return dto;
    }
}