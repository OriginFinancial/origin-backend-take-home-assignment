using Domain.Models;
using MediatR;

namespace Application.Messages.Commands;

public class ProcessEligibilityFileCommand : IRequest<EligibilityProcessResult>
{
    public string CsvFileUrl { get; init; }
    public string EmployerName { get; init; }
    
    public ProcessEligibilityFileCommand(string csvFileUrl, string employerName)
    {
        CsvFileUrl = csvFileUrl;
        EmployerName = employerName;
    }
}