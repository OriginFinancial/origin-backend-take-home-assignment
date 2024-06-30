using Microsoft.Extensions.DependencyInjection;
using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Application.Handlers;

namespace UserAccessManagement.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddTransient<ICommandHandler<AddEligibilityFileCommand, CommandResult>, AddEligibilityFileCommandHandler>();
        services.AddTransient<ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult>, GetLastElibilityFileByEmployerCommandHandler>();

        return services;
    }
}
