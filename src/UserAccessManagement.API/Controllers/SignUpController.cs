using Microsoft.AspNetCore.Mvc;
using UserAccessManagement.API.ActionResults;
using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;

namespace UserAccessManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SignUpController : ControllerBase
{
    private readonly ICommandHandler<SignUpCommand, SignUpCommandResult> _signUpCommandHandler;

    public SignUpController(ICommandHandler<SignUpCommand, SignUpCommandResult> signUpCommandHandler)
    {
        _signUpCommandHandler = signUpCommandHandler;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SignUpCommandResult), 200)]
    [ProducesResponseType(typeof(SignUpCommandResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 500)]
    public async Task<IActionResult> SignUpAsync([FromBody] SignUpCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _signUpCommandHandler.HandleAsync(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}