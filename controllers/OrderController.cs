using Asp.Versioning;
using Ecommerce.config;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrderController(IOrderService _service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder(OrderReqDto dto)
    {
        var data = await _service.CreateOrder(dto);
        return Ok(ApiResponse<Object>.Succeed(data, 200, "Order Cretaed Successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var data = await _service.GetAllOrders(page, pageSize);
        return Ok(ApiResponse<object>.Succeed(data, 200, "Orders Fetched Successfully"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var data = await _service.GetOrderById(id);
        return Ok(ApiResponse<object>.Succeed(data, 200, "Order Fetched Successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderReqDto dto)
    {
        var data = await _service.UpdateOrder(id, dto);
        return Ok(ApiResponse<Object>.Succeed(data, 200, "Order Updated Successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _service.DeleteOrder(id);
        return Ok(ApiResponse<object?>.Succeed(null, 200, "Order Deleted Successfully"));
    }
}