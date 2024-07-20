using System.Reflection;
using Application.Common.Behaviours;
using Application.Messages.Validators.Queries;
using FluentValidation;
using Infrastructure.Configuration;
using MediatR;
using Origin.API;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
builder.Services.Configure<AppSettings>(configuration);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceClients()
    .AddMediatrDependencies();
builder.Services.AddValidatorsFromAssemblyContaining<CheckEmployerByEmailQueryValidator>();
var services = builder.Services;
services.BuildServiceProvider(new ServiceProviderOptions()
{
    ValidateScopes = true,
    ValidateOnBuild = true
});

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.Run();
public partial class Program {}
