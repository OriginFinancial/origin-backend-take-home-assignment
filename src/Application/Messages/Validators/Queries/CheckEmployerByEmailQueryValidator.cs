using Application.Messages.Queries;
using FluentValidation;

namespace Application.Messages.Validators.Queries;

public class CheckEmployerByEmailQueryValidator : AbstractValidator<CheckEmployerByEmailQuery>
{
    public CheckEmployerByEmailQueryValidator()
    {
        RuleFor(query => query.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
    }
}