using UserAccessManagement.UserService.Requests;

namespace UserAccessManagement.UserService.Tests;

public class UserServiceClientTest
{
    private IUserServiceClient _userServiceClient;
    private PostUserRequest _postUserDtcRequest;
    private PostUserRequest _postUserEmployerRequest;

    [SetUp]
    public void Setup()
    {
        // In a real project I would use a mock. Using the "real" client to test the "in memory api".
        _userServiceClient = new UserServiceClient();

        _postUserDtcRequest = new PostUserRequest("test@dtc.com", "password", "CA", 5000, "dtc", "Full Name", default, new DateTime(2000, 1, 1));
        _postUserEmployerRequest = new PostUserRequest("test@employer.com", "password", "US", 10000, "employer", "Full Name", Guid.NewGuid(), new DateTime(2000, 1, 1));
    }

    [Test]
    public async Task PostAsync_DtcUser_AddsUserSuccessfully()
    {
        // Arrange
        var accessType = "dtc";
        var email = _postUserDtcRequest.Email;

        // Act
        var result = await _userServiceClient.PostAsync(_postUserDtcRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.EmployerId, Is.Null);
        Assert.That(result.AccessType, Is.EqualTo(accessType));
        Assert.That(result.Email, Is.EqualTo(email));
    }

    [Test]
    public async Task PostAsync_EmployerUser_AddsUserSuccessfully()
    {
        // Arrange
        var accessType = "employer";
        var email = _postUserEmployerRequest.Email;

        // Act
        var result = await _userServiceClient.PostAsync(_postUserEmployerRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.EmployerId, Is.Not.Null);
        Assert.That(result.AccessType, Is.EqualTo(accessType));
        Assert.That(result.Email, Is.EqualTo(email));
    }

    [Test]
    public async Task GetAsyncById_ReturnsUser()
    {
        // Arrange
        var user = await _userServiceClient.PostAsync(_postUserEmployerRequest);
        var id = user!.Id;

        // Act
        var result = await _userServiceClient.GetAsync(id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetAsyncByEmail_ReturnsUser()
    {
        // Arrange
        var email = _postUserEmployerRequest.Email;
        _ = await _userServiceClient.PostAsync(_postUserEmployerRequest);

        // Act
        var result = await _userServiceClient.GetAsync(email);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
        Assert.That(result.Email, Is.EqualTo(email));
    }

    [Test]
    public async Task PatchAsync_UpdatesUserSuccessfully()
    {
        // Arrange
        var salary = 70000m;
        var country = "BR";

        var user = await _userServiceClient.PostAsync(_postUserEmployerRequest);
        var userId = user!.Id;

        var patchRequest = new PatchUserRequest(userId).Build(country, salary);

        // Act
        var result = await _userServiceClient.PatchAsync(patchRequest);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(userId));
        Assert.That(result.Salary, Is.EqualTo(salary));
        Assert.That(result.Country, Is.EqualTo(country));
    }

    [Test]
    public async Task DeleteAsync_RemovesUserSuccessfully()
    {
        // Arrange
        var email = _postUserDtcRequest.Email;
        var user = await _userServiceClient.PostAsync(_postUserDtcRequest);
        var userId = user!.Id;

        // Act
        await _userServiceClient.DeleteAsync(userId);

        // Assert
        var getUserResult = await _userServiceClient.GetAsync(userId);
        var getUserByEmailResult = await _userServiceClient.GetAsync(email);

        Assert.IsNull(getUserResult);
        Assert.IsNull(getUserByEmailResult);
    }
}