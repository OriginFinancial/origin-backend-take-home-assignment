using Application.Messages.Queries;
using Application.Messages.Validators.Queries;
using FluentValidation.TestHelper;

namespace Application.Tests.Messages.Validators.Queries;

[Trait("Category", "Unit")]
public class GetEmployerByIdQueryValidatorTests
{
    private readonly GetEmployerByIdQueryValidator _validator;

    public GetEmployerByIdQueryValidatorTests()
    {
        _validator = new GetEmployerByIdQueryValidator();
    }

    [Fact]
    public void Id_IsRequired()
    {
        var query = new GetEmployerByIdQuery(null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void Id_MustBeAlphanumericWithDashes()
    {
        var query = new GetEmployerByIdQuery("invalid id");
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id);
    }

    [Fact]
    public void Query_WithValidId_ShouldPassValidation()
    {
        var query = new GetEmployerByIdQuery("valid-id-123");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}