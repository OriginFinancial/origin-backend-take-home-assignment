using UserAccessManagement.Domain.Commom;
using UserAccessManagement.Domain.Enums;

namespace UserAccessManagement.Domain.Entities;

public class EligibilityFile : Entity<long>
{
    public EligibilityFile(Guid employerId, string url)
    {
        Url = url;
        EmployerId = employerId;
        Status = EligibilityFileStatus.Processing;
    }

    public string Url { get; private set; }
    public Guid EmployerId { get; private set; }
    public EligibilityFileStatus Status { get; private set; }

    public void SetProcessed() => ChangeStatus(EligibilityFileStatus.Processed);

    public void SetProcessedWithErrors() => ChangeStatus(EligibilityFileStatus.ProcessedWithErrors);

    private void ChangeStatus(EligibilityFileStatus status)
    {
        Status = status;
        Stamp();
    }
}