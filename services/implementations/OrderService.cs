using Ecommerce.config.errors;
using Ecommerce.Repositories.Implementations;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Schema;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Services.Implementations;

public class OrderService(IUnitOfWork _unitOfWork, OrderMapper mapper) : IOrderService
{
    public async Task<OrderResponseDto> CreateOrder(OrderReqDto dto)
    {
        var (quantitiesByProductId, productsById) = await ValidateAndResolveProductsAsync(dto);

        var order = new Order
        {
            OrderDate = DateTime.UtcNow
        };

        foreach (var (productId, quantity) in quantitiesByProductId)
        {
            var product = productsById[productId];
            var orderItem = mapper.ToEntity(new OrderItemDto
            {
                ProductId = product.Id,
                Quantity = quantity,
            });
            orderItem.UnitPrice = product.Price;
            orderItem.Product = product;
            order.OrderItems.Add(orderItem);
        }

        order.TotalAmmount = order.OrderItems
            .Sum(o => o.UnitPrice * o.Quantity);

        // After calculating order.TotalAmmount:
        NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction
            .AddCustomAttribute("Order.TotalAmount", order.TotalAmmount)
            .AddCustomAttribute("Order.ItemCount", order.OrderItems.Count);

        await _unitOfWork.OrderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return mapper.ToDto(order);
    }

    public async Task<OrderResponseDto> GetOrderById(int orderId)
    {
        if (orderId <= 0) throw new BadRequestException("Invalid order id");

        var order = await _unitOfWork.OrderRepository.GetOrderWithItemsReadOnlyById(orderId);
        if (order == null) throw new NotFoundException($"Order with id {orderId} not found");

        return mapper.ToDto(order);
    }

    public async Task<List<OrderResponseDto>> GetAllOrders(int page = 1, int pageSize = 20)
    {
        if (page <= 0) throw new BadRequestException("Page must be greater than 0");
        if (pageSize <= 0 || pageSize > 100) throw new BadRequestException("PageSize must be between 1 and 100");

        var orders = await _unitOfWork.OrderRepository.GetAllOrdersWithItems(page, pageSize);
        return orders.Select(mapper.ToDto).ToList();
    }
 
    public async Task<OrderResponseDto> UpdateOrder(int orderId, OrderReqDto dto)
    {
        if (orderId <= 0) throw new BadRequestException("Invalid order id");

        var (quantitiesByProductId, productsById) = await ValidateAndResolveProductsAsync(dto);

        var order = await _unitOfWork.OrderRepository.GetOrderWithItemsById(orderId);
        if (order == null) throw new NotFoundException($"Order with id {orderId} not found");

        // Remove items that are no longer in the updated order
        var itemsToRemove = order.OrderItems
            .Where(oi => !quantitiesByProductId.ContainsKey(oi.ProductId))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            order.OrderItems.Remove(item);
        }

        // Update existing items or add new ones
        foreach (var (productId, quantity) in quantitiesByProductId)
        {
            var product = productsById[productId];
            var existingItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
                existingItem.UnitPrice = product.Price;
                existingItem.Product = product;
            }
            else
            {
                var orderItem = mapper.ToEntity(new OrderItemDto
                {
                    ProductId = product.Id,
                    Quantity = quantity,
                });
                orderItem.UnitPrice = product.Price;
                orderItem.Product = product;
                order.OrderItems.Add(orderItem);
            }
        }

        order.TotalAmmount = order.OrderItems
            .Sum(o => o.UnitPrice * o.Quantity);
        order.UpdatedAt = DateTime.UtcNow;

        NewRelic.Api.Agent.NewRelic.GetAgent().CurrentTransaction
            .AddCustomAttribute("Order.Id", order.Id)
            .AddCustomAttribute("Order.TotalAmount", order.TotalAmmount)
            .AddCustomAttribute("Order.ItemCount", order.OrderItems.Count);

        await _unitOfWork.SaveChangesAsync();

        return mapper.ToDto(order);
    }

    public async Task DeleteOrder(int orderId)
    {
        if (orderId <= 0) throw new BadRequestException("Invalid order id");

        var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderId);
        if (order == null) throw new NotFoundException($"Order with id {orderId} not found");

        _unitOfWork.OrderRepository.Delete(order); 
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task<(Dictionary<int, int> QuantitiesByProductId, Dictionary<int, Product> ProductsById)> ValidateAndResolveProductsAsync(OrderReqDto dto)
    {
        if (dto == null) throw new BadRequestException("Empty Request Body");
        if (dto.OrderItems == null || !dto.OrderItems.Any()) throw new BadRequestException("Empty order items");
        if (dto.OrderItems.Any(od => od.Quantity <= 0)) throw new BadRequestException("invalid quantity");

        var quantitiesByProductId = dto.OrderItems
            .GroupBy(od => od.ProductId)
            .ToDictionary(g => g.Key, g => g.Sum(q => q.Quantity));

        var productIds = quantitiesByProductId.Keys.ToList();
        var allProducts = await _unitOfWork.OrderRepository.GetProductById(productIds);

        if (allProducts.Count != productIds.Count)
        {
            throw new BadRequestException("one or more product does not exist");
        }

        return (quantitiesByProductId, allProducts.ToDictionary(p => p.Id));
    }
}