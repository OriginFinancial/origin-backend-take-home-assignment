using Application.Messages.Commands;
using Application.Messages.Handlers.Commands;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.Messages.Handlers.Commands;

[Trait("Category", "Unit")]
public class TerminateUnlistedUsersCommandHandlerTests
{
    
    [Fact]
    public async Task Handle_SuccessfullyTerminatesUsers()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var handler = new TerminateUnlistedUsersCommandHandler(userServiceClientMock.Object, Mock.Of<ILogger<TerminateUnlistedUsersCommandHandler>>());
        var userIds = new HashSet<string> { "user1", "user2" };
        userServiceClientMock.Setup(client => client.TerminateUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        userServiceClientMock.Setup(client => client.GetUserByEmployerIdAsync("1234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserDto>()
            {
                new UserDto()
                {
                    Id = "user123"
                },
                new UserDto()
                {
                    Id = "user2234"
                },
            });
        // Act
        await handler.Handle(new TerminateUnlistedUsersCommand(userIds, "1234"), CancellationToken.None);

        // Assert
        userServiceClientMock.Verify(client => client.TerminateUserAsync(It.IsAny<string>(),
                It.IsAny<CancellationToken>()),
            Times.Exactly(userIds.Count()));
    }

    [Fact]
    public async Task Handle_ThrowsExceptionOnFailure()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var handler = new TerminateUnlistedUsersCommandHandler(userServiceClientMock.Object, Mock.Of<ILogger<TerminateUnlistedUsersCommandHandler>>());
        var userIds = new HashSet<string> { "user1" };
        userServiceClientMock.Setup(client => client.TerminateUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(new TerminateUnlistedUsersCommand(userIds, "1234"), CancellationToken.None));
    }
    
    [Fact]
    public async Task Handle_WithEmptyUserIds_DoesNotCallTerminateUserAsync()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var loggerMock = Mock.Of<ILogger<TerminateUnlistedUsersCommandHandler>>();
        var handler = new TerminateUnlistedUsersCommandHandler(userServiceClientMock.Object, loggerMock);
        var emptyUserIds = new HashSet<string>();

        // Act
        await handler.Handle(new TerminateUnlistedUsersCommand(emptyUserIds, "1234"), CancellationToken.None);

        // Assert
        userServiceClientMock.Verify(client => client.TerminateUserAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}