namespace Infrastructure.Records;

public class CreateUserDto
{
    public string Id { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string AccessType { get; set; }
    public string FullName { get; set; }
    public string EmployerId { get; set; }
    public DateTime? BirthDate { get; set; }
    public decimal? Salary { get; set; }
    
}