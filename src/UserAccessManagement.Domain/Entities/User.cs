using UserAccessManagement.Domain.Commom;

namespace UserAccessManagement.Domain.Entities;

public class User : Entity<Guid>
{
    public User(string email, string password, string country)
        : base()
    {
        Email = email;
        Password = password;
        Country = country;
    }

    public User(string email, string password, string country, Guid employerId)
        : this(email, password, country)
    {
        EmployerId = employerId;
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Country { get; private set; }
    public string? FullName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public decimal? Salary { get; private set; }
    public Guid? EmployerId { get; private set; }

    public void Update(string country, decimal? salary)
    {
        Country = country;
        Salary = salary;
        Stamp();
    }
}