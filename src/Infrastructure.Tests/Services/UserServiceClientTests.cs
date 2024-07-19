using Infrastructure.Records;
using Infrastructure.Services;
using Newtonsoft.Json;
using Moq;
using Moq.Protected;
using System.Net;
using Infrastructure.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace Infrastructure.Tests.Services;

[Trait("Category", "Unit")]
public class UserServiceClientTests
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
    private readonly UserServiceClient _userServiceClient;
    private readonly Mock<ILogger<UserServiceClient>> _mockLogger = new();
    private readonly Mock<IOptions<AppSettings>> _mockOptions = new();

    public UserServiceClientTests()
    {
        _mockOptions.Setup(o => o.Value).Returns(new AppSettings { UserServiceBaseUrl = "http://localhost" });
        var httpClient = new HttpClient(_mockHttpMessageHandler.Object);
        var httpClientFactory = new Mock<IHttpClientFactory>();
        httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        _userServiceClient = new UserServiceClient(httpClientFactory.Object, _mockOptions.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task CheckUserByEmailAsync_ReturnsUser_WhenSuccessful()
    {
        // Arrange
        var expectedUser = new UserDto { Email = "test@example.com" };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedUser))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _userServiceClient.CheckUserByEmailAsync("test@example.com", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task CheckUserByEmailAsync_ReturnsNull_WhenNotFound()
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
        var result = await _userServiceClient.CheckUserByEmailAsync("notfound@example.com", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
     
    [Fact]
    public async Task CreateUserAsync_ReturnsTrue_WhenSuccessful()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Email = "success@example.com" };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _userServiceClient.CreateUserAsync(createUserDto, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateUserAsync_ReturnsFalse_WhenBadRequest()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Email = "fail@example.com" };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _userServiceClient.CreateUserAsync(createUserDto, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public async Task CreateUserAsync_LogsError_WhenExceptionOccurs()
    {
        // Arrange
        var exceptionMessage = "An error occurred during user creation";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(exceptionMessage));

        _mockLogger.Setup(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred while creating user")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));

        var createUserDto = new CreateUserDto { Email = "error@example.com" };

        // Act
        _userServiceClient.CreateUserAsync(createUserDto, CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred while creating user") && v.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
    
    [Fact]
    public async Task GetUserByEmployerIdAsync_ReturnsUsers_WhenSuccessful()
    {
        // Arrange
        var expectedUsers = new List<UserDto> { new UserDto { Email = "test@example.com" } };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonConvert.SerializeObject(expectedUsers))
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        var result = await _userServiceClient.GetUserByEmployerIdAsync("valid-id", CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("test@example.com", result[0].Email);
    }

    [Fact]
    public async Task GetUserByEmployerIdAsync_ReturnsNull_WhenNotFound()
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
        var result = await _userServiceClient.GetUserByEmployerIdAsync("invalid-id", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    
    [Fact]
    public async Task UpdateUserAsync_Success()
    {
        // Arrange
        var userDto = new UserDto { Id = "1", Email = "success@example.com" };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act & Assert
        await _userServiceClient.UpdateUserAsync(userDto, CancellationToken.None);
    }

    [Fact]
    public async Task UpdateUserAsync_LogsError_WhenExceptionOccurs()
    {
        // Arrange
        var exceptionMessage = "An error occurred";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(exceptionMessage));

        _mockLogger.Setup(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred while updating user")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));

        // Act
        await _userServiceClient.UpdateUserAsync(new UserDto(), CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred while updating user") && v.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
    [Fact]
    public async Task UpdateUserAsync_ReturnsFalse_WhenBadRequest()
    {
        // Arrange
        var userDto = new UserDto { Id = "1", Email = "fail@example.com" };
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        await _userServiceClient.UpdateUserAsync(userDto, CancellationToken.None);

        //Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to update user. Status code: BadRequest")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
    
    [Fact]
    public async Task TerminateUserAsync_Success()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act & Assert
        await _userServiceClient.TerminateUserAsync("valid-user-id", CancellationToken.None);
    }

    [Fact]
    public async Task TerminateUserAsync_LogsError_WhenBadRequest()
    {
        // Arrange
        var httpResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(httpResponse);

        // Act
        await _userServiceClient.TerminateUserAsync("invalid-user-id", CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to terminate user. Status code: BadRequest")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public async Task TerminateUserAsync_LogsError_WhenExceptionOccurs()
    {
        // Arrange
        var exceptionMessage = "An error occurred";
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(exceptionMessage));

        _mockLogger.Setup(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred while terminating user")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()));

        // Act
        await _userServiceClient.TerminateUserAsync("any-user-id", CancellationToken.None);

        // Assert
        _mockLogger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred while terminating user") && v.ToString().Contains(exceptionMessage)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }
}