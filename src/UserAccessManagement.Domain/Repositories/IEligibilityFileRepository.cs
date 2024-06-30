using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Domain.Repositories;

public interface IEligibilityFileRepository
{
    Task<EligibilityFile> AddAsync(EligibilityFile eligibilityFile, CancellationToken cancellationToken = default);
    Task<EligibilityFile?> GetByEmployerIdAsync(Guid employerId, CancellationToken cancellationToken = default);
    EligibilityFile Update(EligibilityFile eligibilityFile);
}