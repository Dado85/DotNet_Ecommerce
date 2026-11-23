namespace Ecommerce.Dto;

public class CreateReviewDto
{
    public required string Message { get; set; }
    public int Rating { get; set; }
    public int ProductId { get; set; }
}

public class UpdateReviewDto
{
    public required string Message { get; set; }
    public int Rating { get; set; }
}

public class ReviewResponseDto
{
    public int Id { get; set; }
    public string? Message { get; set; }
    public int Rating { get; set; }
    public int ProductId { get; set; }
}