using Application.Messages.Commands;
using Application.Messages.Validators.Commands;

namespace Application.Tests.Messages.Validators.Commands;

[Trait("Category", "Unit")]
public class UpdateUserDataCommandValidatorTests
{
    private readonly UpdateUserDataCommandValidator _validator;

    public UpdateUserDataCommandValidatorTests()
    {
        _validator = new UpdateUserDataCommandValidator();
    }
    
    [Fact]
    public void Email_IsRequired()
    {
        var command = new UpdateUserDataCommand(null, "CountryName", 50000, "User");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Email_MustBeValidEmail()
    {
        var command = new UpdateUserDataCommand("invalid-email", "CountryName", 50000, "User");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public void Country_IsRequired()
    {
        var command = new UpdateUserDataCommand("email@example.com", null, 50000, "User");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Country");
    }

    [Fact]
    public void Salary_MustBePositive_WhenProvided()
    {
        var command = new UpdateUserDataCommand("email@example.com", "CountryName", -1, "User");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Salary");
    }

    [Fact]
    public void AccessType_MustMatchSpecificValues()
    {
        var command = new UpdateUserDataCommand("email@example.com", "CountryName", 50000, "InvalidAccessType");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "AccessType");
    }

    [Fact]
    public void Command_WithValidData_ShouldPassValidation()
    {
        var command = new UpdateUserDataCommand("email@example.com", "CountryName", 50000, "User");
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }
}