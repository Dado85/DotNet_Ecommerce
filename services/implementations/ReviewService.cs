using Ecommerce.config.errors;
using Ecommerce.Dto;
using Ecommerce.mapper;
using Ecommerce.Repositories.Implementations;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Schema;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Services.Implementations;

public class ReviewService(IUnitOfWork _unitOfWork, ReviewMapper mapper) : IReviewService
{
    public async Task<ReviewResponseDto> AddReviews(CreateReviewDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Message) || dto.Rating < 0 || dto.ProductId <= 0)
        {
            throw new BadRequestException("invalid data from client");
        }

        var product = await _unitOfWork.ProductRepository.GetByIdAsync(dto.ProductId);
        if (product == null)
        {
            throw new NotFoundException($"Product with id {dto.ProductId} not found");
        }

        //dto->entity
        var reviews = mapper.ToEntity(dto);
        await _unitOfWork.ReviewRepository.AddAsync(reviews);
        await _unitOfWork.SaveChangesAsync();
        return mapper.ToDto(reviews);
    }

    public async Task<ReviewResponseDto> UpdateReview(int reviewId, UpdateReviewDto dto)
    {
        if (reviewId <= 0) throw new BadRequestException("Invalid review id");
        if (dto == null || string.IsNullOrWhiteSpace(dto.Message) || dto.Rating < 0)
        {
            throw new BadRequestException("invalid data from client");
        }

        var review = await _unitOfWork.ReviewRepository.GetByIdAsync(reviewId);
        if (review == null)
        {
            throw new NotFoundException($"Review with id {reviewId} not found");
        }

        review.Message = dto.Message.Trim();
        review.Rating = dto.Rating;
        review.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();
        return mapper.ToDto(review);
    }
}