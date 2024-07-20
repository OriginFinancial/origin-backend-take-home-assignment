using Infrastructure.Records;
using MediatR;

namespace Application.Messages.Queries;

public class GetUserByEmailQuery(string email) : IRequest<UserDto?>
{
    public string Email { get; init; } = email;
}