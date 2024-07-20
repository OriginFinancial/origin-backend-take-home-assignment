using Application.Messages.Validators.Commands;
using Application.Tests.Factories;

namespace Application.Tests.Messages.Validators.Commands;

[Trait("Category", "Unit")]
public class CreateUserCommandValidatorTests
{
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _validator = new CreateUserCommandValidator();
    }
    [Fact]
    public void Email_IsRequired()
    {
        // Arrange
        var command = new CreateUserCommandFactory()
            .WithEmail(string.Empty)
            .Create(); 

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }
    
    [Fact]
    public void Password_IsRequired()
    {
        var command = new CreateUserCommandFactory().WithPassword(string.Empty).Create();
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }

    [Fact]
    public void Country_IsRequired()
    {
        var command = new CreateUserCommandFactory().WithCountry(string.Empty).Create();
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Country");
    }

    [Fact]
    public void AccessType_IsRequired()
    {
        var command = new CreateUserCommandFactory().WithAccessType(null).Create(); // Assuming WithAccessType method is implemented to handle null
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "AccessType");
    }

    [Fact]
    public void FullName_WhenProvided_MustNotBeEmpty()
    {
        var command = new CreateUserCommandFactory().WithFullName(string.Empty).Create(); // Assuming WithFullName method is implemented
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "FullName" && e.ErrorMessage.Contains("must not be empty if provided"));
    }

    [Fact]
    public void EmployerId_WhenProvided_MustNotBeEmpty()
    {
        var command = new CreateUserCommandFactory().WithEmployerId(string.Empty).Create(); // Assuming WithEmployerId method is implemented
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EmployerId" && e.ErrorMessage.Contains("must not be empty if provided"));
    }

    [Fact]
    public void BirthDate_WhenProvided_MustBeValid()
    {
        var command = new CreateUserCommandFactory().WithBirthDate(DateTime.UtcNow.AddDays(1)).Create(); // Assuming WithBirthDate method is implemented
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BirthDate" && e.ErrorMessage.Contains("must be a valid date if provided"));
    }

    [Fact]
    public void Salary_WhenProvided_MustBeNonNegative()
    {
        var command = new CreateUserCommandFactory().WithSalary(-1).Create(); // Assuming WithSalary method is implemented
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Salary" && e.ErrorMessage.Contains("must be a non-negative number if provided"));
    }
}