using MediatR;
using Microsoft.Extensions.Logging;
using Application.Messages.Commands;
using Infrastructure.Services.Interfaces;

namespace Application.Messages.Handlers.Commands;

public class TerminateUnlistedUsersCommandHandler : IRequestHandler<TerminateUnlistedUsersCommand>
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly ILogger<TerminateUnlistedUsersCommandHandler> _logger;

    public TerminateUnlistedUsersCommandHandler(IUserServiceClient userServiceClient, ILogger<TerminateUnlistedUsersCommandHandler> logger)
    {
        _userServiceClient = userServiceClient;
        _logger = logger;
    }

    /// <summary>
    /// Get the list of users for specific employer, and terminate the users that are not in the list
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    public async Task Handle(TerminateUnlistedUsersCommand request, CancellationToken cancellationToken)
    {
        var existingUserId = await _userServiceClient.GetUserByEmployerIdAsync(request.EmployerId, cancellationToken);
        foreach (var userId in request.UserIds.Where(userId => existingUserId.All(existingUser => existingUser.Id != userId)))
        {
            await _userServiceClient.TerminateUserAsync(userId, cancellationToken);
        }
    }
}