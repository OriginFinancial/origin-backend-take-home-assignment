using Application.Messages.Commands;
using FluentValidation;

namespace Application.Messages.Validators.Commands;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not a valid email address.");

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.");

        RuleFor(command => command.Country)
            .NotEmpty()
            .WithMessage("Country is required.")
            .Matches("^[A-Z]{2}$").WithMessage("Country must be a valid alpha-2 code.")
            .Must(IsValidIso3166CountryCode).WithMessage("Country must be a valid ISO-3166 country code.");
        

        RuleFor(command => command.AccessType)
            .NotEmpty().WithMessage("Access type is required.")
            .Must(accessType => accessType == "dtc" || accessType == "employer")
            .WithMessage("Access type must be either 'dtc' or 'employer'.");

        RuleFor(command => command.FullName)
            .NotEmpty().When(command => command.FullName != null)
            .WithMessage("Full name is optional but must not be empty if provided.");

        RuleFor(command => command.EmployerId)
            .NotEmpty().When(command => command.EmployerId != null)
            .WithMessage("Employer ID is optional but must not be empty if provided.");

        RuleFor(command => command.BirthDate)
            .Must(BeAValidDate).When(command => command.BirthDate != null)
            .WithMessage("Birth date is optional but must be a valid date if provided.");

        RuleFor(command => command.Salary)
            .GreaterThanOrEqualTo(0).When(command => command.Salary.HasValue)
            .WithMessage("Salary is optional but must be a non-negative number if provided.");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return !date.HasValue || date.Value < DateTime.UtcNow;
    }
    
    private bool IsValidIso3166CountryCode(string code)
    {
        return CountryCodeValidator.IsValidIso3166CountryCode(code.ToLowerInvariant());
    }
}