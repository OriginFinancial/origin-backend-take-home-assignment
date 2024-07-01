using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Application.Models;

public class EligibilityFileModel : EntityModel<long>
{
    public EligibilityFileModel(EligibilityFile eligibilityFile) : base(eligibilityFile)
    {
        Url = eligibilityFile.Url;
        EmployerId = eligibilityFile.EmployerId;
        Status = eligibilityFile.Status.ToString();
        Message = eligibilityFile.Message;
    }

    public string Url { get; private set; }
    public Guid EmployerId { get; private set; }
    public string Status { get; private set; }
    public string? Message { get; private set; }
}