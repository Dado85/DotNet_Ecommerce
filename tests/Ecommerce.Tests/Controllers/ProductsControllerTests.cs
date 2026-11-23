using Ecommerce.config;
using Ecommerce.Controllers;
using Ecommerce.Dto;
using Ecommerce.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ecommerce.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly FakeProductService _fakeService;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _fakeService = new FakeProductService();
        _controller = new ProductsController(_fakeService);
    }

    [Fact]
    public async Task Create_ReturnsOkWithCreatedProduct()
    {
        // Arrange
        var request = new ProductRequestDto
        {
            Title = "Smartphone",
            Price = 799.99m,
            CategoryId = 1,
            Description = "Latest model"
        };
        _fakeService.CreateProductAsyncFunc = dto => Task.FromResult(new ProductResponseDto
        {
            Id = 10,
            Title = dto.Title,
            Price = dto.Price,
            CategoryId = dto.CategoryId,
            Description = dto.Description
        });

        // Act
        var result = await _controller.Create(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<ProductResponseDto>(response.Data);
        Assert.Equal(10, data.Id);
        Assert.Equal("Smartphone", data.Title);
        Assert.Equal(799.99m, data.Price);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithProductList()
    {
        // Arrange
        _fakeService.GetAllProductsAsyncFunc = () => Task.FromResult(new List<ProductResponseDto>
        {
            new() { Id = 1, Title = "Phone", Price = 500, CategoryId = 1 },
            new() { Id = 2, Title = "Laptop", Price = 1200, CategoryId = 1 }
        });

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<List<ProductResponseDto>>(response.Data);
        Assert.Equal(2, data.Count);
    }

    [Fact]
    public async Task GetById_ReturnsOkWithMatchingProduct()
    {
        // Arrange
        int productId = 4;
        _fakeService.GetProductByIdAsyncFunc = id => Task.FromResult<ProductResponseDto?>(new ProductResponseDto
        {
            Id = id,
            Title = "Keyboard",
            Price = 80,
            CategoryId = 1
        });

        // Act
        var result = await _controller.GetById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<ProductResponseDto>(response.Data);
        Assert.Equal(4, data.Id);
        Assert.Equal("Keyboard", data.Title);
    }

    [Fact]
    public async Task GetProductsByCategoryID_ReturnsOkWithProducts()
    {
        // Arrange
        int categoryId = 2;
        _fakeService.GetProductByCategoryIdFunc = catId => Task.FromResult(new List<ProductResponseDto>
        {
            new() { Id = 1, Title = "Book A", Price = 20, CategoryId = catId }
        });

        // Act
        var result = await _controller.GetProductsByCategoryID(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<List<ProductResponseDto>>(response.Data);
        Assert.Single(data);
        Assert.Equal(2, data[0].CategoryId);
    }

    [Fact]
    public async Task GetProductWithCategoryDetails_ReturnsOkWithCategoryInfo()
    {
        // Arrange
        int productId = 7;
        _fakeService.GetProductWithCategoryDetailsFunc = id => Task.FromResult(new ProductWithCategoryResponseDto
        {
            Id = id,
            Title = "Headphones",
            Price = 150,
            Category = new PrdWithCategoryResponseDto { Id = 1, Name = "Audio" }
        });

        // Act
        var result = await _controller.GetProductWithCategoryDetails(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<ProductWithCategoryResponseDto>(response.Data);
        Assert.Equal(7, data.Id);
        Assert.NotNull(data.Category);
        Assert.Equal("Audio", data.Category.Name);
    }

    [Fact]
    public async Task Update_ReturnsOkWithUpdatedProduct()
    {
        // Arrange
        int productId = 1;
        var request = new ProductRequestDto
        {
            Title = "Updated Phone",
            Price = 699,
            CategoryId = 1
        };
        _fakeService.UpdateProductAsyncFunc = (id, dto) => Task.FromResult(new ProductResponseDto
        {
            Id = id,
            Title = dto.Title,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        });

        // Act
        var result = await _controller.Update(productId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<ProductResponseDto>(response.Data);
        Assert.Equal("Updated Phone", data.Title);
        Assert.Equal(699, data.Price);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        // Arrange
        int productId = 1;
        bool deleted = false;
        _fakeService.DeleteProductAsyncFunc = id =>
        {
            deleted = true;
            return Task.CompletedTask;
        };

        // Act
        var result = await _controller.Delete(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object?>>(okResult.Value);
        Assert.True(response.Success);
        Assert.True(deleted);
    }
}
