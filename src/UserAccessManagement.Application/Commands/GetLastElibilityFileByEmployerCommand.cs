using System.Text.Json.Serialization;
using UserAccessManagement.Application.Base;

namespace UserAccessManagement.Application.Commands;

public record GetLastElibilityFileByEmployerCommand([property: JsonPropertyName("employer_name")] string EmployerName)
    : ICommand<GetLastElibilityFileByEmployerCommandResult>;