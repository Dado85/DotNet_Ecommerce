using Ecommerce.Dto;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Tests.Fakes;

public class FakeProductService : IProductService
{
    public Func<ProductRequestDto, Task<ProductResponseDto>>? CreateProductAsyncFunc { get; set; }
    public Func<Task<List<ProductResponseDto>>>? GetAllProductsAsyncFunc { get; set; }
    public Func<int, Task<ProductResponseDto?>>? GetProductByIdAsyncFunc { get; set; }
    public Func<int, Task<List<ProductResponseDto>>>? GetProductByCategoryIdFunc { get; set; }
    public Func<int, Task<ProductWithCategoryResponseDto>>? GetProductWithCategoryDetailsFunc { get; set; }
    public Func<int, ProductRequestDto, Task<ProductResponseDto>>? UpdateProductAsyncFunc { get; set; }
    public Func<int, Task>? DeleteProductAsyncFunc { get; set; }

    public Task<ProductResponseDto> CreateProductAsync(ProductRequestDto dto) =>
        CreateProductAsyncFunc != null
            ? CreateProductAsyncFunc(dto)
            : Task.FromResult(new ProductResponseDto
            {
                Id = 1,
                Title = dto.Title,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            });

    public Task<List<ProductResponseDto>> GetAllProductsAsync() =>
        GetAllProductsAsyncFunc != null
            ? GetAllProductsAsyncFunc()
            : Task.FromResult(new List<ProductResponseDto>
            {
                new() { Id = 1, Title = "Phone", Price = 999, CategoryId = 1 }
            });

    public Task<ProductResponseDto?> GetProductByIdAsync(int id) =>
        GetProductByIdAsyncFunc != null
            ? GetProductByIdAsyncFunc(id)
            : Task.FromResult<ProductResponseDto?>(new ProductResponseDto
            {
                Id = id,
                Title = "Phone",
                Price = 999,
                CategoryId = 1
            });

    public Task<List<ProductResponseDto>> GetProductByCategoryId(int id) =>
        GetProductByCategoryIdFunc != null
            ? GetProductByCategoryIdFunc(id)
            : Task.FromResult(new List<ProductResponseDto>
            {
                new() { Id = 1, Title = "Phone", Price = 999, CategoryId = id }
            });

    public Task<ProductWithCategoryResponseDto> GetProductWithCategoryDetails(int ProductId) =>
        GetProductWithCategoryDetailsFunc != null
            ? GetProductWithCategoryDetailsFunc(ProductId)
            : Task.FromResult(new ProductWithCategoryResponseDto
            {
                Id = ProductId,
                Title = "Phone",
                Price = 999,
                Category = new PrdWithCategoryResponseDto { Id = 1, Name = "Electronics" }
            });

    public Task<ProductResponseDto> UpdateProductAsync(int id, ProductRequestDto dto) =>
        UpdateProductAsyncFunc != null
            ? UpdateProductAsyncFunc(id, dto)
            : Task.FromResult(new ProductResponseDto
            {
                Id = id,
                Title = dto.Title,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            });

    public Task DeleteProductAsync(int id) =>
        DeleteProductAsyncFunc != null
            ? DeleteProductAsyncFunc(id)
            : Task.CompletedTask;
}
