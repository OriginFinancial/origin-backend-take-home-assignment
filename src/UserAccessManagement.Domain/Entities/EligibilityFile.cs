using UserAccessManagement.Domain.Commom;
using UserAccessManagement.Domain.Enums;

namespace UserAccessManagement.Domain.Entities;

public class EligibilityFile : Entity<long>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private EligibilityFile()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public EligibilityFile(Guid employerId, string url)
    {
        Url = url;
        EmployerId = employerId;
        Status = EligibilityFileStatus.Processing;
    }

    public string Url { get; private set; }
    public Guid EmployerId { get; private set; }
    public EligibilityFileStatus Status { get; private set; }
    public string? Message { get; private set; }

    public void SetProcessed(string message)
    {
        Message = message;
        ChangeStatus(EligibilityFileStatus.Processed);
    }

    public void SetError(string message)
    {
        Message = message;
        ChangeStatus(EligibilityFileStatus.Error);
    }

    private void ChangeStatus(EligibilityFileStatus status)
    {
        Status = status;
        Stamp();
    }
}