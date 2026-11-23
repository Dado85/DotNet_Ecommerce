using Ecommerce.config.errors;
using Ecommerce.Dto;
using Ecommerce.mapper;
using Ecommerce.Repositories;
using Ecommerce.Repositories.Interfaces;

namespace Ecommerce.Services.Implementations;

public class CategoryService : ICategoryService
{
    IUnitOfWork UnitofWork;
    CategoryMapper _mapper;
    //CategoryRepository categoryRepository;
    public CategoryService(IUnitOfWork unitOfWork, CategoryMapper mapper)
    {
        UnitofWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<CategoryResponseDto> CreateCategory(CreateCategoryReqDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            throw new BadRequestException("Category name is required");

        var category = _mapper.ToEntity(dto);
        category.Name = category.Name.Trim();

        await UnitofWork.CategoryRepository.AddAsync(category);
        await UnitofWork.SaveChangesAsync();
        return _mapper.ToDto(category);
    }

    public async Task<List<CategoryResponseDto>> GetAllCategories()
    {
        var categories = await UnitofWork.CategoryRepository.GetAllAsync();
        return categories.Select(_mapper.ToDto).ToList();
    }

    public async Task<CategoryResponseDto> GetCategoryById(int id)
    {
        if (id <= 0) throw new BadRequestException("Invalid category id");

        var category = await UnitofWork.CategoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with id {id} not found");

        return _mapper.ToDto(category);
    }

    public async Task<CategoryResponseDto> UpdateCategory(int id, CreateCategoryReqDto dto)
    {
        if (id <= 0) throw new BadRequestException("Invalid category id");
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
            throw new BadRequestException("Category name is required");

        var category = await UnitofWork.CategoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with id {id} not found");

        category.Name = dto.Name.Trim();
        category.UpdatedAt = DateTime.UtcNow;

        await UnitofWork.SaveChangesAsync();
        return _mapper.ToDto(category);
    }

    public async Task DeleteCategory(int id)
    {
        if (id <= 0) throw new BadRequestException("Invalid category id");

        var category = await UnitofWork.CategoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Category with id {id} not found");

        if (await UnitofWork.CategoryRepository.HasProductsAsync(id))
            throw new ConflictException("Cannot delete category with associated products");

        UnitofWork.CategoryRepository.Delete(category);
        await UnitofWork.SaveChangesAsync();
    }
}