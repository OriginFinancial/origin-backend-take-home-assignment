using Hangfire;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;
using UserAccessManagement.API.Filters;
using UserAccessManagement.API.Middleware;
using UserAccessManagement.Application.DependencyInjection;
using UserAccessManagement.BackgroundTask.DependencyInjection;
using UserAccessManagement.EmployerService.DependencyInjection;
using UserAccessManagement.Infrastructure.Data.Context;
using UserAccessManagement.Infrastructure.Data.DependencyInjection;
using UserAccessManagement.UserService.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
});

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddUserServiceClient();
builder.Services.AddEmployerServiceClient();
builder.Services.AddCommandHandlers();
builder.Services.AddHangfireWithRedis(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<UserAccessManagementDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<DbContextTransactionMiddleware>();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    IsReadOnlyFunc = (DashboardContext context) => true
});

app.MapControllers();

app.Run();
