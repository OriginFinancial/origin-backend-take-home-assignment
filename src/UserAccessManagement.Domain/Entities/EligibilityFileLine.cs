using UserAccessManagement.Domain.Commom;
using UserAccessManagement.Domain.Enums;

namespace UserAccessManagement.Domain.Entities;

public class EligibilityFileLine : Entity<long>
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private EligibilityFileLine()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
    }

    public EligibilityFileLine(string content, EligibilityFileLineType lineType, long eligibilityFileId)
    {
        Content = content;
        LineType = lineType;
        EligibilityFileId = eligibilityFileId;
    }

    public string Content { get; private set; }
    public EligibilityFileLineType LineType { get; private set; }
    public long EligibilityFileId { get; private set; }
}