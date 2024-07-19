using Application.Messages.Commands;
using FluentValidation;

public class ProcessEligibilityFileCommandValidator : AbstractValidator<ProcessEligibilityFileCommand>
{
    public ProcessEligibilityFileCommandValidator()
    {
        RuleFor(command => command.CsvFileUrl)
            .NotEmpty().WithMessage("CSV file URL is required.")
            .Must(BeAValidUrl).WithMessage("CSV file URL must be a valid URL.");

        RuleFor(command => command.EmployerName)
            .NotEmpty().WithMessage("Employer name is required.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}