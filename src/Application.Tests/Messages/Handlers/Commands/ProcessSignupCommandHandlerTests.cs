using Application.Messages.Commands;
using Application.Messages.Handlers.Commands;
using Application.Messages.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using Infrastructure.Records;

namespace Application.Tests.Messages.Handlers.Commands;

[Trait("Category", "Unit")]
public class ProcessSignupCommandHandlerTests 
{
    [Fact]
    public async Task Handle_EmployerDataIsNull_CreatesUser()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<ProcessSignupCommandHandler>>();
        var handler = new ProcessSignupCommandHandler(mediatorMock.Object, loggerMock.Object);
        var command = new ProcessSignupCommand("test@example.com", "Password123!", "Country");

        mediatorMock.Setup(x => x.Send(It.IsAny<CheckEmployerByEmailQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((EmployerIdRecord)null);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserAlreadyExists_LogsError()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<ProcessSignupCommandHandler>>();
        var handler = new ProcessSignupCommandHandler(mediatorMock.Object, loggerMock.Object);
        var command = new ProcessSignupCommand("existing@example.com", "Password123!", "Country");

        mediatorMock.Setup(x => x.Send(It.IsAny<CheckEmployerByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EmployerIdRecord("1234"));
        mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new UserDto());
        
        var capturedLogs = new List<(LogLevel logLevel, Exception exception, string message)>();
        MockLogger(loggerMock, capturedLogs);

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        capturedLogs.Should().ContainSingle(x => x.message == "User with email existing@example.com already exists.");
    }

    private static void MockLogger(Mock<ILogger<ProcessSignupCommandHandler>> loggerMock, List<(LogLevel logLevel, Exception exception, string message)> capturedLogs)
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
    public async Task Handle_InvalidPassword_ThrowsInvalidOperationException()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => x.Send(It.IsAny<CheckEmployerByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EmployerIdRecord("1234"));
        mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(UserDto));

        var loggerMock = new Mock<ILogger<ProcessSignupCommandHandler>>();
        var handler = new ProcessSignupCommandHandler(mediatorMock.Object, loggerMock.Object);
        var command = new ProcessSignupCommand("test@example.com", "short", "Country"); // Invalid password
        var capturedLogs = new List<(LogLevel logLevel, Exception exception, string message)>();
        MockLogger(loggerMock, capturedLogs);


        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        capturedLogs.Should().ContainSingle(x => x.message == "Password does not meet the strength requirements.");
    }

    [Fact]
    public async Task Handle_NullPassword_ThrowsInvalidOperationException()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(x => x.Send(It.IsAny<CheckEmployerByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new EmployerIdRecord("1234"));
        mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(UserDto));
        var loggerMock = new Mock<ILogger<ProcessSignupCommandHandler>>();
        
        var capturedLogs = new List<(LogLevel logLevel, Exception exception, string message)>();
        MockLogger(loggerMock, capturedLogs);

        var handler = new ProcessSignupCommandHandler(mediatorMock.Object, loggerMock.Object);
        var command = new ProcessSignupCommand("test@example.com", null, "Country"); // Null password

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        capturedLogs.Should().ContainSingle(x => x.message == "Password does not meet the strength requirements.");
    }
    
    [Fact]
    public async Task Handle_EmployerNotFound_CreatesUser()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<ProcessSignupCommandHandler>>();
        var handler = new ProcessSignupCommandHandler(mediatorMock.Object, loggerMock.Object);
        var command = new ProcessSignupCommand("newuser@example.com", "Password123!", "Country");

        mediatorMock.Setup(x => x.Send(It.IsAny<CheckEmployerByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((EmployerIdRecord)null); // Employer not found
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true)); // Simulate successful user creation

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(It.Is<CheckEmployerByEmailQuery>(q => q.Email == "newuser@example.com"), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_EmployerFoundByEmail_CreatesUser()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        var loggerMock = new Mock<ILogger<ProcessSignupCommandHandler>>();
        var handler = new ProcessSignupCommandHandler(mediatorMock.Object, loggerMock.Object);
        var command = new ProcessSignupCommand("newuser@example.com", "Password123!", "Country");

        mediatorMock.Setup(x => x.Send(It.IsAny<CheckEmployerByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new EmployerIdRecord("1234")); // Employer found
        mediatorMock.Setup(x => x.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserDto)null); // User does not exist
        mediatorMock.Setup(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true)); // Simulate successful user creation

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        mediatorMock.Verify(x => x.Send(It.Is<CheckEmployerByEmailQuery>(q => q.Email == "newuser@example.com"), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Send(It.IsAny<GetUserByEmailQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
} 