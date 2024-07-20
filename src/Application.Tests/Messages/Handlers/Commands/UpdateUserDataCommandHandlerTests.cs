using Application.Messages.Commands;
using Application.Messages.Handlers.Commands;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Messages.Handlers.Commands;

[Trait("Category", "Unit")]
public class UpdateUserDataCommandHandlerTests
{
    [Fact]
    public async Task Handle_UserNotFoundByEmail_ReturnsFalse()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var loggerMock = Mock.Of<ILogger<UpdateUserDataCommandHandler>>();
        var handler = new UpdateUserDataCommandHandler(new HttpClient(), loggerMock, userServiceClientMock.Object);
        var command = new UpdateUserDataCommand(email: "nonexistent@example.com",  country: "Country", salary: 5000,  accessType: "AccessType");

        userServiceClientMock.Setup(client => client.CheckUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((UserDto)null);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_FailureToUpdateUser_ReturnsFalse()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var loggerMock = new Mock<ILogger<UpdateUserDataCommandHandler>>();
        var handler = new UpdateUserDataCommandHandler(new HttpClient(), loggerMock.Object, userServiceClientMock.Object);
        var command = new UpdateUserDataCommand(email: "test@example.com",  country: "Country", salary: 5000,  accessType: "AccessType");
        var userDto = new UserDto { Id = "1", Email = "test@example.com" };
        var capturedLogs = new List<(LogLevel, Exception, string)>();

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
        userServiceClientMock.Setup(client => client.CheckUserByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(userDto);
        userServiceClientMock.Setup(client => client.UpdateUserAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception());

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        userServiceClientMock.Verify(client => client.CheckUserByEmailAsync("test@example.com", It.IsAny<CancellationToken>()), Times.Once);
        userServiceClientMock.Verify(client => client.UpdateUserAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Contains(capturedLogs, log => log.Item3.Contains("Error updating user data for test@example.com") && log.Item2 != null);

    }
    
    [Fact]
    public async Task Handle_SuccessfullyUpdatesUser_ReturnsTrue()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var loggerMock = new Mock<ILogger<UpdateUserDataCommandHandler>>();
        var handler = new UpdateUserDataCommandHandler(new HttpClient(), loggerMock.Object, userServiceClientMock.Object);
        var command = new UpdateUserDataCommand(email: "existing@example.com", country: "NewCountry", salary: 6000, accessType: "NewAccessType");
        var userDto = new UserDto { Id = "1", Email = "existing@example.com" };

        userServiceClientMock.Setup(client => client.CheckUserByEmailAsync("existing@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(userDto);
        userServiceClientMock.Setup(client => client.UpdateUserAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        userServiceClientMock.Verify(client => client.CheckUserByEmailAsync("existing@example.com", It.IsAny<CancellationToken>()), Times.Once);
        userServiceClientMock.Verify(client => client.UpdateUserAsync(It.IsAny<UserDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}