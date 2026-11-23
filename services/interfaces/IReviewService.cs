using Ecommerce.Dto;

namespace Ecommerce.Services.Interfaces;

public interface IReviewService
{
    Task<ReviewResponseDto> AddReviews(CreateReviewDto dto);
    Task<ReviewResponseDto> UpdateReview(int reviewId, UpdateReviewDto dto);
}