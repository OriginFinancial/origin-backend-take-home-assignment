using FluentValidation;
using Application.Messages.Queries;

namespace Application.Messages.Validators.Queries;

public class GetEmployerByIdQueryValidator : AbstractValidator<GetEmployerByIdQuery>
{
    public GetEmployerByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty().WithMessage("Id is required.")
            .Matches("^[a-zA-Z0-9-]*$").WithMessage("Id must be alphanumeric with dashes.");
    }
}