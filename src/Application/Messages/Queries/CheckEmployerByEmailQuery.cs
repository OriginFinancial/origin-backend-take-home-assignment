using Infrastructure.Records;
using MediatR;

namespace Application.Messages.Queries;

public class CheckEmployerByEmailQuery : IRequest<EmployerIdRecord?>
{
    public CheckEmployerByEmailQuery(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}