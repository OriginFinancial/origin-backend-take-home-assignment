using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Domain.Repositories;

public interface IEligibilityFileLineRepository
{
    Task<EligibilityFileLine> AddAsync(EligibilityFileLine eligibilityFileLine, CancellationToken cancellationToken = default);
    Task<IEnumerable<EligibilityFileLine>> FindByEligibilityFileIdAsync(long eligibilityFileId, CancellationToken cancellationToken = default);
    Task InactiveByEligibilityFileId(long eligibilityFileId, CancellationToken cancellationToken = default);
}