using Application.Messages.Commands;
using FluentValidation;

namespace Application.Messages.Validators.Commands;

public class TerminateUnlistedUsersCommandValidator : AbstractValidator<TerminateUnlistedUsersCommand>
{
    public TerminateUnlistedUsersCommandValidator()
    {
        RuleFor(command => command.EmployerId)
            .NotEmpty().WithMessage("EmployerId is required.");
    }
}