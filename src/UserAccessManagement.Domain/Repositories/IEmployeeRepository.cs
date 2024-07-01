using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Domain.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default);
    Task<IEnumerable<Employee>> FindByEligibilityFileId(long eligibilityFileId, CancellationToken cancellationToken);
    Task<Employee?> GetAsync(string email, CancellationToken cancellationToken = default);
    Task InactiveByEligibilityFileId(long eligibilityFileId, CancellationToken cancellationToken = default);
}