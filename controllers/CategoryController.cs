using Asp.Versioning;
using Ecommerce.config;
using Ecommerce.Dto;
using Ecommerce.Services;
using Ecommerce.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]

public class CategoryController : ControllerBase
{
    private ICategoryService _categoryService;
    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryReqDto dto)
    {
        var res = await _categoryService.CreateCategory(dto);
        return Ok(ApiResponse<Object>.Succeed(res, 200, "category Cretaed successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var res = await _categoryService.GetAllCategories();
        return Ok(ApiResponse<object>.Succeed(res, 200, "Categories Fetched Successfully"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id)
    {
        var res = await _categoryService.GetCategoryById(id);
        return Ok(ApiResponse<object>.Succeed(res, 200, "Category Fetched Successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryReqDto dto)
    {
        var res = await _categoryService.UpdateCategory(id, dto);
        return Ok(ApiResponse<object>.Succeed(res, 200, "Category Updated Successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategory(id);
        return Ok(ApiResponse<object?>.Succeed(null, 200, "Category Deleted Successfully"));
    }

}