using Microsoft.EntityFrameworkCore;
using System.Data;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Enums;
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

        await _context.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task<bool> AnyProcessingAsync(Guid employerId, CancellationToken cancellationToken)
    {
        return await _context.EligibilityFiles
            .Where(t => t.Active)
            .Where(t => t.EmployerId == employerId)
            .Where(t => t.Status == EligibilityFileStatus.Processing)
            .AnyAsync(cancellationToken);
    }

    public async Task<EligibilityFile?> GetByEmployerIdAsync(Guid employerId, CancellationToken cancellationToken = default)
    {
        return await _context.EligibilityFiles
            .Where(t => t.Active)
            .FirstOrDefaultAsync(t => t.EmployerId == employerId, cancellationToken);
    }

    public async Task<EligibilityFile?> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.EligibilityFiles
            .Where(t => t.Active)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<EligibilityFile> UpdateAsync(EligibilityFile eligibilityFile, CancellationToken cancellationToken)
    {
        _context.EligibilityFiles.Update(eligibilityFile);

        await _context.SaveChangesAsync(cancellationToken);

        return eligibilityFile;
    }
}