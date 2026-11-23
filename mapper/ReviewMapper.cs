using Ecommerce.Dto;
using Ecommerce.Schema;
using Riok.Mapperly.Abstractions;

namespace Ecommerce.mapper;

[Mapper]
public partial class ReviewMapper
{
    [MapperIgnoreTarget(nameof(Review.Id))]
    [MapperIgnoreTarget(nameof(Review.Product))]
    [MapperIgnoreTarget(nameof(Review.CreatedAt))]
    [MapperIgnoreTarget(nameof(Review.UpdatedAt))]
    public partial Review ToEntity(CreateReviewDto dto);

    //entity->dto
    public partial ReviewResponseDto ToDto(Review review);
}