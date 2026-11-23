using Ecommerce.Dto;
using Ecommerce.Schema;

namespace Ecommerce.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<List<Product>> GetAllWithReviewsAsync();
        Task<Product?> GetByIdWithReviewsAsync(int id);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Product?> GetProductWithCategoryDetails(int ProductId);
        Task<bool> IsProductInOrdersAsync(int productId);
    }
}