namespace Ecommerce.Dto;

public class CreateCategoryReqDto
{
    public string Name { get; set; } = null!;
}

public class CategoryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}