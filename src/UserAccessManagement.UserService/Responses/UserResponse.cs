using System.Text.Json.Serialization;

namespace UserAccessManagement.UserService.Responses;

public record UserResponse
(
    Guid Id,
    string Email,
    string Country,
    decimal? Salary,
    [property: JsonPropertyName("access_type")] string AccessType,
    [property: JsonPropertyName("full_name")] string? FullName,
    [property: JsonPropertyName("employer_id")] Guid? EmployerId,
    [property: JsonPropertyName("birth_date")] DateTime? BirthDate
);