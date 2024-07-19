using System.Net;
using System.Text;
using Application.Messages.Commands;
using Application.Messages.Handlers.Commands;
using Application.Messages.Queries;
using Application.Tests.Factories;
using Domain.Enums;
using Infrastructure.Records;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;

namespace Application.Tests.Messages.Handlers.Commands;

[Trait("Category", "Unit")]
public class ProcessEligibilityFileCommandHandlerTests
{
    [Fact]
    public async Task Handle_EmptyFile_ReturnsEmptyResult()
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var emptyStream = new MemoryStream(Encoding.UTF8.GetBytes("")); // Empty content
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(emptyStream)
            });

        var loggerMock = new Mock<ILogger<ProcessEligibilityFileCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessEligibilityFileCommandHandler(httpClientFactoryMock.Object, loggerMock.Object, mediatorMock.Object);

        // Act
        var result = await handler.Handle(new ProcessEligibilityFileCommand("http://example.com/empty.csv", "employerName"), CancellationToken.None);

        // Assert
        Assert.Empty(result.ProcessedLines);
        Assert.Empty(result.NonProcessedLines);
        Assert.Empty(result.Errors);
    }
    
    
    [Fact]
    public async Task Handle_DownloadFails_ThrowsException()
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound // Simulate failure
            });

        var loggerMock = new Mock<ILogger<ProcessEligibilityFileCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();

        var handler = new ProcessEligibilityFileCommandHandler(httpClientFactoryMock.Object, loggerMock.Object, mediatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => handler.Handle(new ProcessEligibilityFileCommand("http://example.com/nonexistent.csv", "employerName"), CancellationToken.None));
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(54)]
    [InlineData(13)]
    public async Task Handle_NonEmptyFile_ProcessesDataCorrectly(int countOfRecords)
    {
        // Arrange
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var loggerMock = new Mock<ILogger<ProcessEligibilityFileCommandHandler>>();
        var mediatorMock = new Mock<IMediator>();
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var (csvContent, lineModels) = CsvContentFactory.GenerateCsvContent(countOfRecords);
        foreach (var lineModel in lineModels)
        {
            mediatorMock.Setup(x => x.Send(It.Is<GetUserByEmailQuery>(q => q.Email == lineModel.Email), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new UserDto()
                {
                    Email = lineModel.Email,
                    Country = lineModel.Country,
                    Salary = lineModel.Salary,
                    AccessType = AccessTypeEnum.Employer,
                    Id = Guid.NewGuid().ToString(),
                    BirthDate = DateTime.Now.AddYears(-30),
                    FullName = lineModel.FullName,
                    EmployerId = Guid.NewGuid().ToString()
                });
        }
        var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StreamContent(csvStream)
            });


        var handler = new ProcessEligibilityFileCommandHandler(httpClientFactoryMock.Object, loggerMock.Object, mediatorMock.Object);

        // Act
        var result = await handler.Handle(new ProcessEligibilityFileCommand("http://example.com/nonempty.csv", "employerName"), CancellationToken.None);

        // Assert
        Assert.NotEmpty(result.ProcessedLines);
        Assert.Empty(result.NonProcessedLines);
        Assert.Empty(result.Errors);
        mediatorMock.Verify(x => x.Send(
                It.IsAny<TerminateUnlistedUsersCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    
    }
}