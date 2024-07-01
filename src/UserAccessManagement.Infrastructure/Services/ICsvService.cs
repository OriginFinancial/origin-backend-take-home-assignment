using UserAccessManagement.Infrastructure.Csv;

namespace UserAccessManagement.Infrastructure.Services;

public interface ICsvService
{
    IAsyncEnumerable<EligibilityFileCsvLine> ParseCsvAsync(Stream csvStream);
    Task DownloadCsvAsync(string csvUrl, Func<Stream, Task> fnProcessFile);
}