using UserAccessManagement.Domain.Entities;

namespace UserAccessManagement.Application.Models;

public class UserModel : EntityModel<Guid>
{
    public UserModel(User entity) : base(entity)
    {
        Email = entity.Email;
        Country = entity.Country;
        FullName = entity.FullName;
        BirthDate = entity.BirthDate;
        Salary = entity.Salary;
    }

    public UserModel(Guid id, string email, string country, string? fullName, DateTime? birthDate, decimal? salary, bool active)
        : base(id, active)
    {
        Email = email;
        Country = country;
        FullName = fullName;
        BirthDate = birthDate;
        Salary = salary;
    }

    public string Email { get; private set; }
    public string Country { get; private set; }
    public string? FullName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public decimal? Salary { get; private set; }
}