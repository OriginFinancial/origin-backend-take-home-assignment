using Microsoft.Extensions.DependencyInjection;

namespace UserAccessManagement.EmployerService.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmployerServiceClient(this IServiceCollection services)
    {
        services.AddSingleton<IEmployerServiceClient, EmployerServiceClient>();

        return services;
    }
}