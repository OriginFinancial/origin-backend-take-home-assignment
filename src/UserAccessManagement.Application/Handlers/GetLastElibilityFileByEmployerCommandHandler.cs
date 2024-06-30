using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Application.Models;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.EmployerService;

namespace UserAccessManagement.Application.Handlers;

public class GetLastElibilityFileByEmployerCommandHandler : ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult>
{
    private readonly IEligibilityFileRepository _eligibilityFileRepository;
    private readonly IEmployerServiceClient _employerServiceClient;

    public GetLastElibilityFileByEmployerCommandHandler(IEligibilityFileRepository eligibilityFileRepository, IEmployerServiceClient employerServiceClient)
    {
        _eligibilityFileRepository = eligibilityFileRepository;
        _employerServiceClient = employerServiceClient;
    }

    public async Task<GetLastElibilityFileByEmployerCommandResult> HandleAsync(GetLastElibilityFileByEmployerCommand command, CancellationToken cancellationToken = default)
    {
        var employer = await _employerServiceClient.GetAsync(command.EmployerName, cancellationToken);

        if (employer is null)
        {
            return new GetLastElibilityFileByEmployerCommandResult(false, "Employer not found.", default);
        }

        var eligibilityFile = await _eligibilityFileRepository.GetByEmployerIdAsync(employer.Id, cancellationToken);

        if (eligibilityFile is null)
        {
            return new GetLastElibilityFileByEmployerCommandResult(false, "Eligibility file not found.", default);
        }

        return new GetLastElibilityFileByEmployerCommandResult(true, "Eligibility file retrieved successfully.", new ElibilityFileModel(eligibilityFile));
    }
}