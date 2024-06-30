using Microsoft.EntityFrameworkCore;
using System.Data;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Infrastructure.Data.Context;

namespace UserAccessManagement.Infrastructure.Data.Repositories;

public sealed class EligibilityFileRepository : IEligibilityFileRepository
{
    private readonly UserAccessManagementDbContext _context;

    public EligibilityFileRepository(UserAccessManagementDbContext context)
    {
        _context = context;
    }

    public async Task<EligibilityFile> AddAsync(EligibilityFile eligibilityFile, CancellationToken cancellationToken = default)
    {
        var entry = await _context.EligibilityFiles.AddAsync(eligibilityFile, cancellationToken);

        return entry.Entity;
    }

    public async Task<EligibilityFile?> GetByEmployerIdAsync(Guid employerId, CancellationToken cancellationToken = default)
    {
        return await _context.EligibilityFiles
            .Where(t => t.Active)
            .FirstOrDefaultAsync(t => t.EmployerId == employerId, cancellationToken);
    }

    public EligibilityFile Update(EligibilityFile eligibilityFile)
    {
        _context.EligibilityFiles.Update(eligibilityFile);

        return eligibilityFile;
    }
}