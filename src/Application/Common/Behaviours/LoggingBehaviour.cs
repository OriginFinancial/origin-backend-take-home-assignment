namespace Application.Common.Behaviours;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handling {Name} with request: {request} - started.", typeof(TRequest).Name,
                request);

            var response = await next();

            _logger.LogInformation("Handled {Name} - done.", typeof(TRequest).Name);
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error handling {Name}.", typeof(TRequest).Name);
            throw;
        }

    }
}