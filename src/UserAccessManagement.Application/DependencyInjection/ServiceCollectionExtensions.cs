﻿using Microsoft.Extensions.DependencyInjection;
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
}
