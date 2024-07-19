using Application.Messages.Commands;

namespace Application.Tests.Messages.Validators.Commands;

[Trait("Category", "Unit")]
public class ProcessEligibilityFileCommandValidatorTests
{
    private readonly ProcessEligibilityFileCommandValidator _validator;

    public ProcessEligibilityFileCommandValidatorTests()
    {
        _validator = new ProcessEligibilityFileCommandValidator();
    }
    
    [Fact]
    public void CsvFileUrl_MustBeValidUrl()
    {
        // Arrange
        var command = new ProcessEligibilityFileCommand("invalid-url", "ValidEmployerName");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CsvFileUrl" && e.ErrorMessage.Contains("valid URL"));
    }
    
    [Fact]
    public void CsvFileUrl_IsRequired()
    {
        // Arrange
        var command = new ProcessEligibilityFileCommand(null, "ValidEmployerName");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "CsvFileUrl");
    }
    
    [Fact]
    public void EmployerName_IsRequired()
    {
        // Arrange
        var command = new ProcessEligibilityFileCommand("http://validurl.com", null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "EmployerName");
    }
    
    [Fact]
    public void Command_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var command = new ProcessEligibilityFileCommand("http://validurl.com", "ValidEmployerName");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
    }
}