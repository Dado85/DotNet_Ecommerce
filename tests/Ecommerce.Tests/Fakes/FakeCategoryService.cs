using Ecommerce.Dto;
using Ecommerce.Services;

namespace Ecommerce.Tests.Fakes;

public class FakeCategoryService : ICategoryService
{
    public Func<CreateCategoryReqDto, Task<CategoryResponseDto>>? CreateCategoryFunc { get; set; }
    public Func<Task<List<CategoryResponseDto>>>? GetAllCategoriesFunc { get; set; }
    public Func<int, Task<CategoryResponseDto>>? GetCategoryByIdFunc { get; set; }
    public Func<int, CreateCategoryReqDto, Task<CategoryResponseDto>>? UpdateCategoryFunc { get; set; }
    public Func<int, Task>? DeleteCategoryFunc { get; set; }

    public Task<CategoryResponseDto> CreateCategory(CreateCategoryReqDto dto) =>
        CreateCategoryFunc != null
            ? CreateCategoryFunc(dto)
            : Task.FromResult(new CategoryResponseDto { Id = 1, Name = dto.Name });

    public Task<List<CategoryResponseDto>> GetAllCategories() =>
        GetAllCategoriesFunc != null
            ? GetAllCategoriesFunc()
            : Task.FromResult(new List<CategoryResponseDto>
            {
                new() { Id = 1, Name = "Electronics" },
                new() { Id = 2, Name = "Books" }
            });

    public Task<CategoryResponseDto> GetCategoryById(int id) =>
        GetCategoryByIdFunc != null
            ? GetCategoryByIdFunc(id)
            : Task.FromResult(new CategoryResponseDto { Id = id, Name = "Electronics" });

    public Task<CategoryResponseDto> UpdateCategory(int id, CreateCategoryReqDto dto) =>
        UpdateCategoryFunc != null
            ? UpdateCategoryFunc(id, dto)
            : Task.FromResult(new CategoryResponseDto { Id = id, Name = dto.Name });

    public Task DeleteCategory(int id) =>
        DeleteCategoryFunc != null
            ? DeleteCategoryFunc(id)
            : Task.CompletedTask;
}
