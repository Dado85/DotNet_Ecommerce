using Ecommerce.Dto;

namespace Ecommerce.Services;

public interface ICategoryService
{
    Task<CategoryResponseDto> CreateCategory(CreateCategoryReqDto dto);
    Task<List<CategoryResponseDto>> GetAllCategories();
    Task<CategoryResponseDto> GetCategoryById(int id);
    Task<CategoryResponseDto> UpdateCategory(int id, CreateCategoryReqDto dto);
    Task DeleteCategory(int id);
}