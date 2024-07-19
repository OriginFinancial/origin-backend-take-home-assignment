using Application.Messages.Commands;
using Bogus;
using Domain.Enums;

namespace Application.Tests.Factories;

public class CreateUserCommandFactory
{
    private readonly Faker<CreateUserCommand> _faker;
    public CreateUserCommandFactory()
    {
        _faker = new Faker<CreateUserCommand>()
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Country, f => f.Address.Country())
            .RuleFor(u => u.AccessType, f => f.PickRandom(new []{AccessTypeEnum.DTC, AccessTypeEnum.Employer}))
            .RuleFor(u => u.FullName, f => f.Name.FullName())
            .RuleFor(u => u.EmployerId, f => f.Random.Guid().ToString())
            .RuleFor(u => u.BirthDate, f => f.Date.Past(30))
            .RuleFor(u => u.Salary, f => f.Random.Decimal(30000, 100000));
    }

    public CreateUserCommandFactory WithEmail(string email)
    {
        _faker.RuleFor(x => x.Email, email);
        return this;
    }
    public CreateUserCommand Create()
    {
        return _faker.Generate();
    }

    public CreateUserCommandFactory WithPassword(string password)
    {
        _faker.RuleFor(x => x.Password, password);
        return this;
    }
    public CreateUserCommandFactory WithCountry(string country)
    {
        _faker.RuleFor(x => x.Country, country);
        return this;
    }
    
    public CreateUserCommandFactory WithAccessType(string accessType)
    {
        _faker.RuleFor(x => x.AccessType, accessType);
        return this;
    }
    
    public CreateUserCommandFactory WithFullName(string fullName)
    {
        _faker.RuleFor(x => x.FullName, fullName);
        return this;
    }
    
    public CreateUserCommandFactory WithEmployerId(string employerId)
    {
        _faker.RuleFor(x => x.EmployerId, employerId);
        return this;
    }
    
    public CreateUserCommandFactory WithBirthDate(DateTime? birthDate)
    {
        _faker.RuleFor(x => x.BirthDate, birthDate);
        return this;
    }
    
    public CreateUserCommandFactory WithSalary(decimal? salary)
    {
        _faker.RuleFor(x => x.Salary, salary);
        return this;
    }
    
}