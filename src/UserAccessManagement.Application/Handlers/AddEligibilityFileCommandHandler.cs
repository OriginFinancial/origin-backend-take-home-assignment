using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.EmployerService;
using UserAccessManagement.EmployerService.Requests;
using UserAccessManagement.EmployerService.Responses;

namespace UserAccessManagement.Application.Handlers;

public sealed class AddEligibilityFileCommandHandler : ICommandHandler<AddEligibilityFileCommand, CommandResult>
{
    private readonly IEligibilityFileRepository _eligibilityFileRepository;
    private readonly IEmployerServiceClient _employerServiceClient;


    public AddEligibilityFileCommandHandler(IEligibilityFileRepository eligibilityFileRepository, IEmployerServiceClient employerServiceClient)
    {
        _eligibilityFileRepository = eligibilityFileRepository;
        _employerServiceClient = employerServiceClient;
    }

    public async Task<CommandResult> HandleAsync(AddEligibilityFileCommand command, CancellationToken cancellationToken = default)
    {
        var employer = await GetOrCreateEmployerAsync(command.EmployerName, cancellationToken);

        if (employer == null)
        {
            return new CommandResult(false, "Failed to create or retrieve employer information.");
        }

        if (await _eligibilityFileRepository.AnyPendingOrProcessingAsync(employer.Id, cancellationToken))
        {
            return new CommandResult(false, "There is already a file for this employer being processed, please wait.");
        }

        var eligibiltyFile = new EligibilityFile(employer!.Id, command.File);

        await _eligibilityFileRepository.AddAsync(eligibiltyFile, cancellationToken);

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
}