using FluentValidation;
using Application.Messages.Commands;

namespace Application.Messages.Validators.Commands;


public class ProcessSignupCommandValidator : AbstractValidator<ProcessSignupCommand>
{
    public ProcessSignupCommandValidator()
    {
        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(command => command.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");

        RuleFor(command => command.Country)
            .NotEmpty().WithMessage("Country is required.");
    }
}