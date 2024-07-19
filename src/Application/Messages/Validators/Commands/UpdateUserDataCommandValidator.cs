using Application.Messages.Commands;
using FluentValidation;

namespace Application.Messages.Validators.Commands;


public class UpdateUserDataCommandValidator : AbstractValidator<UpdateUserDataCommand>
{
    public UpdateUserDataCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(command => command.Country)
            .NotEmpty().WithMessage("Country is required.");

        RuleFor(command => command.Salary)
            .GreaterThanOrEqualTo(0).When(command => command.Salary.HasValue)
            .WithMessage("Salary must be a positive number.");

        RuleFor(command => command.AccessType)
            .NotEmpty().WithMessage("Access type is required.")
            .Must(accessType => new[] { "Admin", "User", "Guest" }.Contains(accessType))
            .WithMessage("Access type must be either 'Admin', 'User', or 'Guest'.");
    }
}