using System.Text;
using Application.DTOs;
using Application.Messages.Commands;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Application.Messages.Queries;
using Domain.Enums;
using Infrastructure.Records;

namespace Application.Messages.Handlers.Commands;

public class ProcessEligibilityFileCommandHandler : IRequestHandler<ProcessEligibilityFileCommand, EligibilityProcessResult>
{
    private readonly IMediator _mediator;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProcessEligibilityFileCommandHandler> _logger;

    public ProcessEligibilityFileCommandHandler(IHttpClientFactory httpClientFactory,
        ILogger<ProcessEligibilityFileCommandHandler> logger,
        IMediator mediator)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<EligibilityProcessResult> Handle(ProcessEligibilityFileCommand request, CancellationToken cancellationToken)
    {
        // Step 1: Download the CSV file from the provided URL
        var (result ,hashUsers) = await DownloadCsv(request.CsvFileUrl);
        
        await TerminateUnlistedUsers(hashUsers, request.EmployerName);
        return result;
    }

    private async Task<(EligibilityProcessResult, HashSet<string>)> DownloadCsv(string requestCsvFileUrl)
    {
        var result = new EligibilityProcessResult();
        var hashUsers = new HashSet<string>();
        try
        {
            var response = await _httpClient.GetAsync(requestCsvFileUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var firstLine = true;
            var index = 0;
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (firstLine) // Assuming the first line is headers
                    {
                        firstLine = false;
                        continue;
                    }
                    index++;

                    await ProcessLine(line, result, hashUsers, index);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            throw;
        }

        return (result, hashUsers);
    }

    private async Task ProcessLine(
        string? line, EligibilityProcessResult result, HashSet<string> hashUsers, int index)
    {
        var processedLineModels = ParseCsvLines(line);
        foreach (var processedLineModel in processedLineModels)   
        {
            try
            { 
                var user = await GetUser(processedLineModel.Email);
                if (user != null)
                {
                    await UpdateUserData(processedLineModel, user);
                    hashUsers.Add(user.Id);
                }
                else result.NonProcessedLines.Add(index);
                
                result.ProcessedLines.Add(index);
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
            }
        }
    }

    private async Task TerminateUnlistedUsers(HashSet<string> hashUserIds, string employerId)
    {
        await _mediator.Send(new TerminateUnlistedUsersCommand(hashUserIds, employerId));
    }

    private async Task UpdateUserData(CsvLineModel line, UserDto user)
    {
        await _mediator.Send(new UpdateUserDataCommand(user.Email, line.Country, line.Salary, AccessTypeEnum.Employer));
    }

    private async Task<UserDto?> GetUser(string lineEmail)
    {
        return await _mediator.Send(new GetUserByEmailQuery(lineEmail));
    }
    
    private IEnumerable<CsvLineModel> ParseCsvLines(string? csvData)
    {
        if (string.IsNullOrWhiteSpace(csvData)) return Enumerable.Empty<CsvLineModel>();
        var lines = csvData.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        var csvLineModels = new List<CsvLineModel>();

        foreach (var line in lines) 
        {
            var values = SplitCsvLine(line);
            var csvLineModel = new CsvLineModel
            {
                Email = values[0],
                FullName = values.Length > 1 ? values[1] : null,
                Country = values.Length > 2 ? values[2] : null,
                BirthDate = values.Length > 3 ? values[3] : null,
                Salary = values.Length > 4 && decimal.TryParse(values[4], out var salary) ? salary : null
            };
            csvLineModels.Add(csvLineModel);
        }

        return csvLineModels;
    }

    private string?[] SplitCsvLine(string line)
    {
        var values = new List<string>();
        var column = new StringBuilder();
        bool inQuotes = false;

        foreach (var character in line)
        {
            if (character == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (character == ',' && !inQuotes)
            {
                values.Add(column.ToString());
                column.Clear();
            }
            else
            {
                column.Append(character);
            }
        }

        values.Add(column.ToString()); // Add the last column
        return values.ToArray();
    }
}