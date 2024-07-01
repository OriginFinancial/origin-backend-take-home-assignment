using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Application.Models;
using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Services.Interfaces;

namespace UserAccessManagement.Application.Handlers;

public sealed class SignUpCommandHandler : ICommandHandler<SignUpCommand, SignUpCommandResult>
{
    private readonly ISignUpDomainService _signUpDomainService;

    public SignUpCommandHandler(ISignUpDomainService signUpDomainService)
    {
        _signUpDomainService = signUpDomainService;
    }

    public async Task<SignUpCommandResult> HandleAsync(SignUpCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.Validate())
        {
            return new SignUpCommandResult(false, command.ValidationMessages!, default);
        }

        var user = new User(command.Email, command.Password, command.Country, command.FullName, command.BirthDate, command.Salary);

        user = await _signUpDomainService.SignUpAsync(user, cancellationToken);

        return new SignUpCommandResult(true, "User signed up successfully.", new UserModel(user));
    }
}