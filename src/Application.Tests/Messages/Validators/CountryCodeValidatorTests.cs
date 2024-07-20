using Application.Messages.Validators;

namespace Application.Tests.Messages.Validators;

[Trait("Category", "Unit")]
public class CountryCodeValidatorTests
{
    [Fact]
    public void TestValidCountryCode_ShouldReturnTrue()
    {
        // Arrange
        var validCode = "US"; // Assuming "US" is in the list

        // Act
        var result = CountryCodeValidator.IsValidIso3166CountryCode(validCode);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TestInvalidCountryCode_ShouldReturnFalse()
    {
        // Arrange
        var invalidCode = "XX"; // Assuming "XX" is not a valid code

        // Act
        var result = CountryCodeValidator.IsValidIso3166CountryCode(invalidCode);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void TestEmptyOrNullCountryCode_ShouldReturnFalse(string code)
    {
        // Act
        var result = CountryCodeValidator.IsValidIso3166CountryCode(code);

        // Assert
        Assert.False(result);
    }
}