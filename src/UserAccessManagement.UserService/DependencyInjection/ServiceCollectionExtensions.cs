using Microsoft.Extensions.DependencyInjection;

namespace UserAccessManagement.UserService.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUserServiceClient(this IServiceCollection services)
    {
        services.AddSingleton<IUserServiceClient, UserServiceClient>();

        return services;
    }
}