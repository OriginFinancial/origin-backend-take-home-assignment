using Application.Messages.Handlers.Queries;
using Application.Messages.Queries;
using Infrastructure.Configuration;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace Application.Tests.Messages.Handlers.Queries;

[Trait("Category", "Unit")]
public class CheckEmployerByEmailQueryHandlerTests
{
    private readonly Mock<IEmployerServiceClient> _mockEmployerServiceClient;
    private readonly CheckEmployerByEmailQueryHandler _handler;
    private readonly AppSettings _appSettings = new AppSettings();

    public CheckEmployerByEmailQueryHandlerTests()
    {
        _mockEmployerServiceClient = new Mock<IEmployerServiceClient>();
        _handler = new CheckEmployerByEmailQueryHandler(_mockEmployerServiceClient.Object, Options.Create(_appSettings));
    }
    
    [Fact]
    public async Task ReturnsEmployerIdRecord_WhenEmployerExists()
    {
        var email = "existing@example.com";
        var expectedRecord = new EmployerIdRecord( "12345");
        _mockEmployerServiceClient.Setup(x => x.GetEmployerByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRecord);

        var result = await _handler.Handle(new CheckEmployerByEmailQuery(email), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedRecord.Id, result?.Id);
    }

    [Fact]
    public async Task ReturnsNull_WhenEmployerDoesNotExist()
    {
        var email = "nonexisting@example.com";
        _mockEmployerServiceClient.Setup(x => x.GetEmployerByEmailAsync(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((EmployerIdRecord?)null);

        var result = await _handler.Handle(new CheckEmployerByEmailQuery(email), CancellationToken.None);

        Assert.Null(result);
    }
}