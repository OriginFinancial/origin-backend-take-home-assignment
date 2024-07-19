using System.Text;
using Domain.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class EmployerServiceClient : IEmployerServiceClient
{
    private readonly AppSettings _appSettings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<EmployerServiceClient> _logger;
    public EmployerServiceClient(IHttpClientFactory httpClientFactory, IOptions<AppSettings> appSettings, ILogger<EmployerServiceClient> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
        _appSettings = appSettings.Value;
    }

    public async Task<IPerson?> GetEmployerByIdAsync(string id, CancellationToken cancellationToken)
    {
        var url = $"{_appSettings.EmployerServiceBaseUrl}/employers/{id}";
        var response = await _httpClient.GetAsync(url, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get employer by Id. Status code: {StatusCode}",response.StatusCode);
            return default; // Or handle errors as appropriate
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        var employerIdRecord = JsonConvert.DeserializeObject<EmployerDto>(content);
        return employerIdRecord;
    }

    public async Task<EmployerIdRecord> GetEmployerByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var url = $"{_appSettings.EmployerServiceBaseUrl}/employers?email={email}";
        try
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get employer by email. Status code: {StatusCode}", response.StatusCode);
                return default; // Or handle errors as appropriate
            }

            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var employerIdRecord = JsonConvert.DeserializeObject<EmployerIdRecord>(content);
            return employerIdRecord;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on fetching employer by email {Email}: {Message}", email, e.Message);
            throw;
        }
    }

    public async Task CreateEmployerAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"{_appSettings.EmployerServiceBaseUrl}/employers";
            var employerRecord = new EmployerIdRecord(email);
            var content = new StringContent(JsonConvert.SerializeObject(employerRecord),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync(url, content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to save employer by email. Status code: {StatusCode}", response.StatusCode);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error on creating employer by email {Email}: {Message}", email, e.Message);
            throw;
        }
    }
}