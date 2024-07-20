using MediatR;

namespace Application.Messages.Commands;

public class CreateUserCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Country { get; set; }
    public string AccessType { get; set; }
    public string? FullName { get; set; }
    public string? EmployerId { get; set; }
    public DateTime? BirthDate { get; set; }
    public decimal? Salary { get; set; }
    
    public CreateUserCommand(string email,
        string password,
        string country,
        string accessType,
        string? fullName,
        string? employerId,
        DateTime? birthDate,
        decimal? salary)
    {
        Email = email;
        Password = password;
        Country = country;
        AccessType = accessType;
        FullName = fullName;
        EmployerId = employerId;
        BirthDate = birthDate;
        Salary = salary;
    }

    public CreateUserCommand()
    {
        
    }
}