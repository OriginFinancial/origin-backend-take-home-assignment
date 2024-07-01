using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Application.Models;

public class EligibilityFileLineModel
{
    public EligibilityFileLineModel(EligibilityFileLine eligibilityFileLine)
    {
        Content = eligibilityFileLine.Content;
        Status = eligibilityFileLine.LineType.ToString();
    }

    public string Content { get; private set; }
    public string Status { get; private set; }
}