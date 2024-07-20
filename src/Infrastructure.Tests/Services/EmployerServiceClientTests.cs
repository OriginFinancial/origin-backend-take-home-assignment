
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using FluentAssertions;
using Infrastructure.Configuration;
using Infrastructure.Records;
using Infrastructure.Services;

namespace Infrastructure.Tests.Services;

[Trait("Category", "Unit")]
public class EmployerServiceClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
    private readonly EmployerServiceClient _employerServiceClient;
    private readonly Mock<IOptions<AppSettings>> _mockOptions = new();
    private readonly Mock<ILogger<EmployerServiceClient>> _mockLogger = new();

    public EmployerServiceClientTests()
    {
        _mockOptions.Setup(o => o.Value).Returns(new AppSettings { EmployerServiceBaseUrl = "http://localhost" });
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        _employerServiceClient = new EmployerServiceClient(httpClientFactory.Object, _mockOptions.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetEmployerByIdAsync_ReturnsEmployer_WhenSuccessful()
    {
        // Arrange
        var expectedEmployer = new EmployerDto
        {
            Id = "123",
            Email = "test@example.com"

        };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedEmployer))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _employerServiceClient.GetEmployerByIdAsync("123", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("123", result.Id);
        Assert.Equal("test@example.com", result.Email);
    }
    
    [Fact]
    public async Task GetEmployerByIdAsync_ReturnsNull_WhenNotFound()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _employerServiceClient.GetEmployerByIdAsync("nonexistent-id", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task CreateEmployerAsync_CompletesSuccessfully_WhenCalledWithValidData()
    {
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var exception = await Record.ExceptionAsync(() => _employerServiceClient.CreateEmployerAsync("test@example.com", CancellationToken.None));

        Assert.Null(exception); // No exception should be thrown for a successful operation
    }
    
    [Fact]
    public async Task CreateEmployerAsync_LogsError_WhenBadRequest()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var expectedMessage = "Failed to save employer by email. Status code:";
        
        var capturedLogs = new List<(LogLevel logLevel, Exception exception, string message)>();
        MockLogger(_mockLogger, capturedLogs);


        // Act
        await _employerServiceClient.CreateEmployerAsync("invalid@example.com", CancellationToken.None);

        // Assert
        capturedLogs.Should().Satisfy(x => x.message.StartsWith(expectedMessage));
    }
    private static void MockLogger(Mock<ILogger<EmployerServiceClient>> loggerMock, List<(LogLevel logLevel, Exception exception, string message)> capturedLogs)
    {
        loggerMock.Setup(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>())
        ).Callback<LogLevel, EventId, object, Exception, object>((level, eventId, state, exception, formatter) =>
        {
            var logMessage = state.ToString();
            capturedLogs.Add((level, exception, logMessage));
        });
    }
    [Fact]
    public async Task GetEmployerByEmailAsync_ReturnsEmployer_WhenSuccessful()
    {
        var expectedEmployer = new EmployerIdRecord("123");
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedEmployer))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _employerServiceClient.GetEmployerByEmailAsync("test@example.com", CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("123", result.Id); 
    }

    [Fact]
    public async Task GetEmployerByEmailAsync_ReturnsNull_WhenNotFound()
    {
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        var result = await _employerServiceClient.GetEmployerByEmailAsync("notfound@example.com", CancellationToken.None);

        Assert.Null(result);
    }
}