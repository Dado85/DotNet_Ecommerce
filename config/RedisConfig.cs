using StackExchange.Redis;

namespace Ecommerce.Config;

public static class RedisConfig
{
    public static IServiceCollection AddRedis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Redis");

        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<Program>>();

            logger.LogInformation(
                "Connecting to Redis at {ConnectionString}",
                connectionString);

            var redis = ConnectionMultiplexer.Connect(connectionString!);

            logger.LogInformation(
                "Successfully connected to Redis at {ConnectionString}",
                connectionString);

            return redis;
        });

        services.AddSingleton<IDatabase>(sp =>
            sp.GetRequiredService<IConnectionMultiplexer>()
              .GetDatabase());

        return services;
    }
}
