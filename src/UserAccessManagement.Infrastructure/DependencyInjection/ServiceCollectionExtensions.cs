using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System.Data;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Infrastructure.Data.Context;
using UserAccessManagement.Infrastructure.Data.Repositories;

namespace UserAccessManagement.Infrastructure.Data.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database") ?? throw new ArgumentException("ConnectionStrings:Databse");

        services.AddDbContext<UserAccessManagementDbContext>(options => options.UseMySQL(connectionString), ServiceLifetime.Scoped);
        services.AddScoped<IDbConnection, MySqlConnection>(_ => new MySqlConnection(connectionString));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        services.AddTransient<IEligibilityFileRepository, EligibilityFileRepository>();
        services.AddTransient<IEligibilityFileLineRepository, EligibilityFileLineRepository>();

        return services;
    }
}