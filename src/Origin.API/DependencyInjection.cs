using System.Reflection;
using Application.Common.Behaviours;
using Application.Messages.Commands;
using Application.Messages.Handlers.Commands;
using Application.Messages.Handlers.Queries;
using Application.Messages.Queries;
using Domain.Models;
using Infrastructure.Records;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Origin.API;

public static class DependencyInjection
{
    public static IServiceCollection AddServiceClients(this IServiceCollection services)
    {
        services.AddHttpClient()
            .AddScoped<IUserServiceClient, UserServiceClient>()
            .AddScoped<IEmployerServiceClient, EmployerServiceClient>();
        return services;
    }
    public static IServiceCollection AddMediatrDependencies(this IServiceCollection services)
    {
        services.AddMediatR(cf =>
        {
            var assemblies = new List<Assembly>
            {
                typeof(Program).Assembly
            };
            cf.RegisterServicesFromAssemblies(assemblies.ToArray());
            cf.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cf.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddScoped<IRequestHandler<ProcessSignupCommand>, ProcessSignupCommandHandler>()
            .AddScoped<IRequestHandler<CreateUserCommand, bool>, CreateUserCommandHandler>()
            .AddScoped<IRequestHandler<ProcessEligibilityFileCommand, EligibilityProcessResult>, ProcessEligibilityFileCommandHandler>()
            .AddScoped<IRequestHandler<ProcessSignupCommand>, ProcessSignupCommandHandler>()
            .AddScoped<IRequestHandler<TerminateUnlistedUsersCommand>, TerminateUnlistedUsersCommandHandler>()
            .AddScoped<IRequestHandler<UpdateUserDataCommand, bool>, UpdateUserDataCommandHandler>()
            .AddScoped<IRequestHandler<CheckEmployerByEmailQuery, EmployerIdRecord?>, CheckEmployerByEmailQueryHandler>()
            .AddScoped<IRequestHandler<CreateUserCommand, bool>, CreateUserCommandHandler>()
            .AddScoped<IRequestHandler<GetEmployerByIdQuery, EmployerDto?>, GetEmployerByIdQueryHandler>()
            .AddScoped<IRequestHandler<GetUserByEmailQuery, UserDto?>, GetUserByEmailQueryHandler>();
        
        services.AddScoped<IRequestHandler<ProcessSignupCommand>, ProcessSignupCommandHandler>()
            .AddScoped<IRequestHandler<CreateUserCommand, bool>, CreateUserCommandHandler>()
            .AddScoped<IRequestHandler<ProcessEligibilityFileCommand, EligibilityProcessResult>, ProcessEligibilityFileCommandHandler>()
            .AddScoped<IRequestHandler<ProcessSignupCommand>, ProcessSignupCommandHandler>()
            .AddScoped<IRequestHandler<TerminateUnlistedUsersCommand>, TerminateUnlistedUsersCommandHandler>()
            .AddScoped<IRequestHandler<UpdateUserDataCommand, bool>, UpdateUserDataCommandHandler>()
            .AddScoped<IRequestHandler<CheckEmployerByEmailQuery, EmployerIdRecord?>, CheckEmployerByEmailQueryHandler>()
            .AddScoped<IRequestHandler<CreateUserCommand, bool>, CreateUserCommandHandler>()
            .AddScoped<IRequestHandler<GetEmployerByIdQuery, EmployerDto?>, GetEmployerByIdQueryHandler>()
            .AddScoped<IRequestHandler<GetUserByEmailQuery, UserDto?>, GetUserByEmailQueryHandler>();
        return services;
    }
}