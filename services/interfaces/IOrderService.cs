namespace Ecommerce.Services.Interfaces;

public interface IOrderService
{
  Task<OrderResponseDto> CreateOrder(OrderReqDto dto);
  Task<OrderResponseDto> GetOrderById(int orderId);
  Task<List<OrderResponseDto>> GetAllOrders(int page = 1, int pageSize = 20);
  Task<OrderResponseDto> UpdateOrder(int orderId, OrderReqDto dto);
  Task DeleteOrder(int orderId);
}