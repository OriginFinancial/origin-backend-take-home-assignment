using Microsoft.Extensions.Options;
using Application.Messages.Queries;
using Infrastructure.Configuration;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Application.Messages.Handlers.Queries;

public class CheckEmployerByEmailQueryHandler : IRequestHandler<CheckEmployerByEmailQuery, EmployerIdRecord?>
{
    private readonly IEmployerServiceClient _employerService;
    private readonly AppSettings _appSettings;

    public CheckEmployerByEmailQueryHandler(IEmployerServiceClient employerService, IOptions<AppSettings> appSettings)
    {
        _employerService = employerService;
        _appSettings = appSettings.Value;
    }

    public async Task<EmployerIdRecord?> Handle(CheckEmployerByEmailQuery request, CancellationToken cancellationToken)
    {
        var response = await _employerService.GetEmployerByEmailAsync(request.Email, cancellationToken);

        return response;
    }
}