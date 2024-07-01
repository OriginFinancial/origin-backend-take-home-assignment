using System.Text;
using System.Text.Json.Serialization;
using UserAccessManagement.Application.Base;

namespace UserAccessManagement.Application.Commands;

public record AddEligibilityFileCommand
(
    string File,
    [property: JsonPropertyName("employer_name")] string EmployerName
) : ICommand<CommandResult>
{
    public string? ValidationMessages {  get; private set; }

    public bool Validate()
    {
        var valid = true;
        var stringBuilder = new StringBuilder();

        if (string.IsNullOrEmpty(File))
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(File)} is required.");
        }

        if (string.IsNullOrEmpty(EmployerName))
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(EmployerName)} is required.");
        }

        ValidationMessages = stringBuilder.ToString();
        return valid;
    }
}