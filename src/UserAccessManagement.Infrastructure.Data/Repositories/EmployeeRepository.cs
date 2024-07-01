using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Infrastructure.Data.Context;

namespace UserAccessManagement.Infrastructure.Data.Repositories;

public sealed class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly UserAccessManagementDbContext _context;

    public EmployeeRepository(IDbConnection dbConnection, UserAccessManagementDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        var entry = await _context.Employees.AddAsync(employee, cancellationToken);

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task<IEnumerable<Employee>> FindByEligibilityFileId(long eligibilityFileId, CancellationToken cancellationToken)
    {
        var sql = @"
            SELECT
	            e.id          AS Id,
	            e.email       AS Email,
	            e.country     AS Country,
	            e.birth_date  AS BirthDate,
	            e.salary      AS Salary,
                e.employer_id AS EmployerId,
	            e.`active`    AS `Active`,
	            e.created_at  AS CreatedAt,
	            e.updated_at  AS UpdatedAt
            FROM
	            employee e
            WHERE 
	            e.`active` = 1
	            AND e.eligibility_file_id = @EligibilityFileId";

        var employee = await _dbConnection.QueryAsync<Employee>(sql, new { EligibilityFileId = eligibilityFileId });

        return employee;
    }

    public async Task<Employee?> GetAsync(string email, CancellationToken cancellationToken = default)
    {
        var sql = @"
            SELECT
	            e.id          AS Id,
	            e.email       AS Email,
	            e.country     AS Country,
	            e.birth_date  AS BirthDate,
	            e.salary      AS Salary,
                e.employer_id AS EmployerId,
	            e.`active`    AS `Active`,
	            e.created_at  AS CreatedAt,
	            e.updated_at  AS UpdatedAt
            FROM
	            employee e
            WHERE 
	            e.`active` = 1
	            AND e.email = @Email";

        var employee = await _dbConnection.QueryFirstOrDefaultAsync<Employee>(sql, new { Email = email });  

        return employee;
    }

    public async Task InactiveByEligibilityFileId(long eligibilityFileId, CancellationToken cancellationToken = default)
    {
        var transaction = _context.Database.CurrentTransaction!.GetDbTransaction();

        var sql = @"
            UPDATE employee e SET e.`active` = 1, e.updated_at = NOW()
            WHERE e.eligibility_file_id = @EligibilityFileId";

        await _dbConnection.ExecuteAsync(sql, new { EligibilityFileId = eligibilityFileId }, transaction);
    }
}