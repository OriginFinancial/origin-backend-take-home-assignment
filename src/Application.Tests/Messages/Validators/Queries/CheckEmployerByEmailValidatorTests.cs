using Application.Messages.Queries;
using Application.Messages.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Messages.Validators.Queries;

[Trait("Category", "Unit")]
public class CheckEmployerByEmailQueryValidatorTests
{

    private readonly CheckEmployerByEmailQueryValidator _validator;

    public CheckEmployerByEmailQueryValidatorTests()
    {
        _validator = new CheckEmployerByEmailQueryValidator();
    }
    
    [Fact]
    public void Email_IsRequired()
    {
        var query = new CheckEmployerByEmailQuery(null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Email);
    }

    [Fact]
    public void Email_MustBeValidEmail()
    {
        var query = new CheckEmployerByEmailQuery("invalid-email");
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Email);
    }

    [Fact]
    public void Query_WithValidEmail_ShouldPassValidation()
    {
        var query = new CheckEmployerByEmailQuery("email@example.com");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Email);
    }
}