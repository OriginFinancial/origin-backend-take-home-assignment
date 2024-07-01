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

    public User(string email, string password, string country, string? fullName, DateTime? birthDate, decimal? salary) 
        : this(email, password, country)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Salary = salary;
    }

    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Country { get; private set; }
    public string? FullName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public decimal? Salary { get; private set; }
    public Guid? EmployerId { get; private set; }

    public void SetByEmployee(Employee employee)
    {
        Country = employee.Country;
        FullName = employee.FullName;
        BirthDate = employee.BirthDate; 
        Salary = employee.Salary;
        EmployerId = employee.EmployerId;
    }
}