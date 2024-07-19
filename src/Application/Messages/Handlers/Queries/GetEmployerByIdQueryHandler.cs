using Application.Messages.Queries;
using MediatR;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;

namespace Application.Messages.Handlers.Queries;

public class GetEmployerByIdQueryHandler : IRequestHandler<GetEmployerByIdQuery, EmployerDto?>
{
    private readonly IEmployerServiceClient _employerService;

    public GetEmployerByIdQueryHandler(IEmployerServiceClient employerService)
    {
        _employerService = employerService;
    }

    public async Task<EmployerDto?> Handle(GetEmployerByIdQuery request, CancellationToken cancellationToken)
    {
        var employer = await _employerService.GetEmployerByIdAsync(request.Id, cancellationToken);
        if (employer == null) return null;

        return new EmployerDto
        {
            Id = employer.Id,
            FullName = employer.FullName,
            Email = employer.Email,
            BirthDate = employer.BirthDate,
            Salary = employer.Salary
        };
    }
}
