using Ecommerce.config;
using Ecommerce.Controllers;
using Ecommerce.Dto;
using Ecommerce.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ecommerce.Tests.Controllers;

public class CategoryControllerTests
{
    private readonly FakeCategoryService _fakeService;
    private readonly CategoryController _controller;

    public CategoryControllerTests()
    {
        _fakeService = new FakeCategoryService();
        _controller = new CategoryController(_fakeService);
    }

    [Fact]
    public async Task CreateCategory_ReturnsOkWithCreatedCategory()
    {
        // Arrange
        var request = new CreateCategoryReqDto { Name = "Gaming" };
        _fakeService.CreateCategoryFunc = dto => Task.FromResult(new CategoryResponseDto
        {
            Id = 10,
            Name = dto.Name
        });

        // Act
        var result = await _controller.CreateCategory(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(200, response.StatusCode);

        var data = Assert.IsType<CategoryResponseDto>(response.Data);
        Assert.Equal(10, data.Id);
        Assert.Equal("Gaming", data.Name);
    }

    [Fact]
    public async Task GetAllCategories_ReturnsOkWithCategoryList()
    {
        // Arrange
        _fakeService.GetAllCategoriesFunc = () => Task.FromResult(new List<CategoryResponseDto>
        {
            new() { Id = 1, Name = "Electronics" },
            new() { Id = 2, Name = "Books" }
        });

        // Act
        var result = await _controller.GetAllCategories();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<List<CategoryResponseDto>>(response.Data);
        Assert.Equal(2, data.Count);
    }

    [Fact]
    public async Task GetCategoryById_ReturnsOkWithMatchingCategory()
    {
        // Arrange
        int categoryId = 5;
        _fakeService.GetCategoryByIdFunc = id => Task.FromResult(new CategoryResponseDto
        {
            Id = id,
            Name = "Laptops"
        });

        // Act
        var result = await _controller.GetCategoryById(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<CategoryResponseDto>(response.Data);
        Assert.Equal(5, data.Id);
        Assert.Equal("Laptops", data.Name);
    }

    [Fact]
    public async Task UpdateCategory_ReturnsOkWithUpdatedCategory()
    {
        // Arrange
        int categoryId = 2;
        var request = new CreateCategoryReqDto { Name = "Home & Kitchen" };
        _fakeService.UpdateCategoryFunc = (id, dto) => Task.FromResult(new CategoryResponseDto
        {
            Id = id,
            Name = dto.Name
        });

        // Act
        var result = await _controller.UpdateCategory(categoryId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<CategoryResponseDto>(response.Data);
        Assert.Equal(2, data.Id);
        Assert.Equal("Home & Kitchen", data.Name);
    }

    [Fact]
    public async Task DeleteCategory_ReturnsOk()
    {
        // Arrange
        int categoryId = 3;
        bool deleted = false;
        _fakeService.DeleteCategoryFunc = id =>
        {
            deleted = true;
            return Task.CompletedTask;
        };

        // Act
        var result = await _controller.DeleteCategory(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object?>>(okResult.Value);
        Assert.True(response.Success);
        Assert.True(deleted);
    }
}
