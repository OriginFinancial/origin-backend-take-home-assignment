using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Domain.Repositories;

public interface IEligibilityFileRepository
{
    Task<EligibilityFile> AddAsync(EligibilityFile eligibilityFile, CancellationToken cancellationToken = default);
    Task<EligibilityFile?> GetByEmployerIdAsync(Guid employerId, CancellationToken cancellationToken = default);
    Task<EligibilityFile> UpdateAsync(EligibilityFile eligibilityFile, CancellationToken cancellationToken);
    Task<bool> AnyProcessingAsync(Guid employerId, CancellationToken cancellationToken);
    Task<EligibilityFile?> GetByIdAsync(long id, CancellationToken cancellationToken);
}