using Ecommerce.config;
using Ecommerce.Controllers;
using Ecommerce.Dto;
using Ecommerce.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ecommerce.Tests.Controllers;

public class ReviewControllerTests
{
    private readonly FakeReviewService _fakeService;
    private readonly ReviewController _controller;

    public ReviewControllerTests()
    {
        _fakeService = new FakeReviewService();
        _controller = new ReviewController(_fakeService);
    }

    [Fact]
    public async Task CreateReview_ReturnsOkWithCreatedReview()
    {
        // Arrange
        var request = new CreateReviewDto
        {
            Message = "Awesome product!",
            Rating = 5,
            ProductId = 2
        };
        _fakeService.AddReviewsFunc = dto => Task.FromResult(new ReviewResponseDto
        {
            Id = 50,
            Message = dto.Message,
            Rating = dto.Rating,
            ProductId = dto.ProductId
        });

        // Act
        var result = await _controller.CreateReview(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(200, response.StatusCode);

        var data = Assert.IsType<ReviewResponseDto>(response.Data);
        Assert.Equal(50, data.Id);
        Assert.Equal("Awesome product!", data.Message);
        Assert.Equal(5, data.Rating);
        Assert.Equal(2, data.ProductId);
    }

    [Fact]
    public async Task UpdateReview_ReturnsOkWithUpdatedReview()
    {
        // Arrange
        int reviewId = 50;
        var request = new UpdateReviewDto
        {
            Message = "Updated: works even better now",
            Rating = 4
        };
        _fakeService.UpdateReviewFunc = (id, dto) => Task.FromResult(new ReviewResponseDto
        {
            Id = id,
            Message = dto.Message,
            Rating = dto.Rating,
            ProductId = 2
        });

        // Act
        var result = await _controller.UpdateReview(reviewId, request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
        Assert.True(response.Success);

        var data = Assert.IsType<ReviewResponseDto>(response.Data);
        Assert.Equal(50, data.Id);
        Assert.Equal("Updated: works even better now", data.Message);
        Assert.Equal(4, data.Rating);
    }
}
