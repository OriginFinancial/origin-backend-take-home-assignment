using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.BackgroundTask.Tasks;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.EmployerService;
using UserAccessManagement.EmployerService.Requests;
using UserAccessManagement.EmployerService.Responses;

namespace UserAccessManagement.Application.Handlers;

public sealed class AddEligibilityFileCommandHandler : ICommandHandler<AddEligibilityFileCommand, CommandResult>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEligibilityFileRepository _eligibilityFileRepository;
    private readonly IEmployerServiceClient _employerServiceClient;
    private readonly ProcessEligibilityFileBackgraoundTask _processEligibilityFileBackgraoundTask;

    public AddEligibilityFileCommandHandler(IEmployeeRepository employeeRepository, IEligibilityFileRepository eligibilityFileRepository, IEmployerServiceClient employerServiceClient, ProcessEligibilityFileBackgraoundTask processEligibilityFileBackgraoundTask)
    {
        _employeeRepository = employeeRepository;
        _eligibilityFileRepository = eligibilityFileRepository;
        _employerServiceClient = employerServiceClient;
        _processEligibilityFileBackgraoundTask = processEligibilityFileBackgraoundTask;
    }

    public async Task<CommandResult> HandleAsync(AddEligibilityFileCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.Validate())
        {
            return new CommandResult(false, command.ValidationMessages!);
        }

        var employer = await GetOrCreateEmployerAsync(command.EmployerName, cancellationToken);

        if (employer is null)
        {
            return new CommandResult(false, "Failed to create or retrieve employer information.");
        }

        if (await _eligibilityFileRepository.AnyProcessingAsync(employer.Id, cancellationToken))
        {
            return new CommandResult(false, "There is already a file for this employer being processed, please wait.");
        }

        await InactiveLastActiveEligibilityFile(employer, cancellationToken);

        var eligibiltyFile = new EligibilityFile(employer!.Id, command.File);

        await _eligibilityFileRepository.AddAsync(eligibiltyFile, cancellationToken);

        _processEligibilityFileBackgraoundTask.Enqueue(eligibiltyFile.Id, cancellationToken);

        return new CommandResult(true, "File saved successfully. It will be processed shortly, please wait.");
    }

    private async Task<EmployerResponse?> GetOrCreateEmployerAsync(string employerName, CancellationToken cancellationToken)
    {
        var employer = await _employerServiceClient.GetAsync(employerName, cancellationToken);

        if (employer is null)
        {
            employer = await _employerServiceClient.PostAsync(new PostEmployerRequest(employerName), cancellationToken);
        }

        return employer;
    }

    private async Task InactiveLastActiveEligibilityFile(EmployerResponse employer, CancellationToken cancellationToken)
    {
        var lastActivEligibiltyFile = await _eligibilityFileRepository.GetByEmployerIdAsync(employer!.Id, cancellationToken);

        if (lastActivEligibiltyFile is null) return;

        lastActivEligibiltyFile.Inactivate();

        await _eligibilityFileRepository.UpdateAsync(lastActivEligibiltyFile, cancellationToken);
        await _employeeRepository.InactiveByEligibilityFileId(lastActivEligibiltyFile.Id);
    }
}