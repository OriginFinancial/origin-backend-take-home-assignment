using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Domain.Enums;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.EmployerService;

namespace UserAccessManagement.Application.Handlers;

public class GetLastElibilityFileReportByEmployerCommandHandler : ICommandHandler<GetLastElibilityFileReportByEmployerCommand, GetLastElibilityFileReportByEmployerCommandResult>
{
    private readonly IEligibilityFileRepository _eligibilityFileRepository;
    private readonly IEligibilityFileLineRepository _eligibilityFileLineRepository;
    private readonly IEmployerServiceClient _employerServiceClient;

    public GetLastElibilityFileReportByEmployerCommandHandler(IEligibilityFileRepository eligibilityFileRepository, IEligibilityFileLineRepository eligibilityFileLineRepository, IEmployerServiceClient employerServiceClient)
    {
        _eligibilityFileRepository = eligibilityFileRepository;
        _eligibilityFileLineRepository = eligibilityFileLineRepository;
        _employerServiceClient = employerServiceClient;
    }

    public async Task<GetLastElibilityFileReportByEmployerCommandResult> HandleAsync(GetLastElibilityFileReportByEmployerCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.Validate())
        {
            return new GetLastElibilityFileReportByEmployerCommandResult(false, command.ValidationMessages!, []);
        }

        var employer = await _employerServiceClient.GetAsync(command.EmployerName, cancellationToken);

        if (employer is null)
        {
            return new GetLastElibilityFileReportByEmployerCommandResult(false, "Employer not found.", []);
        }

        var eligibilityFile = await _eligibilityFileRepository.GetByEmployerIdAsync(employer.Id, cancellationToken);

        if (eligibilityFile is null)
        {
            return new GetLastElibilityFileReportByEmployerCommandResult(false, "Eligibility file not found.", []);
        }

        if (eligibilityFile.Status == EligibilityFileStatus.Processing)
        {
            return new GetLastElibilityFileReportByEmployerCommandResult(false, "Eligibility file is being processed. Please wait.", []);
        }

        if (eligibilityFile.Status == EligibilityFileStatus.Error)
        {
            return new GetLastElibilityFileReportByEmployerCommandResult(false, "Eligibility file has been processed with erros. Please try again later.", []);
        }

        var eligibilityFileLines = await _eligibilityFileLineRepository.FindByEligibilityFileIdAsync(eligibilityFile.Id, cancellationToken);

        return new GetLastElibilityFileReportByEmployerCommandResult(true, "Eligibility fil linese retrieved successfully.", eligibilityFileLines.Select(t => new Models.EligibilityFileLineModel(t)));
    }
}