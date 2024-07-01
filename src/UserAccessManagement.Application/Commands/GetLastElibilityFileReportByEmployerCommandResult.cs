using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Models;

namespace UserAccessManagement.Application.Commands;

public record GetLastElibilityFileReportByEmployerCommandResult(bool Success, string Message, IEnumerable<EligibilityFileLineModel> ElibilityFileLines) 
    : CommandResult(Success, Message);