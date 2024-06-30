using UserAccessManagement.Domain.Commom;

namespace UserAccessManagement.Domain.Entities;

public class Employee : Entity<long>
{
    private Employee()
    {
    }

    public Employee(string email, string fullName, string country, DateTime? birthDate, decimal? salary, Guid employerId, Employer employer, long eligibilityFileLineId, EligibilityFileLine eligibilityFileLine)
    {
        Email = email;
        FullName = fullName;
        Country = country;
        BirthDate = birthDate;
        Salary = salary;
        EmployerId = employerId;
        Employer = employer;
        EligibilityFileLineId = eligibilityFileLineId;
        EligibilityFileLine = eligibilityFileLine;
    }

    public string Email { get; private set; }
    public string FullName { get; private set; }
    public string Country { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public decimal? Salary { get; private set; }
    public Guid EmployerId { get; private set; }
    public Employer Employer { get; private set; }
    public long EligibilityFileLineId { get; private set; }
    public EligibilityFileLine EligibilityFileLine { get; private set; }
}