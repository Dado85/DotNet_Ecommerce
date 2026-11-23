using Ecommerce.Dto;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Tests.Fakes;

public class FakeOrderService : IOrderService
{
    public Func<OrderReqDto, Task<OrderResponseDto>>? CreateOrderFunc { get; set; }
    public Func<int, Task<OrderResponseDto>>? GetOrderByIdFunc { get; set; }
    public Func<int, int, Task<List<OrderResponseDto>>>? GetAllOrdersFunc { get; set; }
    public Func<int, OrderReqDto, Task<OrderResponseDto>>? UpdateOrderFunc { get; set; }
    public Func<int, Task>? DeleteOrderFunc { get; set; }

    public Task<OrderResponseDto> CreateOrder(OrderReqDto dto) =>
        CreateOrderFunc != null
            ? CreateOrderFunc(dto)
            : Task.FromResult(new OrderResponseDto
            {
                Id = 1,
                OrderDate = DateTime.UtcNow,
                TotalAmmount = 100
            });

    public Task<OrderResponseDto> GetOrderById(int orderId) =>
        GetOrderByIdFunc != null
            ? GetOrderByIdFunc(orderId)
            : Task.FromResult(new OrderResponseDto
            {
                Id = orderId,
                OrderDate = DateTime.UtcNow,
                TotalAmmount = 100
            });

    public Task<List<OrderResponseDto>> GetAllOrders(int page = 1, int pageSize = 20) =>
        GetAllOrdersFunc != null
            ? GetAllOrdersFunc(page, pageSize)
            : Task.FromResult(new List<OrderResponseDto>
            {
                new() { Id = 1, OrderDate = DateTime.UtcNow, TotalAmmount = 100 }
            });

    public Task<OrderResponseDto> UpdateOrder(int orderId, OrderReqDto dto) =>
        UpdateOrderFunc != null
            ? UpdateOrderFunc(orderId, dto)
            : Task.FromResult(new OrderResponseDto
            {
                Id = orderId,
                OrderDate = DateTime.UtcNow,
                TotalAmmount = 150
            });

    public Task DeleteOrder(int orderId) =>
        DeleteOrderFunc != null
            ? DeleteOrderFunc(orderId)
            : Task.CompletedTask;
}
