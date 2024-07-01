using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Enums;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Domain.Services.Interfaces;
using UserAccessManagement.Infrastructure.Csv;
using UserAccessManagement.Infrastructure.Exceptions;
using UserAccessManagement.Infrastructure.Services;
using UserAccessManagement.UserService;

namespace UserAccessManagement.Domain.Services;

public class EligibilityFileDomainService : IEligibilityFileDomainService
{
    private readonly CsvService _csvService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEligibilityFileRepository _eligibilityFileRepository;
    private readonly IEligibilityFileLineRepository _eligibilityFileLineRepository;
    private readonly IUserServiceClient _userServiceClient;

    public EligibilityFileDomainService(CsvService csvService, IEmployeeRepository employeeRepository, IEligibilityFileRepository eligibilityFileRepository, IEligibilityFileLineRepository eligibilityFileLineRepository, IUserServiceClient userServiceClient)
    {
        _csvService = csvService;
        _employeeRepository = employeeRepository;
        _eligibilityFileRepository = eligibilityFileRepository;
        _eligibilityFileLineRepository = eligibilityFileLineRepository;
        _userServiceClient = userServiceClient;
    }

    public async Task ProccessFileAsync(long eligibilityFileId, CancellationToken cancellationToken = default)
    {
        var eligibilityFile = await _eligibilityFileRepository.GetByIdAsync(eligibilityFileId, cancellationToken)
            ?? throw new BusinessException("Eligibility file not found.");

        try
        {
            await _csvService.DownloadCsvAsync(eligibilityFile.Url, async stream =>
            {
                await ProcessFileLineAsync(stream, eligibilityFile, cancellationToken);
            });

            eligibilityFile.SetProcessed("File processed sucessfully.");
        }
        catch (Exception ex)
        {
            eligibilityFile.SetError(ex.Message);
        }

        await _eligibilityFileRepository.UpdateAsync(eligibilityFile, cancellationToken);
        await TerminateNonEligibleUserAccountsAsync(eligibilityFile, cancellationToken);
    }

    private async Task TerminateNonEligibleUserAccountsAsync(EligibilityFile eligibilityFile, CancellationToken cancellationToken = default)
    {
        var users = await _userServiceClient.GetAllByEmployerIdAsync(eligibilityFile.EmployerId, cancellationToken);

        if (!users.Any()) return;

        var employees = await _employeeRepository.FindByEligibilityFileId(eligibilityFile.Id, cancellationToken);

        var usersToTerminate = users.Where(t => !employees.Any(e => e.Email == t.Email));

        foreach (var user in usersToTerminate)
        {
            await _userServiceClient.DeleteAsync(user.Id, cancellationToken);
        }
    }

    private async Task ProcessFileLineAsync(Stream stream, EligibilityFile eligibilityFile, CancellationToken cancellationToken = default)
    {
        await foreach (var csvLine in _csvService.ParseCsvAsync(stream))
        {
            if (csvLine is null) continue;

            var (isValid, errorMessage) = ValidateCsvLine(csvLine);

            var lineContent = isValid ? csvLine.RawContent! : errorMessage!;
            var lineType = isValid ? EligibilityFileLineType.Valid : EligibilityFileLineType.Invalid;

            var line = new EligibilityFileLine(lineContent, lineType, eligibilityFile.Id);

            await _eligibilityFileLineRepository.AddAsync(line, cancellationToken);

            if (isValid)
                await CreateEmployeeAsync(csvLine, eligibilityFile, line.Id, cancellationToken);
        }
    }

    private (bool isValid, string? errorMessage) ValidateCsvLine(EligibilityFileCsvLine csvLine)
    {
        if (csvLine.HasError())
            return (false, csvLine.ErrorMessage);

        var valid = true;
        var errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(csvLine.Email))
        {
            valid = false;
            errorMessage += "Invalid E-Mail; ";
        }

        if (string.IsNullOrWhiteSpace(csvLine.Country))
        {
            valid = false;
            errorMessage += "Invalid Country;";
        }

        return (valid, errorMessage);
    }

    private async Task CreateEmployeeAsync(EligibilityFileCsvLine csvLine, EligibilityFile eligibilityFile, long eligibilityFileLineId, CancellationToken cancellationToken)
    {
        var employee = new Employee(csvLine.Email!, csvLine.FullName, csvLine.Country!, csvLine.BirthDate, csvLine.Salary, eligibilityFile.EmployerId, eligibilityFile.Id, eligibilityFileLineId);

        await _employeeRepository.AddAsync(employee, cancellationToken);
    }
}