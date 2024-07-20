using Application.Messages.Queries;
using Infrastructure.Records;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Messages.Handlers.Queries;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GetUserByEmailQueryHandler> _logger;

    public GetUserByEmailQueryHandler(HttpClient httpClient, ILogger<GetUserByEmailQueryHandler> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var userResponse = await _httpClient.GetAsync($"users?email={request.Email}", cancellationToken);
        if (!userResponse.IsSuccessStatusCode)
        {
            _logger.LogError("User with email {Email} not found.", request.Email);
            return default;
        }

        var user = await userResponse.Content.ReadAsAsync<UserDto>(cancellationToken);
        return user;
    }
}