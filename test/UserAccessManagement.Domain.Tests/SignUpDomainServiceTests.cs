using Moq;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Domain.Services;
using UserAccessManagement.Domain.Services.Interfaces;
using UserAccessManagement.Infrastructure.Exceptions;
using UserAccessManagement.UserService;
using UserAccessManagement.UserService.Requests;
using UserAccessManagement.UserService.Responses;

namespace UserAccessManagement.Domain.Tests;

public class SignUpDomainServiceTests
{
    private ISignUpDomainService _signUpDomainService;
    private Mock<IEmployeeRepository> _mockEmployeeRepository;
    private Mock<IUserServiceClient> _mockUserServiceClient;

    [SetUp]
    public void Setup()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockUserServiceClient = new Mock<IUserServiceClient>();
        _signUpDomainService = new SignUpDomainService(_mockEmployeeRepository.Object, _mockUserServiceClient.Object);
    }

    [Test]
    public async Task SignUpAsync_NewUser_Success()
    {
        // Arrange
        var email = "newuser@example.com";
        var newUser = new User(email, "password", "US", "Test User", DateTime.Now, 100000m);

        _mockEmployeeRepository.Setup(repo => repo.GetAsync(newUser.Email, CancellationToken.None))
            .ReturnsAsync((Employee?)null);

        _mockUserServiceClient.Setup(client => client.GetAsync(newUser.Email, CancellationToken.None))
            .ReturnsAsync((UserResponse?)null);

        _mockUserServiceClient.Setup(client => client.PostAsync(It.IsAny<PostUserRequest>(), CancellationToken.None))
            .ReturnsAsync(new UserResponse(Guid.NewGuid(), email, newUser.Country, newUser.Salary, string.Empty, newUser.FullName, newUser.EmployerId, newUser.BirthDate));

        // Act
        var result = await _signUpDomainService.SignUpAsync(newUser);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo(newUser.Email));
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void SignUpAsync_UserWithEmailAlreadyExists_ExceptionThrown()
    {
        // Arrange
        var email = "existinguser@example.com";
        var existingUser = new User(email, "password", "US", "Test User", DateTime.Now, 100000m);

        _mockEmployeeRepository.Setup(repo => repo.GetAsync(existingUser.Email, CancellationToken.None))
            .ReturnsAsync((Employee?)null);

        _mockUserServiceClient.Setup(client => client.GetAsync(existingUser.Email, CancellationToken.None))
            .ReturnsAsync(new UserResponse(Guid.NewGuid(), email, existingUser.Country, existingUser.Salary, string.Empty, existingUser.FullName, existingUser.EmployerId, existingUser.BirthDate));

        // Act and Assert
        Assert.ThrowsAsync<BusinessException>(async () => await _signUpDomainService.SignUpAsync(existingUser));
    }

    [Test]
    public async Task SignUpAsync_UserAssociatedWithEmployee_Success()
    {
        // Arrange
        var email = "employee@example.com";
        var country = "US";
        var salary = 100000m;
        var existingEmployee = new Employee(email, "Employee Test", country, DateTime.Now, salary, Guid.NewGuid(), 1, 1);
        var newUser = new User(email, "password", "BR", "Test User", DateTime.Now, 50000m);

        _mockEmployeeRepository.Setup(repo => repo.GetAsync(email, CancellationToken.None))
            .ReturnsAsync(existingEmployee);

        _mockUserServiceClient.Setup(client => client.GetAsync(newUser.Email, CancellationToken.None))
           .ReturnsAsync((UserResponse?)null);

        _mockUserServiceClient.Setup(client => client.PostAsync(It.IsAny<PostUserRequest>(), CancellationToken.None))
            .ReturnsAsync(new UserResponse(Guid.NewGuid(), email, country, salary, string.Empty, newUser.FullName, newUser.EmployerId, newUser.BirthDate));

        // Act
        var result = await _signUpDomainService.SignUpAsync(newUser, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo(email));
        Assert.That(result.Country, Is.EqualTo(country));
        Assert.That(result.Salary, Is.EqualTo(salary));
    }
}