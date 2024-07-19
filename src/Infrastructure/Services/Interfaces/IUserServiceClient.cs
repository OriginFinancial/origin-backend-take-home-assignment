using Infrastructure.Records;

namespace Infrastructure.Services.Interfaces;

public interface IUserServiceClient
{
    Task<UserDto> CheckUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<bool> CreateUserAsync(CreateUserDto user, CancellationToken cancellationToken);
    Task<List<UserDto>> GetUserByEmployerIdAsync(string id, CancellationToken cancellationToken);
    Task UpdateUserAsync(UserDto user, CancellationToken cancellationToken);
    
    Task TerminateUserAsync(string userId, CancellationToken cancellationToken);
}