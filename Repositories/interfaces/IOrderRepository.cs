using Ecommerce.Schema;

namespace Ecommerce.Repositories.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Product>> GetProductById(List<int> prdId);
    Task<Order?> GetOrderWithItemsById(int orderId);
    Task<Order?> GetOrderWithItemsReadOnlyById(int orderId);
    Task<List<Order>> GetAllOrdersWithItems(int page, int pageSize);
}