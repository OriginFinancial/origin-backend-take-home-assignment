using Application.Messages.Commands;
using Application.Messages.Validators.Commands;

namespace Application.Tests.Messages.Validators.Commands;

[Trait("Category", "Unit")]
public class ProcessSignupCommandValidatorTests
{
    private ProcessSignupCommandValidator _validator;

    public ProcessSignupCommandValidatorTests()
    {
        _validator = new ProcessSignupCommandValidator();
    }
    [Fact]
    public void Email_IsRequired()
    {
        var command = new ProcessSignupCommand(null, "ValidPassword123", "CountryName");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }
    
    [Fact]
    public void Email_MustBeValidEmail()
    {
        var command = new ProcessSignupCommand("invalid-email", "ValidPassword123", "CountryName");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }
    
    [Fact]
    public void Password_IsRequired()
    {
        var command = new ProcessSignupCommand("email@example.com", null, "CountryName");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }
    
    [Fact]
    public void Password_MustBeAtLeast8CharactersLong()
    {
        var command = new ProcessSignupCommand("email@example.com", "short", "CountryName");
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Password");
    }
    
    [Fact]
    public void Country_IsRequired()
    {
        var command = new ProcessSignupCommand("email@example.com", "ValidPassword123", null);
        var result = _validator.Validate(command);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Country");
    }
    
    [Fact]
    public void Command_WithValidData_ShouldPassValidation()
    {
        var command = new ProcessSignupCommand("email@example.com", "ValidPassword123", "CountryName");
        var result = _validator.Validate(command);
        Assert.True(result.IsValid);
    }
}