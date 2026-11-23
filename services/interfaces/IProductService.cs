using Ecommerce.Dto;

namespace Ecommerce.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProductAsync(ProductRequestDto dto);
        Task<List<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto?> GetProductByIdAsync(int id);
        Task<List<ProductResponseDto>> GetProductByCategoryId(int id);
        Task<ProductWithCategoryResponseDto> GetProductWithCategoryDetails(int ProductId);
        Task<ProductResponseDto> UpdateProductAsync(int id, ProductRequestDto dto);
        Task DeleteProductAsync(int id);

    }
}