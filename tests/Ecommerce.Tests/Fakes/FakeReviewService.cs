using Ecommerce.Dto;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Tests.Fakes;

public class FakeReviewService : IReviewService
{
    public Func<CreateReviewDto, Task<ReviewResponseDto>>? AddReviewsFunc { get; set; }
    public Func<int, UpdateReviewDto, Task<ReviewResponseDto>>? UpdateReviewFunc { get; set; }

    public Task<ReviewResponseDto> AddReviews(CreateReviewDto dto) =>
        AddReviewsFunc != null
            ? AddReviewsFunc(dto)
            : Task.FromResult(new ReviewResponseDto
            {
                Id = 1,
                Message = dto.Message,
                Rating = dto.Rating,
                ProductId = dto.ProductId
            });

    public Task<ReviewResponseDto> UpdateReview(int reviewId, UpdateReviewDto dto) =>
        UpdateReviewFunc != null
            ? UpdateReviewFunc(reviewId, dto)
            : Task.FromResult(new ReviewResponseDto
            {
                Id = reviewId,
                Message = dto.Message,
                Rating = dto.Rating,
                ProductId = 1
            });
}
