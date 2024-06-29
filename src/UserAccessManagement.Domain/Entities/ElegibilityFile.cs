using UserAccessManagement.Domain.Commom;
using UserAccessManagement.Domain.Enums;

namespace UserAccessManagement.Domain.Entities;

public class ElegibilityFile : Entity<long>
{
    public ElegibilityFile(Guid employerId)
    {
        EmployerId = employerId;
        Status = ElegibilityFileStatus.Processing;
    }

    public Guid EmployerId { get; set; }
    public ElegibilityFileStatus Status { get; private set; }

    public void SetProcessed() => ChangeStatus(ElegibilityFileStatus.Processed);

    public void SetProcessedWithErrors() => ChangeStatus(ElegibilityFileStatus.ProcessedWithErrors);

    private void ChangeStatus(ElegibilityFileStatus status)
    {
        Status = status;
        Stamp();
    }
}