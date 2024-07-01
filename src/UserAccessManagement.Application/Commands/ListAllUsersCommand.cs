using System.Text.Json.Serialization;
using UserAccessManagement.Application.Base;

namespace UserAccessManagement.Application.Commands;

public record ListAllUsersCommand([property: JsonPropertyName("employer_name")] string EmployerName) : ICommand<ListAllUsersCommandResult>
{
    public string? ValidationMessages { get; private set; }

    public bool Validate()
    {
        if (string.IsNullOrEmpty(EmployerName))
        {
            ValidationMessages = $"{nameof(EmployerName)} is required.";
            return false;
        }
        return true;
    }
}