using Domain.Interfaces;
using Infrastructure.Records;

namespace Infrastructure.Services.Interfaces;

public interface IEmployerServiceClient
{
    Task<IPerson?> GetEmployerByIdAsync(string id, CancellationToken cancellationToken);
    Task<EmployerIdRecord> GetEmployerByEmailAsync(string email, CancellationToken cancellationToken);
    Task CreateEmployerAsync(string email, CancellationToken cancellationToken);
}

