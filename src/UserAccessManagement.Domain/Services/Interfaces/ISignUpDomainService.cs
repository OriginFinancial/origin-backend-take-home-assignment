using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Domain.Services.Interfaces;

public interface ISignUpDomainService
{
    Task<User> SingUpAsync(User user, CancellationToken cancellationToken = default);
}