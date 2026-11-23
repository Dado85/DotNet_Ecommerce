using System.Text.Json;
using Ecommerce.Dto;
using StackExchange.Redis;
public class ProductRedisCache(IDatabase _redis)
{
    private static string GetKey(int Id) => $"Product:Summary:{Id}";
    private static readonly TimeSpan DefaultExpiry = TimeSpan.FromSeconds(5);
    public async Task<ProductResponseDto?> GetProductCache(int Id)
    {
        var cacheKey = GetKey(Id);
        RedisValue cachedJson = await _redis.StringGetAsync(cacheKey);
        if (cachedJson.IsNullOrEmpty) return null;
        var product = JsonSerializer.Deserialize<ProductResponseDto>(cachedJson.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return product;
    }
    public async Task SetProductCache(int Id, ProductResponseDto prd,TimeSpan? expiry=null)
    {
        var cacheKey = GetKey(Id);
        var json = JsonSerializer.Serialize(prd);
        await _redis.StringSetAsync(cacheKey, json,expiry??DefaultExpiry);
    }
      public async Task RemoveProductAsync(int id)
    {
        var cacheKey = GetKey(id);
        await _redis.KeyDeleteAsync(cacheKey);
    }
}