using Application.Messages.Commands;
using Infrastructure.Records;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Application.Messages.Handlers.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserServiceClient _userServiceClient;

    public CreateUserCommandHandler(IUserServiceClient userServiceClient)
    {
        _userServiceClient = userServiceClient;
    }

    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _userServiceClient.CreateUserAsync(new CreateUserDto
        {
            Email = request.Email,
            Password = request.Password,
            Country = request.Country,
            AccessType = request.AccessType,
            FullName = request.FullName,
            EmployerId = request.EmployerId,
            BirthDate= request.BirthDate,
            Salary = request.Salary
        }, cancellationToken);
        return result;
    }
}