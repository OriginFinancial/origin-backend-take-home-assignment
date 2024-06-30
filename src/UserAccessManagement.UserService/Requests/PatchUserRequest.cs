namespace UserAccessManagement.UserService.Requests;

public class PatchUserRequest
{
    public PatchUserRequest(Guid userId)
    {
        UserId = userId;
        Data = [];    
    }

    public Guid UserId { get; set; }
    public List<PatchUserModel> Data { get; private set; }

    public PatchUserRequest Build(string country, decimal? salary)
    {
        Data = [];

        Data.Add(new PatchUserModel(Field: nameof(country), Value: country));
        Data.Add(new PatchUserModel(Field: nameof(salary), Value: salary));

        return this;
    }

    public record PatchUserModel(string Field, object? Value);
}