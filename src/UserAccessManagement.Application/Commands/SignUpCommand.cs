using System.Text;
using System.Text.RegularExpressions;
using UserAccessManagement.Application.Base;

namespace UserAccessManagement.Application.Commands;

public record SignUpCommand(string Email, string Password, string Country, string? FullName, DateTime? BirthDate, decimal? Salary)
    : ICommand<SignUpCommandResult>
{
    public string? ValidationMessages { get; private set; }

    public bool Validate()
    {
        var valid = true;
        var stringBuilder = new StringBuilder();

        if (string.IsNullOrEmpty(Email))
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(Email)} is required.");
        }

        if (string.IsNullOrEmpty(Password))
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(Password)} is required.");
        }
        else if (!ValidatePasswordStrength())
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(Password)} must be Minimum 8 characters, letters, symbols, and numbers.");
        }

        if (string.IsNullOrEmpty(Country))
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(Country)} is required.");
        }
        else if (Country.Length != 2)
        {
            valid = false;
            stringBuilder.AppendLine($"{nameof(Country)} must be exactly 2 characters.");
        }

        ValidationMessages = stringBuilder.ToString();
        return valid;
    }

    private bool ValidatePasswordStrength()
    {
        var regexPattern = @"^(?=.*\d)(?=.*[#$@!%&*?])[A-Za-z\d#$@!%&*?]{8,}$";

        var regex = new Regex(regexPattern);

        return regex.IsMatch(Password);
    }
}