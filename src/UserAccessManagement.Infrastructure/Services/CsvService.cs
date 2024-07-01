using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using UserAccessManagement.Infrastructure.Csv;
using UserAccessManagement.Infrastructure.Exceptions;

namespace UserAccessManagement.Infrastructure.Services;

public sealed class CsvService : ICsvService
{
    private readonly HttpClient _httpClient;

    public CsvService()
    {
        _httpClient = new();
    }

    public async IAsyncEnumerable<EligibilityFileCsvLine> ParseCsvAsync(Stream csvStream)
    {
        using var streamReader = new StreamReader(csvStream);
        using var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ",",
        });

        while (await csvReader.ReadAsync())
        {
            EligibilityFileCsvLine line;

            try
            {
                line = csvReader.GetRecord<EligibilityFileCsvLine>();
            }
            catch (Exception ex)
            {
                line = new EligibilityFileCsvLine { ErrorMessage = ex.Message };
            }

            yield return line;
        }
    }

    public async Task DownloadCsvAsync(string csvUrl, Func<Stream, Task> fnProcessFile)
    {
        using var response = await _httpClient.GetAsync(csvUrl, HttpCompletionOption.ResponseHeadersRead);

        response.EnsureSuccessStatusCode();

        var contentType = response.Content.Headers.ContentType?.MediaType;

        if (string.IsNullOrWhiteSpace(contentType) || !contentType.Contains("text/csv"))
        {
            throw new BusinessException("The file is not a valid CSV");
        }

        using var stream = await response.Content.ReadAsStreamAsync();

        await fnProcessFile(stream);
    }
}