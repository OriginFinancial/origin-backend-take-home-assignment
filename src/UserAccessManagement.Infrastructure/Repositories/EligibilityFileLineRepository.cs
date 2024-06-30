using Dapper;
using System.Data;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Infrastructure.Data.Context;

namespace UserAccessManagement.Infrastructure.Data.Repositories;

public sealed class EligibilityFileLineRepository : IEligibilityFileLineRepository
{
    private readonly IDbConnection _dbConnection;
    private readonly UserAccessManagementDbContext _context;

    public EligibilityFileLineRepository(IDbConnection dbConnection, UserAccessManagementDbContext context)
    {
        _dbConnection = dbConnection;
        _context = context;
    }

    public async Task<EligibilityFileLine> AddAsync(EligibilityFileLine eligibilityFileLine, CancellationToken cancellationToken = default)
    {
        var entry = await _context.EligibilityFileLines.AddAsync(eligibilityFileLine, cancellationToken);

        return entry.Entity;
    }

    public async Task<IEnumerable<EligibilityFileLine>> FindByEligibilityFileIdAsync(Guid eligibilityFileId, CancellationToken cancellationToken = default)
    {
        var sql = @"
            SELECT
                e.id                  AS Id,
	            e.content             AS Content,
	            e.line_type           AS LineType,
	            e.eligibility_file_id AS EligibilityFileId,
	            e.`active`            AS `Active`,
	            e.created_at          AS CreatedAt,
	            e.updated_at          AS UpdatedAt
            FROM
	            eligibility_file_line e
            WHERE 
                e.`active` = 1
	            AND e.eligibility_file_id = @EligibilityFileId";

        var eligibilityFileLines = await _dbConnection.QueryAsync<EligibilityFileLine>(sql, new { EligibilityFileId = eligibilityFileId });

        return eligibilityFileLines;
    }

    public async Task InactiveByEligibilityFileId(long eligibilityFileId, CancellationToken cancellationToken = default)
    {
        var sql = @"
            UPDATE eligibility_file_line e SET e.`active` = 1, e.updated_at = NOW()
            WHERE e.eligibility_file_id = @EligibilityFileId";

        await _dbConnection.ExecuteAsync(sql, new { EligibilityFileId = eligibilityFileId });
    }
}