using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using UserAccessManagement.Application.BackgraoundTasks;
using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Application.Handlers;
using UserAccessManagement.Domain.Services;
using UserAccessManagement.Domain.Services.Interfaces;
using UserAccessManagement.Infrastructure.Services;

namespace UserAccessManagement.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<AddEligibilityFileCommand, CommandResult>, AddEligibilityFileCommandHandler>();
        services.AddTransient<ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult>, GetLastElibilityFileByEmployerCommandHandler>();
        services.AddTransient<ICommandHandler<GetLastElibilityFileReportByEmployerCommand, GetLastElibilityFileReportByEmployerCommandResult>, GetLastElibilityFileReportByEmployerCommandHandler>();

        services.AddTransient<ICommandHandler<SignUpCommand, SignUpCommandResult>, SignUpCommandHandler>();

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<ICsvService>(); 
        services.AddTransient<IEligibilityFileDomainService, EligibilityFileDomainService>();
        services.AddTransient<ISignUpDomainService, SignUpDomainService>();

        return services;
    }

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
        services.AddTransient<ProcessEligibilityFileBackgraoundTask>();

        return services;
    }
}
