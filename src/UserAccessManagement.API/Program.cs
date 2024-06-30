using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;
using UserAccessManagement.API.Filters;
using UserAccessManagement.Infrastructure.Data.Context;

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
builder.Services.AddDbContext<UserAccessManagementDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("Database") ?? throw new ArgumentException("ConnectionStrings:Databse")));
builder.Services.AddScoped<IDbConnection, MySqlConnection>(_ => new MySqlConnection(builder.Configuration.GetConnectionString("Database") ?? throw new ArgumentException("ConnectionStrings:Databse")));

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

app.MapControllers();

app.Run();