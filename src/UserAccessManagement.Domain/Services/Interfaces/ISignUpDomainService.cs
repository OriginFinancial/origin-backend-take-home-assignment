using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Domain.Services.Interfaces;

public interface ISignUpDomainService
{
    Task<User> SignUpAsync(User user, CancellationToken cancellationToken = default);
}