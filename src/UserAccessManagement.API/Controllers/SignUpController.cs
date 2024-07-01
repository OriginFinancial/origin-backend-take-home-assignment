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
    private readonly ICommandHandler<ListAllUsersCommand, ListAllUsersCommandResult> _listAllUsersCommandHandler;

    public SignUpController(ICommandHandler<SignUpCommand, SignUpCommandResult> signUpCommandHandler, ICommandHandler<ListAllUsersCommand, ListAllUsersCommandResult> listAllUsersCommandHandler)
    {
        _signUpCommandHandler = signUpCommandHandler;
        _listAllUsersCommandHandler = listAllUsersCommandHandler;
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

    [HttpGet("{employerName}")]
    [ProducesResponseType(typeof(ListAllUsersCommandResult), 200)]
    [ProducesResponseType(typeof(ListAllUsersCommandResult), 400)]
    [ProducesResponseType(typeof(ErrorResult), 500)]
    public async Task<IActionResult> ListAllUsersAsync([FromRoute] string employerName, CancellationToken cancellationToken = default)
    {
        var result = await _listAllUsersCommandHandler.HandleAsync(new ListAllUsersCommand(employerName), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}