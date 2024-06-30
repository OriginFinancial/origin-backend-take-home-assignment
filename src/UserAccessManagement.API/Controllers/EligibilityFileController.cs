using Microsoft.AspNetCore.Mvc;
using UserAccessManagement.Application.Base;
using UserAccessManagement.Application.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UserAccessManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EligibilityFileController : ControllerBase
{
    private readonly ICommandHandler<AddEligibilityFileCommand, CommandResult> _addEligibilityFileCommandHandler;
    private readonly ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult> _getLastElibilityFileByEmployerCommandHandler;

    public EligibilityFileController(ICommandHandler<AddEligibilityFileCommand, CommandResult> addEligibilityFileCommandHandler, ICommandHandler<GetLastElibilityFileByEmployerCommand, GetLastElibilityFileByEmployerCommandResult> getLastElibilityFileByEmployerCommandHandler)
    {
        _addEligibilityFileCommandHandler = addEligibilityFileCommandHandler;
        _getLastElibilityFileByEmployerCommandHandler = getLastElibilityFileByEmployerCommandHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetLastElibilityFileByEmployerAsync([FromQuery] string employerName, CancellationToken cancellationToken = default)
    {
        var result = await _getLastElibilityFileByEmployerCommandHandler.HandleAsync(new GetLastElibilityFileByEmployerCommand(employerName), cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddElibilityFileAsync([FromBody] AddEligibilityFileCommand command, CancellationToken cancellationToken = default)
    {
        var result = await _addEligibilityFileCommandHandler.HandleAsync(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}