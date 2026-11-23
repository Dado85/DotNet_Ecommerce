using Ecommerce.Dto;
using Riok.Mapperly.Abstractions;

namespace Ecommerce.mapper;

[Mapper]
public partial class CategoryMapper
{
    public partial Category ToEntity(CreateCategoryReqDto dto);
    public partial CategoryResponseDto ToDto(Category category);
}