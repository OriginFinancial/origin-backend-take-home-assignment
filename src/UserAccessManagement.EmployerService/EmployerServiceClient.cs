using UserAccessManagement.EmployerService.Requests;
using UserAccessManagement.EmployerService.Responses;

namespace UserAccessManagement.EmployerService;

public sealed class EmployerServiceClient : IEmployerServiceClient
{
    private readonly Dictionary<Guid, EmployerResponse> _employersId = [];
    private readonly Dictionary<string, EmployerResponse> _employersName = [];

    public EmployerServiceClient()
    {
        var id = Guid.NewGuid();
        var name = "Employer Ltd";
        var employer = new EmployerResponse(id, name);

        _employersId.Add(id, employer);
        _employersName.Add(name, employer);
    }

    public async Task<EmployerResponse?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (_employersId.TryGetValue(id, out EmployerResponse? value))
            return await Task.FromResult(value);

        return await Task.FromResult<EmployerResponse?>(default);
    }

    public async Task<EmployerResponse?> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        if (_employersName.TryGetValue(name, out EmployerResponse? value))
            return await Task.FromResult(value);

        return await Task.FromResult<EmployerResponse?>(default);
    }

    public async Task<EmployerResponse?> PostAsync(PostEmployerRequest request, CancellationToken cancellationToken = default)
    {
        var employer = new EmployerResponse(Guid.NewGuid(), request.Name);

        _employersId.Add(employer.Id, employer);
        _employersName.Add(employer.Name, employer);

        return await Task.FromResult(employer);
    }
}