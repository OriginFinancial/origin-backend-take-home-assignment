using Application.Messages.Queries;
using Application.Messages.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Messages.Validators.Queries;

[Trait("Category", "Unit")]
public class GetUserByEmailQueryValidatorTests
{
    private readonly GetUserByEmailQueryValidator _validator;

    public GetUserByEmailQueryValidatorTests()
    {
        _validator = new GetUserByEmailQueryValidator();
    }
    
    [Fact]
    public void Email_IsRequired()
    {
        var query = new GetUserByEmailQuery(null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Email);
    }

    [Fact]
    public void Email_MustBeValidEmail()
    {
        var query = new GetUserByEmailQuery("invalid-email");
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Email);
    }

    [Fact]
    public void Query_WithValidEmail_ShouldPassValidation()
    {
        var query = new GetUserByEmailQuery("email@example.com");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Email);
    }
}