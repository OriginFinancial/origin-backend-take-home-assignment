using UserAccessManagement.Domain.Commom;

namespace UserAccessManagement.Domain.Entities;

public class Employee : Entity<long>
{
    private Employee()
    {
    }

    public Employee(string email, string? fullName, string country, DateTime? birthDate, decimal? salary, Guid employerId, long eligibilityFileId, long eligibilityFileLineId)
    {
        Email = email;
        FullName = fullName;
        Country = country;
        BirthDate = birthDate;
        Salary = salary;
        EmployerId = employerId;
        EligibilityFileId = eligibilityFileId;
        EligibilityFileLineId = eligibilityFileLineId;
    }

    public string Email { get; private set; }
    public string? FullName { get; private set; }
    public string Country { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public decimal? Salary { get; private set; }
    public Guid EmployerId { get; private set; }
    public long EligibilityFileId { get; private set; }
    public long EligibilityFileLineId { get; private set; }
}