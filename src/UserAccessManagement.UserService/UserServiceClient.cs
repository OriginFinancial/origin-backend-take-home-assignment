using UserAccessManagement.UserService.Requests;
using UserAccessManagement.UserService.Responses;

namespace UserAccessManagement.UserService;

public class UserServiceClient : IUserServiceClient
{
    private readonly Dictionary<Guid, UserResponse> _usersId = [];
    private readonly Dictionary<string, UserResponse> _usersEmail = [];

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (_usersId.TryGetValue(id, out UserResponse? user))
        {
            _usersId.Remove(id);
            _usersEmail.Remove(user.Email);

            // in the real implementation it would be inactivated instead of removed.
        }

        await Task.CompletedTask;
    }

    public async Task<IEnumerable<UserResponse>> GetAllByEmployerIdAsync(Guid employerId, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_usersId
            .Where(t => t.Value.EmployerId == employerId)
            .Select(t => t.Value)
            .ToList());
    }

    public async Task<UserResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (_usersId.TryGetValue(id, out UserResponse? value))
            return await Task.FromResult(value);

        return await Task.FromResult<UserResponse?>(default);
    }

    public async Task<UserResponse?> GetAsync(string email, CancellationToken cancellationToken = default)
    {
        if (_usersEmail.TryGetValue(email, out UserResponse? value))
            return await Task.FromResult(value);

        return await Task.FromResult<UserResponse?>(default);
    }

    public async Task<UserResponse?> PatchAsync(PatchUserRequest request, CancellationToken cancellationToken = default)
    {
        if (_usersId.TryGetValue(request.UserId, out UserResponse? user))
        {
            foreach (var item in request.Data)
            {
                // need to be improved. `with` creates a clone of record with the properties changed. Do that multiple times in a loop is not good at all. But for this mock implementation I'll keep it since it will happen only twice in this scenario.
                // I created this record with init only properties because in the real implementation it would be crated by the data returned by the real API, and would not be changed.

                if (item.Field == "salary")
                    user = user with { Salary = (decimal?)item.Value };
                else if (item.Field == "country")
                    user = user with { Country = item.Value?.ToString() ?? "XX" };
            }

            _usersId[request.UserId] = user;
            _usersEmail[user.Email] = user;

            return user;
        }

        return await Task.FromResult<UserResponse?>(default);
    }

    public async Task<UserResponse?> PostAsync(PostUserRequest request, CancellationToken cancellationToken = default)
    {
        var user = new UserResponse(Guid.NewGuid(), request.Email, request.Country, request.Salary, request.AccessType, request.FullName, request.EmployerId, request.BirthDate);

        _usersId.Add(user.Id, user);
        _usersEmail.Add(user.Email, user);

        return await Task.FromResult(user);
    }
}
