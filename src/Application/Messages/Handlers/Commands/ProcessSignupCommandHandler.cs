using Application.Messages.Commands;
using Application.Messages.Queries;
using Domain.Enums;
using Infrastructure.Records;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Messages.Handlers.Commands;

public class ProcessSignupCommandHandler : IRequestHandler<ProcessSignupCommand>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProcessSignupCommandHandler> _logger;

    public ProcessSignupCommandHandler(IMediator mediator, ILogger<ProcessSignupCommandHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(ProcessSignupCommand request, CancellationToken cancellationToken)
    {
        var employerData = await _mediator.Send(new CheckEmployerByEmailQuery(request.Email), cancellationToken);
        if (employerData == null)
        {
            await CreateUser(request.Email, request.Password, request.Country, employerData);
            return;
        }

        var existingUser = await _mediator.Send(new GetUserByEmailQuery(request.Email), cancellationToken);
        if (existingUser != null)
        {
            _logger.LogError($"User with email {request.Email} already exists.");
            throw new InvalidOperationException("User already exists.");
        }

        if (!IsValidPassword(request.Password))
        {
            _logger.LogError("Password does not meet the strength requirements.");
            throw new InvalidOperationException("Password does not meet the strength requirements.");
        }

        await CreateUser(request.Email, request.Password, request.Country, employerData);
    }

    // Minimum 8 characters, letters, symbols, and numbers
    private bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) &&
               (password.Length >= 8 &&
               password.Any(char.IsLetter) &&
               password.Any(char.IsDigit) &&
               password.Any(ch => !char.IsLetterOrDigit(ch)));
    }

    private async Task CreateUser(string email, string password, string country, EmployerIdRecord? employerData)
    {
        EmployerDto? employerDto = null;
        if (employerData != null)
        {
            _logger.LogInformation("Extracting Employer information for Id: {Id}.", employerData.Id);
            employerDto = await _mediator.Send(new GetEmployerByIdQuery(employerData.Id));
        }
        
        _logger.LogInformation("Creating user for {Email}.", email);
        await _mediator.Send(new CreateUserCommand(email,
            password,
            country,
            AccessTypeEnum.Employer,
            employerDto?.FullName,
            employerData?.Id,
            employerDto?.BirthDate,
            employerDto?.Salary));
        // Use _mediator to send a command to create the user
    }
}