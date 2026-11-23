using Asp.Versioning;
using Ecommerce.config;
using Ecommerce.Dto;
using Ecommerce.Schema;
using Ecommerce.Services;
using Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] ProductRequestDto request)
    {

        var result = await _productService.CreateProductAsync(request);

        return Ok(ApiResponse<Object>.Succeed(result, 200, "Product Created Successfully."));
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(ApiResponse<Object>.Succeed(products, 200, "Products Fetched Successfully"));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        return Ok(ApiResponse<Object>.Succeed(product!, 200, "Product Fetched Successfully"));
    }
    [HttpGet("category/{categoryId}")]
    public async Task<IActionResult> GetProductsByCategoryID(int categoryId)
    {
        var res = await _productService.GetProductByCategoryId(categoryId);
        return Ok(ApiResponse<Object>.Succeed(res, 200, "Prds with respect to category are fetched"));
    }
    [HttpGet("by-category/{productId}")]
    public async Task<IActionResult> GetProductWithCategoryDetails(int productId)
    {
        var res = await _productService.GetProductWithCategoryDetails(productId);
        return Ok(ApiResponse<Object>.Succeed(res, 200, "Prds with category details are fetched"));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductRequestDto request)
    {
        var res = await _productService.UpdateProductAsync(id, request);
        return Ok(ApiResponse<object>.Succeed(res, 200, "Product Updated Successfully"));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return Ok(ApiResponse<object?>.Succeed(null, 200, "Product Deleted Successfully"));
    }

}