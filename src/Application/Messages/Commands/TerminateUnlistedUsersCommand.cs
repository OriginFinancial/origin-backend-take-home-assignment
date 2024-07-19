using MediatR;

namespace Application.Messages.Commands;

public class TerminateUnlistedUsersCommand : IRequest
{
    public HashSet<string> UserIds { get; }
    
    public string EmployerId { get; set; }

    public TerminateUnlistedUsersCommand(HashSet<string> userIds, string employerId)
    {
        UserIds = userIds;
        EmployerId = employerId;
    }
}