using Ecommerce.config.errors;
using Ecommerce.Data;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories.Implementations
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<List<Product>> GetAllWithReviewsAsync()
        {
            return await _context.Products
                .Include(p => p.Reviews)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<Product?> GetByIdWithReviewsAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Reviews)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var isCategoryExist = await _context.Categories.FindAsync(categoryId) ?? throw new BadRequestException("The category is not present");
            var prd = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
            return prd;
        }
        public async Task<Product?> GetProductWithCategoryDetails(int ProductId)
        {
            var isPrdExist = await _context.Products.FindAsync(ProductId);
            if (isPrdExist == null)
            {
                throw new BadRequestException("The Requested Product is not Present");

            }
            var prd = await _context.Products
                .Include(ct => ct.Category)
                .Include(p => p.Reviews)
                .FirstOrDefaultAsync(p => p.Id == ProductId);
            return prd;
        }
        public async Task<bool> IsProductInOrdersAsync(int productId)
        {
            return await _context.OrderItems.AnyAsync(oi => oi.ProductId == productId);
        }
    }
}