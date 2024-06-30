using UserAccessManagement.EmployerService.Requests;
using UserAccessManagement.EmployerService.Responses;

namespace UserAccessManagement.EmployerService;

public interface IEmployerServiceClient
{
    Task<EmployerResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<EmployerResponse?> GetAsync(string name, CancellationToken cancellationToken = default);
    Task<EmployerResponse?> PostAsync(PostEmployerRequest request, CancellationToken cancellationToken = default);
}