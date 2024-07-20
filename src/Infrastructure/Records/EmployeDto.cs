using Domain.Interfaces;

namespace Infrastructure.Records;

public class EmployerDto : IPerson
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string AccessType { get; set; }
    public string FullName { get; set; }
    public string EmployerId { get; set; }
    public DateTime? BirthDate { get; set; }
    public decimal? Salary { get; set; }
}