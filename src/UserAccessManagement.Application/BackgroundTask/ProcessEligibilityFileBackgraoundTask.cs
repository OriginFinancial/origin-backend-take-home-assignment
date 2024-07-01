using Hangfire;
using UserAccessManagement.Domain.Services.Interfaces;

namespace UserAccessManagement.Application.BackgraoundTasks;

public sealed class ProcessEligibilityFileBackgraoundTask
{
    private readonly IEligibilityFileDomainService _eligibilityFileDomainService;

    public ProcessEligibilityFileBackgraoundTask(IEligibilityFileDomainService eligibilityFileDomainService)
    {
        _eligibilityFileDomainService = eligibilityFileDomainService;
    }

    public void Enqueue(long eligibilityFileId, CancellationToken cancellationToken = default)
    {
        BackgroundJob.Schedule(() => _eligibilityFileDomainService.ProccessFileAsync(eligibilityFileId, cancellationToken), TimeSpan.FromSeconds(10));
    }
}