using Ecommerce.Data;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories.Implementations;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(AppDbContext context) : base(context)
    {

    }
    public async Task<List<Product>> GetProductById(List<int> prdId)
    {
        return await _context.Products.Where(p => prdId.Contains(p.Id)).ToListAsync();
    }
    public async Task<Order?> GetOrderWithItemsById(int orderId)
    {
        return await OrdersWithItemsQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<Order?> GetOrderWithItemsReadOnlyById(int orderId)
    {
        return await OrdersWithItemsQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }

    public async Task<List<Order>> GetAllOrdersWithItems(int page, int pageSize)
    {
        return await OrdersWithItemsQuery()
            .AsNoTracking()
            .OrderByDescending(o => o.OrderDate)
            .ThenByDescending(o => o.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    private IQueryable<Order> OrdersWithItemsQuery()
    {
        return _context.Ordes
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product);
    }
}
