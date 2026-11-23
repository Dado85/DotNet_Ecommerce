using Ecommerce.config;
using Ecommerce.Controllers;
using Ecommerce.Dto;
using Ecommerce.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ecommerce.Tests.Controllers;

public class OrderControllerTests
{
    private readonly FakeOrderService _fakeService;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _fakeService = new FakeOrderService();
        _controller = new OrderController(_fakeService);
    }

    [Fact]
    public async Task CreateOrder_ReturnsOkWithCreatedOrder()
    {
        // Arrange
        var request = new OrderReqDto
        {
            OrderItems = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 2 }
            }
        };
        _fakeService.CreateOrderFunc = dto => Task.FromResult(new OrderResponseDto
        {
            Id = 100,
            OrderDate = DateTime.UtcNow,
            TotalAmmount = 250,
            OrderItems = new List<OrderItemResponseDto>
            {
                new() { ProductId = 1, Quantity = 2, UnitPrice = 125, SubTotal = 250, Title = "Item 1" }
            }
        });

        // Act
        var result = await _controller.CreateOrder(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(200, response.StatusCode);

        var data = Assert.IsType<OrderResponseDto>(response.Data);
        Assert.Equal(100, data.Id);
        Assert.Equal(250, data.TotalAmmount);
        Assert.Single(data.OrderItems);
    }

    [Fact]
    public async Task GetAllOrders_ReturnsOkWithOrderList()
    {
        // Arrange
        _fakeService.GetAllOrdersFunc = (page, pageSize) => Task.FromResult(new List<OrderResponseDto>
        {
            new() { Id = 1, OrderDate = DateTime.UtcNow, TotalAmmount = 100 },
            new() { Id = 2, OrderDate = DateTime.UtcNow, TotalAmmount = 200 }
        });

        // Act
        var result = await _controller.GetAllOrders(1, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<List<OrderResponseDto>>(response.Data);
        Assert.Equal(2, data.Count);
    }

    [Fact]
    public async Task GetOrderById_ReturnsOkWithMatchingOrder()
    {
        // Arrange
        int orderId = 15;
        _fakeService.GetOrderByIdFunc = id => Task.FromResult(new OrderResponseDto
        {
            Id = id,
            OrderDate = DateTime.UtcNow,
            TotalAmmount = 500
        });

        // Act
        var result = await _controller.GetOrderById(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<OrderResponseDto>(response.Data);
        Assert.Equal(15, data.Id);
        Assert.Equal(500, data.TotalAmmount);
    }

    [Fact]
    public async Task UpdateOrder_ReturnsOkWithUpdatedOrder()
    {
        // Arrange
        int orderId = 1;
        var request = new OrderReqDto
        {
            OrderItems = new List<OrderItemDto>
            {
                new() { ProductId = 1, Quantity = 5 }
            }
        };
        _fakeService.UpdateOrderFunc = (id, dto) => Task.FromResult(new OrderResponseDto
        {
            Id = id,
            OrderDate = DateTime.UtcNow,
            TotalAmmount = 625
        });

        // Act
        var result = await _controller.UpdateOrder(orderId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<OrderResponseDto>(response.Data);
        Assert.Equal(1, data.Id);
        Assert.Equal(625, data.TotalAmmount);
    }

    [Fact]
    public async Task DeleteOrder_ReturnsOk()
    {
        // Arrange
        int orderId = 9;
        bool deleted = false;
        _fakeService.DeleteOrderFunc = id =>
        {
            deleted = true;
            return Task.CompletedTask;
        };

        // Act
        var result = await _controller.DeleteOrder(orderId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object?>>(okResult.Value);
        Assert.True(response.Success);
        Assert.True(deleted);
    }
}
