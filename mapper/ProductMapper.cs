using Ecommerce.Dto;
using Ecommerce.Schema;
using Riok.Mapperly.Abstractions;

namespace Ecommerce.mapper;

[Mapper]
public partial class ProductMapper
{
    [MapperIgnoreTarget(nameof(Product.Id))]
    [MapperIgnoreTarget(nameof(Product.Category))]
    [MapperIgnoreTarget(nameof(Product.Reviews))]
    [MapperIgnoreTarget(nameof(Product.OrderItems))]
    public partial Product ToEntity(ProductRequestDto dto);

    public partial ProductResponseDto ToDto(Product product);
    public partial ProductWithCategoryResponseDto ToProductWithCategoryDto(Product product);

    public partial ReviewResponseDto ToReviewDto(Review review);
}