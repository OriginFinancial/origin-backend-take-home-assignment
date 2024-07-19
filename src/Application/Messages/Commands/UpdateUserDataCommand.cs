using MediatR;

namespace Application.Messages.Commands;

public class UpdateUserDataCommand: IRequest<bool>
{
    public string Email { get; set; }
    public string Country { get; set; }
    public decimal? Salary { get; set; }
    public string AccessType { get; set; }

    public UpdateUserDataCommand(string email, string country, decimal? salary, string accessType)
    {
        Email = email;
        Country = country;
        Salary = salary;
        AccessType = accessType;
    }
}