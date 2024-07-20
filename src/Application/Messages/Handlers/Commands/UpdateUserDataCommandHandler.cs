using Application.Messages.Commands;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Messages.Handlers.Commands;

public class UpdateUserDataCommandHandler : IRequestHandler<UpdateUserDataCommand, bool>
{
    private readonly ILogger<UpdateUserDataCommandHandler> _logger;
    private readonly IUserServiceClient _userServiceClient;

    public UpdateUserDataCommandHandler(HttpClient httpClient,
        ILogger<UpdateUserDataCommandHandler> logger,
        IUserServiceClient userServiceClient)
    {
        _logger = logger;
        _userServiceClient = userServiceClient;
    }

    public async Task<bool> Handle(UpdateUserDataCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userServiceClient.CheckUserByEmailAsync(request.Email, cancellationToken);
            if (user == null) return false;

            var updateData = new UserDto()
            {
                Id = user.Id,
                Email = request.Email,
                Country = request.Country,
                AccessType = string.IsNullOrWhiteSpace(request.AccessType) ? user.AccessType : request.AccessType,
            };
            await _userServiceClient.UpdateUserAsync(updateData, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user data for {Email}", request.Email);
            return false;
        }
    }
}