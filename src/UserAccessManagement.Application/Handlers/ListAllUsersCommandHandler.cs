using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using UserAccessManagement.Application.Models;
using UserAccessManagement.EmployerService;
using UserAccessManagement.UserService;

namespace UserAccessManagement.Application.Handlers;

internal class ListAllUsersCommandHandler : ICommandHandler<ListAllUsersCommand, ListAllUsersCommandResult>
{
    private readonly IUserServiceClient _userServiceClient;
    private readonly IEmployerServiceClient _employerServiceClient;

    public ListAllUsersCommandHandler(IUserServiceClient userServiceClient, IEmployerServiceClient employerServiceClient)
    {
        _userServiceClient = userServiceClient;
        _employerServiceClient = employerServiceClient;
    }

    public async Task<ListAllUsersCommandResult> HandleAsync(ListAllUsersCommand command, CancellationToken cancellationToken = default)
    {
        if (!command.Validate())
        {
            return new ListAllUsersCommandResult(false, command.ValidationMessages!, []);
        }

        var employer = await _employerServiceClient.GetAsync(command.EmployerName, cancellationToken);

        if (employer is null)
        {
            return new ListAllUsersCommandResult(false, "Employer not found.", []);
        }

        var users = await _userServiceClient.GetAllByEmployerIdAsync(employer.Id, cancellationToken);
        var usersModel = users?.Select(t => new UserModel(t.Id, t.Email, t.Country, t.FullName, t.BirthDate, t.Salary, true));

        return new ListAllUsersCommandResult(true, "Success", usersModel);
    }
}