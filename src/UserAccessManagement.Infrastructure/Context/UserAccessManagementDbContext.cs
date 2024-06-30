using Microsoft.EntityFrameworkCore;
using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Infrastructure.Data.Context;

public class UserAccessManagementDbContext(DbContextOptions<UserAccessManagementDbContext> options) 
    : DbContext(options)
{
    public DbSet<EligibilityFile> EligibilityFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserAccessManagementDbContext).Assembly);
    }
}