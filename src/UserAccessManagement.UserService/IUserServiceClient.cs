using UserAccessManagement.UserService.Requests;
using UserAccessManagement.UserService.Responses;

namespace UserAccessManagement.UserService;

public interface IUserServiceClient
{
    Task<UserResponse?> PostAsync(PostUserRequest request, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<UserResponse?> GetAsync(string email, CancellationToken cancellationToken = default);
    Task<UserResponse?> PatchAsync(PatchUserRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}