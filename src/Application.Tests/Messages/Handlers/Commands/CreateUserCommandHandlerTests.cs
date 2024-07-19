using Application.Messages.Commands;
using Application.Messages.Handlers.Commands;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using Moq;

namespace Application.Tests.Messages.Handlers.Commands;


[Trait("Category", "Unit")]
public class CreateUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_GivenValidUser_CreatesUserSuccessfully()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var handler = new CreateUserCommandHandler(userServiceClientMock.Object);
        var command = new CreateUserCommand(email: "test@example.com",
            fullName: "FullName",
            password: "Password",
            country: "Country",
            accessType: "AccessType", 
            employerId: "EmployerId",
            birthDate: DateTime.Now,
            salary: 50000);

        userServiceClientMock.Setup(client => client.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        userServiceClientMock.Verify(client => client.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidUser_ReturnsFailure()
    {
        // Arrange
        var userServiceClientMock = new Mock<IUserServiceClient>();
        var handler = new CreateUserCommandHandler(userServiceClientMock.Object);
        var command = new CreateUserCommand(email: "test@example.com",
            fullName: "FullName",
            password: "Password",
            country: "Country",
            accessType: "AccessType", 
            employerId: "EmployerId",
            birthDate: DateTime.Now,
            salary: 50000);

        userServiceClientMock.Setup(client => client.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        userServiceClientMock.Verify(client => client.CreateUserAsync(It.IsAny<CreateUserDto>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}