using Ecommerce.config.errors;
using Ecommerce.Dto;
using Ecommerce.mapper;
using Ecommerce.Repositories.Interfaces;
using Ecommerce.Schema;
using Ecommerce.Services.Interfaces;

namespace Ecommerce.Services.Implementations
{
    public class ProductService(IUnitOfWork unitOfWork, ProductMapper productMapper, ProductRedisCache _prdCache, ILogger<ProductService> _logger) : IProductService
    {
        public async Task<ProductResponseDto> CreateProductAsync(ProductRequestDto dto)
        {
            var product = productMapper.ToEntity(dto);

            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();

            return productMapper.ToDto(product);

        }
        public async Task<List<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllWithReviewsAsync();
            return products.Select(productMapper.ToDto).ToList();
        }
        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Invalid product id");

            //try to get prd from redis
            var cachePrd = await _prdCache.GetProductCache(id);
            if (cachePrd != null)
            {
                _logger.LogInformation($"The prd is returned from Redis cache: {cachePrd}");
                return cachePrd;
            }
            //cache miss do the db call.
            _logger.LogInformation($"Cache miss now its time for db call.");
            var product = await unitOfWork.ProductRepository.GetByIdWithReviewsAsync(id);
            if (product == null)
            {
                throw new NotFoundException($"Product with id {id} not found");
            }

            var dto = productMapper.ToDto(product);
            await _prdCache.SetProductCache(id, dto);
            _logger.LogInformation($"the returned prd from db also set into cache.");
            return dto;
        }
        public async Task<List<ProductResponseDto>> GetProductByCategoryId(int id)

        {
            var prd = await unitOfWork.ProductRepository.GetProductsByCategoryIdAsync(id);
            return prd.Select(productMapper.ToDto).ToList();
        }
        public async Task<ProductWithCategoryResponseDto> GetProductWithCategoryDetails(int ProductId)
        {
            var res = await unitOfWork.ProductRepository.GetProductWithCategoryDetails(ProductId);
            return productMapper.ToProductWithCategoryDto(res!);
        }

        public async Task<ProductResponseDto> UpdateProductAsync(int id, ProductRequestDto dto)
        {
            if (id <= 0) throw new BadRequestException("Invalid product id");
            if (dto == null) throw new BadRequestException("Empty Request Body");
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new BadRequestException("Product title is required");
            if (dto.Price < 0) throw new BadRequestException("Price cannot be negative");

            var product = await unitOfWork.ProductRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Product with id {id} not found");

            if (dto.CategoryId.HasValue)
            {
                var category = await unitOfWork.CategoryRepository.GetByIdAsync(dto.CategoryId.Value)
                    ?? throw new BadRequestException($"Category with id {dto.CategoryId.Value} does not exist");
                product.CategoryId = category.Id;
            }

            product.Title = dto.Title.Trim();
            product.Price = dto.Price;
            product.Rating = dto.Rating;
            product.Description = dto.Description;
            product.ImageUrl = dto.ImageUrl;
            product.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.SaveChangesAsync();
            return productMapper.ToDto(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Invalid product id");

            var product = await unitOfWork.ProductRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Product with id {id} not found");

            if (await unitOfWork.ProductRepository.IsProductInOrdersAsync(id))
                throw new ConflictException("Cannot delete product that is referenced in existing orders");

            unitOfWork.ProductRepository.Delete(product);
            await unitOfWork.SaveChangesAsync();
        }
    }

}