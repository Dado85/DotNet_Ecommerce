using Asp.Versioning;
using Ecommerce.config;
using Ecommerce.Dto;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReviewController(IReviewService _reviewService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
    {
        var result = await _reviewService.AddReviews(dto);
        return Ok(ApiResponse<object>.Succeed(result, 200, "Review Created Successfully"));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto dto)
    {
        var result = await _reviewService.UpdateReview(id, dto);
        return Ok(ApiResponse<object>.Succeed(result, 200, "Review Updated Successfully"));
    }
}
