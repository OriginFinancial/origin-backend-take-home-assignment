using UserAccessManagement.Domain.Entities;
using UserAccessManagement.Domain.Repositories;
using UserAccessManagement.Domain.Services.Interfaces;
using UserAccessManagement.Infrastructure.Exceptions;
using UserAccessManagement.UserService;
using UserAccessManagement.UserService.Requests;

namespace UserAccessManagement.Domain.Services;

public sealed class SignUpDomainService : ISignUpDomainService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUserServiceClient _userServiceClient;

    public SignUpDomainService(IEmployeeRepository employeeRepository, IUserServiceClient userServiceClient)
    {
        _employeeRepository = employeeRepository;
        _userServiceClient = userServiceClient;
    }

    public async Task<User> SignUpAsync(User user, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetAsync(user.Email, cancellationToken);

        if (employee is not null)
        {
            user.SetByEmployee(employee);
        }
        else
        {
            var userWithEmail = await _userServiceClient.GetAsync(user.Email, cancellationToken);

            if (userWithEmail is not null)
                throw new BusinessException("Email already exists");
        }

        var userRequest = new PostUserRequest(
            user.Email,
            user.Password,
            user.Country,
            user.Salary,
            user.EmployerId.HasValue ? "employer" : "dtc",
            user.FullName,
            user.EmployerId,
            user.BirthDate
        );

        var response = await _userServiceClient.PostAsync(userRequest, cancellationToken);

        user.SetId(response!.Id);

        return user;
    }
}