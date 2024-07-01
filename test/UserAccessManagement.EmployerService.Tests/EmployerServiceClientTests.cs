using UserAccessManagement.EmployerService.Requests;

namespace UserAccessManagement.EmployerService.Tests
{
    public class Tests
    {
        private IEmployerServiceClient _employerServiceClient;

        [SetUp]
        public void Setup()
        {
            // In a real project I would use a mock. Using the "real" client to test the "in memory api".
            _employerServiceClient = new EmployerServiceClient();
        }

        [Test]
        public async Task PostAsync_AddsEmployerSuccessfully()
        {
            // Arrange
            var name = "Emplyer Test";
            var employerRequest = new PostEmployerRequest(name);

            //Act
            var result = await _employerServiceClient.PostAsync(employerRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task GetAsyncById_ReturnsEmployer()
        {
            // Arrange
            var name = "Employer Test";
            var employerRequest = new PostEmployerRequest(name);
            var employer = await _employerServiceClient.PostAsync(employerRequest);
            var employerId = employer!.Id;

            //Act
            var result = await _employerServiceClient.GetAsync(employerId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(employerId));
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo(name));
        }

        [Test]
        public async Task GetAsyncByName_ReturnsEmployer()
        {
            // Arrange
            var name = "Employer Ltd"; // Considering the Employer created int the EmployerServiceClient ctor for testing purposes.

            //Act
            var result = await _employerServiceClient.GetAsync(name);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo(name));
        }
    }
}