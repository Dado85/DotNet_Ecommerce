namespace Ecommerce.Dto
{
    public class ProductRequestDto
    {

        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
    public class ProductResponseDto
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public List<ReviewResponseDto> Reviews { get; set; } = new();
    }
    public class PrdWithCategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class ProductWithCategoryResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Rating { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PrdWithCategoryResponseDto? Category { get; set; }
        public List<ReviewResponseDto> Reviews { get; set; } = new();
    }
}