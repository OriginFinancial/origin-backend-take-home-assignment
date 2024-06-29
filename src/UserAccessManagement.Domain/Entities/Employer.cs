using UserAccessManagement.Domain.Commom;

namespace UserAccessManagement.Domain.Entities;

public class Employer : Entity<Guid>
{
    public Employer(string name)
    {
        Name = name;
    }

    public Employer(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public string Name { get; set; }
}