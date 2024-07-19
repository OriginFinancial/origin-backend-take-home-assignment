using Application.Messages.Commands;
using Application.Messages.Validators.Commands;
using FluentValidation.TestHelper;

namespace Application.Tests.Messages.Validators.Commands;

[Trait("Category", "Unit")]
public class TerminateUnlistedUsersCommandValidatorTests
{
    private readonly TerminateUnlistedUsersCommandValidator _validator;

    public TerminateUnlistedUsersCommandValidatorTests()
    {
        _validator = new TerminateUnlistedUsersCommandValidator();
    }
    
    [Fact]
    public void EmployerId_IsRequired()
    {
        var command = new TerminateUnlistedUsersCommand(new HashSet<string>(), null);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.EmployerId);
    }

    [Fact]
    public void Command_WithValidEmployerId_ShouldPassValidation()
    {
        var command = new TerminateUnlistedUsersCommand(new HashSet<string>(), "validEmployerId");
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.EmployerId);
    }
}