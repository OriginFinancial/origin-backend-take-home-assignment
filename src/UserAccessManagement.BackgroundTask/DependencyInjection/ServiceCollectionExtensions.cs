using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace UserAccessManagement.BackgroundTask.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHangfireWithRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(options =>
        {
            var connectionString = configuration.GetConnectionString("Redis") 
                ?? throw new ArgumentException("ConnectionStrings:Databse");

            var redis = ConnectionMultiplexer.Connect(connectionString);

            options.UseRedisStorage(redis, options: new RedisStorageOptions { Prefix = "hangfire" });
        });

        services.AddHangfireServer();

        return services;
    }
}