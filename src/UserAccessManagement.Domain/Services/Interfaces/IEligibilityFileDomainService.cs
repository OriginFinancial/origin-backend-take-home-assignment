namespace UserAccessManagement.Domain.Services.Interfaces;

public interface IEligibilityFileDomainService
{
    Task ProccessFileAsync(long eligibilityFileId, CancellationToken cancellationToken = default);
}