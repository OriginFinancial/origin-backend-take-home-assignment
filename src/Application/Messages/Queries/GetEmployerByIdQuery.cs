using Infrastructure.Records;
using MediatR;

namespace Application.Messages.Queries;

public class GetEmployerByIdQuery(string id) : IRequest<EmployerDto?>
{
    public string Id { get; init; } = id;
}