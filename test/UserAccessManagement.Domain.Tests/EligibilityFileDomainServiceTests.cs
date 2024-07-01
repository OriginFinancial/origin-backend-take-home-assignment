using Moq;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Domain.Services;
using UserAccessManagement.Domain.Services.Interfaces;
using UserAccessManagement.Infrastructure.Services;
using UserAccessManagement.UserService;
using UserAccessManagement.UserService.Responses;

namespace UserAccessManagement.Domain.Tests;

[TestFixture]
public class EligibilityFileDomainServiceTests
{
    private Mock<IEmployeeRepository> _mockEmployeeRepository;
    private Mock<IEligibilityFileRepository> _mockEligibilityFileRepository;
    private Mock<IEligibilityFileLineRepository> _mockEligibilityFileLineRepository;
    private Mock<IUserServiceClient> _mockUserServiceClient;
    private ICsvService _csvService;
    private IEligibilityFileDomainService _eligibilityFileService;

    [SetUp]
    public void Setup()
    {
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockEligibilityFileRepository = new Mock<IEligibilityFileRepository>();
        _mockEligibilityFileLineRepository = new Mock<IEligibilityFileLineRepository>();
        _mockUserServiceClient = new Mock<IUserServiceClient>();
        _csvService = new CsvService();

        _eligibilityFileService = new EligibilityFileDomainService(
            _csvService,
            _mockEmployeeRepository.Object,
            _mockEligibilityFileRepository.Object,
            _mockEligibilityFileLineRepository.Object,
            _mockUserServiceClient.Object
        );
    }

    [Test]
    public async Task ProccessFileAsync_FileFound_ProcessesSuccessfully()
    {
        // Arrange
        long eligibilityFileId = 1;
        var eligibilityFile = new EligibilityFile(Guid.NewGuid(), "https://ildjfbd.blob.core.windows.net/csv/1employee.csv");
        var existingEmployee = new Employee("employee@example.com", "Employee Test", "US", DateTime.Now, 100000m, Guid.NewGuid(), 1, 1);

        _mockEligibilityFileRepository.Setup(repo => repo.GetByIdAsync(eligibilityFileId, CancellationToken.None))
            .ReturnsAsync(eligibilityFile);

        _mockEligibilityFileRepository.Setup(repo => repo.UpdateAsync(It.IsAny<EligibilityFile>(), CancellationToken.None))
            .ReturnsAsync(eligibilityFile);

        _mockEmployeeRepository.Setup(repo => repo.AddAsync(It.IsAny<Employee>(), CancellationToken.None))
            .ReturnsAsync(existingEmployee);

        _mockUserServiceClient.Setup(client => client.GetAsync("employee@example.com", CancellationToken.None))
            .ReturnsAsync(new UserResponse(Guid.NewGuid(), existingEmployee.Email, existingEmployee.Country, existingEmployee.Salary, string.Empty, existingEmployee.FullName, existingEmployee.EmployerId, existingEmployee.BirthDate));

        _mockUserServiceClient.Setup(client => client.PatchAsync(It.IsAny<UserService.Requests.PatchUserRequest>(), CancellationToken.None))
            .ReturnsAsync(new UserResponse(Guid.NewGuid(), existingEmployee.Email, existingEmployee.Country, existingEmployee.Salary, string.Empty, existingEmployee.FullName, existingEmployee.EmployerId, existingEmployee.BirthDate));

        _mockEmployeeRepository.Setup(repo => repo.FindByEligibilityFileId(eligibilityFileId, CancellationToken.None))
            .ReturnsAsync([existingEmployee]);

        _mockUserServiceClient.Setup(client => client.GetAllByEmployerIdAsync(It.IsAny<Guid>(), CancellationToken.None))
            .ReturnsAsync([
                new UserResponse(Guid.NewGuid(), "terminate@example.com", existingEmployee.Country, existingEmployee.Salary, string.Empty, existingEmployee.FullName, existingEmployee.EmployerId, existingEmployee.BirthDate),
                new UserResponse(Guid.NewGuid(), "employee@example.com", existingEmployee.Country, existingEmployee.Salary, string.Empty, existingEmployee.FullName, existingEmployee.EmployerId, existingEmployee.BirthDate)
            ]);

        _mockEmployeeRepository.Setup(repo => repo.FindByEligibilityFileId(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync([existingEmployee]);

        _mockUserServiceClient.Setup(client => client.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        await _eligibilityFileService.ProccessFileAsync(eligibilityFileId);

        // Assert
        _mockEligibilityFileRepository.Verify(repo => repo.UpdateAsync(It.IsAny<EligibilityFile>(), CancellationToken.None), Times.Once);
        _mockEmployeeRepository.Verify(repo => repo.AddAsync(It.IsAny<Employee>(), CancellationToken.None), Times.Once);
        _mockUserServiceClient.Verify(client => client.PatchAsync(It.IsAny<UserService.Requests.PatchUserRequest>(), CancellationToken.None), Times.Once);
        _mockUserServiceClient.Verify(client => client.DeleteAsync(It.IsAny<Guid>(), CancellationToken.None), Times.Once);
    }
}