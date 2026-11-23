using Ecommerce.Repositories.Interfaces;

namespace Ecommerce.Repositories;
public interface ICategoryRepository : IRepository<Category>
{
    Task<bool> HasProductsAsync(int categoryId);
}