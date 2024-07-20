using System.Net;
using System.Text;
using Domain.Enums;
using Infrastructure.Configuration;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class UserServiceClient : IUserServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserServiceClient> _logger;
    private readonly AppSettings _appSettings;
    
    public UserServiceClient(IHttpClientFactory httpClientFactory,
        IOptions<AppSettings> appSettings,
        ILogger<UserServiceClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _appSettings = appSettings.Value;
    }
    
    public async Task<UserDto> CheckUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var requestUri = $"{_appSettings.UserServiceBaseUrl}users?email={email}";
        var userResponse = await _httpClient.GetAsync(requestUri, cancellationToken);
        if (!userResponse.IsSuccessStatusCode)
        {
            _logger.LogError("User with email {Email} not found.", email);
            return default;
        }

        var content = await userResponse.Content.ReadAsStringAsync(cancellationToken);
        var user = JsonConvert.DeserializeObject<UserDto>(content);
        return user;
    }

    public async Task<bool> CreateUserAsync(CreateUserDto user, CancellationToken cancellationToken)
    {
        var requestContent = JsonConvert.SerializeObject(user);

        var content = new StringContent(requestContent, Encoding.UTF8, "application/json");

        try
        {
            var uri = $"{_appSettings.UserServiceBaseUrl}/users";
            var response = await _httpClient.PostAsync(uri, content, cancellationToken);

            if (response.StatusCode is HttpStatusCode.Created or HttpStatusCode.OK)
            {
                return true;
            }

            _logger.LogError("Failed to create user. Status code: {StatusCode}", response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while creating user: {Message}", ex.Message);
            return false;
        }
    }

    public async Task<List<UserDto>> GetUserByEmployerIdAsync(string id, CancellationToken cancellationToken)
    {
        var uri = $"{_appSettings.UserServiceBaseUrl}users/employerId={id}";
        var userResponse = await _httpClient.GetAsync(uri, cancellationToken);
        if (!userResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Search for users with EmployerID {Id} with error {StatusCode}.", id, userResponse.StatusCode);
            return default;
        }

        var content = await userResponse.Content.ReadAsStringAsync(cancellationToken);
        var user = JsonConvert.DeserializeObject<List<UserDto>>(content);
        return user;
    }

    public async Task UpdateUserAsync(UserDto user, CancellationToken cancellationToken)
    {
        var updateRequestBody = new[]
        {
            new { field = "country", value = user.Country },
            new { field = "salary", value = user.Salary?.ToString() },
            new { field = "accessType", value = user.AccessType }
        };
        var requestContent = JsonConvert.SerializeObject(updateRequestBody);

        var content = new StringContent(requestContent, Encoding.UTF8, "application/json");

        try
        {
            var uri = $"{_appSettings.UserServiceBaseUrl}/users/{user.Id}";
            var response = await _httpClient.PostAsync(uri, content, cancellationToken);

            if (response.StatusCode is HttpStatusCode.Created or HttpStatusCode.OK)
            {
                return;
            }

            _logger.LogError("Failed to update user. Status code: {StatusCode}", response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while updating user: {Message}", ex.Message);
        }
    }

    public async Task TerminateUserAsync(string userId, CancellationToken cancellationToken)
    {
        var updateRequestBody = new[]
        {
            new { field = "accessType", value = AccessTypeEnum.DTC }
        };
        var requestContent = JsonConvert.SerializeObject(updateRequestBody);

        var content = new StringContent(requestContent, Encoding.UTF8, "application/json");

        try
        {
            var uri = $"{_appSettings.UserServiceBaseUrl}/users/{userId}";
            var response = await _httpClient.PostAsync(uri, content, cancellationToken);

            if (response.StatusCode is HttpStatusCode.Created or HttpStatusCode.OK)
            {
                return;
            }

            _logger.LogError("Failed to terminate user. Status code: {StatusCode}", response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while terminating user: {Message}", ex.Message);
        }
    }
}