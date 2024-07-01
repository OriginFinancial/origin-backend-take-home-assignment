using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using UserAccessManagement.Infrastructure.Data.Context;

namespace UserAccessManagement.API.Middleware;

public sealed class DbContextTransactionMiddleware
{
    private readonly RequestDelegate _next;
    private IDbContextTransaction? _transaction;

    public DbContextTransactionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, UserAccessManagementDbContext dbContext)
    {
        try
        {
            _transaction = await dbContext.Database.BeginTransactionAsync();

            await _next(context);

            // Considering only OK in this test scenario
            if (context.Response.StatusCode != (int)HttpStatusCode.OK)
            {
                await _transaction.RollbackAsync();
            }
            else
            {
                await _transaction.CommitAsync();
            }
        }
        catch (Exception)
        {
            if (_transaction is not null)
                await _transaction.RollbackAsync();

            throw;
        }
        finally
        {
            _transaction?.Dispose();
        }
    }
}