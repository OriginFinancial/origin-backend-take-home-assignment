using UserAccessManagement.Domain.Commom;
using UserAccessManagement.Domain.Enums;

namespace UserAccessManagement.Domain.Entities;

public class EligibilityFileLine : Entity<long>
{
    private EligibilityFileLine()
    {
    }

    public EligibilityFileLine(string content, EligibilityFileLineType lineType, long eligibilityFileId, EligibilityFile eligibilityFile)
    {
        Content = content;
        LineType = lineType;
        EligibilityFileId = eligibilityFileId;
        EligibilityFile = eligibilityFile;
    }

    public string Content { get; private set; }
    public EligibilityFileLineType LineType { get; private set; }
    public long EligibilityFileId { get; private set; }
    public EligibilityFile EligibilityFile { get; private set; }
}